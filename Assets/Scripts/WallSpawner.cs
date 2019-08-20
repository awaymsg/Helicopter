using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wall = null;
    [SerializeField] private float startingInterval = 0.01f;
    [SerializeField] private float startingVel = 4;
    private float vel;
    private float interval;

    void Start()
    {
        interval = startingInterval;
        vel = startingVel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < interval)
        {
            GameObject tempWall = Instantiate(wall);
            tempWall.transform.position = new Vector3(Random.Range(-5.5f, 5.5f), -0.4f, 12);
            if (tempWall.transform.position.x > 4.7)
            {
                tempWall.transform.position = new Vector3(5.5f, -0.4f, 12);
            } else if (tempWall.transform.position.x < -4.7)
            {
                tempWall.transform.position = new Vector3(-5.5f, -0.4f, 12);
            }
            tempWall.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -vel);
        }
    }
}
