using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : LevelObjectBehaviour
{
    [SerializeField] bool SmoothFollow = false;
    [SerializeField] float smoothTime = 0.06f;

    [SerializeField] float height = 80f;
    [SerializeField] Transform target;

    Vector3 velocity = Vector3.zero;
    Transform tr;
    bool stopCameraAtOnce = false;
    Camera cam;

    public static CameraController Instance { get { return instance; } }
    static CameraController instance;
    public Camera GameCamera { get { return cam; } }

    public void StopCameraDueToDeath()
    {
        stopCameraAtOnce = true;
    }

    void LateUpdate()
    {
        if (stopCameraAtOnce || behaviourEnabled == false) { return; }
        Vector3 tg = target.position + target.up * height;
        if (SmoothFollow)
        {
            tr.position = Vector3.SmoothDamp(tr.position, tg, ref velocity, smoothTime);
        }
        else
        {
            tr.position = tg;
        }
    }

    protected internal override void OnAwake()
    {
        instance = this;
        tr = transform;
        stopCameraAtOnce = false;
        cam = GetComponent<Camera>();
    }
}
