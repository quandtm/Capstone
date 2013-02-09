using System.Threading.Tasks;

namespace Capstone.Editor.Views
{
    public interface IView
    {
        Task HandleNavigationTo(object parameter);
        Task HandleNavigationFrom();
    }
}
