using System.Collections.Generic;
using UnityEngine;
using JJH;

namespace JJH
{
    [CreateAssetMenu(fileName = "PaintTypeManager", menuName = "ScriptableObjects/PaintTypeManager", order = 1)]
    public class PaintTypeManager : ScriptableObject
    {
        public List<PaintTypeColor> paintTypeColors;

        // 특정 PaintTypeEnum에 해당하는 색상을 반환하는 메서드
        public Color GetColorByType(PaintTypeEnum type)
        {
            PaintTypeColor paintTypeColor = paintTypeColors.Find(ptc => ptc.type == type);
            if (paintTypeColor != null)
            {
                return paintTypeColor.color;
            }
            return Color.white; // 기본 색상
        }
    }

    [System.Serializable]
    public class PaintTypeColor
    {
        public PaintTypeEnum type;
        public Color color;
    }

    public enum PaintTypeEnum
    {
        Red,
        Blue,
        Green,
        Yellow,
        Black,
        White
    }
}

