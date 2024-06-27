using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();   
    }

    private void FixedUpdate()
    {
        controller.Move(Vector3.down * 9.81f);
    }
}
