using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardKeep : MonoBehaviour
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
        KeepReward();
        SoundManager.Instance.PlayLoudSound(_UiClick);
    }

    // Update is called once per frame
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

                RewardsGenerator.MoveRewardUIAway();

                GameManager.StateChangeToVictory();
            }
        }
    }
}
