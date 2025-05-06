using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Prrojectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(transform.position.magnitude > 100.0f)
       {
           Destroy(gameObject);
           // ef að skotið er komið langt út af fyrir heimin er því eytt
       }
    }

    public void Launch (Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        EnnemyController enemy = other.GetComponent<EnnemyController>();
        // Tengjum EnemyController skriftuna við hlutinn sem við vorum að skjóta í
        if (enemy != null)
        // ef að andstæðingurinn er tengdur við EnnemyController skriftuna
        {
            enemy.Fix();
            // lögum við andstæðinginn með því að kalla á Fix() fallið í EnemyController skriftunni
        }
        Destroy(gameObject);
        // eyðum skotinu
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        // ef að skotið kemur við eitthvað annað en andstæðing er því líka eytt
    }
}
