using Microsoft.Win32;
using MT940Parser.Commands;
using MT940Parser.Context;
using MT940Parser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT940Parser.ViewModels
{
    public class ImportViewModel: BaseViewModel
    {   
        private readonly Mt940Service _mt940Service;
        private readonly ParserContext _context;
        private string _errorMessage;
        private int _importProgress;
        private string _filePath;
        private bool _isImportFinish;
        private bool _isParsingError;

        #region - Commands -
        public CustomCommand<string[]> DropFileCommand { get; }
        public CustomCommand ChooseFileCommand { get; }
        public CustomCommand ClearCommand { get; }
        #endregion
        #region - Props -
        public string ErrorMessage 
        { 
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }
        public int ImportProgress
        {
            get => _importProgress;
            set => Set(ref _importProgress, value);
        }
        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value);
        }
        public bool IsImportFinish
        {
            get => _isImportFinish;
            set => Set(ref _isImportFinish, value);
        }
        public bool IsParsingError
        {
            get => _isParsingError;
            set => Set(ref _isParsingError, value);
        }

        #endregion

        public ImportViewModel(Mt940Service mt940Service, ParserContext context)
        {
            _mt940Service = mt940Service;
            _context = context;
            this.DropFileCommand = new CustomCommand<string[]>(DropFileAsync);
            this.ChooseFileCommand = new CustomCommand(ChooseFileAsync);
            this.ClearCommand = new CustomCommand(Clear);

            FilePath = "";
            ImportProgress = 0;
        }

        private async Task ChooseFileAsync()
        {
            IsImportFinish = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "STA files (*.sta)|*.sta|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                await ProcessMT940FileAsync(openFileDialog.FileName);
            }
            else
            {
                ErrorMessage = "No file";
            }
        }

        private Task Clear()
        {
            FilePath = "";
            IsParsingError = false;
            IsImportFinish = false;
            ImportProgress = 0;
            _context["Reports"] = null;

            return Task.CompletedTask;
        }

        private async Task DropFileAsync(string[] files)
        {
            IsImportFinish = false;
            if (files.Count() > 1)
            {
                IsImportFinish = true;
                ErrorMessage = "You can drop only one file";
                IsParsingError = true;
                return;
            }
            var file = files[0];
            await ProcessMT940FileAsync(file);
        }

        private async Task ProcessMT940FileAsync(string filePath)
        {
            try
            {
                ImportProgress = 0;

                if (Path.GetExtension(filePath) != ".sta")
                {
                    ErrorMessage = "Wrong file extension. It must be .sta!";
                    IsParsingError = true;
                    return;
                }
                FilePath = filePath;
                var statemenets = _mt940Service.GenerateStatements(filePath);
                ImportProgress = 33;

                var reports = _mt940Service.GenerateReports(statemenets);
                ImportProgress = 66;
                _context["Reports"] = reports;

                var output = Path.ChangeExtension(filePath, "csv");
                await _mt940Service.GenerateReportCSV(reports, output);
                ImportProgress = 100;

                ErrorMessage = "";
                IsParsingError = false;
                IsImportFinish = true;
            }
            catch (Exception ex)
            {
                IsParsingError = true;
                ErrorMessage = ex.Message;
                return;
            }
        }
    }
}
