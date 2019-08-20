using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] private Quaternion baseRotation = new Quaternion(0, 0, 1, 0);

    public void Start()
    {
        Gyro.Instance.EnableGyro();
    }

    private void Update()
    {
        transform.localRotation = Gyro.Instance.GetGyroRotation() * baseRotation;
    }
}
