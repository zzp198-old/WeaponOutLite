using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace WeaponOut;

public class WOConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)] public bool Default;
    [DefaultValue(true)] public bool Generic;
    [DefaultValue(true)] public bool Magic;
    [DefaultValue(true)] public bool Melee;
    [DefaultValue(true)] public bool Ranged;
    [DefaultValue(true)] public bool Summon;
    [DefaultValue(true)] public bool Throwing;
    [DefaultValue(true)] public bool MagicSummonHybrid;
    [DefaultValue(true)] public bool MeleeNoSpeed;
    [DefaultValue(true)] public bool SummonMeleeSpeed;
    [DefaultValue(true)] public bool NotWeapon;

    [Range(0, 1000), DefaultValue(100)] [Tooltip("Change not recommended")]
    public int Scale;
}