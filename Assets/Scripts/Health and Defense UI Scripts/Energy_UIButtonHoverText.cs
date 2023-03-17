using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_UIButtonHoverText : MonoBehaviour
{
    DiceBagScript DiceBagScript;
    EnemyAI EnemyAI;
    Player Player;
    MouseController MouseController;
    RelicScript RelicScript;
    DiceHoverTextManager DiceHoverTextManager;

    public bool BlueEnergyCrystal = false; 
    public bool RegularEnergyCrystal = false;

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
        if (RegularEnergyCrystal == true)
        {
            DiceHoverTextManager.HoverText4.text = "Energy Crystal. "; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);
        }
        else if (BlueEnergyCrystal == true && Player.Energy == 4)
        {
            DiceHoverTextManager.HoverText4.text = "Energy Crystal. "; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f); 
        }

 
    }

    public void OnMouseExit()
    {
        DiceHoverTextManager.HoverTextPanel4.SetActive(false);     
    }

}
