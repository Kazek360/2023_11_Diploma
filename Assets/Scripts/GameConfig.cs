using System.Collections;
using System.Collections.Generic;
using DialogueSystem.UI;
using Quests.UI;
using QuestSystem.UI;
using UnityEngine;


/**
 * A class used to assign references in order to avoid unnecessary strong coupling of classes
 */
public class GameConfig
{
    public static Inventory PlayerInventory;
    public static Canvas canvas;
    public static AbilityLibrary abilityLibrary;
    public static ItemLibrary itemLibrary;
}
