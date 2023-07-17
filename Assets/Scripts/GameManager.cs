using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    private int multiplier;

    private int indexPlayer;

    private int playerHealth;

    private int playerIndex = 0;
    private int power;

    private int price;

    private void Start() {
        InitPlayer(playerIndex);
        price = price += GameSetting.Instance.multiplierPriceUpPower;   
        UiManager.Instance.priceText.text = price.ToString();
        UiManager.Instance.powerText.text = "1";
    }

    public void PlayerClick(){
        if(multiplier == 0){
            score++;
        }
        else{
            score = score += multiplier;
        }
        if(power == 0){
            power = 1;
            playerHealth--;
        }
        else{
            playerHealth = playerHealth -= power;
        }

        if(playerHealth <= 0){
            playerIndex++;
            InitPlayer(playerIndex);
        }

        UpdatePlayerHealth();
        UpdateScoreText();
    }

    public void UpdateScoreText(){
        UiManager.Instance.scoreText.text = score.ToString();
    }

    private void InitPlayer(int index){
        if(playerHealth == 0){
            playerHealth = GameSetting.Instance.multiplierHealtPlsayer;
        }
        else{
            playerHealth = playerHealth += GameSetting.Instance.multiplierHealtPlsayer;
        }

        UpdatePlayerHealth();

        UiManager.Instance.playerButton.GetComponent<Image>().sprite = GameSetting.Instance.spritesPlayers[index];
    }

    public void UpPower(){;
        if(score >= price){
            price = price += GameSetting.Instance.multiplierPriceUpPower;
            power++;
            UiManager.Instance.priceText.text = price.ToString();
            UiManager.Instance.powerText.text = power.ToString();
        }
    }

    public void UpdatePlayerHealth(){
        UiManager.Instance.healthText.text = playerHealth.ToString();
    }
}