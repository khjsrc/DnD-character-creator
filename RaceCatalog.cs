using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CharacterSheet
{
    static class RaceCatalog //convert the whole class to static, move the raceName to the Character class, do the same with CharacterClass class
    {//this all is a fucking mess
        private static readonly string _racesXmlFileName = "CharacterRaces.xml";

        public static string GetKnownLanguages(string raceName)
        {
            return GetRaceXElement(raceName)?.Element("raceInfo").Element("language").Attribute("brief").Value;
        }

        public static bool DoesNeedToLearnLanguage(string raceName)
        {
            int learnedLanguagesAmount = GetKnownLanguages(raceName).Replace(", ", ",").Split(',').Length;
            Int32.TryParse(GetRaceXElement(raceName)?.Element("raceInfo").Element("language").Attribute("amount").Value, out int amountOfLanguagesToLearn);
            if (learnedLanguagesAmount < amountOfLanguagesToLearn) return true;
            else return false;
        }

        public static int GetSpeed(string raceName)
        {
            if (false == Int32.TryParse(GetRaceXElement(raceName)?.Element("raceInfo").Element("speed").Attribute("brief").Value, out int result)) throw new ArgumentException($"Couldn't find the speed stat of the specified race - {raceName}");
            return result;
        }

        public static string GetSize(string raceName)
        {
            return GetRaceXElement(raceName)?.Element("raceInfo").Element("size").Attribute("brief").Value;
        }

        public static string GetRaceFullName(string raceName)
        {
            var raceXElement = GetRaceXElement(raceName);
            if (raceXElement.Element("subrace") != null) return raceXElement?.Element("subrace").Attribute("raceName").Value;
            else return raceXElement?.Attribute("raceName").Value;
        }
        
        public static string GetRaceInfo(string raceName)
        {
            var race = GetRaceXElement(raceName);
            var descElements =
                from desc in race.Descendants("raceInfo").Descendants() //something is off here, rework later
                where desc.Name != "uniqueTraits"
                where desc.Name != "trait"
                select desc;
            string description = string.Empty;
            foreach (XElement element in descElements)
            {
                description += element.Value + '\n';
            }

            return description;
        }

        public static string GetTraitInfo(string traitName)
        {
            XDocument xDoc = XDocument.Load(_racesXmlFileName);
            IEnumerable<XElement> elements =
                from element in xDoc.Descendants("trait")
                where element.Attribute("name").Value.ToLower() == traitName.ToLower()
                select element;

            if (elements.Count() == 0)
            {
                return "I couldn't find anything regarding your request, it must've been stolen by the nearest cave's dwellers.";
            }
            else
            {
                return elements.First().Value.ToString();
            }
        }

        public static List<string> GetRaceTraitNames(string raceName)
        {
            XDocument xDoc = XDocument.Load(_racesXmlFileName);

            var desiredRaces =
                from element in xDoc.Root.Descendants()
                where element.Attribute("raceName") != null
                where element.Attribute("raceName").Value.ToLower() == raceName.ToLower()
                select element;

            // gogo gadget get the traits' names list
            if (desiredRaces.Count() != 0)
            {
                if (desiredRaces.FirstOrDefault().Name == "subrace")
                {
                    var subrace = desiredRaces.FirstOrDefault();
                    var subraceTraits = subrace.Descendants("trait");
                    var parentRace = desiredRaces.FirstOrDefault().Parent;
                    var parentRaceTraits = parentRace.Descendants("trait");

                    var traits = parentRaceTraits.Union(subraceTraits); //or IEnumerable<T>.Concat(), which will not eliminate similar elements

                    var names =
                        from att in traits
                        select att.Attribute("name").Value;

                    return names.ToList();
                }
                else
                {
                    var traits = desiredRaces.FirstOrDefault().Descendants("trait");
                    var names =
                        from att in traits
                        select att.Attribute("name").Value;

                    return names.ToList();
                }
            }
            else
            {
                return new List<string>() { "sorry, there's nothing" };
            }
        }

        public static XElement GetRaceXElement(string raceName)
        {
            XDocument xDoc = XDocument.Load(_racesXmlFileName);
            var matchingRaces =
                from element in xDoc.Root.Descendants()
                where element.Attribute("raceName") != null
                where element.Attribute("raceName").Value.ToLower().Contains(raceName.ToLower())
                select element;


            if (matchingRaces.Count() != 0)
            {
                var firstMatch = matchingRaces.First();
                
                if (firstMatch.Name == "subrace")
                {
                    var otherSubraces = 
                        from subrace in firstMatch.Parent.Descendants("subrace")
                        where subrace.Attribute("raceName").Value.ToLower() != raceName.ToLower()
                        select subrace;
                    
                    otherSubraces.Remove();

                    return firstMatch.Parent;
                }
                else return firstMatch;
            }
            else return null;
        }

        public static Dictionary<string, int> GetAbilityScoreBonuses(string raceName)
        {
            XElement raceElement = GetRaceXElement(raceName);
            Dictionary<string, int> resultDictionary = new Dictionary<string, int>();
            if (IsFinalForm(raceName))
            {
                //gogo gadget get the abilityScores list
                var allScores = 
                    from ele in raceElement.DescendantsAndSelf()
                    where ele.Parent.Name == "abilityScores"
                    select ele;
                foreach (var ability in allScores)
                {
                    Int32.TryParse(ability.Value, out int score);
                    if(resultDictionary.Keys.Contains(ability.Name.ToString())){
                        resultDictionary[ability.Name.ToString()] += score;
                    }
                    else{
                        resultDictionary.Add(ability.Name.ToString(), score);
                    }
                }
            }
            else
            {
                //gogo gadget get only the kernel race with the needed subrace
                return null;
            }

            return resultDictionary;
        }

        public static bool IsFinalForm(string raceName)// this works but uhhhh..... it looks and seems awful
        {
            //basically all this has to do is to return true or false whether the desiredRace is the one of its kind or has subraces
            var baseRace = GetRaceXElement(raceName);
            if(baseRace == null) return false;

            var race =
                from subRace in baseRace.DescendantsAndSelf()
                where subRace.Attribute("raceName") != null
                where subRace.Attribute("raceName").Value.ToLower() == raceName.ToLower()
                select subRace;

            if (race.First().Name.ToString().ToLower() == "race") //if the desired race is a base race, there might be some subraces, check if it's true or not
            {
                var subraces = race.Descendants("subrace");
                if (subraces.Count() > 0)
                {
                    return false;
                }
                else{
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        // private static List<string> GetRacesBySource(string source){

        // }
    }
}