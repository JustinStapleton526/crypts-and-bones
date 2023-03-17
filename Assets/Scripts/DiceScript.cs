using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour
{

    DiceHoverTextManager DiceHoverTextManager;
    MouseController MouseController;
    SoundManager SoundManager;

    //Array of dice sides sprites to load from Resources doler
    private Sprite[] diceSides;
    //Reference to sprite renderer to change sprites
    private SpriteRenderer rend;
    [Header("Dice Rolled Value")]
    public int DiceRolledValue;
    public int DiceModifiedValue;

    [Header("Dice Parameters")]
    public string DiceName;
    public string DiceType;
    //Secondary characteristics such as: Normal, Hollow, Weighted, Ephemeral, Miracle
    public string DiceSecondaryType;
    //Rarity impacts the starting dice range
    public string DiceRarity;
    public string DiceDescription;
    public bool NewlySpawnedDice = false;
    public bool EnemyDice = false;
    public bool GlassUsed = false;

    [Header("Dice Parameters - Rolling Range")]
    public int DiceStartRange;
    public int DiceEndRange;

    [Header("Dice Parameters - in Use / List Management")]
    public bool inHand;
    public bool returnToBag;
    public Vector3 StartingHandLocation;
    public Vector3 LastGridLocation;
    public Vector3 CurrentGridLocation;

    [Header("Dice Parameters - Sprite / Animations")]
    public int[] CommonSides = {1,1,1,2,2,3};
    public int[] RareSides = {2,3,3,3,4,4};
    public int[] EpicSides = {3,4,4,4,5,5};
    public int[] MythicSides = {4,5,5,5,6,6};
    public Sprite[] DiceSprites;

    [Header("Dice Parameters - Enhance Dice Special Params")]
    public int[] EnhanceCommonSides = {1,1,1,1,2,2};
    public int[] EnhanceRareSides = {1,1,1,2,2,2};
    public int[] EnhanceEpicSides = {1,2,2,2,3,3};
    public int[] EnhanceMythicSides = {2,2,2,3,3,4};

    public int[] EnergySides = {1,1,1,1,1,1};

    [Header("Dice Parameters - Amplify Dice Special Params")]
    // Grow dice only have 2 values and they grow each turn they are used
    public bool isAmplifyDice;
    public int[] AmplifySides1 = {2,2,3,3,3,4};
    public int[] AmplifySides2 = {3,3,4,4,4,5};
    public int[] AmplifySides3 = {4,4,5,5,5,6};
    public int[] AmplifySides4 = {5,5,6,6,6,6};
    public int AmplifyLevel = 1; // each time this is used, we will increase the level by 1


    [Header("Dice Properties -- Animation Objects")]
    public GameObject AmplifyAnimation;
    public GameObject EphemeralAnimation;
    public GameObject WeightedAnimation;
    public GameObject GlassAnimation;
    public GameObject LandmarkAnimation;
    public GameObject PoisonAnimation;

    [Header("Audio reference")]
    private bool _isSoundPlaying = false;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioClip _HoverTextClip;


    // Use this for initialization 
    private void Start()
    {
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        MouseController = GameObject.FindObjectOfType<MouseController>();
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("AnimatedDiceSides/");  
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        CurrentGridLocation = transform.position;

        if (DiceSecondaryType == "Amplify")
        {
            isAmplifyDice = true;
            if (this.GetComponent<SpriteRenderer>().enabled)
            {
                AmplifyAnimation.SetActive(true); 
            }
            else
            {
                AmplifyAnimation.SetActive(false);
            }
            
        }
        else if (DiceSecondaryType == "Ephemeral")
        {
            if (this.GetComponent<SpriteRenderer>().enabled)
            {
                EphemeralAnimation.SetActive(true); 
            }
            else
            {
                EphemeralAnimation.SetActive(false);
            }
        }
        else if (DiceSecondaryType == "Weighted")
        {

            if (this.GetComponent<SpriteRenderer>().enabled)
            {
                WeightedAnimation.SetActive(true);
            }
            else
            {
                WeightedAnimation.SetActive(false);
            }

        }
        else if (DiceSecondaryType == "Glass")
        {

            if (this.GetComponent<SpriteRenderer>().enabled)
            {
                GlassAnimation.SetActive(true); 
            }
            else
            {
                GlassAnimation.SetActive(false);
            }
        }
        else if (DiceSecondaryType == "Landmark")
        {

            if (this.GetComponent<SpriteRenderer>().enabled)
            {
                LandmarkAnimation.SetActive(true);
            }
            else
            {
                LandmarkAnimation.SetActive(false);
            }
        }
        else if (DiceSecondaryType == "Poison")
        {

            if (this.GetComponent<SpriteRenderer>().enabled)
            {
                PoisonAnimation.SetActive(true);
            }
            else
            {
                PoisonAnimation.SetActive(false);
            }
        }
        else
        {
            AmplifyAnimation.SetActive(false);
            EphemeralAnimation.SetActive(false);
            WeightedAnimation.SetActive(false);
            GlassAnimation.SetActive(false);
            LandmarkAnimation.SetActive(false);
            PoisonAnimation.SetActive(false);
        }


    }

    public void OnMouseOver() 
    {

        if (!_isSoundPlaying)
            {
                //SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }

        if(DiceType == "Attack")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#C52E1B>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Deals damage to the selected enemy";

        }

        if(DiceType == "Defense")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#2678AD>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Provides armor to absorb enemy attacks.";
        }

        if(DiceType == "Heal")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#77B00C>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Heals your hit points. ";
        }

        if(DiceType == "Enhance")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#FF5AFF>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Boost the values of nearby tiles.";
        }

        if(DiceType == "Bash")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#CC6912>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Deal damage and stun the selected enemy.";
        }

        if(DiceType == "Cleave")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#6719E7>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Deals damage to all enemies.";
        }

        if(DiceType == "Energy")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#00FA28>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Grants 1 additional energy next turn.";
        }

        if(DiceType == "Slam")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#1BC5B1>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "Deals damage based on defense.";
        }

        if(DiceType == "Nemesis")
        {
                DiceHoverTextManager.HoverText1.text = "<color=#FFF200>" + DiceType + "</color>" + " | <color=#C51B69>" + DiceSecondaryType + "</color>" +
            "\n" + "An enemy die clogs your hand...";
        }


            
        
        // else
        // {
        //     DiceHoverTextManager.HoverText1.text = "Type: " + DiceName +
        //             "\n" + "Rarity: " + DiceRarity +
        //             "\n" + "Sub-Type: " + DiceSecondaryType;
        // }

                    


        DiceHoverTextManager.HoverPanelLocation = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
        DiceHoverTextManager.HoverTextPanel.SetActive(true);
        DiceHoverTextManager.HoverTextPanel.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation.x+3.8f, DiceHoverTextManager.HoverPanelLocation.y+.25f);

        if(DiceRarity == "Common" && DiceType != "Enhance")
        {
            DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[0];
            DiceHoverTextManager.HoverTextRarity.text = "<color=#FFFFFF> Common </color>";

            if(DiceType == "Energy")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[9];
            }

            if(DiceSecondaryType == "Landmark" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
            }
            else if(DiceSecondaryType == "Ephemeral" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Ephemeral dice can be placed on the grid for 0 energy.";
            }
            else if(DiceSecondaryType == "Glass" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
            }
            else if(DiceSecondaryType == "Weighted" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
            }
            else if(DiceSecondaryType == "Poison" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
            }
            else if(DiceType == "Nemesis" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "\n\nNemesis dice have no effect when placed on the grid.";
            }
            else if(DiceSecondaryType == "Amplify" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
            }
        }
        else if (DiceRarity == "Rare" && DiceType != "Enhance")
        {
            DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[1];
            DiceHoverTextManager.HoverTextRarity.text = "<color=#006DBA> Rare </color>";
            if(DiceType == "Energy")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[9];
            }

            if(DiceSecondaryType == "Landmark" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
            }
            else if(DiceSecondaryType == "Ephemeral" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Ephemeral dice can be placed on the grid for 0 energy.";
            }
            else if(DiceSecondaryType == "Glass" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
            }
            else if(DiceSecondaryType == "Weighted" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
            }
            else if(DiceSecondaryType == "Poison" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
            }
            else if(DiceSecondaryType == "Amplify" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
            }
        }
        else if (DiceRarity == "Epic" && DiceType != "Enhance")
        {
            DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[2]; 
            DiceHoverTextManager.HoverTextRarity.text = "<color=#8100B9> Epic </color>";

            if(DiceType == "Energy")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[9];
            }
            
            if(DiceSecondaryType == "Landmark" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
            }
            else if(DiceSecondaryType == "Ephemeral" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Ephemeral dice can be placed on the grid for 0 energy.";
            }
            else if(DiceSecondaryType == "Glass" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
            }
            else if(DiceSecondaryType == "Weighted" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
            }   
            else if(DiceSecondaryType == "Poison" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
            }
            else if(DiceSecondaryType == "Amplify" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
            }
        }
        else if (DiceRarity == "Mythic"  && DiceType != "Enhance")
        {
            DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[3];
            DiceHoverTextManager.HoverTextRarity.text = "<color=#FF9309> Mythic </color>";

            if(DiceType == "Energy")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[9];
            }
            
            if(DiceSecondaryType == "Landmark" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
            }
            else if(DiceSecondaryType == "Ephemeral" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Ephemeral dice can be placed on the grid for 0 energy.";
            }
            else if(DiceSecondaryType == "Glass" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text =  
                "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
            }
            else if(DiceSecondaryType == "Weighted" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
            }    
            else if(DiceSecondaryType == "Poison" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
            } 
            else if(DiceSecondaryType == "Amplify" && inHand == true)
            {
                DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                DiceHoverTextManager.HoverText2.text = 
                "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
            }
        }
        
        else if (DiceType == "Enhance")
        {
            if (DiceRarity == "Common")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[5]; 
                DiceHoverTextManager.HoverTextRarity.text = "<color=#FFFFFF> Common </color>";
                if(DiceSecondaryType == "Landmark" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
                }
                else if(DiceSecondaryType == "Ephemeral" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Ephemeral dice can be placed on the grid for 0 energy.";
                }
                else if(DiceSecondaryType == "Glass" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
                }
                else if(DiceSecondaryType == "Weighted" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
                } 
                else if(DiceSecondaryType == "Poison" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
                }
                else if(DiceSecondaryType == "Amplify" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
                }
            }
            else if (DiceRarity == "Rare")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[6];
                DiceHoverTextManager.HoverTextRarity.text = "<color=#006DBA> Rare </color>";
                if(DiceSecondaryType == "Landmark" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
                }
                else if(DiceSecondaryType == "Ephemeral" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Ephemeral dice can be placed on the grid for 0 energy.";
                }
                else if(DiceSecondaryType == "Glass" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
                }
                else if(DiceSecondaryType == "Weighted" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
                }  
                else if(DiceSecondaryType == "Poison" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
                }
                else if(DiceSecondaryType == "Amplify" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
                }
            }
            else if (DiceRarity == "Epic")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[7];
                DiceHoverTextManager.HoverTextRarity.text = "<color=#8100B9> Epic </color>"; 
                if(DiceSecondaryType == "Landmark" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
                }
                else if(DiceSecondaryType == "Ephemeral" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Ephemeral dice can be placed on the grid for 0 energy.";
                }
                else if(DiceSecondaryType == "Glass" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
                }
                else if(DiceSecondaryType == "Weighted" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
                } 
                else if(DiceSecondaryType == "Poison" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
                }
                else if(DiceSecondaryType == "Amplify" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
                }
            }
            else if (DiceRarity == "Mythic")
            {
                DiceHoverTextManager.HoverDiceDetails.sprite = DiceHoverTextManager.HoverDiceSides[8]; 
                DiceHoverTextManager.HoverTextRarity.text = "<color=#FF9309> Mythic </color>";
                if(DiceSecondaryType == "Landmark" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Landmark dice that are placed on the grid prevent that grid space from being blocked.";
                }
                else if(DiceSecondaryType == "Ephemeral" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Ephemeral dice can be placed on the grid for 0 energy.";
                }
                else if(DiceSecondaryType == "Glass" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text =  
                    "Glass dice can only be used once per battle. Returned to your dicebag at the end of the floor";
                }
                else if(DiceSecondaryType == "Weighted" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Weighted dice can be placed on any grid space for 2 energy. Value doubled on play.";
                } 
                else if(DiceSecondaryType == "Poison" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Poison dice will deal damage directly to the enemy's health at the end of the turn.";
                }
                else if(DiceSecondaryType == "Amplify" && inHand == true)
                {
                    DiceHoverTextManager.HoverTextPanel2.SetActive(true); 
                    DiceHoverTextManager.HoverText2.text = 
                    "Amplify dice will increase in value each time they are played.\nAmplify Level: +" + AmplifyLevel;
                }
            }
        }
    }
    public void OnMouseExit() 
    {
        DiceHoverTextManager.HoverTextPanel.SetActive(false); 
        DiceHoverTextManager.HoverTextPanel2.SetActive(false);
        if (_isSoundPlaying)
            {
                _isSoundPlaying = false;
            }
    }

    public void Roll()
    {
        StartCoroutine("RollTheDice");
    }
    
    //Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        //It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;


        //Loop to switch dice sides randomly
        // before final side appears. 20 iterations will happen
        for (int i = 0; i <= 10; i++)
        {
            //Pick up random value from 0 to 5 (all inclusive)
            randomDiceSide = Random.Range(DiceStartRange,DiceEndRange);

            //Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            //Pause before next iteration
            yield return new WaitForSeconds(0.05f);
        }

        int randomIndex = UnityEngine.Random.Range(0, CommonSides.Length);

        // Now we can use the random index to get the value of the dice roll.
        if (DiceRarity == "Common" && DiceType != "Energy")
        {
            if (DiceType == "Enhance")
            {
                int diceRoll = EnhanceCommonSides[randomIndex];  
                // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);
            }
            
            else
            {
                int diceRoll = CommonSides[randomIndex];  
                // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite; 
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);  
            }

        }
        if (DiceRarity == "Rare" && DiceType != "Energy")
        {
            if (DiceType == "Enhance")
            {
                int diceRoll = EnhanceRareSides[randomIndex];  
                // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);
            }
            
            else
            {
                int diceRoll = RareSides[randomIndex]; 
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);  
            }
        }
        if (DiceRarity == "Epic" && DiceType != "Energy")
        {
            if (DiceType == "Enhance")
            {
                int diceRoll = EnhanceEpicSides[randomIndex];  
                // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);
            }
            
            else
            {
                int diceRoll = EpicSides[randomIndex];
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);
            }  
        }
        if (DiceRarity == "Mythic" && DiceType != "Energy")
        {
            if (DiceType == "Enhance")
            {
                int diceRoll = EnhanceMythicSides[randomIndex];  
                // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);
            }
            
            else
            {
                int diceRoll = MythicSides[randomIndex];
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);   
            }
        }


        //THIS IS THE OLD LOGIC FOR AMPLIFY THAT WILL NEED TO BE DELETED
        // if (DiceSecondaryType == "Amplify" && DiceType != "Energy")
        // {
        //     if (AmplifyLevel == 1)
        //     {
        //         int diceRoll = AmplifySides1[randomIndex];  
        //         // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
        //         Sprite diceSprite = DiceSprites[diceRoll - 1];
        //         gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
        //         DiceRolledValue = diceRoll;
        //         yield return new WaitForSeconds(.3f);
        //         //Show final dice value in Console
        //         // Debug.Log("DiceScript - Ienum: " + DiceRolledValue + " - " + DiceType);
        //     }
        //     if (AmplifyLevel == 2)
        //     {
        //         int diceRoll = AmplifySides2[randomIndex];  
        //         // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
        //         Sprite diceSprite = DiceSprites[diceRoll - 1];
        //         gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
        //         DiceRolledValue = diceRoll;
        //         yield return new WaitForSeconds(.3f);
        //         //Show final dice value in Console
        //         // Debug.Log("DiceScript - Ienum: " + DiceRolledValue + " - " + DiceType); 
        //     }
        //     if (AmplifyLevel == 3)
        //     {
        //         int diceRoll = AmplifySides3[randomIndex];  
        //         // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
        //         Sprite diceSprite = DiceSprites[diceRoll - 1];
        //         gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
        //         DiceRolledValue = diceRoll;
        //         yield return new WaitForSeconds(.3f);
        //         //Show final dice value in Console
        //         // Debug.Log("DiceScript - Ienum: " + DiceRolledValue + " - " + DiceType);
        //     }
        //     if (AmplifyLevel >= 4)
        //     {
        //         int diceRoll = AmplifySides4[randomIndex];  
        //         // Finally, we can use the dice roll value to pick the correct sprite from the DiceSprites array.
        //         Sprite diceSprite = DiceSprites[diceRoll - 1];
        //         gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
        //         DiceRolledValue = diceRoll;
        //         yield return new WaitForSeconds(.3f);
        //         //Show final dice value in Console
        //         // Debug.Log("DiceScript - Ienum: " + DiceRolledValue + " - " + DiceType);    
        //     }
        // }

        if (DiceType == "Energy")
            {
                int diceRoll = EnergySides[randomIndex];
                Sprite diceSprite = DiceSprites[diceRoll - 1];
                gameObject.GetComponent<SpriteRenderer>().sprite = diceSprite;
                DiceRolledValue = diceRoll;
                yield return new WaitForSeconds(.3f);   
            }
    }
}
