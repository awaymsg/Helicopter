using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float t;
    private Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
        t = Mathf.Abs(target.transform.position.x - transform.position.x) * 0.02f;
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }
}
