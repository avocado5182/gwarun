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
        }
        if (other.CompareTag("GoldCoin")) {
            gm.coins += 50;
            gm.score += gm.scoreAmt;
            gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);
            
            coinsText.text = gm.coins.ToString();
            other.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (other.CompareTag("Score")) {
            // add one to the score
            gm.score += gm.scoreAmt;
            
            gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);

            gm.SpawnRandomObstacle();
            gm.passedObsts++;

            ScorePlane sp = other.GetComponent<ScorePlane>(); 
            sp.scored = true;
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
        else if (other.CompareTag("Star")) {
            gm.score += gm.scoreAmt * 4; // effectively 500 points, there will be a blip but shhh
            gm.uiManager.UpdateCurrencyText(scoreText, "score", gm.score);
            
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}