using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JJH
{
    [CustomEditor(typeof(PaintTypeManager))]
    public class PaintTypeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // ScriptableObject를 가져옴
            PaintTypeManager manager = (PaintTypeManager)target;

            // 기본 인스펙터 GUI 표시
            DrawDefaultInspector();

            // 열거형 값과 색상 표시 및 편집
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Paint Type Colors", EditorStyles.boldLabel);

            if (manager.paintTypeColors == null)
            {
                manager.paintTypeColors = new List<PaintTypeColor>();
            }

            foreach (PaintTypeEnum type in System.Enum.GetValues(typeof(PaintTypeEnum)))
            {
                PaintTypeColor paintTypeColor = manager.paintTypeColors.Find(ptc => ptc.type == type);
                if (paintTypeColor == null)
                {
                    paintTypeColor = new PaintTypeColor { type = type, color = Color.white };
                    manager.paintTypeColors.Add(paintTypeColor);
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(type.ToString(), GUILayout.Width(100));
                paintTypeColor.color = EditorGUILayout.ColorField(paintTypeColor.color);
                EditorGUILayout.EndHorizontal();
            }

            // 변경 사항이 있으면 ScriptableObject를 저장합니다.
            if (GUI.changed)
            {
                EditorUtility.SetDirty(manager);
            }
        }
    }
}

