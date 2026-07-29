using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RandomSpellMerchantPatcher;

public class Settings
{
    [SettingName("Common Chance of No Tome")]
    [Tooltip("Percentage chance that a common spell-tome roll produces no tome.")]
    public int CommonChanceNone = 50;

    [SettingName("Rare Chance of No Tome")]
    [Tooltip("Percentage chance that a rare spell-tome roll produces no tome.")]
    public int RareChanceNone = 75;

    [SettingName("Very Rare Chance of No Tome")]
    [Tooltip("Percentage chance that a very rare spell-tome roll produces no tome.")]
    public int VeryRareChanceNone = 90;

    [SettingName("Overall Roll Multiplier")]
    [Tooltip("Multiplies the number of rolls generated from each spell list. Higher values add more spell tomes to merchant inventories.")]

    public int RollMag = 2;

    [SettingName("Spell Tier Roll Divisor")]
    [Tooltip("Reduces rolls for higher spell tiers. Apprentice uses this value, Adept uses 2x, Expert uses 3x, and Master uses 4x.")]

    public int TierRollReduction  = 1;

    [SettingName("Use Spell Level Restrictions")]
    [Tooltip("When enabled, merchant spell availability respects spell-level restrictions.")]
    public bool IsSpellLvl = true;
}