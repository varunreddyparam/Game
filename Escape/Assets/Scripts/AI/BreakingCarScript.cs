using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingCarScript : MonoBehaviour
{
    [SerializeField]
    private float maxBreakTorque;
    [SerializeField]
    private float minCarSpeed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "RivalCar")
        {
            float controlCurrentSpeed = other.transform.root.GetComponent<AICarpathfollow>().CurrentAICarSpeed;
            if (controlCurrentSpeed >= minCarSpeed)
            {
                other.transform.root.GetComponent<AICarpathfollow>().InSector = true;
                other.transform.root.GetComponent<AICarpathfollow>().WheelRRCollider.brakeTorque = maxBreakTorque;
                other.transform.root.GetComponent<AICarpathfollow>().WheelRLCollider.brakeTorque = maxBreakTorque;
            }
            else
            {
                other.transform.root.GetComponent<AICarpathfollow>().InSector = false;
                other.transform.root.GetComponent<AICarpathfollow>().WheelRRCollider.brakeTorque = 0;
                other.transform.root.GetComponent<AICarpathfollow>().WheelRLCollider.brakeTorque = 0;
            }
            other.transform.root.GetComponent<AICarpathfollow>().IsBreaking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RivalCar")
        {
            other.transform.root.GetComponent<AICarpathfollow>().InSector = false;
            other.transform.root.GetComponent<AICarpathfollow>().WheelRRCollider.brakeTorque = 0;
            other.transform.root.GetComponent<AICarpathfollow>().WheelRLCollider.brakeTorque = 0;
            other.transform.root.GetComponent<AICarpathfollow>().IsBreaking = false;
        }
    }
}
