using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Observable
{
    int health = 3;
    public int currentHealth;
    AudioSource AS;
    public Material whiteMat;
    MeshRenderer rend;
    Material ogMat;
    public bool notified; 
    void Start()
    {
        currentHealth = health;
        rend = GetComponent<MeshRenderer>();
        AS = GetComponent<AudioSource>();
        menu = GameObject.FindObjectOfType<Menu>();
        ogMat = rend.material;
    }

    public void Update()
    {
        if(currentHealth == 0 && !notified)
        {
            Notify(this.gameObject, Action.OnEnemyDestroy);
            notified = true; 
            menu.AddPoint();
            if(notified)
            Destroy(this.gameObject, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            FindObjectOfType<HitStop>().Stop(0.075f);
            StartCoroutine("TakeDamage");
            currentHealth -= 1;
        }
    }

    IEnumerator TakeDamage()
    {
        while (Time.timeScale != 1)
        {
            yield return null;
        }
        AS.Play();
        rend.material = whiteMat;
        yield return new WaitForSeconds(0.1f);
        rend.material = ogMat; 
    }

    private void Awake()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        AddObserver(gm); 
    }

    private void OnDisable()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        RemoveObserver(gm);
    }
}
