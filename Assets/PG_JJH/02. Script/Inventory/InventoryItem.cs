using Jc;
using System.Collections;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    // 아이템을 체크 하기 위한 인터페이스 상속 --> 실제 아이템에 붙을 친구.
    public class InventoryItem : InteractObject, IInventory
    {
        // 실제 아이템이 가지고 있을 아이템의 기본적인 id , 타입 등의 데이터 
        [Header("아이템이 가지고 있을 데이터")]
        public InvenItem itemData; // 실제 아이템의 데이터 --가지고 있어야 데이터 쓸 수 있을듯? 
        private Transform originalParent; // 원래 부모 trasform 

        [Tooltip("현재 그랩되어 있는지를 확인하는 bool 변수")]
        private bool isGraped;

        public bool ISGraped { get { return isGraped; } }

        public Vector3 grabbedScale = new Vector3(0.01f, 0.01f, 0.01f);
        public Vector3 originalScale;

        public static UnityEvent RestoreSclaeObject = new UnityEvent();


        private void Start()
        {
            InitializeItemData();
            isGraped = false;

            // 아 그냥 이거 각 오브젝트마다 시작할 때 자신의 transform을 저장하고 시작하자. 
            originalScale = transform.localScale;
            trackScale = false;

            RestoreSclaeObject.AddListener(RestoreScale);

        }

        public void SaveScale()
        {
            itemData.SaveOriginalTransform(transform); //자신의 오브젝트의 트랜스폼을 저장해준다. 
        }

        public void RestoreScale()
        {
            itemData.RestoreOriginalTransform(transform);
        }

        private void InitializeItemData()
        {
            // 잘못된 방법. 할당해주는 건 Inspector 에서 하는거고 이거를 하면 new로 빈 껍데기생성임
            // 추가로 인스펙터 할당이 아니라 데이터 테이블을 이용해서 아이템을 받고 매칭 시켜야함. 
            /*if (itemData == null) 
            {
                itemData = new InvenItem();
                Debug.Log("itemData가 null이어서 새로운 InvenItem 인스턴스를 생성했습니다.");
            }*/
        }


        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
        }

        public void AdjustScale()
        {
            transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }

        //XR interaction을 똑같이 상속 하기 때문에 소켓에 닿앗을 때도 발동하는 문제가 발생한다. 
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            SaveScale();
            // 추후에 GetComponenet 등으로 한 다면 이 부분 수정 할 것. --> 최적화 필요한 부분 
            if (args.interactorObject.transform.GetComponentInParent<XROrigin>())
            {
                // 사람에게 붙잡혔을 때 해야하는 작업? 
            }
        }
        // 아이템이 놓이는 순간에 슬롯 안에 있는지 확인.
        // 이거 interactor 에서 exit을 발동시킬 수 있도록 할 수가 있나? 
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
            Debug.Log("아이템의 셀렉트 엑시팅");
            RestoreScale();
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            Debug.Log("아이템의 셀렉티드 엑시트");
            base.OnSelectExited(args);
            RestoreScale();
        }

        //IsGrab 쓰게 되면 사용할 코루틴 
        private IEnumerator ExitRoutine()
        {
            yield return new WaitForSeconds(0.7f);
        }



    }
}



