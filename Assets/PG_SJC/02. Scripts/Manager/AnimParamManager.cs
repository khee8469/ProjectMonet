using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Jc
{
    /// <summary>
    /// 애니메이터 파라미터 ID 세팅, 캐싱
    /// </summary>
    public class AnimParamManager : Singleton<AnimParamManager>
    {
        [SerializeField]
        private int id_MoveSpeed;
        public int MoveSpeed { get { return id_MoveSpeed; } }

        [SerializeField]
        private int id_IsMoving;
        public int IsMoving { get { return id_IsMoving; } }

        [SerializeField]
        private int id_IsInetract;
        public int IsInteract { get { return id_IsInetract; } }

        protected override void Awake()
        {
            base.Awake();
            InitParameters();
        }

        private void InitParameters()
        {
            id_MoveSpeed = Animator.StringToHash("MoveSpeed");
            id_IsInetract = Animator.StringToHash("IsInteract");
        }

    }
}
