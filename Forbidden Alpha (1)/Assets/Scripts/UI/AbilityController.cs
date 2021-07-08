using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    public GameObject abilityMenu;
    public GameObject[] abilityButtons;
    private int[] levelCap;
    private GameObject character;

    private void Start()
    {
        character = null;
        levelCap = new int[3] { 1, 4, 7 };
    }
    public void ActivateMenu(CombatManager manager)
    {
        if (abilityMenu.activeSelf == false)
        {
            if (character == null)
            {
                character = manager.GetCurrentChar();
                updateButtons();
            }
            abilityMenu.SetActive(true);
        }
        else
        {
            abilityMenu.SetActive(false);
        }
    }

    private void updateButtons()
    {
        Ability[] abilities = character.GetComponent<BattleBehaviour>().GetAbilities();
        for(int i = 0; i < abilityButtons.Length; ++i)
        {
            if (abilities[i] != null)
            {
                if (i < levelCap.Length)
                {
                    if (levelCap[i] <= character.GetComponent<BattleBehaviour>().level)
                    {
                        abilityButtons[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        abilityButtons[i].GetComponent<Button>().interactable = false;
                    }
                }
                abilityButtons[i].SetActive(true);
                abilityButtons[i].GetComponentInChildren<Text>().text = abilities[i].GetNombre();

            }
            else
                abilityButtons[i].SetActive(false);
        }
    }

    public void ResetChar()
    {
        character = null;
    }
}
