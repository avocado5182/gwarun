using UnityEngine;
using UnityEngine.UI;

// credit https://www.youtube.com/watch?v=VprqsEsFb5w
public class SafeArea : MonoBehaviour {
    public Image topBar;
    RectTransform rectTransform;
    Rect safeArea;
    Vector2 minAnchor;
    Vector2 maxAnchor;
    
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        
        float topBarHeight = Screen.height - safeArea.size.y;

        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -topBarHeight);

        topBar.rectTransform.anchorMin = new Vector2(0.5f, 1f);
        topBar.rectTransform.anchorMax = new Vector2(0.5f, 1f);
        topBar.rectTransform.pivot = new Vector2(0.5f, 1f);

        topBar.rectTransform.anchoredPosition = Vector2.zero;
        topBar.rectTransform.sizeDelta = new Vector2(Screen.width, topBarHeight);
    }
}
