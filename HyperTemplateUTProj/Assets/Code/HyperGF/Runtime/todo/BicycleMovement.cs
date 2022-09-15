using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BicycleMovement : LevelObjectBehaviour
{
    [SerializeField] float maxHandleAngle = 20f, speedModifier = 40.0f, nextPositionDistance = 5f;
    [SerializeField] Transform pedalRoot, forwardWheelRoot, backwardWheelRoot, forwardHandleRoot;
    [SerializeField] bool debug = false;
    Quaternion initRot;
    protected internal override void OnAwake()
    {
        initRot = forwardHandleRoot.localRotation;   
    }

    protected abstract Vector3 GetPlayerPosition();
    protected abstract Vector3 GetPlayerNextPosition(float distance, out bool valid);
    protected abstract Vector3 GetPlayerForwardDirection();
    protected abstract float GetPlayerSpeed();
    protected abstract bool IsPlayerScriptValid();

    private float lastAngle = 0f;
    void Update()
    {
        if (behaviourEnabled == false) { return; }
        if (IsPlayerScriptValid()) { return; }
        var valid = false;
        var nextPos = GetPlayerNextPosition(nextPositionDistance, out valid);
        if (valid)
        {
            var curPos = GetPlayerPosition();
            var toNextPos = nextPos - curPos;
            toNextPos.y = 0.0f;
            var playerForward = GetPlayerForwardDirection();
            if (debug)
            {
                Debug.DrawRay(curPos, playerForward.normalized * 15f, Color.cyan);
                Debug.DrawRay(curPos, toNextPos.normalized * 15f, Color.black);
                Debug.DrawRay(nextPos, Vector3.up * 15f, Color.blue);
                Debug.DrawRay(curPos, Vector3.up * 15f, Color.red);
            }

            playerForward.y = 0.0f;
            float angle = Vector3.SignedAngle(toNextPos, playerForward, -Vector3.up);

            if (angle > maxHandleAngle) { angle = maxHandleAngle; }
            if (angle < -maxHandleAngle) { angle = -maxHandleAngle; }

            forwardHandleRoot.ExSetRotationAroundLocalXAxis(angle, ref lastAngle);
        }
        else
        {
            forwardHandleRoot.localRotation = Quaternion.Slerp(forwardHandleRoot.localRotation, initRot, 20f * Time.deltaTime);
        }

        var speed = GetPlayerSpeed() * speedModifier;
        MoveWheels(speed, Time.deltaTime);
        void MoveWheels(float speed, float delta)
        {
            pedalRoot.Rotate(speed * delta, 0.0f, 0.0f, Space.Self);
            forwardWheelRoot.Rotate(0.0f, 0.0f, -speed * delta, Space.Self);
            backwardWheelRoot.Rotate(speed * delta, 0.0f, 0.0f, Space.Self);
        }
    }
    //todo assumed axis, vector multiplier and speeds all can change!
}