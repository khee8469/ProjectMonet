using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class Sliceable : MonoBehaviour
    {
        private void Start()
        {
            SliceManager.sliceables.Add(this);
        }
    }
}
