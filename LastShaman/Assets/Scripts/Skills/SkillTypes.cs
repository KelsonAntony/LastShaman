using System;
namespace SkillTypes
{
    //Using bit flags to hold damage types to make dual-types easier to modify 
    [Flags]
    public enum ElementType
    {
        None = 0,
        Fire = 1,
        Frost = 1 << 1,
        Earth = 1 << 2,
        Air = 1 << 3,
        Volcanic = Fire | Earth,
        Water = Frost | Fire,
        Lighting = Air | Frost,
        Tempest = Earth | Air,
        DualSkill = 15,
        Elemental = 16
    }

    /*
    * Elements
    * None         0000
    * Fire         0001
    * Frost        0010
    * Earth        0100
    * Air          1000
    * 
    * Volcanic     0101
    * Water        0011
    * Lightning    1010
    * Tempest      1100
    * Elemental    1111
    */

    //Flags to hold attack types
    [Flags]
    public enum AttackType
    {
        None = 0,
        Basic = 1,
        Ranged = 1 << 1,
        Melee = 1 << 2,
        Area = 1 << 3,
        Direct = 1 << 4,
        Defensive = 1 << 5,
    }

    //Flags to hold debuff types
    [Flags]
    public enum DebuffType
    {
        None = 0,
        Fire = 1,
        Chill = 1 << 1,
        Frozen = 1 << 2,
        Bleeding = 1 << 3,
        Stun = 1 << 4,
    }

    //Enum for Gameobject array for the different prefabs required
    public enum PrefabType
    {
        FIREBALL = 0,
        FIRESPEED,
        PYRO,
        METEOR,
        EARTHWALL,
    }
}