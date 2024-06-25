using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using System.Diagnostics.Contracts;

namespace JJH
{
    public class LobbyScene : BaseScene
    {

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Manager.Scene.LoadScene("Chapter1");
            }
        }

        public override IEnumerator LoadingRoutine()
        {
            Debug.Log("로비씬 로딩 루틴");
            Debug.Log("저장해야 할 일 있으면 여기서도 돌려줘야함");
            yield return null;
            
        }
    }

}

