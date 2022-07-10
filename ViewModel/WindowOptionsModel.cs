using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    [Serializable]
    public class WindowOptionsModel
    {
        public double Width { get; set; } = 1000;
        public double Height { get; set; } = 1000;
        public double IntervalPin { get; set; } = 25;
        public double LengthHead { get; set; } = 200;
        public double RatioXY { get; set; } = 15;
        public double RatioZ { get; set; } = 15;
        public double RatioAlfd { get; set; } = 1;
        
        
        //Для проверки
        public int CountPin { get; set; }

        public bool Check()
        {
            int countPin;
            bool b = int.TryParse(((Width / IntervalPin) * (Height / IntervalPin)).ToString(),out countPin);
            if (b)
                CountPin = countPin;
            return b;
        }

        public bool CheckNull()
        {
            if (Width == 0) return false;
            if (Height == 0) return false;
            if (IntervalPin == 0) return false;
            if (LengthHead == 0) return false;
            if (RatioXY == 0) return false;
            if (RatioZ == 0) return false;
            if (RatioAlfd == 0) return false;
            return true;
        }
        
    }
}
