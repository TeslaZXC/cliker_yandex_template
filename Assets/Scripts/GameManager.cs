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

    private int enemyIndex = 0;
    private int power = 1;

    private int price;
    private int totalScore;

    private void OnEnable() => YandexGame.GetDataEvent += GetLoad;
    private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (YandexGame.SDKEnabled == true)
        {
            GetLoad();
            UpdateText();
        }
    }

    private void Start()
    {
        price += GameSetting.Instance.multiplierPriceUpPower;
        UiManager.Instance.priceText.text = price.ToString();
        InitPlayer();
    }

    private void Update()
    {
        UiManager.Instance.healthBar.value = Mathf.Lerp(UiManager.Instance.healthBar.value, enemyHealth, 5f * Time.deltaTime);
    }

    public void PlayerClick()
    {
        totalScore = score + 5 + power;
        score = score + 5 + power;
        enemyHealth -= power;

        if (enemyHealth <= 0)
        {
            enemyIndex++;
            
            //Если индекс врага вышел из длины спрайтов врагов то ставим - 0
            if (enemyIndex > GameSetting.Instance.spritesPlayers.Length - 1) enemyIndex = 0;

            InitPlayer();
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

    private void InitPlayer()
    {
        enemyHealth = lastEnemyHeatlh + GameSetting.Instance.multiplierHealtPlsayer;
        lastEnemyHeatlh = enemyHealth;

        GameSetting.Instance.enemyPivot.transform.localPosition = new Vector2(Random.Range(-1173, 12), -131);

        UpdateText();
        UiManager.Instance.playerButton.GetComponent<Image>().sprite = GameSetting.Instance.spritesPlayers[enemyIndex];
        UiManager.Instance.healthBar.maxValue = lastEnemyHeatlh;
        UiManager.Instance.healthBar.value = lastEnemyHeatlh;
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
//
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

    public void GetLoad()
    {
        print("score - " + YandexGame.savesData.score);
        score = YandexGame.savesData.score;
        power = YandexGame.savesData.power;
        totalScore = YandexGame.savesData.totalScore;
        enemyIndex = YandexGame.savesData.indexEnemy;
        BackGroundManager.Instance.indexBackGround = YandexGame.savesData.indexBackGround;
        UiManager.Instance.bacgroundImage.sprite = GameSetting.Instance.spriteBackgrounds[YandexGame.savesData.indexBackGround];
        price = YandexGame.savesData.price;
        enemyHealth = YandexGame.savesData.enemyHealth;

        if(YandexGame.savesData.enemyHealth == 0)
        {
            enemyHealth = GameSetting.Instance.multiplierHealtPlsayer;
        }
        else
        {
            enemyHealth = YandexGame.savesData.enemyHealth;
        }

        UpdateText();
    }

    public void ResetSaves()
    {
        YandexGame.ResetSaveProgress();
    }

    public void SaveData()
    {
        YandexGame.savesData.score = score;
        YandexGame.savesData.power = power;
        YandexGame.savesData.indexEnemy = enemyIndex;
        YandexGame.savesData.indexBackGround = BackGroundManager.Instance.indexBackGround;
        YandexGame.savesData.price = price;
        YandexGame.savesData.enemyHealth = enemyHealth;
        YandexGame.savesData.totalScore = totalScore;
        YandexGame.SaveProgress();
    }
}