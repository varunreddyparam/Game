using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWheelScript : MonoBehaviour {

    [SerializeField]
    private WheelCollider wheelCollider;
    private Transform wheelTransform;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        var wheelTransformObject = transform.gameObject.GetComponent<Transform>();
        //Debug.Log("wheelTransformObject Name" + wheelTransformObject.name);
        if (wheelTransformObject.name != "WheelRR" || wheelTransformObject.name != "WheelRL")
        {
            Vector3 vectorObj = transform.localEulerAngles;
            vectorObj.y = wheelCollider.steerAngle - transform.localEulerAngles.z;
            transform.localEulerAngles = vectorObj;
            //transform.localEulerAngles.y = new Vector3(0,wheelCollider.steerAngle - transform.localEulerAngles.z,0);
        }

        RaycastHit hit;
        Vector3 wheelPosition;

        if (Physics.Raycast(wheelCollider.transform.position, -wheelCollider.transform.up, out hit, wheelCollider.radius + wheelCollider.suspensionDistance))
            wheelPosition = hit.point + wheelCollider.transform.up * wheelCollider.radius;
        else
            wheelPosition = wheelCollider.transform.position - wheelCollider.transform.up * wheelCollider.suspensionDistance;

        transform.position = wheelPosition;
    }
}
