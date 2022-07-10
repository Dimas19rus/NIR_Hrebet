using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModel
{
    [Serializable]
    public class MainWindowModel
    {

        public WindowOptionsModel ModelOptions { get; set; }
        public ObservableCollection<ObservableImage> Images { get; set; }
        
        public ObservableImage SelectedImage { get; set; }

        public string NameDataImage { get; set; } = "Data"; //Папка где хранятся все картинки
        public string PathImageAndGCode { get; set; } //Путь в место хранения картинок

        public MainWindowModel()
        {
            Images = new ObservableCollection<ObservableImage>();
            PathImageAndGCode= NameDataImage + "/";
            string path = NameDataImage;

            CheckAndCreateDirectory(path);

            ModelOptions = new WindowOptionsModel();
        }

        public void AddImage(FileInfo fileImage)
        {
            string pathImage = PathImageAndGCode + GetNameFileNotExtension(fileImage.Name)/*fileImage.Name.Replace(fileImage.Extension, "")*/;
            //string pathImageFull = PathImageAndGCode + fileImage.Name.Replace(fileImage.Extension, "") +"/"+ fileImage.Name;
            ObservableImage image = new ObservableImage { FileNameImageOrig = fileImage.Name, DateChange = DateTime.Now, PathData = pathImage };

            Images.Add(image);
        }

        public void DeleteImage(ObservableImage image)
        {
            Images.Remove(image);

            // создаем новый поток
            Thread myThread = new Thread(new ThreadStart(() => DeleteDirectory(image.PathData)));
            myThread.Start(); // запускаем поток

        }

        private static void DeleteDirectory(string directory)
        {
            bool b = true;
            do
            {
                try
                {
                    Directory.Delete(directory, true);
                    b = false;
                }
                catch
                {
                    Thread.Sleep(400);
                }
            }while (b);
        }

        public string GetPathImageSelected(bool fullName)
        {
            string str = SelectedImage.PathData + "/" + SelectedImage.FileNameImageOrig;
            FileInfo file = new FileInfo(str);
            if (fullName)
            {
                return file.FullName;
            }
            else
            {
                return file.DirectoryName;
            }
            
        }

        public string GetNameImageSelected(bool format)
        {
            string str = SelectedImage.PathData + "/" + SelectedImage.FileNameImageOrig;
            FileInfo file = new FileInfo(str);
            if (format)
            {
                return file.Name;
            }
            else
            {
                return GetNameFileNotExtension(file.Name)/*file.Name.Replace(file.Extension, "")*/;
            }
        }

        //изменить название (Доделать)
        public void Change(string name)
        {
            SelectedImage.NameImageOrig = name;
        }

        public bool CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }
            return true;
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool CheckAndCreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }
            return true;
            
        }

        public List<string> StartProc()
        {
            BitmapDebug debug = new BitmapDebug(SelectedImage);

            return StartBuildingGCode(debug.ImagePixcels);
        }

        private List<string> StartBuildingGCode(int[,] pixcels)
        {
            BuildingGCode buildingGCode = new BuildingGCode(ModelOptions.Width, ModelOptions.Height, ModelOptions.IntervalPin, ModelOptions.RatioXY, ModelOptions.RatioZ, ModelOptions.LengthHead, pixcels);

            return buildingGCode.StartBuildingGCode();
        }

        private string GetNameFileNotExtension(string str)
        {
            int i = str.IndexOf('.');
            return str.Remove(i, str.Length - i);
        }



    }
}
