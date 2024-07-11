using System.Collections;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Jc
{
    /// <summary>
    /// Custom Ray Inetractor
    /// </summary>
    public class RayInteractor : XRRayInteractor
    {
        [Header("커스텀 세팅")]
        [SerializeField]
        private PlayerControllerCallback controllerCallback;

        [SerializeField]
        private RayInteractor oppositeInteractor;

        [SerializeField]
        private XRInteractorLineVisual lineVisual;
        [SerializeField]
        private LineRenderer lr;

        [SerializeField]
        private bool isLeftController = false;

        [SerializeField]
        private GameObject controller;

        [Header("밸런싱")]
        [SerializeField]
        private bool isGrab = false;
        public bool IsGrab {get { return isGrab; } }

        [SerializeField]
        private InteractObject currentGrabObject;  // 현재 잡고있는 오브젝트

        private Camera cam;                         // 메인 카메라
        private Transform grabbedTr;                // 그랩한 오브젝트 트랜스폼

        public InventorySlot hoveredSlot;

        [Tooltip("슬롯의 이미 색 변경을 위한 Image 컴포넌트")]
        private Image slotImage;

        public InventorySlot currentSlot = null; // 현재 레이캐스트가 닿은 슬롯을 추적하기 위한 변수

        private bool isInventoryMode = false;

        protected override void Awake()
        {
            base.Awake();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            cam = Camera.main;
        }
        protected override void OnEnable()
        {
            base.OnEnable();

            Manager.UI.OnPopUpChange += OnPopUp;
            if (isLeftController)
            {
                controllerCallback.leftTriggerRef.action.performed += OnSlotSelectEnter;
                controllerCallback.leftTriggerRef.action.canceled += OnSlotSelectExit;
            }
            else
            {
                controllerCallback.rightTriggerRef.action.performed += OnSlotSelectEnter;
                controllerCallback.rightTriggerRef.action.canceled += OnSlotSelectExit;
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Manager.UI.OnPopUpChange -= OnPopUp;
            // 인벤토리 콜백 등록
            if (isLeftController)
            {    
                controllerCallback.leftTriggerRef.action.performed -= OnSlotSelectEnter;
                controllerCallback.leftTriggerRef.action.canceled -= OnSlotSelectExit;
            }
            else
            {
                controllerCallback.rightTriggerRef.action.performed -= OnSlotSelectEnter;
                controllerCallback.rightTriggerRef.action.canceled -= OnSlotSelectExit;
            }

        }

        private void Update()
        {
            if(isInventoryMode)
                FindSlot();
        }

        #region 정재훈 인벤토리 주석
        //인벤토리에서 아이템 ----< 꺼낼 > 때 체크해줘야하는 Enter 함수 
        // 스택형 아이템은 destroy했기 때문에 생성한 다음에 손에 붙여줘야 한다는 것 잊지 말기. 
        //public void OnSlotTriggerEnter(InputAction.CallbackContext context)
        //{
        //    if (Manager.Inventory.isEnable == false)
        //    {
        //        return;
        //    }
        //    if (currentSlot == null)
        //    {
        //        return;
        //    }
        //    FindSlot();

        //    /*if (isGrab == true) // 잡고 있을 때도 빼는 상황 진입해야 하지 않나? 생각해보기. 
        //    {
        //        return;
        //    }*/

        //    //--> 현재 여기가 오류가 뜨고 있다. 0이 없는상황임. --> 이 부분만 해결하면 바로바로 다시 꺼내기 가능해짐.
        //    if (currentSlot == null || currentSlot.ItemID == -1 || currentSlot.currentItem == null)
        //        return;

        //    // 슬롯에서 hover 시 아이템 입 출입 무한 반복 방지 
        //    currentSlot.SetRayHovering(true);

        //    InventoryItem item = currentSlot.currentItem;

        //    // Event 아이템은 체크 할 필요가 없다 -> 어차피 interactor가 불가능하도록 설정되었기 때문에.

        //    // 스택 아이템이 여러개 있는 상황이라면 (실제로 여러개가 있는게 아니기 때문에 생성을 해서 손에 붙여줘야한다.)
        //    if (currentSlot.ItemCount >= 2) // 2개 이상이라면.
        //    {
        //        currentSlot.MinusItemNumber();

        //        Debug.Log("2개 이상 일 때의 아이템의 카운트 -->" + currentSlot.ItemCount);


        //        // 이미 잡을 수 있다는 이 ifㅣ문 안으로 들어온거 자체가 이미 xrgrab이 가능하다는 의미다.
        //        Vector3 newItemScale = item.originalScale; // 원본 아이템의 오리지널 scale을 복사해온다.
        //        InventoryItem newItem = Instantiate(item.gameObject, item.transform.position, item.transform.rotation).GetComponent<InventoryItem>();

        //        Debug.Log("슬롯에 있는 아이템의 저장된 오리지널 스케일" + newItemScale);

        //        newItem.originalScale = newItemScale; // 오리지널 스케일 덮어써서 재설정해주기. 

        //        Debug.Log("저장될 슬롯에 있는 아이템의 오리지널 스케일" + newItemScale);
        //        Debug.Log($"새롭게 생성된 오브젝트의 스케일 상태 -> {newItem.transform.localScale}");
        //        Debug.Log($"새롭게 생성된 오브젝트의 오리지널 스케일 필드 값 ->{newItem.originalScale}");

        //        newItem.transform.localScale = newItemScale;
        //        newItem.SaveScale(); // 세이브를 다시 저장? 

        //        this.interactionManager.SelectEnter(this as IXRSelectInteractor, newItem as IXRSelectInteractable);
        //    }
        //    else if (currentSlot.ItemCount == 1) // 1개 라면 그냥 하던대로 작업해주면 된다. 
        //    {
        //        Debug.Log("아이템 카 운트가 1 인 상황" + currentSlot.ItemCount);
        //        currentSlot.MinusItemNumber();

        //        currentSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, currentSlot.currentItem as IXRSelectInteractable);
        //        currentSlot.SetRayHovering(false);
        //        Debug.Log("1개 일 때의 스택 아이템 remove");
        //    }
        //}

        // 인벤토리에 아이템을 ----< 추가 > 할 때 체크할 Exit 함수 
        //public void OnSlotTriggerExit(InputAction.CallbackContext context)
        //{
        //    if (Manager.Inventory.isEnable == false)
        //    {
        //        return;
        //    }
        //    if (currentSlot == null)
        //    {
        //        return;
        //    }
        //    if (isGrab == false)
        //    {
        //        return;
        //    }

        //    currentSlot.SetRayHovering(true);

        //    // 내 손에서 인벤토리로 넘겨주기. 

        //    if (currentGrabObject == null)
        //    {
        //        return;
        //    }
        //    InventoryItem item = currentGrabObject as InventoryItem;    // 내가 들고있는 아이템

        //    if (item == null)
        //    {
        //        return;
        //    }

        //    if (currentSlot.ItemCount >= 1) //이미 내부에 아이템이 있을 때. (스택 타입 아이템)
        //    {
        //        // slotID 안의 아이템 ID 체크
        //        if (item.itemData.itemID == currentSlot.ItemID)
        //        {
        //            Debug.Log("같은 스택 아이템 추가됨.");
        //            // 함수를 나눠서 count를 증가 시키는 함수를 따로 만들고 destroy 하기. 

        //            currentSlot.AddItemNumber();
        //            Destroy(item.gameObject);
        //            Debug.Log("똑같은 스택형 아이템 추가 후 아이템 파괴.");
        //        }
        //        else //내가 들고 있는 아이템이 스택형 아이템일때 slot 내부의 아이템이 다르다면. (어차피 
        //        {
        //            interactionManager.SelectExit(this as IXRSelectInteractor, item as IXRSelectInteractable);
        //        }
        //    }
        //    else //내부에 아이템이 없을 때 (스택 타입 아이템)
        //    {
        //        // 그런데 current item 이랑 어차피 똑같은거 아닌가 싶은대 
        //        this.interactionManager.SelectEnter(currentSlot as IXRSelectInteractor, item as IXRSelectInteractable);
        //        currentSlot.AddItemNumber();
        //        Debug.Log("스택 타입 아이템 빈 곳에 투입함.");
        //    }
        //}
        #endregion

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            IInteractable itrObject = interactable as IInteractable;

            if (itrObject == null)
                return false;

            return base.CanHover(interactable);
        }
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            IInteractable itrObject = interactable as IInteractable;
            
            if (itrObject == null)
                return false;

            return base.CanSelect(interactable);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            currentGrabObject = args.interactableObject as InteractObject; // 현재 플레이어가 쥐고 있는 아이템. 

            grabbedTr = args.interactableObject.transform;
            isGrab = true;

            // 착시 오브젝트의 경우 무조건 한 손으로만 상호작용 해야함.
            // 반대 인터렉터의 오브젝트 강제로 놓기
            if (oppositeInteractor.isGrab
                && oppositeInteractor.currentGrabObject != null
                && (currentGrabObject is ResizingObject
                || currentGrabObject is PhotoFrame))
            {
                Debug.Log("Opposite interactor select exit");
                oppositeInteractor.interactionManager.SelectExit(oppositeInteractor as IXRSelectInteractor, oppositeInteractor.currentGrabObject as IXRSelectInteractable);
            }

            // 오브젝트 활성화
            if (args.interactableObject is IActivatable)
            {
                IActivatable active = args.interactableObject as IActivatable;
                active.Activate();

                if (active is InteractObject)
                {
                    InteractObject obj = active as InteractObject;
                    if (obj != null)
                    {
                        this.interactionManager.SelectExit(this as IXRSelectInteractor, obj as IXRSelectInteractable);
                        Debug.Log("active 오브젝트 놓기");
                    }

                }
            }
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            ItemObject item = currentGrabObject as ItemObject;
            if (item != null)
                item.ResetScale();

            currentGrabObject = null;

            isGrab = false;
            grabbedTr = null;
        }

        private void OnSlotSelectEnter(InputAction.CallbackContext context)
        {
            if (!Manager.UI.OnPopup || currentSlot == null)
                return;

            if (isGrab || oppositeInteractor.IsGrab)
                return;

            ItemObject item = currentSlot.TakeOutItem();
            if (item == null)
                return;

            item.SetScale();
            interactionManager.SelectEnter(this as IXRSelectInteractor, item as IXRSelectInteractable);
        }
        private void OnSlotSelectExit(InputAction.CallbackContext context)
        {
            if (!Manager.UI.OnPopup || currentSlot == null)
                return;

            if (!isGrab || currentGrabObject == null)
                return;

            ItemObject item = currentGrabObject as ItemObject;
            if (item == null)
                return;

            currentSlot.PutInItem(item);
        }

        // 오브젝트를 잡을 수 있는 거리체크
        private bool GrabableDistance(IInteractable itrObject)
        {
            if (itrObject == null)
                return false;

            // 오브젝트의 그랩 허용 길이
            float grabDist = itrObject.GetInteractDistance();
            // 현재 오브젝트와의 거리
            float distance = (itrObject.GetTransform().position - transform.position).sqrMagnitude;

            if (distance > grabDist * grabDist)
            {
                return false;
            }

            return true;
        }

        // UI 매니저 콜백
        private void OnPopUp()
        {
            isInventoryMode = Manager.UI.OnPopup;

            if (isInventoryMode)
            {
                ItemObject item = currentGrabObject as ItemObject;

                if (item != null)
                    item.SetScaleWithLerp();
                lr.enabled = true;
                //lineVisual.enabled = false;
            }
            else
            {
                ItemObject item = currentGrabObject as ItemObject;

                if (item != null)
                    item.ResetScaleWithLerp();
                lr.enabled = false;
                //lineVisual.enabled = true;
            }
            //lr.enabled = true;
        }
        private void FindSlot()
        {
            if (currentGrabObject != null && currentGrabObject is not ItemObject)
                return;

            Ray ray = new Ray(transform.position, transform.forward);
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 6f);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 6f, Manager.Layer.slotLM))
            {
                InventorySlot slot = hitInfo.transform.GetComponent<InventorySlot>();

                if (slot == null)
                {
                    if (currentSlot != null)
                        currentSlot.OnHoverExit();

                    currentSlot = null;
                    return;
                }

                if (currentSlot != null && currentSlot != slot)
                    currentSlot.OnHoverExit();

                currentSlot = slot;
                currentSlot.OnHoverEnter();
            }
            else
            {
                if (currentSlot != null)
                    currentSlot.OnHoverExit();

                currentSlot = null;
            }
        }
    }
}


