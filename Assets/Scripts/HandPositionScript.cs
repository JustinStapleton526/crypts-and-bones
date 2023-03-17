using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositionScript : MonoBehaviour
{
    public bool OpenDiceSlot;
    public Vector3 HandLocation;
    public GameObject HandPositionId;
    public float time;
    public float interpolationPeriod = 5f;



    private void Start() 
    {
        HandLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1); 
    }
}
