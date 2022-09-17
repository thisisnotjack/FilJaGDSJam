using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachableItem : MonoBehaviour
{
    public GameObject _itemParent;
    public ItemState CurrentState => _currentState;
    public float _detachDist = 0.1f;
    public ItemState _currentState = ItemState.Neutral;
    private AttachPoint _attachedTo;
    public GameObject _previouslyAttachedTo;
    public float _currentDist;
    public enum ItemState
    {
        Neutral,
        Grabbed,
        Attached
    }

    void Update()
    {
        if (CurrentState != ItemState.Attached)
        {
            if (_previouslyAttachedTo != null)
            {
                float dist = Vector3.Distance(transform.position, _previouslyAttachedTo.transform.position);
                _currentDist = dist;
                if (dist > _detachDist)
                {
                    _previouslyAttachedTo = null;
                }
            }
        }
    }
    public bool TryGrab(Transform draggingParent)
    {
        switch (CurrentState)
        {
            case ItemState.Neutral:
                Grab(draggingParent);
                return true;
            case ItemState.Grabbed:
                return false;
            case ItemState.Attached:
                Detach(ItemState.Grabbed);
                Grab(draggingParent);
                return true;
            default:
                return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_previouslyAttachedTo == other.gameObject)
        {
            return; // ignore collision, this is a hacky way to make sure we dont reattach while detaching
        }
        if (CurrentState != ItemState.Attached)
        {
            var attachPoint = other.gameObject.GetComponent<AttachPoint>();
            if (attachPoint != null)
            {
                Attach(attachPoint);
            }
        }
    }

    private void Grab(Transform draggingParent)
    {
        print("Grabbing " );
        _itemParent.transform.parent = draggingParent;
        _currentState = ItemState.Grabbed;
        _itemParent.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Attach(AttachPoint attachPoint)
    {
        print("Attaching to " + attachPoint.transform.name);
        attachPoint.AttachObject(_itemParent, transform);
        _attachedTo = attachPoint;
        _currentState = ItemState.Attached;
        _itemParent.GetComponent<Rigidbody>().isKinematic = true;
        _previouslyAttachedTo = _attachedTo.gameObject;
    }

    public void Detach(ItemState followingState)
    {
        print("Detaching from " + (_attachedTo == null ? "player hand" : _attachedTo.transform.name));
        _itemParent.transform.parent = null;
        _itemParent.GetComponent<Rigidbody>().isKinematic = false;
        if (_attachedTo != null)
        {
            _previouslyAttachedTo = _attachedTo.gameObject;
            _attachedTo.DetachObject(_itemParent);
            _attachedTo = null;
        }
        _currentState = followingState;
    }
}
