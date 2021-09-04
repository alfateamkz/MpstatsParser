using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MpstatsParser.Services;
using MpstatsParser.Models.API;
using MpstatsParser.Models;
using System.Windows;
using System.Threading.Tasks;

namespace MpstatsParser.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        void UpdateInterface()
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
                this.ParserPauseButtonText = "Пауза";
                this.ParserStatusText = "Отключен";
            }
        }
        public MainWindowViewModel()
        {
            UpdateInterface();
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
                        if (!ParserParameters.Params.IsSuspended)
                        {


                            ParserParameters.Params.IsSuspended = true;
                        }
                        else
                        {


                            ParserParameters.Params.IsSuspended = false;
                        }
                        ParserParameters.SaveParameters();
                        UpdateInterface();
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
                        int i = 0;
                        if (!ParserParameters.Params.IsStarted)
                        {
                            ParseCategories();
                        }
                        else
                        {
                            if (MessageBox.Show("Вы действительно хотите остановить парсинг?", "Предупреждение", MessageBoxButton.YesNo)
                            == MessageBoxResult.Yes)
                            {
                                ParserParameters.Params.Categories = new List<SubcategoryModel>();
                                ParserParameters.Params.CurrentCategoryIndex = -1;
                                ParserParameters.Params.IsSuspended = false;
                                ParserParameters.Params.IsStarted = false;
                                

                          
                            }

                            
                        }
                        ParserParameters.SaveParameters();
                        UpdateInterface();
                    }));
            }
        }
        async Task ParseCategories()
        {
            int i = 0;
            ParserParameters.Params.IsStarted = true;
            ParserParameters.Params.IsSuspended = false;

            ParserParameters.Params.Rubricator = MpstatsAPI.GetRubricator();
            try
            {
                foreach (var rub in ParserParameters.Params.Rubricator)
                {
                    SubcategoryModel subcategory = new SubcategoryModel
                    {
                        Subcategories = MpstatsAPI.GetSubcategoryInfo(rub.Path, default, DateTime.Now)
                    };
                    CurrentAction = $"Загрузка списка категорий : {rub.Name}";
                    i++; System.Diagnostics.Debug.WriteLine(i +"  " + rub.Path );
                    ParserParameters.Params.Categories.Add(subcategory);
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
          
            System.Diagnostics.Debug.WriteLine("говно");
            //try
            //{
            //    foreach (var cat in ParserParameters.Params.Categories)
            //    {
            //        cat.Subcategories = MpstatsAPI.GetSubcategoryInfo(cat.Name, default, DateTime.Now);
            //        i += cat.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + cat.Name);
            //        if (cat.Subcategories.Count > 0)
            //        {
            //            foreach (var c in cat.Subcategories)
            //            {
            //                c.Subcategories = MpstatsAPI.GetSubcategoryInfo(c.Name, default, DateTime.Now);
            //                i += c.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + c.Name);
            //                if (c.Subcategories.Count > 0)
            //                {
            //                    foreach (var c2 in c.Subcategories)
            //                    {
            //                        c2.Subcategories = MpstatsAPI.GetSubcategoryInfo(c2.Name, default, DateTime.Now);
            //                        i += c2.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + c2.Name);
            //                        if (c2.Subcategories.Count > 0)
            //                        {
            //                            foreach (var c3 in c2.Subcategories)
            //                            {
            //                                c3.Subcategories = MpstatsAPI.GetSubcategoryInfo(c3.Name, default, DateTime.Now);
            //                                i += c3.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + c3.Name);
            //                                if (c3.Subcategories.Count > 0)
            //                                {
            //                                    foreach (var c4 in c3.Subcategories)
            //                                    {
            //                                        c4.Subcategories = MpstatsAPI.GetSubcategoryInfo(c4.Name, default, DateTime.Now);
            //                                        i += c4.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + c4.Name);
            //                                        if (c4.Subcategories.Count > 0)
            //                                        {
            //                                            foreach (var c5 in c4.Subcategories)
            //                                            {
            //                                                c5.Subcategories = MpstatsAPI.GetSubcategoryInfo(c5.Name, default, DateTime.Now);
            //                                                i += c5.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + c5.Name);
            //                                                if (c5.Subcategories.Count > 0)
            //                                                {
            //                                                    foreach (var c6 in c5.Subcategories)
            //                                                    {
            //                                                        c6.Subcategories = MpstatsAPI.GetSubcategoryInfo(c6.Name, default, DateTime.Now);
            //                                                        i += c6.Subcategories.Count; System.Diagnostics.Debug.WriteLine(i + "  " + c6.Name);
            //                                                    }
            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //    }

            //}
            //catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            ParserParameters.SaveParameters();
            MessageBox.Show(i.ToString());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
