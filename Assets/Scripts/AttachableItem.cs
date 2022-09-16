using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableItem : MonoBehaviour
{
    public GameObject _itemParent; 
    private void OnTriggerEnter(Collider other)
    {
        var attachPoint = other.gameObject.GetComponent<AttachPoint>();
        print("Collided " + other.transform.name);
        if (attachPoint != null )
        {
            print("Touched attach point");
            attachPoint.AttachObject(_itemParent, transform);
        }
    }
}
