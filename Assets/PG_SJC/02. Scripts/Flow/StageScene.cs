using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace Jc
{
    public class StageScene : BaseScene
    {
        [SerializeField]
        private Transform playerStartTr;
        public override IEnumerator LoadingRoutine()
        {
            if (Manager.Scene.PlayerObject != null)
            {
                Debug.Log("플레이어 위치설정");
                Manager.Scene.PlayerObject.GetComponent<CharacterController>().enabled = false;
                Manager.Scene.PlayerObject.transform.position = playerStartTr.position;
                Manager.Scene.PlayerObject.transform.rotation = playerStartTr.rotation;                Manager.Scene.PlayerObject.GetComponent<CharacterController>().enabled = false;
                Manager.Scene.PlayerObject.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                Debug.Log("플레이어가 설정되지 않았습니다.");
            }
            yield return null;
        }
    }
}
