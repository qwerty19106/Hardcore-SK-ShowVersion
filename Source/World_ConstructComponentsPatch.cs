using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShowHSKVersion
{
    [HarmonyPatch(typeof(World), nameof(World.ConstructComponents))]
    class World_ConstructComponentsPatch
    {
        [HarmonyPostfix]
        static void AfterConstructComponents()
        {
            UpdateSeasonsCached();
        }

        public static void UpdateSeasonsCached()
        {
            if (!(HskLauncher.ConfigExists && HskLauncher.ConfigLoaded && HskLauncher.LastInstalledVersionSha != null))
                return;

            DateReadout.Reset();
            //Log.Warning($"ShowHSKVersion: Settings.ShowVersion {Settings.ShowVersion}");
            //Log.Warning($"ShowHSKVersion: HskLauncher.ConfigLoaded {HskLauncher.ConfigLoaded}");

            if (Settings.ShowVersion)
            {
                var SeasonsCachedField = typeof(DateReadout).GetField("seasonsCached", BindingFlags.NonPublic | BindingFlags.Static);
                if (SeasonsCachedField == null)
                {
                    Log.Error("ShowHSKVersion: can not get DateReadout.SeasonsCached field by reflection.");
                    return;
                }

                List<string> SeasonsCached = (List<string>)SeasonsCachedField.GetValue(null);
                var SeasonsCachedCopy = new List<string>(SeasonsCached);
                SeasonsCached.Clear();

                // Add first 7 chars of SHA to each season
                var sha_short = HskLauncher.LastInstalledVersionSha.Substring(0, 7);

                /*string branch = "";
                switch (HskLauncher.SelectedBranch)
                {
                    case HskLauncher.Branch.Stable:
                        branch = "stable";
                        break;
                    case HskLauncher.Branch.Development:
                        branch = "dev";
                        break;
                }*/

                
                /*int length = Enum.GetValues(typeof(Season)).Length;
                for (int j = 0; j < length; j++)
                {
                    Season season = (Season)j;

                    if (Settings.ShowVersion)
                        SeasonsCached.Add((season == Season.Undefined) ? "" : season.LabelCap() + $" ({sha_short})");
                    else
                        SeasonsCached.Add((season == Season.Undefined) ? "" : season.LabelCap() + $" ({sha_short})");
                }*/

                foreach (var season in SeasonsCachedCopy)
                {
                    //SeasonsCached.Add(season + $" ({sha_short}, {branch})");
                    SeasonsCached.Add(season + $" ({sha_short})");
                }
            }

        }
    }
}
