using System.Collections.Generic;
using UnityEngine;

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

        public PaintTypeEnum GetTypeByColor(Color color)
        {
            PaintTypeColor paintTypeColor = paintTypeColors.Find(ptc => ptc.color == color);
            if (paintTypeColor != null)
            {
                return paintTypeColor.type;
            }
            return PaintTypeEnum.None; // 기본 값
        }


    }

    [System.Serializable]
    public class PaintTypeColor
    {
        public PaintTypeEnum type;
        public Color color;
    }

    [System.Serializable]
    [Tooltip("None은 반드시 white 컬러로 설정할 것. ")]
    public enum PaintTypeEnum
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Black,
        White,
        Brown
    }
}

