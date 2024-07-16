using System;
using System.IO;
using UnityEngine;
using JJH;

namespace JJH
{
    public class DataManager : Singleton<DataManager>
    {
        private CanvasData canvasData;
        public CanvasData CanvasData { get { return canvasData; } }


        // 나중에 이 Path 위치만 맞춰주기. 
#if UNITY_EDITOR
        private string path => Path.Combine(Application.dataPath, $"Resources/Data/SaveLoad");
#else
    private string path => Path.Combine(Application.persistentDataPath, $"Resources/Data/SaveLoad");
#endif

        public void NewData()
        {
            canvasData = new CanvasData();
        }

        public void SaveData(int index = 0)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            string json = JsonUtility.ToJson(canvasData, true);
            File.WriteAllText($"{path}/{index}.txt", json);
        }

        public void LoadData(int index = 0)
        {
            if (File.Exists($"{path}/{index}.txt") == false)
            {
                NewData(); // 파일이 존재하지 않으면 뉴 데이터 생성 
                return;
            }

            string json = File.ReadAllText($"{path}/{index}.txt");
            try
            {
                canvasData = JsonUtility.FromJson<CanvasData>(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Load data fail : {ex.Message}");
                NewData();
            }
        }

        public bool ExistData(int index = 0)
        {
            return File.Exists($"{path}/{index}.txt");
        }
    }
}


