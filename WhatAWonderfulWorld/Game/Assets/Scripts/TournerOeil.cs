using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BUT : Faire tourner les yeux du personnage
public class TournerOeil : MonoBehaviour
{
	//Déclaration des variables
	//=================================
	public float speed;


    void Update()
    {
        transform.Rotate(0, 0, speed, Space.Self);
    }
}
