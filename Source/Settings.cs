using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShowHSKVersion
{
    public class Settings : ModSettings
    {

        public static bool ShowVersion = true;


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref ShowVersion, nameof(ShowVersion), false);
        }
    }
}
