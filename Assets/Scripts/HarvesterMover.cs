using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private IHarvesterTarget _target;

    public event Action TargetReached;

    public void SetTarget(IHarvesterTarget target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.Position, Time.deltaTime * _speed);

        if (Vector3.Distance(transform.position, _target.Position) < _target.ReachRadius)
        {
            TargetReached?.Invoke();
            _target = null;
        }
    }
}
