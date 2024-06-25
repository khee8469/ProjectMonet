using UnityEngine;

namespace JJH
{
    public class Paint : MonoBehaviour
    {
        // < 물감 >에 붙일 친구 니까 이거를 이제 펜과 연계해서
        // 펜이 이 물감의 color를 가져와서 그 부분을 펜의 tip에 컬러에 넣는다.
        // 그 부분을 해줘야한다. 


        [Header("컬러 관련 데이터")]
        [Tooltip("컬러 열거형을 설정함.")]
        public PaintTypeEnum paintType;

        [Tooltip("컬러 데이터 스크립터블 오브젝트")]
        public PaintTypeManager paintTypeManager;

        [Tooltip("각자 자신이 가지고 있는 컬러의 상태")]
        [SerializeField] private Color color;

        [Tooltip("물감이 충돌가능한 레이어")]
        [SerializeField] private LayerMask colliderLayer;

        private void Start() // 자신의 색깔을 시작할 때 가지고 오도록 (물감의 색깔임) 
        {
            if (paintTypeManager != null)
            {
                color = paintTypeManager.GetColorByType(paintType);
                GetComponent<Renderer>().material.color = color;
            }
            else
            {
                Debug.LogWarning("PaintTypeManager is not assigned.");
            }

        }

        public PaintTypeEnum GetPaintType()
        {
            return paintType;
        }


        public Color GetColorByType(PaintTypeEnum paintType)
        {
            if (paintTypeManager != null) // 스크립터블 오브젝트가 할당되어 있는 상태라면 
            {
                color = paintTypeManager.GetColorByType(paintType);
                GetComponent<Renderer>().material.color = color;

            }
            else
            {
                color = Color.white;
            }

            return color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("PaintPen"))
            {
                Debug.Log("트리거 진입함");
                Pen pen =other.gameObject?.GetComponent<Pen>();

                pen.ChangeColor(GetPaintType());

            }

            
            
        }
    }

}

