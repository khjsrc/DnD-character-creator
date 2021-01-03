using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CharacterSheet
{
    // add import/export methods that will do the import/export of a character to xml format
    [Serializable]
    class Character
    {
        private EventHandler ExperienceGained;
        public EventHandler LevelledUp;

        private string _characterName, _className, _raceName, _background, _alignment;
        int _armorClass, _initiative, _speed;
        private int _currentHP, _baseHP, _maxHP;

        private bool _increaseHpRandomly;

        private List<string> _traits;
        private List<string> _savingThrows;
        private List<string> _learnedSkills;

        private Abilities _abilities;

        #region properties
        public string Owner{
            get;
            private set;
        }
        public int InitiativeBonus{
            get;
            private set;
        }
        public int PassiveWisdom{
            get{
                int passiveWisdomValue = _abilities.GetAbilityModifier("wis");
                if(_learnedSkills.Contains("Perception")) passiveWisdomValue += ClassCatalog.GetProficiencyBonus(_className, Level);
                return passiveWisdomValue;
            }
            private set{

            }
        }
        public int Level{
            get;
            private set;
        }
        public int Experience{
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Creates a character list for Dungeons and Dragons.
        /// </summary>
        /// <param name="name">Name of the character.</param>
        /// <param name="className">Class of the character.</param>
        /// <param name="raceName">Race of the character.</param>
        /// <param name="randomStats">Generate stats randomly or deliberately. 
        /// Randomly: roll d6 4 times, drop the lowest result and add the rest 3 together, repeat for all abilities.
        /// Deliberately: assign the default values to the abilities (15,14,13,12,10,8)</param>
        /// <param name="statsOrder">Pass an array of stats in the order of importance for you. 
        /// By default the order is: str, dex, con, int, wis, cha.</param>
        public Character(string name, string className, string raceName, bool randomStats = true, string[] statsOrder = null, bool increaseHpRandomly = true)
        {
            if (statsOrder == null) statsOrder = new string[] { "str", "dex", "con", "int", "wis", "cha" };
            
            _characterName = name;
            _className = ClassCatalog.GetClassFullName(className);
            _raceName = RaceCatalog.GetRaceFullName(raceName);
            _increaseHpRandomly = increaseHpRandomly;

            Level = 1;
            Experience = 0;

            _abilities = new Abilities();
            _traits = RaceCatalog.GetRaceTraitNames(raceName);
            _learnedSkills = new List<string>();
            _savingThrows = new List<string>();
            
            if(randomStats) _abilities.GenerateAbilityScoresRandomly(statsOrder);
            else _abilities.GenerateAbilityScoresDeliberately(statsOrder);
            
            foreach (KeyValuePair<string, int> abilityIncrease in RaceCatalog.GetAbilityScoreBonuses(_raceName)) //make a separate method that would do this?
            {
                _abilities.IncreaseAbilityScore(abilityIncrease.Key, abilityIncrease.Value);
            }
            FillSavingThrows();
            
            _baseHP = ClassCatalog.GetStartingHP(_className) + _abilities.GetAbilityModifier("con");
            _currentHP = _baseHP;
            _maxHP = _baseHP;
            
            LevelledUp += OnLevelUp;
            ExperienceGained += OnExperienceGained;
        }

        public Dictionary<string, int> GetSkills(){
            Dictionary<string, int> dict = new Dictionary<string, int>();
            string[] skills = SkillCatalog.GetSkills();
            foreach(string skill in skills){
                string relatedAbility = SkillCatalog.GetSkillRelatedAbility(skill);
                var skillBonus = _abilities.GetAbilityModifier(relatedAbility);
                if(_learnedSkills.Contains(skill)) skillBonus += ClassCatalog.GetProficiencyBonus(_className, Level);
                dict.Add(skill, skillBonus);
            }
            return dict;
        }

        public int GetSkillBonus(string skillName)
        {
            //check if the specified skill exists
            string skillActualName = SkillCatalog.GetSkillFullName(skillName);
            string ability = SkillCatalog.GetSkillRelatedAbility(skillActualName);
            int abilModifier = _abilities.GetAbilityModifier(ability);
            if (_learnedSkills.Contains(skillActualName)) abilModifier += ClassCatalog.GetProficiencyBonus(_className, Level);
            return abilModifier;
        }

        public int GetSavingThrowBonus(string abilityName){
            int savingThrow = _abilities.GetAbilityModifier(abilityName);
            if(_savingThrows.Contains(abilityName.ToLower())) savingThrow += ClassCatalog.GetProficiencyBonus(_className, Level);
            return savingThrow;
        }

        private void LearnTrait(string traitName)
        {
            if (_traits.Contains(traitName)) _traits.Add(traitName);
        }

        private void LearnSkill(string skillName)
        {
            //apparently, the skills can be learned and here i need to check if the requested skill is not learned yet and then add it to the list of learned skills
            skillName = SkillCatalog.GetSkillFullName(skillName);
            if (_learnedSkills.Contains(skillName)) throw new Exception($"The specified skill - {skillName} - is already well known to you.");
            else _learnedSkills.Add(skillName);
        }

        public void FillStartingSkills(IEnumerable<string> skills)
        {
            //skills that the character is proficient with will have an additional scaling based on the character's proficiency, i.e. Athletics as a learned skill will scale off with Strength modifier and the proficiency modifier.
            string startingSkills = ClassCatalog.GetStartingSkills(_className);
            int counter = 0;
            int maxStartingSkills = ClassCatalog.GetStartingSkillsAmount(_className);
            //if skills.length < maxStartingSkills do a workaround or don't do anything at all? or return something else? exception?
            if (skills.Count() < maxStartingSkills) throw new Exception($"Skills in the specified list: {skills.Count()}\nThe amount you need to learn: {maxStartingSkills}");
            foreach (var skill in skills)
            {
                string skillFullName = SkillCatalog.GetSkillFullName(skill);
                if(skillFullName == null) throw new Exception($"The specified skill - {skill} - does not exist.");
                if (counter <= maxStartingSkills && startingSkills.ToLower().Contains(skillFullName.ToLower()))
                {
                    counter++;
                    LearnSkill(skillFullName);
                }
            }
        }

        private void FillSavingThrows()
        {
            string profThrows = ClassCatalog.GetSavingThrows(_className).ToLower();
            foreach(var ability in profThrows.Replace(", ", ",").Split(',')){
                _savingThrows.Add(ability);
            }
        }

        private int GetPassiveWisdom(){
            int passiveWisdomValue = _abilities.GetAbilityModifier("wisdom");
            if(_learnedSkills.Contains("Perception")) passiveWisdomValue += ClassCatalog.GetProficiencyBonus(_className, Level);
            return passiveWisdomValue;
        }

        public void GiveExperience(int exp)
        {
            ExperienceGained(exp, EventArgs.Empty);
        }

        private void OnExperienceGained(object o, EventArgs e)
        {
            // if(Experience + (int)o >= threshold){
            //     invoke LevelUp() event
            // }
            // else{
            //     AddExp()
            // }
        }

        private void OnLevelUp(object o, EventArgs e)
        {
            Level += 1;
            if (_increaseHpRandomly)
            {
                Regex diceRegex = new Regex(@"(\d{0,2}[Dd]{1}\d{1,3})");
                string diceForHPIncrease = diceRegex.Match(ClassCatalog.GetHPIncreaseOnLevelUp(_className)).Value.ToLower();
                if (Int32.TryParse(diceForHPIncrease.Substring(0, diceForHPIncrease.IndexOf('d')), out int amountOfRolls))
                {
                    Int32.TryParse(diceForHPIncrease.Substring(diceForHPIncrease.IndexOf('d') + 1), out int diceNumber);
                    for (int i = 0; i < amountOfRolls; i++)
                    {
                        int hpIncrease = new Random().Next(1, diceNumber + 1);
                        _baseHP += hpIncrease;
                        _maxHP = _baseHP + Level * _abilities.GetAbilityModifier("con");


                        _currentHP = _maxHP; //might need to remove it due to certain misterious circumstances when hp must stay low
                    }
                    //do i need to set the current hp equal to _maxHP?
                }
                else throw new Exception("you fucked up with the HP increase values in the xml file.");
            }
            else
            {
                Regex deliberateHpIncreaseRegex = new Regex(@"(\(or \d+\))");
                string hpIncreaseValue = deliberateHpIncreaseRegex.Match(ClassCatalog.GetHPIncreaseOnLevelUp(_className)).Value.ToLower();
                Regex digit = new Regex(@"(\d+)");
                if (Int32.TryParse(digit.Match(hpIncreaseValue).Value, out int hpIncrease))
                {
                    _baseHP += hpIncrease;
                    _maxHP = _baseHP + Level * _abilities.GetAbilityModifier("con");

                    _currentHP = _maxHP; //might need to remove it due to certain misterious circumstances when hp must stay low
                }
                else throw new Exception("you fucked up with the HP increase values in the xml file.");
            }
        }
    }
}