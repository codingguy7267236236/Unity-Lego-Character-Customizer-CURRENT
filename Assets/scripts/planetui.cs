using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class planetui : MonoBehaviour
{
    GameObject planet;


    public void setPlanet(GameObject p)
    {
        planet = p;
    }

    public void land()
    {
        SceneManager.LoadScene(planet.GetComponent<planet>().planetname);
    }
}
