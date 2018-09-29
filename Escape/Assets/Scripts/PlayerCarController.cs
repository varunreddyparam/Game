using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider WheelFL;
    [SerializeField]
    private WheelCollider WheelFR;
    [SerializeField]
    private WheelCollider WheelRR;
    [SerializeField]
    private WheelCollider WheelRL;
    [SerializeField]
    private float maxTorque; //50;
    [SerializeField]
    private Transform wheelFLTrans;
    [SerializeField]
    private Transform wheelFRTrans;
    [SerializeField]
    private Transform wheelRLTrans;
    [SerializeField]
    private Transform wheelRRTrans;
    private float currentSpeed;
    [SerializeField]
    private int topSpeed; //= 100;
    [SerializeField]
    private int maxBrakeTorque; //= 10;
    private bool braked = false;
    [SerializeField]
    private float loweststeerAtspeed; //= 80;
    [SerializeField]
    private float lowspeedsteerAngel; //= 10;
    [SerializeField]
    private float highspeedsteerAngel;//= 1;
    [SerializeField]
    private float decellarationspeed; //100;
    [SerializeField]
    private float maxreversespeed; //20;
    private int[] gearRatio;
    private int gear;
    private Transform Playertransform;
    [SerializeField]
    private Rigidbody rigidBody;
    private RaycastHit hit;
    private Vector3 wheelPos;
    [SerializeField]
    private Transform COM;
    private float mySidewayFriction;
    private float myForwardFriction;
    private float slipSidewayFriction;
    private float slipForwardFriction;
    private WheelFrictionCurve frictionCurve;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = new Vector3(0, -0.9f, -0.1f);

        SetValues();
    }
    void FixedUpdate()
    {
        Control();
        Handbrake();
    }

    private void Handbrake()
    {
        if (Input.GetButton("Jump"))
        {
            braked = true;
        }
        else
        {
            braked = false;
        }
        if (braked)
        {
            if (currentSpeed > 1)
            {
                WheelFR.brakeTorque = maxBrakeTorque;
                WheelFL.brakeTorque = maxBrakeTorque;
                WheelRR.motorTorque = 0;
                WheelRL.motorTorque = 0;
                WheelFR.brakeTorque = 0;
                WheelFL.brakeTorque = 0;
                SetRearSlip(slipForwardFriction, slipSidewayFriction);
            }
            else if (currentSpeed < 0)
            {
                WheelRR.brakeTorque = maxBrakeTorque;
                WheelRL.brakeTorque = maxBrakeTorque;
                WheelRR.motorTorque = 0;
                WheelRL.motorTorque = 0;
                SetRearSlip(1, 1);
            }
            else
            {
                SetRearSlip(1, 1);
            }
        }
        else
        {
            WheelFR.brakeTorque = 0;
            WheelFL.brakeTorque = 0;
            SetRearSlip(myForwardFriction, mySidewayFriction);
        }

    }


    private void SetValues()
    {
        myForwardFriction = WheelRR.forwardFriction.stiffness;
        mySidewayFriction = WheelRR.sidewaysFriction.stiffness;
        slipForwardFriction = 0.05f;
        slipSidewayFriction = 0.085f;

    }

    private void Control()
    {
        currentSpeed = Mathf.Abs(2 * 22 / 7 * WheelRL.radius * WheelRL.rpm * 60 / 1000);
        currentSpeed = Mathf.Round(1.5f * currentSpeed);

        Debug.Log("*********Curretn speed = " + currentSpeed);

        if (currentSpeed <= topSpeed && currentSpeed > -maxreversespeed)
        {
            WheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical");
            WheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical");
        }
        else
        {
            WheelRR.motorTorque = 0;
            WheelRL.motorTorque = 0;
        }
        if (Input.GetButton("Vertical") == false)
        {
            WheelRL.brakeTorque = decellarationspeed;
            WheelRR.brakeTorque = decellarationspeed;
        }
        else
        {
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
        }
        var Speedfactor = rigidBody.velocity.magnitude / loweststeerAtspeed;
        var currentSteerAngel = Mathf.Lerp(lowspeedsteerAngel, highspeedsteerAngel, Speedfactor);
        WheelFL.steerAngle = 10 * Input.GetAxis("Horizontal");
        WheelFR.steerAngle = 10 * Input.GetAxis("Horizontal");
    }

    void Update()
    {
        wheelFLTrans.Rotate(WheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelFRTrans.Rotate(WheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelRLTrans.Rotate(WheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelRRTrans.Rotate(WheelRR.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        //wheelFLTrans.localEulerAngles = new Vector3(0, WheelFL.steerAngle - wheelFLTrans.localEulerAngles.z, 0);
        //wheelFRTrans.localEulerAngles = new Vector3(0, WheelFR.steerAngle - wheelFRTrans.localEulerAngles.z, 0);
        //WheelPosition();
        ReverseSlip();
        EngineSound();
    }

    private void WheelPosition()
    {
        //FL
        if (Physics.Raycast(WheelFL.transform.position, -WheelFL.transform.up, out hit, Convert.ToInt32(WheelFL.radius + WheelFL.suspensionDistance)))
        {
            wheelPos = hit.point + WheelFL.transform.up * WheelFL.radius;
        }
        else
        {
            wheelPos = WheelFL.transform.position - WheelFL.transform.up * WheelFL.suspensionDistance;
        }
        wheelFLTrans.position = wheelPos;
        Debug.Log("FL" + wheelPos);
        Debug.Log("FL Hit Point" + hit.point);
        Debug.Log("FL transformUp" + WheelFL.transform.up);
        Debug.Log("FL Radius" + WheelFL.radius);

        //FR

        if (Physics.Raycast(WheelFR.transform.position, -WheelFR.transform.up, out hit, Convert.ToInt32(WheelFR.radius + WheelFR.suspensionDistance)))
        {
            wheelPos = hit.point + WheelFR.transform.up * WheelFR.radius;
        }
        else
        {
            wheelPos = WheelFR.transform.position - WheelFR.transform.up * WheelFR.suspensionDistance;
        }
        wheelFRTrans.position = wheelPos;

        //RL

        if (Physics.Raycast(WheelRL.transform.position, -WheelRL.transform.up, out hit, Convert.ToInt32(WheelRL.radius + WheelRL.suspensionDistance)))
        {
            wheelPos = hit.point + WheelRL.transform.up * WheelRL.radius;
        }
        else
        {
            wheelPos = WheelRL.transform.position - WheelRL.transform.up * WheelRL.suspensionDistance;
        }
        wheelRLTrans.position = wheelPos;

        //RR

        if (Physics.Raycast(WheelRR.transform.position, -WheelRR.transform.up, out hit, Convert.ToInt32(WheelRR.radius + WheelRR.suspensionDistance)))
        {
            wheelPos = hit.point + WheelRR.transform.up * WheelRR.radius;
        }
        else
        {
            wheelPos = WheelRR.transform.position - WheelRR.transform.up * WheelRR.suspensionDistance;
        }
        wheelRRTrans.position = wheelPos;
    }

    private void ReverseSlip()
    {
        if (currentSpeed < 0)
        {
            SetFrontSlip(slipForwardFriction, slipSidewayFriction);
        }

        else
        {
            SetFrontSlip(myForwardFriction, mySidewayFriction);
        }
    } 

    private void SetRearSlip(float currentForwardFriction, float currentSidewayFriction)
    {
        frictionCurve = WheelRR.forwardFriction;
        frictionCurve.stiffness = currentForwardFriction;
        WheelRR.forwardFriction = frictionCurve;

        frictionCurve = WheelRL.forwardFriction;
        frictionCurve.stiffness = currentForwardFriction;
        WheelRL.forwardFriction = frictionCurve;

        frictionCurve = WheelRL.sidewaysFriction;
        frictionCurve.stiffness = currentSidewayFriction;
        WheelRL.sidewaysFriction = frictionCurve;

        frictionCurve = WheelRR.sidewaysFriction;
        frictionCurve.stiffness = currentSidewayFriction;
        WheelRR.sidewaysFriction = frictionCurve;
    }

    private void SetFrontSlip(float currentForwardFriction, float currentSidewayFriction)
    {
        frictionCurve = WheelFR.forwardFriction;
        frictionCurve.stiffness = currentForwardFriction;
        WheelFR.forwardFriction = frictionCurve;

        frictionCurve = WheelFL.forwardFriction;
        frictionCurve.stiffness = currentForwardFriction;
        WheelFL.forwardFriction = frictionCurve;

        frictionCurve = WheelFR.sidewaysFriction;
        frictionCurve.stiffness = currentSidewayFriction;
        WheelFR.sidewaysFriction = frictionCurve;

        frictionCurve = WheelFL.sidewaysFriction;
        frictionCurve.stiffness = currentSidewayFriction;
        WheelFL.sidewaysFriction = frictionCurve;
    }

    private void EngineSound()
    {
        float gearMinValue = 0.0f;
        float gearMaxValue = 0.0f;
        for (int i = 0; i < gearRatio.Length; i++)
        {
            gear = i;
            if (gearRatio[i] > currentSpeed)
            {
                break;
            }
            if (i == 0)
            {
                gearMinValue = gearRatio[i];
                gearMaxValue = gearRatio[i + 1];
            }
            if ((gearMinValue - gearMaxValue) == 0)
            {

                float enginepitch = ((currentSpeed - gearMinValue) / (gearMaxValue - gearMinValue)) + 1;
            }
            //audio.pitch = enginepitch;
        }
    }
}


