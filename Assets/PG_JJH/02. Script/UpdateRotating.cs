using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRotating : MonoBehaviour
{
    [SerializeField] private float Direction = 60;

    private void Start()
    {
        Direction = Random.Range(30, 40);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Direction * Time.deltaTime);
    }
}
