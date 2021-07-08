using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public MovementOOC moveScript;
    public CombatManager combatScript;
    //public int actionCode;
    public void Action(int actionCode)
    {
        if(!combatScript.inCombat)
            moveScript.canMove = true;
        Time.timeScale = 1;
        switch (actionCode) {
            case 1:
                menu.SetActive(false);
                break;
            case 2:
                SceneManager.UnloadSceneAsync("Juego");
                SceneManager.LoadScene("Menu");  
                break;
            case 3:
                Application.Quit(0);
                break;
        }
    }
}
