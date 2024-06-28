using System.Collections;
using UnityEngine;

namespace JJH
{
    public class LobbyScene : BaseScene
    {
        public override IEnumerator LoadingRoutine()
        {
            Manager.paint.MyPaintCheck();




            yield return null;

        }

    }

}

