using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using JJH;
using System;

public class Taoru : InteractObject
{
    //닦은 구역 표시하기

    [Tooltip("그리기를 허용할 레이어 마스크")]
    [SerializeField] private LayerMask targetLayer;

    [Tooltip("라인렌더러가 그려지는 거리")]
    [SerializeField] float rayDistance;

    [Tooltip("생성되는 라인렌더러")]
    private LineRenderer lineRenderer;
    public LineRenderer LineRenderer { set { lineRenderer = value; } }

    [Tooltip("생성되는 라인렌더러 색상")]
    [SerializeField] Color materialColor;

    [Tooltip("Defalut-Line 으로 설정할 것")]
    public Material drawingMaterial;

    [Tooltip("라인렌더러 굵기")]
    [Range(0f, 1f)]
    [SerializeField] private float width;

    [Tooltip("라인렌더러 index")]
    [SerializeField] private int index;

    [Tooltip("Noraml 벡터 크기")]
    [SerializeField] private float NormalDis;

    [Tooltip("오브젝트를 잡았는지 확인용")]
    private bool isSelecting;

    [Tooltip("청소가 끝났는지 확인용")]
    private bool isSuccess;
    public bool IsSuccess { get { return isSuccess; } set { isSuccess = value; } }

    [Tooltip("렌더러 생성 및 지우기용")]
    private GameObject lineObject;
    public GameObject LineObject { get { return lineObject; } set { lineObject = value; } }

    private void Update()
    {
        if (isSelecting && !isSuccess)
        {
            Cleaning();
            //Debug.Log("청소중");
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

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, targetLayer))
        {
            if (lineRenderer == null) //이 부분에서 현재 물감에 알맞는 색상으로 만들어줘야 할 것 같아. 
            {
                //오브젝트 생성
                lineObject = new GameObject();
                //오브젝트 위치 지정
                lineObject.transform.position = hit.point + hit.normal * 0.01f;
                lineObject.transform.rotation = Quaternion.identity;
                //라인렌더러 추가
                lineRenderer = lineObject.AddComponent<LineRenderer>();
                //라인렌더러 메터리얼 지정
                lineRenderer.material = new Material(drawingMaterial);
                //라인렌더러를 바닥과 일치하게 만들기
                lineRenderer.alignment = LineAlignment.TransformZ;
                lineRenderer.transform.rotation = Quaternion.Euler(90, 0, 0);

                // 현재 색상 설정
                lineRenderer.material.color = materialColor;

                //일정한 굵기
                lineRenderer.startWidth = lineRenderer.endWidth = width; 
                /*//굴곡
                lineRenderer.numCornerVertices = 0;
                lineRenderer.numCapVertices = 0;*/

                //렌더러 넘버당 위치 지정
                lineRenderer.positionCount = 1; // 시작 포지션 카운트 1
                lineRenderer.SetPosition(0, hit.point + hit.normal * 0.001f);

            }
            else //처음 위치가 생성된 경우. 
            {

                var currentPos = lineRenderer.GetPosition(index);
                //lineRenderer.material.color = materialColor;

                if (Vector3.Distance(currentPos, hit.point) > 0.01f)
                {
                    index++;
                    lineRenderer.positionCount = index + 1;
                    lineRenderer.SetPosition(index, hit.point + hit.normal * 0.001f);
                }
            }
        }
    }
}
