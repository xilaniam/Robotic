using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_ReelAnimator : MonoBehaviour
{
    [SerializeField]
    public GameObject reelPrefab;
    public GameObject[] assetPrefab;
    void Start()
    {
        for(int i=0; i <5; i++)
        {
            GameObject reelInstance = Instantiate(reelPrefab,transform.position, Quaternion.identity,transform);
            for (int j = 0; j < 50; j++)
            {
                int randomAsset = Random.Range(0, assetPrefab.Length);
                Instantiate(assetPrefab[randomAsset], reelInstance.transform.position, Quaternion.identity, reelInstance.transform);
            }
        }
    }
}
