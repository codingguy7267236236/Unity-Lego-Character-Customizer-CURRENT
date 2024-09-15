using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))]
public class skyboxset : MonoBehaviour
{
    [SerializeField] List<Material> _skymaterial;

    Skybox _skybox;

    private void Awake()
    {
        _skybox = GetComponent<Skybox>();
    }

    private void OnEnable()
    {
        ChangeSkybox(0);
    }

    void ChangeSkybox(int skyBox)
    {
        if(_skybox!=null && skyBox>=0 && skyBox <= _skymaterial.Count)
        {
            _skybox.material = _skymaterial[skyBox];
        }
    }
}
