using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    private int score;

    private int enemyHealth;
    private int lastEnemyHeatlh;
    private int enemyIndex;

    private int power;
    private int price;

    private Vector2 enemyPosition;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += GetLoad;
    }
    private void OnDisable() 
    {
        YandexGame.GetDataEvent -= GetLoad;
    } 
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (YandexGame.SDKEnabled)
        {
            GetLoad();
        }
    }

    private void Update()
    {
        UiManager.Instance.healthBar.value = Mathf.Lerp(UiManager.Instance.healthBar.value, enemyHealth, 5f * Time.deltaTime);
    }

    public void GetLoad()
    {
        score = YandexGame.savesData.score;
        power = YandexGame.savesData.power;
        enemyIndex = YandexGame.savesData.indexEnemy;
        BackGroundManager.Instance.indexBackGround = YandexGame.savesData.indexBackGround;
        UiManager.Instance.bacgroundImage.sprite = GameSetting.Instance.spriteBackgrounds[YandexGame.savesData.indexBackGround];

        price = YandexGame.savesData.price;
        if (price == 0)
        {
            price = GameSetting.Instance.multiplierPriceUpPower;
        }

        InitPlayer(true);
        UpdateText();
    }

    public void PlayerClick()
    {
        score = score + 5 + power;
        enemyHealth -= power;

        if (enemyHealth <= 0)
        {
            enemyIndex++;
            
            if (enemyIndex > GameSetting.Instance.spritesPlayers.Length - 1) enemyIndex = 0;
            InitPlayer(false);
        }

        Vector2 spawnPosition = GetMousePositionInCanvas();
        GameObject floatingObject = Instantiate(GameSetting.Instance.floatingTextPrefab, spawnPosition, Quaternion.identity, UiManager.Instance.canvas.transform);
        floatingObject.GetComponentInChildren<TextMeshProUGUI>().text = $"-{power}";

        Destroy(floatingObject, 4.5f);
        UpdateText();
    }

    private Vector2 GetMousePositionInCanvas()
    {
        RectTransform canvasRect = UiManager.Instance.canvas.GetComponent<RectTransform>();

        Vector2 mousePosition = Input.mousePosition;
        Vector2 canvasPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePosition, null, out canvasPosition);

        return canvasRect.TransformPoint(canvasPosition);
    }

    private void InitPlayer(bool first)
    {
        if (first)
        {
            enemyHealth = YandexGame.savesData.enemyHealth;
            if(YandexGame.savesData.enemyHealth == 0)
            {
                enemyHealth = GameSetting.Instance.multiplierHealtPlsayer;
            }
        }
        else
        {

            if (YandexGame.savesData.enemyHealth == 0)
            {
                enemyHealth = lastEnemyHeatlh + GameSetting.Instance.multiplierHealtPlsayer;
            }
            else
            {
                enemyHealth = YandexGame.savesData.enemyHealth + GameSetting.Instance.multiplierHealtPlsayer;
            }
        }

        lastEnemyHeatlh = enemyHealth;

        SetRandomPosition();
        UiManager.Instance.playerButton.GetComponent<Image>().sprite = GameSetting.Instance.spritesPlayers[enemyIndex];
        UiManager.Instance.healthBar.maxValue = lastEnemyHeatlh;
        UiManager.Instance.healthBar.value = lastEnemyHeatlh;

        if(!first) SaveData();

        UpdateText();
    }

    private void SetRandomPosition()
    {
        enemyPosition = new Vector2(Random.Range(137, 1428), 803);
        GameSetting.Instance.enemyPivot.anchoredPosition = enemyPosition;
    }

    public void UpPower(){;
        if (score < price) return;

        score -= price;
        price += GameSetting.Instance.multiplierPriceUpPower;
        power++;

        UpdateText();
    }

    private void UpdateText()
    {
        if (YandexGame.EnvironmentData.language == "ru")
        {
            UiManager.Instance.priceText.text = $"Улучшить силу клика за {price}$";
            UiManager.Instance.powerText.text = $"Сила клика: {power}";
            UiManager.Instance.scoreText.text = $"{score} $";
            UiManager.Instance.healthText.text = $"ХП: {enemyHealth}";
        }
        else if (YandexGame.EnvironmentData.language == "eng")
        {
            UiManager.Instance.priceText.text = $"Improve click power for {price}$";
            UiManager.Instance.powerText.text = $"Click Power: {power}";
            UiManager.Instance.scoreText.text = $"{score} $";
            UiManager.Instance.healthText.text = $"HP: {enemyHealth}";
        }

        if (score >= price)
        {
            UiManager.Instance.upPowerButton.GetComponent<Image>().color = new Color32(83, 223, 131, 255);
        }
        else
        {
            UiManager.Instance.upPowerButton.GetComponent<Image>().color = new Color32(63, 63, 63, 255);
        }
    }

    public void ResetSaves()
    {
        YandexGame.ResetSaveProgress();
    }

    public void SaveData()
    {
        if (!YandexGame.SDKEnabled) return;

        YandexGame.savesData.score = score;
        YandexGame.savesData.power = power;
        YandexGame.savesData.indexEnemy = enemyIndex;
        YandexGame.savesData.indexBackGround = BackGroundManager.Instance.indexBackGround;
        YandexGame.savesData.price = price;
        YandexGame.savesData.enemyHealth = enemyHealth;
        YandexGame.SaveProgress();
    }
}