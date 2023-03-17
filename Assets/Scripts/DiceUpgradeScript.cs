using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUpgradeScript : MonoBehaviour
{
    GameManager GameManager;
    DiceScript DiceScript;
    DiceHoverTextManager DiceHoverTextManager;

    private void Start() 
    {
        DiceScript = GameObject.FindObjectOfType<DiceScript>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
    }

    void OnMouseDown()
    {
        Debug.Log(GameManager.State.ToString());
        // Check if the game manager is in the "Rewards" state
        if (GameManager.State.ToString()  == "UpgradeDice")
        {
            // Upgrade the rarity of the dice
            if (gameObject.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                gameObject.GetComponent<DiceScript>().DiceRarity = "Rare";
                GameManager.StateChangetoReceiveNewDice();
            }
            else if (gameObject.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                gameObject.GetComponent<DiceScript>().DiceRarity = "Epic";
                GameManager.StateChangetoReceiveNewDice();
            }
            else if (gameObject.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                gameObject.GetComponent<DiceScript>().DiceRarity = "Mythic";
                GameManager.StateChangetoReceiveNewDice();
            }
            else if (gameObject.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                Debug.Log("DiceUpgradeScript - Dice is already Mythic grade. Cannot upgrade further. State changing to Receive new dice state.");
            }


            //change game state
            // GameManager.StateChangetoReceiveNewDice();
        }
    }
}
