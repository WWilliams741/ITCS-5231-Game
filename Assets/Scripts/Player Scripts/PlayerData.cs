using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerData 
{
    private string fileName;
    public string name;
    public int vitality; //Health
    public int strength; //Damage
    public int agility;  //Speed
    public double exp;
    public int level;

    public PlayerData(string path)
    {
        fileName = path;
    }

    public string GetFileName()
    {
        return fileName;
    }
}
