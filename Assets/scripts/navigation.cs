using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
static public class navigation
{
    static public void loadscene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
