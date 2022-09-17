using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachableItemBody : MonoBehaviour
{
    public List<AttachPoint> _itemAttachPoint;
    private Rigidbody _rigidbody;
    public bool _isGrabbed;
    public bool IsGrabbed => _isGrabbed;
    private const int angleToSnapTo = 30;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_itemAttachPoint.Count == 0)
            _itemAttachPoint = new List<AttachPoint>(GetComponentsInChildren<AttachPoint>());
    }

    public bool TryGrab(Transform draggingParent)
    {
        if (IsGrabbed)
            return false;
        Grab(draggingParent);
        return true;
    }
    
    private void Grab(Transform draggingParent)
    {
        //detach parent attach points
        if (transform.parent != null)
        {
            var parentAttachPoint = transform.parent.gameObject.GetComponent<AttachPoint>();
            if(parentAttachPoint!= null)
                parentAttachPoint.Detach(true);
        }
        _isGrabbed = true;
        _rigidbody.isKinematic = true;
        transform.parent = draggingParent;
    }
    public void Release(bool keepParent = false)
    {
        _isGrabbed = false;
        if (!keepParent)
        {
            transform.parent = null;
            _rigidbody.isKinematic = true;
        }

        if (transform.parent == null)
        {
            for (int i = 0; i < _itemAttachPoint.Count; i++)
            {
                if (_itemAttachPoint[i]._attachedTo != null)
                {
                    if (_itemAttachPoint[i]._attachedTo._itemBody.transform.parent != _itemAttachPoint[i].transform)
                    {
                        Debug.LogError("FILIP PLEASE LET ME KNOW ABOUT THIS!");
                    }
                }
            }
        }
    }

    public void AttachPointAttached(AttachPoint from, AttachPoint to)
    {
        //if(to plane only normally)
        if (_isGrabbed)
        {
            _isGrabbed = false;
            BuildingManager.Instance.ReleaseObject();
        }
        var eulers = transform.rotation.eulerAngles;
        eulers.y = (Mathf.Round(eulers.y / angleToSnapTo)) * angleToSnapTo;
        print(eulers.y);
        transform.rotation = Quaternion.Euler(eulers);
        from.transform.parent = to.transform;
        transform.parent = from.transform;
        from.transform.localPosition = Vector3.zero;
        
        transform.parent = to.transform;
        from.transform.parent = transform;
        
        _rigidbody.isKinematic = true;
    }

    public AttachPoint GetFirstAttachPointThatIsNot(AttachPoint a)
    {
        for (int i = 0; i < _itemAttachPoint.Count; i++)
        {
            if (_itemAttachPoint[i] != a)
            {
               return _itemAttachPoint[i];
            }
        }

        return null;
    }
    /*
    public void DetachPointFrom(AttachPoint attachPoint)
    {
        transform.parent = null;
        _rigidbody.isKinematic = true;
    }*/
}
