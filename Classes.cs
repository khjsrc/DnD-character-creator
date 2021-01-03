using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace CharacterSheet{
    static class Classes
    {
        //move as much of this shit as possible to the CharacterClass class
        private static readonly string classesFilePath = @"CharacterClasses.xml";
        private static Dictionary<string, byte> classes = new Dictionary<string, byte>(){
            {"barbarian", 1},
            {"bard", 2},
            {"cleric", 3},
            {"druid", 4},
            {"fighter", 5},
            {"monk", 6},
            {"paladin", 7},
            {"ranger", 8},
            {"rogue", 9},
            {"sorcerer", 10},
            {"warlock", 11},
            {"wizard", 12}
        };

        static public List<string> ClassesList
        {
            get
            {
                return new List<string>(classes.Keys);
            }
            set
            {

            }
        }

        static public byte GetClassID(string name)
        {
            try
            {
                return classes[name.ToLower()];
            }
            catch (KeyNotFoundException ex)
            {
                return 255;
            }
        }

        public static string GetClassDescription(string name)//initially that was GetClassInfo returning CharacterClass
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(classesFilePath);
            XmlNode node = xmlDocument.SelectSingleNode($"/characterClasses/class[@Name='{name}']");
            string desc = node.SelectSingleNode($"/class/description").InnerText;
            return desc;
        }
    }
}