using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionSyncManager : MonoBehaviour
{
    public static PositionSyncManager instance;

    //Resources에서 가져오기
    public PositionData positionData; // 위치데이터 저장


    public MiniatureManager miniatureManager;
    public NpcManager npcManager;


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

    private void Start()
    {
        positionData = Resources.Load("PositionData").GetComponent<PositionData>();
        miniatureManager = Resources.Load("MiniatureManager").GetComponent<MiniatureManager>();
        npcManager = Resources.Load("NpcManager").GetComponent<NpcManager>();
    }


    /*public void SavePosition(Vector3 position)
    {
        positionData = new PositionData { position = position };
    }

    public Vector3 LoadPosition()
    {
        return positionData != null ? positionData.position : Vector3.zero;
    }
    */
}
