using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;
using System.Diagnostics.Contracts;

namespace JJH
{
    public class LobbyScene : BaseScene
    {
        [SerializeField] private CanvasOnOff infoCanvas;


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
            infoCanvas.gameObject.SetActive(false);
        }
    }

}

