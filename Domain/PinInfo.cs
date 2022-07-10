using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class PinInfo
    {

        public int ID { get; set; } // Для название подпрограммы
        public double[] XY { get; set; }  //[X,Y](реальные размеры, [мм, мм])
        public double[] Vector { get; set; }  //[Xv,Yv](реальные размеры, [мм, мм])
        public double Distance { get; set; } //Растояние от центра зоны до штыря который нужно толкать
        public double Alfa { get; set; } //Угол поворота (реальные размеры, радианы)

        public double Z { get; set; } // Высота поднятия штыря

        public double GetDegreesAlfa()
        {
            return Alfa * (180 / Math.PI);
        }

        public PinInfo(int id, double[] xy, double xCenterWorkZone, double yCenterWorkZone, double z)
        {
            ID = id;
            XY = xy;
            Vector = new double[] { XY[0] - xCenterWorkZone, XY[1] - yCenterWorkZone };
            Distance = Math.Sqrt(Math.Pow(xCenterWorkZone - xy[0], 2) + Math.Pow(yCenterWorkZone - xy[1], 2));
            Z = z;
        }

        public PinInfo(double[] vector0, int id, double[] xy, double xCenterWorkZone, double yCenterWorkZone, double z) : this(id, xy, xCenterWorkZone, yCenterWorkZone, z)
        {
            SetAlfa(vector0);
        }

        public void SetAlfa(double[] vector0)
        {
            Alfa = Math.Acos((vector0[0] * Vector[0] + vector0[1] * Vector[1]) / (Math.Sqrt(Math.Pow(vector0[0], 2) + Math.Pow(vector0[1], 2)) * Math.Sqrt(Math.Pow(Vector[0], 2) + Math.Pow(Vector[1], 2))));

        }
    }
}
