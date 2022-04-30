using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class gwaSkins {
    public static List<gwaSkin> List = new List<gwaSkin> {
        gwaSkin.Truegwa,
        gwaSkin.gwaBlob,
        gwaSkin.gwaTroll,
        gwaSkin.gwaLeft
    };
}

[Serializable]
public class PlayerData {
    public int coins = 0;
    public int highScore = 0;
    public int[] unlockedSkins;
    public int equippedSkin;

    public static int GetSkinId(gwaSkin skin) {
        return gwaSkins.List.FindIndex(s => s == skin);
    }
    
    public PlayerData() {
        coins = 0;
        highScore = 0;
        
        unlockedSkins = new int[] { 0 };
        equippedSkin = 0;
    }

    public bool BuySkin(int skinId) {
        try {
            List<gwaSkin> allSkins = gwaSkins.List;
            gwaSkin skinToBuy = allSkins[skinId] ?? null;
            Debug.Log($"coins{coins} skinToBuy{skinToBuy}");
            if (skinToBuy == null) return false; // rip
            return coins >= skinToBuy.cost; // we bought the skin
        }
        catch (Exception e) {
            Debug.Log($"rip error");
            Debug.LogError(e);
            return false;
            // throw new Exception($"{e.Message}"); // idk what to put here
        }
    }
}
