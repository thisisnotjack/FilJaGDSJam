using UnityEngine;

public class FollowComponent : MonoBehaviour
{
    private Transform _toFollow;
    private Vector3 _offset;
    private bool _following;

    public void FollowTransform(Transform toFollow, Vector3 offset)
    {
        _toFollow = toFollow;
        _offset = offset;
        _following = true;
    }

    void Update()
    {
        if (_following)
        {
            transform.position = _toFollow.position + _offset;
        }
    }
}
