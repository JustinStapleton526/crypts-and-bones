using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedButtonScript : MonoBehaviour
{
    DiceBoardSpriteScript DiceBoardSpriteScript; 
    Player Player;
    DiceHoverTextManager DiceHoverTextManager;
    MouseController MouseController;
    // Start is called before the first frame update
    void Start()
    {
        DiceBoardSpriteScript = GameObject.FindObjectOfType<DiceBoardSpriteScript>(); 
        Player = GameObject.FindObjectOfType<Player>();
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        MouseController = GameObject.FindObjectOfType<MouseController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DiceBoardSpriteScript.GetComponent<SpriteRenderer>().color.a <.9f)
        {
            this.GetComponent<Image>().enabled = false;
        }
        else
        {
            this.GetComponent<Image>().enabled = true;
        }

        if(Player.Energy <= -1 )
        {
            
            //gameObject.GetComponent<CanvasGroup>().alpha = .75f;
            this.GetComponent<Button>().interactable = false;
            
        }
        else
        {
           gameObject.GetComponent<CanvasGroup>().alpha = 1f;
           this.GetComponent<Button>().interactable = true;
        }
    }

}
