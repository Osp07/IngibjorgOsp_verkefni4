using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZonne : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    // ef að leikmaður kemur við damage zone
    {
       PlayerController controller = other.GetComponent<PlayerController>();
       // sækjum tilvísun í leikmanna skrifurna

       if (controller != null)
       // ef að tilvísun í leikmann er sótt
       {
           controller.ChangeHealth(-1);
           // leikmaður missir eitt stig, notum ChangeHealth() fallið
       }
    }
}
