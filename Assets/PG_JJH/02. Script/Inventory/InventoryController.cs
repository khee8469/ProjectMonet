using Jc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace JJH
{
    public class InventoryController : MonoBehaviour
    {
        // 인벤토리의 열고 닫고를 다른 곳에서 체크 하고 있기 때문에 굳이 여기서?
        // 만약 이벤트가 필요하다면 이제 인벤토리 On Off 이벤트를 달아 주는 식으로 하자. 

        // 임시 
        public static UnityEvent<bool> InventoryEvent = new UnityEvent<bool>();

        public GameObject inventory;
        public GameObject anchor;
        public bool uiActive;

        [SerializeField] PlayerControllerCallback playerControllerCallback;


        private void Awake()
        {
            InventoryEvent.AddListener(OpenInventory);
            InventoryEvent.AddListener(CloseInventory);
            inventory.SetActive(uiActive);
        }

        private void OnEnable() // 이거 지금 콜백에서 주석 처리 되어있어서 등록이 안되는 듯 하다. maybe? 
        {
            playerControllerCallback.leftMenuBTNRef.action.performed += OpenInventory;
        }

        private void OnDisable()
        {
            playerControllerCallback.leftMenuBTNRef.action.performed -= OpenInventory;

        }
        private void OpenInventory(InputAction.CallbackContext callbackContext)
        {
            Debug.Log("메뉴 버튼 클릭 으로 인벤토리 열기");
            uiActive = !uiActive;
            inventory.SetActive(uiActive);

            if (uiActive)
            {
                inventory.transform.position = anchor.transform.position;
                inventory.transform.eulerAngles = new Vector3
                    (anchor.transform.eulerAngles.x * 30, anchor.transform.eulerAngles.y, 0);

                Debug.Log($"앵커의 트랜스폼 로테이션{anchor.transform.eulerAngles.x * 30} ");

            }
        }

        private void RotationEuler()
        {
            Quaternion rotation = Quaternion.Euler(anchor.transform.eulerAngles.x * 15,
                anchor.transform.eulerAngles.y, 0);

            inventory.transform.rotation = rotation;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("메뉴 버튼 클릭 으로 인벤토리 열기");
                uiActive = !uiActive;
                inventory.SetActive(uiActive);

                if (uiActive)
                {
                    inventory.transform.position = anchor.transform.position;
                    inventory.transform.eulerAngles = new Vector3
                        (anchor.transform.eulerAngles.x * 15, anchor.transform.eulerAngles.y, 0);
                    Debug.Log($"{anchor.transform.eulerAngles.x * 15}");
                }
            }

        }

        private void OpenInventory(bool isOpened)
        {
            // 열었을 때 이벤트가 필요하다면.. 써야겠지?
        }

        // 인벤토리를 닫는 함수 --> event와 연결하여 조작 연계
        private void CloseInventory(bool isOpened)
        {
            // 닫았을 때 이벤트가 필요하다면 사용해야겠지.. 
        }

        // 인벤토리에 직접 Add를 할 때 실패하면 사운드 발생.

    }

}
