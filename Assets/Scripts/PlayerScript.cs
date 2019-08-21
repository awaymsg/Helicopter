using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private Fir xFir;
    private Fir yFir;
    private Fir zFir;
    [SerializeField] private int size;
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private GameObject bullet;

    Matrix4x4 calibrationMatrix;
    Vector3 wantedDeadZone = Vector3.zero;
    Vector3 inputDir;
    Vector3 tilt;

    private Weapon minigun;
    private Weapon spreadshot;
    private Weapon[] weapons = new Weapon[3];
    private float fi;
    private bool canShoot;
    private int numWeapons;
    private int weaponIdx;

    private void CalibrateAccelerometer()
    {
        int calSampleSize = 2048;
        Fir calxFir = new Fir(calSampleSize);
        Fir calyFir = new Fir(calSampleSize);
        Fir calzFir = new Fir(calSampleSize);
        for (int i = 0; i < calSampleSize; i++)
        {
            calxFir.writeSample(Input.acceleration.x);
            calyFir.writeSample(Input.acceleration.y);
            calzFir.writeSample(Input.acceleration.z);
            new WaitForSecondsRealtime(0.01f);
        }
        wantedDeadZone = new Vector3(calxFir.getOutput(), calyFir.getOutput(), calzFir.getOutput());
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), wantedDeadZone);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
        calibrationMatrix = matrix.inverse;
    }

    private Vector3 GetAccel(Vector3 accel)
    {
        Vector3 a = calibrationMatrix.MultiplyVector(accel);
        return a;
    }

    private void Awake()
    {
        xFir = new Fir(size);
        yFir = new Fir(size);
        zFir = new Fir(size);
        rb = GetComponent<Rigidbody>();
        minigun = new Minigun("Minigun", 0.1f, 10);
        spreadshot = new SpreadShot("Spreadshot", 0.3f, 10);
        weapons[0] = minigun;
        weapons[1] = spreadshot;
        weaponIdx = 0;
        fi = weapons[weaponIdx].GetFiringInterval();
        numWeapons = 2;
        canShoot = true;

        CalibrateAccelerometer();
    }

    public void Fire()
    {
        weapons[weaponIdx].Fire();
    }

    private void Update()
    {
        xFir.writeSample(Input.acceleration.x);
        yFir.writeSample(Input.acceleration.y);
        zFir.writeSample(Input.acceleration.z);
        
        inputDir = GetAccel(new Vector3(xFir.getOutput(), yFir.getOutput(), zFir.getOutput()));
        
        tilt = new Vector3(inputDir.x * 3, inputDir.y, 0) * moveSpeed;

        tilt = Quaternion.Euler(90, 0, 0) * tilt;

        rb.AddForce(tilt);

        transform.position = new Vector3(transform.position.x, -0.4f, transform.position.z);

        //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -5f, 5f), 0, Mathf.Clamp(rb.velocity.z, -5f, 5f));

        transform.eulerAngles = new Vector3(20 + rb.velocity.z * 12f, 0, rb.velocity.x * -6f);

        if (fi <= 0)
        {
            canShoot = true;
            fi = weapons[weaponIdx].GetFiringInterval();
        }

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Stationary)
            {
                if (canShoot)
                {
                    Fire();
                    canShoot = false;
                }
            }
        }

        if (Input.touchCount == 2)
        {
            Touch t2 = Input.GetTouch(1);
            if (t2.phase == TouchPhase.Began)
            {
                if (weaponIdx + 1 >= weapons.Length || weaponIdx + 1 >= numWeapons)
                {
                    weaponIdx = 0;
                } else if (weaponIdx + 1 < numWeapons)
                {
                    weaponIdx++;
                }
            }
        }

        fi -= Time.deltaTime;
    }

    //weapons
    private interface Weapon
    {
        string GetName();
        float GetFiringInterval();
        void Fire();
        void Reload();
    }

    private class Minigun : Weapon
    {
        private GameObject o;
        private string name;
        private float firingInterval;
        private float damage;

        public Minigun(string n, float fi, float d)
        {
            name = n;
            firingInterval = fi;
            damage = d;
        }

        public string GetName()
        {
            return name;
        }

        public float GetFiringInterval()
        {
            return firingInterval;
        }

        public void Fire()
        {
            GameObject o = GameObject.FindGameObjectWithTag("Player");
            GameObject b = Instantiate(o.GetComponent<PlayerScript>().bullet);
            b.transform.position = o.transform.position + new Vector3(0, 0, 0.1f);
            b.GetComponent<BulletScript>().Go(o.GetComponent<PlayerScript>().rb.velocity, new Vector3(0, 0, 20));
        }

        public void Reload()
        {

        }
    }

    private class SpreadShot : Weapon
    {
        private GameObject o;
        private string name;
        private float firingInterval;
        private float damage;

        public SpreadShot(string n, float fi, float d)
        {
            name = n;
            firingInterval = fi;
            damage = d;
        }

        public string GetName()
        {
            return name;
        }

        public float GetFiringInterval()
        {
            return firingInterval;
        }

        public void Fire()
        {
            GameObject o = GameObject.FindGameObjectWithTag("Player");
            GameObject b = Instantiate(o.GetComponent<PlayerScript>().bullet);
            b.transform.position = o.transform.position + new Vector3(0.1f, 0, 0.1f);
            b.GetComponent<BulletScript>().Go(o.GetComponent<PlayerScript>().rb.velocity, new Vector3(3, 0, 15));
            b = Instantiate(o.GetComponent<PlayerScript>().bullet);
            b.transform.position = o.transform.position + new Vector3(0, 0, 0.1f);
            b.GetComponent<BulletScript>().Go(o.GetComponent<PlayerScript>().rb.velocity, new Vector3(0, 0, 20));
            b = Instantiate(o.GetComponent<PlayerScript>().bullet);
            b.transform.position = o.transform.position + new Vector3(-0.1f, 0, 0.1f);
            b.GetComponent<BulletScript>().Go(o.GetComponent<PlayerScript>().rb.velocity, new Vector3(-3, 0, 15));
        }

        public void Reload()
        {

        }
    }
}