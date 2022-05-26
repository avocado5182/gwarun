using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header("Menu objects")]
    public List<CanvasGroup> mainMenuMenus = new List<CanvasGroup>();
    public Animator gwaAnimator;

    [Header("In-Game objects")]
    public CanvasGroup pauseMenu;
    public CanvasGroup inGameMenu;
    public CanvasGroup retryMenu;
    public Button retryButton;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text coinText; // in-game
    public TMP_Text gameOverText;
    public Transform powerupIconContainer;
    public GameObject powerupIconPrefab;
    public TMP_Text coinAmtText;
    public TMP_Text coinTxtPrefab; // +50 whenever gold coin collected
    public Transform coinTxtContainer;
    public Button doubleCoinsBtn;
    
    [Header("Main Menu objects")]
    public TMP_Text shopCoinText; // in the shop
    public ShopBannerAds shopAds;
    public GameObject menugwaModel;
    public RectTransform shopSafezone;

    [Header("Game Over Messages")] 
    public List<string> gameOverMsgs;
    
    public void LoadNextScene() {
        int index = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        if (index == 1) {
            Debug.Log(Analytics.CustomEvent("StartGame")); 
        }
        LoadScene(index);
    }

    public void SaveAudioVolumes() {
        // save volumes as dB value
        BGMusic bgm = BGMusic.Instance;
        bgm.musicMixer.audioMixer.GetFloat("Volume", out float musicVol);
        bgm.soundMixer.audioMixer.GetFloat("Volume", out float soundVol);
        PlayerPrefs.SetFloat("musicvol", musicVol);
        PlayerPrefs.SetFloat("soundvol", soundVol);
        PlayerPrefs.Save();
    }

    public void GoToMenuScene() {
        LoadScene(0);
    }
    
    public void LoadScene(int sceneIndex) {
        GameManager.Instance.LoadData(GameManager.savePath);
        var loadedData = GameManager.Instance.data;
        if (GameManager.Instance.data.coins != 0 && GameManager.Instance.data.coins !=
            loadedData.coins) {
            SaveSystem.SaveData(GameManager.Instance.data, GameManager.savePath);
        }
        
        SaveAudioVolumes();

        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1f;
    }
    
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void PauseUnpause(bool pause) {
        GameManager.Instance.paused = pause;
        pauseMenu.alpha = Convert.ToSingle(pause);
        pauseMenu.interactable = pause;
        Time.timeScale = Convert.ToSingle(!pause);
    }

    public void ShowRetryScreen() {
        UpdateRetryScreen(true);
    }
    public void UpdateRetryScreen(bool show) {
        GameManager gm = GameManager.Instance;
        
        // save the high score
        if (SaveSystem.SaveExists(GameManager.savePath)) {
            // Debug.Log("save exists");
            gm.LoadData(GameManager.savePath);
            var savedData = gm.data;
            gm.data.unlockedSkins = savedData.unlockedSkins;
            gm.data.equippedSkin = savedData.equippedSkin;
            // Debug.Log(gameMgr.data.highScore);
            // Debug.Log(gameMgr.score);
            bool newHigh = (gm.score > savedData.highScore);
            gm.data.highScore = newHigh ? gm.score : savedData.highScore;
            highScoreText.text =
                $"{(newHigh ? "new " : "")}high score: {(newHigh ? $"{gm.score.ToString()}!!!!" : $"{savedData.highScore.ToString()}\nscore: {gm.score}")}";
            // Debug.Log(gameMgr.data.highScore);
            
            int totalCoins = savedData.coins + gm.coins;
            gm.data.coins = totalCoins;
        }
        else {
            Debug.Log("save doesn't exist");
            // save high score
            if (gm.score > 0) {
                highScoreText.text = $"new high score: {gm.score}!!!!";
                gm.data.highScore = gm.score;
            }
                
            // save coins
            gm.data.coins = gm.coins;
        }

        bool interactable = doubleCoinsBtn.interactable;
        doubleCoinsBtn.interactable = interactable && gm.coins > 0;
        
        // UpdateCurrencyText(coinAmtText, "coins", gameMgr.data.coins);
        coinAmtText.text = $"{(gm.coins > 0 ? $"coins collected: {gm.coins}\n" : "")} total coins: {gm.data.coins}";
        
        SaveSystem.SaveData(gm.data, GameManager.savePath);
        // Debug.Log(SaveSystem.LoadData<PlayerData>(gameMgr.savePath).highScore);
        // Debug.Log(gameMgr.data.highScore);
        
        UpdateMenu(retryMenu, show);
        UpdateMenu(inGameMenu, !show);
        Time.timeScale = Convert.ToSingle(!show);
    }

    // for score, coins, etc.
    public void UpdateCurrencyText(TMP_Text text, string currency, int value) {
        text.text = $"{currency}: {value}";
    }

    // public void OpengwaMenu() {
    //     ShowMenuMenu(2);
    //     
    //     // trigger the trigger
    //     gwaAnimator.SetTrigger("fadeingwa");
    // }

    public void OpenShop() {
        ShowMenuMenu(1);
        
        // update coin text
        int coinAmt = 0;
        if (SaveSystem.SaveExists(GameManager.savePath)) {
            // PlayerData savedData = SaveSystem.LoadData<PlayerData>(GameManager.savePath);
            GameManager.Instance.LoadData(GameManager.savePath);
            var savedData = GameManager.Instance.data;
            coinAmt = savedData.coins;
        }

        UpdateShopCoinText(coinAmt);
        
        // show banner ad
        shopAds.LoadBanner();
        shopAds.ShowBannerAd();
    }

    public void OpenOptions_Menu() {
        ShowMenuMenu(2);
    }

    public void UpdateShopCoinText(int amount) {
        shopCoinText.text = $"coins: {amount}";
    }

    public void UpdateMenu(CanvasGroup menu, bool show=true) {
        menu.GetComponentInChildren<Transform>().gameObject.SetActive(show);
        menu.alpha = Convert.ToSingle(show);
        menu.interactable = show;
        menu.blocksRaycasts = show;
    }

    public void ShowMenuMenu(int index) {
        for (int i = 0; i < mainMenuMenus.Count; i++) {
            CanvasGroup menu = mainMenuMenus[i];

            bool isMenuToShow = (i == index);
            UpdateMenu(menu, isMenuToShow);
            menugwaModel.SetActive(index == 0);
        }

        if (index != 1) shopAds.HideBannerAd();
    }
    
    public void HideMenuMenu(int index) {
        CanvasGroup menu = mainMenuMenus[index];

        UpdateMenu(menu, false);
    }

    public IEnumerator MakeCoinText(Vector3 pos, int coins, Transform parent) {
        TMP_Text txt = Instantiate(coinTxtPrefab, parent);
        txt.transform.localPosition = pos;
        txt.transform.rotation = Quaternion.identity;
        txt.text = $"+{coins}";
        float t = 0;
        float txtSpeed = 1f;
        float time = 1f;
        float tMult = 2f;
        while (t < time) {
            // Vector3 txtPos = txt.rectTransform.localPosition;
            // txt.rectTransform.localPosition = new Vector3(
            //     txtPos.x,
            //     txtPos.y + (Time.deltaTime * txtSpeed / time),
            //     txtPos.z
            // );
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, time - (t * tMult));
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(txt.gameObject);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
