using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfases
{
    public interface IView
    {
        void RefreshHeroesList();
        void UpdateStatusBar(string status);
        void ShowMessage(string message, string title = "Информация");
        void ShowError(string error, string title = "Ошибка");
    }
}
