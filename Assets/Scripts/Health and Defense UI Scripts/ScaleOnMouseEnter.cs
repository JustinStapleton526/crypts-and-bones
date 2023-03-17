using UnityEngine;

public class ScaleOnMouseEnter : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        transform.localScale = originalScale * 1.05f; // Increase the scale by 5%
    }

    private void OnMouseExit()
    {
        transform.localScale = originalScale; // Reset the scale to its original value
    }
}
