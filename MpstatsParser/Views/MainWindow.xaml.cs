using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MpstatsParser.Models;
using MpstatsParser.Services;
namespace MpstatsParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindowViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ParserParameters.Params.IsStarted)
            {
                ViewModels.MainWindowViewModel context = (ViewModels.MainWindowViewModel)this.DataContext;
                if (MessageBox.Show("Вы действительно хотите закрыть парсер. " +
                    "Прогресс будет сохранен и восстановлен после повторного запуска", "Предупреждение",
                    MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    ParserParameters.SaveParameters();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        
        }
    }
}
