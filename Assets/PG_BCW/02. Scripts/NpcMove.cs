using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcMove : MonoBehaviour
{
    void Start()
    {
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

            if (!PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(transform.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition.Add(transform.name, transform.localPosition);
            }
            //딕셔너리에 같은 key의 데이터가 있으면
            else if (PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(transform.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition.Remove(transform.name);
                PositionSyncManager.Instance.PositionData.SavePosition.Add(transform.name, transform.localPosition);
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
