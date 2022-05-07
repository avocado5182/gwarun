using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    public PlayerMovement playerMovement;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Untagged") || other.CompareTag("Section")) return;
        if (other.CompareTag("Coin")) {
            // Destroy(other.gameObject);
            GameManager.Instance.coins++;
            
            TMP_Text coinsText = GameManager.Instance.uiManager.coinText;
            coinsText.text = GameManager.Instance.coins.ToString();
            // GameManager.Instance.uiManager.UpdateCurrencyText(coinsText, "", GameManager.Instance.coins);
            other.GetComponent<MeshRenderer>().enabled = false;
        }
        if (other.CompareTag("GoldCoin")) {
            // Destroy(other.gameObject);
            GameManager.Instance.coins += 50;
            
            TMP_Text coinsText = GameManager.Instance.uiManager.coinText;
            coinsText.text = GameManager.Instance.coins.ToString();
            // GameManager.Instance.uiManager.UpdateCurrencyText(coinsText, "", GameManager.Instance.coins);
            other.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (other.CompareTag("Score")) {
            // add one to the score
            GameManager.Instance.score += GameManager.Instance.scoreAmt;
            
            TMP_Text scoreText = GameManager.Instance.uiManager.scoreText;
            // scoreText.text = GameManager.Instance.score.ToString();
            GameManager.Instance.uiManager.UpdateCurrencyText(scoreText, "score", GameManager.Instance.score);

            GameManager.Instance.SpawnRandomObstacle();
            GameManager.Instance.passedObsts++;

            ScorePlane sp = other.GetComponent<ScorePlane>(); 
            sp.scored = true;
            // if (sp.NoSiblings()) Destroy(other.transform.parent.gameObject);
        }
        else if (other.CompareTag("SpawnPlane")) {
            // spawn new section/base
            GameManager.Instance.SpawnRandomSection();
        }
        else if (other.CompareTag("Obstacle")) {
            playerMovement.movementIsEnabled = false;

            if (GameManager.Instance.gameEnded) return;
            
            // show retry screen
            GameManager.Instance.EndGame();

            GameManager.Instance.gameEnded = true;
        }
    }
}