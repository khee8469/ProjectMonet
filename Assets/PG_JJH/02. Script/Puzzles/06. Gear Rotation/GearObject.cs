using Jc;
using System.Collections;
using UnityEngine;

namespace JJH
{
    public class GearObject : ItemObject  // 인벤토리에 들어가는 아이템 이므로 이것만 있어도 ok! 
    {
        // 재천이가 만든 거로 통일해서 id 체크 해주는 식으로 하고 
        // 일단 임시로 기어 id 써서 매핑 시키기. 

        [Tooltip("실제 기어의 ID")]
        public int gearID;

        [Tooltip("기어가 회전 할 방향을 정해 줄 int 값")]
        [SerializeField]
        private float rotationDirection;

        [Tooltip("정답 판정 시 생성해줄 너트")]
        public GameObject nut;

        [Tooltip("꺼줄? 기어 콜라이더--> 안쓸듯")]
        [SerializeField] public Collider col;

        [Tooltip("리지드바디 ")]
        private Rigidbody rb;

        private void Start()
        {
            nut.gameObject.SetActive(false); // 일단 시작 시 에는 꺼주기. --> 완성 시에 생성해 줄 nut 
            trackRotation = true;

            rotationDirection = Random.Range(5, 10);

            rb = GetComponent<Rigidbody>();

        }

        // 완료 시 톱니바퀴의 회전 시작. --> 얘네는 어차피 지금 참조 없어. 뭐지 뭐가 문제냐??? 실행을 안하는데
        public void StartRotate()
        {
            StartCoroutine(RotationRoutine(rotationDirection));
        }

        public void OnNutActive()
        {
            nut.gameObject.SetActive(true);
        }

        private IEnumerator RotationRoutine(float Direction)
        {
            while (true)
            {
                transform.Rotate(0, 0, Direction * Time.deltaTime);
                yield return null;
            }
        }
        public void RigidChange()
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            if (col != null)
            {
                col.enabled = false;
            }


        }
    }

}
