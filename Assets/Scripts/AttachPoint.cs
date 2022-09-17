using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    [SerializeField]
    private bool _isTaken;

    [SerializeField] private GameObject _attachedObject;


    public bool AttachObject(GameObject obj, Transform attachPoint)
    {
        if (_isTaken)
            return false;
        if(BuildingManager.Instance.CurrentlyDragging == obj)
            BuildingManager.Instance.ReleaseObject(rigidbodyKinematic:true);
        _attachedObject = obj;
        _isTaken = true;
        attachPoint.transform.parent = transform;
        _attachedObject.transform.parent = attachPoint;
        attachPoint.transform.localPosition = Vector3.zero;
        
        _attachedObject.transform.parent = transform;
        attachPoint.transform.parent = _attachedObject.transform;
        return true;
    }

    public void DetachObject(GameObject obj)
    {
        if (_attachedObject == obj)
        {
            _isTaken = false;
            _attachedObject = null;
        }
    }
}
