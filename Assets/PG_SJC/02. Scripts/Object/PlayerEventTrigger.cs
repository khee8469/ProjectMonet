using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Jc
{
    public class PlayerEventTrigger : MonoBehaviour
    {
        [SerializeField]
        private LayerMask playerLM;

        [Tooltip("트리거 진입 시 발생할 이벤트")]
        public UnityEvent onTriggerEnter;

        [Tooltip("트리거 탈출 시 발생할 이벤트")]
        public UnityEvent onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            
        }
        private void OnTriggerExit(Collider other)
        {
                
        }

    }
}
