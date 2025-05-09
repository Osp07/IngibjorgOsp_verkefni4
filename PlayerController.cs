using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// notumst við InputSystem í unity
using UnityEngine.SceneManagement;
// notumst við SceneManagement í unity

public class PlayerController : MonoBehaviour
{
    AudioSource audioSource;
    // vísum í hljóðklippu

    public InputAction MoveAction;
    // breyta sem vísir í InputAction sem við tengjum við skriftuna í unity

    public InputAction talkAction;
    // breyta sem heldur utan um það þegar notandinn gerir eitthvað (stillum það í unity sem ef að ýtt er á x á lyklaborðinu)

    public GameObject projectilePrefab;
    // breyta sem skilgreinir skotið (skotið verður tengt við í unity)

    Rigidbody2D rigidbody2d;
    Vector2 move;
    // skilgreinum rigidbody á leikmanninum og hreyfimyndina

    public int maxHealth = 5;
    // breyta sem heldur utan hæsta gildi heilsunnar
    public int health { get { return currentHealth; }}
    // heilsan er sama gildi og núgildandi heilsa
    int currentHealth;
    // teljari sem heldur utan um hvað heilsan er í hvert skiptið

    public float speed = 3.0f;
    // breyta sem segir hraðan á hreyfingu

    Animator animator;
    // vísum í animator
    Vector2 moveDirection = new Vector2(1, 0);
    // breyta sem heldur utan um 
    
    // variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    // breyta fyrir hversu lengi leikmaður er að jafna sig eftir að missa líf
    bool isInvincible;
    // isInvincible er bool sem getur annaðhvort verið true eða false
    float damageCooldown;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        // virkjum MoveAction aðgerðina sem er sjálfgefið óvirk
        talkAction.Enable();
        // virkjum talkAction aðgerðina sem er sjálfgefið óvirk
        rigidbody2d = GetComponent<Rigidbody2D>();
        // sækjum rigidbody á leikmanninum
        currentHealth = maxHealth;
        // heilsan byrjar í hæsta mögulega gildinu

        animator = GetComponent<Animator>();
        // sækjum animator componentið á leikmanninn

        audioSource = GetComponent<AudioSource>();
        // sækjum audio source componentið á leikmanninn
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
        // færum leikmanninn eftir því hvað er verið að slá inn í lykjaborðið sem er lesið í gegnum InputAction objectið

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
            // ef að hreyfing í eina átt er ekki alveg 100% þá jöfnum við hana út
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        // setjum viðeigandi animation á hverja hreyfingu

        if (isInvincible)
        // ef að isInvincible er true
        {
            damageCooldown -= Time.deltaTime;
            // telur teljarinn niður um eina sekúndu
            if (damageCooldown < 0)
            {
                isInvincible = false;
                // alveg þangað til að teljarinn er orðinn lægri en 0, þá verðu isInvinsible false og teljarinn hættir að telja
            }
        }
          if(Input.GetKeyDown(KeyCode.C))
        {
           Launch();
           // ef að ýtt er á takkan c á lyklaborðinu er kallað á fallið launch sem skýtur skoti
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
           FindFriend();
           // ef að ýtt er á x á lyklaborðinu er kallað á fallið FindFriend() sem athugar hvort að leikmaður sé í nánd við vininn
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
        // færum leikmannin á þeim hraða sem skilgreindur er í sekúntum svo hann sé ekki að fara að breytast eftir upplausn
    }

    public void ChangeHealth (int amount)
    // fall sem breytir heilsunni
    {
        if (amount < 0)
        // ef að heilsan er í meira en 0
        {
            if (isInvincible)
            // ef að inInvincible er nú þegar í gangi
            {
                return;
                // förum við út úr fallinu
            }
            isInvincible = true;
            // setjum isInvincible í gang
            damageCooldown = timeInvincible;
            // teljaran sem telur niður meðan leikmaður er að jafna sig
            animator.SetTrigger("Hit");
            // leikmaður fylgir animationinu "Hit"
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // breytum current health en notum Clamp til að passa að heath getur ekki verið hærra en 5 og lægra en 0
        UIHandller.instance.SetHealthValue(currentHealth / (float)maxHealth);
        // birtum heilsuna á ui á skjánum
        if (currentHealth == 0)
        {
            SceneManager.LoadScene(3);
            // ef að current health fer í núll hleðst sena númer 3 og leikmaður tapar
        }
    }

    void Launch()
    {
        Debug.Log("skjotaaaa");
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        // notum Instantiate() til að setja inn skotið á þeim stað sem við viljum
        Prrojectile projectile = projectileObject.GetComponent<Prrojectile>();
        // tengjum skotið við Projectile skriftuna
        projectile.Launch(moveDirection, 300);
        // hreyfum skotið (því er skotið á miklum hraða)
        animator.SetTrigger("Launch");
        // setjum animation á karakterinn þegar hann skýtur
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,  moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        // notum raycast til að athuga hvort að leikmaður sé í ákveðinni fjarlægð og snýr að vininum, þá er hit virkt
        if (hit.collider != null)
        // ef að hit er virkt
        {
            NonPllayerCharacter character = hit.collider.GetComponent<NonPllayerCharacter>();
            // sækjum tilvísun í skriftuna sem tengd er við vinin
            if (EnnemyController.fixCount == 3)
            {
                SceneManager.LoadScene(2);
                // ef að leikmaður er búin að laga 3 andstæðinga og er í nánd við vininn hoppar hann yfir á lokasenu
            }
            else
            {
                if (character != null)
                {
                    UIHandller.instance.DisplayDialogue();
                    // ef að leikmaður er í nánd við vininn verða skilaboðin birt
                }
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        // þegar kallað er á fallið PlaySound() er hljóðklippan spiluð einu sinni
    }
}

