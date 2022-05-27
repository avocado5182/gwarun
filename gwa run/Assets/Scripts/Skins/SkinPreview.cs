using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinPreview : MonoBehaviour {
    public TMP_Text skinNameText;
    public Button purchaseSkinBtn;
    public TMP_Text skinCostText;
    public Image skinImage;
    
    public void SetPreviewPosition(int index, int distance) {
        Vector3 newPos = new Vector3(index * distance, 0, 0);
        SetPreviewPosition(newPos);
    }

    public void SetPreviewPosition(Vector3 position) {
        transform.position = position;
    }


    public void UpdateNameText(string text) {
        skinNameText.text = text;
    }

    public void UpdateSkinPreviewTexture(Texture2D texture) {
        Sprite previewSprite = Sprite.Create(
            texture,
            new Rect(
                new Vector2(
                    0, 
                    transform.parent.GetComponent<RectTransform>().sizeDelta.y * 0.75f
                ), 
                new Vector2(texture.width, texture.height)
            ),
            new Vector2(0.5f, 0.5f)
        );
        previewSprite.name = "Shop Carousel Preview";
        
        // Image skinPreview = gameObject.GetComponent<Image>();
        // skinPreview.sprite = previewSprite;
        skinImage.sprite = previewSprite;
    }

    public void UpdateSkinCostText(int cost) {
        UpdateSkinCostText($"{cost} coins");
    }
    
    public void UpdateSkinCostText(string text) {
        skinCostText.text = text;
    }

    public void UpdateButtonColor(Color color) {
        purchaseSkinBtn.image.color = color;
    }
}
