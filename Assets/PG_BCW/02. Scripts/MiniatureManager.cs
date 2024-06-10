using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniatureManager : MonoBehaviour
{
    [Tooltip("미니어처 리스트")]
    [SerializeField] List<Miniature> miniatures;
    public List<Miniature> Miniatures { get { return miniatures; } }

    int sceneNumber;

    private void Awake()
    {
        miniatures = GetComponentsInChildren<Miniature>().ToList();


    }

    private void Start()
    {
        if (gameObject.name == "Scene_1 Miniature")
            sceneNumber = 0;
        else if (gameObject.name == "Scene_2 Miniature")
            sceneNumber = 1;
        else if (gameObject.name == "Scene_3 Miniature")
            sceneNumber = 2;
        else if (gameObject.name == "Scene_4 Miniature")
            sceneNumber = 3;
        else
            Debug.Log("ERROR");

        Debug.Log(sceneNumber);

        SetMiniPosition();
    }

    private void SetMiniPosition()
    {
        foreach (Miniature miniature in miniatures)
        {
            //데이터가 잇으면
            if (PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(miniature.name))
            {
                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].TryGetValue(miniature.name, out Vector3 position);
                miniature.transform.localPosition = new Vector3(position.x, 0.5f, position.z);
            }
            //데이터가 없으면
            else if (!PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].ContainsKey(miniature.name))
            {

                PositionSyncManager.Instance.PositionData.SavePosition[sceneNumber].Add(miniature.gameObject.name, miniature.transform.localPosition);
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
