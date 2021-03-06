﻿using System;
using UnityEngine;

/// <summary>
/// This class stores damage values for each weapon
/// </summary>
[Serializable]
public class WeaponProperties
{
    /// <summary>
    /// Type of the weapon
    /// </summary>
    public Weapon.WEAPON_TYPE weaponType;

    /// <summary>
    /// Damage inflicted by the weapon
    /// </summary>
    public float damage;

    /// <summary>
    /// The location the weapon upgrade will appear
    /// </summary>
    public Vector3 location;
}
