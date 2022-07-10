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
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для WindowOptions.xaml
    /// </summary>
    public partial class WindowOptions : Window
    {
        public WindowOptionsModel Model => (WindowOptionsModel)DataContext;
        public MainWindowModel MainModel => MainWindow.Model;
        public MainWindow MainWindow;
        public WindowOptions(MainWindow mainWindow )
        {
            DataContext = new WindowOptionsModel();
            MainWindow = mainWindow;
            InitializeComponent();
        }

        public WindowOptions(MainWindow mainWindow, WindowOptionsModel windowOptionsModel)
        {
            if(windowOptionsModel!=null)
                DataContext = windowOptionsModel;
            else
                DataContext = new WindowOptionsModel();
            MainWindow = mainWindow;
            InitializeComponent();
        }

        

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            if(Model.Check())
            {
                TextBlockStatus.Text = "Успешно";
                TextBlockCountPin.Text = Model.CountPin.ToString();
            }
            else
            {
                TextBlockStatus.Text = "Ошибка!!!";
            }
        }

        //добавить обработку неправельной записи
        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            ButtonCheck_Click(sender, e);

            //Model.Width = int.Parse(TextBoxWidth.Text);
            //Model.Height = int.Parse(TextBoxHeight.Text);
            //Model.IntervalPin = int.Parse(TextBoxIntervalPin.Text);
            //Model.LengthHead = int.Parse(TextBoxLengthHead.Text);
            //Model.LengthHead = int.Parse(TextBoxRatioXY.Text);
            //Model.RatioZ = int.Parse(TextBoxRatioZ.Text);
            //Model.RatioAlfd = int.Parse(TextBoxRatioAlfd.Text);
            MainModel.ModelOptions = Model;
            MainWindow.Serializable();

            Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }
}
