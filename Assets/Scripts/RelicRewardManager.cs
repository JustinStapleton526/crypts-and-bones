using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicRewardManager : MonoBehaviour
{
    private static RelicRewardManager _instance;
    public static RelicRewardManager Instance { get { return _instance; } }

    Player Player;
    LevelManager LevelManager;
    GameManager GameManager;
    MouseController mc;
    Animator Animator;
    DiceBagScript DiceBagScript;
    GridPositionScript GridPositionScript;
    DiceHoverTextManager DiceHoverTextManager;
    EnemyAI EnemyAI;

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
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        Player = GameObject.FindObjectOfType<Player>();
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        mc = FindObjectOfType<MouseController>();
        GridPositionScript = GameObject.FindObjectOfType<GridPositionScript>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
    }

    public void ArtifactManagerRewardFunction()
    {
        StartCoroutine(GenerateArtifactReward());
    }

    List<int> availableRelicIDs = new List<int> {1, 2, 3, 4, 5, 6, 7};

    IEnumerator GenerateArtifactReward()
    {
        if(LevelManager.CurrentLevel == 3)
            {
                int index = UnityEngine.Random.Range(0, availableRelicIDs.Count);
                int relicID = availableRelicIDs[index];
                GameManager.Artifact1.GetComponent<RelicScript>().RelicID = relicID;
                GameManager.Artifact1.GetComponent<RelicScript>().RelicEnabled = true;
                availableRelicIDs.RemoveAt(index);
                Debug.Log(availableRelicIDs);
            }
        else if(LevelManager.CurrentLevel == 5)
            {
                int index = UnityEngine.Random.Range(0, availableRelicIDs.Count);
                int relicID = availableRelicIDs[index];
                GameManager.Artifact2.GetComponent<RelicScript>().RelicID = relicID;
                GameManager.Artifact2.GetComponent<RelicScript>().RelicEnabled = true;
                availableRelicIDs.RemoveAt(index);
                Debug.Log(availableRelicIDs);
            }
        else if(LevelManager.CurrentLevel == 7)
            {
                int index = UnityEngine.Random.Range(0, availableRelicIDs.Count);
                int relicID = availableRelicIDs[index];
                GameManager.Artifact3.GetComponent<RelicScript>().RelicID = relicID;
                GameManager.Artifact3.GetComponent<RelicScript>().RelicEnabled = true;
                availableRelicIDs.RemoveAt(index);
                Debug.Log(availableRelicIDs);
            }
        else if(LevelManager.CurrentLevel == 9)
            {
                int index = UnityEngine.Random.Range(0, availableRelicIDs.Count);
                int relicID = availableRelicIDs[index];
                GameManager.Artifact4.GetComponent<RelicScript>().RelicID = relicID;
                GameManager.Artifact4.GetComponent<RelicScript>().RelicEnabled = true;
                availableRelicIDs.RemoveAt(index);
                Debug.Log(availableRelicIDs);
            }
        yield return new WaitForSeconds(.10f);
    }
}
