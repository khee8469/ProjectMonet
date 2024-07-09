using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEngine.XR.Interaction.Toolkit;

namespace JJH
{
    public class LightHouseDoor : MonoBehaviour, IPuzzleable
    {
        // 등대 입구 문 --> 열쇠가 있어야 열 수 있다.
        // 플레이어의 인벤토리를 한 번 순회해서 아이템이 있을 때
        // 문을 그냥 transform 이동 이나 힌지 조인트 써서 열어주자.
        // 그 뒤로는 문이 계속 열려 있어야 하니까. 그 부분을 저장하는 변수 하나 추가 할 것.

        // 소켓에 열쇠 넣으면 힌지 조인트 On 시키자. 이런 방식으로 진행 할 것.
        // 재천이 문 사용해서 힌지 조인트 그대로 이용하자.

        [Tooltip("문의 힌지 조인트")]
        [SerializeField] private new HingeJoint hingeJoint;

        [Tooltip("자식으로 가진 소켓")]
        [SerializeField] private XRSocketInteractor socket;

        [Tooltip("열쇠의 item ID ")]
        [SerializeField] private int keyID;

        // 소켓에 넣으면 힌지 조인트 on 해서 밀고 들어 갈 수 있도록 한다.
        // 앞으로 힌지 조인트 계속 켜주면 된다. 

        // 이거는 퍼즐이 아니니까 인터페이스를 상속하면 안될텐데 관리를 어떻게 해 줄지 고민해야함. 






        public void ActiveSetting()
        {

        }

        public void CompleteSetting()
        {

        }

        public void DisActiveSetting()
        {

        }

        public void RegistObject(PuzzleManager puzzle)
        {

        }

        public void UpdatePuzzleManager(PuzzleManager puzzle, int index)
        {
            
        }
    }

}

