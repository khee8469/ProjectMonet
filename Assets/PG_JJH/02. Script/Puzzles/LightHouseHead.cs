using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using Jc;

namespace JJH
{

    public class LightHouseHead : MonoBehaviour, IPuzzleable
    {
        [Tooltip("퍼즐 매니저")]
        [SerializeField]
        private PuzzleManager puzzleManager;

        [Tooltip("체크 해야 할 검은 구멍")]
        [SerializeField]
        private Chapter2SunHole sunHole;

        [Tooltip("레이 캐스트 발사 위치")]
        [SerializeField]
        private Transform rayStartPos;

        [Tooltip("퍼즐의 On OFF 상태 체크 --> 임시 (나중에 Manager와 연계 할 것")]
        [SerializeField] private bool puzzleOn;

        [Tooltip("닿아야 할 태양구멍의 layer --> Puzzle")]
        [SerializeField] LayerMask layerMask;

        [Tooltip("보여 줄 빛 기둥")]
        [SerializeField] private GameObject pillar_Of_Light;

        [Tooltip("레이캐스트 발사 거리")]
        [SerializeField] private float distance = 4000f;

        private void Start()
        {
            //pillar_Of_Light.gameObject.SetActive(false);
        }

        private void Update()
        {
            // 퍼즐이 On 된 상태에서만 RayCast 및 빛 기둥 발사를 해야한다.
            if(puzzleOn ==true)
            {
                // 다른 함수로 Pillar 를 Active 하는 부분을 CallBack 으로 불러서 (이전 퍼즐에서)
                // 켜줘야 한다. Ray도 마찬가지고 
                RayOn();
            }
        }
        private void RayOn()
        {
            RaycastHit hit;
            Debug.DrawRay(rayStartPos.position, rayStartPos.forward * distance, Color.red);
            if (Physics.Raycast(rayStartPos.position , rayStartPos.forward , out hit , distance, layerMask))
            {
                // 이미 레이어 마스크 체크로 들어갔기 때문에 그냥 레이가 부딪혔으면 부딪힌 거임.
                sunHole = hit.collider.GetComponent<Chapter2SunHole>();
                if(sunHole != null)
                {
                    Debug.Log("구멍 체크 완료");
                }
            }
        }

        public void PuzzleOn()
        {
            puzzleOn = true;
            Debug.Log("등대 퍼즐 켜짐");
        }




        public void ActiveSetting()
        {

        }

        public void CompleteSetting()
        {

        }

        public void DisActiveSetting()
        {

        }

        public void RegistObject(PuzzleManager puzzle)
        {

        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {

        }
    }
}


