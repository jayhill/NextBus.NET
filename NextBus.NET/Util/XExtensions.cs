namespace NextBus.NET.Util
{
    using System;
    using System.Xml.Linq;
    using Infrastructure;

    public static class XExtensions
    {
        public static string GetAttributeValue(this XElement element, string name)
        {
            if (element == null)
                return null;

            var attribute = element.Attribute(name);
            return attribute == null ? null : attribute.Value;
        }

        public static T GetAttributeValue<T>(this XElement element, string name, Func<string,T> conversion)
        {
            if (element == null)
                return default(T);

            var attribute = element.Attribute(name);
            if (attribute == null)
                return default(T);

            try
            {
                return conversion(attribute.Value);
            }
            catch (Exception ex)
            {
                throw new XmlParseException(string.Format(
                    "Failed to convert attribute value [{0}] to [{1}].",
                    attribute.Value, typeof(T).Name), ex);
            }
        }
    }
}