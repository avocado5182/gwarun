using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
public class DoubleCoinsAdButton : MonoBehaviour, IUnityAdsListener {

    #if UNITY_IOS
        string gameId = "4735526";
        public string baPlacementId = "Double_Coins_iOS";
    #elif UNITY_ANDROID
        string gameId = "4735527";
        public string dcPlacementId = "Double_Coins_Android";
    #endif

    Button btn;
    public bool hasCompleted;
    public bool testMode;

    void Start () {   
        btn = GetComponent <Button> ();

        // Set interactivity to be dependent on the Placement’s status:
        btn.interactable = Advertisement.IsReady (dcPlacementId); 

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (btn) btn.onClick.AddListener (ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo () {
        GameManager.Instance.uiManager.doubleCoinsBtn.interactable = false;
        Advertisement.Show (dcPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == dcPlacementId && 
            GameManager.Instance.coins > 0 &&
            !hasCompleted) {        
            btn.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            // Reward the user for watching the ad to completion.
            if (placementId == dcPlacementId && !hasCompleted) {
                Debug.Log("ad watched to completion!!");
                hasCompleted = true;
                GameManager gm = GameManager.Instance;
                if (gm.coins > 0) {
                    // then we can double (note that ui has already been showed and values have updated)
                    gm.data.coins += gm.coins;
                    gm.coins *= 2;
                    gm.uiManager.coinAmtText.text = $"coins collected: {gm.coins}\n total coins: {gm.data.coins}";
                }
                SaveSystem.SaveData(gm.data, GameManager.savePath);
                Debug.Log(SaveSystem.LoadData<PlayerData>(GameManager.savePath).coins);
            }
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
            Debug.Log("ad skipped :(");
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}
