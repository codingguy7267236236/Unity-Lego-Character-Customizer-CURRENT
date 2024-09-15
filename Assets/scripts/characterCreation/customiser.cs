using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class customiser : MonoBehaviour
{
    [SerializeField] private GameObject player;
    //current part
    [SerializeField] private List<GameObject> selectedPart;
    //current selected mode like head texture, torso texture, etc..
    [SerializeField] private int mode = 0;

    //ui stuff
    [SerializeField] private GameObject items;
    [SerializeField] private GameObject hairsMenu;
    [SerializeField] private GameObject modelsMenu;
    //the loadcreate menu section
    [SerializeField] private GameObject loadcreateUI;
    [SerializeField] private InputField cnameInput;
    [SerializeField] private GameObject charactersSelection;
    [SerializeField] private List<GameObject> uimenus;

    //prefab
    [SerializeField] private Transform itemPrefab;
    [SerializeField] private Transform characterSelectionBtn;

    //models
    private Transform models;


    //pre load stuff and functions
    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    //setting up each category page thing
    private void setUp()
    {
        //grabbing all the materials that will fill each category
        List<List<Material>> materials = player.GetComponent<character>().materials;
        //looping through each category to fill in the items menu
        for (int itm = 0; itm < items.transform.childCount; itm++)
        {
            //loading all textures from specified folder
            string fldnam = items.transform.GetChild(itm).name;
            Debug.Log("Folder name:"+fldnam);
            Sprite[] imgs = Resources.LoadAll($"graphics/customiser/{fldnam}", typeof(Sprite)).Cast<Sprite>().ToArray();
            Debug.Log("Materials"+ imgs.Length.ToString());
            //getting content section
            Transform content = items.transform.GetChild(itm).GetChild(0).GetChild(0);
            //looping over all of materials in index itm
            for(int num=0; num<materials[itm].Count;num++)
            {
                Material material = materials[itm][num];
                //creating ui element
                GameObject item = Instantiate(itemPrefab, content).gameObject;
                item.transform.parent = content;
                item.name = $"Item {num}";
                //setting item ui data
                item.GetComponent<UIItem>().customiser = this;
                item.GetComponent<UIItem>().setData(itm, num);

                //setting the onclick acion
                item.GetComponent<Button>().onClick.AddListener(item.GetComponent<UIItem>().updateTexture);

                //getting and setting the ui image
                item.GetComponent<Button>().image.sprite = imgs[num];
            }
        }

        //for the hair menu section
        for(int itm=0; itm< player.GetComponent<character>().hairs.Count; itm++)
        {
            Transform content = hairsMenu.transform.GetChild(0).GetChild(0);
            GameObject item = Instantiate(itemPrefab, content).gameObject;
            item.transform.parent = content;
            item.name = $"Item {itm}";
            //setting item ui data
            item.GetComponent<UIItem>().customiser = this;
            item.GetComponent<UIItem>().setHair(itm);

            //setting the onclick acion
            item.GetComponent<Button>().onClick.AddListener(item.GetComponent<UIItem>().updateHair);

            //getting and setting ui item texture
        }

        // model menu
        for(int i=0; i<player.transform.GetChild(2).transform.childCount; i++)
        {
            Transform content = modelsMenu.transform.GetChild(0).GetChild(0);
            GameObject item = Instantiate(itemPrefab, content).gameObject;
            item.transform.parent = content;
            item.name = $"Item {i}";
            //setting item ui data
            item.GetComponent<UIItem>().customiser = this;
            item.GetComponent<UIItem>().setHair(i);

            //setting the onclick acion
            item.GetComponent<Button>().onClick.AddListener(item.GetComponent<UIItem>().updateModel);
        }

        //setting the input field bar
        cnameInput.text = player.GetComponent<character>().fname;
        //load all character section
        loadCharacters();
    }



    //in game stuff
    public void selectPart(int part)
    {
        mode = part;
        //checking this each time incase model has changed
        figure fig = player.GetComponent<character>().figure.GetComponent<figure>();
        List<List<GameObject>> partsCollection = fig.transformsData();
        //if the part is hair then we try get the first child element
        if (part == 0)
        {
            try
            {
                selectedPart = new List<GameObject>() { partsCollection[part][0].transform.GetChild(0).gameObject };
            }
            catch(Exception)
            {
                selectedPart = partsCollection[part];
            }
        }
        //if not hair peice select the index region
        else
        {
            //getting the part gameobject
            //because the figure clas doesn't deal with hats and hairs right now minus 1 will later remove
            selectedPart = partsCollection[part];
        }
    }

    public void SelectTextureCat(int category)
    {
        // setting other menus dictionary
        Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>()
        {
            {4, hairsMenu },
            {5, modelsMenu }
        };
        //if hairs selected open hair menu
        if (category >= 4)
        {

            foreach(KeyValuePair<int,GameObject> entry in dict)
            {
                if(category == entry.Key)
                {
                    entry.Value.SetActive(true);
                }
                else
                {
                    entry.Value.SetActive(false);
                }
            }
            items.SetActive(false);
        }
        //if not hairs menu
        else
        {
            items.SetActive(true);
            foreach (KeyValuePair<int, GameObject> entry in dict)
            {
                entry.Value.SetActive(false);
            }
            //looping through all the item screens for each category
            for (int i = 0; i < items.transform.childCount; i++)
            {
                //if this category is the same as the menu we need activate
                if (i == category)
                {
                    items.transform.GetChild(i).gameObject.SetActive(true);
                }
                //for the rest hide them
                else
                {
                    items.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }


    public void selectTexture(int cat, int item)
    {
        //calling player update part to update parts texture and save the data
        player.GetComponent<character>().updatePart(selectedPart, mode, cat, item);
    }

    public void selectHair(int hairpiece)
    {
        player.GetComponent<character>().updateHair(hairpiece);
        selectedPart = player.GetComponent<character>().figureparts[0];
    }


    //update the character name
    public void updateCharacterName()
    {
        player.GetComponent<character>().updateName(cnameInput.GetComponent<InputField>().text);
    }

    public void newCharacter()
    {
        player.GetComponent<character>().newCharacter();
        //setting the input field bar
        nameBoxUpdate();
    }

    //loading all characters created by user
    public void loadCharacters()
    {
        //going to the character saved folder to get all filenames
        string path = Application.dataPath + "/Save-Data/characters/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.json");

        //looping over each file to get just the filename without extension to use as
        //the name for the button to load pre-existing characters
        foreach (FileInfo f in info)
        {
            string nam = f.Name;
            string[] nam1 = nam.Split(' ', '.');
            string cname = nam1[0];
            //creating menu icon object
            GameObject btn = Instantiate(characterSelectionBtn, charactersSelection.transform).gameObject;
            btn.transform.parent = charactersSelection.transform;
            //setting the text element to character name
            btn.transform.GetChild(0).GetComponent<Text>().text = cname;
            //set the ui elements child
            btn.GetComponent<UIItem>().setCharacter(cname);
            //adding on click event function
            btn.GetComponent<Button>().onClick.AddListener(btn.GetComponent<UIItem>().loadCharacter);
            btn.GetComponent<UIItem>().customiser = this;
        }
    }

    public void loadCharacter(string nam)
    {
        player.GetComponent<character>().setCharacter(nam);
        nameBoxUpdate();
        changeMenu(0);
    }

    public void nameBoxUpdate()
    {
        cnameInput.text = player.GetComponent<character>().fname;
    }

    public void ChangeModel(int model_index)
    {
        player.GetComponent<character>().UpdateModel(model_index);
    }


    //change menu stuff
    public void changeMenu(int men)
    {
        for(int i=0; i < uimenus.Count; i++)
        {
            if (i == men)
            {
                uimenus[i].SetActive(true);
            }
            else
            {
                uimenus[i].SetActive(false);
            }
        }
    }
}
