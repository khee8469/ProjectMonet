using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionSyncManager : MonoBehaviour
{
    private static PositionSyncManager instance;
    public static PositionSyncManager Instance {  get { return instance; } }

    //Resources에서 가져오기
    [SerializeField]
    private PositionData positionData; // 위치데이터 저장
    public PositionData PositionData { get { return positionData; } }

    /*[SerializeField]
    private MiniatureManager miniatureManager;
    public MiniatureManager MiniatureManager { get { return miniatureManager; } }*/

    /*private NpcManager npcManager;
    public NpcManager NpcManager { get { return npcManager; } }*/



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
        //미니어처들의 처음위치 저장
        /*if (miniatureManager != null)
        {
            foreach (MiniatureMove miniature in miniatureManager.Miniatures)
            {
                positionData.SavePosition.Add(miniature.name, miniature.transform.localPosition);
            }
        }*/
    }

    private void OnDisable()
    {
        positionData.SavePosition.Clear();
    }


    private void SetMiniaturePosition()
    {

    }

    private void SetNpcPosition()
    {

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
