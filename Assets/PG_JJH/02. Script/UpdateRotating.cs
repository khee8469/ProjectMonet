using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRotating : MonoBehaviour
{
    [Tooltip("회전 방향")]
    [SerializeField] private float direction = 60f;

    public float Direction { get { return direction; } }

    private void Start()
    {
        StartCoroutine(RotationRoutine(direction));
    }


    private IEnumerator RotationRoutine(float direction)
    {
        while (true)
        {
            transform.Rotate(direction * Time.deltaTime, 0, 0);
            yield return null;
        }

    }


}
