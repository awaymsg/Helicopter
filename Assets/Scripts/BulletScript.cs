using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody rb;

    public void Go(Vector3 pVel, Vector3 bVel)
    {
        pVel.x = pVel.x * -.2f;
        rb.velocity = bVel + pVel;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 8)
        {
            Destroy(gameObject);
        }
    }
}
