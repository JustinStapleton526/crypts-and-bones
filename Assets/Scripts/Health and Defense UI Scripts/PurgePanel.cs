using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgePanel : MonoBehaviour
{
    GameManager GameManager;
    DiceBagScript DiceBagScript;
    SoundManager SoundManager;

    [Header("Audio reference")]
    [SerializeField] private AudioClip _UiClick;
    [SerializeField] private AudioClip _UiErrorClick;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindObjectOfType<GameManager>();
        DiceBagScript = GameObject.FindObjectOfType<DiceBagScript>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    void OnMouseDown()
    {
        if (DiceBagScript.diceBag.Count <= 5)
        {
            SoundManager.Instance.PlayLoudSound(_UiErrorClick);
            return;
        }
        else
        {
        SoundManager.Instance.PlayLoudSound(_UiClick);
           StartCoroutine(PurgePanelOrderOfOperations_2()); 
        }
        
    }

    IEnumerator PurgePanelOrderOfOperations_1()
    {
        GameManager.StateChangeToPurgeState();
        yield return null;
    }
    IEnumerator PurgePanelOrderOfOperations_2()
    {
        yield return new WaitForSeconds(.5f);
        GameManager.MovePurgeRewardsOutOfView();
        GameManager.MoveUpgradeRewardsOutOfView();
        yield return StartCoroutine(PurgePanelOrderOfOperations_1());
    }

}
