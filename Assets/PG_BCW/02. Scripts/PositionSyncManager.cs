using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionSyncManager : MonoBehaviour
{
    public enum MiniatureNum { First, Second, Third, Fourth, }

    private static PositionSyncManager instance;
    public static PositionSyncManager Instance {  get { return instance; } }

    //Resources에서 가져오기
    [SerializeField]
    private PositionData positionData; // 위치데이터 저장
    public PositionData PositionData { get { return positionData; } }




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
    }
}
