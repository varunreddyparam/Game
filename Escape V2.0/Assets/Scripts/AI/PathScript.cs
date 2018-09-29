using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathScript : MonoBehaviour
{

    //[HideInInspector]
    public Transform[] nodes;

    // Use this for initialization
    void Start()
    {

    }

    void OnDrawGizmos()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();

        nodes = new Transform[transforms.Length - 1];

        for (int n = 0; n < nodes.Length; n++)
        {
            nodes[n] = transforms[n + 1];
        }

        for (int n = 0; n < nodes.Length; n++)
        {
            Vector3 pos = nodes[n].position;
            if (n > 0)
            {
                Vector3 prev = nodes[n - 1].position;
                Gizmos.DrawLine(prev, pos);
                Gizmos.DrawWireSphere(nodes[n].position, 0.3f);
            }
            //else
            //{
                //Gizmos.DrawLine(nodes[n].position - Vector3.down, nodes[(n + 1) % nodes.Length].position - Vector3.down);
                //Gizmos.DrawWireSphere(nodes[n].position, 0.3f);
            //}
        }
    }
}
