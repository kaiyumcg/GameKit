using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class CarFakePhysics : MonoBehaviour
{
    [SerializeField]
    Transform visualRoot, carBody,
        wh_forward_L, wh_forward_R,
        wh_backward_L, wh_backward_R, smokept;
    internal Transform VisualRoot { get { return visualRoot; } }
    internal Transform SmokePoint { get { return smokept; } }

    const float carbump_min = -1f, carbump_max = 1f,
        forward_wheel_left_angle = -30f, forward_wheel_right_angle = 30f,
        backward_wheel_left_angle = -20f, backward_wheel_right_angle = 20f;

    bool withinBumpiness = false, lerping = false;
    float bumpiness_from_volume = 1.0f, dt = 0.0f;
    TweenerCore<Quaternion, Vector3, QuaternionOptions> tw = null;
    bool isVisible = true, isMoving = false;
    public bool IsVisible { get { return isVisible; } set { isVisible = value; } }
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

    public void OnInit()
    {
        lerping = withinBumpiness = false;
        bumpiness_from_volume = 1.0f;
        carBody.localEulerAngles = Vector3.zero;
    }

    private void OnDestroy()
    {
        if (tw != null && tw.IsActive()) { tw.Kill(); tw = null; }
    }

    internal void OnUpdate(float delta, Vector3 willVector, Vector3 targetForwardVector)
    {
        this.dt = delta;

        if (isVisible == false)
        {
            return;
        }

        SteerWheels(willVector, targetForwardVector);
        if (isMoving)
        {
            FakeCarBump();
        }
        else if (withinBumpiness && isMoving == false)
        {

        }
        else
        {
            ResetCarBump();
        }
    }

    public void ResetPhysics()
    {
        if (wh_forward_L != null) { wh_forward_L.localEulerAngles = Vector3.zero; }
        if (wh_forward_R != null) { wh_forward_R.localEulerAngles = Vector3.zero; }

        if (wh_backward_L != null) { wh_backward_L.localEulerAngles = Vector3.zero; }
        if (wh_backward_R != null) { wh_backward_R.localEulerAngles = Vector3.zero; }
        if (carBody != null) { carBody.localEulerAngles = Vector3.zero; }
    }

    public void OnEnterBumpinessTag(float amount)
    {
        bumpiness_from_volume = amount;
        withinBumpiness = true;
    }

    public void OnExitBumpinessTag()
    {
        bumpiness_from_volume = 1.0f;
        withinBumpiness = false;
    }

    void ResetCarBump()
    {
        carBody.localRotation = Quaternion.Slerp(carBody.localRotation, Quaternion.identity, 20f * dt);
        if (tw != null && tw.IsActive()) { tw.Kill(); tw = null; }
        lerping = false;
    }

    void FakeCarBump()
    {
        if (lerping == false)
        {
            lerping = true;
            var target_vl = Vector3.zero;
            target_vl.z = UnityEngine.Random.Range(carbump_min, carbump_max) * bumpiness_from_volume;

            if (tw != null && tw.IsActive()) { tw.Kill(); tw = null; }
            tw = carBody.DOLocalRotate(target_vl, 0.1f).OnComplete(() =>
            {
                lerping = false;
            });
        }
    }

    void SteerWheels(Vector3 willVector, Vector3 targetForwardVector)
    {
        var willDir = willVector;
        willDir.y = 0.0f;
        var fdir = targetForwardVector;
        fdir.y = 0.0f;
        var angleBetween = Vector3.SignedAngle(fdir, willDir, Vector3.up);
        var f_angle = angleBetween.Remap(-45f, 45f, forward_wheel_left_angle, forward_wheel_right_angle);
        var b_angle = angleBetween.Remap(-45f, 45f, backward_wheel_left_angle, backward_wheel_right_angle);
        float backwardAngle; float forwardAngle;
        if (angleBetween < 0.0f)
        {
            //left
            forwardAngle = -f_angle;
            backwardAngle = b_angle;
        }
        else
        {
            //right
            forwardAngle = f_angle;
            backwardAngle = -b_angle;
        }

        wh_forward_L.localEulerAngles = new Vector3(0.0f, forwardAngle, 0.0f);
        wh_forward_R.localEulerAngles = new Vector3(0.0f, forwardAngle, 0.0f);

        wh_backward_L.localEulerAngles = new Vector3(0.0f, backwardAngle, 0.0f);
        wh_backward_R.localEulerAngles = new Vector3(0.0f, backwardAngle, 0.0f);
    }
}