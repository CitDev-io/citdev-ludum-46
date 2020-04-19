using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunConfiguration : ScriptableObject
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileRate;
    public int maxEnergy;
    public int fireCost;
    public int rechargeCarryModeBonus;
    public int rechargeRate;
    public int lowEnergyCost;
}