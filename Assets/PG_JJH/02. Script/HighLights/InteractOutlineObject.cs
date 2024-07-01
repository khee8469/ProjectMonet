using EPOOutline;
using Jc;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    [RequireComponent(typeof(Outlinable))]
    public class InteractOutlineObject : InteractObject
    {
        // 만약 상호작용이 가능한 오브젝트가 모두 잡혀 있을 때 아웃라인을 그려줘야 한다면
        // 그냥 InteractObject 의 override 에서 아웃라인 on off를 진행해주면 된다. 
        // 추가로 requirement 해주면된다.-> 모든 오브젝트가 그렇다고 한다면 . 

        // 이 스크립트를 오브젝트에 붙이기 보다는 그냥 이 내용을 각자의 오브젝트에서 추가로 더해주는 느낌으로 가는게 좋다.
        // 다중 상속도 안되고 xrGrab도 하나 밖에 못 붙이기 때문에. 

        Outlinable myOutline;


        private void Start()
        {
            myOutline = GetComponent<Outlinable>();
            // 아웃라인의 세팅을 저장해주자. 
            myOutline.ComplexMaskingMode = ComplexMaskingMode.ObstaclesMode;
            myOutline.DrawingMode = OutlinableDrawingMode.Normal;
            myOutline.OutlineLayer = 17; // 사실 크게 의미는 없는듯.
            myOutline.RenderStyle = RenderStyle.FrontBack;
            myOutline.enabled = false;
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            // 잡히는 오브젝트 들은 player의 left right controller 쪽에
            // Player 태그를 붙여줘야 한다. 

            base.OnSelectEntered(args);

            if (args.interactorObject.transform.GetComponent<CustomCheck>() != null)
            {
                myOutline.enabled = true;

            }
            /*if (args.interactorObject.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("플레이어에게 잡힘");
                myOutline.enabled = true;
            }*/
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            myOutline.enabled = false;

        }

    }
}


