using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public enum ColorType{ Red = 0, Blue, Green, Yellow}
    [Serializable]
    public struct Environment
    {
        public Renderer renderer;
        public Material[] originMaterials;

        public Environment(Renderer renderer, Material[] originMaterials)
        {
            this.renderer = renderer;
            this.originMaterials = originMaterials;
        }
    }

    public class EnvironmentController : MonoBehaviour
    {
        [SerializeField]
        private List<Environment> redList = new List<Environment>();
        [SerializeField]
        private List<Environment> blueList = new List<Environment>();
        [SerializeField]
        private List<Environment> greenList = new List<Environment>();
        [SerializeField]
        private List<Environment> yellowList = new List<Environment>();

        private List<List<Environment>> evtList = new List<List<Environment>>();

        private void Start()
        {
            
        }

        private void RegistEnvironment()
        {
            // 리스트 등록
            List<GameObject[]> objs = new List<GameObject[]>();
            objs.Add(GameObject.FindGameObjectsWithTag("Red"));
            objs.Add(GameObject.FindGameObjectsWithTag("Blue"));
            objs.Add(GameObject.FindGameObjectsWithTag("Green"));
            objs.Add(GameObject.FindGameObjectsWithTag("Yellow"));

            for(int i =0; i<objs.Count; i++)
            {
                foreach(GameObject ob in objs[i])
                {
                    Renderer renderer = ob.GetComponent<Renderer>();
                    if (renderer == null) return;

                    Material[] materials = renderer.materials;
                    if (materials == null || materials.Length < 1) return;

                    //evList[i].environmentList.Add
                }
            }

        }
    }
}