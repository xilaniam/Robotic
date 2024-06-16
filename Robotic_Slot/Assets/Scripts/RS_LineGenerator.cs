using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class RS_LineGenerator : MonoBehaviour
{
    public LineRenderer lr;
    public List<Material> materials;
    public RS_ResultReel reels;
    public float animDuration = 3f;

    private Vector3[] linepoints;
    private float _extraLength = 2f;

    

    private void Awake()
    {
        reels = GameObject.Find("HoriontalFinalManager").GetComponent<RS_ResultReel>();
    }
    void Start()
    {
        lr.material = materials[Random.Range(0, materials.Count)];
    }
    public void DrawLine(int[][] arr)
    {
        float posY = 0;
        float posX = 0;


        lr.positionCount = 1;
        posX = reels.items[0].transform.position.x - _extraLength;
        posY = GetPosY(arr[0][0]);
        linepoints = new Vector3[7];
        linepoints[0] = new Vector3(posX, posY, 90);

        float[] posXValues = { reels.items[0].transform.position.x, reels.items[3].transform.position.x, reels.items[6].transform.position.x, reels.items[9].transform.position.x, reels.items[12].transform.position.x };

        for (int i = 0; i < arr.Length; i++)
        {
            posX = posXValues[i];
            posY = GetPosY(arr[i][0]);
            linepoints[i + 1] = new Vector3(posX, posY, 90);
        }

        posX = reels.items[12].transform.position.x + _extraLength;
        posY = GetPosY(arr[arr.Length - 1][0]);
        linepoints[6] = new Vector3(posX, posY, 90);
        StartCoroutine(AnimateLine());


    }

    private float GetPosY(int value)
    {
        switch (value)
        {
            case 0:
                return reels.items[0].transform.position.y;//1.5f;//
            case 1:
                return reels.items[1].transform.position.y;//-1.5f;//
            case 2:
                return reels.items[2].transform.position.y;//-4.5f;//
            default:
                Debug.LogError("Invalid value for posY");
                return 0f;
        }
    }
    IEnumerator AnimateLine()
    {
        float segmentDuration = animDuration / linepoints.Length;
        lr.SetPosition(0, linepoints[0]);

        for (int i = 0; i < linepoints.Length - 1; i++)
        {
            lr.positionCount++;
            float StartTime = Time.time;
            Vector3 startPos = linepoints[i];
            Vector3 endPos = linepoints[i + 1];
            Vector3 pos = startPos;
            while (pos != endPos)
            {
                float t = (Time.time - StartTime) / segmentDuration;
                pos = Vector3.Lerp(startPos, endPos, t);
                lr.SetPosition(i + 1, pos);
                yield return null;
            }

        }
    }
}




