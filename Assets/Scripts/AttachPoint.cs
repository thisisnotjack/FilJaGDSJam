using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachPoint : MonoBehaviour
{
    public AttachableItemBody _itemBody;
    public AttachPoint AttachedTo => _attachedTo;
    public AttachPoint _attachedTo;
    public float _detachDist = 0.3f;
    
    AttachPoint _previouslyAttachedTo;

    void Update()
    {
        if (_previouslyAttachedTo != null)
        {
            float dist = Vector3.Distance(transform.position, _previouslyAttachedTo.transform.position);
            if (dist > _detachDist)
            {
                _previouslyAttachedTo = null;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_previouslyAttachedTo != null && _previouslyAttachedTo.gameObject == other.gameObject)
        {
            return; // ignore collision, this is a hacky way to make sure we dont reattach while detaching
        }
        if (AttachedTo == null && _itemBody != null)
        {
            var attachPoint = other.gameObject.GetComponent<AttachPoint>();
            if (_itemBody.IsGrabbed)
            {
                if (attachPoint != null)
                {
                    Attach(attachPoint, true);
                }
            }
            else if (attachPoint != null)
            {
                var parentItemBody = GetParentItemBody();
                if (parentItemBody.IsGrabbed)
                {
                    InvertParentStructure();
                    Attach(attachPoint, true);
                    parentItemBody.Release(true);
                    BuildingManager.Instance.ReleaseObject();
                }
            }
        }
    }

    private AttachableItemBody GetParentItemBody()
    {
        var bodies = transform.GetComponentsInParent<AttachableItemBody>();
        return bodies[bodies.Length -1];
    }
    
    
    // 0 <- 1 <- 2 <- 3 
    // 0 <- 3 <- 2 <- 1
    
    // 0 <- 3     
    // 0 <- 1 <- 2
    private void InvertParentStructure()
    {
        var bodies = transform.GetComponentsInParent<AttachableItemBody>();
        var parent = bodies[bodies.Length - 1].transform.parent;
        bodies[0].transform.parent = parent;
        AttachPoint parentAttachPoint = _itemBody.GetFirstAttachPointThatIsNot(this);

        if (parentAttachPoint == null)
            return;
        for (int i = 1; i < bodies.Length; i++)
        {
            bodies[i].transform.parent = parentAttachPoint.transform;
            parentAttachPoint = bodies[i].GetFirstAttachPointThatIsNot(this);
        }
    }

    public bool Attach(AttachPoint attachPoint, bool callAttachRec)
    {
        if (_attachedTo != null)
            return false;
        if (callAttachRec)
        {
            bool canAttach = attachPoint.Attach(this, false);
            if (!canAttach)
                return false;
            if (_itemBody)
                _itemBody.AttachPointAttached(this, attachPoint);
        }

        _attachedTo = attachPoint;
        _previouslyAttachedTo = _attachedTo;
        return true;
    }

    public void Detach(bool callDetachRec)
    {
        if (_attachedTo != null)
        {
            _previouslyAttachedTo = _attachedTo;
            if (callDetachRec)
            {
                _attachedTo.Detach(false);
                //if(_itemBody)
                 //   _itemBody.DetachPointFrom(_attachedTo);
            }
            
            _attachedTo = null;
        }
    }
}
