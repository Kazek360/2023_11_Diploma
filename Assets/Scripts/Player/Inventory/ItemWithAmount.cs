using System;
using UnityEngine;

namespace Player.Inventory
{
    [Serializable]
    public class ItemWithAmount
    {
        public ScriptableObject item;
        public int amount = 1;
    }
}