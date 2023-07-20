using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;

public class TextTranslate : MonoBehaviour
{
    [SerializeField] private string ru;
    [SerializeField] private string eng;

    private void Awake()
    {
        if(YandexGame.EnvironmentData.language == "ru")
        {
            transform.GetComponent<TextMeshProUGUI>().text = ru;
        }
        else if(YandexGame.EnvironmentData.language == "eng")
        {
            transform.GetComponent<TextMeshProUGUI>().text = eng;
        }
    }
}