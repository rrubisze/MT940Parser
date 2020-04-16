using MT940Parser.Commands;
using MT940Parser.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MT940Parser.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        private readonly Mt940Service _mt940Service;

        public CustomCommand<object> DropFileCommand { get; }

        public MainWindowViewModel(Mt940Service mt940Service)
        {
            _mt940Service = mt940Service;
            this.DropFileCommand = new CustomCommand<object>(DropFile);
        }

        private async Task DropFile(object arg)
        {
            throw new NotImplementedException();
        }
    }
}
