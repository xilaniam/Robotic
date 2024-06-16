using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
public class RS_AnimController : MonoBehaviour
{
    public GameObject animReel;
    public GameObject ResultReel;
    public GameObject linePrefab;
    public AudioClip ReelAudio, ResultAudio , Button;
    private RS_ButtonController lineInputs;

    public static event Action animationFinished;
    public List<int[][]> PlayLineLists;
    public GameObject[] buttons;
    public TextMeshProUGUI win;

    private List<GameObject> instantiatedLines = new List<GameObject>();
    private int buttonPressed;

    private Vector3 animReelOriginalPos;
    private Vector3 resultReelOriginalPos;
    private bool skip = false;
    private bool isAutoSpinning = false;
    private Coroutine autoSpinCoroutine;


    private void Awake()
    {
    }
    void Start()
    {
        PlayLineLists = new List<int[][]>();
        lineInputs = FindObjectOfType<RS_ButtonController>();
        animReelOriginalPos = animReel.transform.localPosition;
        resultReelOriginalPos = ResultReel.transform.localPosition;
        RS_APIHandler.responseReceived += onAPIResponse;
    }

    private void OnDestroy()
    {
        RS_APIHandler.responseReceived -= onAPIResponse;
    }
    public void spinSlot()
    {

        RS_SoundFXManager.Instance.PlaySound(Button, 1f);
        for (int i = 0; i < buttons.Length - 2; i++)
        {
            buttons[i].GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        win.text = "0";
        RS_APIHandler.instance.sendSpinRequest(lineInputs.lineValue.ToString(), lineInputs.lineBetValue.ToString());
    }

    private void onAPIResponse()
    {
        StartCoroutine(spinReel());
    }

    //------------------Reel Animation------------------------//
    IEnumerator spinReel()
    {
        deleteLines();
        buttonPressed++;
        float playLength = 0;
        if (!skip) playLength = 0;
        else playLength = 0.6f;
        RS_SoundFXManager.Instance.PlaySound(ReelAudio, 1f,playLength);
 
        if (buttonPressed == 2)
        {
            buttonPressed = 1;
            ResultReel.GetComponent<HorizontalLayoutGroup>().enabled = true;
            ResultReel.transform.localPosition = resultReelOriginalPos;
        }


        animReel.GetComponent<HorizontalLayoutGroup>().enabled = false;
        animReel.transform.DOLocalMoveY(-12, 1700f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Restart).SetSpeedBased(true);
        yield return new WaitForSeconds(.4f);
        for (int i = 0; i < animReel.transform.childCount; i++)
        {
            if (!skip)
            {
                yield return new WaitForSeconds(0.26f);
            }
            RS_SoundFXManager.Instance.PlaySound(ResultAudio, 1f);
            ResultReel.GetComponent<HorizontalLayoutGroup>().enabled = false;
            if (!skip) ResultReel.transform.GetChild(i).DOLocalMoveY(-648f, 1700f).SetEase(Ease.InSine).SetSpeedBased(true);
            else ResultReel.transform.GetChild(i).transform.localPosition = new Vector3(ResultReel.transform.GetChild(i).transform.localPosition.x, -648, ResultReel.transform.GetChild(i).transform.localPosition.z);
            if (!skip) yield return new WaitForSeconds(0.3f);
            animReel.transform.GetChild(i).gameObject.SetActive(false);
        }
        StartCoroutine(resetAnimPosition());
        
    }

    IEnumerator resetAnimPosition()
    {
        Vector3 offset = new Vector3(0,625f,0);
        yield return null;
        //Kill all the tween animation for anim reel
        animReel.transform.DOKill();
        //Go back to its nondisplay position;
        animReel.transform.localPosition = animReelOriginalPos + offset;
        animReel.GetComponent<HorizontalLayoutGroup>().enabled = true;
        //Enable the active for child of anim reel
        for (int i = 0; i < animReel.transform.childCount; i++) 
        {
            animReel.transform.GetChild(i).gameObject.SetActive(true);
        }
        //enable the buttons
        for (int i = 0; i < buttons.Length - 1; i++)
        {
            buttons[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        //for drawing lines
        StartCoroutine(waitForLinevalues());
    }
    //------------------Reel Animation------------------------//


    public void fastButton()
    {
        RS_SoundFXManager.Instance.StopSound(ReelAudio);
        RS_SoundFXManager.Instance.PlaySound(Button, 1f);
        if (!skip)
        {
            skip = true;
            buttons[buttons.Length - 1].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            skip = false;
            buttons[buttons.Length - 1].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 255);
        }
        PlayLineLists.Clear();
    }


    //------------------AutoSpin---------------------------//
    public void StartAutoSpin()
    {
        buttons[4].SetActive(false);
        buttons[5].SetActive(true);
        isAutoSpinning = true;
        autoSpinCoroutine = StartCoroutine(AutoSpin());
    }
    private IEnumerator AutoSpin()
    {
        while (isAutoSpinning)
        {
            spinSlot();
            if (!skip) yield return new WaitForSeconds(4.5f);
            else yield return new WaitForSeconds(2.5f);
        }
    }
    public void StopAutoSpin()
    {
        RS_SoundFXManager.Instance.PlaySound(Button, 1f);
        buttons[4].SetActive(true);
        buttons[5].SetActive(false);
        if (autoSpinCoroutine != null)
        {
            StopCoroutine(autoSpinCoroutine);
            autoSpinCoroutine = null;
        }
        isAutoSpinning = false;
    }


    //------------------AutoSpin---------------------------//


    //-----------------------Line Generation--------------------//
    IEnumerator waitForLinevalues()
    {
        generateLine();
        yield return null;
    }

    void generateLine()
    {
        if (RS_APIHandler.instance.spinResponse.playlineWithWins.Count != 0)
        {
            for (int i = 0; i < RS_APIHandler.instance.spinResponse.playlineWithWins.Count; i++)
            {
                GameObject line = Instantiate(linePrefab);
                instantiatedLines.Add(line);
                RS_LineGenerator linescript = line.GetComponent<RS_LineGenerator>();
                linescript.DrawLine(RS_APIHandler.instance.spinResponse.playlineWithWins[i].playline);
            }
        }
        animationFinished?.Invoke();
    }
    void deleteLines()
    {
        foreach (GameObject line in instantiatedLines)
        {
            Destroy(line);
        }
        instantiatedLines.Clear();
    }
    //-----------------------Line Generation--------------------//
}
