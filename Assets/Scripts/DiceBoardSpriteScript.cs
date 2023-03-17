using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBoardSpriteScript : MonoBehaviour
{
    Player Player;

    public SpriteRenderer spriterenderer;
    public Sprite FourEnergyLeft;
    public Sprite ThreeEnergyLeft;
    public Sprite TwoEnergyLeft;
    public Sprite OneEnergyLeft; 
    public Sprite NoEnergyLeft;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindObjectOfType<Player>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.CurrentHealth >= 1)
        {
            if(Player.GetComponent<Player>().Energy == 3)
            {
                spriterenderer.sprite = ThreeEnergyLeft;
            }
            else if(Player.GetComponent<Player>().Energy == 2)
            {
                spriterenderer.sprite = TwoEnergyLeft; 
            }
            else if(Player.GetComponent<Player>().Energy == 1)
            {
                spriterenderer.sprite = OneEnergyLeft;       
            }
            else if(Player.GetComponent<Player>().Energy <= 0)
            {
                spriterenderer.sprite = NoEnergyLeft;    
            }
            else if(Player.GetComponent<Player>().Energy >= 4)
            {
                spriterenderer.sprite = FourEnergyLeft;    
            }
        }
   
    }
}
