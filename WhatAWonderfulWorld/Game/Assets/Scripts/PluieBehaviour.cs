using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluieBehaviour : MonoBehaviour
{
	public GameObject ploc;

    //Quand collision avec le sol
    void OnTriggerEnter2D(Collider2D other) 
    {
       	if (other.gameObject.CompareTag("plateforme"))
        {
            // Spawn une particule de pluie
        	GameObject instancePloc;
        	instancePloc = Instantiate(ploc, transform.position, transform.rotation);

            // Détruit l'instance de pluie
        	Destroy(gameObject);
        }
    }
}
