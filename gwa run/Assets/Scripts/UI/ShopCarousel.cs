using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopCarousel : MonoBehaviour {
    public UIManager uiMgr;
    public Transform skinsContainer;
    public GameObject skinPreviewPrefab;
    public RectTransform carouselContainer;
    public float skinDistance = 480f; // x distance between skins in the shop
    public float previewWidth = 1000f;

    List<gwaSkin> skins = new List<gwaSkin>();
    int currentIndex = 0;

    void Awake() {
        skins = GameManager.Instance.skins;
        currentIndex = 0;
    }

    // Start is called before the first frame update
    void Start() {
        skinDistance = Display.main.renderingWidth * 1.5f; // on low widths, just 1 x the width doesn't work????
        previewWidth = Display.main.renderingWidth / 2.667f;
        InitCarousel();
        RenderCarousel(currentIndex);
    }

    // works with negative numbers (in c#, -1 % 3 returns -1 but this returns 2)
    int mod(int k, int n) {
        return ((k %= n) < 0) ? k + n : k;
    }
    
    int BoundIndex<T>(IList<T> list, int index, bool direction) {
        if (list.Count == 0) throw new IndexOutOfRangeException("List passed into BoundIndex()'s count is 0");
        if (list.Count == 1) return 0;
        int returnIndex = mod(index + (direction ? 1 : -1), list.Count);
        return returnIndex;
    }

    public void PreviousElement() {
        currentIndex = BoundIndex(skins, currentIndex, false);
        RenderCarousel(currentIndex);
    }
    
    public void NextElement() {
        currentIndex = BoundIndex(skins, currentIndex, true);
        RenderCarousel(currentIndex);
    }

    public void ReloadCarousel(int fromIndex) {
        InitCarousel();
        RenderCarousel(fromIndex);
    }
    
    void InitCarousel() {
        // delete all (now old) previews to reload
        if (skinsContainer.childCount != 0)
            foreach (Transform preview in skinsContainer.transform) Destroy(preview.gameObject);

        for (int i = 0; i < skins.Count; i++) {
            gwaSkin skin = skins[i];
            // // float previewXPos = (i * previewWidth) + (skinDistance * i);
            // float previewXPos = i * (previewWidth + skinDistance);
            
            GameObject skinPreview = Instantiate(skinPreviewPrefab, new Vector3(), Quaternion.identity);
            skinPreview.name = $"Skin Preview for {skin.skinName}";
            RectTransform rectTransform = skinPreview.GetComponent<RectTransform>();
            rectTransform.SetParent(skinsContainer, false);

            rectTransform.localScale = Vector3.one;
            
            // update skin preview fields(?)(i.e. name text, preview texture, etc)
            SkinPreview preview = skinPreview.GetComponent<SkinPreview>();
            preview.UpdateNameText($"{skin.skinName}");
            if (SaveSystem.SaveExists(GameManager.savePath)) {
                // PlayerData savedData = SaveSystem.LoadData<PlayerData>(GameManager.savePath);
                GameManager.Instance.LoadData(GameManager.savePath);
                var savedData = GameManager.Instance.data;
                bool isUnlocked = Array.Exists(savedData.unlockedSkins, unlockedSkin => unlockedSkin == skin.id);
                bool isEquipped = isUnlocked && savedData.equippedSkin == skin.id;

                // Debug.Log($"{skin.skinName}: {(isUnlocked ? (isEquipped ? "Unlocked and Equipped" : "Unlocked") : "To buy")}");
                preview.UpdateSkinCostText(
                    isUnlocked
                        ? ((isEquipped) ? "equipped" : "equip")
                        : $"{skin.cost} coins"
                );
                
                preview.UpdateButtonColor(
                    isUnlocked
                        ? ((isEquipped) ? Color.green : Color.blue)
                        : Color.red
                );

                // int skinId = i;
                int skinId = skin.id;
                var i1 = i;
                SkinPreviewPurchaseButton btn = preview.purchaseSkinBtn.GetComponent<SkinPreviewPurchaseButton>();
                btn.i = i1;
                btn.previewSkinId = skinId;
                btn.skin = skin;
                btn.sc = this;
                btn.preview = preview;
            }
            else preview.UpdateSkinCostText(skin.cost);

            preview.UpdateSkinPreviewTexture(skin.shopPreview);
        }
    }

    int IndexRelativeToIndex(int indx1, int indx2, int listLength) {
        // indx1 relative to indx2;
        // i.e. IndexRelativeToIndex(1, 2, 3) = 2
        //      |-*-----*-|
        //      |-2--0--1-| // indexes relative to 2(indx2) // also list length = 3
        //      |-???--???--???-| 
        //      |-0--1--2-| // indexes starting from 0(constant)
        //      |-------*-|
        
        //      IndexRelativeToIndex(0, 1, 3) = 2
        //      |-*-----*-|
        //      |-1--2--0-| // indexes relative to 1(indx2)
        //      |-???--???--???-| 
        //      |-0--1--2-| // indexes relative to 0(constant)
        //      |-------*-|
        
        //      IndexRelativeToIndex(0, 2, 5) = 3
        //      |-*--------*----|
        //      |-2--3--4--0--1-| // indexes relative to 1(indx2)
        //      |-???--???--???--???--???-| 
        //      |-0--1--2--3--4-| // indexes relative to 0(constant)
        //      |----------*----|
        
        // list length - indx2 + indx1 ????
        
        //      IndexRelativeToIndex(6, 1, 10) = 5
        //      |-*--------------*-------------|
        //      |-1--2--3--4--5--6--7--8--9--0-| // indexes relative to 1(indx2)
        //      |-???--???--???--???--???--???--???--???--???--???-| 
        //      |-0--1--2--3--4--5--6--7--8--9-| // indexes relative to 0(constant)
        //      |----------------*-------------|
        // 10 - 1 + 6
        // 15
        // but, 15 % 10 = 5 (our answer) so it could just be the above but % list length
        // (list length - indx2 + indx1) % list length
        return (listLength - indx2 + indx1) % listLength;
    }
    
    void RenderCarousel(int indexToRender) {
        for (int i = 0; i < skins.Count; i++) {
            // relative to the indexToRender!!!!
            // the above i is for positioning

            // indx1 = (indexToRender + i) % gwaSkins.Count
            // indx2 = indexToRender
            int relativeIndex = IndexRelativeToIndex(i, indexToRender, skins.Count);
            List<RectTransform> objTransforms = new List<RectTransform>();
            foreach (Transform child in skinsContainer)
                objTransforms.Add(child.GetComponent<RectTransform>());

            for (int j = 0; j < objTransforms.Count; j++) {
                RectTransform objTransform = objTransforms[j];
                float newXPos = (j + relativeIndex + 1) % (objTransforms.Count) * (previewWidth + skinDistance);
                objTransform.localPosition = new Vector3(newXPos, 0, 0);
            }
        }
    }
}