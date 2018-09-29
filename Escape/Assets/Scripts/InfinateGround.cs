using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinateGround : MonoBehaviour
{

    [SerializeField]
    private Renderer groundRenderer;
    [SerializeField]
    private float parallelxSpeed = 10;
    private GameObject player;
    float offsetX, offsetY;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        if (player != null)
            ScrollBackground(player.GetComponent<CarController>().Speed,groundRenderer);
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;
        Movement();
        

    }

    void Movement()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }

    private void ScrollBackground(float scrollSpeed, Renderer rend)
    {
        offsetX = transform.position.x / parallelxSpeed;
        offsetY = transform.position.z / parallelxSpeed;

        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
