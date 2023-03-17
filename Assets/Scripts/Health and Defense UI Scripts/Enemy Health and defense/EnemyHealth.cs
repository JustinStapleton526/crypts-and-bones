using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Player Player;
    EnemyAI EnemyAI;

    [Header("Sprite Renderer")]
    public SpriteRenderer SpriteRenderer;

    [Header("This Game Object")]
    public GameObject thisEnemy;

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
        if(thisEnemy.GetComponent<EnemyAI>().CurrentHealth >= 1 && thisEnemy.GetComponent<EnemyAI>().CurrentHealth <= HealthSprites.Length)
        {
            int spriteIndex = thisEnemy.GetComponent<EnemyAI>().CurrentHealth - 1;
            SpriteRenderer.sprite = HealthSprites[spriteIndex];  
        }
        else
        {
            SpriteRenderer.sprite = null;
        }

    }
}
