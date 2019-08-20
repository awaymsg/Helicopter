using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    private static Gyro instance;
    public static Gyro Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Gyro>();
                if (instance == null)
                {
                    instance = new GameObject("GyroManager", typeof(Gyro)).GetComponent<Gyro>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private Vector3 rotateamt = new Vector3(0, 0, -45);
    private Rigidbody rb;
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    public void EnableGyro()
    {
        if (gyroActive)
        {
            return;
        }

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;
        }
    }

    public Quaternion GetGyroRotation()
    {
        return rotation;
    }

    void Update()
    {
        if (gyroActive)
        {
            rotation = gyro.attitude;
            Debug.Log(rotation);
        }
    }
}
