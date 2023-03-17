using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIScript : MonoBehaviour
{   
    Player Player;
    EnemyAI EnemyAI;

    [Header("Sprite Renderer")]
    public SpriteRenderer SpriteRenderer;

    [Header("This Game Object")]
    public GameObject thisPlayer;

    [Header("Health Sprites")]
    public Sprite[] HealthSprites;


    void Start()
    {
        var script = this;
        Player = GameObject.FindObjectOfType<Player>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
        SpriteRenderer = script.GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        if(thisPlayer.GetComponent<Player>().CurrentHealth >= 1 && thisPlayer.GetComponent<Player>().CurrentHealth <= HealthSprites.Length)
        {
            int spriteIndex = thisPlayer.GetComponent<Player>().CurrentHealth - 1;
            SpriteRenderer.sprite = HealthSprites[spriteIndex];  
        }
        else
        {
            SpriteRenderer.sprite = null;
        }
    }

}
