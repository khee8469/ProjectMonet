using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using JJH;
using System;

public class taoru : InteractObject
{
    [Tooltip("해당 캔버스와 관련된 참조")]
    private DrawObjectManager drawManager;

    [Tooltip("그리기를 허용할 레이어 마스크")]
    [SerializeField] private LayerMask drawingLayer;

    [Tooltip("닦아지는 거리")]
    [SerializeField] float rayDistance;
    
    [Tooltip("그려줄 라인 렌더러")]
    [SerializeField] private LineRenderer currentDrawing;

    [Tooltip("Defalut-Line 으로 설정할 것")]
    public Material drawingMaterial;

    [Tooltip("라인렌더러의 포지션 위한 인덱스")]
    [SerializeField] private int index;

    [Tooltip("펜의 크기 조절 기능")]
    [Range(0.01f, 0.1f)] public float penWidth = 0.01f;

    [Tooltip("박스의 크기")]
    public Vector3 boxSize = new Vector3(0.1f, 0.1f, 0.1f);

    [Tooltip("박스의 방향")]
    public Quaternion boxOrientation = Quaternion.identity;

    [Tooltip("Noraml 벡터 크기")]
    private float NormalDis = 0.01f;
    //잡으면 레이로 책상을 확인하는 이벤트 시작

    private bool isSelecting;
    //private bool 

    private void Update()
    {
        if (isSelecting )
        {
            Cleaning();
            Debug.Log("청소중");
        }
            
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        isSelecting = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        isSelecting = false;
    }


    private void Cleaning()
    {
        RaycastHit hit;

        if (Physics.BoxCast(transform.position, boxSize, transform.forward, out hit, boxOrientation, rayDistance, drawingLayer))
        {
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

            Vector3 drawPosition = hit.point + hit.normal * NormalDis;
            Debug.Log("칠하는중");

            if (currentDrawing == null) //이 부분에서 현재 물감에 알맞는 색상으로 만들어줘야 할 것 같아. 
            {
                index = 0;

                GameObject lineObj = new GameObject("Line");

                lineObj.transform.position = transform.position;
                currentDrawing = lineObj.AddComponent<LineRenderer>();

                currentDrawing.material = new Material(drawingMaterial);

                // 현재 색상 설정
                currentDrawing.material.color = Color.clear;

                currentDrawing.startWidth = currentDrawing.endWidth = penWidth; //일정한 굵기. 

                currentDrawing.positionCount = 1; // 시작 포지션 카운트 1

                currentDrawing.SetPosition(0, drawPosition);

                drawManager.AddLineRenderer(currentDrawing, penWidth);

                //lineList.Add(lineObj);

                Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);

                
            }

            else //생성된 경우. 
            {

                var currentPos = currentDrawing.GetPosition(index);
                

                if (Vector3.Distance(currentPos, drawPosition) > 0.01f)
                {
                    index++;
                    currentDrawing.positionCount = index + 1;

                    currentDrawing.SetPosition(index, drawPosition);
                    drawManager.AddLineRenderer(currentDrawing, penWidth);

                    Vector2Int pixelPosition = drawManager.WorldToPixel(drawPosition);
                }
            }
        }

        
 
    }



}
