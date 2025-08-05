using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class DynamicPaddingByHeight : MonoBehaviour
{
    private RectTransform parentRect;
    private LayoutGroup layoutGroup;

    [Header("Reference Settings")]
    public float referenceHeight = 449.9109f;
    public float referenceTopPadding = 12f;
    public float referenceBottomPadding = 44f;

    [Header("Optional Limits")]
    public float minPadding = 0f;
    public float maxPaddingMultiplier = 2f; // Top could be 24, Bottom 88 at 2x height

    void Awake()
    {
        layoutGroup = GetComponent<LayoutGroup>();
        parentRect = transform.parent.GetComponent<RectTransform>();

        if (parentRect == null)
        {
            Debug.LogError("DynamicPaddingByHeight: Parent RectTransform not found.");
            enabled = false;
        }
    }

    void LateUpdate()
    {
        float currentHeight = parentRect.rect.height;

        // Calculate how much larger or smaller the height is compared to the reference
        float scale = currentHeight / referenceHeight;

        // Clamp scale to prevent excessive padding growth or shrink
        scale = Mathf.Clamp(scale, 0f, maxPaddingMultiplier);

        // Calculate new padding values
        int topPadding = Mathf.RoundToInt(referenceTopPadding * scale);
        int bottomPadding = Mathf.RoundToInt(referenceBottomPadding * scale);

        // Apply safe minimums
        topPadding = Mathf.Max(topPadding, (int)minPadding);
        bottomPadding = Mathf.Max(bottomPadding, (int)minPadding);

        // Update only if values changed
        if (layoutGroup.padding.top != topPadding || layoutGroup.padding.bottom != bottomPadding)
        {
            layoutGroup.padding.top = topPadding;
            layoutGroup.padding.bottom = bottomPadding;

            LayoutRebuilder.MarkLayoutForRebuild(layoutGroup.GetComponent<RectTransform>());
        }
    }
}
