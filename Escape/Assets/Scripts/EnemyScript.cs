using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    [SerializeField]
    private float speed = 40, rotatingSpeed = 90;

    private GameObject target;
    private Rigidbody mybody;
	// Use this for initialization
	void Start () {
        mybody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        Vector3 pointTarget = transform.position - target.transform.position;
        pointTarget.Normalize();
        float value = Vector3.Cross(pointTarget, transform.forward).y;
        mybody.angularVelocity = rotatingSpeed * value * new Vector3(0, 1, 0);
        mybody.velocity = transform.forward * speed;
	}
}
