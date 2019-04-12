using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = @"Anchorang/Item", order = 0)]
    public class Item : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public List<Ability> Abilities;

        public void Use(GameObject owner)
        {
            foreach (var ability in Abilities)
            {
                this.SendMessageTo(new UseAbilityMessage{Ability = ability}, owner);
            }
        }
    }
}