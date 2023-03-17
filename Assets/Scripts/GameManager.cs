using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;




public class GameManager : MonoBehaviour
{
    LevelManager LevelManager;
    DiceBagScript DiceBagScript;
    Drag Drag;
    Player Player;
    RewardReroll RewardReroll;
    RelicScript RelicScript;
    EventLogicHandler EventLogicHandler;
    SoundManager SoundManager;
    

    [Header("Game State Parameters")]
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    [Header("Event Collections")]
    public GameObject EventScreen; //heal to full 


    [Header("Raycast Blocker")]
    public GameObject RaycastBlocker;

    [Header("DiceBoard Object for Animations between states")]
    public GameObject DiceBoardSprite;
    public GameObject DiceBoardButton;

    [Header("Hand Pos Sprite Object for Animations")]
    public GameObject HandSlotsSprite;

    [Header("Reward Options")]
    public GameObject PurgeOption;
    public GameObject PurgeText;
    public GameObject UpgradeOption;
    public GameObject UpgradeText;

    [Header("MISC - variables for attack coroutine test")]
    public float speed = .2f;
    public Vector3 targetPosition = new Vector3(0,12,0);
    public Vector3 originPosition = new Vector3(0,.08f,0);

    [Header("MISC - Transition Panel")]
    public GameObject TransitionPanel;

    [Header("MISC - Rewards Generator Game Object")]
    public GameObject RewardsGeneratorObject;

    [Header("MISC - Rewards Generator Reroll Object")]
    public GameObject RewardsGeneratorReroll;

    [Header("MISC - Artifact 1 Object")]
    public GameObject Artifact1;
    [Header("MISC - Artifact 2 Object")]
    public GameObject Artifact2;
    [Header("MISC - Artifact 3 Object")]
    public GameObject Artifact3;
    [Header("MISC - Artifact 4 Object")]
    public GameObject Artifact4;

    private void Awake() 
    {
        Instance = this;      
    }

    // On start up, this creates a list of all game objects in the scene tagged "Dice"
    private void Start() 
    {
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        Drag[] Drag = GameObject.FindObjectsOfType<Drag>();
        Player = GameObject.FindObjectOfType<Player>();
        RewardReroll = GameObject.FindObjectOfType<RewardReroll>();
        RelicScript = GameObject.FindObjectOfType<RelicScript>();
        EventLogicHandler = GameObject.FindObjectOfType<EventLogicHandler>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    
        // RewardsGenerator = GameObject.FindObjectsOfType<RewardsGenerator>();
        //When this starts, the gamestate will be the TitleScreen. in the full build
        UpdateGameState(GameState.CryptNavigation);
    }

     private void Update()
     {
         if (State.ToString() == "Victory")
         {
                RewardsGeneratorReroll.SetActive(true);
                Debug.Log("Game Manager just enabled the reroll button");
         }
     }
    
    public string GetStateString()
    {
        return State.ToString();
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState){
            case GameState.TitleScreen:
                break;

            case GameState.LoadPlayerData:
            //Load any player preferences or custom dicebags / relics / items.
                break;

            case GameState.CryptNavigation:
            //The player can select the next stage to move towards. This is for ver 2.0 // this should determine the level data (monsters / loot)
                StartCoroutine(GameSetupCookTime());
                break;

            case GameState.BattleSetup:
                
            //This state should be used for any initialization of modules for battle to work. Ideally, anything we can't do in runtime.
            // should spawn in UI.
            // do calculations that determine what enemies will be spawning for this battle.
            // ver 1.0
            // if levlemanager current level = 4/7/9/12 go to a state to trigger the event.
                Debug.Log("Progressing from BattleSetUp Phase to Spawn enemies phase.");
                RaycastBlocker.SetActive(true);
                DiceBoardMoveOutOfView(); /// this is new -- pending removal
                UpdateGameState(GameState.SpawnPlayerAndEnemies);
                RewardReroll.RefreshRerollPanel();
                break;

            case GameState.SpawnPlayerAndEnemies:
                LevelManager.SpawnEnemies();
                //DiceBoardFadeIn(); // new pending removal
                Debug.Log("Enemy Spawned, moving to roll/deal dice.");
                UpdateGameState(GameState.EnemyActions);
            //Spawn enemies & player sprites.
                break;

            case GameState.EnemyActions:
                Debug.Log("EnemyActions State");
                RaycastBlocker.SetActive(true);
                StartCoroutine(DetermineEnemyActions());
                MoveTransitionPanelOutOfView();
                // UpdateGameState(GameState.SpawnDiceAndRollDice);
            //spawn up to 5 random dice from dicebag. We must have checks for full handslots, so that those are not rerolled. 
                break;

            case GameState.TakePoisonDamage:
                break;

            case GameState.SpawnDiceAndRollDice:
                Debug.Log("SpawnDiceAndRollDice State");
                //DiceBoardFadeIn();
                DiceBoardMoveIntoView(); // new pending removal
                StartCoroutine(DealDice());
                UpdateGameState(GameState.PlayerActions);
            //the enemy will determine the attacks and targets to use.
                break;

            case GameState.PlayerActions:
                Debug.Log("PlayerActions State");
                DiceBagScript.EnableRerollButton();
                //RelicScript.SelectRandomGrid(RelicScript.AdjacentGrids);
            //the player will now be able to click and drag dice, interact with UI, and view hovertext. Move to damage state once the player selects Finish.
            //the finish button is what pushes us to the player damage state.
                break;

            case GameState.PlayerDamage:
                Debug.Log("PlayerDamage State");
                RaycastBlocker.SetActive(true);
                // DiceBoardFadeOut();
            //we will calculate the damage the player does to the enemy, and then the enemy's damage on the player.
                break;

            case GameState.EnemyDamage:
                Debug.Log("EnemyDamage State");
            //Enemy damage and effects are applied.
                RaycastBlocker.SetActive(true);
                StartCoroutine(EnemyAttacks());
                // UpdateGameState(GameState.ResetVariables);
                break;

            case GameState.ResetVariables:
                Debug.Log("ResetVariables State");
            //Reset Enemy Parameters
                StartCoroutine(ResetEnemyVariables());
                // THIS ROLLS OVER TO - UpdateGameState(GameState.EnemyActions);
                //UpdateGameState(GameState.BattleEnd);
                break;

            case GameState.BattleEnd:
                Debug.Log("BattleEnd State");
                StartCoroutine(TakePoisonDamage());
                // all end turn logic moved under the take poison damage function


            //battle concludes when the player or enemy dies. We then do any garbage collection and staging to return the scene to normal before the next level is generated.
            // use this state to doublecheck everything is reset and accurate. Then we change the level to +1 on the levelManager and move towards the rewards screen
                break;

            case GameState.Rewards:
                Debug.Log("Rewards State");
                //disable hand position sprites
                    HandSlotsFadeOut();
                //return ALL dice to dicebag and reset 
                    DiceBagScript.ReturnUsedDice_LevelEnd();
                //move purge/upgrade dice option into view
                    MoveUpgradeRewardsIntoView();
                    MovePurgeRewardsIntoView();
                // the rewards screen will display the reward choices. purge dice, upgrade dice, get new dice
                // each one of these options takes you to a different state. make 3 buttons for each option, each button updates the game state appropriately
                // all upgrade/purge/earn new dice should only happen once the state has been changed 
                break;

            case GameState.PurgeDice:
                Debug.Log("Purge Dice State");
                //make dicebag inventory active 
                    DiceBagScript.DiceBagInventory.enabled = true;
                    PurgeText.SetActive(true);
                // this will allow the user to delete a game object on left mouse click
                break;

            case GameState.UpgradeDice:
                Debug.Log("Upgrade Dice State");
                //make dicebag inventory active 
                    DiceBagScript.DiceBagInventory.enabled = true;
                    UpgradeText.SetActive(true);
                // during this state, users can double click to upgrade a single dice the state is then changed to victory
                break;

            case GameState.ReceiveNewDice:
                Debug.Log("Receive New Dice State");
                PurgeText.SetActive(false);
                UpgradeText.SetActive(false);
                // even current level is even, move to victory screen else reward a new dice
                // if (LevelManager.CurrentLevel % 2 == 0 )
                // {
                //     UpdateGameState(GameState.Victory);
                // }
                if(LevelManager.CurrentLevel == 3 || LevelManager.CurrentLevel == 5 || LevelManager.CurrentLevel == 7 || LevelManager.CurrentLevel == 9 || LevelManager.CurrentLevel == 12 || LevelManager.CurrentLevel == 15 || LevelManager.CurrentLevel == 16 || LevelManager.CurrentLevel == 17 || LevelManager.CurrentLevel == 18 || LevelManager.CurrentLevel == 19 || LevelManager.CurrentLevel == 20)
                {
                    RewardsGenerator RewardsGenerator = RewardsGeneratorObject.GetComponent<RewardsGenerator>();
                    RewardsGenerator.RandomRewardsRoutine_Rare();
                }
                else
                {
                    RewardsGenerator RewardsGenerator = RewardsGeneratorObject.GetComponent<RewardsGenerator>();
                    RewardsGenerator.RandomRewardsRoutine();
                }
                break;

            case GameState.RewardChoices:
                Debug.Log("Reward Choices State");

                DiceBagScript.DiceBagInventory.enabled = true;
                SoundManager.Instance.PlayLoudSound(DiceBagScript._diceBagOpenClip);
                //use this to allow users to select the buttons to keep or reroll. once complete, forward to victory state.
                break;

            case GameState.Purgatory:
            Debug.Log("Purgatory State");
                RefreshScenePrepareForNextLevel();
            // place a coroutine in here to update level manager currentlevel ++ event_2_st
            // change state to BattleSetup state
                break;

            case GameState.Victory:
            //update level data // reset everything and then refresh the scene from the beginning for the next encounter 

            DiceBagScript.DiceBagInventory.enabled = false;

            MoveTransitionPanelIntoView();
            UpdateGameState(GameState.Purgatory); 
                break;

            case GameState.event_1_start:
            //determine random value 
                int RandomEvent1 = UnityEngine.Random.Range(1, 7);
            //trigger event data 
                EventLogicHandler.EventFocusFunction(RandomEvent1);
                break;

            case GameState.event_2_start:
            //determine random value 
                int RandomEvent2 = UnityEngine.Random.Range(1, 7);
            //trigger event data 
                EventLogicHandler.EventFocusFunction(RandomEvent2);
                break;

            case GameState.event_3_start:
            //determine random value 
                int RandomEvent3 = UnityEngine.Random.Range(1, 7);
            //trigger event data 
                EventLogicHandler.EventFocusFunction(RandomEvent3);
                break;

            case GameState.event_4_start:
            //determine random value 
                int RandomEvent4 = UnityEngine.Random.Range(1, 7);
            //trigger event data 
                EventLogicHandler.EventFocusFunction(RandomEvent4);
                break;

            case GameState.event_5_start:
            //trigger event data 
                break;

            case GameState.artifacteventstart:
            //trigger event data 
                break;

            case GameState.Defeat:
                Debug.Log("Defeat State");
                Debug.Log("You have Died.");
                // change scene to game over screen 
                // make the scene manager line part of a couroutine with a delay to support a death animation.
                StartCoroutine(GameOverCoroutine());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    public enum GameState
    {
        TitleScreen,
        LoadPlayerData,
        CryptNavigation,
        BattleSetup,
        SpawnPlayerAndEnemies,
        EnemyActions,
        TakePoisonDamage,
        SpawnDiceAndRollDice,
        PlayerActions, 
        PlayerDamage,
        EnemyDamage,
        ResetVariables,
        BattleEnd,
        Rewards,
        PurgeDice,
        UpgradeDice,
        ReceiveNewDice,
        RewardChoices,
        Purgatory,
        Victory,
        Defeat,
        event_1_start, 
        event_2_start, 
        event_3_start, 
        event_4_start, 
        event_5_start, 
        artifacteventstart,  
    }

    public void StateChangetoBattleSetup()
    {
        UpdateGameState(GameState.BattleSetup);
    }

    public void StateChangetoReceiveNewDice()
    {
        UpdateGameState(GameState.ReceiveNewDice);
    }

    public void StateChangeToPurgeState()
    {
        UpdateGameState(GameState.PurgeDice);
    }
    public void StateChangeToUpgradeState()
    {
        UpdateGameState(GameState.UpgradeDice);
    }
    public void StateChangeButtonClick()
    {
        UpdateGameState(GameState.EnemyDamage);
    }

    public void StateChangeToBattleEnd()
    {
        UpdateGameState(GameState.BattleEnd);
    }

    public void StateChangeDefeat()
    {
        UpdateGameState(GameState.Defeat);
    }

    public void StateChangeToVictory()
    {
        UpdateGameState(GameState.Victory);
    }

    public void StateChangeToRewards()
    {
        UpdateGameState(GameState.Rewards);
    }

    public void HandSlotsFadeIn()
    {
        StartCoroutine(FadeInHandSlots());
    }

    public void HandSlotsFadeOut()
    {
        StartCoroutine(FadeOutHandSlots());   
    }
    public void DiceBoardFadeIn()
    {
       StartCoroutine(FadeIn());
    }

    public void DiceBoardFadeOut()
    {
        StartCoroutine(FadeOut());  
    }

    public void DiceBoardMoveIntoView()
    {
        StartCoroutine(MoveIntoView(DiceBoardSprite, new Vector3( -.33f, -.4f, 0f), 1f));  //.39
    }

    public void DiceBoardMoveOutOfView()
    {
        StartCoroutine(MoveOutOfView(DiceBoardSprite, new Vector3(-.33f, 15f, 0f), 1f)); 
    }

    IEnumerator DetermineEnemyActions()
    {
        var findEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in findEnemy)
        {
            if(enemy != null)
            {
                enemy.GetComponent<EnemyAI>().DetermineAmountOfActions();
                yield return new WaitForSeconds(.25f);
                enemy.GetComponent<EnemyAI>().DetermineActions();
                yield return new WaitForSeconds(.25f);   
            }

        }

        DiceBagScript.RollAndDealDice();
        UpdateGameState(GameState.SpawnDiceAndRollDice);
    }

    public void TakePoisonDamageFunction()
    {
        StartCoroutine(TakePoisonDamage());
    } 


    IEnumerator TakePoisonDamage()
    {
        var findEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in findEnemy)
        {
            if(enemy != null)
            {
                enemy.GetComponent<EnemyAI>().TakePoisonDamage();
            }
        }
        yield return new WaitForSeconds(1f); 
        if (LevelManager.AliveEnemies <= 0)
            {
                UpdateGameState(GameState.Rewards);
            }
        else
            {
                UpdateGameState(GameState.EnemyActions);
            }
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        MoveTransitionPanelIntoView();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameOver");

    }

    IEnumerator DealDice()
    {
        yield return new WaitForSeconds(.5f);
        DiceBagScript.RollAndDealDice();
        //This is the timer waited before we're able to interact with the rolled dice
        yield return new WaitForSeconds(1f);
        RaycastBlocker.SetActive(false);
    }

    
    IEnumerator EnemyAttacks()
    {
        yield return new WaitForSeconds(1f);
        var findEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        var activeEnemies = findEnemy.Where(e => e != null && e.activeInHierarchy);
        foreach (GameObject enemy in activeEnemies)
        {
            enemy.GetComponent<EnemyAI>().ExecuteAction1();
            yield return new WaitForSeconds(.5f);

            if (enemy != null && enemy.activeInHierarchy) // Check if the GameObject is still active
            {
                enemy.GetComponent<EnemyAI>().ExecuteAction2();
                yield return new WaitForSeconds(.5f);

                if (enemy != null && enemy.activeInHierarchy) // Check if the GameObject is still active
                {
                    enemy.GetComponent<EnemyAI>().ExecuteAction3();
                    yield return new WaitForSeconds(.5f);
                }
            }
        }
        UpdateGameState(GameState.ResetVariables);
    }

    IEnumerator ResetEnemyVariables()
    {
        var findEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in findEnemy)
        {
            yield return new WaitForSeconds(.1f);
            if(enemy != null)
            {
                enemy.GetComponent<EnemyAI>().ResetEnemyVariables();  
            }
        }
        UpdateGameState(GameState.BattleEnd);
        
    }

    IEnumerator FadeOut()
    {
        // Get the current color of the sprite renderer
        Color color = DiceBoardSprite.GetComponent<SpriteRenderer>().color;

        // Interpolate the alpha value over 1 second
        for (float t = 0.5f; t < 1f; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1, 0, t);
            DiceBoardSprite.GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        // Get the current color of the sprite renderer
        Color color = DiceBoardSprite.GetComponent<SpriteRenderer>().color;

        // Interpolate the alpha value over 1 second
        for (float t = 0.3f; t < 1f; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t);
            DiceBoardSprite.GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }


    IEnumerator FadeOutHandSlots()
    {
        // Get the current color of the sprite renderer
        Color color = HandSlotsSprite.GetComponent<SpriteRenderer>().color;

        // Interpolate the alpha value over 1 second
        for (float t = 0.5f; t < 1f; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1, 0, t);
            HandSlotsSprite.GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }

    IEnumerator FadeInHandSlots()
    {
        // Get the current color of the sprite renderer
        Color color = HandSlotsSprite.GetComponent<SpriteRenderer>().color;

        // Interpolate the alpha value over 1 second
        for (float t = 0.3f; t < 1f; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t);
            HandSlotsSprite.GetComponent<SpriteRenderer>().color = color;
            yield return null;
        }
    }

    public void MoveEvent1OutOfView()
    {
        StartCoroutine(MoveOutOfView(EventScreen, new Vector3(0f, 60f, 0f), 3f)); 
    }


    public void MoveTransitionPanelIntoView()
    {
        StartCoroutine(MoveIntoView(TransitionPanel, new Vector3(0f, 0f, 0f), 3f)); 
    }
    public void MoveTransitionPanelOutOfView()
    {
        StartCoroutine(MoveOutOfView(TransitionPanel, new Vector3(27f, 0f, 0f), 3f)); 
    }

    public void MovePurgeRewardsIntoView()
    {
        StartCoroutine(MoveIntoView(PurgeOption, new Vector3(5f, 0f, -6f), 1.5f));
    }

    public void MoveUpgradeRewardsIntoView()
    {
        StartCoroutine(MoveIntoView(UpgradeOption, new Vector3(.25f, 0f, -6f), 1.5f));
    }

    public void MovePurgeRewardsOutOfView()
    {
        StartCoroutine(MoveOutOfView(PurgeOption, new Vector3(17.36f, 0f, 0f), .5f));
    }

    public void MoveUpgradeRewardsOutOfView()
    {
        StartCoroutine(MoveOutOfView(UpgradeOption, new Vector3(12.61f, 0f, 0f), .5f));
    }

    IEnumerator MoveIntoView(GameObject objectToMove, Vector3 endPosition, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator MoveOutOfView(GameObject objectToMove, Vector3 endPosition, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void RefreshScenePrepareForNextLevel()
    {
        StartCoroutine(PurgatoryDelay());
    }


    IEnumerator PurgatoryDelay()
    {
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeInHandSlots());
            var findHands = GameObject.FindGameObjectsWithTag("Hand");
            foreach (GameObject hand in findHands)
            {
                yield return new WaitForSeconds(.25f);
                hand.GetComponent<HandPositionScript>().OpenDiceSlot = true;
            }
            Player.DefensePoints = 0;


            if(LevelManager.CurrentLevel == 3 && EventLogicHandler.Event1Complete == false)
            {
                UpdateGameState(GameState.event_1_start);
                yield return new WaitForSeconds(.5f);
            }
            else if(LevelManager.CurrentLevel == 6 && EventLogicHandler.Event2Complete == false)
            {
                 UpdateGameState(GameState.event_2_start);
                 yield return new WaitForSeconds(.5f);
            }
            else if(LevelManager.CurrentLevel == 9 && EventLogicHandler.Event3Complete == false)
            {
                 UpdateGameState(GameState.event_3_start);
                 yield return new WaitForSeconds(.5f);
            }
            else if(LevelManager.CurrentLevel == 12 && EventLogicHandler.Event4Complete == false)
            {
                 UpdateGameState(GameState.event_4_start);
                 yield return new WaitForSeconds(.5f);
            }
            else
            {
                LevelManager.CurrentLevel ++;
                yield return new WaitForSeconds(.5f);
                UpdateGameState(GameState.BattleSetup);  
            }
            
            // if current level == 3, 8, 13,18,23 then go to to eventstart
            // else if current level == 5,10,15,20 then go to artifacteventstart
            //else current level++, wait .5 seconds, then update game state to BattleSetup 

            //the code below should be tied to the artifact event?

        }
    }

    IEnumerator GameSetupCookTime()
    {
        Debug.Log("Artificial cook time to prevent problems with initialization");
       yield return new WaitForSeconds(2.5f); 
       UpdateGameState(GameState.BattleSetup);
    }

}
