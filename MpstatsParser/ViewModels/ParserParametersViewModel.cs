using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Windows.Forms;


namespace MpstatsParser.ViewModels
{
    public class ParserParametersViewModel : INotifyPropertyChanged
    {

        public ParserParametersViewModel()
        {
            var parameters = Models.ParserParameters.GetParserParameters();
            this.APIKey = parameters.APIKey;
            this.ResultsFilePath = parameters.FileResultPath;
            this.SkuPriceFrom = parameters.SKUPriceFrom;

        }

        private string aPIKey;
        public string APIKey
        {
            get { return aPIKey; }
            set
            {
                aPIKey = value;
                OnPropertyChanged("APIKey");
            }
        }
        private string resultsFilePath;
        public string ResultsFilePath
        {
            get { return resultsFilePath; }
            set
            {
                resultsFilePath = value;
                OnPropertyChanged("ResultsFilePath");
            }
        }
        private double skuPriceFrom;
        public double SkuPriceFrom
        {
            get { return skuPriceFrom; }
            set
            {
                skuPriceFrom = value;
                OnPropertyChanged("SkuPriceFrom");
            }
        }



        private RelayCommand selectResultsFilepath;
        public RelayCommand SelectResultsFilepath
        {
            get
            {
                return selectResultsFilepath ??
                    (selectResultsFilepath = new RelayCommand(obj =>
                    {
                        FolderBrowserDialog dialog = new FolderBrowserDialog();
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            ResultsFilePath = dialog.SelectedPath;
                        }
                    }));
            }
        }

        private RelayCommand selectApiKey;
        public RelayCommand SelectApiKey
        {
            get
            {
                return selectApiKey ??
                    (selectApiKey = new RelayCommand(obj =>
                    {
                        Views.SelectAPIKey f = new Views.SelectAPIKey();
                        f.Show();
                    }));
            }
        }
        private RelayCommand saveChanges;
        public RelayCommand SaveChanges
        {
            get
            {
                return saveChanges ??
                    (saveChanges = new RelayCommand(obj =>
                    {
                        try
                        {
                            Models.ParserParameters.Params.SKUPriceFrom = SkuPriceFrom;
                            Models.ParserParameters.Params.FileResultPath = ResultsFilePath;
                            Models.ParserParameters.SaveParameters();
                        }
                        catch
                        {
                            System.Windows.MessageBox.Show("Не удалось сохранить. Неверно заполненные поля");
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
