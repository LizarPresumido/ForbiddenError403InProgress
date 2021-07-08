using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class CombatManager : MonoBehaviour
{
    /// <summary>
    /// Inicializacion de otros scripts y array de los personajes
    /// </summary>
    private BattleBehaviour currentChar;
    private MovementOOC moveController;
    public GameObject[] viruses;
    public GameObject[] enemies;
    public List<GameObject> enemiesSpawned;
    public Color moveTileColor;
    public Color activeAttackTileColor;
    public Color passiveAttackTileColor;
    public Color activeBuffTileColor;

    //Variables de combate
    #region CombatVariables
    public GameObject virusLeader;
    public GameObject[] characters;
    public Tilemap tilemap;
    public GameObject combatUi;
    public Camera camera;

    /// <summary>
    /// Ayuda visual para los comandos basicos
    /// </summary>
    private BoundsInt move;
    private BoundsInt attackArea;
    private BoundsInt skillRange;

    [HideInInspector]
    public bool inCombat, inPause;

    private Vector3 vector3Aux = new Vector3(99, 99, 99);
    private GameObject character2;

    private int turnCounter = 0;
    private Color defaultTileColor;

    private bool abilitySelected;
    private int selectedAbility;


    #endregion

    //Variables de UI
    #region UIVariables
    //Array de stats para la UI
    public StatsUI[] stats;
    public AbilityController abilitiesUI;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        abilitySelected = false;
        moveController = virusLeader.GetComponent<MovementOOC>();
        defaultTileColor = tilemap.GetColor(moveController.getPosition());
        inCombat = false;
        inPause = false;
        setStatsUI();
    }

    void Update()
    {

        if (!inPause && inCombat && currentChar.getState() != StateManager.ooc && !currentChar.isEnemy && !currentChar.inAction)
        {
            if (currentChar.getState() == StateManager.move)
            {
                bool move = false;
                Vector3Int movePos = tilemap.WorldToCell(currentChar.transform.position);
                Animator animator = currentChar.gameObject.GetComponent<Animator>();
                if (Input.GetKeyDown(KeyCode.W) && currentChar.currentRam > 0)
                {
                    movePos += new Vector3Int(0, 1, 0);
                    animator.SetInteger("direccion", 3);
                    move = true;
                }
                else if (Input.GetKeyDown(KeyCode.D) && currentChar.currentRam > 0)
                {
                    movePos += new Vector3Int(1, 0, 0);
                    animator.SetInteger("direccion", 1);
                    move = true;
                }
                else if (Input.GetKeyDown(KeyCode.S) && currentChar.currentRam > 0)
                {
                    movePos += new Vector3Int(0, -1, 0);
                    animator.SetInteger("direccion", 0);
                    move = true;
                }
                else if (Input.GetKeyDown(KeyCode.A) && currentChar.currentRam > 0)
                {
                    movePos += new Vector3Int(-1, 0, 0);
                    animator.SetInteger("direccion", 2);
                    move = true;
                }
                if (move && !hasEnemy(movePos) && tilemap.HasTile(movePos))
                {
                    if (availableTile(movePos, characters))
                    {
                        currentChar.gameObject.GetComponent<MovementOOC>().setPos(movePos);
                        currentChar.SetMove(tilemap.GetCellCenterWorld(movePos));
                    }
                    else
                    {
                        character2 = getCharacterAtPosition(tilemap.GetCellCenterWorld(movePos));
                        character2.GetComponent<BattleBehaviour>().SetMove(currentChar.transform.position);
                        currentChar.SetMove(tilemap.GetCellCenterWorld(movePos));
                        character2.gameObject.GetComponent<MovementOOC>().setPos(currentChar.gameObject.GetComponent<MovementOOC>().getPosition());
                        currentChar.gameObject.GetComponent<MovementOOC>().setPos(movePos);
                    }
                    --currentChar.currentRam;
                }


            }
            else if (currentChar.getState() == StateManager.attack)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && currentChar.currentRam > 0)
                {
                    Vector3Int clickPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    if (tilemap.GetColor(clickPos) == activeAttackTileColor)
                    {
                        currentChar.atacar(clickPos, tilemap.WorldToCell(currentChar.transform.position));
                        AttackHit(clickPos);
                        --currentChar.currentRam;
                    }
                }
            }
            clearVisualHelp();
            startCombat();
            updateStats();
        }
    }

    //Aqui se maneja el combate
    #region Combat
    /// <summary>
    /// Metodo para establecer las propiedades de combate
    /// </summary>
    public void setCombat()
    {
        inCombat = true;
        GameManager.Instance.CombatMove();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        int total = viruses.Length + enemiesSpawned.Count;
        characters = new GameObject[total];
        int i;
        for (i = 0; i < viruses.Length; ++i)
            characters[i] = viruses[i];
        for (int j = 0; i < total; ++i, ++j)
            characters[i] = enemiesSpawned[j];

        orderByInciative();
        TurnManager();
    }
    /// <summary>
    /// Terminar el combate
    /// </summary>
    public void endCombat()
    {
        currentChar.setState(2);
        combatUi.SetActive(false);
        inCombat = false;
        clearVisualHelp();
        if (virusLeader.GetComponent<BattleBehaviour>().currentMegas == 0)
        {
            virusLeader = GameManager.Instance.GetLeader();
            moveController = virusLeader.GetComponent<MovementOOC>();
            virusLeader.GetComponent<MovementOOC>().setPos(tilemap.WorldToCell(virusLeader.transform.position));
        }
        else
            virusLeader.GetComponent<MovementOOC>().setPos(tilemap.WorldToCell(virusLeader.transform.position));
        GameManager.Instance.CombatMove();
        camera.GetComponent<Follow>().setFollow(virusLeader);
    }

    /// <summary>
    /// Estados de combate 
    /// </summary>
    private void startCombat()
    {
        if (hasEnemies())
        {
            if (currentChar.isEnemy != true)
            {
                combatUi.SetActive(true);
                switch (currentChar.getState())
                {
                    case StateManager.move:
                        movement();
                        break;
                    case StateManager.attack:
                        attack();
                        break;
                    case StateManager.ability:
                        if(abilitySelected)
                            Ability();
                        break;
                    case StateManager.ooc:
                        abilitiesUI.ResetChar();
                        ++turnCounter;
                        currentChar.resetRam();
                        TurnManager();
                        break;
                }
            }
            else
            {
                currentChar.ia.SetTarget(characters, tilemap);
                StartCoroutine(IATurn());
            }
        }
        else
            endCombat();

    }

    IEnumerator IATurn()
    {
        do {
            if (!currentChar.inAction)
            {
                currentChar.ia.setAction();
                if (currentChar.ia.state == BaseIA.iaState.attack)
                    updateStats();
            }
            yield return 0;
        } while (currentChar.ia.state != BaseIA.iaState.endTurn);
        ++turnCounter;
        currentChar.resetRam();
        TurnManager();
        StopCoroutine(IATurn());
        
    }

    /// <summary>
    /// Ordenar los personajes por iniciativa (Mayor a menor)
    /// </summary>
    public void orderByInciative()
    {
        GameObject virusAux;
        for (int i = 0; i < characters.Length; ++i)
        {
            for (int j = 0; j < characters.Length; ++j)
            {
                if (characters[i].GetComponent<BattleBehaviour>().getIniciative() > characters[j].GetComponent<BattleBehaviour>().getIniciative())
                {
                    virusAux = characters[i];
                    characters[i] = characters[j];
                    characters[j] = virusAux;
                }
            }
        }
    }

    /// <summary>
    /// Funcion para manejar los turnos
    /// </summary>
    public void TurnManager()
    {
        if (turnCounter != characters.Length)
        {
            while (characters[turnCounter] == null && turnCounter != characters.Length)
            {
                ++turnCounter;
            }
            if (turnCounter != characters.Length)
                currentChar = characters[turnCounter].GetComponent<BattleBehaviour>();
            else
                turnCounter = 0;
        }
        else
        {
            turnCounter = 0;
            while (characters[turnCounter] == null && turnCounter != characters.Length)
            {
                ++turnCounter;
            }
            currentChar = characters[turnCounter].GetComponent<BattleBehaviour>();
        }
        if (!currentChar.isEnemy)
            combatUi.SetActive(true);
        else
        {
            combatUi.SetActive(false);
            currentChar.GetComponent<BattleBehaviour>().resetRam();
            camera.GetComponent<Follow>().setFollow(characters[turnCounter]);
            startCombat();
        }
        if (characters[turnCounter].GetComponent<BattleBehaviour>().currentMegas != 0) {
            //camera follow for characters
            camera.GetComponent<Follow>().setFollow(characters[turnCounter]);
        } else
            setState(2);
    }

    /// <summary>
    /// Ayuda visual para movimiento del jugador
    /// </summary>
    public void movement()
    {
        abilitySelected = false;
        Vector3Int currentPosition = tilemap.WorldToCell(currentChar.transform.position);
        move = currentChar.getMoveArea(currentPosition);
        BoundsInt.PositionEnumerator tiles = move.allPositionsWithin;
        tiles = tiles.GetEnumerator();
        while (tiles.MoveNext())
        {
            if (!hasEnemy(tiles.Current) && (tiles.Current.x - currentPosition.x == 0 || tiles.Current.y - currentPosition.y == 0) && tilemap.GetCellCenterWorld(tiles.Current) != currentChar.transform.position)
            {
                tilemap.RemoveTileFlags(tiles.Current, TileFlags.LockColor);
                tilemap.SetColor(tiles.Current, moveTileColor);
            }
        }
    }

    /// <summary>
    /// Ayuda visual para el ataque
    /// </summary>
    public void attack()
    {
        abilitySelected = false;
        Vector3Int currentPosition = tilemap.WorldToCell(currentChar.transform.position);
        attackArea = currentChar.getMoveArea(currentPosition);
        BoundsInt.PositionEnumerator tiles = attackArea.allPositionsWithin;
        tiles = tiles.GetEnumerator();
        while (tiles.MoveNext())
        {
            if (!currentPosition.Equals(tiles.Current) && ((tiles.Current.x - currentPosition.x == 0 || tiles.Current.y - currentPosition.y == 0)))
            {
                tilemap.RemoveTileFlags(tiles.Current, TileFlags.LockColor);
                if (hasEnemy(tiles.Current))
                    tilemap.SetColor(tiles.Current, activeAttackTileColor);
                else
                    tilemap.SetColor(tiles.Current, passiveAttackTileColor);
            }
        }
    }

    private void Ability()
    {
        Ability ability = currentChar.GetAbilities()[selectedAbility];
        ability.SetRangoHabilidad(tilemap.WorldToCell(currentChar.transform.position));
        Vector3Int currentPosition = tilemap.WorldToCell(currentChar.transform.position);
        skillRange = ability.GetRangoHabilidad();
        BoundsInt.PositionEnumerator tiles = skillRange.allPositionsWithin;
        tiles = tiles.GetEnumerator();
        if (ability.GetTipo() == 1)
        {
            while (tiles.MoveNext())
            {
                tilemap.RemoveTileFlags(tiles.Current, TileFlags.LockColor);
                tilemap.SetColor(tiles.Current, activeBuffTileColor);
            }
        }
        else
        {
            while (tiles.MoveNext())
            {
                if (!currentPosition.Equals(tiles.Current))
                {
                    tilemap.RemoveTileFlags(tiles.Current, TileFlags.LockColor);
                    if (hasEnemy(tiles.Current))
                        tilemap.SetColor(tiles.Current, activeAttackTileColor);
                    else
                        tilemap.SetColor(tiles.Current, passiveAttackTileColor);
                }
            }
        }
    }

    /// <summary>
    /// Metodo para eliminar toda ayuda visual
    /// </summary>
    public void clearVisualHelp()
    {
        BoundsInt.PositionEnumerator tiles = new BoundsInt.PositionEnumerator();
        switch (currentChar.getState())
        {
            case StateManager.move:
                tiles = move.allPositionsWithin;
                break;
            case StateManager.attack:
                tiles = attackArea.allPositionsWithin;
                break;
            case StateManager.ability:
                tiles = skillRange.allPositionsWithin;
                break;
        }
        tiles = tiles.GetEnumerator();

        while (tiles.MoveNext())
        {
            tilemap.SetColor(tiles.Current, defaultTileColor);
        }
    }

    /// <summary>
    /// Metodo para comprobar si la casilla esta disponible
    /// </summary>
    /// <param name="p1"></param>
    /// <returns></returns>
    private bool availableTile(Vector3Int p1, GameObject[] array)
    {
        bool available = true;
        for (int i = 0; i < array.Length && available; ++i)
        {
            if (array[i] != null && p1.Equals(tilemap.WorldToCell(array[i].transform.position)))
                available = false;
        }
        return available;
    }

    private bool hasEnemy(Vector3Int p1)
    {
        bool available = false;
        for (int i = 0; i < characters.Length && !available; ++i)
        {
            if (characters[i] != null && characters[i].GetComponent<BattleBehaviour>().isEnemy && p1.Equals(tilemap.WorldToCell(characters[i].transform.position)))
                available = true;
        }
        return available;
    }

    /// <summary>
    /// Setter de estado de combate
    /// </summary>
    /// <param name="state"></param>
    public void setState(int state)
    {
        if (!inPause && !currentChar.inAction)
        {
            currentChar.setState(state);
            startCombat();
        }
    }

    #endregion
    //Aqui se maneja la UI
    #region UI

    /// <summary>
    /// Actualizar stats en UI
    /// </summary>
    private void updateStats()
    {
        for (int i = 0; i < stats.Length; ++i)
        {
            stats[i].update();
        }
    }

    /// <summary>
    /// Poner las stats en la UI
    /// </summary>
    private void setStatsUI()
    {
        for (int i = 0; i < stats.Length; ++i)
        {
            stats[i].character = viruses[i].GetComponent<BattleBehaviour>();
            //este codigo esta por un bug a la hora de actualizar la UI
            stats[i].character.currentMegas = stats[i].character.megas;
            stats[i].character.currentRam = stats[i].character.ram;
        }
        updateStats();
    }

    #endregion

    public void Spawn(Room r)
    {
        if (!r.combat)
        {
            List<Vector3Int> spawnPoints = new List<Vector3Int>();
            for (int i = 0; i < viruses.Length; ++i)
            {
                spawnPoints.Add(tilemap.WorldToCell(viruses[i].transform.position));
                //Debug.Log("Virus "+ i + ": "+tilemap.WorldToCell(viruses[i].transform.position));
            }
            int enemyCant = Random.Range(3, 6);
            GameObject clon;
            Vector3Int position;
            virusLeader.GetComponent<MovementOOC>().forceMove();
            for (int i = 0; i < enemyCant; ++i)
            {
                clon = enemies[Random.Range(0, enemies.Length)];
                do
                {
                    position = r.getRoomTile();
                    //position -= new Vector3Int(0, 0, 1);
                } while (spawnPoints.Contains(position));
                spawnPoints.Add(position);
                clon.transform.position = tilemap.GetCellCenterWorld(position);
                enemiesSpawned.Add(Instantiate(clon));
            }
            setCombat();
        }
    }

    private void AttackHit(Vector3Int pos)
    {
        BattleBehaviour bb;
        for (int i = 0; i < characters.Length; ++i)
        {
            if (characters[i] != null)
            {
                bb = characters[i].GetComponent<BattleBehaviour>();
                if (bb.isEnemy && pos.Equals(tilemap.WorldToCell(characters[i].transform.position)))
                {
                    if (bb.GetHit(currentChar.attack))
                    {
                        Destroy(characters[i]);
                        characters[i] = null;
                    }
                }
            }
        }
    }

    private bool hasEnemies()
    {
        bool has = false;
        int i;
        for (i = 0; i < characters.Length && !has; ++i)
        {
            if (characters[i] != null)
                if (characters[i].GetComponent<BattleBehaviour>().isEnemy)
                    has = true;
        }
        return has;
    }

    private GameObject getCharacterAtPosition(Vector3 pos)
    {
        GameObject character = null;
        for (int i = 0; i < characters.Length && character == null; ++i)
        {
            //Debug.Log(Vector3.Distance(characters[i].transform.position, pos));
            if (Vector3.Distance(characters[i].transform.position, pos) < 0.5f) character = characters[i];
        }
        return character;
    }

    public GameObject GetCurrentChar()
    {
        return currentChar.gameObject;
    }

    public void ActivarHabilidad(int code)
    {
        clearVisualHelp();
        abilitySelected = true;
        selectedAbility = code;
        Ability ability = currentChar.GetAbilities()[code];
        ability.SetRangoHabilidad(tilemap.WorldToCell(currentChar.transform.position));
        Vector3Int currentPosition = tilemap.WorldToCell(currentChar.transform.position);
        skillRange = ability.GetRangoHabilidad();
        BoundsInt.PositionEnumerator tiles = skillRange.allPositionsWithin;
        tiles = tiles.GetEnumerator();
        if (ability.GetTipo() == 1)
        {
            while (tiles.MoveNext())
            {
                tilemap.RemoveTileFlags(tiles.Current, TileFlags.LockColor);
                tilemap.SetColor(tiles.Current, activeBuffTileColor);
            }
        }
        else
        {
            while (tiles.MoveNext())
            {
                if (!currentPosition.Equals(tiles.Current))
                {
                    tilemap.RemoveTileFlags(tiles.Current, TileFlags.LockColor);
                    if (hasEnemy(tiles.Current))
                        tilemap.SetColor(tiles.Current, activeAttackTileColor);
                    else
                        tilemap.SetColor(tiles.Current, passiveAttackTileColor);
                }
            }
        }
    }
}
