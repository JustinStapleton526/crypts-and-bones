using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fin_UIButtonHoverText : MonoBehaviour
{
    DiceBagScript DiceBagScript;
    EnemyAI EnemyAI;
    Player Player;
    MouseController MouseController;
    RelicScript RelicScript;
    DiceHoverTextManager DiceHoverTextManager;

    // Start is called before the first frame update
    void Start()
    {
        MouseController = GameObject.FindObjectOfType<MouseController>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
        Player = GameObject.FindObjectOfType<Player>();
        RelicScript = GameObject.FindObjectOfType<RelicScript>();
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
    }


    public void OnMouseOver() 
    {
        
            DiceHoverTextManager.HoverText4.text = "Click to end your turn and execute your actions."; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);

            if(Player.Energy <= -1)
            {
                DiceHoverTextManager.HoverText4.text = "Not enough energy! Remove dice from the grid to continue.";  
            }
        
    }

    public void OnMouseExit()
    {
        DiceHoverTextManager.HoverTextPanel4.SetActive(false);     
    }
}