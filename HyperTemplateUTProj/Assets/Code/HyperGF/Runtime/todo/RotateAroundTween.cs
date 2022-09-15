using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTween : LevelObjectBehaviour
{
    [SerializeField] Space space = Space.Self;
    [SerializeField] Vector3 axis;
    [SerializeField] float speed = 10f;
    Transform tr;
    protected internal override void OnAwake()
    {
        tr = transform;
    }

    private void Update()
    {
        tr.Rotate(axis, speed * Time.deltaTime, space);
    }
}
