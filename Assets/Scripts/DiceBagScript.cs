using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class DiceBagScript : MonoBehaviour
{

    private static DiceBagScript _instance;
    public static DiceBagScript Instance { get { return _instance; } }

    GameManager GameManager;
    GridPositionScript GridPositionScript;
    Player Player;
    LevelManager LevelManager;
    SoundManager SoundManager;

    [Header("Dice Bag Inventory Image")]
    public GameObject RaycastBlockerDiceBag;

    [Header("Dice Bag Inventory Image")]
    public SpriteRenderer DiceBagInventory;

    [Header("Dice Bag Parameters")]
    public static List<GameObject> diceBag = new List<GameObject>();
    public static List<GameObject> handPositions = new List<GameObject>();
    public int diceBagCount;
    public Vector3 CurrentPosition;

    [Header("Hand Position Objects")]
    public GameObject HandPosition1;
    public GameObject HandPosition2;
    public GameObject HandPosition3;
    public GameObject HandPosition4;
    public GameObject HandPosition5;
    HandPositionScript HandPositionScript;

    [Header("NemesisDice")]
    public GameObject NemesisDice;

    [Header("Reroll button reference")]
    public Button button;

    [Header("Audio reference")]
    [SerializeField] private AudioClip _diceRollClip;
    public AudioClip _diceBagOpenClip;
    [SerializeField] private AudioClip _gridButtonClip;


    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }


    void Start()
    {
        FindAllDice();
        HandPositionScript = GameObject.FindObjectOfType<HandPositionScript>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceBagInventory = DiceBagInventory.GetComponent<SpriteRenderer>();
        GridPositionScript = GameObject.FindObjectOfType<GridPositionScript>();
        Player = GameObject.FindObjectOfType<Player>();
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    private void Update() 
    {
      SortDiceGrid();

    }

    public void RemoveFromDiceBag(GameObject dice2)
    {
        diceBag.Remove(dice2);
        Debug.Log("dicebagscript test 2" + dice2);
        diceBagCount = diceBag.Count;
    }


    public void FindAllDice()
    {
        //Associate all active dice objects in the game view to our diceBag list.
        var findDice = GameObject.FindGameObjectsWithTag("Dice");
        foreach (GameObject dice in findDice)
        {
            diceBag.Add(dice);
            diceBagCount = diceBag.Count;
            var diceVars = dice.GetComponent<DiceScript>();
            diceVars.inHand = false;
            diceVars.returnToBag = false;
            // Debug.Log("Dice Bag Count: "+ diceBagCount + "\n Dice Name: "+ diceVars.DiceName);
        }
    }

    void SortDiceGrid()
    {

        diceBag.RemoveAll(item => item == null);

        int rows = 5;
        int columns = 5;
        int index = 0;
        float cellWidth = .5f;
        float cellHeight = .5f;
        float spacingX = .31f;
        float spacingY = .32f;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (index < diceBag.Count)
                {
                    GameObject dice = diceBag[index];
                    float x = column * (cellWidth + spacingX);
                    float y = row * (cellHeight + spacingY);
                    dice.transform.localPosition = new Vector3(x, -y, 0);
                    index++;
                }
            }
        }

        bool inventoryEnabled = DiceBagInventory.enabled;

        foreach (GameObject dice in diceBag)
        {
            SpriteRenderer spriteRenderer = dice.GetComponent<SpriteRenderer>();
            Drag dragScript = dice.GetComponent<Drag>();

            spriteRenderer.enabled = inventoryEnabled; 
            // if (dragScript != null)
            // {
            //     dragScript.enabled = !inventoryEnabled;
            // }
        }

        RaycastBlockerDiceBag.SetActive(!inventoryEnabled);

    }

    public void EnableDiceBagSprites()
    {
        SoundManager.Instance.PlayLoudSound(_diceBagOpenClip);
            if (DiceBagInventory.enabled == false)
            {
                DiceBagInventory.enabled = true;
                //Play audio clip
            }
            else
            {
                DiceBagInventory.enabled = false;
            }

    }

    public void CreateNemesisDice()
    {
        Debug.Log("Dicebagscript - Nemesis Dice created");
        GameObject nemesisDice = Instantiate(NemesisDice) as GameObject;
        nemesisDice.transform.SetParent(this.transform);
        diceBag.Add(nemesisDice);
        diceBagCount = diceBag.Count;
    }

    public void ReturnUsedDice()
    {
        // This should happen only if energy level is >0 .. etc if you drag 4 or 5 dice on the grid it will say -1 or -2 and be invalid
        //Any Dice tagged with returnToBag = true, we will re-add them to the diceBag list.

        //Associate all active dice objects in the game view to our diceBag list.

        if(Player.GetComponent<Player>().Energy >= 0)
        {
            SoundManager.Instance.PlayLoudSound(_gridButtonClip);

            var findDice = GameObject.FindGameObjectsWithTag("Dice");

            foreach (GameObject dice in findDice)
            {
                if (dice.GetComponent<DiceScript>().returnToBag == true)
                    {
                        dice.GetComponent<Drag>().HandPosBinding = null;
                        if (dice.GetComponent<DiceScript>().GlassUsed != true)
                            {
                                diceBag.Remove(dice);
                                diceBagCount = diceBag.Count;

                                diceBag.Add(dice);
                                diceBagCount = diceBag.Count;

                                var diceVars = dice.GetComponent<DiceScript>();
                                diceVars.inHand = false;
                                diceVars.returnToBag = false;
                                dice.GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = false;
                                dice.transform.position = new Vector2(-20,-20);

                                //add code here to execute attacks / defense / heal 
                                GridPositionScript.DetermineDiceEffectCoroutine_function();
                                // Debug.Log("return function: " + dice);
                                // Debug.Log("Dice Bag Count: "+ diceBagCount + "\n Dice Name: "+ diceVars.DiceName);

                                if (diceVars.isAmplifyDice == true)
                                    {
                                        diceVars.AmplifyLevel++;
                                    }


                                var findGrids = GameObject.FindGameObjectsWithTag("Grid");
                                
                                foreach (GameObject grid in findGrids)
                                    {
                                        if(grid.GetComponent<GridPositionScript>().LandMarkGrid == true)
                                        {
                                            grid.GetComponent<GridPositionScript>().LandMarkGridPersist = true;
                                            grid.GetComponent<GridPositionScript>().SlotInUse = false;
                                            grid.GetComponent<GridPositionScript>().EnhanceRoot = false;
                                            grid.GetComponent<GridPositionScript>().EnhanceChild = false;
                                            grid.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                                            grid.GetComponent<GridPositionScript>().AttachedDice = null;
                                            grid.GetComponent<GridPositionScript>().AttachedDiceType = null;
                                            grid.GetComponent<GridPositionScript>().AttachedDiceValue = 0;

                                            // grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
                                            // grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount = 0;
                                        }
                                        else
                                        {
                                            grid.GetComponent<GridPositionScript>().SlotInUse = false;
                                            grid.GetComponent<GridPositionScript>().EnhanceRoot = false;
                                            grid.GetComponent<GridPositionScript>().EnhanceChild = false;
                                            grid.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                                            grid.GetComponent<GridPositionScript>().AttachedDice = null;
                                            grid.GetComponent<GridPositionScript>().AttachedDiceType = null;
                                            grid.GetComponent<GridPositionScript>().AttachedDiceValue = 0;
                                            // grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
                                            // grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount = 0;

                                            if (grid.GetComponent<GridPositionScript>().BlockCountdown >= 1)
                                            {
                                            grid.GetComponent<GridPositionScript>().BlockCountdown --;
                                            }
                                        }
                                        
                                    }
                            }
                        else
                        {
                            diceBag.Remove(dice);
                            diceBagCount = diceBag.Count;

                            var diceVars = dice.GetComponent<DiceScript>();
                            
                            diceVars.inHand = false;
                            diceVars.returnToBag = false;
                            dice.GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = false;
                            dice.transform.position = new Vector2(-20,-20);

                            //add code here to execute attacks / defense / heal 
                            GridPositionScript.DetermineDiceEffectCoroutine_function();
                            // Debug.Log("return function: " + dice);
                            // Debug.Log("Dice Bag Count: "+ diceBagCount + "\n Dice Name: "+ diceVars.DiceName);

                            if (diceVars.isAmplifyDice == true)
                                {
                                    diceVars.AmplifyLevel++;
                                }


                            var findGrids = GameObject.FindGameObjectsWithTag("Grid");
                            
                            foreach (GameObject grid in findGrids)
                                {
                                    if(grid.GetComponent<GridPositionScript>().LandMarkGrid == true)
                                    {
                                        grid.GetComponent<GridPositionScript>().LandMarkGridPersist = true;
                                        grid.GetComponent<GridPositionScript>().SlotInUse = false;
                                        grid.GetComponent<GridPositionScript>().EnhanceRoot = false;
                                        grid.GetComponent<GridPositionScript>().EnhanceChild = false;
                                        grid.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                                        grid.GetComponent<GridPositionScript>().AttachedDice = null;
                                        grid.GetComponent<GridPositionScript>().AttachedDiceType = null;
                                        grid.GetComponent<GridPositionScript>().AttachedDiceValue = 0;
                                        // grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
                                        // grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount = 0;
                                    }
                                    else
                                    {
                                        grid.GetComponent<GridPositionScript>().SlotInUse = false;
                                        grid.GetComponent<GridPositionScript>().EnhanceRoot = false;
                                        grid.GetComponent<GridPositionScript>().EnhanceChild = false;
                                        grid.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                                        grid.GetComponent<GridPositionScript>().AttachedDice = null;
                                        grid.GetComponent<GridPositionScript>().AttachedDiceType = null;
                                        grid.GetComponent<GridPositionScript>().AttachedDiceValue = 0;
                                        // grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
                                        // grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount = 0;

                                        if (grid.GetComponent<GridPositionScript>().BlockCountdown >= 1)
                                            {
                                            grid.GetComponent<GridPositionScript>().BlockCountdown --;
                                            }
                                    }
                                }
                        }
                    } 
            }
            //GameManager.DiceBoardFadeOut();
            GameManager.DiceBoardMoveOutOfView(); // new pending removal

            Player.GetComponent<Player>().Energy = 3; 

            if(Player.GetComponent<Player>().EnergyBuff == true)
            {
                Player.GetComponent<Player>().Energy = 4;
                Player.GetComponent<Player>().EnergyBuff = false;
            }

            //check if level manager is 0 
            GameManager.GetComponent<GameManager>().StateChangeButtonClick();
        }
        else
        {
            Debug.Log("Energy Problem - value is less than 0");
            return;
        }
    }


    public void RollAndDealDice()
    {
        //Associate all active dice objects in our hand to handDice list.
        var findHands = GameObject.FindGameObjectsWithTag("Hand");
        foreach (GameObject hand in findHands)
        {
            handPositions.Add(hand);
            var handVars = hand.GetComponent<HandPositionScript>();
            if (handVars.OpenDiceSlot == true)
            {
                if (diceBagCount >= 1)
                { 
                        // Grab random dice. 
                        var randDice = diceBag[Random.Range(0, diceBagCount)];
                        // Debug.Log("\n Random - Dice Name: " + randDice.GetComponent<DiceScript>().DiceName);

                        // Remove the random dice from the diceBag list.
                        diceBag.Remove(randDice);
                        diceBagCount = diceBag.Count;
                        // Debug.Log("\n Remove from Dicebag - Random - Dice Name: " + randDice.GetComponent<DiceScript>().DiceName);

                        // Update the transform position of the dice and associate the hand position to the dice after drawing.
                        randDice.transform.position = hand.GetComponent<HandPositionScript>().HandLocation;
                        randDice.GetComponent<DiceScript>().StartingHandLocation = hand.GetComponent<HandPositionScript>().HandLocation;
                        // Debug.Log("\n Random - Dice Name: " + randDice.GetComponent<DiceScript>().DiceName + " hand location: " + hand.GetComponent<HandPositionScript>().HandLocation);

                        //Update sprite renderer
                        randDice.GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = true;

                        // Update the inHand bool.
                        randDice.GetComponent<DiceScript>().inHand = true;

                        // Roll the dice.
                        randDice.GetComponent<DiceScript>().Roll();
                        // Update the OpenDiceSlot bool.
                        hand.GetComponent<HandPositionScript>().OpenDiceSlot = false;

                        //Play audio clip of dice rolling
                        SoundManager.Instance.PlaySound(_diceRollClip);
                }  
                else 
                {
                        Debug.Log("The dicebag has no more dice!");
                        return;
                }
            } 
            else
            {
                Debug.Log("No more handpositions are available!");  
            }
        }
    }


    public void ReturnUsedDice_LevelEnd()
    {
        // This should happen only if energy level is >0 .. etc if you drag 4 or 5 dice on the grid it will say -1 or -2 and be invalid
        //Any Dice tagged with returnToBag = true, we will re-add them to the diceBag list.

        //Associate all active dice objects in the game view to our diceBag list.
        var findDice = GameObject.FindGameObjectsWithTag("Dice");

        foreach (GameObject dice in findDice)
            {
                diceBag.Remove(dice);
                diceBagCount = diceBag.Count;

                diceBag.Add(dice);
                diceBagCount = diceBag.Count;

                var diceVars = dice.GetComponent<DiceScript>();
                diceVars.inHand = false;
                diceVars.returnToBag = false;
                diceVars.GlassUsed = false;
                dice.GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = false;
                dice.transform.position = new Vector2(-20,-20);

                if (diceVars.isAmplifyDice == true)
                    {
                        diceVars.AmplifyLevel = 0;
                    }
                if (dice.GetComponent<DiceScript>().DiceType == "Nemesis")
                {
                    diceBag.Remove(dice);
                    diceBagCount = diceBag.Count;
                    Destroy(dice);
                }
                
            } 

        var findGrids = GameObject.FindGameObjectsWithTag("Grid");

        foreach (GameObject grid in findGrids)
            {
                grid.GetComponent<GridPositionScript>().SlotInUse = false;
                grid.GetComponent<GridPositionScript>().EnhanceRoot = false;
                grid.GetComponent<GridPositionScript>().EnhanceChild = false;
                grid.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                grid.GetComponent<GridPositionScript>().AttachedDice = null;
                grid.GetComponent<GridPositionScript>().AttachedDiceType = null;
                grid.GetComponent<GridPositionScript>().AttachedDiceValue = 0;
                grid.GetComponent<GridPositionScript>().isBlocked = false;
                grid.GetComponent<GridPositionScript>().BlockCountdown = 0;
                grid.GetComponent<GridPositionScript>().LandMarkGrid = false;
                grid.GetComponent<GridPositionScript>().LandMarkGridPersist = false;


                // grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
                // grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount = 0;
            }        
    }

    public void ReturnUsedDice_RerollButtonFunction()
    {
        SoundManager.Instance.PlayLoudSound(_gridButtonClip);
        //Any Dice tagged with returnToBag = true, we will re-add them to the diceBag list.

        //Associate all active dice objects in the game view to our diceBag list.
        var findDice = GameObject.FindGameObjectsWithTag("Dice");

        foreach (GameObject dice in findDice)
            {
                if(dice.GetComponent<DiceScript>().GlassUsed != true)
                {
                    diceBag.Remove(dice);
                    diceBagCount = diceBag.Count;

                    diceBag.Add(dice);
                    diceBagCount = diceBag.Count;

                    var diceVars = dice.GetComponent<DiceScript>();
                    diceVars.inHand = false;
                    diceVars.returnToBag = false;
                    dice.GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = false;
                    dice.transform.position = new Vector2(-20,-20);     
                }
            }

        var findHands = GameObject.FindGameObjectsWithTag("Hand");
        foreach (GameObject hand in findHands)
        {
            hand.GetComponent<HandPositionScript>().OpenDiceSlot = true;
        }   


        var findGrids = GameObject.FindGameObjectsWithTag("Grid");

        foreach (GameObject grid in findGrids)
            {
                grid.GetComponent<GridPositionScript>().SlotInUse = false;
                grid.GetComponent<GridPositionScript>().EnhanceRoot = false;
                grid.GetComponent<GridPositionScript>().EnhanceChild = false;
                grid.GetComponent<GridPositionScript>().EnhanceRootValue = 0;
                grid.GetComponent<GridPositionScript>().AttachedDice = null;
                grid.GetComponent<GridPositionScript>().AttachedDiceType = null;
                grid.GetComponent<GridPositionScript>().AttachedDiceValue = 0;
                // grid.GetComponent<GridPositionScript>().isBlocked = false;
                grid.GetComponent<GridPositionScript>().LandMarkGrid = false;
                // grid.GetComponent<GridPositionScript>().EnemyInfluence = false;
                // grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount = 0;
            }        
    }

    public void RerollButtonFunction()
    {
        StartCoroutine(RerollbuttonGUI());
    }

    public void EnableRerollButton()
    {
        //this is being enabled in the gamemanager script
        button.interactable = true; 
    }

    IEnumerator RerollbuttonGUI()
    {
        GameManager.RaycastBlocker.SetActive(true);
        //return all dice to dicebag
        ReturnUsedDice_RerollButtonFunction();
        //wait
        yield return new WaitForSeconds(.25f);
        //deal new dice to hand
        RollAndDealDice();
        //wait
        yield return new WaitForSeconds(.25f);
        //disable the rerollbutton
        button.interactable = false;
        Player.Energy = 3;
        GameManager.RaycastBlocker.SetActive(false);
    }
}
