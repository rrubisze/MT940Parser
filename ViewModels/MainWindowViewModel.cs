using MT940Parser.Commands;
using MT940Parser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MT940Parser.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        private readonly Mt940Service _mt940Service;

        public CustomCommand<string[]> DropFileCommand { get; }

        public MainWindowViewModel(Mt940Service mt940Service)
        {
            _mt940Service = mt940Service;
            this.DropFileCommand = new CustomCommand<string[]>(DropFile);
        }

        private async Task DropFile(string[] files)
        {
            if (files.Count() > 1)
            {
                //this.Info.Content = "You can drop only one file";
                return;
            }

            try
            {
                if (System.IO.Path.GetExtension(files[0]) != ".sta")
                {
                    //this.Info.Content = "Wrong file extension. It must be .sta!";
                    return;
                }
                await _mt940Service.ProcessCsvFile(files[0]);
                //this.Info.Content = "";
            }
            catch (Exception ex)
            {
                //this.Info.Content = ex.Message;
                return;
            }
        }
    }
}
