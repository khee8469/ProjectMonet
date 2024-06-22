using JJH;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCW_DrawingCompleteManager : MonoBehaviour
{
    /*[Tooltip("그림의 bool 변수 체크해줄 딕셔너리")]
    [SerializeField]
    public Dictionary<DrawObjectManager, bool> drawCompleteCheckDic
           = new Dictionary<DrawObjectManager, bool>();

    [Tooltip("bool 변수와 맞춰줄 draw list ")]
    [SerializeField]
    public List<DrawObjectManager> sortedDrawObjectManagers =
        new List<DrawObjectManager>();


    [Tooltip("완성된 그림을 가지고 있는 배열")]
    // 이를 이용해 변수에 따라 오브젝트를 켜줘서 해당 오브젝트에서 onEnable 발동 할 수 있도록 한다. 
    [SerializeField] private GameObject[] finishedPainting;

    [Tooltip("그림의 컬러를 비교해 줄 스크립터블 오브젝트")]
    [SerializeField] private PaintTypeManager paintTypeManager;

    [Tooltip("딕셔너리의 무결성 유지를 위한 임시 key 저장용 리스트")]
    List<DrawObjectManager> keysToModifty = new List<DrawObjectManager>();
    private void Start()
    {
        // Find로 배열을 찾아서 저장한다. --> 배열은 항상 정렬 순서가 보장된다. 
        DrawObjectManager[] drawObjectManagers =
            GameObject.FindObjectsOfType<DrawObjectManager>();

        // DrawObjectManager 에 붙어있는 IComparable 을 이용하여 정렬한다.
        Array.Sort(drawObjectManagers); //어차피 둘이 같은 타입을 찾는 find를 하기 때문에 길이는 무조건 같

        for (int i = 0; i < drawObjectManagers.Length; i++) //어차피 이 둘은 길이가 똑같음. 
        {
            drawCompleteCheckDic.Add(drawObjectManagers[i], ChapterManager.isDrawing_Complete[i]);

            if (drawCompleteCheckDic.TryGetValue(drawObjectManagers[i], out bool isTure))
            {
                if (isTure) // ture 라면 그 알파값을 full 로 올려줘서 보이도록 해줘야함. 
                {
                    DrawObjectManager dr = drawObjectManagers[i].GetComponent<DrawObjectManager>();
                    if (dr != null)
                    {
                        SpriteRenderer sprite = dr.GetComponent<SpriteRenderer>();
                        Color color = sprite.color;
                        color.a = 1f;
                        sprite.color = color;


                    }

                }
            }


        }
        // start 에서 이벤트 발동 시켜서... 씬 해금 상태 유지 및 씬 컬러 상태 체크 해주기.
        // 씬 컬러 상태는 유지라기 보다는 한 번만 발동해주면 (static bool만 바꿔주면 계속 유지됨. )


    }

    public void DrawComplete(int drawingNumber, bool finishied, int instanceID) // 그림이 완성되었을 때 (완전히) 진행할 함수 
                                                                                // 챕터도 해금 시켜줘야 하고. 포스트프로세싱도 종료 시켜줘야하기 때문에 
                                                                                // 싱글턴 매니저와 연계가 필요하다. 
    {

        ChapterManager.isDrawing_Complete[drawingNumber] = finishied; // 드로우 컴플리트를 부를 때 값을 지정?

        // 임시 키 저장용 리스트 초기화
        keysToModifty.Clear(); // 초기화 안하면 이거 계속 들어있음. 


        foreach (var obj in drawCompleteCheckDic)
        {
            if (obj.Key.drawBoardNumber == (DrawBoardNumber)drawingNumber) // 결국은 매개변수를 다른곳에서 받아야함
            {
                if (obj.Key.GetInstanceID() == instanceID)
                {
                    Debug.Log($"{obj.Key} 의 if문 들어가서 true 값으로 변환됨.");
                    keysToModifty.Add(obj.Key);
                }
            }
        }

        // foreach 문 종료 후 딕셔너리 수정 
        foreach (var key in keysToModifty)
        {
            drawCompleteCheckDic[key] = finishied;
        }


        // 모든 오브젝트가 완성되었는지 확인하기. --> 타입을 매개변수로 받기 때문에 그 순간의 매개변수로 체크
        bool allComplete = true;

        foreach (var kvp in drawCompleteCheckDic)
        {
            if ((kvp.Key.drawBoardNumber == (DrawBoardNumber)drawingNumber) && kvp.Value == false)
            {
                Debug.Log($"{kvp.Key}의 작업이 아직 완료되지 않았습니다.");
                allComplete = false;
                break;
            }
        }

        // 아 이부분 한 번 또 enum 으로 if문 체크해줘야 하나? 아니면 어차피 순차적으로니까 또 체크해줄 필요는없나?

        if (allComplete) // 이게 지금 모두 true 라면 
        {
            Debug.Log("올컴플리트 if문 진입");

            // 0 1 2 3 --> 4개의 씬 
            ChapterManager.Instance.CheckDrawComplete(drawingNumber, true); // 씬의 필터 해제

            // 내부코드는 그대로 받아들이도록 수정함 --> 결국 0 이면 1챕터 개방이므로 + 1 필요 ??
            if (drawingNumber <= 2)
            {
                ChapterManager.Instance.UnlockStage(drawingNumber + 1, true); // 3 부터는 인덱스 터짐. 
            }
            // 완성본 그림 알파값 업그레이드 해주기

            FinishedDraw.FinishAlphaUp.Invoke(drawingNumber);
        }


    }*/
}
