using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Job
{
    internal static class XPChart
    {
        private static readonly Dictionary<int, int> _xpAtLevel = new Dictionary<int, int>();
        private static int _levelCount;

        /// <summary>
        /// Reads the exptable.2da file and stores its values into memory for quicker lookups later.
        /// </summary>
        public static void Register()
        {
            const string FileName = "exptable";

            var rows = NWNXUtil.Get2DARowCount(FileName);

            for (int x = 0; x <= rows - 1; x++)
            {
                string value = _.Get2DAString(FileName, "XP", x);
                if (value == "0xFFFFFF") continue;

                int level = Convert.ToInt32(_.Get2DAString(FileName, "Level", x));
                _xpAtLevel[level] = Convert.ToInt32(value);
                _levelCount++;
            }
        }

        /// <summary>
        /// Retrieves the amount of XP needed by a specified level.
        /// </summary>
        /// <param name="level">The level to retrieve</param>
        /// <returns>The amount of XP needed for a specific level</returns>
        public static int GetByLevel(int level)
        {
            if (!_xpAtLevel.ContainsKey(level))
            {
                throw new Exception($"Level {level} is not registered in the XPChart.");
            }

            return _xpAtLevel[level];
        }

        /// <summary>
        /// Retrieves the level based on a specified XP amount.
        /// </summary>
        /// <param name="xp">The amount to XP to check</param>
        /// <returns>The level, based on the XP given</returns>
        public static int GetLevelByXP(int xp)
        {
            int level = 0;
            for (int x = 1; x <= _levelCount; x++)
            {
                int levelXP = _xpAtLevel[x];

                if (levelXP > xp)
                    break;

                level++;
            }

            return level;
        }
    }
}
