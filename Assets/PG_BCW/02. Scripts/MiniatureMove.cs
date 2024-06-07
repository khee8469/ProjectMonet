using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureMove : MonoBehaviour
{
    [SerializeField] Transform transform;

    public void SetPosition()
    {
        this.transform.position = transform.position;
    }
}
