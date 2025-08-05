using UnityEngine;
using UnityEngine.UI;

public class ScrollableBackground : MonoBehaviour
{
    public RectTransform achievements_contentRectTransform;  // Achievements Content RectTransform
    public RectTransform shop_contentRectTransform;  // Shop Content RectTransform
    public RectTransform achievements_backgroundRectTransform; // Achievements Background RectTransform
    public RectTransform shop_backgroundRectTransform; // Shop Background RectTransform
    public Vector2 scrollSpeed = new Vector2(1f, 1f);  // Speed of background scrolling

    private Vector2 achievements_initialContentPosition, shop_initialContentPosition;

    void Start()
    {
        achievements_initialContentPosition = achievements_contentRectTransform.anchoredPosition;
        shop_initialContentPosition = shop_contentRectTransform.anchoredPosition;
    }

    void Update()
    {
        // Calculate the difference in the content's position
        Vector2 achievementsMovement = achievements_contentRectTransform.anchoredPosition - achievements_initialContentPosition;
        Vector2 shopMovement = shop_contentRectTransform.anchoredPosition - shop_initialContentPosition;

        // Move the background accordingly (you can adjust this for parallax effects or different speeds)
        achievements_backgroundRectTransform.anchoredPosition = achievementsMovement * scrollSpeed;
        shop_backgroundRectTransform.anchoredPosition = shopMovement * scrollSpeed;

        // Optional: If you want to reset initial position based on content position
        // initialContentPosition = contentRectTransform.anchoredPosition;
    }
}