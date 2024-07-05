using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    public class LightHouseDoor : MonoBehaviour, IPuzzleable
    {
        // 등대 입구 문 --> 열쇠가 있어야 열 수 있다.
        // 플레이어의 인벤토리를 한 번 순회해서 아이템이 있을 때
        // 문을 그냥 transform 이동 이나 힌지 조인트 써서 열어주자.
        // 그 뒤로는 문이 계속 열려 있어야 하니까. 그 부분을 저장하는 변수 하나 추가 할 것.
        // manager 접근 해서 slot 순회 하면 되지롱 


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

