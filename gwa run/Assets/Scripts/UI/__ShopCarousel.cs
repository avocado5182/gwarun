using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using UnityEditor;

public class ShopCarousel_ : MonoBehaviour {
    public List<gwaSkin> gwaSkins = new List<gwaSkin>();
    public Transform skinsContainer;
    public GameObject skinPreviewPrefab;
    public RectTransform carouselContainer;
    public float skinDistance = 480f; // x distance between skins in the shop
    int currentIndex;
    bool hasRendered = false;
    public float previewWidth = 1000f;

    // Start is called before the first frame update
    void Start() {
        skinDistance = Display.main.renderingWidth * 1.5f; // on low widths just the width doesn't work????
        previewWidth = Display.main.renderingWidth / 2.667f;
        RenderCarousel();
    }

    // works with negative numbers (in c#, -1 % 3 returns -1 but this returns 2)
    int mod(int k, int n) {
        return ((k %= n) < 0) ? k + n : k;
    }
    
    int BoundIndex<T>(IList<T> list, int index, bool direction=true) {
        if (list.Count == 0) throw new IndexOutOfRangeException("List passed into BoundIndex()'s count is 0");
        if (list.Count == 1) return 0;
        int lastIndex = (list.Count - 1);
        int returnIndex = mod((index + lastIndex) + 1, list.Count + (direction ? 0 : -1));
        return returnIndex;
    }

    public void PreviousElement() {
        int newIndex = BoundIndex(gwaSkins, currentIndex - 1);
        // Debug.Log($"currentIndex:{currentIndex}\nnewIndex:{newIndex}");

        bool moving = true;
        bool direction = false; // true is forwards, false is backwards
        // Debug.Log($"moving:{moving}\ndirection:{direction}");
        
        currentIndex = newIndex;
        // Debug.Log($"currentIndex:{currentIndex}");
        RenderCarousel(moving, direction);
    }

    public void NextElement() {
        int newIndex = BoundIndex(gwaSkins, currentIndex + 1);
        // Debug.Log($"currentIndex:{currentIndex}\nnewIndex:{newIndex}");

        bool moving = true;
        bool direction = true; // true is forwards, false is backwards
        // Debug.Log($"moving:{moving}\ndirection:{direction}");
        
        currentIndex = newIndex;
        RenderCarousel(moving, direction);
    }

    public void RenderCarousel() {
        // unity says i can't do optional parameters so here we are
        RenderCarousel(false, false);
    }
    public void RenderCarousel(bool moving, bool direction) {
        if (!hasRendered) {
            hasRendered = true;
            // Debug.Log($"currentIndex: {currentIndex}");
            for (int startingIndex = currentIndex; startingIndex < gwaSkins.Count; startingIndex++) {
                // Debug.Log($"startingIndex: { startingIndex }");
                int boundIndex = BoundIndex(gwaSkins, startingIndex);
                
                #region skin preview
                gwaSkin skin = gwaSkins[startingIndex];
                // GameObject skinPreviewObj = new GameObject();
                // skinPreviewObj.transform.parent = skinsContainer;
                // skinPreviewObj.name = $"{ skin.skinName } {boundIndex}";
                //
                // Texture2D previewTex = skin.shopPreview;
                //
                // Sprite previewSprite = Sprite.Create(
                //     previewTex,
                //     new Rect(new Vector2(0, carouselContainer.sizeDelta.y * 0.75f), new Vector2(previewTex.width, previewTex.height)),
                //     new Vector2(0.5f, 0.5f)
                // );
                // previewSprite.name = "Shop Carousel Preview";
                //
                // Image skinPreview = skinPreviewObj.AddComponent<Image>();
                // skinPreview.sprite = previewSprite;
                // // skinPreview.color = Random.ColorHSV();
                //
                // RectTransform skinPreviewTransform = skinPreviewObj.GetComponent<RectTransform>();
                // float scale = previewWidth / previewTex.width;
                // skinPreviewTransform.localScale = new Vector3(0.5f, 0.5f, 1f);
                // skinPreviewTransform.anchoredPosition = new Vector2(skinDistance * 2 * boundIndex, 0);
                // skinPreviewTransform.sizeDelta = new Vector2(previewTex.width, previewTex.height);
                #endregion
                //
                #region name text
                //
                // GameObject nameTextObj = new GameObject();
                // nameTextObj.name = $"{skin.name} skin text";
                // nameTextObj.transform.parent = skinPreviewTransform;
                //
                // RectTransform nameTextTransform = nameTextObj.AddComponent<RectTransform>();
                //
                // nameTextTransform.anchorMin = new Vector2(0.5f, 0.5f);
                // nameTextTransform.anchorMax = new Vector2(0.5f, 0.5f);
                // nameTextTransform.localScale = Vector3.one;
                // nameTextTransform.sizeDelta = new Vector2(previewTex.width,previewTex.height);
                // nameTextTransform.anchoredPosition = new Vector2(0, previewTex.height);
                //
                // TMP_Text nameText = nameTextObj.AddComponent<TextMeshProUGUI>();
                // nameText.fontSize = 18;
                // nameText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                // nameText.verticalAlignment = VerticalAlignmentOptions.Bottom;
                // nameText.text = skin.name;
                //
                #endregion
                //
                #region purchase button
                //
                // GameObject purchaseBtnObj = new GameObject();
                // purchaseBtnObj.name = "purchase btn";
                // purchaseBtnObj.transform.parent = skinPreviewTransform;
                // RectTransform purchaseBtnTransform = purchaseBtnObj.AddComponent<RectTransform>();
                //
                // purchaseBtnTransform.anchorMin = new Vector2(0.5f, 0.5f);
                // purchaseBtnTransform.anchorMax = new Vector2(0.5f, 0.5f);
                // purchaseBtnTransform.localScale = new Vector2(1, 1);
                // // purchaseBtnTransform.sizeDelta = new Vector2(1, (previewTex.height * 0.375f) * (previewTex.width / previewTex.height));
                // purchaseBtnTransform.sizeDelta = new Vector2(99, previewTex.height/2);
                // purchaseBtnTransform.anchoredPosition = new Vector2(0, previewTex.height * -0.6875f);
                //
                // Button purchaseBtn = purchaseBtnObj.AddComponent<Button>();
                // purchaseBtn.onClick.AddListener(() => Debug.Log($"hi from {skin.name}"));
                //
                #endregion
                //
                #region purchase button text
                //
                // GameObject purchaseBtnTxtObj = new GameObject();
                // purchaseBtnTxtObj.name = "purchase btn text";
                // purchaseBtnTxtObj.transform.parent = purchaseBtnTransform;
                //
                // RectTransform purchaseBtnTxtTransform = purchaseBtnTxtObj.AddComponent<RectTransform>();
                //
                // purchaseBtnTxtTransform.anchorMin = Vector2.zero;
                // purchaseBtnTxtTransform.anchorMax = Vector2.one;
                // purchaseBtnTxtTransform.offsetMin = Vector2.zero;
                // purchaseBtnTxtTransform.offsetMax = Vector2.zero;
                // purchaseBtnTxtTransform.localScale = Vector3.one;
                //
                //
                // TMP_Text purchaseBtnTxt = purchaseBtnTxtObj.AddComponent<TextMeshProUGUI>();
                // purchaseBtnTxt.fontSize = 1;
                // purchaseBtnTxt.horizontalAlignment = HorizontalAlignmentOptions.Center;
                // purchaseBtnTxt.verticalAlignment = VerticalAlignmentOptions.Bottom;
                // purchaseBtnTxt.text = $"{skin.cost} coins";
                //
                #endregion
                
                // instantiate skin preview
                GameObject skinPreview = Instantiate(skinPreviewPrefab, skinsContainer);
                skinPreview.name = $"Skin {skin.skinName}";
                SkinPreview sp = skinPreview.GetComponent<SkinPreview>();

                sp.SetPreviewPosition(new Vector3(skinDistance * 2 * boundIndex, 0, 0));
                sp.UpdateNameText($"{skin.skinName}");
                sp.UpdateSkinPreviewTexture(skin.shopPreview);
                sp.UpdateSkinCostText(skin.cost);
            }
        }
        else {
            // loop through rendered objects
            RectTransform[] renderedObjects = new RectTransform[skinsContainer.childCount];
            for (int i = 0; i < skinsContainer.childCount; i++) {
                renderedObjects[i] = skinsContainer.GetChild(i).GetComponent<RectTransform>();
            }

            // Debug.Log($"currentIndex: {currentIndex}");
            // Debug.Log($"renderedObjects.Length: {renderedObjects.Length}");


            int renderIndex = 0;
            for (int i = currentIndex; i < renderedObjects.Length + currentIndex; i++) {
                int index = i % (renderedObjects.Length);

                RectTransform obj = skinsContainer.GetChild(index).GetComponent<RectTransform>();
                // Debug.Log($"index: {index}");
                // Debug.Log(obj.name);
                
                int newRenderIndex = BoundIndex(renderedObjects, renderIndex + (direction ? 1 : -1));
                // Debug.Log($"index: {index}\nnewRenderIndex:{newRenderIndex}");

                Vector2 objPos = obj.anchoredPosition;
                obj.anchoredPosition = new Vector2(newRenderIndex * skinDistance * 2, objPos.y);

                if (moving) renderIndex = newRenderIndex;
                // Debug.Log($"bound renderIndex:{renderIndex}");
            }
        }
    }
}