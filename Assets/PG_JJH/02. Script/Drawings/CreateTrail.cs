using UnityEngine;
using JJH;

/// <summary>
/// This script creates a trail at the location of a gameobject with a particular width and color.
/// </summary>

namespace JJH
{
    public class CreateTrail : MonoBehaviour
    {
        /* // 붓에 붙어야하는 스크립트 

         [Header("그림 그리기 관련")]
         [Tooltip("렌더러 프리팹")]
         public GameObject trailPrefab = null; // 트레일렌더러 프리팹은 인스펙터차에서 할당됨. 

         [Tooltip("그리기 기능 on/off bool 변수")]
         [SerializeField] bool isPainting;

         [Header("그리기 범위 판정 레이캐스트")]
         [Tooltip("레이 캐스트 체크 layer")]
         [SerializeField] LayerMask targetLayer;
         [Tooltip("레이캐스트 시작 위치 -자식 오브젝트")]
         [SerializeField] Transform rayCastStartPos;
         [Tooltip("레이캐스트 범위")]
         private float distance = 1;

         // width와 color를 내부에서 관리하고 있는데 이 부분을 (결국 다른 스크립트를 참조하거나 해줘야함...)
         // 물감 오브젝트와 연계하여 color를 변경시켜주는 방식으로 진행해보자. 

         private float width = 0.005f;
         private Color color = Color.black;

         private GameObject currentTrail = null;

         // 누르고 잇는 상태 일때만 나오도록 해야함 (마우스 좌클릭 ) 

         private void Start() // 막 삭제되는 붓은 아니니까 그냥 start에서 하면 되나? 
         {
             targetLayer = LayerMask.GetMask("DrawBoard");
             rayCastStartPos = transform.Find("RayPos");
         }

         public void StartTrail()
         {
             if (!currentTrail)
             {
                 // RayCast 체크 
                 RaycastHit hit;
                 if(Physics.Raycast(rayCastStartPos.position,rayCastStartPos.forward, out hit,distance, targetLayer ))
                 {
                     // 생성 위치를 이렇게 해주니까... 아마 딱 붙어서 안에 들어간것 같은데? 
                     // vector normal 로 조금 앞으로 해줄까?

                     *//*currentTrail = Instantiate(trailPrefab, hit.transform.localPosition + (hit.normal*1f)
                         ,transform.rotation, hit.transform);*//*

                     currentTrail = Instantiate(trailPrefab, transform.position,
                         transform.rotation, transform);

                     ApplySettings(currentTrail);
                 }
             }
         }

         private void Update()
         {
             // 레이 범위 그리기
             Debug.DrawRay(rayCastStartPos.position, rayCastStartPos.forward * distance, Color.red);

         }

         private void ApplySettings(GameObject trailObject)
         {
             TrailRenderer trailRenderer = trailObject.GetComponent<TrailRenderer>();
             trailRenderer.widthMultiplier = width;
             trailRenderer.startColor = color;
             trailRenderer.endColor = color;
         }

         public void EndTrail()
         {
             if (currentTrail)
             {
                 currentTrail.transform.parent = null;
                 currentTrail = null;
             }
         }

         public void SetWidth(float value)
         {
             width = value;
         }

         public void SetColor(Color value)
         {
             color = value;
         }*/

        [SerializeField] GameObject linePrefab;
        [SerializeField] LayerMask targetLayer;

        [SerializeField] Transform rayCastStartPos;

        //[SerializeField] private float distance = 1f;

        private float width = 0.01f;
        private Color color = Color.black;

        private GameObject currentLine = null;

        private void Start()
        {
            targetLayer = LayerMask.GetMask("DrawBoard");
            rayCastStartPos = transform.Find("RayPos");
        }

        public void StartTrail()
        {
            if (!currentLine)
            {
                /*RaycastHit hit;

                if (Physics.Raycast(rayCastStartPos.position, rayCastStartPos.forward,
                        out hit, distance, targetLayer))
                {
                    currentLine = Instantiate(linePrefab, hit.point,
                        transform.rotation, hit.transform);

                    ApplySettings(currentLine);
                }*/

                currentLine = Instantiate(linePrefab, transform.position, transform.rotation, transform);
                ApplySettings(currentLine);

            }
        }

        private void ApplySettings(GameObject lineObject)
        {
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.widthMultiplier = width;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        public void EndTrail()
        {
            if (currentLine)
            {
                currentLine.transform.parent = null;
                currentLine = null;
            }
        }
        public void SetWidth(float value)
        {
            width = value;
        }

        public void SetColor(Color value)
        {
            color = value;
        }



    }

}
