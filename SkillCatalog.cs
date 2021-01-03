using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CharacterSheet
{
    static class SkillCatalog
    {
        private static readonly string _skillsXmlFileName = @"ClassSkills.xml";

        public static XElement GetSkillXElement(string skillName){
            XDocument xdoc = XDocument.Load(_skillsXmlFileName);
            skillName = skillName.ToLower();
            var skill = 
                (from ele in xdoc.Descendants("skill")
                where ele.Attribute("name") != null
                where ele.Attribute("name").Value.ToLower().Contains(skillName)
                select ele).FirstOrDefault();
            if(skill == null) throw new SkillNotFoundException($"The specified skill - {skillName} - is not found.");
            return skill;
        }

        public static string[] GetSkills(){
            XDocument xdoc = XDocument.Load(_skillsXmlFileName);
            var skills = 
                from ele in xdoc.Descendants("skill")
                select ele.Attribute("name").Value;
            return skills.ToArray();
        }

        public static string GetSkillFullName(string skillName){
            return GetSkillXElement(skillName)?.Attribute("name").Value;
        }

        public static string GetSkillDescription(string skillName)
        {
            return GetSkillXElement(skillName)?.Value;
        }

        public static string GetSkillRelatedAbility(string skillName){
            return GetSkillXElement(skillName)?.Attribute("ability").Value;
        }
    }

    [System.Serializable]
    public class SkillNotFoundException : System.Exception
    {
        public SkillNotFoundException() { }
        public SkillNotFoundException(string message) : base(message) { }
        public SkillNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected SkillNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}