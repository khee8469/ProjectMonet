using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour
{
    int sceneNumber;

    void Start()
    {
        if (transform.parent.name == "Scene_1")
            sceneNumber = 0;
        else if (transform.parent.name == "Scene_2")
            sceneNumber = 1;
        else if (transform.parent.name == "Scene_3")
            sceneNumber = 2;
        else if (transform.parent.name == "Scene_4")
            sceneNumber = 3;
        else
            Debug.Log("ERROR");

        Debug.Log(sceneNumber);

        coroutine = StartCoroutine(NpcTestMove());
    }

    private Coroutine coroutine;

    private IEnumerator NpcTestMove()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            int randomMove = Random.Range(0, 3);
            switch (randomMove)
            {
                case 0:
                    transform.position = transform.position + Vector3.forward * 1;
                    break;
                case 1:
                    transform.position = transform.position + Vector3.back * 1;
                    break;
                case 2:
                    transform.position = transform.position + Vector3.left * 1;
                    break;
                case 3:
                    transform.position = transform.position + Vector3.right * 1;
                    break;
            }

            //위치데이터 저장
            //딕셔너리에 같은 key의 데이터가 없으면 저장
            if (!PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(transform.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(transform.name, transform.localPosition);
            }
            //딕셔너리에 같은 key의 데이터가 있으면 삭제 후 다시 저장
            else if (PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(transform.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Remove(transform.name);
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(transform.name, transform.localPosition);
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
