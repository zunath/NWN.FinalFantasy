using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class JukeboxDialog: DialogBase
    {
        private static readonly HashSet<Song> _songs = new HashSet<Song>();
        private const string MainPageId = "MAIN_PAGE";

        private class Song
        {
            public int ID { get; }
            public string DisplayName { get; }

            public Song(int id, string displayName)
            {
                ID = id;
                DisplayName = displayName;
            }
        }

        /// <summary>
        /// When the module loads, read the ambientmusic.2da file for all active songs.
        /// Add these to the cache.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void LoadSongList()
        {
            const string File = "ambientmusic";
            int rowCount = Util.Get2DARowCount(File);

            for (int row = 0; row < rowCount; row++)
            {
                string description = Get2DAString(File, "Description", row);
                string resource = Get2DAString(File, "Resource", row);
                string displayName = Get2DAString(File, "DisplayName", row);

                // Skip record if a name cannot be determined.
                if (string.IsNullOrWhiteSpace(description) &&
                    string.IsNullOrWhiteSpace(displayName)) continue;

                string name = string.IsNullOrWhiteSpace(description) ?
                    displayName :
                    GetStringByStrRef(Convert.ToInt32(description));

                _songs.Add(new Song(row, name));
            }
        }

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .AddPage(MainPageId, (page) =>
                {
                    page.Header = "Please select a song.";

                    foreach (var song in _songs)
                    {
                        page.AddResponse(song.DisplayName, () =>
                        {
                            var area = GetArea(player);
                            FloatingTextStringOnCreature($"Song Selected: {song.DisplayName}", player, false);

                            MusicBackgroundChangeDay(area, song.ID);
                            MusicBackgroundChangeNight(area, song.ID);
                            MusicBackgroundPlay(area);
                        });
                    }

                });

            return builder.Build();
        }
    }
}
