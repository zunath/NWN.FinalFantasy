namespace NWN.FinalFantasy.Core.NWScript.Enum
{
    public enum BonusType
    {
        Attack = 1,
        Damage,
        SavingThrow,
        Ability,
        Skill,
        TouchAttack
    }

    public enum ObjectTypeEngine
    {
        Invalid = -1,
        GUI = 1,
        Tile,
        Module,
        Area,
        Creature,
        Item,
        Trigger,
        Projectile,
        Placeable,
        Door,
        AreaOfEffect,
        Waypoint,
        Encounter,
        Store,
        Portal,
        Sound
    }

    public enum SaveReturn
    {
        Failed,
        Success,
        Immune
    }

    public enum CastingMode
    {
        Disabled,
        Activated
    }

    public enum TouchAttackReturn
    {
        Miss,
        Hit,
        Critical
    }

    public enum ArmorClassBonus
    {
        Dodge,
        Natural,
        Armor,
        Shield,
        Deflection
    }

    public enum ToggleMode
    {
        Detect,
        Stealth,
        Parry,
        PowerAttack,
        ImprovedPowerAttack,
        CounterSpell,
        FlurryOfBlows,
        RapidShot,
        Expertise,
        ImprovedExpertise,
        DefensiveCast,
        DirtyFighting,
        DefensiveStance
    }

    public enum CombatModeEngine
    {
        None,
        Parry,
        PowerAttack,
        ImprovedPowerAttack,
        CounterSpell,
        FlurryOfBlows,
        RapidShot,
        Expertise,
        ImprovedExpertise,
        DefensiveCasting,
        DirtyFighting,
        DefensiveStance
    }
}