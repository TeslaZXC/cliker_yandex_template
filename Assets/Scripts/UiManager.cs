using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UiManager : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text powerText;
    [SerializeField] public TMP_Text priceText;

    [SerializeField] public Button playerButton;
    [SerializeField] public Button upPowerButton;

    [SerializeField] private UnityEvent playerAction;
    [SerializeField] private UnityEvent upPowerAction;

    [SerializeField] public TMP_Text healthText;

    public static UiManager Instance{get;private set;}

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        playerButton.onClick.AddListener(playerAction.Invoke);
        upPowerButton.onClick.AddListener(upPowerAction.Invoke);
    }
}