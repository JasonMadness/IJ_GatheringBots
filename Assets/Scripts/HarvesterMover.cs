using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Crystal _target;

    public void SetTarget(Crystal target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
    }
}
