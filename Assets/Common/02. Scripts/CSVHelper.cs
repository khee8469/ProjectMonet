using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Jc;
using System.Reflection;
using UnityEditor;

public class CSVHelper
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();

        TextAsset data = Resources.Load(file.Replace(".csv", "")) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    public static List<Dictionary<string, object>> Read(string filePath, bool isResources)
    {
        var list = new List<Dictionary<string, object>>();
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return list;
        }
        var data = File.ReadAllText(filePath);
        var lines = Regex.Split(data, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    public static void Write<T>(string file, List<T> datas)
    {
        if (datas == null || datas.Count == 0)
        {
            Debug.Log("데이터가 존재하지 않습니다.");
            return;
        }

        StringBuilder sb = new StringBuilder();
        Type type = typeof(T);                              // 제네릭의 타입을 Get
        FieldInfo[] fields = type.GetFields();   // 프로퍼티를 Get

        // CSV 헤더
        for (int i = 0; i < fields.Length; i++)
        {
            // 1열의 Key 값 세팅
            sb.Append(fields[i].Name);
            if (i < fields.Length - 1)
            {
                // 마지막 열을 제외한 열들은 쉼표로 구분
                sb.Append(",");
            }
        }
        sb.AppendLine();

        // 리스트의 각 항목을 CSV 형식으로 추가
        for (int i = 0; i < datas.Count; i++)
        {
            var item = datas[i];
            // 일반화된 자료형의 필드 개수만큼 순회
            for (int j = 0; j < fields.Length; j++)
            {
                var value = fields[j].GetValue(item);
                sb.Append(value?.ToString());
                if (j < fields.Length - 1)
                {
                    // 마지막 열을 제외한 열들은 쉼표로 구분
                    sb.Append(",");
                }
            }
            // 마지막 행을 제외한 행들은 엔터로 구분
            if (i < datas.Count - 1)
            {
                sb.AppendLine();
            }
        }

        File.WriteAllText(file, sb.ToString());
    }
}