using System;
using System.Collections.Generic;
using System.Linq;

namespace CharacterSheet
{
    class Abilities
    {
        private Dictionary<string, int> _abilityScores;

        public int this[string name]
        {
            get
            {
                if (_abilityScores.Keys.Contains(name)) return _abilityScores[name];
                else return -1;
            }
            private set
            {
                _abilityScores[name] = value;
            }
        }

        public Abilities()
        {
            _abilityScores = new Dictionary<string, int>(){
                {"strength", 0},
                {"dexterity", 0},
                {"constitution", 0},
                {"intelligence", 0},
                {"wisdom", 0},
                {"charisma", 0}
            };
        }

        public IEnumerable<string> GetAbilities()
        {
            return _abilityScores.Keys;
        }

        public int GetAbilityModifier(string name)
        {
            name = GetAbilityMatchingName(name); //name.ToLower();
            if (_abilityScores.Keys.Contains(name))
            {
                return (_abilityScores[name] - 10) / 2;
            }
            else
            {
                return 0;
            }
        }

        private string GetAbilityMatchingName(string name)
        {
            return _abilityScores.Where(x => x.Key.ToLower().Contains(name.ToLower())).FirstOrDefault().Key;
        }

        internal void ChangeAbilityScore(string name, int amount)
        { //probably should restrict the usage of this and also, normally an ability can not go higher than 20
            _abilityScores[_abilityScores.Where(k => k.Key.Contains(name)).FirstOrDefault().Key] += amount;
        }

        internal void GenerateAbilityScoresRandomly()
        {
            int[] results = new int[6];
            for (int j = 0; j < results.Length; j++)
            {
                int[] diceRolls = new int[4];
                Random rng = new Random();
                for (int i = 0; i < diceRolls.Length; i++)
                {
                    int roll = rng.Next(1, 7);
                    diceRolls[i] = roll;
                }
                int lowest = 0;
                for (int i = 0; i < diceRolls.Length; i++)
                {
                    if (diceRolls[i] < diceRolls[lowest])
                    {
                        lowest = i;
                    }
                }
                diceRolls[lowest] = 0;

                int sum = 0;
                foreach (int roll in diceRolls)
                {
                    sum += roll;
                }

                results[j] = sum;
            }

            for (int i = 0; i < results.Length; i++)
            {
                _abilityScores[_abilityScores.Keys.ToArray()[i]] = results[i];
            }
        }

        /// <summary>
        /// Roll d6 4 times, drop the lowest result and add the rest 3 together, then assign the results according to the abilityScoresOrder.
        /// </summary>
        /// <param name="abilityScoresOrder">Stats in order of importance, the first stat gets the highest value.</param>
        internal void GenerateAbilityScoresRandomly(IEnumerable<string> abilityScoresOrder)
        {
            int[] results = new int[6];
            for (int j = 0; j < results.Length; j++)
            {
                int[] diceRolls = new int[4];
                Random rng = new Random();
                for (int i = 0; i < diceRolls.Length; i++)
                {
                    int roll = rng.Next(1, 7);
                    diceRolls[i] = roll;
                }
                int lowest = 0;
                for (int i = 0; i < diceRolls.Length; i++)
                {
                    if (diceRolls[i] < diceRolls[lowest])
                    {
                        lowest = i;
                    }
                }
                diceRolls[lowest] = 0;

                int sum = 0;
                foreach (int roll in diceRolls)
                {
                    sum += roll;
                }

                results[j] = sum;
            }

            bool sort = true;
            while (sort)
            {
                sort = false;
                for (int i = 0; i < results.Length - 1; i++)
                {
                    int temp = results[i];
                    if (results[i] < results[i + 1])
                    {
                        sort = true;
                        results[i] = results[i + 1];
                        results[i + 1] = temp;
                    }
                }
            }

            string[] abs = abilityScoresOrder.ToArray();
            for (int i = 0; i < abs.Length; i++)
            {
                var ability = _abilityScores.FirstOrDefault(ability =>
                {
                    return ability.Key.StartsWith(abs[i]);
                });
                _abilityScores[ability.Key] = results[i];
            }
        }

        /// <summary>
        /// Assign the pre-determined values to the stats
        /// </summary>
        /// <param name="abilityScoresOrder">Stats in order of importance, the first stat gets the highest value.</param>
        internal void GenerateAbilityScoresDeliberately(IEnumerable<string> abilityScoresOrder)
        {
            int[] scores = { 15, 14, 13, 12, 10, 8 };
            int i = 0;
            foreach (string abilityName in abilityScoresOrder)
            {
                var ability = _abilityScores.FirstOrDefault(ability =>
                {
                    return ability.Key.StartsWith(abilityName);
                });
                _abilityScores[ability.Key] = scores[i];
                i++;
            }
        }
    }
}