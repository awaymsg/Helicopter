using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private Fir xFir;
    private Fir yFir;
    private Fir zFir;
    [SerializeField] private float t;
    [SerializeField] private int size;
    [SerializeField] private float moveSpeed = 0;

    Matrix4x4 calibrationMatrix;
    Vector3 wantedDeadZone = Vector3.zero;
    Vector3 inputDir;
    Vector3 tilt;

    private void CalibrateAccelerometer()
    {
        wantedDeadZone = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), wantedDeadZone);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
        calibrationMatrix = matrix.inverse;
    }

    private Vector3 GetAccel(Vector3 accel)
    {
        Vector3 a = calibrationMatrix.MultiplyVector(accel);
        return a;
    }

    void Start()
    {
        xFir = new Fir(size);
        yFir = new Fir(size);
        zFir = new Fir(size);
        CalibrateAccelerometer();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        xFir.writeSample(Input.acceleration.x);
        yFir.writeSample(Input.acceleration.y);
        zFir.writeSample(Input.acceleration.z);
        
        inputDir = GetAccel(new Vector3(xFir.getOutput(), yFir.getOutput(), zFir.getOutput()));
        
        tilt = new Vector3(inputDir.x * 3, inputDir.y, 0) * moveSpeed;

        tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rb.AddForce(tilt);

        transform.eulerAngles = new Vector3(20 + rb.velocity.z * 12f, 0, rb.velocity.x * -6f);
    }

}