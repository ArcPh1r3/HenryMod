using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nopejs : MonoBehaviour
{
    [SerializeField, Range(-1,1)]
    private float x;
    [SerializeField, Range(-1, 1)]
    private float y = 1;
    [SerializeField, Range(-1, 1)]
    private float z;

    [SerializeField]
    private float mult = 10; 

    void Update()
    {
        transform.Rotate(new Vector3(x, y, z) * mult * Time.deltaTime);
    }
}
