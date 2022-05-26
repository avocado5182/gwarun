using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkinPreviewPurchaseButton : MonoBehaviour {
    [Header("Changed at runtime")]
    public SkinPreview preview;
    public ShopCarousel sc;
    public int previewSkinId;
    public int i;
    public gwaSkin skin;
    
    public void OnSkinPurchaseBtnClick() {
        // if coins > cost:
        //     buy();
        //     equip();
        //     replace the button with a new button, text saying "equipped"
        // // else:
        // //     have a tooltip or something saying "you can't afford that!"
        GameManager.Instance.LoadData(GameManager.savePath);
        var savedData = GameManager.Instance.data;
        if (savedData.equippedSkin == previewSkinId) return; // already equipped, no need for listener
        if (savedData.unlockedSkins.Contains(previewSkinId)) { // unlocked, shows "equip", equip this
            // skin is unlocked, make it equip
            savedData.equippedSkin = previewSkinId;
            
            GameManager.Instance.data = savedData;
            SaveSystem.SaveData(GameManager.Instance.data, GameManager.savePath);
            
            // update button to say equipped, update other unlocked ones to say equip
            int fromIndex = i;
            sc.ReloadCarousel(fromIndex);
            sc.uiMgr.UpdateShopCoinText(savedData.coins);

            // Debug.Log($"equipped skin {skin.skinName}");
            // Debug.Log($"(skinName from skinId is {skins[skinId].skinName})");
        }
        else if (savedData.coins >= skin.cost && !savedData.unlockedSkins.Contains(previewSkinId)) {//haven't unlocked this yet, possible to buy
            // buy the skin
            // Debug.Log($"can afford skin {skin.skinName}");
            if (savedData.BuySkin(previewSkinId)) {
                savedData.coins -= skin.cost;
                // Debug.Log($"savedData.coins{savedData.coins}");
                
                // equip it
                // gwaSkin boughtSkin = skins[skinId];
                // Debug.Log($"{ boughtSkin.skinName }, { skinId }");
                // Debug.Log($"[{ string.Join(", ", savedData.unlockedSkins) }]");
                savedData.unlockedSkins = savedData.unlockedSkins.Append(previewSkinId).ToArray();
                // Debug.Log($"[{ string.Join(", ", savedData.unlockedSkins) }]");
                savedData.equippedSkin = previewSkinId;
                
                GameManager.Instance.data = savedData;
                SaveSystem.SaveData(GameManager.Instance.data, GameManager.savePath);
                
                sc.ReloadCarousel(i);
                
                preview.UpdateSkinCostText("equipped");
                preview.UpdateButtonColor(Color.green);
                
                sc.uiMgr.UpdateShopCoinText(savedData.coins);
                
                Debug.Log($"bought skin {skin.skinName}");

                // // these lines are only for debugging loading
                // PlayerData loaded = SaveSystem.LoadData<PlayerData>(GameManager.savePath);
                // Debug.Log($"[{ string.Join(", ", loaded.unlockedSkins) }]");
                
                
                // Debug.Log($"[{ string.Join(", ", savedData.unlockedSkins) }]");
            }
        }
    }
}
