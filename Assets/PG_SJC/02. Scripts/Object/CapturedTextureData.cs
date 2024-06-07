using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct PosRotPair
    {
        public Vector3 position;
        public Quaternion rotation;

        public PosRotPair(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}
