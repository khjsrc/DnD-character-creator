

namespace CharacterSheet
{
    /// <summary>
    /// Ways of assigning the character's stats.
    /// </summary>
    public enum StatsAssignment
    {
        /// <summary>
        /// The stats assign randomly, following the rule "4d6, drop the lowest, sum up the results"
        /// </summary>
        FullRandom,
        /// <summary>
        /// The stats are assigned according to the given order, descending. Follows the rule "4d6, drop the lowest, sum up the results"
        /// </summary>
        SemiRandom,
        /// <summary>
        /// The stats are assigned according to the given order, descending. The stat scores in this case are: { 15, 14, 13, 12, 10, 8 }
        /// </summary>
        Deliberately,
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        PointsBased
    }

    /// <summary>
    /// The list of all available character classes.
    /// </summary>
    public enum CharacterClass
    {
        /// <summary>
        /// mighty warrior, can rage
        /// </summary>
        Barbarian,
        /// <summary>
        /// mighty warrior, can sing
        /// </summary>
        Bard,
        /// <summary>
        /// mighty warri... no, wait, it's a caster, can use cantrips
        /// </summary>
        Cleric,
        /// <summary>
        /// mighty caster, can become a mighty warrior
        /// </summary>
        Druid,
        /// <summary>
        /// mighty warrior, cannot become a mighty caster
        /// </summary>
        Fighter,
        /// <summary>
        /// mighty fister, loves fisting enemies
        /// </summary>
        Monk,
        /// <summary>
        /// mighty warrior, can pray
        /// </summary>
        Paladin,
        /// <summary>
        /// mighty bowstring puller, can pull strings
        /// </summary>
        Ranger,
        /// <summary>
        /// stealthy warrior, can poke knives into enemies' kidneys
        /// </summary>
        Rogue,
        /// <summary>
        /// mighty caster, will never become a mighty warrior
        /// </summary>
        Sorcerer,
        /// <summary>
        /// mighty caster, can seal pacts
        /// </summary>
        Warlock,
        /// <summary>
        /// mighty caster, probably can do wizardry
        /// </summary>
        Wizard
    }

    /// <summary>
    /// The list of all available races. 0b01 - subrace, 0b10 - core race w/ subraces, 0b11 - core race w/o subraces
    /// </summary>
    enum CharacterRace
    {
        Dragonborn = 0b11_0000_0001,
        /// <summary>
        /// Not a subclass, hence can't be picked.
        /// </summary>
        Dwarf = 0b10_0000_0010,
        HillDwarf = 0b01_0000_0011,
        MountainDwarf = 0b01_0000_0100,
        /// <summary>
        /// Not a subclass, hence can't be picked.
        /// </summary>
        Elf = 0b10_0000_0101,
        HighElf = 0b01_0000_0110,
        WoodElf = 0b01_0000_0111,
        Drow = 0b01_0000_1000,
        /// <summary>
        /// Not a subclass, hence can't be picked.
        /// </summary>
        Gnome = 0b10_0000_1001,
        ForestGnome = 0b01_0000_1010,
        RockGnome = 0b01_0000_1011,
        HalfElf = 0b11_0000_1100,
        /// <summary>
        /// Not a subclass, hence can't be picked.
        /// </summary>
        Halfling = 0b10_0000_1101,
        Lightfoot = 0b01_0000_1110,
        Stout = 0b01_0000_1111,
        HalfOrc = 0b11_0001_0000,
        Human = 0b11_0001_0001,
        Tiefling = 0b11_0001_0010
    }

    /// <summary>
    /// Not sure if I need this.
    /// </summary>
    public enum RaceType
    {
        /// <summary>
        /// Equal to 0b11_0000_0000
        /// </summary>
        CoreWithoutSubraces = 768,
        /// <summary>
        /// Equal to 0b10_0000_0000
        /// </summary>
        CoreWithSubraces = 512,
        /// <summary>
        /// Equal to 0b01_0000_0000
        /// </summary>
        Subrace = 256
    }
}