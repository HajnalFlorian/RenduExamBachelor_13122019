using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BUT : Gère les déplacements des ennemis
public class EnnemiBehaviour : MonoBehaviour
{
	// -------------------------------------
	public float delai = 0.0f;
	bool gauche = true;
	// -------------------------------------

  
	void Start()
	{
		StartCoroutine(ChangerDirection());
	}


    void Update()
    {
    	if(gauche)
    	{
    		transform.Translate(-Vector3.right * 30 * Time.deltaTime);
    	}
    	else
    	{
    		transform.Translate(Vector3.right * 30 * Time.deltaTime);
    	}
    }

    // Change la direction de l'ennemi après un certains temps
    IEnumerator ChangerDirection()
    {
    	yield return new WaitForSeconds(delai);

    	if(gauche)
    	{
    		gauche = false;
    	}
    	else
    	{
    		gauche = true;
    	}

    	StartCoroutine(ChangerDirection());
    }
}
