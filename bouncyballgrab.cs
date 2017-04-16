using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class bouncyballgrab: MonoBehaviour
{
    custom_TrackedObject activeGrabObject;

    Quaternion initialRotation;
    Vector3 inititalOffset;

    bool activeScale = false;
    bool activeMove = false;

    Vector3 prevpos;


    void Start()
    {
        activeGrabObject = null;
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    void Update()
    {
        updateMove();
    }

    void updateMove()
    {
        if (activeMove)
        {

            gameObject.transform.rotation = activeGrabObject.currRotation * initialRotation;


            gameObject.transform.position = activeGrabObject.deviceRay.origin +
                    inititalOffset.x * activeGrabObject.currUpVec +
                    inititalOffset.y * activeGrabObject.currRightVec +
                    inititalOffset.z * activeGrabObject.currForwardVec;
            prevpos = transform.position;

        }
    }


    public void grab(custom_TrackedObject controller)
    {
        activeGrabObject = controller;

        if (activeGrabObject == null)
        {
            activeMove = false;
        }
        else
        {
            initialRotation = Quaternion.Inverse(activeGrabObject.currRotation) * gameObject.transform.rotation;
            Vector3 tmpVec = gameObject.transform.position - activeGrabObject.currPosition;

            inititalOffset.Set(
                Vector3.Dot(activeGrabObject.currUpVec, tmpVec),
                Vector3.Dot(activeGrabObject.currRightVec, tmpVec),
                Vector3.Dot(activeGrabObject.currForwardVec, tmpVec)
                );
            activeMove = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void release(custom_TrackedObject controller)
    {


        activeGrabObject = null;
        activeMove = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //gameObject.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<VelocityEstimator>().GetVelocityEstimate());
    }




}

