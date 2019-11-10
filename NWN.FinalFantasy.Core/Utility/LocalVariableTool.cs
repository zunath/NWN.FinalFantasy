using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using Serilog;
using static NWN._;

namespace NWN.FinalFantasy.Core.Utility
{
    public class LocalVariableTool
    {
        /// <summary>
        /// Retrieves all local string variables on an object matching a given prefix.
        /// Returns a list containing the scripts to run sequentially.
        /// </summary>
        /// <param name="target">The object to pull variables from</param>
        /// <param name="prefix">The prefix to look for</param>
        /// <returns>A list of scripts to run, ordered from lowest to highest</returns>
        public static IEnumerable<string> FindByPrefix(NWGameObject target, string prefix)
        {
            var variableCount = NWNXObject.GetLocalVariableCount(target);
            var variableList = new SortedList<int, string>();

            for (int x = 0; x < variableCount; x++)
            {
                var variable = NWNXObject.GetLocalVariable(target, x);
                if (variable.Type == LocalVariableType.String &&
                    variable.Key.StartsWith(prefix))
                {
                    // If the rest of the variable key can be converted to an int, add it to the list.
                    var skipCharacters = prefix.Length;
                    var orderSubstring = variable.Key.Substring(skipCharacters);

                    if (!int.TryParse(orderSubstring, out var order))
                    {
                        // Couldn't parse an integer out of the key name. Move to the next variable.
                        continue;
                    }

                    // If the variable ID has already been assigned, skip to the next variable.
                    if (variableList.ContainsKey(order))
                    {
                        Log.Warning($"Variable '{prefix}' for ID {order} already exists. Ignoring second entry.");
                        continue;
                    }

                    // Add the script to the list.
                    var value = GetLocalString(target, variable.Key);
                    variableList.Add(order, value);
                }
            }

            return variableList.Values.ToList();
        }

        /// <summary>
        /// Finds an open ID number for a given script prefix.
        /// </summary>
        /// <param name="obj">The object to check variables on</param>
        /// <param name="prefix">The prefix to look for</param>
        /// <returns>An open ID number</returns>
        public static int GetOpenScriptID(NWGameObject obj, string prefix)
        {
            int id = 1;

            while (!string.IsNullOrWhiteSpace(GetLocalString(obj, prefix + id)))
            {
                id++;
            }

            return id;
        }
    }
}
