using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    [SerializeField] private int category=0;
    [SerializeField] private int index=0;
    [SerializeField] private int hair = 0;
    public customiser customiser;

    //character selction details
    [SerializeField] private string character;

    public void setData(int cat, int ind)
    {
        index = ind;
        category = cat;
    }

    public void setHair(int hirpeice)
    {
        hair = hirpeice;
    }

    public void updateTexture()
    {
        customiser.selectTexture(category,index);
    }

    public void updateHair()
    {
        customiser.selectHair(hair);
    }

    public void updateModel()
    {
        customiser.ChangeModel(hair);
    }

    public void setCharacter(string nam)
    {
        character = nam;
    }

    public void loadCharacter()
    {
        customiser.loadCharacter(character);
    }
}
