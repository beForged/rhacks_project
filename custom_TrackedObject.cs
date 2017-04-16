using UnityEngine;
using System.Collections;
using Valve.VR;

public class custom_TrackedObject : SteamVR_TrackedObject
{
    VRControllerState_t state;
    VRControllerState_t prevState;

    public grabObject activeBeamInterceptObj;

    float beamDist;

    CVRSystem vrSystem;

    public Ray deviceRay;

    public Vector3 currPosition;
    public Vector3 currRightVec;
    public Vector3 currUpVec;
    public Vector3 currForwardVec;
    public Quaternion currRotation;
    public float currRayAngle;
    
    void Start()
    {
        currRayAngle = 0;
        vrSystem = OpenVR.System;
    }

    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(deviceRay.origin, deviceRay.direction, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.gameObject.GetComponent<grabObject>())
            {
                activeBeamInterceptObj = hitInfo.collider.GetComponent<grabObject>();
                beamDist = hitInfo.distance;
            }
            else
            {
                activeBeamInterceptObj = null;
            }
           
        }
        else
        {
            activeBeamInterceptObj = null;
        }

        currPosition = transform.position;
        currRightVec = transform.right;
        currUpVec = transform.up;
        currForwardVec = transform.forward;
        currRotation = transform.rotation;
        deviceRay.origin = currPosition;

        //Quaternion rayRotation = Quaternion.AngleAxis(currRayAngle, currRightVec);

        deviceRay.direction = currForwardVec;

        handleStateChanges();


    }

    void handleStateChanges()
    {
        bool stateIsValid = vrSystem.GetControllerState((uint) index, ref state,
            (uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(VRControllerState_t)));


        if (!stateIsValid) Debug.Log("Invalid State for Idx: " + index);

        if (stateIsValid && state.GetHashCode() != prevState.GetHashCode())
        {
            if ((state.ulButtonPressed & SteamVR_Controller.ButtonMask.Trigger) != 0 &&
                (prevState.ulButtonPressed & SteamVR_Controller.ButtonMask.Trigger) == 0)
            {
                if (activeBeamInterceptObj == null)
                {
                    return;
                }
                activeBeamInterceptObj.grab(this);

            }
            else if ((state.ulButtonPressed & SteamVR_Controller.ButtonMask.Trigger) == 0 &&
                     (prevState.ulButtonPressed & SteamVR_Controller.ButtonMask.Trigger) != 0)
            {
                if (activeBeamInterceptObj == null)
                {
                    return;
                }
                activeBeamInterceptObj.release(this);
                activeBeamInterceptObj = null;
            }

            prevState = state;
        }
    }
}
