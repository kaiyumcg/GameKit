using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootHandler : LevelObjectBehaviour
{
    [SerializeField] bool SmoothFollow = false;
    [SerializeField] float smoothTime = 0.06f;

    [SerializeField] RangedVector3 rotationOffset;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Transform target;
    [SerializeField] Camera cam;
    [SerializeField] Transform cameraTransform;
    Vector3 velocity = Vector3.zero;
    float camTrInitPosY;
    Transform tr, camTr;
    bool stopCameraAtOnce = false;

    public static CameraRootHandler Instance { get { return instance; } }
    static CameraRootHandler instance;

    public void SetOffset(float yOffset)
    {
        var pos = camTr.localPosition;
        pos.y = camTrInitPosY + yOffset;
        camTr.localPosition = pos;
    }

    public void StopCameraDueToDeath()
    {
        stopCameraAtOnce = true;
    }

    bool isPaused = false;
    public void EndRoadCameraSetPaused(bool isPaused) { this.isPaused = isPaused; }

    void LateUpdate()
    {
        if (stopCameraAtOnce || isPaused) { return; }
        
        Vector3 tg = target.position + offsetPosition;
        if (SmoothFollow)
        {
            tr.position = Vector3.SmoothDamp(tr.position, tg, ref velocity, smoothTime);
        }
        else
        {
            tr.position = tg;
        }

        var toTarget = target.position - cameraTransform.position;
        toTarget = toTarget.normalized + rotationOffset.Get();
        if (Mathf.Approximately(toTarget.magnitude, 0.0f) == false)
        {
            var qot = Quaternion.LookRotation(toTarget, Vector3.up);
            cameraTransform.rotation = qot;
        }
    }

    protected internal override void OnAwake()
    {
        camTr = cam.transform;
        camTrInitPosY = camTr.localPosition.y;
        cam.depthTextureMode = DepthTextureMode.Depth;
        instance = this;
        tr = transform;
        stopCameraAtOnce = false;
    }
}
