using Code.UI.Windows;
using System;

namespace Code.Infrastructure.Data.StaticData.Window
{
    [Serializable]
    public class WindowConfig
    {
        public WindowID WindowId;
        public WindowBase prefab;
    }
}
