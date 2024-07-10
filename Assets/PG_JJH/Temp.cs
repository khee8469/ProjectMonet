using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);
    }
}
