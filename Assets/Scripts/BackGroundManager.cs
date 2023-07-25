using UnityEngine;
using YG;

public class BackGroundManager : MonoBehaviour
{
    [HideInInspector] public int indexBackGround;

    private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;
    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

    public static BackGroundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SelectBacground()
    {
        ExampleOpenRewardAd(1);
    }

    void Rewarded(int id)
    {
        if (id == 1)
        {
            print("select background");

            int size = GameSetting.Instance.spriteBackgrounds.Length;
            indexBackGround++;

            print(size);
            print(indexBackGround);

            size--;

            if(indexBackGround > size)
            {
                indexBackGround = 0;
            }
            GameManager.Instance.SaveData();
            UiManager.Instance.bacgroundImage.sprite = GameSetting.Instance.spriteBackgrounds[indexBackGround];
        }
    }

    void ExampleOpenRewardAd(int id)
    {
        YandexGame.RewVideoShow(id);
    }
}