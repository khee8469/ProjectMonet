using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    public class LobbyScene : BaseScene
    {


        private void OnEnable()
        {
            
        }

        public override IEnumerator LoadingRoutine()
        {
            Debug.Log("로비씬 로딩 루틴");
            yield return null;
        }
    }

}

