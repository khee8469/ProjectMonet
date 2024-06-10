using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{
    [Tooltip("미니어처 리스트")]
    [SerializeField] List<MiniatureMove> miniatures;
    public List<MiniatureMove> Miniatures {  get { return miniatures; } }

    private void Awake()
    {
        miniatures = GetComponentsInChildren<MiniatureMove>().ToList<MiniatureMove>();
    }

    private void Start()
    {
        foreach (MiniatureMove miniature in miniatures)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(miniature.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition.TryGetValue(miniature.name, out Vector3 position);
                miniature.transform.localPosition = new Vector3(position.x, 0.5f, position.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(miniature.name))
            {
                continue;
            }
        }
    }


    /*//위치 재배치
    private void Start()
    {
        coroutine = StartCoroutine(DelayStart());
    }

    Coroutine coroutine;
    //PositionSyncManager의 start의 초기 딕셔너리값이 안들어가서
    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (MiniatureMove miniature in miniatures)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(miniature.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition.TryGetValue(miniature.name, out Vector3 position);
                miniature.transform.localPosition = new Vector3(position.x, 0.5f, position.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition.ContainsKey(miniature.name))
            {
                continue;
            }
        }
    }*/
}
