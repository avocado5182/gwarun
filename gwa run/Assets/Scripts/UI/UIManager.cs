using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header("Menu objects")]
    public List<CanvasGroup> mainMenuMenus = new List<CanvasGroup>();
    public Animator gwaAnimator;

    
    [Header("In-Game objects")]
    public CanvasGroup pauseMenu;
    public CanvasGroup retryMenu;
    public Button retryButton;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text coinText; // in-game
    public TMP_Text coinAmtText;
    public TMP_Text shopCoinText; // in the shop
    public GameObject menugwaModel;
    
    public void LoadNextScene() {
        int index = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        LoadScene(index);
    }

    // public void ReloadMenuScene() {
    //     // assuming this is on the menu but ykw we'll check
    //     
    //     // if (SceneManager.GetActiveScene().buildIndex == 0) {
    //     //     mainMenu.blocksRaycasts = true;
    //     // }
    //     LoadScene(0);
    // }

    public void GoToMenuScene() {
        LoadScene(0);
    }
    
    public void LoadScene(int sceneIndex) {
        if (GameManager.Instance.data.coins != 0 && GameManager.Instance.data.coins !=
            SaveSystem.LoadData<PlayerData>(GameManager.Instance.savePath).coins) {
            SaveSystem.SaveData(GameManager.Instance.data, GameManager.Instance.savePath);
        } 
        Debug.Log($"{GameManager.Instance.data.coins} in gm, {SaveSystem.LoadData<PlayerData>(GameManager.Instance.savePath).coins} saved");
        Debug.Log($"{GameManager.Instance.data.unlockedSkins.Length} unlocked skins");
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
        // save the high score
        if (SaveSystem.SaveExists(GameManager.Instance.savePath)) {
            Debug.Log("save exists");
            PlayerData savedData = SaveSystem.LoadData<PlayerData>(GameManager.Instance.savePath);
            GameManager.Instance.data.unlockedSkins = savedData.unlockedSkins;
            GameManager.Instance.data.equippedSkin = savedData.equippedSkin;
            // Debug.Log(gameMgr.data.highScore);
            // Debug.Log(gameMgr.score);
            bool newHigh = (GameManager.Instance.score > savedData.highScore);
            GameManager.Instance.data.highScore = newHigh ? GameManager.Instance.score : savedData.highScore;
            highScoreText.text =
                $"{(newHigh ? "new " : "")}high score: {(newHigh ? $"{GameManager.Instance.score.ToString()}!!!!" : $"{savedData.highScore.ToString()}\nscore: {GameManager.Instance.score}")}";
            // Debug.Log(gameMgr.data.highScore);
            
            int totalCoins = savedData.coins + GameManager.Instance.coins;
            GameManager.Instance.data.coins = totalCoins;
        }
        else {
            Debug.Log("save doesn't exist");
            // save high score
            if (GameManager.Instance.score > 0) {
                highScoreText.text = $"new high score: {GameManager.Instance.score}!!!!";
                GameManager.Instance.data.highScore = GameManager.Instance.score;
            }
                
            // save coins
            GameManager.Instance.data.coins = GameManager.Instance.coins;
        }
        
        // UpdateCurrencyText(coinAmtText, "coins", gameMgr.data.coins);
        coinAmtText.text = $"{(GameManager.Instance.coins > 0 ? $"coins collected: {GameManager.Instance.coins}\n" : "")} total coins: {GameManager.Instance.data.coins}";
        
        SaveSystem.SaveData(GameManager.Instance.data, GameManager.Instance.savePath);
        // Debug.Log(SaveSystem.LoadData<PlayerData>(gameMgr.savePath).highScore);
        // Debug.Log(gameMgr.data.highScore);
        
        UpdateMenu(retryMenu, show);
        Time.timeScale = Convert.ToSingle(!show);
    }

    // for score, coins, etc.
    public void UpdateCurrencyText(TMP_Text text, string currency, int value) {
        text.text = $"{currency}: {value}";
    }

    public void OpengwaMenu() {
        ShowMenuMenu(1);
        
        // trigger the trigger
        gwaAnimator.SetTrigger("fadeingwa");
    }

    public void OpenShop() {
        ShowMenuMenu(2);
        int coinAmt = 0;
        if (SaveSystem.SaveExists(GameManager.Instance.savePath)) {
            PlayerData savedData = SaveSystem.LoadData<PlayerData>(GameManager.Instance.savePath);
            coinAmt = savedData.coins;
        }

        shopCoinText.text = $"coins: {coinAmt}";
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
    }
    
    public void HideMenuMenu(int index) {
        CanvasGroup menu = mainMenuMenus[index];

        UpdateMenu(menu, false);
    }

    public static void BuySkin(int skinIndex) {
        
    }
    
    public void QuitGame() {
        Application.Quit();
    }
}
