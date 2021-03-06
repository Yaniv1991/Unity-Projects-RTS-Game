﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float LifeSpan;
    public float speed;
    public int Damage;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        LifeSpan -= Time.deltaTime;
        if (LifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.GetComponent<EnemyUnit>().TakeDamage(Damage);
        Destroy(gameObject);
    }
}
