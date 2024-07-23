using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using JJH;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using EPOOutline.Demo;
using System.Net.NetworkInformation;
namespace JJH
{
    public class GearSocket : XRSocketInteractor  , IPuzzleable
    {

        [Header("퍼즐 매니저 에디터 세팅")]
        [SerializeField]
        private GearManager gearPuzzle;

        [Tooltip("퍼즐과 매칭 시킬 인덱스")]
        [SerializeField] private int puzzleIndex;

        [Tooltip("해당 소켓에 들어가야 할 기어 오브젝트")]       
        [SerializeField] private GearObject targetGear; // 이거도 프리팹으로 둬야함!!

        /*[Tooltip("target gear ID")]
        [SerializeField] int gearID;*/
        
        public GearObject TargetGear { get { return targetGear; } }

        [Tooltip("자신의 소켓 충돌 판정콜라이더 --> 퍼즐 상태에 따른 on off 용")]
        private Collider socketCollider;

        [Tooltip("나중에 완료되어 있을 때 생성해 줄 숨겨둔 기어")]
        [SerializeField] private GameObject gearGameObject;

        protected override void Awake()
        {
            base.Awake();
            RegistObject(gearPuzzle);
            socketCollider = GetComponent<Collider>();
            gearGameObject.SetActive(false); 
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            GearObject obj = args.interactableObject as GearObject; // 기어 오브젝트 일 때만 소켓에 들어간다. 
            // 기어 라면 일단 들어는 가야한다.
            if (obj == null) return; // 기어가 아니면 리턴 시킨다. 

            if (this.hasSelection) // 현재 소켓에 아이템이 들어가 있다. 
            {
                IXRSelectInteractor interactor = obj.firstInteractorSelecting;
                if (interactor is XRBaseControllerInteractor)
                {
                    Debug.Log("소켓에 이미 아이템이 있고 사람이 넣으려고 했음");
                    obj.interactionManager.SelectExit(interactor as IXRSelectInteractor,
                        obj as IXRSelectInteractable);
                }
            }

            gearPuzzle.gearList.Add(obj);

            // 이거 나중에 수정 
            if (TargetGear.gearID == obj.gearID) // 정답 기어 라면 
            {
                gearPuzzle.UpdateCondition(puzzleIndex);
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            GearObject obj = args.interactableObject as GearObject;
            gearPuzzle.gearList.Remove(obj);

            if (TargetGear.gearID == obj.gearID) // 정답 기어 라면 
            {
                gearPuzzle.UpdateCondition(puzzleIndex, false); // bool 값을 false로 변경. 
            }

        }

        // 활성화 상태면 콜라이더 켜주기.
        public void ActiveSetting()
        {
            gearGameObject.SetActive(false);
            socketCollider.enabled = true;
        }

        // 각각의 소켓에 대해서 gearIDiTEM을 생성해놓는다. 
        public void CompleteSetting() // 다시 세팅 할 
        {
            gearPuzzle.UpdateCondition(puzzleIndex);
            gearPuzzle.OnClearPuzzle();
            gearGameObject.gameObject.SetActive(true); // 미리 넣어두고 꺼놓은 기어 켜주기. 
            socketCollider.enabled = false;

        }

        // 퍼즐 비활성화 상태 적용 --> 콜라이더 꺼주기. 
        public void DisActiveSetting()
        {
            socketCollider.enabled = false;
            
        }

        // 퍼즐 등록 
        public void RegistObject(PuzzleManager puzzle)
        {
            puzzle.puzzleObjects.Add(this);
        }

        // 그냥 업데이트 condition 자체를 부르는 중 
        // 자체적으로 완료되면 onclear 호출 되도록 되어 있음. 
        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            puzzle.UpdateCondition(index); // 이거는 그냥 만들어만 두고 부르는 곳이 없는듯?
        }


        private IEnumerator SocketOnRoutine(GearObject obj)
        {
            Vector3 startPos = obj.transform.position;
            Vector3 endPos = attachTransform.position;

            float elapsed = 0f;
            while(elapsed < 1f)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp (startPos, endPos, elapsed / 1f);
                yield return Time.deltaTime;
            }
        }



    }
}


