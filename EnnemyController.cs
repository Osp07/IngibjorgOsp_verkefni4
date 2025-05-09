using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    public static int fixCount = 0;
    // bý til teljara sem heldur utan um hversu marga andstæðinga er búið að laga

    public ParticleSystem smokeEffect;
    // breyta sem sækir tilvísun í reyk effect

    AudioSource audioSource;
    // breyta sem skilgreinir hljóðbút

    public bool vertical;
    // breyta sem heldur utan um hvort að hreyfing andstæðingsins er lóðrétt eða ekki

    bool broken = true;
    // breyta sem skilgreinir hvort andstæðingurinn sé bilaður eða ekki (hann byrjar á því að vera bilaður)

    public float speed;
    // breyta sem heldur utan um hraða andstæðingsins
    Rigidbody2D rigidbody2d;
    // skilgreinum rigidbody componentið fyrir andstæðinginn

    public float changeTime = 3.0f;
    // breyta sem heldur utan um hversu lengi andstæðingur labbar í ákveðna átt
    float timer;
    // breyta sem heldur utan um hve lengi
    int direction = 1;
    // breyta sem segir andstæðingi að fara einn áfram, hvaða átt sem það væri
    Animator animator;
    // skilgreynum animator component

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // sækjum tilvísun í hljóðbútinn

        rigidbody2d = GetComponent<Rigidbody2D>();
        // sækjum tilvísun í rigidbody2d componentið

        animator = GetComponent<Animator>();
        // sækjum tilvísun í animator componentið

        timer = changeTime;
        // ensdurstillum teljaran

        fixCount = 0;
        // núllstillum teljarann sem telur hve mörg vélmenni er búið að laga
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        // á hverri sekúndu telur teljarinn niður
        if (timer < 0)
        {
            direction = -direction;
            // ef að teljarinn er orðin neðar en núll, gerum hann pósitífan
            timer = changeTime;
            // endurstillum hann í 3 sekúndur
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
        // staðsettning andstæðings fer eftir hvar rigidbody staðsettning er
        if (vertical)
        // ef að hakað er við vertical
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            // færum andstæðingin áfram eftir y ásnum á þeim hraða sem við settum fyrir
            animator.SetFloat("Move X", 0);
            // setjum animation á hreyfinguna, sem fer ekkert áfram af því að við erum að fara meðfram y ásnum
            animator.SetFloat("Move Y", direction);
            // setjum animation á hreyfinguna
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            // færum andstæðinginn áfram eftir x ásnum á þeim hraða sem við settum fyrir
            animator.SetFloat("Move X", direction);
            // setjum animation á hreyfinguna
            animator.SetFloat("Move Y", 0);
            // setjum animation á hreyfinguna, sem fer ekkert áfram af því að við erum að fara meðfram x ásnum
        } 
        rigidbody2d.MovePosition(position);
        // færum rigidbody og position
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
            // ef að leikmaður kemur við andstæðing missir hann eitt líf
        }
    }

    public void Fix()
   {
       animator.SetTrigger("Fixed");
       // setjum animation á andstæðinginn þegar búið er að laga hann
       broken = false;
       rigidbody2d.simulated = false;
       // ef að kallað er á Fix() fallið hættir andstæðingurinn að vera bilaður og slökt verður á rigidbody eiginleikunum hans
       audioSource.Stop();
       // hljóðbúturinn hættir að spila
       smokeEffect.Stop();
       // reykurinn hættir
       fixCount += 1;
       // teljum með teljaranum að búið er að laga einn andstæðing
   }
}
