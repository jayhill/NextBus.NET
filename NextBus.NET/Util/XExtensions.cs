﻿namespace NextBus.NET.Util
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

        public static string GetElementValue(this XElement element, string name)
        {
            return element == null ? null : element.Value;
        }

        public static T GetElementValue<T>(this XElement element, string name, Func<string, T> conversion)
        {
            var value = GetElementValue(element, name);
            if (value == Null.OrEmpty)
                return default(T);

            try
            {
                return conversion(value);
            }
            catch (Exception ex)
            {
                throw new XmlParseException(string.Format(
                    "Failed to convert element value [{0}] to [{1}].",
                    element.Value, typeof(T).Name), ex);
            }
        }
    }
}