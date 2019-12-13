using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SpawnerPluieBehaviour : MonoBehaviour
{
	public Transform personnage;
	public GameObject pluie;
	public GameObject eclair;
	public GameObject aura;

	void Start()
	{
		StartCoroutine(SpawnPluie());
		StartCoroutine(SpawnEclair());
	}

    // Le spawner suit le joueur
    void Update()
    {
        transform.position = new Vector2(personnage.position.x + 175,personnage.position.y + 400);
    }

    // Spawn une goutte de pluie toutes les 0.2 secondes
    IEnumerator SpawnPluie()
    {	
        // La pluie spawn aléatoirement depuis le spawner
    	int posX = 0;
    	posX = Random.Range(-415, 415);

        // Instanciation de la pluie
    	GameObject instancePluie;
        instancePluie = Instantiate(pluie, new Vector2(transform.position.x + posX, transform.position.y), transform.rotation);

        yield return new WaitForSeconds((float)0.2);

        // Répétition du timer
        StartCoroutine(SpawnPluie());
    }

    // Spawn de l'éclair
    IEnumerator SpawnEclair()
    {	
        // Donne une valeur aléatoire à la position de l'orage
    	int posX = 0;
    	posX = Random.Range(-415, 415);
    	float delai = 0f;
    	delai = Random.Range(4f, 7f);

    	yield return new WaitForSeconds(delai);

        // Joue un son de tonnerre
    	GetComponent<AudioSource>().Play(0);
    	GameObject instanceEclair;
        instanceEclair = Instantiate(eclair, new Vector2(transform.position.x + posX, transform.position.y - 263), transform.rotation);

        // Fait spawn l'aura
        GameObject instanceAura;
        instanceAura = Instantiate(aura, new Vector2(transform.position.x + posX, transform.position.y - 263), transform.rotation);

        yield return new WaitForSeconds((float)0.04);

        Destroy(instanceEclair);

        yield return new WaitForSeconds((float)0.04);

        GameObject instanceEclair2;
        instanceEclair2 = Instantiate(eclair, new Vector2(transform.position.x + posX, transform.position.y - 263), transform.rotation);

        yield return new WaitForSeconds((float)0.04);

        Destroy(instanceEclair2);

        yield return new WaitForSeconds((float)0.3);

       	StartCoroutine(LessAura(instanceAura));

        StartCoroutine(SpawnEclair());
    }

    // Diminution progressive de l'intensité de l'aura
    IEnumerator LessAura(GameObject instanceAura)
    {
    	instanceAura.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity -= (float)0.05;

    	yield return new WaitForSeconds((float)0.09);

    	if(instanceAura.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity <= 0)
    		Destroy(instanceAura);
    	else
    		StartCoroutine(LessAura(instanceAura));
    }
}
