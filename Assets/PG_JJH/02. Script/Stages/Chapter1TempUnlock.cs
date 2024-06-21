using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using JJH;

namespace JJH
{
    public class Chapter1TempUnlock : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Manager.Scene.LoadScene("LobbyScene");
                
            }
         
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Manager.Scene.LoadScene("Chapter1");
            }

        }
    }

}

