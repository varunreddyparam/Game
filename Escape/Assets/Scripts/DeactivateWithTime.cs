using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateWithTime : MonoBehaviour {
    [SerializeField]
    private float deactivateTime;

    private float currentTime;

	// Use this for initialization
	void Start () {
        currentTime = deactivateTime;
	}
	
	// Update is called once per frame
	void Update () {
        currentTime -= Time.deltaTime;
        if(currentTime < 0)
        {
            currentTime = deactivateTime;
            gameObject.SetActive(false);
        }
	}
}
