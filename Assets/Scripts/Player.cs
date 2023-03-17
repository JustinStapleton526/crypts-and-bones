using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    GameManager GameManager;
    Animator Animator;
    SoundManager SoundManager;

    [Header("Player - Misc Options/Parameters")]
    public int MaxHealth = 20;
    public int Energy = 3;
    public int CurrentHealth;
    public int DefensePoints;
    public int DefensePointsMax = 10;
    public bool RerollAvailable = false;
    public bool Bleeding = false;
    public bool ThornsBuff = false;
    public bool EnergyBuff = false;
    public int poisonDamage = 1;

    [Header("Player - Attack animation operators")]
    public float duration = 1f;
    public float distance = 10f;

    [Header("Player - Animation properties")]
    public GameObject PlayerGameObject;
    public Vector3 CurrentLocation;
    public GameObject FloatingText;

    [Header("Thorns Sprite")]
    public GameObject ThornsSprite;

    [Header("Particle Effects")]
    public GameObject HealthParticle;
    public GameObject DefendParticle;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _takeDamageClip;
    

    private void Start() 
    {
        GameManager = GameObject.FindObjectOfType<GameManager>();
        CurrentHealth = MaxHealth;   
        CurrentLocation = transform.position;
        Application.targetFrameRate = 60;
        Animator = GetComponent<Animator>();
        Animator.SetBool("DeathAnim", false);
        SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    private void Update() 
    {
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        if (DefensePoints >= DefensePointsMax)
        {
            DefensePoints = DefensePointsMax;
        }

        if (CurrentHealth <= 0)
        {
            Death();
        }

        if (ThornsBuff == true)
        {
            ThornsSprite.SetActive(true);
        }
        else
        {
            ThornsSprite.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        ShowDamage(damage.ToString());
        // Subtract damage from defend points first
        DefensePoints -= damage;
        // Instantiate floatingpoints game object for pop up text
        //Instantiate(FloatingText, transform.position, Quaternion.identity);
        // If there are still some points of damage left,
        // subtract the remaining damage from health points
        if (DefensePoints < 0)
        {
            CurrentHealth += DefensePoints;
            DefensePoints = 0;
        }

        // Make sure the health points don't go below zero
        CurrentHealth = Mathf.Max(CurrentHealth, 0);
        SoundManager.Instance.PlayLoudSound(_takeDamageClip);
        AttackAnimationDamaged();
        HealthParticle.SetActive(true);
        if (CurrentHealth == 0)
        {
            Death();
        }
    }

    private void ShowDamage(string text)
    {
        if(FloatingText)
        {
            var floatingTextPosition = new Vector3(transform.position.x,transform.position.y + 2f,transform.position.z - 10f);
            GameObject prefab = Instantiate(FloatingText, floatingTextPosition,Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    private void Death()
    {
            Animator.SetBool("DeathAnim", true);
            Destroy(gameObject, 5f);
            GameManager.StateChangeDefeat();
    }

    public void AttackAnimation()
    {
        Animator.SetBool("isAttacking", false);
        StartCoroutine(WaitForAnimationToFinish());
    }

    IEnumerator WaitForAnimationToFinish()
    {
        // Wait for the length of the animation
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

        // Set the isAttacking parameter back to false
        Animator.SetBool("isAttacking", true);
    }


    public void AttackAnimationForward()
    {
        StartCoroutine(MoveIntoView(PlayerGameObject, new Vector3(CurrentLocation.x + 4f, CurrentLocation.y, 0f), .25f));
        
    }

    public void AttackAnimationUpward()
    {
        StartCoroutine(MoveIntoView(PlayerGameObject, new Vector3(CurrentLocation.x, CurrentLocation.y +1f, 0f), .25f));
    }

    public void AttackAnimationDamaged()
    {
        StartCoroutine(MoveIntoView(PlayerGameObject, new Vector3(CurrentLocation.x-1f, CurrentLocation.y, 0f), .25f));
    }

    IEnumerator MoveIntoView(GameObject objectToMove, Vector3 endPosition, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            Animator.enabled = false;
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, CurrentLocation, (elapsedTime / time));
            yield return new WaitForEndOfFrame();
            Animator.enabled = true;
        }
    }
}
