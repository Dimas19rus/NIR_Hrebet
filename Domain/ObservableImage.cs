using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class ObservableImage
    {
        public string PathData { get; set; } = ""; //Расположение папки картинки data/name
        public string FileNameImageOrig { get; set; } = ""; //Название оригинальной картинки
        
        public string NameImageOrig //Название файла без расширения (формата)
        {
            get
            {
                int i = FileNameImageOrig.IndexOf('.');
                return FileNameImageOrig.Remove(i, FileNameImageOrig.Length-i);
            }

            set
            {
                FileNameImageOrig = FileNameImageOrig.Replace(NameImageOrig, value);
                UpdateName(value);
            }
        }
        public string FileNameImageBlackAndWhite { get; set; } = ""; //Название черно-белой картинки
        public string FileNameImageProcessed { get; set; } = ""; //Название черно-белой, подогнаной под размер рабочей поверхности картинки
        public string FileNameGCode { get; set; } = ""; //Название .txt файла с G-кодам
        public DateTime DateChange { get; set; } //Дата последних изменений
        
        private void UpdateName(string name)
        {
            
            if (FileNameImageBlackAndWhite != "")
                FileNameImageBlackAndWhite = "BW" + name + GetExtensionFile(FileNameImageOrig); /*FileNameImageBlackAndWhite.Replace(NameImageOrig, name);*/
            if (FileNameImageProcessed != "")
                FileNameImageProcessed = "Proc" + name + GetExtensionFile(FileNameImageOrig); /*FileNameImageProcessed.Replace(NameImageOrig, name);*/
            if (FileNameGCode != "")
                FileNameGCode = "GCode" + name + GetExtensionFile(FileNameImageOrig); /*FileNameGCode.Replace(NameImageOrig, name);*/
            UpdateDateChange();
        }

        public string GetExtensionFile(string str)
        {
            int i = str.IndexOf('.');
            return str.Remove(0, i);
        }

        public void UpdateDateChange()
        {
            DateChange = DateTime.Now;
        }
    }
}
