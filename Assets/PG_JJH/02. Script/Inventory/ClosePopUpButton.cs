using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

namespace JJH
{
    public class ClosePopUpButton : MonoBehaviour
    {
        [SerializeField] Jc.UIManager UIManager;

        public void closePopUpButton()
        {
            UIManager.CloseInfoGroup(); 
        }

    }
}


