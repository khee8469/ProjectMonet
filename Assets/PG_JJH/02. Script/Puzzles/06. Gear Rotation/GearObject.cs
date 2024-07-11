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
        // 나중에 상속하는거 itemobject ? 재천이 꺼로 바꾸고
        // 추가로 기어 4개가 모두 정상적으로 완료되면 active true로 자신의 볼트 생성
        // 벽에 걸 때 내 손이 아니면 들어가서는 안되고 밑으로 쭉 떨어져야함. --> rigidbody 필수. 



        [Header("할당된 아이템의 ID")] // 프리팹 연동 필요함. 
        [SerializeField] private int itemID;

        [Tooltip("실제 기어의 ID")]
        [SerializeField] private int gearID;

        [Tooltip("기어가 회전 할 방향을 정해 줄 int 값")]
        [SerializeField]
        private float rotationDirection = 60f;

        private void Start()
        {
            if (gearID == -1) // 장식용 기어들한테 넣어줄 예정 
            {
                interactionLayers = 0; // 움직여서는 안되는 오브젝트라면 nothing으로 설정 < 0 > 
            }
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args) // 재상속을 이미 했는데 도대체 왜 스케일이 변하냐?
        {
            Debug.Log($"Object Position: {transform.position}");
            Debug.Log($"Object Scale: {transform.localScale}");
            base.OnSelectEntered(args);
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
        }


        // 완료 시 톱니바퀴의 회전 시작. --> 얘네는 어차피 지금 참조 없어. 뭐지 뭐가 문제냐??? 실행을 안하는데
        public void StartRotate()
        {
            Debug.Log("코루틴 호출 됨");
            StartCoroutine(RotationRoutine(rotationDirection));
        }

        private IEnumerator RotationRoutine(float Direction)
        {
            while (true)
            {
                transform.Rotate(0, Direction * Time.deltaTime, 0);

                yield return null;
            }

        }


    }

}
