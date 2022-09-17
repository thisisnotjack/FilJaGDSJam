using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float _rotateSpeed;
    public Vector3 _rotateDir;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateSpeed * _rotateDir * Time.deltaTime);
    }
}
