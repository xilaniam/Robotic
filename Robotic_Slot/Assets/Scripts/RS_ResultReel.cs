using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class RS_ResultReel : MonoBehaviour
{
    public GameObject reelPrefab;
    public GameObject[] asset;
    private int[][] slots;
    public List<GameObject> items = new List<GameObject>();
    void Start()
    {

        RS_APIHandler.responseReceived += FillReelWithAssets;
    }

    private void OnDestroy()
    {
        RS_APIHandler.responseReceived -= FillReelWithAssets;
    }
    void FillReelWithAssets()
    {
        StartCoroutine(WaitForReelResult());
    }

    IEnumerator WaitForReelResult()
    {
        RS_SpinInfo spinInfo = RS_APIHandler.instance.spinResponse;
        slots = new int[5][];
        for (int i = 0; i < 5; i++)
        {
            slots[i] = new int[3];
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                slots[j][i] = spinInfo.slots[i][j];
            }
        }
        yield return null;
        PopulateReels(slots);
    }

    void PopulateReels(int[][] arr)
    {
        items.Clear();
        if(transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < arr.Length; i++)
        {
            GameObject reel = Instantiate(reelPrefab, transform.position, Quaternion.identity, transform);
            for (int j = 0; j < arr[i].Length; j++)
            {
                //Debug.Log($"Slot[{i}][{j}] = {arr[i][j]}");
                GameObject item = Instantiate(asset[arr[i][j]], reel.transform.position, Quaternion.identity, reel.transform);
               //Debug.Log(item.transform.name + " : " + item.transform.position);
                items.Add(item);
            }
        }
    }
}
