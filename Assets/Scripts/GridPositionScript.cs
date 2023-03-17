using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GridPositionScript : MonoBehaviour
{
    DiceBagScript DiceBagScript;
    EnemyAI EnemyAI;
    Player Player;
    MouseController MouseController;
    RelicScript RelicScript;
    DiceHoverTextManager DiceHoverTextManager;
    SoundManager SoundManager;

    Animator Animator;

    public Vector3 GridLocation;
    [Header("Grid Object Properties")]
    public bool SlotInUse;
    public bool isBlocked = false;
    public bool previouslyBlocked = false;
    public int BlockCountdown;

    [Header("Grid Object Sprites")]
    public Sprite BlockSprite;
    public Sprite LandmarkSprite;

    [Header("Grid Object Properties -- Enhance Dice")]
    public bool EnhanceRoot;
    public int EnhanceRootValue;
    public int GridToDiceModifier;
    public bool EnhanceChild;
    [Header("Grid Object Properties -- Enhance Affected Grid Positions")]
    public GameObject EnhanceChildPrefab;
    public GameObject[] Enhance1;
    public GameObject[] Enhance2;
    public GameObject[] Enhance3;
    public GameObject[] EnhanceClear;

    [Header("Grid Object Properties -- Other Dice")]
    public GameObject AttachedDice;
    public String AttachedDiceType;
    public int AttachedDiceValue;
    public bool LandMarkGrid = false;
    public bool LandMarkGridPersist = false;

    [Header("Grid Object Properties -- Enemy Influenced Grid Positions")]
    public bool EnemyInfluence = false;
    public int EnemyInfluenceAmount = 0;
    public GameObject EnemyInfluenceSprite;

    [Header("Grid Object Properties -- Artifact Child Properties")]
    public bool ArtifactCandidate = false;
    public bool ArtifactChild = false; 
    public int ArtifactChildModifier;
    public GameObject ArtifactParent;

    [Header("Sound Effects")]
    public bool _isSoundPlaying = false;
    [SerializeField] private AudioClip _HoverTextClip;


    void Start()
    {
        MouseController = GameObject.FindObjectOfType<MouseController>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
        Player = GameObject.FindObjectOfType<Player>();
        RelicScript = GameObject.FindObjectOfType<RelicScript>();
        //Retrieve the gameobjects current vector 2 location and return it in the inspector. 
        GridLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1);  
        Animator = GetComponent<Animator>();
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    public void OnMouseOver() 
    {


        if(EnemyInfluence == true)
        {
            DiceHoverTextManager.HoverText4.text = "<color=#C51B69>Nemesis Grid</color> \n-1 from dice face value."; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);

            if (!_isSoundPlaying)
            {
                SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }
        }
        else if(isBlocked == true)
        {
            DiceHoverTextManager.HoverText4.text = "<color=#C51B69>Blocked Grid</color> \nThis grid cannot be used."; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);

            if (!_isSoundPlaying)
            {
                SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }
        }
        else if(LandMarkGridPersist == true)
        {
            DiceHoverTextManager.HoverText4.text = "<color=#C51B69>Landmark Grid</color>\nThis grid cannot be blocked."; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);

            if (!_isSoundPlaying)
            {
                SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }
        }
        else if(EnhanceChild == true)
        {
            DiceHoverTextManager.HoverText4.text = "Dice played here will be modified by an <color=#C51B69>Enhance</color> die."; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);

            if (!_isSoundPlaying)
            {
                SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }
        }
        else if(ArtifactChild == true)
        {
            DiceHoverTextManager.HoverText4.text = "Dice played here will be modified by a nearby <color=#C51B69>Artifact</color>."; 
            DiceHoverTextManager.HoverPanelLocation4 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel4.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel4.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation4.x+2.8f, DiceHoverTextManager.HoverPanelLocation4.y+.5f);

            if (!_isSoundPlaying)
            {
                SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }
        }
    }

    public void OnMouseExit()
    {
        DiceHoverTextManager.HoverTextPanel4.SetActive(false);
        if (_isSoundPlaying)
            {
                _isSoundPlaying = false;
            }     
    }

    public void FindAllGridPositions()
    {
        //Associate all active dice objects in the game view to our diceBag list.
        var findGrid = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject grid in findGrid)
        {
            Debug.Log("gridPositionScript -- FindAllGridPositions:" + grid);
        }
    }
    public void ResetAllGridPositions()
    {
        //Associate all active dice objects in the game view to our diceBag list.
        var findGrid = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject grid in findGrid)
        {
            grid.GetComponent<GridPositionScript>().EnhanceChild = false;
        }
    }

    public void ResetNemesisSlots()
    {
        var findGrid = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject grid in findGrid)
        {
            grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
        }
    }

    public void HighlightChildren()
    {
    //     if (EnhanceRoot == true) 
    //    {
        // This will find all grid positions that match the range of the dice value 
         // Find all grid positions, for each grid positions, if the transform position is = to 1(guessing) it will be enhance child 
         // if enhance child... any dice value added will be +1 ... also update sprite to reflect that it is a child 
            if (EnhanceRootValue == 1)
            {
                foreach (GameObject child in Enhance1)
                {
                    child.GetComponent<GridPositionScript>().EnhanceChild = true; 
                    child.GetComponent<GridPositionScript>().GridToDiceModifier = EnhanceRootValue;
                }
            }
            else if (EnhanceRootValue == 2)
            {
                foreach (GameObject child in Enhance2)
                {
                    child.GetComponent<GridPositionScript>().EnhanceChild = true; 
                    child.GetComponent<GridPositionScript>().GridToDiceModifier = EnhanceRootValue;
                }
            }
            else if (EnhanceRootValue >= 3)
            {
                foreach (GameObject child in Enhance3)
                {
                    child.GetComponent<GridPositionScript>().EnhanceChild = true; 
                    child.GetComponent<GridPositionScript>().GridToDiceModifier = EnhanceRootValue;
                }  
            } 
    }
    

    void Update() 
    {
        // if (EnhanceChild == true)
        // {
        //     EnhanceChildPrefab.SetActive(true);
            
        // }
        if (EnhanceChild == false)
        {
            EnhanceChildPrefab.SetActive(false);
            GridToDiceModifier = 0;
        }
        // else
        // {
        //     Debug.Log("GridPositionScript - Something went wrong in the Update method.");
        // }

        if (EnhanceRoot == true) 
        {
            HighlightChildren();
        }

        if(isBlocked == true)
        {
            Animator.SetBool("isBlock", true);
            EnemyInfluence = false;
            EnemyInfluenceAmount = 0;
            EnemyInfluenceSprite.SetActive(false);
            //gameObject.GetComponent<SpriteRenderer>().sprite = BlockSprite;
            if (BlockCountdown == 0)
            {
                isBlocked =false;
            }
        }
        else if(LandMarkGridPersist == true && EnhanceChild == true)
        {
            Animator.SetBool("isLandmark", true);
            Animator.SetBool("isBlock", false);
            EnhanceChildPrefab.SetActive(true);
            isBlocked = false;
            EnemyInfluence = false;
            EnemyInfluenceAmount = 0;
            EnemyInfluenceSprite.SetActive(false);
            //gameObject.GetComponent<SpriteRenderer>().sprite = LandmarkSprite;
        }
        else if(ArtifactChild == true && EnhanceChild == true)
        {
            Animator.SetBool("isArtifactChild", true);
            Animator.SetBool("isBlock", false);
            Animator.SetBool("isLandmark", false);
            EnhanceChildPrefab.SetActive(true);
            EnemyInfluence = false;
            EnemyInfluenceAmount = 0;
            EnemyInfluenceSprite.SetActive(false);
            LandMarkGridPersist = false;
            isBlocked = false;
            //gameObject.GetComponent<SpriteRenderer>().sprite = LandmarkSprite;
        }
        else if(ArtifactChild == true)
        {
            Animator.SetBool("isArtifactChild", true);
            Animator.SetBool("isBlock", false);
            Animator.SetBool("isLandmark", false);
            EnemyInfluence = false;
            EnemyInfluenceAmount = 0;
            EnemyInfluenceSprite.SetActive(false);
            LandMarkGridPersist = false;
            isBlocked = false;
            
            //gameObject.GetComponent<SpriteRenderer>().sprite = LandmarkSprite;
        }
        else if(LandMarkGridPersist == true)
        {
            Animator.SetBool("isLandmark", true);
            //gameObject.GetComponent<SpriteRenderer>().sprite = LandmarkSprite;
        }
        else if(EnhanceChild == true)
        {
            EnhanceChildPrefab.SetActive(true);
            //gameObject.GetComponent<SpriteRenderer>().sprite = LandmarkSprite;
        }
        else
        {
            Animator.SetBool("isBlock", false);
            Animator.SetBool("isLandmark", false);
            Animator.SetBool("isArtifactChild", false);
            //gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }

        if(EnemyInfluence == true)
        {
            EnemyInfluenceAmount = 1;
            EnemyInfluenceSprite.SetActive(true);
        }
        if(EnemyInfluence == false)
        {
            EnemyInfluenceAmount = 0;
            EnemyInfluenceSprite.SetActive(false);
        }   
    }


    public void DetermineDiceEffect()
    {
        List<int> attackValues = new List<int>();
        List<int> defendValues = new List<int>();
        List<int> healValues = new List<int>();
        List<int> poisonValues = new List<int>();
        var findGrid = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject grid in findGrid)
        {
            // Debug.Log("determine Dice Effect " + grid.GetComponent<GridPositionScript>().AttachedDice);
            if (grid.GetComponent<GridPositionScript>().AttachedDice != null)
            {
                // Debug.Log("determine Dice Effect - not null " + grid.name);
                if (grid.GetComponent<GridPositionScript>().EnhanceChild == true)
                    {
                        var attachedModifiedValue = grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().DiceModifiedValue;

                        attachedModifiedValue = grid.GetComponent<GridPositionScript>().GridToDiceModifier + grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().DiceRolledValue - grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount + grid.GetComponent<GridPositionScript>().ArtifactChildModifier + grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().AmplifyLevel;
                        //Debug.Log("determine Dice Effect - not null + enhance child - after" + grid.name + " " + grid.GetComponent<GridPositionScript>().AttachedDice + "- dice roll modified by enhance - " + attachedModifiedValue);
                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Attack" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            attackValues.Add(attachedModifiedValue);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Enhance" )
                        {
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Slam" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            attackValues.Add(attachedModifiedValue + Player.DefensePoints);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Energy" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            Player.GetComponent<Player>().EnergyBuff = true;
                            Debug.Log("Energy Die detected");
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Bash" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            if(MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 != 0)
                            {
                                MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 = 0;
                                MouseController.selectedUnit.GetComponent<EnemyAI>().SpriteRenderer_Slot1.sprite = null;   
                            }
                            else if(MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 == 0 && MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 != 0)
                            {
                                MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 = 0;
                                MouseController.selectedUnit.GetComponent<EnemyAI>().SpriteRenderer_Slot2.sprite = null;   
                            }
                            else if(MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 == 0 && MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 == 0)
                            {
                                MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction3 = 0;
                                MouseController.selectedUnit.GetComponent<EnemyAI>().SpriteRenderer_Slot3.sprite = null;   
                            }
                            attackValues.Add(attachedModifiedValue);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Cleave" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            var findEnemy = GameObject.FindGameObjectsWithTag("Enemy");

                            // foreach (GameObject enemy in findEnemy)
                            // {
                            //     enemy.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            // }

                            int enemyCount = findEnemy.Length;
                            int damagePerEnemy = Mathf.CeilToInt((float)attachedModifiedValue / enemyCount);

                            foreach (GameObject enemy in findEnemy)
                            {
                                enemy.GetComponent<EnemyAI>().TakeDamage(damagePerEnemy);

                                if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                                {
                                    enemy.GetComponent<EnemyAI>().PoisonTickDamage ++;
                                }
                            }

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                            
                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Defense" )
                        {
                            defendValues.Add(attachedModifiedValue);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Heal" )
                        {
                            Player.CurrentHealth = Player.CurrentHealth + attachedModifiedValue;

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }
                        
                        //this line might be causing a bug
                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Nemesis" )
                        {
                            DiceBagScript.diceBag.Remove(grid.GetComponent<GridPositionScript>().AttachedDice.gameObject);
                            Debug.Log("testing removal of nemesis die from dicebag" + grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.name);
                        }
                    }
                else
                    {
                        var attachedModifiedValue = grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().DiceModifiedValue;

                        attachedModifiedValue = grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().DiceRolledValue - grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount + grid.GetComponent<GridPositionScript>().ArtifactChildModifier + grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().AmplifyLevel;
                        //Debug.Log("determine Dice Effect - not null + enhance child - after" + grid.name + " " + grid.GetComponent<GridPositionScript>().AttachedDice + "- dice roll unmodified by enhance - " + attachedModifiedValue);
                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Attack" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            attackValues.Add(attachedModifiedValue);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Enhance" )
                        {
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }
                        
                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Slam" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            attackValues.Add(attachedModifiedValue + Player.DefensePoints);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }
                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Energy" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            Player.GetComponent<Player>().EnergyBuff = true;
                            Debug.Log("Energy Die detected");
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Bash" )
                        {
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attachedModifiedValue);
                            //attackValues.Add(attachedModifiedValue);
                            if(MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 != 0)
                            {
                                MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 = 0;
                                MouseController.selectedUnit.GetComponent<EnemyAI>().SpriteRenderer_Slot1.sprite = null;   
                            }
                            else if(MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 == 0 && MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 != 0)
                            {
                                MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 = 0;
                                MouseController.selectedUnit.GetComponent<EnemyAI>().SpriteRenderer_Slot2.sprite = null;   
                            }
                            else if(MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction1 == 0 && MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 == 0)
                            {
                                MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction3 = 0;
                                MouseController.selectedUnit.GetComponent<EnemyAI>().SpriteRenderer_Slot3.sprite = null;   
                            }

                            // MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction2 = 0;
                            // MouseController.selectedUnit.GetComponent<EnemyAI>().RandomAction3 = 0;
                            attackValues.Add(attachedModifiedValue);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Cleave" )
                        {
                            
                            var findEnemy = GameObject.FindGameObjectsWithTag("Enemy");

                            int enemyCount = findEnemy.Length;
                            int damagePerEnemy = Mathf.CeilToInt((float)attachedModifiedValue / enemyCount);

                            foreach (GameObject enemy in findEnemy)
                            {
                                enemy.GetComponent<EnemyAI>().TakeDamage(damagePerEnemy);

                                if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                                {
                                    enemy.GetComponent<EnemyAI>().PoisonTickDamage ++;
                                }
                            }

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }

                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Defense" )
                        {
                            defendValues.Add(attachedModifiedValue);

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }

                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Heal" )
                        {
                            Player.CurrentHealth = Player.CurrentHealth + attachedModifiedValue;

                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Poison")
                            {
                                poisonValues.Add(1 * Player.poisonDamage);
                            }
                            if (grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceSecondaryType == "Glass")
                            {
                                grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().GlassUsed = true;
                            }
                        }

                        //might be causing a bug
                        if ( grid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceType == "Nemesis" )
                        {
                            DiceBagScript.diceBag.Remove(grid.GetComponent<GridPositionScript>().AttachedDice.gameObject);
                            Debug.Log("testing removal of nemesis die from dicebag" + grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.name);
                        }
                        
                    }
            }
        }  

        int poisonSum = poisonValues.Sum();
        if ( MouseController.selectedUnit != null && poisonSum >= 1)
        {
            MouseController.selectedUnit.GetComponent<EnemyAI>().PoisonTickDamage = MouseController.selectedUnit.GetComponent<EnemyAI>().PoisonTickDamage + poisonSum;
        }

        int defendSum = defendValues.Sum();
        Player.GetComponent<Player>().DefensePoints = Player.GetComponent<Player>().DefensePoints + defendSum;

        int attackSum = attackValues.Sum();
        if ( MouseController.selectedUnit != null && attackSum >= 1)
        {
            MouseController.selectedUnit.GetComponent<EnemyAI>().TakeDamage(attackSum);
        }   
        // else
        // {
        //     return;
        // }

    }

    public void DetermineDiceEffectCoroutine_function()
    {
        StartCoroutine(DetermineDiceEffectCoroutine());
    }

    IEnumerator DetermineDiceEffectCoroutine()
    {
        DetermineDiceEffect();
        yield return new WaitForSeconds(.25f);
    }

}


