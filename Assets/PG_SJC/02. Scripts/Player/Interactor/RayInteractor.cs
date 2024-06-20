using JJH;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private InteractObject currentGrabObject;   // 현재 잡고있는 오브젝트

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
            if(isLeftController)
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
            if(isLeftController)
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

            if (currentGrabObject == null) return;

            Debug.Log($"Attempting to SelectExit: {currentGrabObject.name} from {curSlot.name}");

            // 인벤토리에서 내 손으로 옮겨줘야 하고 
            curSlot.SetRayHovering(true);
            
            this.interactionManager.SelectExit(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable);
            
            Debug.Log($"enter  --> 아이템 꺼낼 시 : {curSlot}");
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

            this.interactionManager.SelectEnter(curSlot as IXRSelectInteractor, currentGrabObject as IXRSelectInteractable); 
            //interactionManager.SelectExit(this as IXRSelectInteractor , currentGrabObject as IXRSelectInteractable);

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

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            currentGrabObject = args.interactableObject as InteractObject;

            grabbedTr = args.interactableObject.transform;
            isGrab = true;
            lineVisual.enabled = true;
            aimRect.gameObject.SetActive(false);
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

            Ray ray = new Ray(transform.position + new Vector3(0,0,0.5f), transform.forward);
            Debug.DrawRay(transform.position, transform.forward * 500f, Color.red, 5f);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 500f, Manager.Layer.slotLM))
            {
                InventorySlot hitSlot = hitInfo.collider.GetComponent<InventorySlot>();

                if (hitSlot == null)
                    return null;
                else
                {
                    SpriteRenderer renderer= hitSlot.GetComponent<SpriteRenderer>();
                    if(renderer!= null)
                    {
                        renderer.color = Color.red;
                    }

                    return hitSlot;
                }
                    
            }
            return null;
        }
    }
}
