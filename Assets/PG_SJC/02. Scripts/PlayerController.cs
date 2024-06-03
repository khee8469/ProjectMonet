using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Vector3 moveDir;

    private void OnRotate(InputValue value)
    {
        //Debug.Log(value.Get<Vector2>());
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void Move()
    {
        if (moveDir == Vector3.zero) return;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }
}