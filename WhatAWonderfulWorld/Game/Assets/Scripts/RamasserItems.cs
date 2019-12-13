using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RamasserItems : MonoBehaviour
{
	// Déclaration des variables
	// --------------------------
	int cpt = 0;
	public GameObject score;
	public GameObject ptc;
	// --------------------------

    // Quand le personnage rentre en colision avec un item -> set le score, provoque une particule et joue un son
    void OnCollisionEnter2D(Collision2D other) 
    {
       	if (other.gameObject.CompareTag("coin"))
        {
        	cpt += 1;

        	StartCoroutine(AddScore());
        	Destroy(other.gameObject);

        	GameObject instancePtc;
        	instancePtc = Instantiate(ptc, transform.position, transform.rotation);
        	GetComponent<AudioSource>().Play(0);
        }
        else if (other.gameObject.CompareTag("baguette"))
        {
        	cpt += 10;

        	StartCoroutine(AddScore());
        	Destroy(other.gameObject);

        	GameObject instancePtc;
        	instancePtc = Instantiate(ptc, transform.position, transform.rotation);
        	GetComponent<AudioSource>().Play(0);
        }
        else if (other.gameObject.CompareTag("lingot"))
        {
        	cpt += 20;

        	StartCoroutine(AddScore());
        	Destroy(other.gameObject);

        	GameObject instancePtc;
        	instancePtc = Instantiate(ptc, transform.position, transform.rotation);
        	GetComponent<AudioSource>().Play(0);
        }
        else if (other.gameObject.CompareTag("orange"))
        {
        	cpt += 50;

        	StartCoroutine(AddScore());
        	Destroy(other.gameObject);

        	GameObject instancePtc;
        	instancePtc = Instantiate(ptc, transform.position, transform.rotation);
        	GetComponent<AudioSource>().Play(0);
        }

    }


    // Le score est incrémenté tous les 1 point pour donner une fluidité
    IEnumerator AddScore()
    {
    	score.GetComponent<Text>().text = (int.Parse(score.GetComponent<Text>().text) + 1).ToString();

    	yield return new WaitForSeconds((float)0.1);

    	cpt--;

    	if(cpt >= 0)
    	{
    		StartCoroutine(AddScore());
    	}
    	else
    	{
    		cpt = 0;
    	}
    }
}
