using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] bool SmoothFollow = false;
    [SerializeField] float smoothTime = 0.06f;

    [SerializeField] Transform target;
    Vector3 velocity = Vector3.zero;
    Transform tr;
    bool stopCameraAtOnce = false;

    public static CameraController instance;

    void Awake()
    {
        instance = this;
        tr = transform;
        stopCameraAtOnce = false;
    }

    public void StopCameraDueToDeath()
    {
        stopCameraAtOnce = true;
    }

    void LateUpdate()
    {
        if (stopCameraAtOnce) { return; }
        Vector3 tg = target.position;
        if (SmoothFollow)
        {
            tr.position = Vector3.SmoothDamp(tr.position, tg, ref velocity, smoothTime);
        }
        else
        {
            tr.position = tg;
        }
    }
}
