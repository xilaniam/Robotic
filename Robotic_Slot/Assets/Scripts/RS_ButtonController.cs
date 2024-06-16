using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class RS_ButtonController : MonoBehaviour
{
    public TextMeshProUGUI lineBet;
    public TextMeshProUGUI line;
    public Image spinButtonImage;
    public GameObject Slot, Setting, GameRule;
    public AudioClip buttonSound;

    public float[] lineBetincrementList = new float[5];
    int betIndex = 0;

    public float lineValue = 1;
    public float lineBetValue = 1;
    private float maxlineValue = 10;
    private float minValue = 1;


    private void Awake()
    {
    }
    async void Start()
    {
        RS_remoteJson config = await RS_RemoteConfig.GetConfigResponseJsonAsync();
        lineBetincrementList = config.BetIncreaseAmountList;
        lineValue = 1;
        lineBetValue = 1;
        lineBet.text = lineBetValue.ToString();
        line.text = lineValue.ToString();
    }

    public void IncreaselineBet()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        if (betIndex >= lineBetincrementList.Length-1)
        {
            
            betIndex = lineBetincrementList.Length - 1;
            lineBetValue = lineBetincrementList[lineBetincrementList.Length - 1];
        }
        else
        {
            betIndex++;
            lineBetValue = lineBetincrementList[betIndex];
        }
        lineBet.text = lineBetValue.ToString();
    }

    public void Increaseline()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        lineValue++;
        lineValue = (lineValue > maxlineValue) ? maxlineValue : lineValue;
        line.text = lineValue.ToString();
    }
    public void DecreaselineBet()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        if (betIndex == 0)
        {

            betIndex = 0;
            lineBetValue = lineBetincrementList[0];
        }
        else
        {
            betIndex--;
            lineBetValue = lineBetincrementList[betIndex];
        }
        lineBet.text = lineBetValue.ToString();
    }
    public void Decreaseline()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        lineValue--;
        lineValue = (lineValue < minValue) ? minValue : lineValue;
        line.text = lineValue.ToString();
    }

    public void settings()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        GameRule.SetActive(false);
        Setting.SetActive(true);
    }

    public void gamerule()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        Setting.SetActive(false);
        GameRule.SetActive(true);
    }

    public void slot()
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        GameRule.SetActive(false);
        Setting.SetActive(false);
        Slot.SetActive(true);
    }

    public void stop(Sprite stopImage)
    {
        RS_SoundFXManager.Instance.PlaySound(buttonSound, 1f);
        spinButtonImage.sprite = stopImage;
    }

}


