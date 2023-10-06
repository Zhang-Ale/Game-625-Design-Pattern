using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : Observable
{
    public float _fireRate = 0;
    public Transform shootPosition;
    public GameObject _bulletPrefab;
    public float forceMultiplicator = 10;
    public GameObject pistol;
    public Animator pistolAnim; 
    void Update()
    {
        Vector3 mouseInWorldSpace = Camera.main.ScreenToWorldPoint
            (new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.y));
        Vector3 direction = mouseInWorldSpace - transform.position;

        float angle = Mathf.Acos(Vector3.Dot(Vector3.forward, direction.normalized)) * Mathf.Rad2Deg;
        pistol.transform.rotation = Quaternion.LookRotation(direction);
        if (Input.GetMouseButtonDown(0) && _fireRate < Time.time)
        {
            ShootBullet(direction);
            Notify(this.gameObject, Action.OnPlayerShoot);
            _fireRate = Time.time + 0.5f;
        }
    }

    void ShootBullet(Vector3 direction)
    {
        pistolAnim.SetTrigger("Shoot"); 
        GameObject bullet = Instantiate(_bulletPrefab, shootPosition.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = direction * forceMultiplicator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            Notify(this.gameObject, Action.OnPowerUpCollect);
            Destroy(other.gameObject);
        }
    }

    private void Awake()
    {
        IObserver gm = GameObject.FindFirstObjectByType<PlayerActions>();
        AddObserver(gm);
    }

    private void OnDisable()
    {
        IObserver gm = GameObject.FindFirstObjectByType<PlayerActions>();
        RemoveObserver(gm);
    }
}
