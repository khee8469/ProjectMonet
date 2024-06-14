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

        private void Start() // 자신의 색깔을 시작할 때 가지고 오도록 (물감의 색깔임) 
        {
            Color color = GetColorByType(paintType);
            GetComponent<Renderer>().material.color = color; // 마테리얼의 컬러 변경 

        }

        public Color GetColorByType(PaintTypeEnum paintType)
        {
            Color color;

            if (paintTypeManager != null) // 스크립터블 오브젝트가 할당되어 있는 상태라면 
            {
                color = paintTypeManager.GetColorByType(paintType);
                GetComponent<Renderer>().material.color = color;
                
            }
            else
            {
                color= Color.white;
            }

            return color; 
        }
    }

}

