using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoGearUpdate : MonoBehaviour
{

    [Tooltip("회전 방향")]
    [SerializeField] private float direction = 10f;

    public float Direction { get { return direction; } }

    

    public void RotateRoutine()
    {
        StartCoroutine(RotationRoutine(direction));
    }

    private IEnumerator RotationRoutine(float direction)
    {
        while (true)
        {
            transform.Rotate(0, 0, direction * Time.deltaTime);
            yield return null;
        }

    }



}
