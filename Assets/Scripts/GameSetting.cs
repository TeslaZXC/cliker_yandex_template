using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Transform enemyPivot;
    public int multiplierHealtPlsayer;
    public Sprite[] spritesPlayers;       
    public int multiplierPriceUpPower;

    public static GameSetting Instance{get;private set;}

    private void Awake() {
        Instance = this;
    }
}