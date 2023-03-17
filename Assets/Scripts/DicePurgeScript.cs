using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicePurgeScript : MonoBehaviour
{
    GameManager GameManager;
    DiceScript DiceScript;
    DiceBagScript DiceBagScript;
    DiceHoverTextManager DiceHoverTextManager;
    

    private void Start() 
    {
        DiceScript = GameObject.FindObjectOfType<DiceScript>();
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
    }

    void OnMouseDown()
    {
        Debug.Log(GameManager.State.ToString());
        // Check if the game manager is in the "PurgeDice" state
        if (GameManager.State.ToString()  == "PurgeDice")
        {
            StartCoroutine(PurgeDice());
        }
    }


    IEnumerator PurgeDice()
    {
        Debug.Log("coroutine - purge dice");
        DiceBagScript.RemoveFromDiceBag(gameObject);
        yield return new WaitForSeconds(.10f);
        DiceHoverTextManager.HoverTextPanel.SetActive(false);
        yield return StartCoroutine(ChangeState());
    }

    IEnumerator DestroyObject()
    {
        Debug.Log("coroutine - destroy object");
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
        yield return null;
    }

        IEnumerator ChangeState()
    {
        Debug.Log("coroutine - change state ");
        GameManager.StateChangetoReceiveNewDice();
        yield return StartCoroutine(DestroyObject());
    }
}
