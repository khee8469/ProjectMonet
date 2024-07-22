using Jc;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class HiddenPatternObject : XRKnob, IInteractable
    {
        // 회전 되어야 하는 오브젝트임. 
        // --> 1. 가장 내부에 있는 오브젝트는 움직이지 않음.
        // --> 2. 가장 내부 부터 하나 씩 맞춰야 하기 때문에 1 2 3 4 이면
        // 최초 시작은 1 3 4 가 고정되어서 움직일 수 없게 해야 한다.
        // 2번 조각과 1번이 맞으면 1 2 를 고정 시키고 3을 움직일 수 있게 한다.

        // 추가로 회전 시키는 거니까 이거 xr sample 확인하자. 
        // Advanced option 의 sorting priority 설정해주면 같은 위치에 놓여있어도 rendering 순서를 정해 줄 수 있다. 

        [Header("원반 Spec")]
        [Tooltip("각 원반 들의 아이디")]
        [SerializeField] private int patternID;

        public int PatternID { get { return patternID; } }


        [Tooltip("자식으로 있는 Hnadle의 콜라이더 --> 인스펙터에서 직접 할당 ")]
        [SerializeField] public new Collider collider;

        [Tooltip("딸려 오는 거리")]
        [SerializeField] private float grabDistance = 3f;


        [Tooltip("자신을 관리해 줄 컨트롤러")]
        [SerializeField] private HiddenPatternController controller;


        protected override void Awake()
        {
            base.Awake();
            clampedMotion = false;
        }

        private void Start()
        {
            if (patternID == 1) // ID 1번인 가장 내부는 콜랑리더 꺼주고 인터액션 꺼줘서 못 만지도록 하기. 
            {
                interactionLayers = 0; // 0번이 nothing 임!!
                collider.enabled = false; // 이거는 어차피 그대로 
            }

        }

        public Transform GetTransform() // 자신의 트랜스폼 리턴. 
        {
            return transform;
        }

        public float GetInteractDistance()
        {
            return 0;
        }

        public float GetDistanceThreshold()
        {
            return grabDistance;
        }
    }

    [System.Serializable] // json으로 저장해서 씬 간 저장해 둘 쿼터니언 값 
    public class PatternData
    {
        // 원반의 현재 회전값 --> 그런데 이거 저장 해줘야하나?
        Quaternion rotation;

    }



}

