using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceHoverTextManager : MonoBehaviour
{
    DiceScript DiceScript;
    MouseController MouseController;
    EnemyAI EnemyAI;


    public GameObject HoverTextPanel;
    public Vector2 HoverPanelLocation;

    [Header("Enemy Panel Details")]
    public GameObject HoverEnemyPanel;
    public Vector2 HoverEnemyPanelLocation;
    public Text EnemyPanelText;
    public GameObject[] EnemySides;
    public GameObject PoisonIcon;
    public Text PoisonText;
    [Header("Enemy Reaction Details - 1")]
    public GameObject ReactionTextPanel1;
    public Vector2 ReactionPanelLocation1;
    public Text ReactionText1;
    [Header("Enemy Reaction Details - 2")]
    public GameObject ReactionTextPanel2;
    public Vector2 ReactionPanelLocation2;
    public Text ReactionText2;

    [Header("Dice Hover Text Details")]
    public SpriteRenderer HoverDiceDetails;
    public Vector2 HoverDiceDetailsPanelLocation;
    public Sprite[] HoverDiceSides;

    public Text HoverText1;
    public Text HoverTextRarity;

    [Header("Dice Hover Text Panel 2 Details")]
    public GameObject HoverTextPanel2;
    public Vector2 HoverPanelLocation2;
    public Text HoverText2;

    [Header("Dice Hover Text RELIC Details")]
    public GameObject HoverTextPanel3;
    public Vector2 HoverPanelLocation3;
    public Text HoverText3;

    [Header("Dice Hover Text GRID Details")]
    public GameObject HoverTextPanel4;
    public Vector2 HoverPanelLocation4;
    public Text HoverText4;

    private void Start() 
    {
        DiceScript = GameObject.FindObjectOfType<DiceScript>();
        MouseController = GameObject.FindObjectOfType<MouseController>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
    }

    // private void Update() 
    // {

    //     var focusedTileHit = MouseController.GetFocusedOnTile();
    //     if (focusedTileHit.HasValue)
    //     {
    //         GameObject collider = focusedTileHit.Value.collider.gameObject;
    //         //var EnemySides = collider.GetComponent<EnemyAI>().EnemyAbilityDice;

    //         Debug.Log(collider);
    //     }        
    // }

}
