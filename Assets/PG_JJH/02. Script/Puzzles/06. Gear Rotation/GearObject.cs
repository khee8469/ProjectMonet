using Jc;
using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class GearObject : InventoryItem
    {
        // 인벤토리에 들어가고 잡을 수 있는 아이템

        [Header("할당된 아이템의 ID")] // 프리팹 연동 필요함. 
        [SerializeField] private int itemID;

        [Tooltip("실제 기어의 ID")]
        [SerializeField] private int gearID;
        
        [Header("퍼즐 매니저 에디터 세팅")]
        [SerializeField]
        private PuzzleManager puzzle;

        [Tooltip("기어가 회전 할 방향을 정해 줄 int 값")]
        [SerializeField]
        private float rotationDirection = 60f;

        private void Start()
        {
            if(gearID == -1) // 장식용 기어들한테 넣어줄 예정 
            {
                interactionLayers = 0; // 움직여서는 안되는 오브젝트라면 nothing으로 설정 < 0 > 
            }

            Physics.SyncTransforms();

        }

        // 완료 시 톱니바퀴의 회전 시작.
        public void StartRotate()
        {
            Debug.Log("코루틴 호출 됨");
            StartCoroutine(RotationRoutine(rotationDirection));
        }

        private IEnumerator RotationRoutine(float Direction)
        {
            while(true)
            {
                transform.Rotate(0, Direction * Time.deltaTime, 0);

                yield return null;  
            }

        }


    }

}
