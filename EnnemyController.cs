using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    public bool vertical;

    bool broken = true;
    // breyta sem skilgreinir hvort andstæðingurinn sé bilaður eða ekki (hann byrjar á því að vera bilaður)

    public float speed;
    Rigidbody2D rigidbody2d;

    public float changeTime = 3.0f;
    float timer;
    int direction = 1;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        // ef að andstæðingurinn er ekki bilaður
        {
            return;
            // förum við út úr FixedUpdate() fallinu, annars er farið í gegnum restina af kóðanum
        }

        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;

            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;

            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        } 
        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
   {
       broken = false;
       rigidbody2d.simulated = false;
       // ef að kallað er á Fix() fallið hættir andstæðingurinn að vera bilaður og slökt verður á rigidbody eiginleikunum hans
       animator.SetTrigger("Fixed");
       // setjum animation á andstæðinginn þegar búið er að laga hann
   }
}
