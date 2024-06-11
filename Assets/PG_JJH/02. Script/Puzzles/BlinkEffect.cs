using System.Collections;
using UnityEngine;

namespace JJH
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlinkEffect : MonoBehaviour
    {
        // 일정 시간이 지나면 힌트를 발동시키는 스크립트
        // 그냥 모든 힌트 오브젝트들이 이 스크립트를 가지도록 하고 각자 id를 가지자. 
        [Header("힌트 관련 체크 필드")]
        [Tooltip("각각 오브젝트의 ID")]
        [SerializeField] int itemID; // 각 아이템의 아이디로 코루틴을 조절. 
        [SerializeField] private LayerMask targetLayer;
        [Tooltip("힌트 발동까지 걸리는 시간")]
        [SerializeField] float blinkThreshold = 30f; //일단 30초로 




        // oop에 따른 각각의 필드는 각각 다른 인스턴스의 필드라고 한다... 
        private Coroutine blinkCoroutine; //while문 진입 코루틴



        [SerializeField] private float elapseTime; //누적 시간  

        [Header("반짝임 관련 마테리얼 ")]
        [Tooltip("오브젝트의 원본 마테리얼 ")]
        [SerializeField] Material objectMaterial;
        [Tooltip("오브젝트의 원래 색상 저장")]
        [SerializeField] Color originalColor;
        [Tooltip("반짝이는 색상")]
        [SerializeField] private Color blinkColor = Color.blue;


        private void Start()
        {
            targetLayer = LayerMask.GetMask("Player");
            objectMaterial = GetComponent<Renderer>().material;
            originalColor = objectMaterial.GetColor("_EmissionColor");

            // 마테리얼의 emiision을 보장 ? 
            objectMaterial.EnableKeyword("_EMISSION");
        }

        private void OnTriggerEnter(Collider other)
        {

            if (Extension.Contain(targetLayer, other.gameObject.layer)) //Player 레이어를 체크해서 발동. 
            {
                Debug.Log("플레이어 트리거");
                if (blinkCoroutine == null)
                {
                    StartCoroutine(BlinkRoutine(itemID)); // 코루틴 시작
                }
            }
        }

        private void OnTriggerExit(Collider other)  // 그래도 player가 나가면 계속 반짝일 이유는 없으므로 일단 중지 
        {
            if (Extension.Contain(targetLayer, other.gameObject.layer))
            {
                Debug.Log("플레이어 엑시트");
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                    blinkCoroutine = null;
                }
                ResetBlink();
            }
        }


        protected void Update()
        {
            
        }


        IEnumerator BlinkRoutine(int _itemID)
        {
            if (_itemID != itemID)
            {
                yield break; // id가 맞지 않으면 코루틴 탈출 --> 트리거 된 객체만 제대로 블링크 할 수 있도록 
            }

           // elapseTime = 0f; -->나갔다 왔을 때 초기화 원하면 주석 삭제 


            while (true)
            {
                elapseTime += Time.deltaTime;

                if (elapseTime >= blinkThreshold)
                {
                    blinkCoroutine = StartCoroutine(Blink());
                    yield break;
                }

                yield return null;
            }
        }

        // 실제로 빛을 내고 있는 블링크 루틴 --> 중지시키려면 여기를 중지 시켜줘야함. 
        IEnumerator Blink()
        {

            float blinkDuration = 0.5f; // 반짝이는 간격

            while (true)
            {
                objectMaterial.SetColor("_EmissionColor", Color.blue);
                yield return new WaitForSeconds(blinkDuration);

                objectMaterial.SetColor("_EmissionColor", originalColor);
                yield return new WaitForSeconds(blinkDuration);
            }
        }

        private void ResetBlink()
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            objectMaterial.SetColor("_EmissionColor", originalColor);
        }

        public void StopBlink() //어차피 해당 소켓 이나 힌트 완료 지점에서 getcomponent로 가져오는 스크립트이므로
                                // 매개변수 가질 필요 없음 
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                ResetBlink();
            }
        }


    }
}


