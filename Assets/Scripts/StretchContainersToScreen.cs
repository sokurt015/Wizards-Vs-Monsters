using UnityEngine;

public class StretchContainersToScreen : MonoBehaviour
{
    void Start()
    {
        // Array of parent page names
        string[] parentNames = { "Adventure", "Modes", "Achievements", "Shop" };

        foreach (string parentName in parentNames)
        {
            Transform parent = transform.Find(parentName);
            if (parent != null && parent.childCount > 0)
            {
                foreach (Transform child in parent)
                {
                    RectTransform rt = child.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        rt.anchorMin = new Vector2(0, 0);
                        rt.anchorMax = new Vector2(1, 1);
                        rt.offsetMin = Vector2.zero;
                        rt.offsetMax = Vector2.zero;
                        rt.pivot = new Vector2(0.5f, 0.5f);
                        rt.anchoredPosition = Vector2.zero;
                    }
                }
            }
        }
    }
}
