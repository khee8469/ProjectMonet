using UnityEngine;
using JJH;

namespace JJH
{
    public class GameManager : Singleton<GameManager>
    {
        public void Test()
        {
            Debug.Log(GetInstanceID());
        }
    }

}
