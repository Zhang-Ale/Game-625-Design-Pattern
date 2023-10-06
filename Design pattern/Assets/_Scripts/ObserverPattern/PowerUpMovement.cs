using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class PowerUpMovement : Observable
{
    private GameObject _destination;
    private GameObject[] _destinations;
    private GameObject enemy;
    public float smellSense = 5;
    public GameObject part;
    [SerializeField]bool isSeeking;

    private void SetNextDestination()
    {
        int index = Random.Range(0, _destinations.Length);
        _destination = _destinations[index];
        agent.destination = _destination.transform.position;
    }
    void Start()
    {
        SetUp();
        _destinations = GameObject.FindGameObjectsWithTag("Destination");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    void Update()
    {
        if (menu.gameStarted)
        {
            SetNextDestination();

            var distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToTarget < smellSense)
            {
                agent.destination = enemy.transform.position;
                moveSpeed = 40;
                isSeeking = true;
            }
            else
            {
                isSeeking = false;
                moveSpeed = 30;
                var distanceToDestination = Vector3.Distance(transform.position, _destination.transform.position);

                if (distanceToDestination < .5f)
                {
                    SetNextDestination();
                }
            }
        }

        if (GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            InstantiateParticle(part); 
        }
    }
}
