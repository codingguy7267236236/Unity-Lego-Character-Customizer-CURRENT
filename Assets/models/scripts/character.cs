using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class character : MonoBehaviour
{
    public GameObject figure;
    public List<Transform> hairs;
    public List<Material> colours;
    public List<Material> faces;
    public List<Material> torsos;
    public List<Material> legs;

    public Transform models;

    //more easier understood format for customiser system
    public List<List<Material>> materials = new List<List<Material>>();
    public List<List<GameObject>> figureparts;

    //player character data object
    [SerializeField] private saveCharacter figData;
    public string fname;

    // Start is called before the first frame update
    void Start()
    {
        //doing all the load stuff
        materials = new List<List<Material>>() { colours, faces, torsos, legs };
        models = transform.GetChild(2).transform;
        //loading the materials and hairs to do with character customising
        loadResources();
        //loading figure data
        loadCharacter();
    }


    void loadResources()
    {
        List<string> mFldrs = new List<string>() { "colours","faces","torsos","legs"};
        //looping over each folder and appending materials to the correct list
        for(int i=0; i < mFldrs.Count; i++)
        {
            //loading materials from folder
            Material[] mats = Resources.LoadAll($"materials/{mFldrs[i]}",typeof(Material)).Cast<Material>().ToArray();
            //looping through each material and appending to the list
            foreach(Material ob in mats)
            {
                materials[i].Add(ob);
            }
        }

        //loading the the hairs
        hairs = Resources.LoadAll("hairs", typeof(Transform)).Cast<Transform>().ToList();
    }



    //editing character model stuff
    public void setCharacter(string nam)
    {
        fname = nam;
        loadCharacter();
    }
    //load character function to be used
    void loadCharacter()
    {
        figData = savingCharacter.LoadData(fname);
        //figData.viewData();
        MakeCharacter();
    }
    
    //blanking character to default no abilities no textures, etc.
    public void newCharacter()
    {
        figData.Init();
        fname = figData.name;
        MakeCharacter();
    }

    //loading character materials and constructing figure
    void MakeCharacter()
    {
        // getting the figure model type and deactivating the other figure models
        for(int i=0; i<models.transform.childCount; i++)
        {
            GameObject fig = models.GetChild(i).gameObject;
            if (i == figData.type)
            {
                fig.SetActive(true);
                figure = fig;
                figureparts = figure.GetComponent<figure>().transformsData();
            }
            else
            {
                fig.SetActive(false);
            }
        }

        List<List<int>> parts = figData.ConvertForCustomiser();
        //figure transform data
        figureparts = figure.GetComponent<figure>().transformsData();

        //loop over each part and supply the correct info
        for(int i=0; i<figureparts.Count;i++)
        {
            List<GameObject> figpart = figureparts[i];
            List<int> data = parts[i];
            //material
            Material mat = materials[data[0]][data[1]];
            //if i=0 then hair piece selection
            if (i == 0)
            {
                //clearing any hair in the hair parent element
                foreach(Transform child in figpart[0].transform)
                {
                    Destroy(child.gameObject);
                }
                //instatiating the hair piece
                GameObject hairr = Instantiate(hairs[figData.hair].gameObject, figpart[0].transform);
                hairr.transform.parent = figpart[0].transform;
                hairr.transform.localScale = new Vector3(1, 1, 1);
                figpart = new List<GameObject>() { hairr};
            }
            //anything else do the usual

            //now loop over all the selected parts in figpart to apply the material
            texturePart(figpart, mat);
        }
    }

    public void UpdateModel(int index)
    {
        figData.type = index;
        MakeCharacter();
        SaveCharacter();
    }

    public void texturePart(List<GameObject> figpart, Material mat)
    {
        foreach (GameObject p in figpart)
        {
            p.GetComponent<MeshRenderer>().material = mat;
        }
    }


    public void updatePart(List<GameObject> curPart, int partInd, int cat, int item)
    {
        //Debug.Log($"Mode: {cat}   Item: {item}");
        //getting the specified material
        Material m = materials[cat][item];
        //grab the parts to modify texture
        texturePart(curPart, m);
        //updating the stored data to be sent to json
        savePart(partInd,cat,item);
        SaveCharacter();
    }

    public void updateHair(int hairpiece)
    {
        figData.hair = hairpiece;
        MakeCharacter();
        SaveCharacter();
    }


    public void updateName(string nam)
    {
        fname = nam;
        figData.name = fname;
    }



    //updating the saveCharacter class that is used for storing json
    private void SaveCharacter()
    {
        savingCharacter.SaveData(figData);
    }

    private void savePart(int part, int cat, int itm)
    {
        if (part == 0)
        {
            figData.hairCol = new List<int>() { cat, itm };
        }
        else if (part == 1)
        {
            figData.head = new List<int>() { cat, itm };
        }
        else if (part == 2)
        {
            figData.torso = new List<int>() { cat, itm };
        }
        else if (part == 3)
        {
            figData.arms = new List<int>() { cat, itm };
        }
        else if (part == 4)
        {
            figData.hands = new List<int>() { cat, itm };
        }
        else if (part == 5)
        {
            figData.legs = new List<int>() { cat, itm };
        }
    }

}
