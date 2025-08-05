using UnityEngine;
using System.Collections;

public class AdventureSlide : MonoBehaviour
{
    public RectTransform currentSwitch; // visible at start (LOCATIONS/STAGES)
    public RectTransform nextSwitch;    // starts offscreen right (LOCATIONS/STAGES)
    public RectTransform currentPanel; // visible at start (MENU ADVENTURE)
    public RectTransform nextPanel;    // starts offscreen right (MENU ADVENTURE)
    public float slideDuration = 0.5f;

    private float screenWidth;
    private float switchWidth;
    private RectTransform previousPanel;
    private RectTransform previousSwitch;

    private void Start()
    {
        screenWidth = GetCanvasWidth();

        if (nextPanel != null)
        {
            // Start next panel offscreen to the right
            nextPanel.anchoredPosition = new Vector2(screenWidth, 0);
        }

        switchWidth = GetContainerWidth();

        if (nextSwitch != null)
        {
            // Start next panel offscreen to the right
            nextSwitch.anchoredPosition = new Vector2(switchWidth, 0);
        }
    }

    public void SlideForward() //MENU SWITCH TO ADVENTURE
    {
        screenWidth = GetCanvasWidth();
        previousPanel = currentPanel;

        StartCoroutine(SlidePanels(
            from: currentPanel,
            fromEnd: new Vector2(-screenWidth, 0),         // slide left
            to: nextPanel,
            toStart: new Vector2(screenWidth, 0),          // start from right
            toEnd: Vector2.zero,
            onComplete: () =>
            {
                currentPanel = nextPanel;   // Now next becomes current
            }));
    }

    public void SlideBackward() //MENU SWITCH TO ADVENTURE
    {
        screenWidth = GetCanvasWidth();

        if (previousPanel == null)
        {
            Debug.LogWarning("No previous panel stored. Can't slide backward.");
            return;
        }

        StartCoroutine(SlidePanels(
            from: currentPanel,
            fromEnd: new Vector2(screenWidth, 0),          // slide right
            to: previousPanel,
            toStart: new Vector2(-screenWidth, 0),         // start from left
            toEnd: Vector2.zero,
            onComplete: () =>
            {
                nextPanel = currentPanel;           // move current to next
                currentPanel = previousPanel;       // previous becomes current
                previousPanel = null;               // clear
            }));

        SwitchLocation();
    }

    public void SwitchStage() //ADVENTURE LOCATION SWITCH TO STAGES
    {
        switchWidth = GetContainerWidth();
        previousSwitch = currentSwitch;

        StartCoroutine(SlideSwitch(
            from: currentSwitch,
            fromEnd: new Vector2(-switchWidth, 0),         // slide left
            to: nextSwitch,
            toStart: new Vector2(switchWidth, 0),          // start from right
            toEnd: Vector2.zero,
            onComplete: () =>
            {
                currentSwitch = nextSwitch;
            }));
    }

    public void SwitchLocation() //ADVENTURE STAGES SWITCH TO LOCATION
    {
        switchWidth = GetContainerWidth();

        if (previousSwitch == null)
        {
            Debug.LogWarning("No previous panel stored. Can't slide backward.");
            return;
        }

        StartCoroutine(SlideSwitch(
            from: currentSwitch,
            fromEnd: new Vector2(switchWidth, 0),          // slide right
            to: previousSwitch,
            toStart: new Vector2(-switchWidth, 0),         // start from left
            toEnd: Vector2.zero,
            onComplete: () =>
            {
                nextSwitch = currentSwitch;
                currentSwitch = previousSwitch;
                previousSwitch = null;
            }));
    }

    IEnumerator SlidePanels(RectTransform from, Vector2 fromEnd,
                        RectTransform to, Vector2 toStart, Vector2 toEnd,
                        System.Action onComplete)
    {
        float time = 0f;
        Vector2 fromStart = from.anchoredPosition;

        from.gameObject.SetActive(true);
        to.gameObject.SetActive(true);

        to.anchoredPosition = toStart; // Set up the "to" panel to start offscreen

        while (time < slideDuration)
        {
            float t = time / slideDuration;
            float easedT = EaseInOutCubic(t); // both panels use the same eased time

            from.anchoredPosition = Vector2.Lerp(fromStart, fromEnd, easedT);
            to.anchoredPosition = Vector2.Lerp(toStart, toEnd, easedT);

            time += Time.deltaTime;
            yield return null;
        }

        from.anchoredPosition = fromEnd;
        to.anchoredPosition = toEnd;

        from.gameObject.SetActive(false); // hide the old panel
        onComplete?.Invoke();
    }

    IEnumerator SlideSwitch(RectTransform from, Vector2 fromEnd,
                        RectTransform to, Vector2 toStart, Vector2 toEnd,
                        System.Action onComplete)
    {
        float time = 0f;
        Vector2 fromStart = from.anchoredPosition;

        from.gameObject.SetActive(true);
        to.gameObject.SetActive(true);

        to.anchoredPosition = toStart; // Set up the "to" panel to start offscreen

        while (time < slideDuration)
        {
            float t = time / slideDuration;
            float easedT = EaseInOutCubic(t); // both panels use the same eased time

            from.anchoredPosition = Vector2.Lerp(fromStart, fromEnd, easedT);
            to.anchoredPosition = Vector2.Lerp(toStart, toEnd, easedT);

            time += Time.deltaTime;
            yield return null;
        }

        from.anchoredPosition = fromEnd;
        to.anchoredPosition = toEnd;

        from.gameObject.SetActive(false); // hide the old panel
        onComplete?.Invoke();
    }

    float GetCanvasWidth() //FOR MAINMENU
    {
        Canvas canvas = currentPanel.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        return canvasRect.rect.width;
    }

    float GetContainerWidth() //FOR ADVENTURE
    {
        return currentSwitch.transform.parent.GetComponent<RectTransform>().rect.width;
    }

    // Cubic easing function that eases in during the first half and eases out during the second half.
    float EaseInOutCubic(float t)
    {
        return t < 0.5f
            ? 4f * t * t * t //ease-in: the animation starts slowly and speeds up toward the middle.
                             // - Cubing makes it start slowly (0^3 = 0, 0.5^3 = 0.125), producing acceleration.
                             // - Multiplying by 4 ensures the midpoint lands at exactly 0.5 (so the whole curve is continuous).
            : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f; //ease-out: the animation starts quickly and slows down toward the end.
                                                     // 1. -2 * t + 2 maps t from [0.5, 1.0] → [1.0, 0.0]
                                                     //    This mirrors the second half, so we can reapply the cubic curve in reverse.
                                                     // 2. Raise the mirrored time to the power of 3 (cube it) to apply the same acceleration curve.
                                                     //    This results in a decelerating movement since t is decreasing.
                                                     // 3. Subtract from 1 to flip the result back (since it's an ease-out).
                                                     // 4. Divide by 2 to scale the second half properly (so the total output range remains [0, 1]).
    }
}