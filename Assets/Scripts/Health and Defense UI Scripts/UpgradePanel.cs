using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    GameManager GameManager;
    SoundManager SoundManager;

    [Header("Audio reference")]
    [SerializeField] private AudioClip _UiClick;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindObjectOfType<GameManager>();
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    void OnMouseDown()
    {
        SoundManager.Instance.PlayLoudSound(_UiClick);
        StartCoroutine(PurgePanelOrderOfOperations_2());
    }

    IEnumerator PurgePanelOrderOfOperations_1()
    {
        GameManager.StateChangeToUpgradeState();
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
