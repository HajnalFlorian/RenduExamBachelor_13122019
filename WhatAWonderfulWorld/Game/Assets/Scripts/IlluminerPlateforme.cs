using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class IlluminerPlateforme : MonoBehaviour
{
	GameObject light;

    // Set l'intensité de toutes les plateformes à 0 au début du niveau
    void Awake()
    {
        light = this.gameObject.transform.GetChild(0).gameObject;
        light.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0f;
    }

    // Quand le joueur marche sur une plateforme, elle s'illumine
    // LE PERSONNAGE DOIT AVOIR LE TAG "PLAYER" !!!! C'est fait
    void OnCollisionEnter2D(Collision2D other) 
    {
       	if (other.gameObject.tag == "player")
        {
        	light.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0.7f;
        }
    }
}
