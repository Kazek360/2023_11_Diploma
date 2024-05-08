using System.Collections;
using System.Collections.Generic;
using DialogueSystem.UI;
using UnityEngine;


/**
 * A class used to assign references in order to avoid unnecessary strong coupling of classes
 */
public class GameConfig
{
    public static DialogueUI DialogueUI;
    public static Inventory PlayerInventory;
    public static PlayerStats PlayerStats;
    public static Canvas canvas;
}
