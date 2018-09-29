using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    [SerializeField]
    private float invincibleTime = 2;

    [SerializeField]
    private int life = 3;
    [SerializeField]
    private GameObject smokeEffect, fireEffect, explosionEffect; 

    private int currentlife;
    private float currentInvincibleTime;
    private bool isColliding = false;

	// Use this for initialization
	void Start () {
        currentlife = life;
        currentInvincibleTime = invincibleTime;
	}
	
	// Update is called once per frame
	void Update () {
        if(isColliding)
        {
            currentInvincibleTime -= Time.deltaTime;
            Debug.Log("currentInvincibleTime" + currentInvincibleTime);
            if (currentInvincibleTime <=0)
            {
                ReduceLife();
                Debug.Log("currentLife" + currentlife);

            }
        }
	}

    //collision continusly is happening
    private void OnCollisionStay(Collision other)
    {
        if(other.collider.CompareTag("Enemy"))
        {
            isColliding = true;
        }
    }

    // when nomore colliding each other and called once
    public void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            isColliding = false;
        }
    }

    public void ReduceLife()
    {
        currentlife--;
        currentInvincibleTime = invincibleTime;
        switch (currentlife)
        {
            case -1:
                CurrentLifeIsLessthanZero();
                break;
            case 0:
                CurrentLifeIsLessthanZero();
                break;
            case 1:
                Debug.Log("case 1" + currentlife);
                smokeEffect.SetActive(false);
                fireEffect.SetActive(true);
                break;
            case 2:
                Debug.Log("case 2" + currentlife);
                smokeEffect.SetActive(true);
                break;
         }
    }

    public void DefaultSettings()
    {
        currentlife = life;
        smokeEffect.SetActive(false);
        fireEffect.SetActive(false);
    }

    public void CurrentLifeIsLessthanZero()
    {
        Debug.Log("Enemy Destroyed");
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);
        GameObject explosion = ObjectPooling.instance.GetPooledObject("Explosion");
        explosion.SetActive(true);
        explosion.transform.position = this.transform.position;
        EnemySpawner.instance.CurrentPoliceCar--;
        gameObject.SetActive(false);
    }
}
