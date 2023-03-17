using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsGenerator : MonoBehaviour
{
    DiceBagScript DiceBagScript;
    // Drag your prefabs into the Inspector
    [Header("Random Dice Generator Prefabs")]
    public GameObject attackDicePrefab;
    public GameObject defenseDicePrefab;
    public GameObject healDicePrefab;
    public GameObject enhanceDicePrefab;
    public GameObject cleaveDicePrefab;
    public GameObject bashDicePrefab;
    public GameObject slamDicePrefab;
    public GameObject energyDicePrefab;

    [Header("Pretty Portraits for Dice Rewards")]
    public GameObject PrettyPortrait;
    public Sprite[] DiceSideSprites;
    public SpriteRenderer DiceSidesRend;
    public Vector3 DiceEndPosition = new Vector3(-1.84f, 1.5f, 0f);
    public Vector3 PrettyPortraitEndPosition; //= new Vector3(0f, -1.06f, -1);
    public Vector3 PrettyPortraitStartPosition; //= new Vector3(0f, 10f, -1);
    public Text PrettyPortraitText1;
    public Text PrettyPortraitText2;



    private void Start() 
    {
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        PrettyPortraitStartPosition = new Vector3(transform.position.x+ 1.4f,transform.position.y + 10,0f);
        PrettyPortraitEndPosition = new Vector3(transform.position.x + 1.4f,transform.position.y + .16f,0f);
    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SpawnCommonDice()
    {
        // Put the prefabs into an array
        GameObject[] dicePrefabs = { attackDicePrefab, defenseDicePrefab,energyDicePrefab, healDicePrefab };

        // Choose a random prefab from the array
        GameObject prefab = dicePrefabs[Random.Range(0, dicePrefabs.Length)];

        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(-1.84f, 20f, 0f);

        // Instantiate the prefab
        GameObject dice = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Make the instantiated object a child of the "Dice Bag" game object
        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;

        // Choose a random type from the options "common", "rare", and "epic"
        string[] types = {"Common", "Common", "Common", "Rare"};
        string type = types[Random.Range(0, types.Length)];

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Normal","Poison","Landmark","Ephemeral","Weighted", "Glass", "Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = type;
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;

        if(prefab == healDicePrefab)
        {
            dice.GetComponent<DiceScript>().DiceSecondaryType = "Glass";  
        }


        if(prefab == energyDicePrefab)
        {
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Amplify" )
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Glass";   
            }
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Weighted") 
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Landmark";   
            }
              
        }
        
        
        StartCoroutine(MoveIntoView(dice, DiceEndPosition, .5f));
        StartCoroutine(MoveIntoView(PrettyPortrait, PrettyPortraitEndPosition, .5f));

        // dice.GetComponent<SpriteRenderer>().enabled = true;
        // DiceBagScript.diceBag.Remove(dice);
        // DiceBagScript.diceBag.Add(dice);
        // DiceBagScript.diceBagCount = DiceBagScript.diceBag.Count;
        PrettyPortraitText1.text = dice.GetComponent<DiceScript>().DiceType + "\n" + dice.GetComponent<DiceScript>().DiceSecondaryType + "\n" + dice.GetComponent<DiceScript>().DiceRarity;
        PrettyPortraitText2.text = dice.GetComponent<DiceScript>().DiceDescription;

        if(dice.GetComponent<DiceScript>().DiceType == "Enhance")
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[4];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[5];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[6];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[7];
            }
        }
        else
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[0];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[1];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[2];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[3];
            }
        }

    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public void SpawnRareDice()
    {
        // Put the prefabs into an array
        GameObject[] dicePrefabs = {slamDicePrefab,cleaveDicePrefab,bashDicePrefab,enhanceDicePrefab };

        // Choose a random prefab from the array
        GameObject prefab = dicePrefabs[Random.Range(0, dicePrefabs.Length)];

        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(-1.84f, 20f, 0f);

        // Instantiate the prefab
        GameObject dice = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Make the instantiated object a child of the "Dice Bag" game object
        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;

        // Choose a random type from the options "common", "rare", and "epic"
        string[] types = {"Rare", "Rare", "Rare", "Epic"};
        string type = types[Random.Range(0, types.Length)];

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Poison","Landmark","Ephemeral","Weighted","Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = type;
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;


        if(prefab == enhanceDicePrefab)
        {
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Amplify" )
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Landmark";   
            }
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Weighted") 
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Ephemeral";   
            }
              
        }

        
        StartCoroutine(MoveIntoView(dice, DiceEndPosition, .5f));
        StartCoroutine(MoveIntoView(PrettyPortrait, PrettyPortraitEndPosition, .5f));

        // dice.GetComponent<SpriteRenderer>().enabled = true;
        // DiceBagScript.diceBag.Remove(dice);
        // DiceBagScript.diceBag.Add(dice);
        // DiceBagScript.diceBagCount = DiceBagScript.diceBag.Count;
        PrettyPortraitText1.text = dice.GetComponent<DiceScript>().DiceType + "\n" + dice.GetComponent<DiceScript>().DiceSecondaryType + "\n" + dice.GetComponent<DiceScript>().DiceRarity;
        PrettyPortraitText2.text = dice.GetComponent<DiceScript>().DiceDescription;

        if(dice.GetComponent<DiceScript>().DiceType == "Enhance")
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[4];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[5];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[6];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[7];
            }
        }
        else
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[0];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[1];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[2];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[3];
            }
        }

    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public void SpawnEpicDice()
    {
        // Put the prefabs into an array
        GameObject[] dicePrefabs = { attackDicePrefab, slamDicePrefab, cleaveDicePrefab, bashDicePrefab, defenseDicePrefab, healDicePrefab, enhanceDicePrefab };

        // Choose a random prefab from the array
        GameObject prefab = dicePrefabs[Random.Range(0, dicePrefabs.Length)];

        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(-1.84f, 20f, 0f);

        // Instantiate the prefab
        GameObject dice = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Make the instantiated object a child of the "Dice Bag" game object
        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;

        // Choose a random type from the options "common", "rare", and "epic"
        string[] types = {"Epic","Epic","Epic","Epic","Mythic"};
        string type = types[Random.Range(0, types.Length)];

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Poison","Landmark","Ephemeral","Weighted","Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = type;
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;

        if(prefab == enhanceDicePrefab)
        {
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Amplify" )
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Ephemeral";   
            }
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Weighted") 
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Ephemeral";   
            }
              
        }
        
        StartCoroutine(MoveIntoView(dice, DiceEndPosition, .5f));
        StartCoroutine(MoveIntoView(PrettyPortrait, PrettyPortraitEndPosition, .5f));

        // dice.GetComponent<SpriteRenderer>().enabled = true;
        // DiceBagScript.diceBag.Remove(dice);
        // DiceBagScript.diceBag.Add(dice);
        // DiceBagScript.diceBagCount = DiceBagScript.diceBag.Count;
        PrettyPortraitText1.text = dice.GetComponent<DiceScript>().DiceType + "\n" + dice.GetComponent<DiceScript>().DiceSecondaryType + "\n" + dice.GetComponent<DiceScript>().DiceRarity;
        PrettyPortraitText2.text = dice.GetComponent<DiceScript>().DiceDescription;

        if(dice.GetComponent<DiceScript>().DiceType == "Enhance")
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[4];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[5];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[6];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[7];
            }
        }
        else if(dice.GetComponent<DiceScript>().DiceType == "Energy")
        {
            DiceSidesRend.sprite = DiceSideSprites[8];
        }
        else
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[0];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[1];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[2];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[3];
            }
        }

    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void RerollDice()
    {
        // Put the prefabs into an array
        GameObject[] dicePrefabs = { attackDicePrefab, attackDicePrefab, attackDicePrefab,energyDicePrefab,energyDicePrefab,slamDicePrefab,slamDicePrefab, attackDicePrefab,cleaveDicePrefab,cleaveDicePrefab,bashDicePrefab,bashDicePrefab, defenseDicePrefab, defenseDicePrefab, defenseDicePrefab, healDicePrefab, enhanceDicePrefab,enhanceDicePrefab };

        // Choose a random prefab from the array
        GameObject prefab = dicePrefabs[Random.Range(0, dicePrefabs.Length)];

        // Specify the position at which to instantiate the prefab
        Vector3 spawnPosition = new Vector3(DiceEndPosition.x, DiceEndPosition.y, 0f);

        // Instantiate the prefab
        GameObject dice = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Make the instantiated object a child of the "Dice Bag" game object
        dice.transform.parent = GameObject.Find("Dice Bag").transform;
        dice.transform.localPosition = Vector3.zero;
        dice.transform.localRotation = Quaternion.identity;
        dice.transform.localScale = Vector3.one;
        dice.transform.position = new Vector3(-1.84f, 1.5f, 0f);

        // Choose a random type from the options "common", "rare", and "epic"
        string[] types = {"Common", "Common", "Common", "Rare", "Rare", "Rare"};
        string type = types[Random.Range(0, types.Length)];

        // Choose a random subtype from the options "ephemeral" - no energy cost, "weighted" - 2 energy cost, "glass" - 1 time use, and "amplify"
        string[] subtypes = {"Normal","Normal","Normal","Normal","Normal","Poison","Poison","Poison","Poison","Poison","Poison", "Landmark","Landmark","Landmark", "Ephemeral","Ephemeral", "Weighted","Weighted","Weighted","Weighted", "Glass","Glass","Glass","Glass","Glass", "Amplify","Amplify"};
        string subtype = subtypes[Random.Range(0, subtypes.Length)];

        // Set the type and subtype of the dice
        dice.GetComponent<DiceScript>().DiceRarity = type;
        dice.GetComponent<DiceScript>().DiceSecondaryType = subtype;
        dice.GetComponent<DiceScript>().NewlySpawnedDice = true;
        dice.GetComponent<SpriteRenderer>().enabled = true;

        if(prefab == enhanceDicePrefab)
        {
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Amplify" )
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Landmark";   
            }
            if (dice.GetComponent<DiceScript>().DiceSecondaryType == "Weighted") 
            {
                dice.GetComponent<DiceScript>().DiceSecondaryType = "Ephemeral";   
            }
              
        }

        PrettyPortraitText1.text = dice.GetComponent<DiceScript>().DiceType + "\n" + dice.GetComponent<DiceScript>().DiceSecondaryType + "\n" + dice.GetComponent<DiceScript>().DiceRarity;
        PrettyPortraitText2.text = dice.GetComponent<DiceScript>().DiceDescription;
        

        if(dice.GetComponent<DiceScript>().DiceType == "Enhance")
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[4];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[5];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[6];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[7];
            }
        }
        else
        {
            if(dice.GetComponent<DiceScript>().DiceRarity == "Common")
            {
                DiceSidesRend.sprite = DiceSideSprites[0];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Rare")
            {
                DiceSidesRend.sprite = DiceSideSprites[1];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Epic")
            {
                DiceSidesRend.sprite = DiceSideSprites[2];
            }
            else if(dice.GetComponent<DiceScript>().DiceRarity == "Mythic")
            {
                DiceSidesRend.sprite = DiceSideSprites[3];
            }
        }
    }

    public void DestroyRecentlySpawnedDice(GameObject dice)
    {
        //find all dice, if it is NewlySpawned, destroy it, else, return.
    }

    public void RandomRewardsRoutine()
    {
        StartCoroutine(RandomizeCommonDice());
    }
    public void RandomRewardsRoutine_Rare()
    {
        StartCoroutine(RandomizeRareDice());
    }
    public void RandomRewardsRoutine_Epic()
    {
        StartCoroutine(RandomizeEpicDice());
    }

    public void MoveRewardUIAway()
    {
        StartCoroutine(MoveOutOfView(PrettyPortrait, PrettyPortraitStartPosition, .5f));
    }


    // the only goal is to generate a reward, spawn a portrait and buttons, and then change the game state. DO NOT OVERCOMPLICATE THIS.
    IEnumerator RandomizeCommonDice()
    {
        yield return new WaitForSeconds(1f);
        SpawnCommonDice();
        yield return new WaitForSeconds(.10f);
        yield return StartCoroutine(SpawnPrettyPortrait());
    }
    IEnumerator RandomizeRareDice()
    {
        yield return new WaitForSeconds(1f);
        SpawnRareDice();
        yield return new WaitForSeconds(.10f);
        yield return StartCoroutine(SpawnPrettyPortrait());
    }
    IEnumerator RandomizeEpicDice()
    {
        yield return new WaitForSeconds(1f);
        SpawnEpicDice();
        yield return new WaitForSeconds(.10f);
        yield return StartCoroutine(SpawnPrettyPortrait());
    }

    IEnumerator SpawnPrettyPortrait()
    {
        //spawn prefab and buttons 
        //StartCoroutine(MoveIntoView(PrettyPortrait, PrettyPortraitEndPosition, .5f));
        //PrettyPortrait.transform.position = PrettyPortraitEndPosition;
        yield return new WaitForSeconds(.10f);
        yield return StartCoroutine(RewardChoicesGameState());
    }


    IEnumerator RewardChoicesGameState()
    {
        
        yield return new WaitForSeconds(.10f);
        yield return null;
    }

    IEnumerator MoveIntoView(GameObject objectToMove, Vector3 endPosition, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            objectToMove.GetComponent<SpriteRenderer>().enabled = true;
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

}
