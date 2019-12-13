using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// BUT : Script permettant de stocker les variables du character ainsi que de savoir ses inputs

public class DeplacerSlime : MonoBehaviour
{
    public bool auSol;
    public bool frapper = false;
    public Sprite On;
    public Sprite Off;
    public GameObject flecheDroite;
    public GameObject flecheGauche;
    public GameObject victoire;
    public GameObject defaite;

    public int deplacement = 0;

    Image MonImageGauche;
    Image MonImageDroite;

    public bool isDead = false;

    void Awake()
    {
        MonImageGauche = flecheGauche.GetComponent<Image>();
        MonImageDroite = flecheDroite.GetComponent<Image>();
    }

    // Modification des UI : fléches de déplacement
    void Update()
    {
        if (deplacement == -1)
        {
            MonImageGauche.overrideSprite = On;
        }
        else if(deplacement == 0)
        {
            MonImageDroite.overrideSprite = Off;
            MonImageGauche.overrideSprite = Off;
        }
        else if(deplacement == 1)
        {
            MonImageDroite.overrideSprite = On;
        }
    }

    // Savoir si le personnage touche une plateforme
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "plateforme")
        {
            auSol = true;
        }
    }

    // Savoir si le personnage ne touche plus une plateforme
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "plateforme")
        {
            auSol = false;
        }
    }

    // Disabled les inputs du personnage en cas de condition de win ou de défaite
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("ennemi"))
        {
            isDead = true;
            defaite.transform.position = new Vector2(transform.position.x + 200, transform.position.y + 90);
            StartCoroutine(GrowDefaite());
        }
        else if (other.gameObject.CompareTag("win"))
        {
            isDead = true;
            victoire.transform.position = new Vector2(transform.position.x + 200, transform.position.y + 90);
            StartCoroutine(GrowVictoire());
        }
    }

    // Fait grandir le cadre de défaite
    IEnumerator GrowDefaite()
    {
        if(defaite.transform.localScale.x < 300)
        {
            defaite.transform.localScale = new Vector2(defaite.transform.localScale.x + 1f, defaite.transform.localScale.y + 1f);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(GrowDefaite());
        }
    }

    // Fait grandir le cadre de victoire
    IEnumerator GrowVictoire()
    {
        if(victoire.transform.localScale.x < 300)
        {
            victoire.transform.localScale = new Vector2(victoire.transform.localScale.x + 1f, victoire.transform.localScale.y + 1f);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(GrowVictoire());
        }
    }
}
