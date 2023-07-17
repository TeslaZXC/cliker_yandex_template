using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UiManager : MonoBehaviour
{
    public Transform canvas; //AAAAAAAAA —” ¿
    public TMP_Text scoreText;
    public TMP_Text powerText;
    public TMP_Text priceText;

    public Button playerButton;
    public Button upPowerButton;

    [SerializeField] private UnityEvent playerAction;
    [SerializeField] private UnityEvent upPowerAction;

    public TMP_Text healthText;
    public Slider healthBar;

    public static UiManager Instance{get;private set;}

    private void Awake() {
        Instance = this;

        playerButton.onClick.AddListener(playerAction.Invoke);
        upPowerButton.onClick.AddListener(upPowerAction.Invoke);
    }
}