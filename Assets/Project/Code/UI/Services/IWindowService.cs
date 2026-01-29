using Code.Infrastructure.Services;
using Code.UI.Windows;

namespace Code.UI.Services
{
    public interface IWindowService: IService
    {
        void Open(WindowID windowId);
    }
}
