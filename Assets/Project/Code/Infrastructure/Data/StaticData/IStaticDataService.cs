using Code.Infrastructure.Data.StaticData.Window;
using Code.Infrastructure.Services;
using Code.UI.Windows;

namespace Code.Infrastructure.Data.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        WindowConfig ForWindow(WindowID window);
    }
}
