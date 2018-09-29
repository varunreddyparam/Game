using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    [SerializeField]
    private float speed;
    [SerializeField]
    private float angleSpeed;
    [SerializeField]
    private int life = 3; // toatal life of player

    public float Speed { get { return speed; } }

    private Rigidbody myBody;
    private int currentAngle;
    private int currentLife;

	// Use this for initialization
	void Start () {
        myBody = GetComponent<Rigidbody>();
        currentLife = life;
	}
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float x = Input.mousePosition.x;
            if(x < Screen.width / 2 && x > 0)
            {
                MoveLeft();
            }
            if (x > Screen.width / 2 && x < Screen.width)
            {
                MoveRight();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    public void MoveLeft()
    {
        transform.Rotate(-Vector3.up * angleSpeed * Time.deltaTime);
    }
    public void MoveRight()
    {
        transform.Rotate(Vector3.up * angleSpeed * Time.deltaTime);
    }

    void ReduceLife()
    {
        currentLife--;

        GUIManager.instance.ReduceLife(currentLife);

        if(currentLife<=0)
        {
            GameObject explosion = ObjectPooling.instance.GetPooledObject("Explosion");
            explosion.SetActive(true);
            explosion.transform.position = this.transform.position;
            GUIManager.instance.GameOverMethod();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            ReduceLife();
            other.GetComponent<Damage>().ReduceLife();
        }
    }
}
