using System.Collections;
using UnityEngine;
using JJH;

namespace JJH
{
    public abstract class BaseScene : MonoBehaviour
    {
        public abstract IEnumerator LoadingRoutine();
    }

}

