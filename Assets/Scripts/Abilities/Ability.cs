using System.Collections.Generic;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = @"Anchorang/Ability", order = 0)]
    public class Ability : ScriptableObject
    {
        public string Name;
        public List<Aura> Aura;

        public void UseAbility(GameObject owner)
        {
            foreach (var aura in Aura)
            {
                this.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, owner);
            }
        }

    }
}