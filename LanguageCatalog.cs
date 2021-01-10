using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace CharacterSheet
{
    static class LanguageCatalog
    {
        private static readonly string _languagesXmlFileName = "Languages.xml";

        public static XElement GetLanguageXElement(string languageName)
        {
            XDocument xdoc = XDocument.Load(_languagesXmlFileName);
            var lang =
                (from langEle in xdoc.Descendants("language")
                 where langEle.Attribute("name").Value.ToLower() != null
                 where langEle.Attribute("name").Value.ToLower().Contains(languageName)
                 select langEle).FirstOrDefault();
            return lang;
        }

        public static string GetLanguageFullName(string languageName)
        {
            return GetLanguageXElement(languageName)?.Attribute("name").Value;
        }

        public static string GetLanguageType(string languageName)
        {
            return GetLanguageXElement(languageName)?.Attribute("type").Value;
        }

        public static string GetLanguageScript(string languageName)
        {
            return GetLanguageXElement(languageName)?.Attribute("script").Value;
        }

        public static string GetLanguageDescription(string languageName)
        {
            return GetLanguageXElement(languageName)?.Value;
        }
    }
}