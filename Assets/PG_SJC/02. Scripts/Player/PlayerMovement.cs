using JJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    private CharacterController controller;

    bool isRegistered = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();   
    }

    private void OnEnable()
    {
        if (!isRegistered)
            Manager.Scene.PlayerObject = gameObject;    
    }

    private void FixedUpdate()
    {
        controller.Move(Vector3.down * 9.81f);
    }
}
