using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImagePressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = defaultSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        img.sprite = pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        img.sprite = defaultSprite;
    }
}