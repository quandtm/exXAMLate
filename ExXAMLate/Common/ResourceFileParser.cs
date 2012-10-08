using System.Collections.Generic;
using System.Xml.Linq;

namespace ExXAMLate.Common
{
    public static class ResourceFileParser
    {
        public static IList<string> GetKeysBasedOn(string doc, string basedOnKey)
        {
            return GetKeys(XDocument.Load(doc), "BasedOn", @"{StaticResource " + basedOnKey + "}");
        }

        public static IList<string> GetKeysTargeting(string doc, string target)
        {
            return GetKeys(XDocument.Load(doc), "TargetType", target);
        }

        public static IList<string> GetKeys(XDocument doc, string attribute, string value)
        {
            var items = new List<string>();
            foreach (var style in doc.Descendants(Hammer.Pants.ResourceFileParser.XamlPresentationNamespace + "Style"))
            {
                var key = style.Attribute(Hammer.Pants.ResourceFileParser.XamlRootNamespace + "Key");
                if (key == null || key.Value == null)
                    continue;

                var t = style.Attribute(attribute);
                if (t != null && t.Value == value)
                    items.Add(key.Value);
            }

            return items;
        }
    }
}