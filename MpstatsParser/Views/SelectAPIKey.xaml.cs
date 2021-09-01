using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MpstatsParser.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectAPIKey.xaml
    /// </summary>
    public partial class SelectAPIKey : Window
    {
        public SelectAPIKey()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.SelectAPIKeyViewModel();
        }
    }
}
