using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace SeleniumAutotest
{
    internal static class ValuesFromParameters
    {
        private static Random Random { get; set; } = new Random();

        public static string ProcessInput(string input, List<Parameter> ProjectParameters, List<Parameter> TestParameters)
        {
            if (input.Contains("/null/"))
                return null;

            input = ReplaceParameters(input, ProjectParameters, 3);

            if (TestParameters != null)
            {
                input = ReplaceParameters(input, TestParameters, 3);
            }

            string patternD = @"/randomD(\d+)/";
            string patternL = @"/randomL(\d+)/";
            string patternC = @"/randomC(\d+)/";

            input = Regex.Replace(input, patternD, match => GenerateRandomDigits(int.Parse(match.Groups[1].Value)));
            input = Regex.Replace(input, patternL, match => GenerateRandomLetters(int.Parse(match.Groups[1].Value)));
            input = Regex.Replace(input, patternC, match => GenerateRandomChars(int.Parse(match.Groups[1].Value)));

            input = ReplaceAndEvaluateExpressions(input);

            return input;
        }

        static string ReplaceAndEvaluateExpressions(string input)
        {
            string pattern = @"\^(.+?)\^";

            var scriptOptions = ScriptOptions.Default
                .WithReferences(AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location)))
                .WithImports("System", "System.Linq", "System.Collections.Generic");

            return Regex.Replace(input, pattern, match =>
            {
                string code = match.Groups[1].Value;

                try
                {
                    var result = CSharpScript.EvaluateAsync<string>(code, scriptOptions).Result;
                    return result;
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            });
        }

        static string ReplaceParameters(string input, List<Parameter> parameters, int maxDepth)
        {
            if (maxDepth <= 0)
            {
                return input;
            }

            string pattern = @"%(\w+)%";
            return Regex.Replace(input, pattern, match =>
            {
                string key = match.Groups[1].Value;
                var parameter = parameters.Find(p => p.Name == key);
                if (parameter != null)
                {
                    if (parameter.Value == null)
                    {
                        parameter.Value = ReplaceParameters(parameter.Pattern, parameters, maxDepth - 1);
                    }
                    return parameter.Value;
                }
                return match.Value; // Если параметр не найден, не заменяем его
            });
        }

        static string GenerateRandomDigits(int length)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(Random.Next(0, 10));
            }
            return result.ToString();
        }

        static string GenerateRandomLetters(int length)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(letters[Random.Next(letters.Length)]);
            }
            return result.ToString();
        }

        static string GenerateRandomChars(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[Random.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }
}
