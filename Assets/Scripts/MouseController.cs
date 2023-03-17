using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseController : MonoBehaviour
{
    GameManager GameManager;

    public EnemyAI selectedUnit;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUGGER FOR RAYCAST
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 100f;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

        if (Input.GetMouseButtonDown (0))
        {
            var focusedTileHit = GetFocusedOnTile();

            if (focusedTileHit.HasValue)
            {
                GameObject collider = focusedTileHit.Value.collider.gameObject;
                Debug.Log("mousecontroller - Collider Name :" + collider.name);

                if (collider.tag == "Enemy")
                {
                    var collider_script_variables = collider.GetComponent<EnemyAI>();
                    collider_script_variables.Selected();   
                }
                else if (collider.tag == "Dice" && GameManager.GetComponent<GameManager>().GetStateString() == "PurgeDice")
                {
                    Debug.Log("Purge Dice State Collider detected.");
                }
            }   
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
        // Debug.Log("Attempted raycasthit2d");
        
        if(hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
                //return hits.OrderByDescending(i => i.collider.transform.position.z).Last();                       
            } 
        return null;      
    }  
    
    // public void GetFocusedOnTile()
    // {
    //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        
    //     RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
    //     for (int i = 0; i < hits.Length; ++i)
    //     {
    //         Debug.Log("mousecontroller - list of hits :" + i);
    //     }
    // }
        
}
