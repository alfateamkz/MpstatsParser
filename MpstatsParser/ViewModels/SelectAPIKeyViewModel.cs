using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MpstatsParser.ViewModels
{
    public class SelectAPIKeyViewModel : INotifyPropertyChanged
    {
        public SelectAPIKeyViewModel()
        {
            var parameters = Models.ParserParameters.GetParserParameters();
            this.APIKey = parameters.APIKey;
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

        private RelayCommand saveApiKey;
        public RelayCommand SaveApiKey
        {
            get
            {
                return saveApiKey ??
                    (saveApiKey = new RelayCommand(obj =>
                    {
                        if (!string.IsNullOrEmpty(APIKey))
                        {
                            Models.ParserParameters.Params.APIKey = APIKey;
                            Models.ParserParameters.SaveParameters();
                            MessageBox.Show("API ключ успешно сохранён");
                        }
                        else
                        {
                            MessageBox.Show("Нельзя сохранить пустой ключ");
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
