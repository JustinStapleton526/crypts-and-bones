using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.05f; // Increase the scale by 5%
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // Reset the scale to its original value
    }
}