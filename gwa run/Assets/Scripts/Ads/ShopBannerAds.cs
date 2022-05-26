using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

public class ShopBannerAds : MonoBehaviour {

    #if UNITY_IOS
        string gameId = "4735526";
        public string baPlacementId = "Banner_iOS";
    #elif UNITY_ANDROID
        string gameId = "4735527";
        public string baPlacementId = "Banner_Android";
    #endif

    public BannerPosition bp = BannerPosition.BOTTOM_CENTER;
    public bool testMode;

    void Start () {
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(bp);
    }

     // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
 
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(baPlacementId, options);
    }
 
    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");   
    }
 
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }
 
    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
 
        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(baPlacementId, options);
    }
 
    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() {
        Debug.Log("banner clicked");
    }

    void OnBannerShown() {
        Debug.Log("banner shown");
    }

    void OnBannerHidden() {
        Debug.Log("banner hidden");
    }
}
