using UnityEngine;
using JJH;

namespace JJH
{
    public static class Extension
    {
        public static bool Contain(this LayerMask layerMask, int layer)
        {
            return ((1 << layer) & layerMask) != 0;
        }
    }

}

