using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            if (GameManager.magnet) {
                other.transform.position = new Vector3(
                    playerMovement.maxXValue + 2f, // otherwise the coin will stay at the player x and collide forever
                    // (this x position is reset on regeneration)
                    other.transform.position.y,
                    other.transform.position.z
                );
            }
        }
        if (other.CompareTag("GoldCoin")) {
            gm.coins += 50;
            gm.score += gm.scoreAmt;
            gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);
            
            coinsText.text = gm.coins.ToString();
            other.GetComponent<MeshRenderer>().enabled = false;
            // other.transform.position = new Vector3(
            //     playerMovement.maxXValue + 2f,
            //     other.transform.position.y,
            //     other.transform.position.z
            // );
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
            playerMovement.movementIsEnabled = false;

            if (gm.gameEnded) return;
            
            // show retry screen
            gm.EndGame();

            gm.gameEnded = true;
        }
        else if (other.CompareTag("Powerup")) { // tagged is the thing with the collider
            other.GetComponent<MeshRenderer>().enabled = false;

            Powerup p = other.GetComponent<Powerup>();
            if (gm.currPowerup != null && gm.currEffect != null) {
                if (gm.currEffect != p.onEffect) {
                    if (p.name != "Magnet") GameManager.magnet = false;
                    gm.currPowerup.StopRunning();
                }
            } 
            
            gm.currPowerup = p;
            Debug.Log("wasdjfafdssssssssss");
            gm.currEffect = p.onEffect;
            p.onEffect.Invoke(); //mmmm
        }
    }
}