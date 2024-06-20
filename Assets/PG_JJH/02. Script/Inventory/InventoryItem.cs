using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
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

        private void Start()
        {
            InitializeItemData();
            isGraped = false;
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

        //XR interaction을 똑같이 상속 하기 때문에 소켓에 닿앗을 때도 발동하는 문제가 발생한다. 
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if (args.interactorObject is XRSocketInteractor)
            {               
                Debug.Log($"{gameObject.name} is now socketed.");
            }
            else if (args.interactorObject is XRGrabInteractable)
            {
                isGrabbed = true;
                Debug.Log($"{gameObject.name} is now being grabbed."); 
            }

        }

        // 아이템이 놓이는 순간에 슬롯 안에 있는지 확인.
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            StartCoroutine(ExitRoutine()); // 0.7초 딜레이 

            if (args.interactorObject is XRSocketInteractor)
            {
                Debug.Log($"{gameObject.name} is no longer socketed.");
            }
            else if (args.interactorObject is XRGrabInteractable)
            {
                isGrabbed = false;
                Debug.Log($"{gameObject.name} is no longer being grabbed.");
            }

        }
        private IEnumerator ExitRoutine()
        {
            yield return new WaitForSeconds(0.7f);
        }

        

    }
}



