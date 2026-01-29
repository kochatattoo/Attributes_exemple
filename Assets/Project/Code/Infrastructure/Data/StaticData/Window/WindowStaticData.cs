using System.Collections.Generic;
using UnityEngine;

namespace Code.Infrastructure.Data.StaticData.Window
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/Window static data")]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs;
    }
}
