using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JJH;


namespace JJH
{
    public class TempTrail : MonoBehaviour
    {
        [SerializeField] GameObject linePrefab;
        [SerializeField] LayerMask targetLayer;

        [SerializeField] Transform rayCastStartPos;

        [SerializeField] private float distance = 1f;

        private float width = 0.01f;
        private Color color = Color.black;

        private GameObject currentLine = null;

        private void Start()
        {
            targetLayer = LayerMask.GetMask("DrawBoard");
            rayCastStartPos = transform.Find("RayPos");
        }

        public void StartTrial()
        {
            if(!currentLine)
            {
                RaycastHit hit;

                if(Physics.Raycast(rayCastStartPos.position , rayCastStartPos.forward ,
                        out hit , distance , targetLayer))
                {
                    currentLine = Instantiate(linePrefab, hit.point,
                        transform.rotation, hit.transform);

                    ApplySettings(currentLine);
                }

                 
            }
        }

        private void ApplySettings(GameObject lineObject)
        {
            LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineRenderer.widthMultiplier = width;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        public void EndTrail()
        {
            if(currentLine)
            {
                currentLine.transform.parent = null;
                currentLine = null; 
            }
        }
        public void SetWidth(float value)
        {
            width = value;
        }

        public void SetColor(Color value)
        {
            color = value;
        }

    }
}

