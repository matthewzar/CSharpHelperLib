using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollectionOfHelpers.StringExtensions
{
    public static class StringFormatting
    {
        /// <summary>
        /// Returns a version of the string where all duplicated spaces are reduced to a single space.
        /// For example "Hello     I'm  Jim" become "Hello I'm Jim"
        /// </summary>
        /// <param name="toFormat"></param>
        /// <returns></returns>
        public static string RemoveDuplicateSpaces(this string toFormat)
        {
            while (toFormat.Contains("  "))
            {
                toFormat = toFormat.Replace("  ", " ");
            }
            return toFormat;
        }

        /// <summary>
        /// Returns a string where Case-seperated words become serperated by spaces.
        /// For example "ASimpleExample" becomes "A Simple Example".
        /// Also handles acronys such as "IBMWatson" becomes "IBM Watson"
        /// Consecutive acronyms, plural acronyms, and SingleLetter-Acronym pairs are difficult to differentiate, 
        /// eg: "BuyAnIBMPC", "PCsRule", and "APLCCompany" can't be parsed correctly.
        /// </summary>
        /// <param name="toFormat"></param>
        /// <returns></returns>
        public static string RemoveCamelCase(this string toFormat)
        {
            //https://stackoverflow.com/questions/155303/net-how-can-you-split-a-caps-delimited-string-into-an-array
            return Regex.Replace(toFormat, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ", RegexOptions.Compiled);
        }
        
        //TODO - test whether this version of the camel case splitting regex is more/less efficient
        //(and what cases it covers) compared to the above one.
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
        
        /// <summary>
        /// Removes all instances of subString from mainString.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Remove(this string mainString, string subString)
        {
            return mainString.Replace(subString, "");
        }
    }
}
