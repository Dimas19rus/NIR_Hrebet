
using System;
using System.IO;
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
using System.Windows.Forms;
using System.Reflection;
using ViewModel;
using System.Drawing;
using System.Security.Policy;
using System.Collections.ObjectModel;
using Domain;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Threading;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowModel Model => (MainWindowModel)DataContext;

        public WindowOptionsModel ModelOptions => Model.ModelOptions;

        public ImageSource _imageDefould;

        public MainWindow()
        {
            Deserialize();
            Model.SelectedImage = null;
            InitializeComponent();
            Update();
            _imageDefould = ImageOriginal.Source;
        }

        public void Update()
        {
            if (Model.SelectedImage != null)
            {
                try
                {
                    ImageOriginal.Source = new BitmapImage(new Uri(Model.GetPathImageSelected(true)));
                    TextBlockLoading.Text = "";
                }
                catch
                {
                    ImageOriginal.Source = _imageDefould;
                    TextBlockLoading.Text = "Изображение не было загружено!!!";
                }
            }

            CheckEnabledButton();

        }

        private void ButtonAddImage_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Multiselect = true;
                fileDialog.InitialDirectory = "c:\\";
                fileDialog.Title = "Добавление изображений";
                fileDialog.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";

                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int i = 0; i < fileDialog.FileNames.Length; i++)
                    {
                        string pathImage = fileDialog.FileNames[i];
                        FileInfo file = new FileInfo(pathImage);

                        string extension = file.Extension;
                        string nameNotExtension = file.Name.Replace(extension, "");
                        string pathSave = Model.PathImageAndGCode + nameNotExtension;

                        // \Data\#name#\#name.format#
                        string pathSaveImage = pathSave + "/" + fileDialog.SafeFileNames[i];

                        Model.CheckAndCreateDirectory(pathSave);

                        File.Copy(pathImage, pathSaveImage, true);

                        FileInfo image = new FileInfo(pathSaveImage);

                        Model.AddImage(image);
                    }

                }
            }

            Serializable();
        }

        private void ImagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
            if (Model.SelectedImage == null)
                TextBoxNameFile.Text = "";
            else
                TextBoxNameFile.Text = Model.SelectedImage.NameImageOrig;
            TextBoxGCode.Text = "";
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            WindowOptions window = new WindowOptions(this, ModelOptions);
            window.ShowDialog();
        }

        private void ButtonDisplayImage_Click(object sender, RoutedEventArgs e)
        {
            WindowDisplayImages window = new WindowDisplayImages();
            if (Model.SelectedImage.FileNameImageOrig != "")
            {
                string path = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameImageOrig;
                FileInfo image = new FileInfo(path);
                window.SetImageOrig(new Uri(image.FullName));
            }

            if (Model.SelectedImage.FileNameImageBlackAndWhite != "")
            {
                string path = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameImageBlackAndWhite;
                FileInfo image = new FileInfo(path);
                window.SetImageBlackAndWhite(new Uri(image.FullName));
            }

            if (Model.SelectedImage.FileNameImageProcessed != "")
            {
                string path = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameImageProcessed;
                FileInfo image = new FileInfo(path);
                window.SetImageProcessed(new Uri(image.FullName));
            }


            window.ShowDialog();
        }

        private void ButtonDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            ImageOriginal.Source = _imageDefould;
            while (ListViewImages.SelectedItems.Count > 0)
            {
                Model.DeleteImage((ObservableImage)ListViewImages.SelectedItems[0]);
            }

            Serializable();
        }

        private void ButtonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Model.GetPathImageSelected(false));
        }


        private void CheckEnabledButton()
        {
            if (Model.SelectedImage != null)
            {
                double opacity = 1;
                ButtonOpenImage.IsEnabled = true;
                ButtonOpenImage.Opacity = opacity;

                ButtonDisplayImage.IsEnabled = true;
                ButtonDisplayImage.Opacity = opacity;

                ButtonDeleteImage.IsEnabled = true;
                ButtonDeleteImage.Opacity = opacity;

                ButtonStart.IsEnabled = true;
                ButtonStart.Opacity = opacity;

                ButtonSave.IsEnabled = true;
                ButtonSave.Opacity = opacity;
                if (Model.SelectedImage.FileNameGCode != "")
                {
                    ButtonLoadingGCode.IsEnabled = true;
                    ButtonLoadingGCode.Opacity = opacity;
                }
                else
                {
                    opacity = 0.5;
                    ButtonLoadingGCode.IsEnabled = false;
                    ButtonLoadingGCode.Opacity = opacity;
                }
            }
            else
            {
                double opacity = 0.5;
                ButtonOpenImage.IsEnabled = false;
                ButtonOpenImage.Opacity = opacity;

                ButtonDisplayImage.IsEnabled = false;
                ButtonDisplayImage.Opacity = opacity;

                ButtonDeleteImage.IsEnabled = false;
                ButtonDeleteImage.Opacity = opacity;

                ButtonStart.IsEnabled = false;
                ButtonStart.Opacity = opacity;

                ButtonSave.IsEnabled = false;
                ButtonSave.Opacity = opacity;

                ButtonLoadingGCode.IsEnabled = false;
                ButtonLoadingGCode.Opacity = opacity;
            }
            bool b;


            if (Model.SelectedImage != null && TextBoxNameFile.Text != Model.SelectedImage.NameImageOrig)
            {
                double opacity = 1;

                ButtonUndoNameFile.IsEnabled = true;
                ButtonUndoNameFile.Opacity = opacity;

                ButtonChangetNameFile.IsEnabled = true;
                ButtonChangetNameFile.Opacity = opacity;

                TextBoxNameFile.IsEnabled = true;
                TextBoxNameFile.Opacity = opacity;
            }
            else
            {
                double opacity = 0.5;

                ButtonUndoNameFile.IsEnabled = false;
                ButtonUndoNameFile.Opacity = opacity;

                ButtonChangetNameFile.IsEnabled = false;
                ButtonChangetNameFile.Opacity = opacity;

                TextBoxNameFile.IsEnabled = false;
                TextBoxNameFile.Opacity = opacity;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            Model.SelectedImage.FileNameGCode = Model.GetNameImageSelected(false) + ".txt";

            string writePath = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameGCode;

            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(TextBoxGCode.Text);
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("При сохранение G-кода произошла ошибка!!! \nПоробуйте еще раз.", "Ошибка!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
            }
            Serializable();
            TextBlockLoading.Text = "G-код сохранён";
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            List<string> listGCode = Model.StartProc();


            TextBoxGCode.Text = "";
            Thread TMyfunc = new Thread(delegate ()
            {
                float k = (float)100 / (float)listGCode.Count;
                string s = "";


                Dispatcher.Invoke(new Action(() =>
                {
                    ProgresBarForeground(ToBrush("#FF06B025"), "Сборка G-кода...", 0);
                    TextBlockLoading.Text = "Сборка G-кода...";
                    ProgressBarGCode.Value = 0;
                }), DispatcherPriority.ContextIdle);

                foreach (string str in listGCode)
                {
                    s += str;
                    Dispatcher.Invoke(new Action(() =>
                    {
                        ProgressBarGCode.Value += k;
                    }), DispatcherPriority.ContextIdle);

                }


                Dispatcher.Invoke(new Action(() =>
                {
                    TextBoxGCode.Text = "";
                    TextBoxGCode.Text = s;
                    ProgressBarGCode.Value = 100;
                    TextBlockLoading.Text = "G-код собран";
                    Serializable();
                }), DispatcherPriority.ContextIdle);

            });
            TMyfunc.Start();

            
        }


        public SolidColorBrush ToBrush(string HexColorString)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(HexColorString));
        }


        public void Serializable()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("Data.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Model);
            }
        }

        public void Deserialize()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream("Data.dat", FileMode.OpenOrCreate))
                {
                    DataContext = formatter.Deserialize(fs);
                }
            }
            catch (Exception)
            {
                DataContext = new MainWindowModel();
                Model.ModelOptions = new WindowOptionsModel();
            }
        }

        private void ButtonLoadingGCode_Click(object sender, RoutedEventArgs e)
        {

            if (Model.SelectedImage.FileNameGCode != "")
            {
                string ReadPath = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameGCode;
                using (FileStream fstream = File.OpenRead($"{ReadPath}"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    TextBoxGCode.Text = textFromFile;
                    ProgresBarForeground(ToBrush("#FF06B025"), "Файл загружен", 100);
                }
            }
            else
            {
                ProgresBarForeground(new SolidColorBrush(Colors.Red), "Файл с G-кодом отсутсвует!!!", 100);
            }
        }

        private void ProgresBarForeground(SolidColorBrush color, string message, float valueProgres)
        {
            ProgressBarGCode.Foreground = color;
            ProgressBarGCode.Value = valueProgres;
            TextBlockLoading.Text = message;
        }


        private void ButtonChangetNameFile_Click(object sender, RoutedEventArgs e)
        {
            string oldPathImageOrig = "", newPathImageOrig = "",
                oldPathGCode = "", newPathGCode = "", 
                oldPathBlackAndWhite = "", newPathBlackAndWhite = "", 
                oldPathImageProcessed = "", newPathImageProcessed = "";

            
            bool bImageOrig = false;
            bool bGCode = false;
            bool bImageBlackAndWhite = false;
            bool bImageProcessed = false;

            if (Model.SelectedImage.FileNameImageOrig != "")
            {
                bImageOrig = true;
                oldPathImageOrig = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameImageOrig;
                newPathImageOrig = Model.SelectedImage.PathData + "/" + TextBoxNameFile.Text + Model.SelectedImage.GetExtensionFile(Model.SelectedImage.FileNameImageOrig);
            }
                
            if (Model.SelectedImage.FileNameGCode != "")
            {
                bGCode = true;
                oldPathGCode = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameGCode;
                newPathGCode = Model.SelectedImage.PathData + "/GCode" + TextBoxNameFile.Text + Model.SelectedImage.GetExtensionFile(Model.SelectedImage.FileNameGCode);
            }
                
            if (Model.SelectedImage.FileNameImageBlackAndWhite != "")
            {
                bImageBlackAndWhite = true;
                oldPathBlackAndWhite = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameImageBlackAndWhite;
                newPathBlackAndWhite = Model.SelectedImage.PathData + "/BW" + TextBoxNameFile.Text + Model.SelectedImage.GetExtensionFile(Model.SelectedImage.FileNameImageBlackAndWhite);
            }

            if (Model.SelectedImage.FileNameImageProcessed != "")
            {
                bImageProcessed = true;
                oldPathImageProcessed = Model.SelectedImage.PathData + "/" + Model.SelectedImage.FileNameImageProcessed;
                newPathImageProcessed = Model.SelectedImage.PathData + "/Proc" + TextBoxNameFile.Text + Model.SelectedImage.GetExtensionFile(Model.SelectedImage.FileNameImageProcessed);
            }

            Thread TMyfunc = new Thread(delegate ()
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    ProgressBarGCode.Value = 0;
                    TextBlockLoading.Text = "Переименование файлов...";
                    ImageOriginal.Source = _imageDefould;

                }), DispatcherPriority.ContextIdle);
                bool b = true;

                while (b)
                    try
                    {
                        //File.Move(Model.SelectedImage.PathData, Model.NameDataImage+"/"+ TextBoxNameFile.Text);
                        if (bImageOrig)
                            File.Move(oldPathImageOrig, newPathImageOrig);
                        b = false;
                    }
                    catch
                    {
                        b = true;
                    }

                Dispatcher.Invoke(new Action(() =>
                {
                    ProgressBarGCode.Value += 50;
                }), DispatcherPriority.ContextIdle);

                b = true;
                while (b)
                    try
                    {
                        if (bGCode)
                            File.Move(oldPathGCode, newPathGCode);
                        b = false;
                    }
                    catch
                    {
                        b = true;
                    }
                Dispatcher.Invoke(new Action(() =>
                {
                    ProgressBarGCode.Value += 10;
                }), DispatcherPriority.ContextIdle);

                b = true;
                while (b)
                    try
                    {
                        if (bImageBlackAndWhite)
                            File.Move(oldPathBlackAndWhite, newPathBlackAndWhite);
                        b = false;
                    }
                    catch
                    {
                        b = true;
                    }
                Dispatcher.Invoke(new Action(() =>
                {
                    ProgressBarGCode.Value += 10;
                }), DispatcherPriority.ContextIdle);

                b = true;
                while (b)
                    try
                    {
                        if (bImageProcessed)
                            File.Move(oldPathImageProcessed, newPathImageProcessed);
                        b = false;
                    }
                    catch
                    {
                        b = true;
                    }
                Dispatcher.Invoke(new Action(() =>
                {
                    Model.Change(TextBoxNameFile.Text);
                    TextBlockLoading.Text = "Успешно переименовано";
                    ProgressBarGCode.Value = 100;
                    //ImageOriginal.Source = new BitmapImage(new Uri(Model.GetPathImageSelected(true)));
                    Serializable();
                }), DispatcherPriority.ContextIdle);
                
            });

            TMyfunc.Start();

            
        }

        private void ButtonUndoNameFile_Click(object sender, RoutedEventArgs e)
        {
            TextBoxNameFile.Text = Model.SelectedImage.NameImageOrig;
        }

        private void TextBoxNameFile_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Model.SelectedImage == null)
            {
                double opacity = 0.5;
                ButtonUndoNameFile.IsEnabled = false;
                ButtonUndoNameFile.Opacity = opacity;

                ButtonChangetNameFile.IsEnabled = false;
                ButtonChangetNameFile.Opacity = opacity;
            }
            else
            {
                if (TextBoxNameFile.Text == Model.SelectedImage.NameImageOrig)
                {
                    double opacity = 0.5;
                    ButtonUndoNameFile.IsEnabled = false;
                    ButtonUndoNameFile.Opacity = opacity;

                    ButtonChangetNameFile.IsEnabled = false;
                    ButtonChangetNameFile.Opacity = opacity;
                }
                else
                {
                    double opacity = 1;
                    ButtonUndoNameFile.IsEnabled = true;
                    ButtonUndoNameFile.Opacity = opacity;

                    ButtonChangetNameFile.IsEnabled = true;
                    ButtonChangetNameFile.Opacity = opacity;
                }
            }
        }
    }
}
