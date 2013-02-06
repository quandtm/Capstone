using System.Threading.Tasks;

namespace Capstone.Editor.Views
{
    public interface IView
    {
        void HandleNavigationTo(object parameter);
        Task HandleNavigationFrom();
    }
}
