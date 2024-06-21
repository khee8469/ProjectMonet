using JJH;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
        //인벤토리에서 아이템 꺼낼 때 체크해줘야하는 Enter 함수 
        public void OnSlotTriggerEnter(InputAction.CallbackContext context)
        {
            InventorySlot curSlot = FindSlot();

            if (curSlot == null)
            {
                Debug.Log("curSlot is null");
                return;
            }

            if (JJH.Manager.Inventory.isEnable == false)
            {
                Debug.Log("Inventory is not enabled");
                return;
            }
            if (isGrab == true)
            {
                return;
            }

            //if (currentGrabObject == null) return;

            Debug.Log("꺼내기 시도");
            // 인벤토리에서 내 손으로 옮겨줘야 하고 
            curSlot.SetRayHovering(true);
            XRSocketInteractor A = curSlot.GetComponent<XRSocketInteractor>();

            Debug.Log(A.name);

            // 여기서 >0 이어도. 그 내부의 curslot의 item이 stack형의 아이템이라면
            // 꺼낼 때 또 분기 처리 해줘야한다. 


            if (curSlot.interactablesSelected.Count > 0)
            {
                curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, A.interactablesSelected[0] as IXRSelectInteractable);
                curSlot.SetRayHovering(false);
                StartCoroutine(startHoverRouitne());
            }
            Debug.Log($"enter  --> 아이템 꺼낼 시 : {curSlot}");
        }

        // 아이템을 빼는 순간에 다시 아이템이 들어가는 상황을 방지하기 위한 코루틴 딜레이
        // 그런데 이거 작동하나? 안하는거 같은데.
        private IEnumerator startHoverRouitne()
        {
            yield return new WaitForSeconds(0.5f);
        }

        // 인벤토리에 아이템을 추가 할 때 체크할 Exit 함수 
        public void OnSlotTriggerExit(InputAction.CallbackContext context)
        {
            InventorySlot curSlot = FindSlot();
            Debug.Log(curSlot);
            if (curSlot == null)
            {
                Debug.Log("curSlot is null");
                return;
            }
            if (JJH.Manager.Inventory.isEnable == false)
            {
                Debug.Log("Inventory is not enabled");
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
                Debug.Log("currentGrabObject is null");
                return;
            }
            InventoryItem item = currentGrabObject as InventoryItem;

            if (item == null)
            {
                Debug.Log("currentGrabObject is not an InventoryItem");
                return;
            }

            // 아이템을 추가 할 때 슬롯에 아이템이 있다면 자신의 손으로 빼주는 로직. 
            XRSocketInteractor slotItem = curSlot.GetComponent<XRSocketInteractor>();
            if (slotItem != null)
            {
                if (slotItem.interactablesSelected.Count > 0) //지금 내부에 하나 이상 있으면. 
                {
                    // 손에 있는거를 넣고. --> 그게 그러면 [1] 인덱스일테니까 그거를 빼면 되지않을까?
                    this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
                    curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);
                }
                else
                {
                    this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
                }

            }

            /* if(slotItem != null)
             {
                 this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);

                 curSlot.interactionManager.SelectEnter(this as IXRSelectInteractor, slotItem.interactablesSelected[0] as IXRSelectInteractable);
             }*/
            Debug.Log($"exit -->아이템 추가 시 : {curSlot}");
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
                    SpriteRenderer renderer = hitSlot.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        renderer.color = Color.red; //임시 변경 코루틴 같은거로 바꾸기. 
                    }

                    return hitSlot;
                }

            }
            return null;
        }
    }
}
