using Code.Infrastructure.Data.StaticData.Window;
using Code.UI.Windows;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Infrastructure.Data.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataWindowsPath = "StaticData/ui/WindowStaticData";

        private Dictionary<WindowID, WindowConfig> _windowConfigs;

        public void Load()
        {
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public WindowConfig ForWindow(WindowID windowId) =>
             _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
                ? windowConfig
                : null;
    }
}
