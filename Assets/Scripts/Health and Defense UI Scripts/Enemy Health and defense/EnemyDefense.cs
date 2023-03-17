using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefense : MonoBehaviour
{
    Player Player;
    EnemyAI EnemyAI;

    [Header("Sprite Renderer")]
    public SpriteRenderer SpriteRenderer;

    [Header("This Game Object")]
    public GameObject thisEnemy;

    [Header("Defense Sprites")]
    public Sprite[] DefendSprites;


    void Start()
    {
        var script = this;
        Player = GameObject.FindObjectOfType<Player>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
        SpriteRenderer = script.GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {  
        if(thisEnemy.GetComponent<EnemyAI>().CurrentDefense >= 1 && thisEnemy.GetComponent<EnemyAI>().CurrentDefense <= DefendSprites.Length)
        {
            int spriteIndex = thisEnemy.GetComponent<EnemyAI>().CurrentDefense - 1;
            SpriteRenderer.sprite = DefendSprites[spriteIndex];  
        }
        else
        {
            SpriteRenderer.sprite = null;
        }
      
    }
}
