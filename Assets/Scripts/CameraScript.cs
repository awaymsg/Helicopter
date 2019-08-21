using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private int size;
    private float t;
    private Vector3 targetPos;
    private Fir xFir;

    private void Awake()
    {
        xFir = new Fir(size);
    }

    // Update is called once per frame
    void Update()
    {
        xFir.writeSample(target.transform.position.x / 5f);
        targetPos = new Vector3(xFir.getOutput(), transform.position.y, transform.position.z);
        t = Mathf.Abs(xFir.getOutput() - transform.position.x) * 0.015f;
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }
}
