using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CharacterSheet
{
    class Program
    {
        static void Main(string[] args)
        {
            // string[] abilityScores = {"strength", "dexterity", "constitution", "intelligence", "wisdom", "charisma"};
            // string[] otherAbilityScores = {"dex", "con", "cha", "int", "wis", "str"};
            #region characterClass crap
            // XmlDocument classesdotxml = new XmlDocument();

            // var declaration = classesdotxml.CreateXmlDeclaration("1.0", "utf-8", "yes");
            // classesdotxml.AppendChild(declaration);

            // XmlElement classRoot = classesdotxml.CreateElement("characterClasses");
            // classesdotxml.AppendChild(classRoot);
            // int i = 1;
            // foreach(string cls in Classes.ClassesList){
            //     XmlElement classElement = classesdotxml.CreateElement("class");
            //     classElement.SetAttribute("Name", cls);
            //     classElement.SetAttribute("ID", i++.ToString());
            //     XmlElement desc = classesdotxml.CreateElement("description");
            //     desc.InnerText = $"...soon the flames will fade, and only {cls} will remain...";
            //     classElement.AppendChild(desc);

            //     //proficiencies part
            //     var profs = classElement.AppendChild(classesdotxml.CreateElement("proficiencies"));
            //     #region REEEE
            //     XmlElement armor = classesdotxml.CreateElement("armor");
            //     armor.InnerText = "placeholder";
            //     profs.AppendChild(armor);
            //     XmlElement weapons = classesdotxml.CreateElement("weapons");
            //     weapons.InnerText = "placeholder";
            //     profs.AppendChild(weapons);
            //     XmlElement tools = classesdotxml.CreateElement("tools");
            //     tools.InnerText = "placeholder";
            //     profs.AppendChild(tools);
            //     XmlElement savingThrows = classesdotxml.CreateElement("savingThrows");
            //     savingThrows.InnerText = "placeholder";
            //     profs.AppendChild(savingThrows);
            //     XmlElement skills = classesdotxml.CreateElement("skills");
            //     skills.InnerText = "placeholder";
            //     profs.AppendChild(skills);
            //     #endregion

            //     var startingEquipEle = classesdotxml.CreateElement("startingEquipment");
            //     startingEquipEle.InnerText = "You start with the following Equipment, in addition to the Equipment granted by your background:\n1. \n2. \n3. \n";
            //     classElement.AppendChild(startingEquipEle);

            //     //rules regarding hp of the characters
            //     XmlElement hpInfo = classesdotxml.CreateElement("hitPointsRules");
            //     var initialHPrule = classesdotxml.CreateElement("initialHP");
            //     initialHPrule.InnerText = "6-12 + your Constitution modifier";
            //     var furtherHPrule = classesdotxml.CreateElement("higherLevelsHP");
            //     furtherHPrule.InnerText = $"1d6-12(or x) + your Constitution modifier per {cls} level after 1st";
            //     hpInfo.AppendChild(initialHPrule);
            //     hpInfo.AppendChild(furtherHPrule);

            //     //what the character learns after reaching a certain level
            //     XmlElement breakdown = classesdotxml.CreateElement("levelsBreakdown");
            //     int proficiencyBonus = 2;
            //     for(int level = 1; level <= 20; level++){
            //         var temp = classesdotxml.CreateElement("level");
            //         temp.SetAttribute("number", $"{level}");
            //         var profEle = classesdotxml.CreateElement("proficiencyBonus");
            //         profEle.InnerText = $"+{proficiencyBonus}";
            //         temp.AppendChild(profEle);
            //         var featuresEle = classesdotxml.CreateElement("features");
            //         var featureEle = classesdotxml.CreateElement("feature");
            //         featureEle.SetAttribute("Name", "placeholder");
            //         featureEle.InnerText = "placeholder";
            //         temp.AppendChild(featureEle);
            //         breakdown.AppendChild(temp);
            //         if(level % 4 == 0)proficiencyBonus++;
            //     }
            //     classElement.AppendChild(hpInfo);
            //     classElement.AppendChild(breakdown);

            //     XmlElement archetypes = classesdotxml.CreateElement("archetypes");
            //     for(int p = 0; p < 4; p++){
            //         XmlElement archetype = classesdotxml.CreateElement("archetype");
            //         archetype.SetAttribute("Name", "Path of the Ape");
            //         XmlElement skillsBreakdown = classesdotxml.CreateElement("archetypeFeatures");
            //         for(int l = 0; l < 4; l++){
            //             XmlElement skill = classesdotxml.CreateElement("archetypeFeature");
            //             skill.SetAttribute("level", l.ToString());
            //             skill.InnerText = "descriptionOfTheSkill";
            //             skillsBreakdown.AppendChild(skill);
            //         }
            //         archetype.AppendChild(skillsBreakdown);
            //         archetypes.AppendChild(archetype);
            //     }
            //     classElement.AppendChild(archetypes);

            //     classRoot.AppendChild(classElement);
            // }
            // classesdotxml.Normalize();
            // classesdotxml.Save("CharacterClasses.xml");
            #endregion

            #region characterRace crap
            // XmlDocument racesdotxml = new XmlDocument();
            // var racesDeclaration = racesdotxml.CreateXmlDeclaration("1.0", "utf-8", "yes");
            // racesdotxml.AppendChild(racesDeclaration);
            // XmlElement racesRoot = racesdotxml.CreateElement("characterRaces");
            // racesdotxml.AppendChild(racesRoot);

            // foreach(string r in Races.RacesList){
            //     XmlElement raceElement = racesdotxml.CreateElement("race");
            //     raceElement.SetAttribute("Name", r);

            //     //race description
            //     XmlElement info = racesdotxml.CreateElement("raceInfo");

            //     XmlElement desc = racesdotxml.CreateElement("description");
            //     desc.InnerText = "Your race is one of the races of this world.";
            //     info.AppendChild(desc);

            //     XmlElement age = racesdotxml.CreateElement("age");
            //     age.InnerText = "The individuals of your race approach the inevitable end this fast";
            //     info.AppendChild(age);

            //     XmlElement alignment = racesdotxml.CreateElement("alignment");
            //     alignment.InnerText = "your race is extremely evil or awfully good (select the desired option)";
            //     info.AppendChild(alignment);

            //     XmlElement size = racesdotxml.CreateElement("size");
            //     size.InnerText = "Your size is medium. I'd even say it's average. You're not an outstanding creature, deal with it";
            //     info.AppendChild(size);

            //     XmlElement speed = racesdotxml.CreateElement("speed");
            //     speed.InnerText = "Your base walking speed is 30 feet.";
            //     info.AppendChild(speed);

            //     XmlElement language = racesdotxml.CreateElement("language");
            //     language.InnerText = "Surprisingly enough but you can speak with the species of the same breed as long as they're not deaf. Oh, and so happens, you probably know an additional language of your choice.";
            //     info.AppendChild(language);

            //     XmlElement uniqueTraits = racesdotxml.CreateElement("uniqueTraits");
            //     for(int j = 0; j <= 3; j++){
            //         XmlElement tempEle = racesdotxml.CreateElement("trait");
            //         tempEle.InnerText = string.Empty;
            //         tempEle.SetAttribute("Name", "traitName");
            //         uniqueTraits.AppendChild(tempEle);
            //     }
            //     info.AppendChild(uniqueTraits);

            //     raceElement.AppendChild(info);

            //     XmlElement abils = racesdotxml.CreateElement("abilityScores");
            //     foreach(string s in abilityScores){
            //         XmlElement tempEle = racesdotxml.CreateElement(s);
            //         tempEle.InnerText = "+0";
            //         abils.AppendChild(tempEle);
            //     }
            //     raceElement.AppendChild(abils);

            //     for(int subNumber = 0; subNumber <= 1; subNumber++){
            //         XmlElement subrace = racesdotxml.CreateElement("subrace");
            //         subrace.SetAttribute("subraceName", "subracePlaceholder");

            //         XmlElement subraceInfo = racesdotxml.CreateElement("subraceInfo");
            //         subrace.AppendChild(subraceInfo);

            //         XmlElement subraceDesc = racesdotxml.CreateElement("description");
            //         subraceDesc.InnerText = "Your subrace is this bad.";
            //         subraceInfo.AppendChild(subraceDesc);

            //         XmlElement subUniqueTraits = racesdotxml.CreateElement("uniqueTraits");
            //         for(int j = 0; j <= 3; j++){
            //             XmlElement tempEle = racesdotxml.CreateElement("trait");
            //             tempEle.InnerText = string.Empty;
            //             tempEle.SetAttribute("Name", "traitName");
            //             subUniqueTraits.AppendChild(tempEle);
            //         }
            //         subraceInfo.AppendChild(subUniqueTraits);

            //         XmlElement subraceAbils = racesdotxml.CreateElement("abilityScores");
            //         foreach(string s in abilityScores){
            //             XmlElement tempEle = racesdotxml.CreateElement(s);
            //             tempEle.InnerText = "+0";
            //             subraceAbils.AppendChild(tempEle);
            //         }
            //         subrace.AppendChild(subraceAbils);

            //         raceElement.AppendChild(subrace);
            //     }

            //     racesRoot.AppendChild(raceElement);
            // }

            // racesdotxml.Save("CharacterRaces.xml");
            #endregion

            Character a = new Character("Cookie Monster", "cleric", "tiefling");
            Character b = new Character(
                name: "Johnny Wicked",
                className: "druid",
                raceName: "lightfoot"
            );

            var temp1 = ClassCatalog.GetClassFeatures("cleric");
            var temp2 = ClassCatalog.GetBarbarianRageInfo(8);
            a.GetSkills();
            System.Console.WriteLine($"Select {ClassCatalog.GetStartingSkillsAmount(a.ClassName)} : {ClassCatalog.GetStartingSkills(a.ClassName)}");
            string skills = Console.ReadLine();
            a.FillStartingSkills(skills.Replace(", ", ",").Split(','));
            System.Console.WriteLine($"Select {ClassCatalog.GetStartingSkillsAmount(b.ClassName)} : {ClassCatalog.GetStartingSkills(b.ClassName)}");
            skills = Console.ReadLine();
            b.FillStartingSkills(skills.Replace(", ", ",").Split(','));

            System.Console.WriteLine(a.Proficiencies);

            // System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Character));
            // var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//serializedCharacter.xml";
            // System.IO.FileStream stream = System.IO.File.Create(path);

            // xmlSerializer.Serialize(stream, a);
            // stream.Close();

            foreach(var t in System.Enum.GetValues(typeof (CharacterRaces))){
                System.Console.WriteLine($"{t.ToString().PadRight(20)} | {(int)t} | {((RaceType)((int)t & (int)RaceType.CoreWithoutSubraces)).ToString().PadRight(20)} | id {(int)t & 0b00_1111_1111}");
            }

            var temp = RaceCatalog.GetRaceXElement(CharacterRaces.HillDwarf);

            System.Console.WriteLine("end");
        }
    }
}
