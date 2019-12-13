using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrapperCoffre : MonoBehaviour
{

	// -----------------------------------------------------------
	// Déclaration des items que contient le coffre
	public GameObject[] items;
	public GameObject ptcItems;

	int nbItems = 0;
	int cptItems = 0;
    [SerializeField]
    bool isOuvert = false;

    DeplacerSlime detection;
    bool contact = false;
    // -----------------------------------------------------------



    void Awake()
	{
		nbItems = items.Length;
        detection = GameObject.Find("Blob").GetComponent<DeplacerSlime>();
	}

    // Pour tester la fonction de l'explosion de coffre, appuyez sur espace
    void Update()
    {
        if (detection.frapper == true && contact == true && isOuvert == false)
        {
          	CoffreExplose();
            GetComponent<AudioSource>().Play(0);
        }
    }


    // Fonction permettant d'exploser le coffre
    public void CoffreExplose()
    {
        isOuvert = true;

    	// On affecte le couvercle à une variable du script
    	GameObject couvercle;
    	couvercle = this.gameObject.transform.GetChild(0).gameObject;

    	// On supprime les effets de particules
    	Destroy(this.gameObject.transform.GetChild(1).gameObject);

    	// Le couvercle se détache du parent
    	couvercle.transform.SetParent(GameObject.Find("Canvas").transform);
    	// On set la gravité du couvercle
    	couvercle.GetComponent<Rigidbody2D>().gravityScale = 10;
    	// On lui applique des forces de levée et de rotation
    	couvercle.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 5000.0f), ForceMode2D.Impulse);
    	couvercle.GetComponent<Rigidbody2D>().AddTorque(1, ForceMode2D.Impulse);

    	// Après un court instance, on détruit le couvercle = garbage collector
    	StartCoroutine(DetruireCouvercle(couvercle));

    	//Activation des particules d'items
    	GameObject instancePtcItems;
        instancePtcItems = Instantiate(ptcItems, transform.position, transform.rotation);
    	//Activation du lancé d'item
    	StartCoroutine(LancerItem(instancePtcItems));
    }


    // Fonction Timer qui détruira le couvercle
    IEnumerator DetruireCouvercle(GameObject couvercle)
    {
    	yield return new WaitForSeconds(5);

    	Destroy(couvercle);
    }


    // Fonction Timer permettant de lancer un item
    IEnumerator LancerItem(GameObject instancePtcItems)
    {
    	yield return new WaitForSeconds((float)0.5);

    	// Instanciation de l'item
    	GameObject instanceItem;
        instanceItem = Instantiate(items[cptItems], transform.position, transform.rotation);

        // Ajout d'une force à l'item
        float forceX = 0;
        forceX = Random.Range(-3000f, 3000f);
        instanceItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, 3000.0f), ForceMode2D.Impulse);

        cptItems++;

        // Lance tous les items que contient le coffre
        if(cptItems < items.Length)
        {
        	StartCoroutine(LancerItem(instancePtcItems));
        }
        else
        {
        	Destroy(instancePtcItems);
        }	
    }

    // Regarde si le personnage est devant un coffre
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            contact = true;
        }
    }

    // Regarde si le personnage n'est plus devant le coffre
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            contact = false;
        }
    }
}
