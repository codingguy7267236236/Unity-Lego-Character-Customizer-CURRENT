using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class figure:MonoBehaviour
{
    [SerializeField] private List<GameObject> hair;
    [SerializeField] private List<GameObject> head;
    [SerializeField] private List<GameObject> torso;
    [SerializeField] private List<GameObject> arms;
    [SerializeField] private List<GameObject> hands;
    [SerializeField] private List<GameObject> legs;

    public List<List<GameObject>> transformsData()
    {
        List<List<GameObject>> data = new List<List<GameObject>>() { hair, head, torso, arms, hands, legs };
        return data;
    }
}
