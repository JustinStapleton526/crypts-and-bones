using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class RelicScript : MonoBehaviour
{
    //THIS SCRIPT WILL LIVE ON THE RELIC SPACES THEMSELVES. WE WILL MANAGE THE ENABLING OF THEM FROM THE GAME MANAGER. 
    LevelManager LevelManager;
    Player Player;
    DiceHoverTextManager DiceHoverTextManager;
    MouseController MouseController;

    [Header("Adjacent Grids")]
    public GameObject[] AdjacentGrids;
    public GameObject SelectedGrid;

    [Header("Relic Params")]
    public bool RelicEnabled = false; 
    public int RelicID = 0;
    public Sprite ArtifactChildSprite;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindObjectOfType<Player>();
        LevelManager = GameObject.FindObjectOfType<LevelManager>();
        DiceHoverTextManager = GameObject.FindObjectOfType<DiceHoverTextManager>();
        MouseController = GameObject.FindObjectOfType<MouseController>();
        ExecuteGridCoroutine();
    }

    public void OnMouseOver() 
    {
        if (RelicEnabled == true && RelicID == 1)
        {
            DiceHoverTextManager.HoverText3.text = "An adjacent grid space will add 2 to the dice face value."; 
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
        if (RelicEnabled == true && RelicID == 2)
        {
            DiceHoverTextManager.HoverText3.text = "Max defense raised to 20."; 
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
        if (RelicEnabled == true && RelicID == 3)
        {
            DiceHoverTextManager.HoverText3.text = "Max health raised to 30."; 
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
        if (RelicEnabled == true && RelicID == 4)
        {
            DiceHoverTextManager.HoverText3.text = "Playing a <color=#2678AD>Defense</color> die in this space applies Thorns."; 
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
        if (RelicEnabled == true && RelicID == 5)
        {
            DiceHoverTextManager.HoverText3.text = "Playing an <color=#C52E1B>Attack</color> die in this space doubles the face value.";
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
        if (RelicEnabled == true && RelicID == 6)
        {
            DiceHoverTextManager.HoverText3.text = "An adjacent grid space will add 1 to the dice face value."; 
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
        if (RelicEnabled == true && RelicID == 7)
        {
            DiceHoverTextManager.HoverText3.text = "Playing a <color=#2678AD>Defense</color> die in this space doubles the face value."; 
            DiceHoverTextManager.HoverPanelLocation3 = MouseController.GetFocusedOnTile().Value.collider.gameObject.transform.position;
            DiceHoverTextManager.HoverTextPanel3.SetActive(true); 
            DiceHoverTextManager.HoverTextPanel3.transform.position = new Vector2(DiceHoverTextManager.HoverPanelLocation3.x+2.8f, DiceHoverTextManager.HoverPanelLocation3.y+.5f);
        }
                   
    }

    public void OnMouseExit() 
    {
        DiceHoverTextManager.HoverTextPanel3.SetActive(false); 
    }

    // RELIC INFO
    // RELICID - 1; FIND 2 RANDOM ADJACENT GRIDS THAT ARE NOT IN USE AND GIVE THEM +1 TO DICE ROLL 
    // RELICID - 2; FIND 1 RANDOM ADJACENT GRID THAT IS NOT IN USE AND GIVE IT +2 TO DICE ROLL
    // RELICID - 3; 20% CHANCE TO GRANT AN ADDITIONAL ENERGY THIS TURN -- IMPLEMENT LAST 
    // RELICID - 4; MAX DEFENSE INCREASED TO 20
    // RELICID - 5; WHEN DEFENSE DICE IS PLAYED ON THIS GRID, DOUBLE VALUE 
    // RELICID - 6; WHEN 5 DEFENSE IS ACCRUED APPLY THORNS BUFF 
    // RELICID - 7; MAX HEALTH +10 
    // RELICID - 8; DEALING 10 ATTACK DAMAGE TRIGGERS CRITICAL + 10 DAMAGE 

    void Update()
    {
        if (RelicEnabled == true)
        {
            SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject;
            this.GetComponent<SpriteRenderer>().enabled = true;


            if (RelicID == 1)
            {
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 2;
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChild = true; 
                // if(SelectedGrid.GetComponent<GridPositionScript>().AttachedDice != null)
                // {
                //     // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject;
                //     SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 2; 
                // }
                // else
                // {
                //     // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = null;
                //     SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0; 
                // }
            }
            else if (RelicID == 2)
            {
                // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject;
                // Relic effect: Player defense +10
               Player.GetComponent<Player>().DefensePointsMax = 20;
               SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0;
            }
            else if (RelicID == 3)
            {
                // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject;
                // Relic effect: Player health +10
                Player.GetComponent<Player>().MaxHealth = 30;
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0;
            }
            else if (RelicID == 4)
            {
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChild = true; 
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0;
                // Relic effect: Apply thorns buff
                if(SelectedGrid.GetComponent<GridPositionScript>().AttachedDiceType == "Defense")
                {
                    Player.GetComponent<Player>().ThornsBuff = true; 
                    // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject; 
                }
                // else
                // {
                //     Player.GetComponent<Player>().ThornsBuff = false;  
                //     // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = null;
                // }
            }
            else if (RelicID == 5)
            {
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChild = true; 
                if(SelectedGrid.GetComponent<GridPositionScript>().AttachedDiceType == "Attack")
                {
                    SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = SelectedGrid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceRolledValue;
                }
                else
                {
                    SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0;
                }
            }
            else if (RelicID == 6)
            {
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChild = true;
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 1;  
                // SelectedGrid2.GetComponent<GridPositionScript>().ArtifactChild = true; 
                // SelectedGrid2.GetComponent<GridPositionScript>().ArtifactChildModifier = 1;
                // if(SelectedGrid.GetComponent<GridPositionScript>().AttachedDice != null | SelectedGrid2.GetComponent<GridPositionScript>().AttachedDice != null)
                // {
                //     SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 1; 
                //     SelectedGrid2.GetComponent<GridPositionScript>().ArtifactChildModifier = 1;
                //     // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject;
                //     // SelectedGrid2.GetComponent<GridPositionScript>().ArtifactParent = this.gameObject;
                // }
                // else
                // {
                //     SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0; 
                //     SelectedGrid2.GetComponent<GridPositionScript>().ArtifactChildModifier = 0; 
                //     // SelectedGrid.GetComponent<GridPositionScript>().ArtifactParent = null;
                //     // SelectedGrid2.GetComponent<GridPositionScript>().ArtifactParent = null;
                // }
            }
            else if (RelicID == 7)
            {
                SelectedGrid.GetComponent<GridPositionScript>().ArtifactChild = true; 
                if(SelectedGrid.GetComponent<GridPositionScript>().AttachedDiceType == "Defense")
                {
                    SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = SelectedGrid.GetComponent<GridPositionScript>().AttachedDice.GetComponent<DiceScript>().DiceRolledValue;
                }
                else
                {
                    SelectedGrid.GetComponent<GridPositionScript>().ArtifactChildModifier = 0;
                }
            }
        }
    }

    //THIS NEEDS TO BE A COROUTINE WITH A SLIGHT DELAY AT THE BEGINNING
    public GameObject SelectRandomGrid(GameObject[] grids)
    {
        var availableGrids = grids.Where(grid => grid.GetComponent<GridPositionScript>().ArtifactCandidate != true);
        GameObject selectedGrid = availableGrids.ElementAt(Random.Range(0, availableGrids.Count()));
        selectedGrid.GetComponent<GridPositionScript>().ArtifactCandidate = true;
        return selectedGrid;
    }

    public GameObject SelectRandomGrid2(GameObject[] grids)
    {
        var availableGrids2 = grids.Where(grid => grid.GetComponent<GridPositionScript>().ArtifactCandidate != true);
        GameObject selectedGrid = availableGrids2.ElementAt(Random.Range(0, availableGrids2.Count()));
        selectedGrid.GetComponent<GridPositionScript>().ArtifactCandidate = true;
        return selectedGrid;
    }

    public void ExecuteGridCoroutine()
    {
        StartCoroutine(SelectRandomGridCoroutine());
    }

    IEnumerator SelectRandomGridCoroutine()
{
    yield return new WaitForSeconds(3f);
    SelectedGrid = SelectRandomGrid(AdjacentGrids);
    while (SelectedGrid == null)
    {
        yield return null;
    }
    yield return new WaitForSeconds(3f);
}
}
