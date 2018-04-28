using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Workflow.Engine.Services.Action.Dto;

namespace Workflow.Engine.Services.Action.Utils
{
    public static class Util
    {
        public static T GetParameter<T>(string actionParameter, string propertyName, T defaultValue)
        {
            if (TryParse(actionParameter, out JObject input))
            {
                var result = input[propertyName];
                if (result != null)
                {
                    return result.ToObject<T>();
                }
            }
            return defaultValue;
        }

        public static T AutoBindInput<T>(JObject entity, string parameters, string propertyName, string defaultPropertyName)
        {
            try
            {
                var attributeName = GetParameter<string>(parameters, propertyName, null);

                if (string.IsNullOrWhiteSpace(attributeName))
                {
                    return entity[defaultPropertyName].Value<T>();
                }

                var value = FindAutoMapExpression(attributeName, entity);

                if (string.IsNullOrWhiteSpace(value))
                {
                    return entity[defaultPropertyName].Value<T>();
                }

                return entity[value].Value<T>();
            }
            catch { }
            return default(T);
        }

        public static bool TryParse(string value, out JObject obj)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                obj = null;
                return false;
            }
            value = value.Trim();
            if ((value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]")))
            {
                try
                {
                    obj = JObject.Parse(value);
                    return true;
                }
                catch
                {
                    obj = null;
                    return false;
                }
            }
            obj = null;
            return false;
        }

        public static bool TryDeserializeObject<T>(string value, out T result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default(T);

                return false;
            }

            value = value.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || (value.StartsWith("[") && value.EndsWith("]")))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<T>(value);

                    return true;
                }
                catch
                {
                    result = default(T);

                    return false;
                }
            }

            result = default(T);

            return false;
        }

        public static void AddOrUpdateAttribute(JObject entity, string attributeName, object attributeValue)
        {
            entity[attributeName] = JToken.FromObject(attributeValue);
        }

        public static string FindAutoMapExpression(string expression, JObject entity)
        {
            return Regex.Replace(expression, @"\$([a-zA-Z0-9]+\.?)+", match =>
            {
                var attributeName = match.Value.Replace("$", string.Empty);
                var attributeValue = entity.SelectToken(attributeName);
                return match.Value.Replace("$" + attributeName, attributeValue != null ? attributeValue.ToString() : string.Empty);
            });
        }

        public static StringBuilder ManuallyFillTemplate(StringBuilder template, IEnumerable<ReplaceDtoInput> placeholders, JObject entity)
        {
            if (template == null || placeholders == null || entity == null)
            {
                return template;
            }
            foreach (var placeholder in placeholders)
            {
                if (string.IsNullOrWhiteSpace(placeholder.From))
                {
                    continue;
                }
                var valueFromEntity = FindAutoMapExpression(placeholder.To, entity);
                if (string.IsNullOrWhiteSpace(valueFromEntity))
                {
                    template.Replace(placeholder.From, placeholder.To);
                }
                else
                {
                    template.Replace(placeholder.From, valueFromEntity);
                }
            }
            return template;
        }

        public static StringBuilder AutoFillTemplate(StringBuilder template, JObject entity)
        {
            if (template == null || entity == null)
            {
                return template;
            }
            var placeholders = Regex.Matches(template.ToString(), @"\{(.*?)\}").Cast<Match>().Select(match => match.Value).ToList();
            var property = entity["RemoveUnmatched"];
            var removeUnmatched = property != null ? property.Value<bool>() : false;
            foreach (var placeholder in placeholders)
            {
                var propertyName = NormalizePlaceholder(placeholder);
                var propertyValue = entity[propertyName];
                if (propertyValue == null)
                {
                    if (removeUnmatched)
                    {
                        template.Replace(placeholder, string.Empty);
                    }

                    continue;
                }
                template.Replace(placeholder, propertyValue.ToString());
            }
            return template;
        }

        public static string AutoFillIteratorsTemplate(StringBuilder template, JToken entity)
        {
            if (template == null || entity == null)
            {
                return template.ToString();
            }
            var iterators = Regex.Matches(template.ToString(), @"<!-- iterator -->[a-zA-Z0-9\D]+<!-- iterator -->");
            foreach (Match iterator in iterators)
            {
                var iteratorHtmlBody = iterator.Value;
                iteratorHtmlBody = iteratorHtmlBody.Replace("<!-- iterator -->", string.Empty);
                var source = entity.ToArray<JToken>();
                var allPlaceholders = Regex.Matches(iteratorHtmlBody, @"\{(.*?)\}").Cast<Match>().Select(match => match.Value).ToList();
                var allData = string.Empty;
                var test = 0;
                foreach (var sourceItem in source)
                {
                    test++;
                    var result = iteratorHtmlBody;
                    foreach (var placeholder in allPlaceholders)
                    {
                        var propertyName = NormalizePlaceholder(placeholder);
                        var propertyValue = sourceItem[propertyName];
                        if (propertyValue == null)
                        {
                            result = result.Replace(placeholder, string.Empty);
                            continue;
                        }
                        result = result.Replace(placeholder, Convert.ToString(propertyValue));
                    }
                    if (test == 20)
                    {
                        test = 0;
                        result = result.Replace("<tr>", "<tr style=\"page-break-after:always;\">");
                    }
                    allData += result;
                }
                template.Replace(iterator.Value, allData);
            }
            return template.ToString();
        }

        public static string NormalizePlaceholder(string placeholder)
        {
            placeholder = placeholder.ToLowerInvariant();
            placeholder = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(placeholder);
            placeholder = placeholder.Replace("{", string.Empty);
            placeholder = placeholder.Replace("}", string.Empty);
            placeholder = placeholder.Replace("_", string.Empty);
            placeholder = placeholder.Replace(" ", string.Empty);
            return placeholder;
        }
    }
}
