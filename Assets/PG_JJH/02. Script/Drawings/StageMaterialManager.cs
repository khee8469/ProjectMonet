using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using UnityEngine.Events;

namespace JJH
{
    public class StageMaterialManager : Singleton<StageMaterialManager>
    {

        // 아니면 dictionary 로 value 값에 color 값을 저장해두고 color 값 맞는 마테리얼 key를 통해
        // tryGetKey로 접근해서 그 부분만 다른 value로 넣어주고 그 value에 맞게 material을 바꿔주는 방식도 괜찮을지도?


        [Header("각 씬의 마테리얼을 관리 할 stage material 매니저")]

        [SerializeField]
        private List<Material> stage1_MatList = new List<Material>();

        [SerializeField]
        private List<Material> stage2_MatList = new List<Material>();

        [SerializeField]
        private List<Material> stage3_MatList = new List<Material>();

        [SerializeField]
        private List<Material> stage4_MatList = new List<Material>();

        [SerializeField]
        [Header("씬 마테리얼 변경 이벤트")]
        [Tooltip("씬에 알맞게 마테리얼을 변경해주자.")]
        public static UnityEvent< int, int > MaterialEvent = new UnityEvent< int , int>();
        // 일단 임시로 인트 형으로 이벤트 진행 

        protected override void Awake()
        {
            base.Awake();

            // 추가적으로 해야 할 작업 (Awake 는 싱글턴이므로 한 번 만 실행됨)


        }

        protected void ChangeMat(int sceneNumber , int MatList)
        {

        }


        

    }
}

