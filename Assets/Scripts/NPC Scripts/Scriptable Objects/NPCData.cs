using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "NPC Data")]
public class NPCData : ScriptableObject
{
    public new string name;
    public int vitality; //Health
    public int strength; //Damage
    public int agility;  //Speed
}
