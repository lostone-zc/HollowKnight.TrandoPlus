using Newtonsoft.Json;
using Modding;
using RandomizerMod.Logging;
using System;
using System.IO;

namespace TrandoPlus
{
    public class TrandoPlus : Mod, IGlobalSettings<GlobalSettings>
    {
        internal static TrandoPlus instance;

        public static GlobalSettings GS = new();
        public void OnLoadGlobal(GlobalSettings s) => GS = s;
        public GlobalSettings OnSaveGlobal() => GS;

        public TrandoPlus() : base(null)
        {
            instance = this;
        }
        
        public override string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
        
        public override void Initialize()
        {
            Log("Initializing Mod...");
            bool rando = ModHooks.GetMod("Randomizer 4") is Mod;

            if (rando) MenuHolder.Hook();
            if (rando) DoorRandoAdder.Hook();
            if (rando) DropRandoAdder.Hook();
            if (rando) ConditionManager.Hook();

            if (rando) HookSettingsLog();
        }

        private void HookSettingsLog()
        {
            SettingsLog.AfterLogSettings += AddSettingsToLog;
        }

        private void AddSettingsToLog(LogArguments args, TextWriter tw)
        {
            tw.WriteLine("Logging TrandoPlus settings:");
            using JsonTextWriter jtw = new(tw) { CloseOutput = false, };
            RandomizerMod.RandomizerData.JsonUtil._js.Serialize(jtw, GS);
            tw.WriteLine();
        }
    }
}