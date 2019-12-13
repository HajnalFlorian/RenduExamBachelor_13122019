using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public GameObject Jouer;
    public GameObject Instruction;
    public GameObject Grinchistes;
    public GameObject Quitter;
    public GameObject Retour;
    public GameObject Text_Instructions;
    public GameObject Text_Grinchistes;
    public GameObject Les_Grinchistes;
    int Menu = 0;

    // Start is called before the first frame update
    void Start()
    {
        Jouer.SetActive(true);
        Instruction.SetActive(true);
        Grinchistes.SetActive(true);
        Quitter.SetActive(true);
        Retour.SetActive(false);
        Text_Instructions.SetActive(false);
        Text_Grinchistes.SetActive(false);
        Les_Grinchistes.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitMenu()
    {
        Jouer.SetActive(false);
        Instruction.SetActive(false);
        Grinchistes.SetActive(false);
        Quitter.SetActive(false);
    }

    public void EnterMenu()
    {
        Jouer.SetActive(true);
        Instruction.SetActive(true);
        Grinchistes.SetActive(true);
        Quitter.SetActive(true);
    }

    public void ReturnMenu()
    {
        Retour.SetActive(false);
        if(Menu == 1)
        {
            Text_Instructions.SetActive(false);
        }
        else if(Menu == 2)
        {
            Text_Grinchistes.SetActive(false);
            Les_Grinchistes.SetActive(false);
        }
        EnterMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Calibrage");
    }

    public void InstrctionLoad()
    {
        Menu = 1;
        ExitMenu();
        Retour.SetActive(true);
        Text_Instructions.SetActive(true);
    }

    public void GrinchisteLoad()
    {
        Menu = 2;
        ExitMenu();
        Retour.SetActive(true);
        Text_Grinchistes.SetActive(true);
        Les_Grinchistes.SetActive(true);
    }

    public void QuitProject()
    {
        Application.Quit();
        Debug.Break();
    }
}
