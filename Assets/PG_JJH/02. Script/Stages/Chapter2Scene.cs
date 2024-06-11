using System.Collections;
using UnityEngine;

namespace JJH
{
    public class Chapter2Scene : BaseScene
    {
        public override IEnumerator LoadingRoutine()
        {

            Debug.Log("2챕터 로딩 루틴");
            yield return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Manager.Scene.LoadScene("LobbyScene");

            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChapterManager.Instance.UnlockStage(2, true);

            }
        }
    }

}

