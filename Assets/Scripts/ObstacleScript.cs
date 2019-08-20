using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.z < -5)
        {
            Destroy(gameObject);
        }
    }
}
