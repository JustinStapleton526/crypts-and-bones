using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventLogicHandler : MonoBehaviour
{
    Player Player;
    LevelManager LevelManager;
    GameManager GameManager;
    DiceBagScript DiceBagScript;
    GridPositionScript GridPositionScript;
    MouseController mc;
    DiceHoverTextManager DiceHoverTextManager;
    RelicRewardManager RelicRewardManager;


    public int CurrentEvent;
    public bool Event1Complete = false;
    public bool Event2Complete = false;
    public bool Event3Complete = false;
    public bool Event4Complete = false;

    [Header("Healing Well Parameters")]
    public Text EventScreenBannerText;
    public Text EventScreenStoryBite;
    public Text EventScreenAcceptButtonText;
    public Text EventScreenDeclineButtonText;

    [Header("Event Dice Reward Parameters")]
    public GameObject _prefab_Defense;
    public GameObject _prefab_Enhance;
    public GameObject _prefab_Heal;
    public GameObject _prefab_Cleave;
    public GameObject _prefab_Bash;
    public GameObject _prefab_Slam;
    public Vector3 DiceEndPosition = new Vector3(-1.84f, 1.5f, 0f);

    

    void Start()
    {
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        Player = GameObject.FindObjectOfType<Player>();
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        mc = FindObjectOfType<MouseController>();
        GridPositionScript = GameObject.FindObjectOfType<GridPositionScript>();
        RelicRewardManager = GameObject.FindObjectOfType<RelicRewardManager>();  
    }
    
 
////////////////////////////////////////////////////////////////////////////
// Focus functions
    public void EventFocusFunction(int EventID)
    {
        Debug.Log("EventLogicHandler - EventID - " + EventID);
        CurrentEvent = EventID;
        StartCoroutine(EventFocusCoroutine(EventID));
    }

    IEnumerator EventFocusCoroutine(int EventID)
    {
        // move event screen into position -- change the name from healingwellevent to just eventscreen
        GameManager.EventScreen.transform.position = new Vector3(0,0,0);

        //fade out hand slots
        GameManager.HandSlotsFadeOut();
        yield return new WaitForSeconds(.25f);

        // different event systems story blurbs 
        if(EventID == 1)
        {
            EventScreenBannerText.text = "The Mysterious Well";
            EventScreenStoryBite.text = "the adventurer sees a strange, green mist pillowing from the corner of their eye.\nDespite their misgivings, they approach the mist- their sword drawn for battle. With each step, the mist grows thicker, obscuring their vision and stifling their breath. \nAs the green vapors part in front of them, an eerie well appears. The well is unlike any they have ever seen. Macabre avian idols decorate the surface...";
            EventScreenAcceptButtonText.text = "Drink from the mysterious well... [Heal]";
            EventScreenDeclineButtonText.text = "Clear your dice bag and move on... [Purge]";
        }
        else if (EventID == 2)
        {
            EventScreenBannerText.text = "The Glowing Crystal";
            EventScreenStoryBite.text = "the adventurer drew closer to the glowing crystal, their heart racing with anticipation. The crystal shone with a brilliant radiance, casting an ethereal glow upon the surrounding cave. With trembling hands, they reached out to touch the crystal, and as their fingers brushed against its surface, they felt a faint vibration...";
            EventScreenAcceptButtonText.text = "Place an epic Enhance die into your bag... [Receive Die]";
            EventScreenDeclineButtonText.text = "Ignore the crystal and clear your dice bag... [Purge]";
        }
        else if (EventID == 3)
        {
            EventScreenBannerText.text = "The Decorated Shield";
            EventScreenStoryBite.text = "the adventurer stumbled upon a decorative battle-hardened shield lying amidst the ruins of an ancient battlefield. Its intricate design was marred by the marks of numerous battles, but it was still in good condition...";
            EventScreenAcceptButtonText.text = "Place an epic Defense die into your bag... [Receive Die]";
            EventScreenDeclineButtonText.text = "Place an epic Slam die into your bag... [Receive Die]";
        }
        else if (EventID == 4)
        {
            EventScreenBannerText.text = "The Fallen Warrior";
            EventScreenStoryBite.text = "As the adventurer journeyed through the dark passages, they stumbled upon the bones of an old warrior, long since passed. Among the bones lay a collection of weapons, rusted and worn, but still serviceable. The adventurer felt a sense of reverence and awe as they gazed upon the weapons, knowing they had once been wielded by a great warrior in battle...";
            EventScreenAcceptButtonText.text = "Place an epic Cleave die into your bag... [Receive Die]";
            EventScreenDeclineButtonText.text = "Place an epic Bash die into your bag... [Receive Die]";
        }
        else if (EventID == 5)
        {
            EventScreenBannerText.text = "The Wandering Merchant";
            EventScreenStoryBite.text = "As the adventurer journeyed through the dark passages, they stumbled upon an odd merchant with a menacing twinkle in their eye. The merchant beckoned the adventurer over and showed them a collection of wares, each more peculiar than the last. There were trinkets and baubles from far-off lands, and strange artifacts that seemed to pulse with an otherworldly energy...";
            EventScreenAcceptButtonText.text = "Place a mythic Heal die into your bag... [Receive Die]";
            EventScreenDeclineButtonText.text = "Purchase a potion... [Heal]";
        }
        else if (EventID == 6)
        {
            EventScreenBannerText.text = "Pile of Scraps";
            EventScreenStoryBite.text = "As the adventurer journeyed through the dark passages, they stumbled upon a pile of scraps and debris, seemingly abandoned by a previous caravans. Despite the mess, the adventurer had a keen eye and saw potential in the wreckage. After rummaging through the scraps, they managed to salvage a rusty, but serviceable...";
            EventScreenAcceptButtonText.text = "Place an epic Defense die into your bag... [Receive Die]";
            EventScreenDeclineButtonText.text = "Ignore the scrap and clear your dice bag... [Purge]";
        }
        

        //move transition screen over current view
        GameManager.MoveTransitionPanelOutOfView();
        yield return new WaitForSeconds(1.5f);
    }

////////////////////////////////////////////////////////////////////////////
//Decline/skip function 
    public void EventDeclineFunction()
    {
        Debug.Log("EventLogicHandler - EventID - " + CurrentEvent);
        StartCoroutine(EventDeclineCoroutine());
    }

    IEnumerator EventDeclineCoroutine()
    {
        if(CurrentEvent == 1)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic.");
            GameManager.MoveEvent1OutOfView();
            yield return new WaitForSeconds(.75f);

            //Move to purge state
            GameManager.StateChangeToPurgeState();

            if(LevelManager.CurrentLevel == 3)
                {
                    Event1Complete = true;
                }
                else if(LevelManager.CurrentLevel == 6)
                {
                    Event2Complete = true;
                }
                else if(LevelManager.CurrentLevel == 9)
                {
                    Event3Complete = true;
                }
                else if(LevelManager.CurrentLevel == 12)
                {
                    Event4Complete = true;
                }
        }

        else if (CurrentEvent == 2)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic.");
            GameManager.MoveEvent1OutOfView();
            yield return new WaitForSeconds(.75f);

            //Move to purge state
            GameManager.StateChangeToPurgeState();

            if(LevelManager.CurrentLevel == 3)
                {
                    Event1Complete = true;
                }
                else if(LevelManager.CurrentLevel == 6)
                {
                    Event2Complete = true;
                }
                else if(LevelManager.CurrentLevel == 9)
                {
                    Event3Complete = true;
                }
                else if(LevelManager.CurrentLevel == 12)
                {
                    Event4Complete = true;
                }
        }

        else if (CurrentEvent == 3)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic.");

            InstantiateEventRewards_Slam();
            yield return new WaitForSeconds(1.5f);
            //move transition screen over current view
            GameManager.MoveTransitionPanelIntoView();
            yield return new WaitForSeconds(1.5f);

            // move event UI out of screen
            GameManager.EventScreen.transform.position = new Vector3(0,600,0);

            //increment current level by 1
            LevelManager.CurrentLevel ++;

            //fade in hand slots
            GameManager.HandSlotsFadeIn();
            yield return new WaitForSeconds(1.5f);
            
            if(LevelManager.CurrentLevel == 3)
                {
                    Event1Complete = true;
                }
                else if(LevelManager.CurrentLevel == 6)
                {
                    Event2Complete = true;
                }
                else if(LevelManager.CurrentLevel == 9)
                {
                    Event3Complete = true;
                }
                else if(LevelManager.CurrentLevel == 12)
                {
                    Event4Complete = true;
                }

            //change game state to beginning of main battle loop
            GameManager.StateChangetoBattleSetup(); 
        }

        else if (CurrentEvent == 4)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic.");
            InstantiateEventRewards_Bash();
            yield return new WaitForSeconds(1.5f);

            //move transition screen over current view
            GameManager.MoveTransitionPanelIntoView();
            yield return new WaitForSeconds(1.5f);

            // move event UI out of screen
            GameManager.EventScreen.transform.position = new Vector3(0,600,0);

            //increment current level by 1
            LevelManager.CurrentLevel ++;

            //fade in hand slots
            GameManager.HandSlotsFadeIn();
            yield return new WaitForSeconds(1.5f);
            
            if(LevelManager.CurrentLevel == 3)
                {
                    Event1Complete = true;
                }
                else if(LevelManager.CurrentLevel == 6)
                {
                    Event2Complete = true;
                }
                else if(LevelManager.CurrentLevel == 9)
                {
                    Event3Complete = true;
                }
                else if(LevelManager.CurrentLevel == 12)
                {
                    Event4Complete = true;
                }

            //change game state to beginning of main battle loop
            GameManager.StateChangetoBattleSetup(); 
        }

        else if (CurrentEvent == 5)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic.");

            Player.CurrentHealth = Player.MaxHealth;
            //move transition screen over current view
            GameManager.MoveTransitionPanelIntoView();
            yield return new WaitForSeconds(1.5f);

            // move event UI out of screen
            GameManager.EventScreen.transform.position = new Vector3(0,600,0);

            //increment current level by 1
            LevelManager.CurrentLevel ++;

            //fade in hand slots
            GameManager.HandSlotsFadeIn();
            yield return new WaitForSeconds(1.5f);
            
            if(LevelManager.CurrentLevel == 3)
                {
                    Event1Complete = true;
                }
                else if(LevelManager.CurrentLevel == 6)
                {
                    Event2Complete = true;
                }
                else if(LevelManager.CurrentLevel == 9)
                {
                    Event3Complete = true;
                }
                else if(LevelManager.CurrentLevel == 12)
                {
                    Event4Complete = true;
                }

            //change game state to beginning of main battle loop
            GameManager.StateChangetoBattleSetup(); 
        }

        else if (CurrentEvent == 6)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic.");
            GameManager.MoveEvent1OutOfView();
            yield return new WaitForSeconds(.75f);

            //Move to purge state
            GameManager.StateChangeToPurgeState();

            if(LevelManager.CurrentLevel == 3)
                {
                    Event1Complete = true;
                }
                else if(LevelManager.CurrentLevel == 6)
                {
                    Event2Complete = true;
                }
                else if(LevelManager.CurrentLevel == 9)
                {
                    Event3Complete = true;
                }
                else if(LevelManager.CurrentLevel == 12)
                {
                    Event4Complete = true;
                }
        }   
    }

////////////////////////////////////////////////////////////////////////////
//accept function 
    public void EventAcceptFunction()
    {
        StartCoroutine(EventAcceptCoroutine());
    }

    IEnumerator EventAcceptCoroutine()
    {
        if(CurrentEvent == 1)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic. Accept function");
            //heal player health to full
            Player.CurrentHealth = Player.MaxHealth;
        }
        else if (CurrentEvent == 2)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic. Accept function");
            //heal player health to full
            InstantiateEventRewards_Enhance();
        }
        else if (CurrentEvent == 3)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic. Accept function");
            //heal player health to full
            InstantiateEventRewards_Defense();
        }
        else if (CurrentEvent == 4)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic. Accept function");
            //heal player health to full
            InstantiateEventRewards_Cleave();
        }
        else if (CurrentEvent == 5)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic. Accept function");
            //heal player health to full
            InstantiateEventRewards_Heal();
        }
        else if (CurrentEvent == 6)
        {
            Debug.Log(CurrentEvent + " - Moved into EventLogicHandler execution logic. Accept function");
            //heal player health to full
            InstantiateEventRewards_Defense();
        }

        yield return new WaitForSeconds(1.5f);
        //move transition screen over current view

        GameManager.MoveTransitionPanelIntoView();
        yield return new WaitForSeconds(1.5f);

        // move event UI out of screen
        GameManager.EventScreen.transform.position = new Vector3(0,600,0);

        //increment current level by 1
        LevelManager.CurrentLevel ++;

        //fade in hand slots
        GameManager.HandSlotsFadeIn();
        yield return new WaitForSeconds(1.5f);
        
        //change game state to beginning of main battle loop
        GameManager.StateChangetoBattleSetup(); 
    }


    ///////////////////////////////////////////////

    public void InstantiateEventRewards_Defense()
    {
        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);
        // Instantiate the prefab
        GameObject dice = Instantiate(_prefab_Defense, spawnPosition, Quaternion.identity);

        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Poison", "Landmark","Ephemeral", "Weighted","Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = "Epic";
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        KeepReward();
    }
    public void InstantiateEventRewards_Heal()
    {
        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);
        // Instantiate the prefab
        GameObject dice = Instantiate(_prefab_Heal, spawnPosition, Quaternion.identity);

        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Glass", "Glass", "Glass", "Weighted"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = "Mythic";
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        KeepReward();
    }
    public void InstantiateEventRewards_Cleave()
    {
        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);
        // Instantiate the prefab
        GameObject dice = Instantiate(_prefab_Cleave, spawnPosition, Quaternion.identity);

        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Poison", "Landmark","Ephemeral", "Weighted","Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = "Epic";
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        KeepReward();
    }
    public void InstantiateEventRewards_Bash()
    {
        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);
        // Instantiate the prefab
        GameObject dice = Instantiate(_prefab_Bash, spawnPosition, Quaternion.identity);

        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Poison", "Landmark","Ephemeral", "Weighted","Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = "Epic";
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        KeepReward();
    }
    public void InstantiateEventRewards_Enhance()
    {
        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);
        // Instantiate the prefab
        GameObject dice = Instantiate(_prefab_Enhance, spawnPosition, Quaternion.identity);

        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Poison", "Landmark","Ephemeral", "Weighted"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = "Epic";
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        KeepReward();
    }
    public void InstantiateEventRewards_Slam()
    {
        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);
        // Instantiate the prefab
        GameObject dice = Instantiate(_prefab_Slam, spawnPosition, Quaternion.identity);

        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Poison", "Landmark","Ephemeral", "Weighted"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = "Epic";
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        KeepReward();
    }

    public void KeepReward()
    {
        var findDice = GameObject.FindGameObjectsWithTag("Dice");

        foreach (GameObject dice in findDice)
        {
            var diceVars = dice.GetComponent<DiceScript>();
            if (diceVars.NewlySpawnedDice == true)
            {
                DiceBagScript.diceBag.Add(dice);
                DiceBagScript.diceBagCount = DiceBagScript.diceBag.Count;

                diceVars.NewlySpawnedDice = false;
            }
        }
        //StartCoroutine(KeepRewardCoroutine());
    }

    // IEnumerator KeepRewardCoroutine()
    // {
    //     var findDice = GameObject.FindGameObjectsWithTag("Dice");

    //     foreach (GameObject dice in findDice)
    //     {
    //         var diceVars = dice.GetComponent<DiceScript>();
    //         if (diceVars.NewlySpawnedDice == true)
    //         {
    //             DiceBagScript.diceBag.Add(dice);
    //             DiceBagScript.diceBagCount = DiceBagScript.diceBag.Count;

    //             diceVars.NewlySpawnedDice = false;
    //         }
    //     }

    //     yield return new WaitForSeconds(.5f); 
    // }
}
