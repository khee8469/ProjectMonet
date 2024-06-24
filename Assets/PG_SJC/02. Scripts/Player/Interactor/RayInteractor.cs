using JJH;
using System.Collections;
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
        private bool isLeftController = false;

        [SerializeField]
        private GameObject controller;

        [SerializeField]
        private RectTransform canvasRect;
        private float canvasWidth;
        private float canvasHeight;
        [SerializeField]
        private RectTransform aimRect;

        [Header("밸런싱")]
        private bool isGrab = false;

        private InteractObject currentGrabObject { get; set; }   // 현재 잡고있는 오브젝트

        private XRInteractorLineVisual lineVisual;  // 라인 비주얼
        private LineRenderer lr;                    // 라인 렌더러
        private Camera cam;                         // 메인 카메라
        private Transform grabbedTr;                // 그랩한 오브젝트 트랜스폼

        public InventorySlot hoveredSlot;

        [Tooltip("슬롯의 이미 색 변경을 위한 Image 컴포넌트")]
        private Image slotImage;

        protected override void Awake()
        {
            base.Awake();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            lineVisual = GetComponent<XRInteractorLineVisual>();
            lr = GetComponent<LineRenderer>();
            cam = Camera.main;
            lineVisual.enabled = false;
            canvasHeight = canvasRect.sizeDelta.y;
            canvasWidth = canvasRect.sizeDelta.x;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (isLeftController)
            {
                controllerCallback.leftTriggerRef.action.performed += OnSlotTriggerEnter;
                controllerCallback.leftTriggerRef.action.canceled += OnSlotTriggerExit;
            }
            else
            {
                controllerCallback.rightTriggerRef.action.performed += OnSlotTriggerEnter;
                controllerCallback.rightTriggerRef.action.canceled += OnSlotTriggerExit;
            }
        }
        protected override void OnDisable()
        {
            if (isLeftController)
            {
                controllerCallback.leftTriggerRef.action.performed -= OnSlotTriggerEnter;
                controllerCallback.leftTriggerRef.action.canceled -= OnSlotTriggerExit;
            }
            else
            {
                controllerCallback.rightTriggerRef.action.performed -= OnSlotTriggerEnter;
                controllerCallback.rightTriggerRef.action.canceled -= OnSlotTriggerExit;
            }
            base.OnDisable();
        }
        private void Update()
        {
            AimPosition();

            // 잡고 있는 상황에서 인벤토리가 켜져있다면. --> 잡고 있는 오브젝트의 스케일을 조정해준다. 
            if (isGrab && JJH.Manager.Inventory.isEnable)
            {
                InventoryItem current = currentGrabObject.GetComponent<InventoryItem>();

                if (current != null)
                {
                    current.AdjustScale();
                    FindSlot(); // 일단 update에서 돌리는 거로 실험해보기. 
                }
            }
            else if (isGrab && JJH.Manager.Inventory.isEnable == false)
            {
                InventoryItem current = currentGrabObject.GetComponent<InventoryItem>();

                if (current != null)
                {
                    current.RestoreScale();
                }
            }
        }

        #region 컨트롤러 콜백
        //인벤토리에서 아이템 < 꺼낼 > 때 체크해줘야하는 Enter 함수 
        // 스택형 아이템은 destroy했기 때문에 생성한 다음에 손에 붙여줘야 한다는 것 잊지 말기. 
        public void OnSlotTriggerEnter(InputAction.CallbackContext context)
        {
            InventorySlot curSlot = FindSlot();

            if (curSlot == null)
            {
                return;
            }

            if (JJH.Manager.Inventory.isEnable == false)
            {
                return;
            }
            if (isGrab == true)
            {
                return;
            }

            InventorySlot slotItem = curSlot.GetComponent<InventorySlot>();
            if (slotItem == null) return;
            //if (currentGrabObject == null) return;
            if (curSlot.interactablesSelected.Count <= 0) return; // 슬롯에 아이템이 하나도 없는 경우. 
            IXRSelectInteractable xrGrab = curSlot.interactablesSelected[0];

            if (!(xrGrab is InteractObject objects))
            {
                Debug.Log("상호작용이 불가능한 이벤트용 아이템 --> 빼기 불가능");
                return;
            }

            curSlot.SetRayHovering(true);

            InventoryItem item = null;
            if (curSlot.interactablesSelected.Count > 0) // 일단 있어야 꺼낼 수 있다는 것. 
            {
                IXRSelectInteractable xRSelectInteractable = curSlot.interactablesSelected[0];

                if (xRSelectInteractable is InventoryItem inventoryItem)
                {
                    item = inventoryItem; // item -> 현재 슬롯에 넣어져 있는 아이템 

                    // Event 아이템은 체크 할 필요가 없다 -> 어차피 interactor가 불가능하도록 설정되었기 때문에.

                    if (item.itemData.stackType == StackTypeItem.Stackable) // 꺼내려는 아이템이 스택 타입이라면.
                    {
                        // 스택 아이템이 여러개 있는 상황이라면 (실제로 여러개가 있는게 아니기 때문에 생성을 해서 손에 붙여줘야한다.)
                        if (slotItem.ItemCount >= 2) // 2개 이상이라면.
                        {
                            slotItem.MinusItemNumber();

                            Debug.Log("2개 이상 일 때의 아이템의 카운트 -->" + slotItem.ItemCount);


                            // 이미 잡을 수 있다는 이 ifㅣ문 안으로 들어온거 자체가 이미 xrgrab이 가능하다는 의미다.
                            Vector3 newItemScale = item.originalScale; // 원본 아이템의 오리지널 scale을 복사해온다.
                            GameObject newItem = Instantiate(item.gameObject, item.transform.position, item.transform.rotation);

                            Debug.Log("슬롯에 있는 아이템의 저장된 오리지널 스케일" + newItemScale);

                            InventoryItem newInventoryItem = newItem.GetComponent<InventoryItem>();

                            newInventoryItem.originalScale = newItemScale; // 오리지널 스케일 덮어써서 재설정해주기. 

                            Debug.Log("저장될 슬롯에 있는 아이템의 오리지널 스케일" + newItemScale);
                            Debug.Log($"새롭게 생성된 오브젝트의 스케일 상태 -> {newInventoryItem.transform.localScale}");
                            Debug.Log($"새롭게 생성된 오브젝트의 오리지널 스케일 필드 값 ->{newInventoryItem.originalScale}");

                            newInventoryItem.transform.localScale = newItemScale;
                            newInventoryItem.SaveScale(); // 세이브를 다시 저장? 

                            Rigidbody rb = newInventoryItem.GetComponent<Rigidbody>();
                            rb.isKinematic = true; // 어차피 손에 붙으면 자동으로 kinematic 된다. 

                            this.interactionManager.SelectEnter(this as IXRSelectInteractor, newInventoryItem as IXRSelectInteractable);
                            rb.isKinematic = false;

                        }
                        else if (slotItem.ItemCount == 1) // 1개 라면 그냥 하던대로 작업해주면 된다. 
                        {
                            Debug.Log("아이템 카 운트가 1 인 상황" +slotItem.ItemCount);
                            slotItem.MinusItemNumber();

                            curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);
                            curSlot.SetRayHovering(false);
                            Debug.Log("1개 일 때의 스택 아이템 remove");
                        }
                    }
                    else if (item.itemData.stackType == StackTypeItem.Non_Stack) // 꺼내려는 아이템이 일반 아이템 이라면. 
                    {
                        curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);
                        curSlot.SetRayHovering(false);
                    }

                    //StartCoroutine(startHoverRouitne());
                }
            }
        }
        private IEnumerator startHoverRouitne()
        {
            yield return new WaitForSeconds(0.7f);
        }

        // 인벤토리에 아이템을 < 추가 > 할 때 체크할 Exit 함수 
        public void OnSlotTriggerExit(InputAction.CallbackContext context)
        {
            InventorySlot curSlot = FindSlot();
            if (curSlot == null)
            {

                return;
            }
            if (JJH.Manager.Inventory.isEnable == false)
            {

                return;
            }
            if (isGrab == false)
            {
                return;
            }
            curSlot.SetRayHovering(true);

            // 내 손에서 인벤토리로 넘겨주기. 

            if (currentGrabObject == null)
            {

                return;
            }
            InventoryItem item = currentGrabObject as InventoryItem;

            if (item == null)
            {

                return;
            }

            // 아이템을 <추가> 할 때 슬롯에 아이템이 있다면 자신의 손으로 빼주는 로직. 
            InventorySlot slotItem = curSlot.GetComponent<InventorySlot>();

            if (slotItem != null)
            {
                // item은 player가 들고 있는 아이템 slotItem은 현재 ray된 slot 
                if (item.itemData.stackType == StackTypeItem.Stackable) // 아이템이 스택 타입일 때
                {
                    if (slotItem.interactablesSelected.Count >= 1) //이미 내부에 아이템이 있을 때. (스택 타입 아이템)
                    {
                        // slotID 안의 아이템 ID 체크
                        if (item.itemData.itemID == slotItem.GetItemIDInSlot(slotItem.slotID))
                        {
                            Debug.Log("같은 스택 아이템 추가됨.");
                            // 함수를 나눠서 count를 증가 시키는 함수를 따로 만들고 destroy 하기. 

                            slotItem.AddItemNumber();
                            Destroy(item.gameObject);
                            Debug.Log("똑같은 스택형 아이템 추가 후 아이템 파괴.");

                        }
                        else //내가 들고 있는 아이템이 스택형 아이템일때 slot 내부의 아이템이 다르다면. (어차피 
                        {
                            IXRSelectInteractable interactable = slotItem.interactablesSelected[0];
                            InventoryItem slotInventoryItem = interactable.transform.GetComponent<InventoryItem>();

                            if (slotInventoryItem != null)
                            {
                                // 이 때 슬롯 내부의 아이템이 일반 아이템이라면 ( non - stack) 교체 가능해야함. 
                                if (slotInventoryItem.itemData.stackType == StackTypeItem.Non_Stack) // non stack 이라면 어차피 1개일 거니까. 
                                {
                                    slotItem.AddItemNumber();
                                    this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
                                    curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);

                                    // 이부분 SlectEnter라 크기 조정이 발동이 안되나?
                                    Debug.Log("일반 아이템 꺼내고 스택형 아이템 추가함.");
                                }
                                else // event 형 + 다른 id를 가진 stack 아이템 이면 추가 불가능. 
                                {
                                    slotItem.NotAddText(); // 다른 스택형 아이템에 스택형 아이템 투입 불가능. 
                                }
                            }
                        }
                    }
                    else if (slotItem.interactablesSelected.Count <= 0) //내부에 아이템이 없을 때 (스택 타입 아이템)
                    {
                        // 그런데 current item 이랑 어차피 똑같은거 아닌가 싶은대 
                        this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, item as IXRSelectInteractable);
                        slotItem.AddItemNumber();
                        Debug.Log("스택 타입 아이템 빈 곳에 투입함.");
                    }
                }
                else // 아이템이 일반 타입일 때. <추가 시에>
                {
                    if (slotItem.interactablesSelected.Count > 0) //지금 내부에 하나 이상 있으면. 
                    {
                        // 내부에 있는 아이템이 stack item 이면 count가 증가 해 있을 수 있기 때문에 count 제거 필요.

                        IXRSelectInteractable interactable = slotItem.interactablesSelected[0];
                        InventoryItem slotInventoryItem = interactable.transform.GetComponent<InventoryItem>();
                        if (slotInventoryItem != null)
                        {
                            if(slotInventoryItem.itemData.stackType==StackTypeItem.Stackable) // 내부에 있던 아이템이 스택 아이템이라면
                            {
                                if(curSlot.ItemCount>=2)
                                {
                                    slotItem.NotAddText(); // 다른 스택형 아이템에 스택형 아이템 투입 불가능. 
                                    Debug.Log("스택형 아이템이 여러개 일 때 일반형 아이템도 투입 불가능");
                                }
                                else if(curSlot.ItemCount<=1)
                                {
                                    slotItem.MinusItemNumber(); // 숫자 빼주기 필요. 
                                    this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
                                    curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);
                                    Debug.Log("내부의 스택아이템과 손에 있는 일반 아이템 교환");
                                }
                            }
                            else // non - stack item 이라면 
                            {
                                this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
                                curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);
                                Debug.Log("일반 아이템 과 일반아이템 교환");
                            }
                        }
                    }
                    else // 내부에 아무것도 없을 때 
                    {
                        this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
                        Debug.Log("일반 아이템 투입");
                    }
                }
            }
        }
        #endregion


        public override bool CanHover(IXRHoverInteractable interactable)
        {
            InteractObject itrObject = interactable as InteractObject;
            if (itrObject == null)
                return false;

            if (!GrabableDistance(itrObject))
                return false;

            return base.CanHover(interactable);
        }
        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            InteractObject itrObject = interactable as InteractObject;
            if (itrObject == null)
                return false;

            if (!GrabableDistance(itrObject))
                return false;

            return base.CanSelect(interactable);
        }

        // 플레이어가 아이템 잡은 상황. --> 인벤토리가 켜져있다면 이 CurrentGrabObject의 스케일 조정 필요
        // 

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            currentGrabObject = args.interactableObject as InteractObject; // 현재 플레이어가 쥐고 있는 아이템. 


            grabbedTr = args.interactableObject.transform;
            isGrab = true;
            lineVisual.enabled = true;
            aimRect.gameObject.SetActive(false);

            // save origin 값의 변경. 조정 필요할듯? 


        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);



            currentGrabObject = null;

            isGrab = false;
            grabbedTr = null;
            lineVisual.enabled = false;
            aimRect.gameObject.SetActive(true);
        }

        // 오브젝트를 잡을 수 있는 거리체크
        private bool GrabableDistance(InteractObject itrObject)
        {
            if (itrObject == null)
                return false;

            // 오브젝트의 그랩 허용 길이
            float grabDist = itrObject.GrabDistance;
            // 현재 오브젝트와의 거리
            float distance = (itrObject.transform.position - transform.position).sqrMagnitude;

            if (distance > grabDist * grabDist)
            {
                return false;
            }

            return true;
        }

        // 에임 포지셔닝
        private void AimPosition()
        {
            Vector2 viewportPos = Vector2.zero;

            // 오브젝트를 Select중 일 경우
            if (isGrab)
            {
                viewportPos = cam.WorldToViewportPoint(grabbedTr.position);
            }
            else
            {
                viewportPos = cam.WorldToViewportPoint(rayEndPoint);
            }

            Vector2 screenPos = new Vector2(
                ((viewportPos.x * canvasWidth) - (canvasWidth * 0.5f)),
                ((viewportPos.y * canvasHeight) - (canvasHeight * 0.5f)));

            aimRect.anchoredPosition = screenPos;
        }


        public GameObject hitBox = null; // 
        public InventorySlot currentSlot = null; // 현재 레이캐스트가 닿은 슬롯을 추적하기 위한 변수

        private InventorySlot FindSlot()
        {
            // 컨트롤러의 전방으로 레이캐스팅

            Ray ray = new Ray(transform.position + new Vector3(0, 0, 0.5f), transform.forward);
            Debug.DrawRay(transform.position, transform.forward * 500f, Color.red, 5f);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 500f, Manager.Layer.slotLM))
            {
                InventorySlot hitSlot = hitInfo.collider.GetComponent<InventorySlot>();

                if (hitSlot == null)
                    return null;
                else
                {
                    if (currentSlot != null && currentSlot != hitSlot)
                    {
                        // 이전에 레이캐스트가 닿았던 슬롯의 색상을 원래대로 돌려줌
                        currentSlot.slotImage.color = currentSlot.OriginalColor;
                    }

                    hitBox = hitSlot.gameObject;
                    hitSlot.slotImage.color = Color.red;
                    currentSlot = hitSlot;
                    return hitSlot;
                }
            }
            else
            {
                if (hitBox != null)
                {
                    InventorySlot slot = hitBox.GetComponent<InventorySlot>();
                    if (slot != null)
                    {
                        slot.slotImage.color = slot.OriginalColor;
                        hitBox = null;
                    }
                }

                if (currentSlot != null)
                {
                    // 내부에 아이템이 없을 때만 색을 다시 오리지널 컬러로 되돌려줌 
                    if(currentSlot.interactablesSelected.Count<=0)
                    {
                        currentSlot.slotImage.color = currentSlot.OriginalColor;                       
                    }

                    currentSlot = null; // 현재 레이캐스트가 닿은 슬롯 초기화
                }

                return null;
            }

        }
    }
}
