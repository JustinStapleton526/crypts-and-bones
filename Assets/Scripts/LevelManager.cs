using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    GameManager GameManager;
    [Header("Level Details")]
    public int CurrentLevel;
    public int LastLevel = 31;
    public bool CurrentLevelComplete = false;
    public Sprite [] BackgroundSprites;
    public Text CurrentLevelText;
    [Header("Player")] 
    public GameObject playerPrefab;
    [Header("Level Enemies")]    
    public GameObject[] enemyPrefabs;
    public Vector3 EnemyPosition_1;
    public Vector3 EnemyPosition_2;
    public Vector3 EnemyPosition_3;
    public int AliveEnemies;
 



    private void Start() 
    {
        GameManager = GameObject.FindObjectOfType<GameManager>();
        CurrentLevel = 1;// if we do a dungeon map, we can save the level# to the user preferences and at game start we can make this currentlevel = user.pref.get(int "currentlevel");

        EnemyPosition_1 = new Vector3(8.03f,-3.02f,0);
        EnemyPosition_2 = new Vector3(4.89f,-1.75f,0);
        EnemyPosition_3 = new Vector3(7.66f,-.38f,0);
    }

    public void AdvanceLevel()
    {
        CurrentLevel++;
        CurrentLevelComplete = false;
    }

    public void SpawnEnemies()
    {
        if(CurrentLevel == 1)
        {
            CurrentLevelText.text = "Tutorial - Floor " + CurrentLevel;

            //instantiate enemy
            GameObject enemy2 = Instantiate(enemyPrefabs[0],EnemyPosition_2,Quaternion.identity);
            SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
            spriteRenderer2.sortingOrder = 2;
            // add this enemy to AliveEnemyCounter
            AliveEnemies++;
            // add this enemy to AliveEnemyCounter
        }

        else if(CurrentLevel == 2)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[6],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[5],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else if (randomizer == 2)
                {
                    //instantiate enemy
                    GameObject enemy2 = Instantiate(enemyPrefabs[6],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemy
                    GameObject enemy3 = Instantiate(enemyPrefabs[0],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[0],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[7],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
        }

        else if (CurrentLevel == 3)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[5],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[10],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else if (randomizer == 2)
                {
                    //instantiate enemy
                    GameObject enemy2 = Instantiate(enemyPrefabs[6],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemy
                    GameObject enemy3 = Instantiate(enemyPrefabs[10],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[7],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[10],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
        }
        else if (CurrentLevel == 4)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[0],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[6],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[6],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
            else if (randomizer == 2)
                {
                    //instantiate enemy
                    GameObject enemy2 = Instantiate(enemyPrefabs[1],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemy
                    GameObject enemy3 = Instantiate(enemyPrefabs[0],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[0],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[7],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[2],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
        }
        else if (CurrentLevel == 5)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[1],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[9],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter 
                }
            else if (randomizer == 2)
                {
                    //instantiate enemy
                    GameObject enemy2 = Instantiate(enemyPrefabs[10],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemy
                    GameObject enemy3 = Instantiate(enemyPrefabs[2],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[6],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[11],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[1],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }

        }
        else if (CurrentLevel == 6)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[1],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[1],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter 
                }
            else if (randomizer == 2)
                {
                    //instantiate enemy
                    GameObject enemy2 = Instantiate(enemyPrefabs[1],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemy
                    GameObject enemy3 = Instantiate(enemyPrefabs[2],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[7],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[5],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[1],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }

        }
        else if (CurrentLevel == 7)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[10],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[5],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;  
                }
            else if (randomizer == 2)
                {
                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[1],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[11],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[8],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[5],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
  
        }
        else if (CurrentLevel == 8)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,4);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[1],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[4],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;  
                }
            else if (randomizer == 2)
                {
                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[1],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[3],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[3],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[7],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
  
        }
        else if (CurrentLevel == 9)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,3);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[8],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[4],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;  
                }
            else if (randomizer == 2)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[3],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[11],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[4],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[7],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[9],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[1],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
        }
        else if (CurrentLevel == 10)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

            int randomizer = UnityEngine.Random.Range(1,3);

            if (randomizer == 1)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[9],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[1],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++;  
                }
            else if (randomizer == 2)
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[2],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[11],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[5],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
            else
                {
                    GameObject enemy1 = Instantiate(enemyPrefabs[7],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy2 = Instantiate(enemyPrefabs[8],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                    AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                    GameObject enemy3 = Instantiate(enemyPrefabs[1],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                    AliveEnemies++; 
                }
        }

        else if (CurrentLevel == 11)
        {
            CurrentLevelText.text = "Floor " + CurrentLevel;

                GameObject enemy1 = Instantiate(enemyPrefabs[3],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                 AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                GameObject enemy2 = Instantiate(enemyPrefabs[5],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                GameObject enemy3 = Instantiate(enemyPrefabs[9],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                AliveEnemies++;
            
        }
        else if (CurrentLevel == 12)
        {
                CurrentLevelText.text = "Floor " + CurrentLevel;

                GameObject enemy1 = Instantiate(enemyPrefabs[11],EnemyPosition_1,Quaternion.identity);
                    SpriteRenderer spriteRenderer1 = enemy1.GetComponent<SpriteRenderer>();
                    spriteRenderer1.sortingOrder = 3;
                 AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                GameObject enemy2 = Instantiate(enemyPrefabs[9],EnemyPosition_2,Quaternion.identity);
                    SpriteRenderer spriteRenderer2 = enemy2.GetComponent<SpriteRenderer>();
                    spriteRenderer2.sortingOrder = 2;
                AliveEnemies++;
                    // add this enemy to AliveEnemyCounter

                    //instantiate enemies for 1 
                GameObject enemy3 = Instantiate(enemyPrefabs[8],EnemyPosition_3,Quaternion.identity);
                    SpriteRenderer spriteRenderer3 = enemy3.GetComponent<SpriteRenderer>();
                    spriteRenderer3.sortingOrder = 1;
                AliveEnemies++;
        }
        else if (CurrentLevel == 13)
        {
            
        }
        else if (CurrentLevel == 14)
        {
            
        }
        else if (CurrentLevel == 15)
        {
            
        }
        else if (CurrentLevel == 16)
        {
            
        }
        else if (CurrentLevel == 17)
        {
            
        }
        else if (CurrentLevel == 18)
        {
            
        }
        else if (CurrentLevel == 19)
        {
            
        }
        else if (CurrentLevel == 20)
        {
            
        }
        else if (CurrentLevel == 21)
        {
            
        }
        else if (CurrentLevel == 22)
        {
            
        }
        else if (CurrentLevel == 23)
        {
            
        }
        else if (CurrentLevel == 24)
        {
            
        }
        else if (CurrentLevel == 25)
        {
            
        }
        else if (CurrentLevel == 26)
        {
            
        }
        else if (CurrentLevel == 27)
        {
            
        }
        else if (CurrentLevel == 28)
        {
            
        }
        else if (CurrentLevel == 29)
        {
            
        }
        else if (CurrentLevel == 30)
        {
            
        }
    }
    public void LevelComplete()
    {
        // enemy script will check if EnemiesAlive variable is = to 0 on death. if = to 0, mark level as complete.
        CurrentLevelComplete = true;
        //if true, we will start the menus/game starts for rewards before eventually triggering the next level. 
    }

    

}
