using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed;
    public float LifeTime;

    Rigidbody rb;
    float currentLifeTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * Speed;

        currentLifeTime += Time.deltaTime;
        if(currentLifeTime > LifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {

        }
        else if (other.tag == "Enemy")
        {

        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
