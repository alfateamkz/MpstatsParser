using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MpstatsParser.Services;
using MpstatsParser.Models.API;
using MpstatsParser.Models;
using MpstatsParser.Models.Excel;
using MpstatsParser.Exceptions;
using System.Windows;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Threading;

namespace MpstatsParser.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        async Task UpdateInterfaceAsync()
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(async () =>   
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
            });           
        }
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
            //Возобновление работы парсера
            if (ParserParameters.Params.IsStarted)
            {
                ParserParameters.Params.IsStarted = false;
                ParserParameters.Params.IsSuspended = false;
                StartOrStopParser.Execute(null);
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
                    (startOrStopParser = new RelayCommand(async obj =>
                    {
                       await Dispatcher.CurrentDispatcher.InvokeAsync(async () =>
                        {
                            int i = 0;
                            if (!ParserParameters.Params.IsStarted)
                            {
                                ParserParameters.Params.IsStarted = true;
                                ParserParameters.Params.IsSuspended = false;
                                await StartParsing();
                                await ExportReportToExcel.Export(ParserParameters.Params.FileResultPath,
                                    ParserParameters.Params.ExcelReportRows);
                                MessageBox.Show("Парсинг успешно завершен,\n" +
                                    $"Файл с отчетом находится по пути :\n" +
                                    @$"{ParserParameters.Params.FileResultPath}\Report.xlsx", "Уведомление",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                                await ParserParameters.ResetParserParameters();
                            }
                            else
                            {
                                if (MessageBox.Show("Вы действительно хотите остановить парсинг?", "Предупреждение", MessageBoxButton.YesNo)
                                == MessageBoxResult.Yes)
                                {

                                    await ParserParameters.ResetParserParameters();
                                }

                            }
                            await ParserParameters.SaveParametersAsync();
                            await UpdateInterfaceAsync();
                        });
                    }));
            }
        }

      
        private async Task StartParsing()
        {
          await GetCategoriesFromAPI(ParserParameters.Params.GetCategoriesIteration);
          await GetAllSubcategoryDetailsFromAPI(ParserParameters.Params.GetSubcategoryInfoIteration);
        }
        async Task GetCategoriesFromAPI(int iteration = 0)
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(async()  =>
            {
                ParserParameters.Params.Rubricator = MpstatsAPI.GetRubricator();
                try
                {
                    bool isPaused = false;
                    while (ParserParameters.Params.IsStarted)
                    {
                        if (ParserParameters.Params.IsCategoriesGot)
                        {
                            break;
                        }
                        while (!ParserParameters.Params.IsSuspended)
                        {

                            for (int i = iteration; i < ParserParameters.Params.Rubricator.Count; i++)
                            {

                                isPaused = false;
                                var rub = ParserParameters.Params.Rubricator[i];
                                CurrentAction = $"Загрузка списка категорий : {rub.Path}";
                                SubcategoryModel subcategory = new SubcategoryModel
                                {
                                    Subcategories = MpstatsAPI.GetSubcategoryInfo(rub.Path, default, DateTime.Now)
                                };
                                System.Diagnostics.Debug.WriteLine(i + "  " + rub.Path);
                                ParserParameters.Params.Categories.Add(subcategory);
                            }
                        }
                        if (!isPaused)
                        {
                            isPaused = true;
                            ParserParameters.SaveParameters();
                        }
                    }
                }
                catch (APIRequestLimitException ex)
                {
                    CurrentAction = $"Кончились лимиты на запросы в день. Попробуем ещё раз через час";
                    System.Diagnostics.Debug.WriteLine("код 429");
                    await Task.Delay(3600 * 1000);

                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
                ParserParameters.SaveParameters();
                System.Diagnostics.Debug.WriteLine("Информация о категориях спарсена");

            });
          
        }

        async Task GetAllSubcategoryDetailsFromAPI(int iteration = 0)
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(async () =>
            {
                try
                {
                    bool isPaused = false;
                    ParserParameters.Params.ExcelReportRows = new List<ExcelRowModel>();
                    for (int i = iteration; i < ParserParameters.Params.Categories.Count; i++)
                    {
                        while (ParserParameters.Params.IsStarted)
                        {
                            while (!ParserParameters.Params.IsSuspended)
                            {
                                isPaused = false;
                                var cat = ParserParameters.Params.Categories[i];
                                CurrentAction = $"Получение информации о категории : {cat.Path}";
                                ExcelRowModel row = new ExcelRowModel();
                                row.Category = cat.Path;
                                // 1.Выручка 6 / 20 - Выручка 6 / 21
                                // Выручка в данной категории за месяц.
                                //Пример.В подкатегории Аксессуары / Сумки и рюкзаки/ Рюкзаки
                                //выбираем длину периода 01.03.2021 – 31.03.2021 и вкладку «Продавцы», см ниже
                                var first = MpstatsAPI.GetCategorySellers(cat.Path, new DateTime(2020, 6, 1), new DateTime(2020, 6, 30));
                                row.Revenue6_20 = first.Sum(o => o.Revenue).ToString();
                                first = MpstatsAPI.GetCategorySellers(cat.Path, new DateTime(2021, 6, 1), new DateTime(2021, 6, 30));
                                row.Revenue6_21 = first.Sum(o => o.Revenue).ToString();
                                //2.	Столбцы: Выруч 6/21 Топ1, Выруч 6/21 Топ2, Выруч 6/21 Топ3, Выруч 6/21 Топ4, Выруч 6/21 Остальные.
                                //Это выручка за июнь 2021 года, с 1.06.2021 по 30.06.2021 самых крупных поставщиков.
                                row.Revenue6_21_Top1 = first[0].Revenue.ToString();
                                row.Revenue6_21_Top2 = first[1].Revenue.ToString();
                                row.Revenue6_21_Top3 = first[2].Revenue.ToString();
                                row.Revenue6_21_Top4 = first[3].Revenue.ToString();
                                row.Revenue6_21_Other = first.Skip(4).Sum(o => o.Revenue).ToString();
                                //3.Столбец «SKU с продажами 6 / 21»
                                //Выбираем в подкатегории Аксессуары/ Сумки и рюкзаки/ Рюкзаки длину периода 01.06.2021 – 30.06.2021 и вкладку «Товары».
                                //Далее выбираем Фильтры->Выручка->Больше чем 0->Применить.
                                var third = MpstatsAPI.GetCategoryProducts(cat.Path, new DateTime(2021, 6, 1), new DateTime(2021, 6, 30))
                                    .Where(o => o.Revenue > 0).ToList();
                                row.SKU6_21 = third.Count.ToString();
                                //4.	Столбец «Кол-во SKU с выручкой > x 6/21»
                                //Перед парсингом в парсере мне необходимо иметь возможность задать это значение х.
                                //К примеру, я задам 200 000.
                                var fourth = third.Where(o => o.Revenue > ParserParameters.Params.SKUPriceFrom).ToList();
                                row.SKU6_21_X = fourth.Count.ToString();
                                //5.	Столбцы «Товаров с продажами 13.7.20-19.7.20», «Товаров с продажами 11.1.21-17.1.21»,
                                //«Товаров с продажами 5.7.21-11.7.21». Столбцы «Выручка на товар 13.7.20-19.7.20»,
                                //«Выручка на товар 11.1.21-17.1.21», «Выручка на товар 5.7.21-11.7.21»
                                var fifth = MpstatsAPI.GetCategoryTrends(cat.Path, new DateTime(2020, 7, 13), new DateTime(2020, 7, 19));
                                row.ProductWithSalesQuantity13_19july20 = fifth[0].Items.ToString();
                                row.ProductWithSalesRevenue13_19july20 = fifth[0].ProductRevenue.ToString();
                                fifth = MpstatsAPI.GetCategoryTrends(cat.Path, new DateTime(2021, 7, 5), new DateTime(2021, 7, 11));
                                row.ProductWithSalesQuantity5_11july21 = fifth[0].Items.ToString();
                                row.ProductWithSalesRevenue5_11july21 = fifth[0].ProductRevenue.ToString();
                                fifth = MpstatsAPI.GetCategoryTrends(cat.Path, new DateTime(2021, 1, 11), new DateTime(2021, 1, 17));
                                row.ProductWithSalesQuantity11_17january21 = fifth[0].Items.ToString();
                                row.ProductWithSalesRevenue11_17january21 = fifth[0].ProductRevenue.ToString();

                                ParserParameters.Params.ExcelReportRows.Add(row);
                                System.Diagnostics.Debug.WriteLine($"{iteration}  {cat.Path}");
                            }
                            if (!isPaused)
                            {
                                isPaused = true;
                                ParserParameters.SaveParameters();
                            }
                        }
                    }
                    ParserParameters.SaveParameters();
                    System.Diagnostics.Debug.WriteLine("Парсинг завершен???");
                }
                catch (APIRequestLimitException ex)
                {
                    CurrentAction = $"Кончились лимиты на запросы в день. Попробуем ещё раз через час";
                    System.Diagnostics.Debug.WriteLine("код 429");
                    await Task.Delay(3600 * 1000);

                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            });
         
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
