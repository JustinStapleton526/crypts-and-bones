using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBackground : MonoBehaviour
{
    public GameObject Grid;
    public SpriteRenderer SpriteRenderer;
    public Sprite BubbleSprites_1;
    public Sprite BubbleSprites_2;
    public Sprite BubbleSprites_3;
    public Sprite BubbleSprites_4;
    public Sprite BubbleSprites_5;
    public Sprite BubbleSprites_6;
    public Sprite BubbleSprites_7;
    public Sprite BubbleSprites_8;
    public Sprite BubbleSprites_9;
    public Sprite BubbleSprites_10;
    public Sprite BubbleSprites_11;
    public Sprite BubbleSprites_12;
    public Sprite BubbleSprites_13;
    public Sprite BubbleSprites_14;
    public Sprite BubbleSprites_15;
    public Sprite BubbleSprites_16;
    public Sprite BubbleSprites_17;
    public Sprite BubbleSprites_18;
    public Sprite BubbleSprites_19;
    public Sprite BubbleSprites_20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Grid.GetComponent<GridPositionScript>().AttachedDice != null)
        {   
            var attachedModifiedValue = Grid.GetComponent<GridPositionScript>().GridToDiceModifier + Grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().DiceRolledValue - Grid.GetComponent<GridPositionScript>().EnemyInfluenceAmount + Grid.GetComponent<GridPositionScript>().ArtifactChildModifier + Grid.GetComponent<GridPositionScript>().AttachedDice.gameObject.GetComponent<DiceScript>().AmplifyLevel;

            if (attachedModifiedValue == 1 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_1;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (attachedModifiedValue == 2 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_2;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (attachedModifiedValue == 3 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_3;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (attachedModifiedValue == 4 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_4;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;        
            }
            else if (attachedModifiedValue == 5 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_5;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;   
            }
            else if (attachedModifiedValue == 6 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_6;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 7 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_7;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;   
            }
            else if (attachedModifiedValue == 8 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_8;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 9 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_9;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 10 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_10;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 11 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_11;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 12 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_12;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 13 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_13;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 14 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_14;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 15 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_15;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 16 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_16;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 17 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_17;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 18 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_18;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 19 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_19;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 20 )
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = BubbleSprites_20;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;    
            }
            else if (attachedModifiedValue == 0 ) 
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;    
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }


    }
}
