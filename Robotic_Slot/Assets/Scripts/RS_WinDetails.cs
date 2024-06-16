using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RS_WinDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Win;
    [SerializeField] private TextMeshProUGUI Totalbet;
    [SerializeField] private TextMeshProUGUI Balance;
    [SerializeField] private TextMeshProUGUI line;
    [SerializeField] private TextMeshProUGUI lineBet;

    private float linebetValue = 0;
    private float lineVaue = 0;
    void Start()
    {
        StartCoroutine(CheckBalance());
        RS_AnimController.animationFinished += UpdateWinResults;
    }

    private void OnDestroy()
    {
        RS_AnimController.animationFinished -= UpdateWinResults;
    }

    private void Update()
    {
        float.TryParse(line.text, out lineVaue);
        float.TryParse(lineBet.text, out linebetValue);

        Totalbet.text = (lineVaue*linebetValue).ToString();
    }
    void UpdateWinResults()
    {
        RS_SpinInfo spinInfo = RS_APIHandler.instance.spinResponse;
        Win.text = spinInfo.win.ToString();
        Totalbet.text = spinInfo.totalBet.ToString();
        Balance.text = spinInfo.balance.ToString();
    }

    private IEnumerator CheckBalance()
    {
        // Wait until the login process is complete
        yield return StartCoroutine(RS_APIHandler.instance.PostLoginRequest("https://api.projectrsh.com/v1/player/login"));

        // Now request the balance
        yield return StartCoroutine(RS_APIHandler.instance.BalanceRequest("https://api.projectrsh.com/v1/play/test/goh/balance", FetchBalance));
    }

    private void FetchBalance(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            Balance.text = response;
        }
        else
        {
            Debug.Log("Failed to retrieve data");
        }
    }
}
