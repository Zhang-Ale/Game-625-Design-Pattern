using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Player
{
    public class GenerateBullet : MonoBehaviour
    {
        //public int numberOfBullets = 50;
        public float radius = 1.0f;
        public BulletPool bulletPool;
        public Transform shootPosition;
        public GameObject _bulletPrefab;

        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                FireBurst();

            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                for (int i = 0; i < 20; i++)
                {
                    bulletPool.ReturnBullet();
                }
            }
        }

        void SingleShot()
        {
            GameObject newBullet = bulletPool.GetBullet();
        }
        void FireBurst()
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject newBullet = bulletPool.GetBullet();
                newBullet.transform.position = shootPosition.transform.position;
                Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
                float angleIncrement = 360f / Random.Range(-90, 90);

                if (angleIncrement != 0) //to make sure angle is never i*0
                {
                    float angle = i * angleIncrement;
                    Vector3 spawnPosition = shootPosition.transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
                    Vector3 bulletDirection = (spawnPosition - shootPosition.transform.position).normalized;
                    bulletRigidbody.AddForce(bulletDirection * 10f, ForceMode.VelocityChange);
                }
            }
            return;
        }
    }
}
