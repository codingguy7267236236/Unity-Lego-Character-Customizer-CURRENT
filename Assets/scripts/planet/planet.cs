using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class planet : MonoBehaviour
{
    private BoxCollider bc;
    public string planetname;

    [SerializeField] private GameObject ui;
    [SerializeField] private planetui canvas;

    private void Start()
    {
        ui = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        bc = GetComponent<BoxCollider>();
        canvas = GameObject.Find("Canvas").GetComponent<planetui>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            updatePlanetInfoUI();
            openUI();
            canvas.setPlanet(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        closeUI();
    }

    private void closeUI()
    {
        ui.SetActive(false);
    }

    private void openUI()
    {
        ui.SetActive(true);
    }

    private void updatePlanetInfoUI()
    {
        Text text = ui.transform.GetChild(0).GetComponent<Text>();
        text.text = planetname;
    }
}
