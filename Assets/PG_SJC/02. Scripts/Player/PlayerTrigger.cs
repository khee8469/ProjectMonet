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

        private void OnTriggerEnter(Collider other)
        {
            // NPC 트리거 
            if (Manager.Layer.npcLM.Contain(other.gameObject.layer))
            {
                NPC target = other.GetComponent<NPC>();

                if(target != null) 
                    OnNPCEnter?.Invoke(target);
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

        }
    }
}
