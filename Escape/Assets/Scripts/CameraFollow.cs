using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject player;

    // Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        Movement();
	}
    void Movement()
    {
        float posX = player.transform.position.x;
        float posZ = player.transform.position.z - 25;

        transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
