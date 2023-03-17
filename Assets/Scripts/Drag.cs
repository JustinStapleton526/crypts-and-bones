using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;




public class Drag : MonoBehaviour
{
    GameManager GameManager;
    Player Player;
    SoundManager SoundManager;

    public GameObject LastCollider;
    public GameObject CurrentCollider;
    public GameObject HandPosBinding;
    

    //IMPORTANT NOTE. THE DICE MUST BE AT A LOWER LEVEL THAN THE HAND AND GRID POSITIONS OR ELSE THE ONMOUSEDOWN() WILL INTERMITTENTLY FAIL

    [Header("Audio reference")]
    [SerializeField] private AudioClip _pickClip;
    [SerializeField] private AudioClip _dropClip;

    void Update()
    {
        if(transform.position == gameObject.GetComponent<DiceScript>().StartingHandLocation)  
      { 
        gameObject.GetComponent<DiceScript>().returnToBag = false;
      }
    }

    void Start()
    {
        GameManager = GameObject.FindObjectOfType<GameManager>();
        Player = GameObject.FindObjectOfType<Player>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }


    void OnMouseDown() 
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit.HasValue)
        {   
            //the actual item hit by the raycast
            GameObject collider = focusedTileHit.Value.collider.gameObject;
            
            //variables that cover various pieces of data about the gameobject we hit with the raycast
            var collidertag = collider.tag;
            var collidername = collider.name;
            LastCollider = collider;

            //Play audio clip of dice rolling
            SoundManager.Instance.PlayLoudSound(_pickClip);
            // Debug.Log("OnMouseDown(1) hit: " + collider + " " + collidername + " " + collidertag);

            //Change Slot in use to false on mouse down
            if (collidertag == "Grid")
            {
                if (collider.GetComponent<GridPositionScript>().SlotInUse == true)
                {
                    collider.GetComponent<GridPositionScript>().SlotInUse = false;
                    collider.GetComponent<GridPositionScript>().AttachedDice = null;
                    collider.GetComponent<GridPositionScript>().AttachedDiceType = null;
                    collider.GetComponent<GridPositionScript>().AttachedDiceValue = 0;
                    collider.GetComponent<GridPositionScript>().LandMarkGrid = false;
                    Player.GetComponent<Player>().Energy ++;
                    Player.GetComponent<Player>().ThornsBuff = false; 


                    if(gameObject.GetComponent<DiceScript>().DiceType == "Enhance")
                    {
                        collider.GetComponent<GridPositionScript>().ResetAllGridPositions();
                        collider.GetComponent<GridPositionScript>().EnhanceChild = false;
                        collider.GetComponent<GridPositionScript>().EnhanceRoot = false;
                        collider.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                    }
                    if(gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Ephemeral")
                    {
                        Player.GetComponent<Player>().Energy --;  
                    }
                    if(gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Weighted")
                    {
                        Player.GetComponent<Player>().Energy ++;  
                        gameObject.GetComponent<DiceScript>().DiceRolledValue = gameObject.GetComponent<DiceScript>().DiceRolledValue / 2;
                        if (collider.GetComponent<GridPositionScript>().previouslyBlocked == true)
                        {
                            collider.GetComponent<GridPositionScript>().isBlocked = true;
                        }
                    }
                    if(gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                    {
                        gameObject.GetComponent<DiceScript>().GlassUsed = false;
                    }
                    
                }

            }
            else if (collidertag == "Hand")
            {
                //if we click and raycast hits a hand position, we will flip the bool to true and bind the collider to handposbinding var
                if (collider.GetComponent<HandPositionScript>().OpenDiceSlot == false)
                {
                    collider.GetComponent<HandPositionScript>().OpenDiceSlot = true; 
                    HandPosBinding = collider.GetComponent<HandPositionScript>().HandPositionId;
                    // Debug.Log("HandPosBinding: " + HandPosBinding);
                }
            }  
        }
    }

    void OnMouseUp() 
    {
        var focusedTileHit = GetFocusedOnTile();

        if (focusedTileHit.HasValue)
        {   
            //the actual item hit by the raycast
            GameObject collider = focusedTileHit.Value.collider.gameObject;
            
            //variables that cover various pieces of data about the gameobject we hit with the raycast
            var collidertag = collider.tag;
            var collidername = collider.name;

            //Play audio clip of dice rolling
            SoundManager.Instance.PlayLoudSound(_dropClip);


            if (collidertag == "Grid" && collider.GetComponent<GridPositionScript>().SlotInUse == false)
            {
                //this is the logic for a weighted die to be placed on a blocked grid 
                if (collider.GetComponent<GridPositionScript>().isBlocked == true && gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Weighted")
                {
                    transform.position = collider.GetComponent<GridPositionScript>().GridLocation;
                    CurrentCollider = collider;
                    gameObject.GetComponent<DiceScript>().LastGridLocation = transform.position;

                    if (collider.GetComponent<GridPositionScript>().SlotInUse == false)
                    {
                        var EnemyInfluenceAmount = collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount;
                        var ArtifactChildModifier = collider.GetComponent<GridPositionScript>().ArtifactChildModifier;
                        var DiceRolledValue = gameObject.GetComponent<DiceScript>().DiceRolledValue;

                        collider.GetComponent<GridPositionScript>().SlotInUse = true;
                        gameObject.GetComponent<DiceScript>().returnToBag = true;

                        //double the roll value 
                        gameObject.GetComponent<DiceScript>().DiceRolledValue = DiceRolledValue * 2;
                        
                        collider.GetComponent<GridPositionScript>().isBlocked = false;
                        collider.GetComponent<GridPositionScript>().previouslyBlocked = true;
                        collider.GetComponent<GridPositionScript>().EnhanceChild = false;
                        collider.GetComponent<GridPositionScript>().LandMarkGridPersist = false;
                        collider.GetComponent<GridPositionScript>().EnemyInfluence = false;



                        collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                        collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                        collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue - collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount + collider.GetComponent<GridPositionScript>().ArtifactChildModifier;

                        Player.GetComponent<Player>().Energy = Player.GetComponent<Player>().Energy -2;
                    }
                    else
                    {
                        collider.GetComponent<GridPositionScript>().isBlocked = true;   
                    }
                }


                //check if the grid is blocked by an enemy
                else if (collider.GetComponent<GridPositionScript>().isBlocked != true)
                {
                    //This snaps the dice to the collider transform.position (ex: dice -> grid / hand)
                    transform.position = collider.GetComponent<GridPositionScript>().GridLocation;
                    //Update last grid collider variable
                    CurrentCollider = collider;
                    //Update dice last transform position for comparisons
                    gameObject.GetComponent<DiceScript>().LastGridLocation = transform.position;
                    // Debug.Log("On Mouse Up hit: " + collider + " " + collidername + " " + collidertag);

                    //Change Slot in use to true on mouse up
                    if (collider.GetComponent<GridPositionScript>().SlotInUse == false)
                    {
                        var EnemyInfluenceAmount = collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount;
                        var ArtifactChildModifier = collider.GetComponent<GridPositionScript>().ArtifactChildModifier;
                        var DiceRolledValue = gameObject.GetComponent<DiceScript>().DiceRolledValue;

                        collider.GetComponent<GridPositionScript>().SlotInUse = true;
                        gameObject.GetComponent<DiceScript>().returnToBag = true;
                        //This code enables the boolean "enhanceroot" if the dice dropped on the grid is an enhance dice
                        if (gameObject.GetComponent<DiceScript>().DiceType == "Enhance")
                        {
                            if (gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Ephemeral")
                            {
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue - collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount + collider.GetComponent<GridPositionScript>().ArtifactChildModifier;

                                collider.GetComponent<GridPositionScript>().EnhanceRoot = true;
                                collider.GetComponent<GridPositionScript>().EnhanceRootValue = gameObject.GetComponent<DiceScript>().DiceRolledValue - collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount + collider.GetComponent<GridPositionScript>().ArtifactChildModifier;
                            }
                            else if (gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Weighted")
                            {
                                gameObject.GetComponent<DiceScript>().DiceRolledValue = DiceRolledValue * 2;

                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue - collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount + collider.GetComponent<GridPositionScript>().ArtifactChildModifier;

                                collider.GetComponent<GridPositionScript>().EnhanceRoot = true;
                                collider.GetComponent<GridPositionScript>().EnhanceRootValue = gameObject.GetComponent<DiceScript>().DiceRolledValue - collider.GetComponent<GridPositionScript>().EnemyInfluenceAmount + collider.GetComponent<GridPositionScript>().ArtifactChildModifier;
                                Player.GetComponent<Player>().Energy = Player.GetComponent<Player>().Energy -2;
                            }
                            else if(gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Landmark")
                            {
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = DiceRolledValue - EnemyInfluenceAmount + ArtifactChildModifier;
                                Debug.Log(DiceRolledValue + " " + EnemyInfluenceAmount + " " + ArtifactChildModifier);

                                collider.GetComponent<GridPositionScript>().LandMarkGrid = true;

                                collider.GetComponent<GridPositionScript>().EnhanceRoot = true;
                                collider.GetComponent<GridPositionScript>().EnhanceRootValue = DiceRolledValue - EnemyInfluenceAmount + ArtifactChildModifier;
                                Player.GetComponent<Player>().Energy --;
                            }
                            else if(gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = DiceRolledValue - EnemyInfluenceAmount + ArtifactChildModifier;
                                Debug.Log(DiceRolledValue + " " + EnemyInfluenceAmount + " " + ArtifactChildModifier);

                                gameObject.GetComponent<DiceScript>().GlassUsed = true;
;
                                Player.GetComponent<Player>().Energy --;
                            }
                    
                            else
                            {
                                gameObject.GetComponent<DiceScript>().DiceRolledValue = DiceRolledValue;

                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = DiceRolledValue - EnemyInfluenceAmount + ArtifactChildModifier;

                                collider.GetComponent<GridPositionScript>().EnhanceRoot = true;
                                collider.GetComponent<GridPositionScript>().EnhanceRootValue = DiceRolledValue - EnemyInfluenceAmount + ArtifactChildModifier;
                                //collider.GetComponent<GridPositionScript>().HighlightChildren();
                                Player.GetComponent<Player>().Energy --;
                            }
                        }
                        //Update the Attached Dice properties so we know which dice have been activated for the battle phase.
                        else if (gameObject.GetComponent<DiceScript>().DiceType == "Attack" | gameObject.GetComponent<DiceScript>().DiceType == "Defense" | gameObject.GetComponent<DiceScript>().DiceType == "Heal" | gameObject.GetComponent<DiceScript>().DiceType == "Cleave" | gameObject.GetComponent<DiceScript>().DiceType == "Bash" | gameObject.GetComponent<DiceScript>().DiceType == "Nemesis" | gameObject.GetComponent<DiceScript>().DiceType == "Slam" | gameObject.GetComponent<DiceScript>().DiceType == "Energy") 
                        {
                            if (gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Ephemeral")
                            {
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue;
                            }
                            else if (gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Weighted")
                            {
                                gameObject.GetComponent<DiceScript>().DiceRolledValue = gameObject.GetComponent<DiceScript>().DiceRolledValue * 2;

                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue;
                                Player.GetComponent<Player>().Energy = Player.GetComponent<Player>().Energy -2;
                            }
                            else if (gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Landmark")
                            {
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue;
                                collider.GetComponent<GridPositionScript>().LandMarkGrid = true;
                                Player.GetComponent<Player>().Energy --;
                            }
                            else if(gameObject.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = DiceRolledValue - EnemyInfluenceAmount + ArtifactChildModifier;
                                Debug.Log(DiceRolledValue + " " + EnemyInfluenceAmount + " " + ArtifactChildModifier);

                                gameObject.GetComponent<DiceScript>().GlassUsed = true;
                                Player.GetComponent<Player>().Energy --;
                            }
                            else
                            {
                                gameObject.GetComponent<DiceScript>().DiceRolledValue = DiceRolledValue;
                                collider.GetComponent<GridPositionScript>().AttachedDice = gameObject;
                                collider.GetComponent<GridPositionScript>().AttachedDiceType = gameObject.GetComponent<DiceScript>().DiceType;
                                collider.GetComponent<GridPositionScript>().AttachedDiceValue = gameObject.GetComponent<DiceScript>().DiceRolledValue;
                                Player.GetComponent<Player>().Energy --;
                            }
                            
                        }
                    }
                }
                else
                {
                    //if the dice transform position does not match any of the grid locations, return it to its starting position. 
                    var StartingHandLocation = gameObject.GetComponent<DiceScript>().StartingHandLocation;
                    transform.position = StartingHandLocation;
                    //reset open dice slot var using collider binding
                    HandPosBinding.GetComponent<HandPositionScript>().OpenDiceSlot = false;
                    CurrentCollider = null;
                    if(LastCollider.tag == "Grid")
                    {
                        LastCollider.GetComponent<GridPositionScript>().SlotInUse = false;
                        LastCollider.GetComponent<GridPositionScript>().EnhanceRoot = false;
                    }

                }

            }
            else
            {
                if (HandPosBinding == null)
                {
                    return; 
                }
                else
                {

                    //if the dice transform position does not match any of the grid locations, return it to its starting position. 
                    var StartingHandLocation = gameObject.GetComponent<DiceScript>().StartingHandLocation;
                    transform.position = StartingHandLocation;
                    //reset open dice slot var using collider binding
                    HandPosBinding.GetComponent<HandPositionScript>().OpenDiceSlot = false;
                    CurrentCollider = null;
                    if(LastCollider.tag == "Grid")
                    {
                        LastCollider.GetComponent<GridPositionScript>().SlotInUse = false;
                        LastCollider.GetComponent<GridPositionScript>().EnhanceRoot = false;
                    }

                }
                
            }
        }
    }

    void OnMouseDrag() 
    {
        transform.position = GetMousePos();
    }

    Vector3 GetMousePos() 
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -1;
        return mousePos;   
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
        // Debug.Log("Attempted raycasthit2d");
        
        if(hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
                //return hits.OrderByDescending(i => i.collider.transform.position.z).Last();                       
            } 
        return null;      
    }  

}
