using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Security.Cryptography;


public class RS_APIHandler : MonoBehaviour
{
    [SerializeField] private string LoginApiUri;
    [SerializeField] private string SpinUri;
    [SerializeField] private string Username;
    [SerializeField] private string Password;
    //private string balanceURI = "https://api.projectrsh.com/v1/play/test/goh/balance";
    private string token;
    public RS_SpinInfo spinResponse;
    public static RS_APIHandler instance { get; private set; }
    public static event Action responseReceived;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(instance);
        StartCoroutine(PostLoginRequest(LoginApiUri));
    }
    void Start()
    {
        
    }

    public IEnumerator PostLoginRequest(string uri)
    {
        RS_Login data = new RS_Login();
        data.playerId = Username;
        data.password = Password;

        string jsonData = JsonConvert.SerializeObject(data);

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bytes);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseJson = webRequest.downloadHandler.text;
                RS_LoginInfo info = JsonConvert.DeserializeObject<RS_LoginInfo>(responseJson);
                token = info.token;
            }
        }
    }

    public IEnumerator SpinRequest(string token,string lineValue,string lineBetValue)
    {
        RS_Spin spinData = new RS_Spin();
        spinData.playLine = lineValue;
        spinData.lineBet = lineBetValue;
        
        string jsondata = JsonConvert.SerializeObject(spinData);

        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(SpinUri,jsondata))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsondata);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bytes);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string responseJson = webRequest.downloadHandler.text;
                spinResponse = JsonConvert.DeserializeObject<RS_SpinInfo>(responseJson);
                responseReceived?.Invoke();
                Debug.Log("Authorization success: " + responseJson);
            }
            else
            {
                Debug.LogError("Authorization failed: " + webRequest.error);
            }

        }
    }


    public void sendSpinRequest(string lineValue,string lineBetValue)
    {
        StartCoroutine(SpinRequest(token,lineValue,lineBetValue));
    }

    public IEnumerator BalanceRequest(string URI, Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(URI))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("balance retrieved success: " + webRequest.downloadHandler.text);
                callback?.Invoke(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Authorization failed: " + webRequest.error);
            }

        }
    }
}
