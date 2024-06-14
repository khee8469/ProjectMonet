using UnityEngine;
using UnityEngine.Events;

namespace JJH
{
    public class DrawingCompleteManager : Singleton<DrawingCompleteManager>
    {
        // 로비씬에서 이거를 가지고 있어서. 
        // 완료 되면 여기 있는 오브젝트들 하나씩 켜주기.

        [SerializeField] private GameObject[] completeImages = new GameObject[4];

        // 여기서 이제 중요한 작업 --> 그림이 완성될 시 스테이지 해금 (알맞는 해금)
        // 그림이 완성될 시 --> 해당 맵에 있는 포스트 프로세싱 제거

        // 그림이 완성 될 시 --> 완성 이미지 띄워주고 그동안 그린 이미지는 삭제해주기 (안보이게 하기 )

        // 3가지를 한 번에 하기 위해서는 이벤트가 최고

        // ++ 중요한 것은 이제 그림의 완성을 어떻게 체크를 해줄지가 중요하다. 









    }
}

