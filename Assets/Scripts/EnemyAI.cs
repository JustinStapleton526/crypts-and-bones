using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    Player Player;
    LevelManager LevelManager;
    GameManager GameManager;
    MouseController mc;
    Animator Animator;
    DiceBagScript DiceBagScript;
    GridPositionScript GridPositionScript;
    DiceHoverTextManager DiceHoverTextManager;
    RelicRewardManager RelicRewardManager;
    SoundManager SoundManager;

    [Header("Enemy AI - Available Actions")]
    public bool BossMonster = false;
    public bool CanHeal;
    public bool CanBlockGrid;
    public int BlockedGrids = 0;

    [Header("Enemy AI - Readied Actions")]
    public int AttackReady = 0;
    public int DefendReady = 0;
    public int HealReady = 0;
    public int GridBlockReady = 0;

    [Header("Enemy AI - Action Preview")]
    public SpriteRenderer SpriteRenderer_Slot1;
    public SpriteRenderer SpriteRenderer_Slot2;
    public SpriteRenderer SpriteRenderer_Slot3;
    public Sprite AttackSprite;
    public Sprite DefendSprite;
    public Sprite HealSprite;
    public Sprite XSprite;
    public Sprite FocusSprite;
    public Sprite SunderSprite;
    public Sprite AoeHealSprite;
    public Sprite AoeDefendSprite;
    public Sprite NemesisDiceSprite;
    public Sprite LifestealSprite;

    //This code determines how many actions the unit will make during its activation
    [Header("Enemy AI - Chance of multiple attacks")]
    public int[] EnemyActionDice = {1, 2, 3, 4, 5, 6};
    public int EnemyActionCount = 0;

    //The value rolled on this dice determines which move it will use for its activation; 1 = attack 2= defend 3= heal 4 = rend 5= cleave 6= block grid
    [Header("Enemy AI - Random Action Selector")]
    public int[] EnemyAbilityDice = {1, 2, 3, 4, 5, 6};
    public int RandomAction1 = 0;
    public int RandomAction2 = 0;
    public int RandomAction3 = 0;

    [Header("Enemy AI - Enemy Stats")]
    public bool EnemyLeader = false;
    public int MaxHealth;
    public int CurrentHealth;
    public int CurrentDefense;
    public int AttackDamage;
    public int HealAmount;
    public int DefendAmount;
    public bool Focused = false;
    public int PoisonTickDamage = 0;

    [Header("Enemy AI - Targetted Enemy")]
    public bool selected;
    public GameObject TargetSprite;
    public GameObject FloatingText;

    [Header("Enemy AI - Animation properties")]
    public GameObject EnemyGameObject;
    public Vector3 CurrentLocation;

    [Header("Particle Effects")]
    public GameObject HealthParticle;

    [Header("Sound Effects")]
    public bool _isSoundPlaying = false;
    [SerializeField] private AudioClip _takeDamageClip;
    [SerializeField] private AudioClip _defendClip;
    [SerializeField] private AudioClip _healClip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _HoverTextClip;


    [Header("Reactions 1")]
    public bool ReactionEnabled = false; // trigger for all reactions in slot 1 
    
    public bool StoneSkin = false; //limit damage over 6 to 6
    public bool Martyr = false; // on death heal allies w/ heal amount
    public bool Resiliant = false; //on death add 5 armor 
    public bool Trickster = false; // taking 6+ damage spawns nemesis die
    public bool Deadlock = false; // taking 6+ damage spawns blocked grid
    public bool ShieldWall = false; // taking 6+ damage adds 1 block to everyone
    public bool BloodLust = false; //taking 6+ damage causes focused
    public bool Luminous = false; // on death give player +1 energy
    public bool Revenge = false; // on death deal 2 damage to player
    public bool Fortified = false; //when damage is taken, apply 2 defense 
    [Header("Reactions 2")]
    public bool ReactionEnabled2 = false; // trigger for all reactions in slot 2
     public bool StoneSkin2 = false; //limit damage over 6 to 6
    public bool Martyr2 = false; // on death heal allies w/ heal amount
    public bool Resiliant2 = false; //on death add 5 armor 
    public bool Trickster2 = false; // taking 6+ damage spawns nemesis die
    public bool Deadlock2 = false; // taking 6+ damage spawns blocked grid
    public bool ShieldWall2 = false; // taking 6+ damage adds 1 block to everyone
    public bool BloodLust2 = false; //taking 6+ damage causes focused
    public bool Luminous2 = false; // on death give player +1 energy
    public bool Revenge2 = false; // on death deal 2 damage to player
    public bool Fortified2 = false; //when damage is taken, apply 2 defense

    public void Start() 
    {
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        Player = GameObject.FindObjectOfType<Player>();
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        mc = FindObjectOfType<MouseController>();
        GridPositionScript = GameObject.FindObjectOfType<GridPositionScript>();
        RelicRewardManager = GameObject.FindObjectOfType<RelicRewardManager>();
        SoundManager =  GameObject.FindObjectOfType<SoundManager>();
        //This defaults to a selected unit at the start of the game so that it doesn't get missed.
        if (mc.selectedUnit != this)
        {
            mc.selectedUnit = this;
        }
        
        CurrentHealth = MaxHealth;
        CurrentLocation = gameObject.transform.position;
        Animator = GetComponent<Animator>();
        Animator.SetBool("DeathAnim", false);
    }

    private void Update() 
    {
       if (mc.selectedUnit == this)
       {
            TargetSprite.GetComponent<SpriteRenderer>().enabled = true;
       } 
       else
       {
            TargetSprite.GetComponent<SpriteRenderer>().enabled = false;
       }

       if (mc.selectedUnit == null)
                {
                    mc.selectedUnit = this;
                }


        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        if(CurrentDefense >= 10)
        {
            CurrentDefense = 10;
        }


    }

    public void OnMouseOver() 
    {
        if (!_isSoundPlaying)
            {
                SoundManager.Instance.PlayLoudSound(_HoverTextClip);
                _isSoundPlaying = true;
            }

        DiceHoverTextManager.EnemyPanelText.text =
                    
                    "Damage: " + AttackDamage +
                    "\n" + "Defense: " + DefendAmount;
                    
        DiceHoverTextManager.HoverEnemyPanelLocation = mc.GetFocusedOnTile().Value.collider.gameObject.transform.position;
        DiceHoverTextManager.HoverEnemyPanel.SetActive(true); 
        DiceHoverTextManager.HoverEnemyPanel.transform.position = new Vector2(DiceHoverTextManager.HoverEnemyPanelLocation.x-1.5f, DiceHoverTextManager.HoverEnemyPanelLocation.y-.50f);

        GameObject[] HoverTextEnemySides = DiceHoverTextManager.EnemySides;

        for (int i = 0; i < EnemyAbilityDice.Length; i++)
        {
            if (EnemyAbilityDice[i] == 1)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = AttackSprite;
            }
            else if (EnemyAbilityDice[i] == 2)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = DefendSprite;
            }
            else if (EnemyAbilityDice[i] == 3)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = HealSprite;
            }
            else if (EnemyAbilityDice[i] == 4)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = AoeDefendSprite;
            }
            else if (EnemyAbilityDice[i] == 5)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = FocusSprite;
            }
            else if (EnemyAbilityDice[i] == 6)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = XSprite;
            }
            else if (EnemyAbilityDice[i] == 7)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = NemesisDiceSprite;
            }
            else if (EnemyAbilityDice[i] == 8)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = AoeHealSprite;
            }
            else if (EnemyAbilityDice[i] == 9)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = SunderSprite;
            }
            else if (EnemyAbilityDice[i] == 10)
            {
                HoverTextEnemySides[i].GetComponent<SpriteRenderer>().sprite = LifestealSprite;
            }
        }

        if(PoisonTickDamage >= 1)
        {
            DiceHoverTextManager.PoisonIcon.SetActive(true);
            DiceHoverTextManager.PoisonText.text = PoisonTickDamage.ToString();
        }
        else
        {
            DiceHoverTextManager.PoisonIcon.SetActive(false); 
        }


        if(ReactionEnabled == true)
        {
            DiceHoverTextManager.ReactionTextPanel1.transform.position = new Vector2(DiceHoverTextManager.HoverEnemyPanelLocation.x-4f, DiceHoverTextManager.HoverEnemyPanelLocation.y+1.15f);
            DiceHoverTextManager.ReactionTextPanel1.SetActive(true); 

            if(StoneSkin == true)
            {
                DiceHoverTextManager.ReactionText1.text =  
                "<color=#C51B69>Stone Skin</color> \nLimit damage taken to a maximum of 6."; 
            }
            if(Trickster == true)
            {
                DiceHoverTextManager.ReactionText1.text =  
                "<color=#C51B69>Trickster</color> \nTaking 6+ damage creates a Nemesis Die."; 
            }
            if(Deadlock == true)
            {
                DiceHoverTextManager.ReactionText1.text =  
                "<color=#C51B69>Deadlock</color> \nTaking 6+ damage blocks a random grid."; 
            }
            if(ShieldWall == true)
            {
                DiceHoverTextManager.ReactionText1.text =  
                "<color=#C51B69>Shield Wall</color> \nTaking 6+ damage immediately generates "+DefendAmount+" defense for all enemies."; 
            }
            if(BloodLust == true)
            {
                DiceHoverTextManager.ReactionText1.text =   
                "<color=#C51B69>Blood Lust</color> \nTaking 6+ damage will double the damage of the enemy's next attack."; 
            }
            if(Fortified == true)
            {
                DiceHoverTextManager.ReactionText1.text =    
                "<color=#C51B69>Fortified</color> \nEach time damage is taken, subtract "+DefendAmount+"."; 
            }
            if(Revenge == true)
            {
                DiceHoverTextManager.ReactionText1.text =   
                "<color=#C51B69>Revenge</color> \nOn death, deal 2 damage to the player."; 
            }
            if(Luminous == true)
            {
                DiceHoverTextManager.ReactionText1.text =   
                "<color=#C51B69>Luminous</color> \nOn death, grant the player an additional energy next turn."; 
            }
            if(Martyr == true)
            {
                DiceHoverTextManager.ReactionText1.text =    
                "<color=#C51B69>Martyr</color> \nOn death, heal all allies."; 
            }
            if(Resiliant == true)
            {
                DiceHoverTextManager.ReactionText1.text =   
                "<color=#C51B69>Resiliant</color> \nOn death, grant all allies defense."; 
            }
                
        }
        if(ReactionEnabled2 == true)
        {
            DiceHoverTextManager.ReactionTextPanel2.transform.position = new Vector2(DiceHoverTextManager.HoverEnemyPanelLocation.x-4f, DiceHoverTextManager.HoverEnemyPanelLocation.y+3f);
            DiceHoverTextManager.ReactionTextPanel2.SetActive(true);

            if(StoneSkin2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Stone Skin</color> \nLimit damage taken to a maximum of 6."; 
            }
            if(Trickster2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Trickster</color> \nTaking 6+ damage creates a Nemesis Die."; 
            }
            if(Deadlock2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Deadlock</color> \nTaking 6+ damage blocks a random grid."; 
            }
            if(ShieldWall2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Shield Wall</color> \nTaking 6+ damage immediately generates "+DefendAmount+" defense for all enemies."; 
            }
            if(BloodLust2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Blood Lust</color> \nTaking 6+ damage will double the damage of the enemy's next attack."; 
            }
            if(Fortified2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Fortified</color> \nEach time damage is taken, subtract "+DefendAmount+"."; 
            }
            if(Revenge2 == true)
            {
                DiceHoverTextManager.ReactionText2.text =  
                "<color=#C51B69>Revenge</color> \nOn death, deal 2 damage to the player."; 
            }
            if(Luminous2 == true)
            {
                DiceHoverTextManager.ReactionText2.text = 
                "<color=#C51B69>Luminous</color> \nOn death, grant the player an additional energy next turn."; 
            }
            if(Martyr2 == true)
            {
                DiceHoverTextManager.ReactionText2.text =   
                "<color=#C51B69>Martyr</color> \nOn death, heal all allies."; 
            }
            if(Resiliant2 == true)
            {
                DiceHoverTextManager.ReactionText2.text =  
                "<color=#C51B69>Resiliant</color> \nOn death, grant all allies defense."; 
            }
                
        }
    }

    public void OnMouseExit() 
    {
        DiceHoverTextManager.ReactionTextPanel1.SetActive(false);
        DiceHoverTextManager.ReactionTextPanel2.SetActive(false);
        DiceHoverTextManager.HoverEnemyPanel.SetActive(false); 
        if (_isSoundPlaying)
            {
                _isSoundPlaying = false;
            }

    }









    public void FindNemesisSlots()
    {
        List<GameObject> eligibleGrids = new List<GameObject>();
        var findNemesisGrids = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject grid in findNemesisGrids)
        {
            if (grid.GetComponent<GridPositionScript>().isBlocked == false && grid.GetComponent<GridPositionScript>().LandMarkGridPersist == false && grid.GetComponent<GridPositionScript>().EnemyInfluence == false && grid.GetComponent<GridPositionScript>().ArtifactChild == false)
            {
                eligibleGrids.Remove(grid);
                eligibleGrids.Add(grid);
            }
        }

        if(eligibleGrids.Count >= 1)
        {
            int randomIndex = Random.Range(0, eligibleGrids.Count);
            GameObject randomGrid = eligibleGrids[randomIndex];

            randomGrid.GetComponent<GridPositionScript>().EnemyInfluence = true;

            eligibleGrids.Clear();
        }
        else
        {
            return;
        }
    }

    private void ShowDamage(string text)
    {
        if(FloatingText)
        {
            var floatingTextPosition = new Vector3(transform.position.x - 1f,transform.position.y + 2.5f,transform.position.z - 10f);
            GameObject prefab = Instantiate(FloatingText, floatingTextPosition,Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    public void DetermineAmountOfActions()
    {
        int randomNumber = EnemyActionDice[Random.Range(0,EnemyActionDice.Length)];

        Debug.Log(randomNumber);
        EnemyActionCount = randomNumber;
        //return randomNumber;
        // We are going to use this random number generator to determine the amount of actions the enemy player will make 
        // we then need to determine which kind of actions these are
    }

    public void DetermineActions()
    {
        FindNemesisSlots();
        
        if (EnemyActionCount == 3)
        {
            int randomNumber1 = EnemyAbilityDice[Random.Range(0,EnemyAbilityDice.Length)];
            RandomAction1 = randomNumber1;
            EnemyActionCount --;
            
            if (RandomAction1 == 1)
            {
                SpriteRenderer_Slot1.sprite = AttackSprite; 
            }
            else if (RandomAction1 == 2)
            {
                SpriteRenderer_Slot1.sprite = DefendSprite;  
            }
            else if (RandomAction1 == 3)
            {
                // Find all the game objects with the "enemy" tag
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                // Check if all enemies have full health
                bool allEnemiesFullHealth = true;
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
                    {
                        allEnemiesFullHealth = false;
                        break;
                    }
                }

                // If all enemies have full health, call the Attack() function
                if (allEnemiesFullHealth == true)
                {
                    //return;
                    RandomAction1 = 1;
                    SpriteRenderer_Slot1.sprite = AttackSprite;
                }

                else
                {
                    RandomAction1 = 3;
                    SpriteRenderer_Slot1.sprite = HealSprite;  
                }
 
            }
            else if (RandomAction1 == 4)
            {
                SpriteRenderer_Slot1.sprite = AoeDefendSprite;  
            }
            else if (RandomAction1 == 5)
            {
                if(Focused == true)
                {
                    RandomAction1 = 1;
                    SpriteRenderer_Slot1.sprite = AttackSprite;
                }

                else
                {
                    RandomAction1 = 5;
                    SpriteRenderer_Slot1.sprite = FocusSprite;  
                }
                  
            }
            else if (RandomAction1 == 6)
            {
                SpriteRenderer_Slot1.sprite = XSprite;  
            }
            else if (RandomAction1 == 7)
            {
                SpriteRenderer_Slot1.sprite = NemesisDiceSprite;  
            }
            else if (RandomAction1 == 8)
            {
                SpriteRenderer_Slot1.sprite = AoeHealSprite;  
            }
            else if (RandomAction1 == 9)
            {
                SpriteRenderer_Slot1.sprite = SunderSprite;  
            }
            else if (RandomAction1 == 10)
            {
                SpriteRenderer_Slot1.sprite = LifestealSprite;  
            }


            int randomNumber2 = EnemyAbilityDice[Random.Range(0,EnemyAbilityDice.Length)];
            RandomAction2 = randomNumber2;
            EnemyActionCount --;

            if (RandomAction2 == 1)
            {
                if(RandomAction1 == 1)
                {
                    RandomAction2 = 2;
                    SpriteRenderer_Slot2.sprite = DefendSprite;
                    RandomAction3 = 6;
                    SpriteRenderer_Slot3.sprite = XSprite;
                }
                else
                {
                    RandomAction2 = 1;
                    SpriteRenderer_Slot2.sprite = AttackSprite;
                }
                 
            }
            else if (RandomAction2 == 2)
            {
                SpriteRenderer_Slot2.sprite = DefendSprite;  
            }
            else if (RandomAction2 == 3)
            {

                // Find all the game objects with the "enemy" tag
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                // Check if all enemies have full health
                bool allEnemiesFullHealth = true;
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
                    {
                        allEnemiesFullHealth = false;
                        break;
                    }
                }

                // If all enemies have full health, call the Attack() function
                if (allEnemiesFullHealth == true)
                {
                    //return;
                    RandomAction2 = 1;
                    SpriteRenderer_Slot2.sprite = AttackSprite;
                }

                else
                {
                    RandomAction2 = 3;
                    SpriteRenderer_Slot2.sprite = HealSprite;  
                }
 
            }
            else if (RandomAction2 == 4)
            {
                SpriteRenderer_Slot2.sprite = AoeDefendSprite;  
            }
            else if (RandomAction2 == 5)
            {
                if(Focused == true)
                {
                    RandomAction2 = 1;
                    SpriteRenderer_Slot2.sprite = AttackSprite;
                }

                else
                {
                    RandomAction2 = 5;
                    SpriteRenderer_Slot2.sprite = FocusSprite;  
                }
            }
            else if (RandomAction2 == 6)
            {
                SpriteRenderer_Slot2.sprite = XSprite;  
            }
            else if (RandomAction2 == 7)
            {
                SpriteRenderer_Slot2.sprite = NemesisDiceSprite;  
            }
            else if (RandomAction2 == 8)
            {
                SpriteRenderer_Slot2.sprite = AoeHealSprite;  
            }
            else if (RandomAction2 == 9)
            {
                SpriteRenderer_Slot2.sprite = SunderSprite;  
            }
            else if (RandomAction2 == 10)
            {
                SpriteRenderer_Slot2.sprite = LifestealSprite;  
            }

            int randomNumber3 = EnemyAbilityDice[Random.Range(0,EnemyAbilityDice.Length)];
            RandomAction3 = randomNumber3;
            EnemyActionCount --;

            if (RandomAction3 == 1)
            {
                SpriteRenderer_Slot3.sprite = AttackSprite; 
            }
            else if (RandomAction3 == 2)
            {
                SpriteRenderer_Slot3.sprite = DefendSprite;  
            }
            else if (RandomAction3 == 3)
            {

                // Find all the game objects with the "enemy" tag
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                // Check if all enemies have full health
                bool allEnemiesFullHealth = true;
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
                    {
                        allEnemiesFullHealth = false;
                        break;
                    }
                }

                // If all enemies have full health, call the Attack() function
                if (allEnemiesFullHealth == true)
                {
                    //return;
                    RandomAction3 = 1;
                    SpriteRenderer_Slot3.sprite = AttackSprite;
                }

                else
                {
                    RandomAction3 = 3;
                    SpriteRenderer_Slot3.sprite = HealSprite;  
                }

            }
            else if (RandomAction3 == 4)
            {
                SpriteRenderer_Slot3.sprite = AoeDefendSprite;  
            }
            else if (RandomAction3 == 5)
            {
                if(Focused == true)
                {
                    RandomAction3 = 1;
                    SpriteRenderer_Slot3.sprite = AttackSprite;
                }

                else
                {
                    SpriteRenderer_Slot3.sprite = FocusSprite;  
                }  
            }
            else if (RandomAction3 == 6)
            {
                SpriteRenderer_Slot3.sprite = XSprite;  
            }
            else if (RandomAction3 == 7)
            {
                SpriteRenderer_Slot3.sprite = NemesisDiceSprite;  
            }
            else if (RandomAction3 == 8)
            {
                SpriteRenderer_Slot3.sprite = AoeHealSprite;  
            }
            else if (RandomAction3 == 9)
            {
                SpriteRenderer_Slot3.sprite = SunderSprite;  
            }
            else if (RandomAction3 == 10)
            {
                SpriteRenderer_Slot3.sprite = LifestealSprite;  
            }
        }
        else if (EnemyActionCount == 2)
        {
            int randomNumber1 = EnemyAbilityDice[Random.Range(0,EnemyAbilityDice.Length)];
            RandomAction1 = randomNumber1;
            EnemyActionCount --;

            if (RandomAction1 == 1)
            {
                SpriteRenderer_Slot1.sprite = AttackSprite; 
            }
            else if (RandomAction1 == 2)
            {
                SpriteRenderer_Slot1.sprite = DefendSprite;  
            }
            else if (RandomAction1 == 3)
            {

                // Find all the game objects with the "enemy" tag
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                // Check if all enemies have full health
                bool allEnemiesFullHealth = true;
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
                    {
                        allEnemiesFullHealth = false;
                        break;
                    }
                }

                // If all enemies have full health, call the Attack() function
                if (allEnemiesFullHealth == true)
                {
                    //return;
                    RandomAction1 = 1;
                    SpriteRenderer_Slot1.sprite = AttackSprite;
                }

                else
                {
                    RandomAction1 = 3;
                    SpriteRenderer_Slot1.sprite = HealSprite;  
                }

            }
            else if (RandomAction1 == 4)
            {
                SpriteRenderer_Slot1.sprite = AoeDefendSprite;  
            }
            else if (RandomAction1 == 5)
            {
                if(Focused == true)
                {
                    RandomAction1 = 1;
                    SpriteRenderer_Slot1.sprite = AttackSprite;
                }

                else
                {
                    SpriteRenderer_Slot1.sprite = FocusSprite;  
                } 
            }
            else if (RandomAction1 == 6)
            {
                SpriteRenderer_Slot1.sprite = XSprite;  
            }
            else if (RandomAction1 == 7)
            {
                SpriteRenderer_Slot1.sprite = NemesisDiceSprite;  
            }
            else if (RandomAction1 == 8)
            {
                SpriteRenderer_Slot1.sprite = AoeHealSprite;  
            }
            else if (RandomAction1 == 9)
            {
                SpriteRenderer_Slot1.sprite = SunderSprite;  
            }
            else if (RandomAction1 == 10)
            {
                SpriteRenderer_Slot1.sprite = LifestealSprite;  
            }
            

            int randomNumber2 = EnemyAbilityDice[Random.Range(0,EnemyAbilityDice.Length)];
            RandomAction2 = randomNumber2;
            EnemyActionCount --;

            if (RandomAction2 == 1)
            {
                if(RandomAction2 == 1)
                {
                    RandomAction2 = 2;
                    SpriteRenderer_Slot2.sprite = DefendSprite;
                }
                else
                {
                    RandomAction2 = 1;
                    SpriteRenderer_Slot2.sprite = AttackSprite; 
                }
                
            }
            else if (RandomAction2 == 2)
            {
                SpriteRenderer_Slot2.sprite = DefendSprite;  
            }
            else if (RandomAction2 == 3)
            {

                // Find all the game objects with the "enemy" tag
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                // Check if all enemies have full health
                bool allEnemiesFullHealth = true;
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
                    {
                        allEnemiesFullHealth = false;
                        break;
                    }
                }

                // If all enemies have full health, call the Attack() function
                if (allEnemiesFullHealth == true)
                {
                    //return;
                    RandomAction2 = 1;
                    SpriteRenderer_Slot2.sprite = AttackSprite;
                }

                else
                {
                    RandomAction2 = 3;
                    SpriteRenderer_Slot2.sprite = HealSprite;  
                }
            }
            else if (RandomAction2 == 4)
            {
                SpriteRenderer_Slot2.sprite = AoeDefendSprite;  
            }
            else if (RandomAction2 == 5)
            {
                if(Focused == true)
                {
                    RandomAction2 = 1;
                    SpriteRenderer_Slot2.sprite = AttackSprite;
                }

                else
                {
                    SpriteRenderer_Slot2.sprite = FocusSprite;  
                }  
            }
            else if (RandomAction2 == 6)
            {
                SpriteRenderer_Slot2.sprite = XSprite;  
            }
            else if (RandomAction2 == 7)
            {
                SpriteRenderer_Slot2.sprite = NemesisDiceSprite;  
            }
            else if (RandomAction2 == 8)
            {
                SpriteRenderer_Slot2.sprite = AoeHealSprite;  
            }
            else if (RandomAction2 == 9)
            {
                SpriteRenderer_Slot2.sprite = SunderSprite;  
            }
            else if (RandomAction2 == 10)
            {
                SpriteRenderer_Slot2.sprite = LifestealSprite;  
            }
        }
        else
        {
            int randomNumber1 = EnemyAbilityDice[Random.Range(0,EnemyAbilityDice.Length)];
            RandomAction1 = randomNumber1;
            EnemyActionCount --;

            if (RandomAction1 == 1)
            {
                SpriteRenderer_Slot1.sprite = AttackSprite; 
            }
            else if (RandomAction1 == 2)
            {
                SpriteRenderer_Slot1.sprite = DefendSprite;  
            }
            else if (RandomAction1 == 3)
            {

                // Find all the game objects with the "enemy" tag
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                // Check if all enemies have full health
                bool allEnemiesFullHealth = true;
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
                    {
                        allEnemiesFullHealth = false;
                        break;
                    }
                }

                // If all enemies have full health, call the Attack() function
                if (allEnemiesFullHealth == true)
                {
                    //return;
                    RandomAction1 = 1;
                    SpriteRenderer_Slot1.sprite = AttackSprite;
                }

                else
                {
                    RandomAction1 = 3;
                    SpriteRenderer_Slot1.sprite = HealSprite;  
                }

            }
            else if (RandomAction1 == 4)
            {
                SpriteRenderer_Slot1.sprite = AoeDefendSprite;  
            }
            else if (RandomAction1 == 5)
            {
                if(Focused == true)
                {
                    RandomAction1 = 1;
                    SpriteRenderer_Slot1.sprite = AttackSprite;
                }

                else
                {
                    SpriteRenderer_Slot1.sprite = FocusSprite;  
                } 
            }
            else if (RandomAction1 == 6)
            {
                SpriteRenderer_Slot1.sprite = XSprite;  
            }
            else if (RandomAction1 == 7)
            {
                SpriteRenderer_Slot1.sprite = NemesisDiceSprite;  
            }
            else if (RandomAction1 == 8)
            {
                SpriteRenderer_Slot1.sprite = AoeHealSprite;  
            }
            else if (RandomAction1 == 9)
            {
                SpriteRenderer_Slot1.sprite = SunderSprite;  
            }
            else if (RandomAction1 == 10)
            {
                SpriteRenderer_Slot1.sprite = LifestealSprite;  
            }

            
        }
        // for (int i = EnemyActionCount; i < 1 ;i--)
        // {
            //
        // }

        //return randomNumber;
        // We are going to use this random number generator to determine the amount of actions the enemy player will make 
        // we then need to determine which kind of actions these are
    }

    public void ExecuteAction1()
    {
        Debug.Log("executing action 1." + RandomAction1);
        //we need to code a way to look for amount of actions and execute them 1 by 1.. probably a coroutine.
        if(RandomAction1 == 1)
        {
            attack();
        }
        else if(RandomAction1 == 2)
        {
            defend();   
        }
        else if(RandomAction1 == 3)
        {
            heal();
        }
        else if(RandomAction1 == 4)
        {
            AOEDefend();    
        }
        else if(RandomAction1 == 5)
        {
            Focus();
        }
        else if(RandomAction1 == 6)
        {
            blockGrid();
        }
        else if(RandomAction1 == 7)
        {
            DiceBagScript.CreateNemesisDice();
            AttackAnimationUpward();
        }
        else if(RandomAction1 == 8)
        {
            AOEHeal();
        }
        else if(RandomAction1 == 9)
        {
            Sunder();
        }
        else if(RandomAction1 == 10)
        {
            Lifesteal();
        }

        else if(RandomAction1 == 0)
        {
            Debug.Log("Enemy Action turned to 0");
        }
        else
        {
            // Debug.Log("attack execution - value is 0");
            return;
        }
        //ExecuteAction2();
    }
    public void ExecuteAction2()
    {
        Debug.Log("executing action 2." + RandomAction2);
        if(RandomAction2 == 1)
        {
            attack();
        }
        else if(RandomAction2 == 2)
        {
            defend();   
        }
        else if(RandomAction2 == 3)
        {
            heal();
        }
        else if(RandomAction2 == 4)
        {
            AOEDefend();    
        }
        else if(RandomAction2 == 5)
        {
            Focus();
        }
        else if(RandomAction2 == 6)
        {
            blockGrid();
        }
        else if(RandomAction2 == 7)
        {
            DiceBagScript.CreateNemesisDice();
            AttackAnimationUpward();
        }
        else if(RandomAction2 == 8)
        {
            AOEHeal();
        }
        else if(RandomAction2 == 9)
        {
            Sunder();
        }
        else if(RandomAction2 == 10)
        {
            Lifesteal();
        }
        else
        {
            // Debug.Log("attack execution - value is 0");
            return;
        }
        //ExecuteAction3();
    }
    public void ExecuteAction3()
    {
        Debug.Log("executing action 3." + RandomAction3);
        if(RandomAction3 == 1)
        {
            attack();
        }
        else if(RandomAction3 == 2)
        {
            defend();   
        }
        else if(RandomAction3 == 3)
        {
            heal();
        }
        else if(RandomAction3 == 4)
        {
            AOEDefend();    
        }
        else if(RandomAction3 == 5)
        {
            Focus();
        }
        else if(RandomAction3 == 6)
        {
            blockGrid();
        }
        else if(RandomAction3 == 7)
        {
            DiceBagScript.CreateNemesisDice();
            AttackAnimationUpward();
        }
        else if(RandomAction3 == 8)
        {
            AOEHeal();
        }
        else if(RandomAction3 == 9)
        {
            Sunder();
        }
        else if(RandomAction3 == 10)
        {
            Lifesteal();
        }
        else
        {
            // Debug.Log("attack execution - value is 0");
            return;
        }

    }

    public void ResetEnemyVariables()
    {
        //reset all variables so we're ready for a fresh roll next turn 
        RandomAction1 = 0;
        RandomAction2 = 0;
        RandomAction3 = 0;
        GridPositionScript.ResetNemesisSlots();

        SpriteRenderer_Slot1.sprite = null;
        SpriteRenderer_Slot2.sprite = null;
        SpriteRenderer_Slot3.sprite = null;
        
        Player.ThornsBuff = false;


    }
    public void Death()
    {
        mc.selectedUnit = null;
        SoundManager.Instance.PlayLoudSound(_dieClip);
        //check if alive enemies (level manager) = 0. if it does, we need to change the game state to the reward game state
        if (LevelManager.AliveEnemies == 1)
        {
            LevelManager.AliveEnemies = 0;
            if(Luminous == true || Luminous2 == true)
            {
                Player.EnergyBuff = true;
            }
            // GameManager.StateChangeToRewards();
            DiceHoverTextManager.HoverEnemyPanel.SetActive(false); 
            Animator.SetBool("DeathAnim", true);
            Destroy(gameObject, .75f);
        }
        else
        {
            LevelManager.AliveEnemies --; 
            if(Luminous == true || Luminous2 == true)
            {
                Player.EnergyBuff = true;
            }
            DiceHoverTextManager.HoverEnemyPanel.SetActive(false); 
            Animator.SetBool("DeathAnim", true);
            Destroy(gameObject, .75f);  
        }
    }

    public void Lifesteal()
    {
        if (Focused == true)
        {
            int focusedDamage = AttackDamage *2;
            Player.TakeDamage(focusedDamage);
            Focused = false;
            CurrentHealth = CurrentHealth += focusedDamage;
            AttackAnimationForward();
            if (Player.ThornsBuff == true)
            {
                TakeDamage(2);
            }
        }
        else
        {
            Player.TakeDamage(AttackDamage);
            AttackAnimationForward();
            CurrentHealth = CurrentHealth += AttackDamage;
            if (Player.ThornsBuff == true)
            {
                TakeDamage(2);
            }
        }
    }

    //1 = attack 2= defend 3= heal 4 = bash 5= cleave 6= block grid

    public void attack()
    {
        if (Focused == true)
        {
            int focusedDamage = AttackDamage *2;
            Player.TakeDamage(focusedDamage);
            Focused = false;
            AttackAnimationForward();
            if (Player.ThornsBuff == true)
            {
                TakeDamage(2);
            }
        }
        else
        {
            Player.TakeDamage(AttackDamage);
            AttackAnimationForward();
            if (Player.ThornsBuff == true)
            {
                TakeDamage(2);
            }
        }
    }

    public void defend()
    {
        SoundManager.Instance.PlayLoudSound(_defendClip);
        CurrentDefense = CurrentDefense + DefendAmount;
        AttackAnimationUpward();
    }

    public void heal()
    {
        HealEnemies();
    }

    public void AOEDefend()
    {
        SoundManager.Instance.PlayLoudSound(_defendClip);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            enemyAI.CurrentDefense = enemyAI.CurrentDefense + DefendAmount;
            AttackAnimationUpward();
        } 
    }
    public void AOEHeal()
    {
        SoundManager.Instance.PlayLoudSound(_healClip);
        // return;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            AttackAnimationUpward();
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            enemyAI.CurrentHealth += HealAmount;
        }     
    }

    public void Focus()
    {
        AttackAnimationUpward();
        Focused = true;
    }
    public void Sunder()
    {
        //lower hero defense to 0
        AttackAnimationForward();
        Player.DefensePoints = 0;
    }

    public void blockGrid()
{
    // Find all GameObjects tagged with "Grid"
    GameObject[] grids = GameObject.FindGameObjectsWithTag("Grid");

    // Create a list to store grids that have isBlocked and isLandmark set to false
    List<GameObject> availableGrids = new List<GameObject>();

    for (int i = 0; i < grids.Length; i++)
    {
        // Check if the grid's isBlocked and isLandmark are set to false
        bool isBlocked = grids[i].GetComponent<GridPositionScript>().isBlocked;
        bool isLandmark = grids[i].GetComponent<GridPositionScript>().LandMarkGrid;
        bool isNemesis = grids[i].GetComponent<GridPositionScript>().EnemyInfluence;
        bool LandmarkPersist = grids[i].GetComponent<GridPositionScript>().LandMarkGridPersist;
        bool isArtifactChild = grids[i].GetComponent<GridPositionScript>().ArtifactChild;

        if (isBlocked == false && isLandmark == false && LandmarkPersist == false && isNemesis == false && isArtifactChild == false)
        {
            // Add the grid to the list
            availableGrids.Add(grids[i]);
            Debug.Log("blocked grids - available grids list -  " + grids[i]);
        }
    }

    // check if all grids are blocked or landmark
    if (availableGrids.Count == 0)
    {
        Debug.Log("All grid spots are unavailable for block!");
        //return;
        attack();
    }
    else
    {
        // Pick a random grid from the list
        int randomIndex = Random.Range(0, availableGrids.Count);
        GameObject randomGrid = availableGrids[randomIndex];

        // Update isBlocked to true
        randomGrid.GetComponent<GridPositionScript>().isBlocked = true;
        randomGrid.GetComponent<GridPositionScript>().BlockCountdown = 3;
        AttackAnimationUpward();
    }
    //Empty the list
    availableGrids.Clear();
}

    public void TakeDamage(int damage)
    {
        ShowDamage(damage.ToString());
        HealthParticle.SetActive(true);
        SoundManager.Instance.PlayLoudSound(_takeDamageClip);

        Debug.Log("Take Damage Triggered" + " " + damage + " " + gameObject.name);
        // Subtract damage from defend points first
        if(damage >= 1)
        {
            // REACTIONS CODE Block
            if(StoneSkin == true && damage >= 6 || StoneSkin2 == true && damage >= 6)
            {
                CurrentDefense -= 6;
                Player.AttackAnimationForward();
            }
            else if(Trickster == true && damage >= 6 || Trickster2 == true && damage >= 6)
            {
                DiceBagScript.CreateNemesisDice(); 
                CurrentDefense -= damage;
                Player.AttackAnimationForward();
            }
            else if(Deadlock == true && damage >= 6 || Deadlock2 == true && damage >= 6)
            {
                blockGrid();
                CurrentDefense -= damage;
                Player.AttackAnimationForward();
            }
            else if(ShieldWall == true && damage >= 6 || ShieldWall2 == true && damage >= 6)
            {
                AOEDefend();
                CurrentDefense -= damage;
                Player.AttackAnimationForward();
            }
            else if(BloodLust == true && damage >= 6 || BloodLust2 == true && damage >= 6)
            {
                Focused = true;
                CurrentDefense -= damage;
                Player.AttackAnimationForward();
            }
            else if(Fortified == true|| Fortified2 == true)
            {
                CurrentDefense -= damage;
                Player.AttackAnimationForward();
                defend();
            }
            else
            {
                CurrentDefense -= damage;
                Player.AttackAnimationForward(); 
            }
            // place if statements in this block for enemy reactions 
        }

        


        // If there are still some points of damage left,
        // subtract the remaining damage from health points
        if (CurrentDefense < 0)
        {
            CurrentHealth += CurrentDefense;
            CurrentDefense = 0;
        }

        // Make sure the health points don't go below zero
        CurrentHealth = Mathf.Max(CurrentHealth, 0);
        AttackAnimationDamaged();
        if (CurrentHealth == 0 && BossMonster == true)
        {
            Death();
            ArtifactReward();
        }
        else if (CurrentHealth == 0)
        {
            Death();
        }
    }

     public void TakeDamage2(int damage)
     // this is for poison damage and should not trigger reactions 
    {
        ShowDamage(damage.ToString());
        Debug.Log("Take Damage Triggered" + " " + damage + " " + gameObject.name);
        // Subtract damage from defend points first
        if(damage >= 1)
        {
            CurrentDefense -= damage;

        }

        // If there are still some points of damage left,
        // subtract the remaining damage from health points
        if (CurrentDefense < 0)
        {
            CurrentHealth += CurrentDefense;
            CurrentDefense = 0;
        }

        // Make sure the health points don't go below zero
        CurrentHealth = Mathf.Max(CurrentHealth, 0);
        if (CurrentHealth == 0 && BossMonster == true)
        {
            Death();
            ArtifactReward();
        }
        else if (CurrentHealth == 0)
        {
            Death();
        }
    }

    public void TakePoisonDamage()
    {
        if( PoisonTickDamage >= 1)
        {
            TakeDamage2(PoisonTickDamage); 
        }
    }

    public void Selected()
        { 
            if (selected == true)
                {
                    // selected = false;
                    // mc.selectedUnit = null;
                    // print("EnemyAI - Selected Function: " + gameObject.name + " Unselected"); 
                    return;
                }
            else
                {
                    if(mc.selectedUnit != null)
                    {
                        mc.selectedUnit.selected = false;
                    }
                
                selected = true;
                mc.selectedUnit = this;
                print("Unitscript - Selected Function: " + gameObject.name + " Selected"); 
                }
        }

    void HealEnemies()
    {
        // Find all the game objects with the "enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check if all enemies have full health
        bool allEnemiesFullHealth = true;
        foreach (GameObject enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI.CurrentHealth < enemyAI.MaxHealth)
            {
                allEnemiesFullHealth = false;
                break;
            }
        }

        // If all enemies have full health, call the Attack() function
        if (allEnemiesFullHealth == true)
        {
            //return;
            attack();
        }
        else
        {
            // Find the enemy with the lowest health points
            GameObject enemyToHeal = null;
            int minHealth = int.MaxValue;
            foreach (GameObject enemy in enemies)
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI.CurrentHealth < minHealth)
                {
                    enemyToHeal = enemy;
                    minHealth = enemyAI.CurrentHealth;
                }
            }

            // Heal the enemy with the lowest health points
            if (enemyToHeal != null)
            {
                SoundManager.Instance.PlayLoudSound(_healClip);
                EnemyAI enemyAI = enemyToHeal.GetComponent<EnemyAI>();
                enemyAI.CurrentHealth += HealAmount;
                AttackAnimationUpward();
                Debug.Log("heal function triggered.");
            }
            else
            {
                //this should attack if enemytoheal = null
                attack();   
            }
        }
    }


    public void AttackAnimationForward()
    {
        StartCoroutine(MoveIntoView(EnemyGameObject, new Vector3(CurrentLocation.x - 4f, CurrentLocation.y, 0f), .25f));
    }

    public void AttackAnimationUpward()
    {
        StartCoroutine(MoveIntoView(EnemyGameObject, new Vector3(CurrentLocation.x, CurrentLocation.y +1f, 0f), .25f));
    }

    public void AttackAnimationDamaged()
    {
        StartCoroutine(MoveIntoView(EnemyGameObject, new Vector3(CurrentLocation.x-1f, CurrentLocation.y, 0f), .10f));
    }

    IEnumerator MoveIntoView(GameObject objectToMove, Vector3 endPosition, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, CurrentLocation, (elapsedTime / time));
            yield return new WaitForEndOfFrame();
        }
    }

    public void ArtifactReward()
    {
        RelicRewardManager.ArtifactManagerRewardFunction();
    }
}
