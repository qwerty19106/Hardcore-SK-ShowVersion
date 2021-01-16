using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShowHSKVersion
{
    [StaticConstructorOnStartup]
    public static class HskLauncher
    {
        public enum Branch
        {
            Stable,
            Development
        }

        const string ConfigPath = @"%USERPROFILE%\AppData\Roaming\HSK-Launcher\config.xml";

        public static string LastInstalledVersion { get; private set; }
        public static string LastInstalledVersionSha { get; private set; }
        public static Branch? SelectedBranch { get; private set; }

        public static bool ConfigExists { get; private set; }
        public static bool ConfigLoaded { get; private set; }

        static HskLauncher()
        {
            var filePath = Environment.ExpandEnvironmentVariables(ConfigPath);
            ConfigExists = File.Exists(filePath);
            ConfigLoaded = false;

            if (!ConfigExists)
                return;

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(filePath);

                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode childnode in xRoot.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "LastInstalledVersion":
                            LastInstalledVersion = childnode.InnerText;
                            break;
                        case "LastInstalledVersionSha":
                            LastInstalledVersionSha = childnode.InnerText;
                            break;
                        case "SelectedBranch":
                            switch (childnode.InnerText)
                            {
                                case "Stable":
                                    SelectedBranch = Branch.Stable;
                                    break;
                                case "Development":
                                    SelectedBranch = Branch.Development;
                                    break;
                                default:
                                    Log.Error($"ShowHSKVersion: unknown SelectedBranch {childnode.InnerText}");
                                    return;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ShowHSKVersion: error on loading HskLauncher config: {ex}");
                return;
            }

            ConfigLoaded = true;
        }
    }
}
