using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [Header("Diceboard Object")]
    public GameObject DiceBoardObject;
    DiceBoard DiceBoard;

    [Header("Dicebag Object")]
    public GameObject DiceBagObject;
    DiceBagScript DiceBag;



    private void Start() 
    {
        DiceBoard = GameObject.FindObjectOfType<DiceBoard>();
        DiceBag = GameObject.FindObjectOfType<DiceBagScript>();

    }


      

}
