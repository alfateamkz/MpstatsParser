using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MpstatsParser.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            var parameters = Models.ParserParameters.GetParserParameters();
            if (parameters.Categories?.Count > 0)
            {
                this.Progress = (parameters.CurrentCategoryIndex + 1) / ((double)parameters.Categories?.Count);
            }
            
            this.CurrentAction = "";
            if (parameters.IsStarted)
            {
                this.ParserStatusText = "Работает";
                this.ParserPauseButtonText = "Приостановить";
                this.ParserStartStopButtonText = "Остановить";
                this.ParserPauseButtonAccessibility = true;
                if (parameters.IsSuspended)
                {
                    this.ParserStatusText = "Приостановлен";
                    this.ParserPauseButtonText = "Возобновить";
                    

                }
            }
            else
            {
                this.ParserPauseButtonAccessibility = false;
                this.ParserStartStopButtonText = "Запустить";
                this.ParserStatusText = "Отключен";
            }
        }



        private double progress;
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }
        private string currentAction;
        public string CurrentAction
        {
            get { return currentAction; }
            set
            {
                currentAction = value;
                OnPropertyChanged("CurrentAction");
            }
        }

        private string parserStatusText;
        public string ParserStatusText
        {
            get { return parserStatusText; }
            set
            {
                parserStatusText = value;
                OnPropertyChanged("ParserStatusText");
            }
        }


        private string parserStartStopButtonText;
        public string ParserStartStopButtonText
        {
            get { return parserStartStopButtonText; }
            set
            {
                parserStartStopButtonText = value;
                OnPropertyChanged("ParserStartStopButtonText");
            }
        }
        private string parserPauseButtonText;
        public string ParserPauseButtonText
        {
            get { return parserPauseButtonText; }
            set
            {
                parserPauseButtonText = value;
                OnPropertyChanged("ParserPauseButtonText");
            }
        }

        private bool parserPauseButtonAccessibility;
        public bool ParserPauseButtonAccessibility
        {
            get { return parserPauseButtonAccessibility; }
            set
            {
                parserPauseButtonAccessibility = value;
                OnPropertyChanged("ParserPauseButtonAccessibility");
            }
        }





        private RelayCommand openSettings;
        public RelayCommand OpenSettings
        {
            get
            {
                return openSettings ??
                    (openSettings = new RelayCommand(obj =>
                    {
                        Views.ParserSettings f = new Views.ParserSettings();
                        f.Show();
                    }));
            }
        }
        private RelayCommand suspendParsing;
        public RelayCommand SuspendParsing
        {
            get
            {
                return suspendParsing ??
                    (suspendParsing = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        private RelayCommand startOrStopParser;
        public RelayCommand StartOrStopParser
        {
            get
            {
                return startOrStopParser ??
                    (startOrStopParser = new RelayCommand(obj =>
                    {

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
