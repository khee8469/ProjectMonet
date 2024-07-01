using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PositionSyncManager : MonoBehaviour
{
    //plableDatamanager로 이동
    /*private static PositionSyncManager instance;
    public static PositionSyncManager Instance {  get { return instance; } }

    //Resources에서 가져오기
    [SerializeField]
    private PositionData miniPositionData; // 위치데이터 저장
    public PositionData MiniPositionData { get { return miniPositionData; } }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        positionData.SavePosition.Clear();
    }*/
}
