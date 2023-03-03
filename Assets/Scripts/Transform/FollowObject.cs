using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _transformToFollow;
    [SerializeField] private bool _followX;
    [SerializeField] private bool _followY;
    [SerializeField] private bool _followZ;
    private Vector3 _startPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (_transformToFollow == null)
        {
            Debug.LogError("FollowObject: TransformToFollow is null!");
        }
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_followX)
        {
            transform.position = new Vector3(_transformToFollow.position.x, transform.position.y, transform.position.z);
        }
        if (_followY)
        {
            transform.position = new Vector3(transform.position.x, _transformToFollow.position.y, transform.position.z);
        }
        if (_followZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _transformToFollow.position.z);
        }
    }
}
