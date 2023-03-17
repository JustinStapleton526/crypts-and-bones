using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUIScript : MonoBehaviour
{   
    Player Player;
    EnemyAI EnemyAI;

    [Header("Sprite Renderer")]
    public SpriteRenderer SpriteRenderer;

    [Header("This Game Object")]
    public GameObject thisPlayer;

    [Header("Defense Sprites")]
    public Sprite[] DefenseSprites;

    void Start()
    {
        var script = this;
        Player = GameObject.FindObjectOfType<Player>();
        EnemyAI = GameObject.FindObjectOfType<EnemyAI>();
        SpriteRenderer = script.GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        if(thisPlayer.GetComponent<Player>().DefensePoints >= 1 && thisPlayer.GetComponent<Player>().DefensePoints <= DefenseSprites.Length)
        {
            int spriteIndex = thisPlayer.GetComponent<Player>().DefensePoints - 1;
            SpriteRenderer.sprite = DefenseSprites[spriteIndex];  
        }
        else
        {
            SpriteRenderer.sprite = null;
        }
    }

}
