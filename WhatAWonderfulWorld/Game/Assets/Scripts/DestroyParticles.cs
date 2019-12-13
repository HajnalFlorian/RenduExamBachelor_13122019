using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BUT : Pour éviter que les particules restent sur la scene du projet et prennent de la ressource ! Eviter les lags et augmenter la performance
//ENTREE : La particule auquel est attaché le script
//SORTIE : Destruction de la scene
//NOTE : Utilisation d'un timer Coroutine-Wait

public class DestroyParticles : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Destruct());
    }

    IEnumerator Destruct()
    {

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}