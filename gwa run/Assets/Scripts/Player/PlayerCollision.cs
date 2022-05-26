using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour {
    public PlayerMovement playerMovement;
    TMP_Text scoreText;
    TMP_Text coinsText;
    GameManager gm;

    void Start() {
        gm = GameManager.Instance;
        coinsText = gm.uiManager.coinText;
        scoreText = gm.uiManager.scoreText;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Untagged") || other.CompareTag("Section")) return;
        if (other.CompareTag("Coin")) {
            gm.coins++;
        
            coinsText.text = gm.coins.ToString();
            other.GetComponent<MeshRenderer>().enabled = false;
        }
        if (other.CompareTag("GoldCoin")) {
            gm.coins += 50;
            gm.score += gm.scoreAmt;
        
            gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);
            coinsText.text = gm.coins.ToString();

            StartCoroutine(gm.uiManager.MakeCoinText(
                new Vector3(90, -65, 0),
                50,
                gm.uiManager.coinTxtContainer));
            
            other.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (other.CompareTag("Score")) {
            ScorePlane sp = other.GetComponent<ScorePlane>();
            gm.score += sp.scoreAmt;
            
            gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);

            gm.SpawnRandomObstacle();
            gm.passedObsts++;
        }
        else if (other.CompareTag("SpawnPlane")) {
            // spawn new section/base
            gm.SpawnRandomSection();
        }
        else if (other.CompareTag("Obstacle")) {
            if (!gm.isStarActive) {
                playerMovement.movementIsEnabled = false;

                if (gm.gameEnded) return;
            
                // show retry screen
                gm.EndGame();

                gm.gameEnded = true;
            }
            else {
                Transform parent = other.transform.parent;
                ScorePlane sp = parent.GetComponentInChildren<ScorePlane>();
                gm.score += sp.scoreAmt;
            
                gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);

                gm.SpawnRandomObstacle();
                gm.passedObsts++;
            }
        }
        else if (other.CompareTag("Powerup")) { // tagged is the thing with the collider
            other.GetComponent<MeshRenderer>().enabled = false;

            Powerup p = other.GetComponent<Powerup>();
            if (gm.currPowerup != null && gm.currEffect != null) {
                if (gm.currEffect != p.onEffect) {
                    Powerup currPowerup = gm.currPowerup;
                    if (currPowerup.name == "Potion") gm.isTimeSlow = false;
                    if (currPowerup.name == "Star") gm.isStarActive = false;
                    
                    // destantiate powerup icon
                    Predicate<GameObject> match = go => go.name.Contains(currPowerup.name);
                    gm.powerupIcons.RemoveAll(match);
                    foreach (Transform child in gm.uiManager.powerupIconContainer)
                        if (match.Invoke(child.gameObject)) Destroy(child.gameObject);
                    
                    gm.currPowerup.StopRunning();
                }
            }

            gm.currPowerup = p;
            gm.currEffect = p.onEffect;
            gm.powerupTimeLeft = p.duration;
            
            // instantiate powerup icon
            GameObject powerupGO = Instantiate(gm.uiManager.powerupIconPrefab);
            powerupGO.transform.SetParent(gm.uiManager.powerupIconContainer, false);
            Canvas.ForceUpdateCanvases();
            gm.powerupIcons.Add(powerupGO);
            Image img = powerupGO.GetComponent<Image>();
            img.sprite = p.effectIcon;
            powerupGO.name = p.name;
            gm.hasSetTime = true;
            
            p.onEffect.Invoke(); //mmmm
        }
    }
} 