using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Items
{
    public class TestMeleeWeapon
    {
        public readonly string Id = "weapon.testmeleeweapon";
        public readonly string WeaponName = "Test Melee Weapon";
        public int Damage = 1;
        public float AttackSpeed = 1f;  // 1 unit = 1 attack / second
        public float Range = 1;         // Absolute range
        public float CoolDown = 0f;
    }
}