using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSkip : MonoBehaviour
{
    DiceBagScript DiceBagScript;
    DiceScript DiceScript;
    RewardsGenerator RewardsGenerator;
    GameManager GameManager;
    SoundManager SoundManager;

    [Header("Audio reference")]
    [SerializeField] private AudioClip _UiClick;


    // Start is called before the first frame update
    void Start()
    {
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        DiceScript = GameObject.FindObjectOfType<DiceScript>();
        RewardsGenerator = GameObject.FindObjectOfType<RewardsGenerator>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    private void OnMouseDown() 
    {
        SkipReward();
        SoundManager.Instance.PlayLoudSound(_UiClick);
    }

    // Update is called once per frame
    public void SkipReward()
    {
        var findDice = GameObject.FindGameObjectsWithTag("Dice");

        foreach (GameObject dice in findDice)
        {
            var diceVars = dice.GetComponent<DiceScript>();
            if (diceVars.NewlySpawnedDice == true)
            {
                Destroy(dice);
                DiceBagScript.diceBag.Remove(dice);
                DiceBagScript.diceBagCount = DiceBagScript.diceBag.Count;

                RewardsGenerator.MoveRewardUIAway();
                
                GameManager.StateChangeToVictory();
            }
        }
    }
}
