using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BUT : Animer les aliments présents dans le level, leur donnant une rotation fluide
public class RotationAliment : MonoBehaviour
{
	int cpt = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PencherGauche());
    }


    // Fait pencher l'objet vers la gauche
    IEnumerator PencherGauche()
    {
    	yield return new WaitForSeconds((float)0.05);

    	transform.Rotate(0.0f, 0.0f, transform.rotation.z + 1, Space.World);

    	if(cpt == 25)
    	{
    		StartCoroutine(PencherDroite());
    		cpt = 0;
    	}
    	else
    	{
    		StartCoroutine(PencherGauche());
    		cpt++;
    	}
    }


    // Fait pencher l'objet vers la droite
    IEnumerator PencherDroite()
    {
    	yield return new WaitForSeconds((float)0.05);
    	
    	transform.Rotate(0.0f, 0.0f, transform.rotation.z - 1, Space.World);

    	if(cpt == 25)
    	{
    		StartCoroutine(PencherGauche());
    		cpt = 0;
    	}
    	else
    	{
    		StartCoroutine(PencherDroite());
    		cpt++;
    	}
    }

}
