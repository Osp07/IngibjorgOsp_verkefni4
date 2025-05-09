using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    public AudioClip collectedClip;
    // breyta sem geymir tilvísun í hljóðbút

    void OnTriggerEnter2D (Collider2D other)
    // ef að leikmaður kemur við karamelluna
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        // sækjum tilvísun í skriftuna af leikmanni
        

        if (controller != null && controller.health < controller.maxHealth)
        // ef að tilvísun í leikmann er sótt og leikmaður er undir hæsta lífs gildi
        {
            controller.PlaySound(collectedClip);
            // spilum hljóðbútin
            // hljóðbúturinn er spilaður í gegnum leikmannin vegna þess að hlutnum verður eytt
            controller.ChangeHealth(1);
            // köllum á fallið sem breytir heilsunni (hækkar um 1)
            Destroy(gameObject);
            // eyðum karamellunni
        }
    }
}
