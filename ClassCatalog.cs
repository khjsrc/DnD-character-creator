using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CharacterSheet
{
    static class ClassCatalog
    {
        private static readonly string _classesXmlFileName = @"CharacterClasses.xml";

        private static XElement GetClassXElement(string className)
        {
            XDocument xdoc = XDocument.Load(_classesXmlFileName);
            var classEle =
                (from e in xdoc.Descendants("class")
                 where e.Attribute("name") != null
                 where e.Attribute("name").Value.ToLower().Contains(className.ToLower())
                 select e).FirstOrDefault();
            return classEle;
        }

        private static XElement GetClassXElement(CharacterClass characterClass)
        {
            XDocument xdoc = XDocument.Load(_classesXmlFileName);
            var classEle =
                (from e in xdoc.Descendants("class")
                 where e.Attribute("name") != null
                 where e.Attribute("name").Value.ToLower() == characterClass.ToString().ToLower()
                select e).FirstOrDefault();
            return classEle;
        }

        private static XElement GetArchetypeXElement(string archetypeName)
        {
            XDocument xdoc = XDocument.Load(_classesXmlFileName);
            var archetype =
                (from e in xdoc.Descendants("archetype")
                 where e.Attribute("name") != null
                 where e.Attribute("name").Value.ToLower().Contains(archetypeName.ToLower())
                 select e).FirstOrDefault();
            return archetype;
        }

        public static string GetClassFullName(string className)
        {
            return GetClassXElement(className)?.Attribute("name").Value;
        }

        public static string GetArchetypeFullName(string archetypeName)
        {
            return GetArchetypeXElement(archetypeName)?.Attribute("name").Value;
        }

        public static string GetStartingEquipment(string className)
        {
            return GetClassXElement(className)?.Element("startingEquipment").Value;
        }

        public static string GetClassDescription(string className)
        {
            return GetClassXElement(className)?.Element("description").Value;
        }

        public static string GetProficiencies(string className) //look at GetStartingSkills()
        {
            var profsElements = GetClassXElement(className)?.Element("proficiencies").Elements();
            if (profsElements != null)
            {
                string result = string.Empty;
                foreach (XElement ele in profsElements)
                {
                    if(ele.Name == "savingThrows" || ele.Name == "startingSkills") continue;
                    result += ele.Name + ": " + ele.Value + '\n';
                }
                return result;
            }
            else return null;
        }

        public static string GetSavingThrows(string className)
        {
            return GetClassXElement(className)?.Element("proficiencies").Element("savingThrows").Value;
        }

        public static string GetStartingSkills(string className) //combine with the proficiencies list somehow?
        {
            return GetClassXElement(className)?.Element("proficiencies").Element("startingSkills").Value;
        }

        public static int GetStartingSkillsAmount(string className)
        {
            return Convert.ToInt32(GetClassXElement(className)?.Element("proficiencies").Element("startingSkills").Attribute("amount").Value);
        }

        public static int GetStartingHP(string className)
        {
            string hp = GetClassXElement(className)?.Element("hitPointsRules").Element("initialHP").Value;
            //Regex abilityNameRegex = new Regex(@"[SDCIW]{1}\w+");
            //Int32.TryParse(abilityNameRegex.Match(hp).Value, out int hpFromAbilityScore);
            Regex digitRegex = new Regex(@"\d+");
            Int32.TryParse(digitRegex.Match(hp).Value, out int hpBasic);
            return hpBasic;
        }

        public static string GetHPIncreaseOnLevelUp(string className)
        {
            return GetClassXElement(className)?.Element("hitPointsRules").Element("higherLevelsHP").Value;

            // Regex diceRegex = new Regex(@"(\d{0,2}[Dd]{1}\d{1,3})");
            // Regex deliberateHpIncreaseRegex = new Regex(@"(\(or \d+\))");
            // string randomHpIncrease = diceRegex.Match(hpIncrease).Value;
            // string deliberateHpIncrease = deliberateHpIncreaseRegex.Match(hpIncrease).Value;
        }

        public static string GetLevelUpHPIncrease(string className)
        {
            return GetClassXElement(className)?.Element("hitPointsRules").Element("higherLevelsHP").Value;
        }

        public static int GetProficiencyBonus(string className, int level)
        {
            //a simple formula will suffice for now because i haven't found any exceptions that would require to do it differently
            return 2 + (level - 1) / 4;
        }

        public static List<string> GetClassFeatures(string className)
        {
            var features = GetClassXElement(className)?.Element("levelsBreakdown").Descendants("feature");
            if (features != null)
            {
                var featureNames =
                    from e in features
                    where e.Attribute("name") != null
                    select e.Attribute("name").Value;
                return featureNames.ToList();
            }
            else return null;
        }

        public static List<string> GetClassFeatures(CharacterClass characterClass)
        {
            var features = GetClassXElement(characterClass)?.Element("levelsBreakdown").Descendants("feature");
            if (features != null)
            {
                var featureNames =
                    from e in features
                    where e.Attribute("name") != null
                    select e.Attribute("name").Value;
                return featureNames.ToList();
            }
            else return null;
        }

        public static List<string> GetClassFeatures(string className, int level)
        {
            var breakdown = GetClassXElement(className)?.Element("levelsBreakdown");
            if (breakdown != null)
            {
                var features =
                    from e in breakdown.Elements("level")
                    where e.Attribute("number") != null
                    where e.Attribute("number").Value == level.ToString()
                    select e.Elements("feature");

                List<string> result = new List<string>();
                foreach (var e in features.FirstOrDefault())
                {
                    result.Add(e.Attribute("name").Value);
                }
                return result;
            }
            else return null;
        }

        public static List<string> GetClassFeatures(CharacterClass characterClass, int level)
        {
            var breakdown = GetClassXElement(characterClass)?.Element("levelsBreakdown");
            if (breakdown != null)
            {
                var features =
                    from e in breakdown.Elements("level")
                    where e.Attribute("number") != null
                    where e.Attribute("number").Value == level.ToString()
                    select e.Elements("feature");

                List<string> result = new List<string>();
                foreach (var e in features.FirstOrDefault())
                {
                    result.Add(e.Attribute("name").Value);
                }
                return result;
            }
            else return null;
        }

        public static List<string> GetClassLevelPlaceholders(string className, int level) //rename it according to the things it must return
        {
            List<string> result = new List<string>();
            var breakdown = GetClassXElement(className)?.Element("levelsBreakdown");
            if (breakdown != null)
            {
                var levelElement =
                    (from ele in breakdown.Descendants("level")
                     where ele.Attribute("number") != null
                     where ele.Attribute("number").Value == level.ToString()
                     select ele).FirstOrDefault();

                foreach (XElement xEle in levelElement.Descendants())
                {
                    string item = string.Empty;
                    item += xEle.Name + "/";
                    foreach (XAttribute xAtt in xEle.Attributes())
                    {
                        item += xAtt.Name + ":" + xAtt.Value + "/";
                    }
                    item += "description:" + xEle.Value;

                    result.Add(item);
                }
            }
            return result;
        }

        public static List<string> GetClassLevelPlaceholders(CharacterClass characterClass, int level) //rename it according to the things it must return
        {
            List<string> result = new List<string>();
            var breakdown = GetClassXElement(characterClass)?.Element("levelsBreakdown");
            if (breakdown != null)
            {
                var levelElement =
                    (from ele in breakdown.Descendants("level")
                     where ele.Attribute("number") != null
                     where ele.Attribute("number").Value == level.ToString()
                     select ele).FirstOrDefault();

                foreach (XElement xEle in levelElement.Descendants())
                {
                    string item = string.Empty;
                    item += xEle.Name + "/";
                    foreach (XAttribute xAtt in xEle.Attributes())
                    {
                        item += xAtt.Name + ":" + xAtt.Value + "/";
                    }
                    item += "description:" + xEle.Value;

                    result.Add(item);
                }
            }
            return result;
        }

        public static List<string> GetClassArchetypes(string className)
        {
            var requestedClass = GetClassXElement(className);
            if (requestedClass != null)
            {
                var archetypes =
                   from e in requestedClass.Descendants("archetype")
                   where e.Attribute("name") != null
                   select e.Attribute("name").Value;
                return archetypes.ToList();
            }
            else return null;
        }

        public static List<string> GetClassArchetypes(CharacterClass characterClass)
        {
            var requestedClass = GetClassXElement(characterClass);
            if (requestedClass != null)
            {
                var archetypes =
                   from e in requestedClass.Descendants("archetype")
                   where e.Attribute("name") != null
                   select e.Attribute("name").Value;
                return archetypes.ToList();
            }
            else return null;
        }

        public static string GetArchetypeDescription(string archetypeName)
        {
            return GetArchetypeXElement(archetypeName)?.Element("description").Value;
        }

        public static List<string> GetArchetypeFeatures(string archetypeName)
        {
            var cls = GetArchetypeXElement(archetypeName);
            if (cls != null)
            {
                var features =
                    from e in cls.Descendants("archetypeFeature")
                    where e.Attribute("name") != null
                    select e.Attribute("name").Value;
                return features.ToList();
            }
            else return null;
        }

        public static string GetFeatureDescription(string featureName)
        {
            //both for archetype and normal features
            XDocument xdoc = XDocument.Load(_classesXmlFileName);
            var featureDesc =
                (from e in xdoc.Descendants("feature").Concat(xdoc.Descendants("archetypeFeature"))
                 where e.Attribute("name") != null
                 where e.Attribute("name").Value.ToLower().Contains(featureName.ToLower())
                 select e).FirstOrDefault();
            if (featureDesc != null) return featureDesc.Value;
            else return null;
        }

        #region what the actual fuck?
        public static string GetClassUniqueResource(CharacterClass characterClass, int level)
        {
            string toReturn = string.Empty;
            var barbarian = GetClassXElement(characterClass);
            var resource =
                (from levelEle in barbarian.Descendants("level")
                 where levelEle.Attribute("number") != null
                 where levelEle.Attribute("number").Value == level.ToString()
                 select levelEle.Element("uniqueResource")).FirstOrDefault();
            foreach(var res in resource.Descendants()){
                toReturn += "Name: " + res.Attribute("name").Value + " | ";
                toReturn += "Effect: " + res.Attribute("effect").Value + " +" + res.Attribute("bonus").Value + " | ";
                toReturn += "Amount: " + res.Attribute("amount").Value;
            }
            return toReturn;
        }
        #endregion
    }
}