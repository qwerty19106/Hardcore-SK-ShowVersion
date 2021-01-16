using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ShowHSKVersion
{
    public class ShowHSKVersionMod : Mod
    {
        public ShowHSKVersionMod(ModContentPack content) : base(content)
        {
            Harmony harmonyInstance = new Harmony("ShowHSKVersion");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            this.GetSettings<Settings>();
        }
        public override string SettingsCategory()
        {
            return "Show HSK Version";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard(GameFont.Small)
            {
                ColumnWidth = inRect.width
            };
            list.Begin(inRect);

            if (!HskLauncher.ConfigExists)
            {
                list.Label("ERROR: HardcoreSK launcher is not installed. Build version will not be displayed.");
            }
            else if (!HskLauncher.ConfigLoaded)
            {
                list.Label("ERROR: Failed to load launcher config. Build version will not be displayed.");
            }
            else if (HskLauncher.LastInstalledVersionSha == null)
            {
                list.Label("ERROR: You use old version of the HardcoreSK launcher. Upgrade the HardcoreSK launcher to display build version.");
            }
            else
            {
                bool ShowVersion = Settings.ShowVersion;

                list.CheckboxLabeled(
                "Show build version in season string",
                ref ShowVersion,
                "Build version will be showed in season string, for example \"Winter (abcd123)\".");

                list.Label("Enable it before sending screenshot to HardcoreSK Project Team as bug report.");


                if (ShowVersion != Settings.ShowVersion)
                {
                    Settings.ShowVersion = ShowVersion;
                    World_ConstructComponentsPatch.UpdateSeasonsCached();
                }
            }

            list.End();

            
        }
    }
}
