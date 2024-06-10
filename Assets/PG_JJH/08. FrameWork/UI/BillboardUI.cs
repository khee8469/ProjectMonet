using UnityEngine;
using JJH;

namespace JJH
{
    public class BillboardUI : BaseUI
    {
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

}
