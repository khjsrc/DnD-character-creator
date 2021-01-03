using System;
using System.Collections;
using System.Collections.Generic;

namespace CharacterSheet
{
    static class Races
    {
        private static Dictionary<string, byte> races = new Dictionary<string, byte>(){
            {"dragonborn", 1},
            {"dwarf", 2},
            {"elf", 3},
            {"gnome", 4},
            {"half-elf", 5},
            {"halfling", 6},
            {"half-orc", 7},
            {"human", 8},
            {"tiefling", 9}
        };

        static public List<string> RacesList
        {
            get
            {
                return new List<string>(races.Keys);
            }
            set
            {

            }
        }

        static public byte GetRaceID(string name)
        {
            try
            {
                return races[name.ToLower()];
            }
            catch (KeyNotFoundException ex)
            {
                return 255;
            }
        }
    }
}