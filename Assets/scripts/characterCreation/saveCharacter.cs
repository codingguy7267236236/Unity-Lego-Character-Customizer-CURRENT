using System.Collections.Generic;
using UnityEngine;

public class saveCharacter
{
    public string name;
    public int type;

    //the accessories for the figure
    public int hair;
    //the materials the figure uses
    public List<int> hairCol;
    public List<int> head;
    public List<int> torso;
    public List<int> arms;
    public List<int> hands;
    public List<int> legs;

    public void Init()
    {
        name = null;
        type = 0;
        hair = 0;
        hairCol = new List<int>() { 0, 0 };
        head = new List<int>() { 0,0};
        torso = new List<int>() { 0, 0 };
        arms = new List<int>() { 0, 0 };
        hands = new List<int>() { 0, 0 };
        legs = new List<int>() { 0, 0 };
    }

    public void viewData()
    {
        Debug.Log($"Name: {name}  Hair: {hair} Hair-Colour: {hairCol}  Head: {head[0]} {head[1]}  Torso: {torso[0]} {torso[1]}  Arms: {arms[0]} {arms[1]}  Hands: {hands[0]} {hands[1]}  Legs: {legs[0]} {legs[1]}");
    }

    public List<List<int>> ConvertForCustomiser()
    {
        List<List<int>> data = new List<List<int>>() { hairCol, head, torso, arms, hands, legs };
        return data;
    }

}
