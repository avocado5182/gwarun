using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "gwaSkin")]
[Serializable]
public class gwaSkin : ScriptableObject {
    public string skinName = "gwa Skin";
    public int id;
    public Mesh mesh;
    public Material[] mats;
    public Texture2D shopPreview;
    public int cost;

    public static gwaSkin Truegwa {
        get {
            gwaSkin skin = Resources.Load<gwaSkin>("gwa Skins/default skin");
            return skin;
        }
    }

    public static gwaSkin gwaBlob {
        get {
            gwaSkin skin = Resources.Load<gwaSkin>("gwa Skins/gwa blob");

            return skin;
        }
    }
    
    public static gwaSkin gwaTroll {
        get {
            gwaSkin skin = Resources.Load<gwaSkin>("gwa Skins/gwa troll");

            return skin;
        }
    }

    public static gwaSkin redgwa {
        get {
            gwaSkin skin = Resources.Load<gwaSkin>("gwa Skins/red gwa");

            return skin;
        }
    }
}
