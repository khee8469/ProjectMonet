using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    // 플레이어 충돌 확인
    public class PlayerTrigger : MonoBehaviour
    {
        public UnityAction<NPC> OnNPCEnter;
        public UnityAction<NPC> OnNPCExit;

        //상호작용 npc용
        public UnityAction<InteractorNPC> OnInNPCEnter;
        public UnityAction<InteractorNPC> OnInNPCExit;


        private void OnTriggerEnter(Collider other)
        {
            // NPC 트리거 
            if (Manager.Layer.npcLM.Contain(other.gameObject.layer))
            {
                NPC target = other.GetComponent<NPC>();

                if(target != null) 
                    OnNPCEnter?.Invoke(target);
            }
            // 상호작용 NPC 트리거
            else if (Manager.Layer.InNpcLM.Contain(other.gameObject.layer))
            {
                InteractorNPC target = other.GetComponent<InteractorNPC>();

                if (target != null)
                    OnInNPCEnter?.Invoke(target);

                //대사 출력 : 상호작용 완료 여부에 따라 대사 달라야함
                //target.OnTalkInteractor();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // NPC 트리거 해제
            if (Manager.Layer.npcLM.Contain(other.gameObject.layer))
            {
                NPC target = other.GetComponent<NPC>();

                if (target != null)
                    OnNPCExit?.Invoke(target);
            }
            // 상호작용 NPC 트리거 해제
            if (Manager.Layer.InNpcLM.Contain(other.gameObject.layer))
            {
                InteractorNPC target = other.GetComponent<InteractorNPC>();

                if (target != null)
                    OnInNPCExit?.Invoke(target);
            }
        }
    }
}
