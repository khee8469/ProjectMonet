using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

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
            yield return null;
        }
    }

}

