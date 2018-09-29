using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarpathfollow : MonoBehaviour
{

    private Transform[] path;
    [SerializeField]
    private Transform pathGroup;
    [SerializeField]
    private float maxSteer = 15.0f;
    [SerializeField]
    private int currentPathObj;
    [SerializeField]
    private WheelCollider wheelFL;
    [SerializeField]
    private WheelCollider wheelFR;
    [SerializeField]
    private WheelCollider wheelRL;
    [SerializeField]
    private WheelCollider wheelRR;
    [SerializeField]
    private float maxTorque = 50;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float topSpeed = 240;
    [SerializeField]
    private float decellarationSpeed = 10;
    //[SerializeField]
    //private Transform centerOfMass;
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private float distFromPath;
    //var breakingMesh : Renderer;
    //var idleBreakLight : Material;
    //var activeBreakLight : Material;
    private bool isBreaking;
    private bool inSector;
    public float CurrentAICarSpeed { get { return currentSpeed; } }
    public bool IsBreaking { get { return isBreaking; } set { isBreaking = value; } }
    public bool InSector { get { return inSector; } set { inSector = value; } }
    public WheelCollider WheelRRCollider { get { return wheelRR; } set { wheelRR = value; } }
    public WheelCollider WheelRLCollider { get { return wheelRL; } set { wheelRL = value; } }

    private float sensorLength = 10;
    private float frontSensorStartPoint = 5;
    private float frontSensorSideDist = 5;
    private float frontSensorsAngle = 90;
    private float sidewaySensorLength = 5;
    private float avoidSpeed = 10;
    private int flag = 0;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = new Vector3(0, -0.9f, -0.1f);
        GetPath();
    }

    private void GetPath()
    {
        Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
        //Debug.Log("path_objs" + path_objs.Length);
        path = new Transform[path_objs.Length - 1];

        for (int n = 0; n < path.Length; n++)
        {
            path[n] = path_objs[n];
        }
    }


    void Update()
    {
        if (flag == 0)
            GetSteer();
        Move();
        BreakingEffect();
        Sensors();
    }

    private void GetSteer()
    {
        //Debug.Log("currentPathObj " + currentPathObj);
        //Debug.Log("pathLength " + path.Length);
        Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
        float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;

        if (steerVector.magnitude <= distFromPath)
        {
            currentPathObj++;
            if (currentPathObj >= path.Length)
            {
                currentPathObj = 0;
            }
        }
    }

    private void Move()
    {
        currentSpeed = 2 * (22 / 7) * wheelRL.radius * wheelRL.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);
        if (currentSpeed <= topSpeed && !inSector)
        {
            wheelRL.motorTorque = maxTorque;
            wheelRR.motorTorque = maxTorque;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
        else if (!inSector)
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
            wheelRL.brakeTorque = decellarationSpeed;
            wheelRR.brakeTorque = decellarationSpeed;
        }
    }

    private void BreakingEffect()
    {
        if (isBreaking)
        {
            //breakingMesh.material = activeBreakLight;
        }
        else
        {
            //breakingMesh.material = idleBreakLight;
        }
    }

    private void Sensors()
    {
        flag = 0;
        float avoidSenstivity = 0;
        Vector3 pos;
        RaycastHit hit;
        var rightAngle = Quaternion.AngleAxis(frontSensorsAngle, transform.up) * transform.forward;
        var leftAngle = Quaternion.AngleAxis(-frontSensorsAngle, transform.up) * transform.forward;


        pos = transform.position;
        pos += transform.forward * frontSensorStartPoint;
        // BRAKING SENSOR

        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
            if (hit.transform.tag != "Plane")
            {
                flag++;
                wheelRL.brakeTorque = decellarationSpeed;
                wheelRR.brakeTorque = decellarationSpeed;
                Debug.DrawLine(pos, hit.point, Color.red);
            }
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }

        //Front Mid Sensor
        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {

            Debug.DrawLine(pos, hit.point, Color.green);
        }

        //Front Straight Right Sensor
        pos += transform.right * frontSensorSideDist;

        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
            if (hit.transform.tag != "Plane")
            {
                flag++;
                avoidSenstivity -= 0.5f;
                Debug.Log("Avoiding");
                Debug.DrawLine(pos, hit.point, Color.green);
            }
        }

        //Front Angled Right Sensor

        if (Physics.Raycast(pos, rightAngle, out hit, sensorLength))
        {

            Debug.DrawLine(pos, hit.point, Color.green);
        }


        //Front Straight left Sensor
        pos = transform.position;
        pos += transform.forward * frontSensorStartPoint;
        pos -= transform.right * frontSensorSideDist;

        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
            if (hit.transform.tag != "Plane")
            {
                flag++;
                avoidSenstivity += 0.5f;
                Debug.Log("Avoiding");
                Debug.DrawLine(pos, hit.point, Color.green);
            }
        }


        //Front Angled left Sensor
        if (Physics.Raycast(pos, leftAngle, out hit, sensorLength))
        {

            Debug.DrawLine(pos, hit.point, Color.green);
        }

        //Right SideWay Sensor
        if (Physics.Raycast(transform.position, transform.right, out hit, sidewaySensorLength))
        {

            Debug.DrawLine(transform.position, hit.point, Color.green);
        }


        //Left SideWay Sensor
        if (Physics.Raycast(transform.position, -transform.right, out hit, sidewaySensorLength))
        {

            Debug.DrawLine(transform.position, hit.point, Color.green);
        }

        if (flag != 0)
            AvoidSteer(avoidSenstivity);
    }

    private void AvoidSteer(float senstivity)
    {
        wheelFL.steerAngle = maxSteer * senstivity;
        wheelFR.steerAngle = maxSteer * senstivity;
        Debug.Log("MaxSteer" + (maxSteer * senstivity));
    }
}
