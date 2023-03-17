using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuScript : MonoBehaviour
{
    public GameObject menuScreen;
    public float scaleDuration = 0.5f;
    public Vector3 targetScale = new Vector3(1.0f, 1.0f, 1.0f);

    private Vector3 initialScale;
    private float elapsedTime = 0.0f;
    private bool isScaling = false;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = menuScreen.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScaling)
        {
            // Calculate the new scale using Lerp function
            float t = elapsedTime / scaleDuration;
            menuScreen.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // Update elapsed time
            elapsedTime += Time.deltaTime;

            // If scaling is completed, stop scaling and update the isScaling flag
            if (t >= 1.0f)
            {
                isScaling = false;
            }
        }
    }

    public void OnMenuButtonClick()
      {
        if (isScaling)
        {
            // If the menu screen is currently scaling up, stop scaling and reset the scale to the initial scale
            isScaling = false;
            menuScreen.transform.localScale = initialScale;
        }
        else
        {
            // Otherwise, start scaling up the menu screen from its initial scale to the target scale
            initialScale = menuScreen.transform.localScale;
            if (initialScale == Vector3.zero)
            {
                targetScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {
                targetScale = Vector3.zero;
            }
            isScaling = true;
            elapsedTime = 0.0f;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("GameOver");
    }
}
