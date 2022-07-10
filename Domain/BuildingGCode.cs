using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    
    public class BuildingGCode
    {
        
        public BuildingGCode(double widthWorkZone, double heightWorkZone, double intervalPin, double ratioXY, double ratioZ,  double lengthHead, int[,] imagePixcelBright)
        {
            WidthWorkZone = widthWorkZone;
            HeightWorkZone = heightWorkZone;
            IntervalPin = intervalPin;
            RatioXY = ratioXY;
            RatioZ = ratioZ;
            LengthHead = lengthHead;
            ImagePixcelBright = imagePixcelBright;
            SetCountPin();
            FillListPin();
        }

        public double WidthWorkZone { get; set; }
        public double HeightWorkZone { get; set; }
        public double IntervalPin { get; set; }
        public double EdgeIndentWidth { get; set; }
        public double EdgeIndentHeight { get; set; }

        public double RatioXY { get; set; }
        public double RatioZ { get; set; }
        public double RatioAlfa { get; set; }

        public int CountPinX { get; set; }
        public int CountPinY { get; set; }

        public int CountPin { get; set; }


        public double LengthHead { get; set; }

        public double XCenterWorkZone { get; set; }
        public double YCenterWorkZone { get; set; }

        int[,] ImagePixcelBright { get; set; }


        public List<PinInfo> ListPin { get; set; } = new List<PinInfo>();

        public void SetCountPin()
        {
            CountPinX = (int)((WidthWorkZone - (2 * EdgeIndentWidth)) / IntervalPin);
            CountPinY = (int)((HeightWorkZone - (2 * EdgeIndentHeight)) / IntervalPin);

            CountPin = CountPinX * CountPinY;
        }

        public double[] GetXYPin(int x, int y)
        {
            double[] xy = { EdgeIndentWidth + x * IntervalPin, EdgeIndentHeight + y * IntervalPin };
            return xy;
        }

        public void FillListPin()
        {
            
            List<PinInfo> listPin = new List<PinInfo>();
           
            double[] vector0 = { WidthWorkZone - XCenterWorkZone, 0 };
            int n = 0;
            for ( int x = 0; x < CountPinX; x++)
            {
                for (int y = 0; y < CountPinY; y++)
                {
                    
                    PinInfo pin = new PinInfo(vector0, ++n, GetXYPin(x, y), XCenterWorkZone,  YCenterWorkZone, ImagePixcelBright[y,x]); // ++n, n++
                    listPin.Add(pin);
                }
            }

            ListPin = listPin;
        }

        
        public string Go_G0_XY_Coordinates(PinInfo pin)
        {
            return "G0 X" + pin.XY[0] * RatioXY + " Y" + pin.XY[1] * RatioXY + "\n";
        }
        public string Go_G0_XYA_Coordinates(PinInfo pin)
        {
            return "G0 X" + pin.XY[0] * RatioXY + " Y" + pin.XY[1] * RatioXY + " A" + pin.Alfa * RatioAlfa + "\n";
        }
        public string Go_G0_Z_Coordinates(PinInfo pin)
        {
            return "G0 Z" + pin.Z * RatioZ + "\n";
        }
        public string Go_G0_Z_Zero()
        {
            return "G0 Z0" + "\n";
        }
        //Запуск подпрограммы
        public string Go_Subroutine(PinInfo pin)
        {
            return "M98 P" + pin.ID + "\n";
        }
        public string Building_Subroutine(PinInfo pin)
        { 
            return "O" + pin.ID + "\n" + Go_G0_XYA_Coordinates(pin) + Go_G0_Z_Coordinates(pin) + Go_G0_Z_Zero() + "M99" + "\n";
        }

        public string EndProgram()
        {
            return "M02";
        }

        public string FinishProgram()
        {
            return "M30";
        }

        public List<string> StartBuildingGCode() 
        {
            List<string> listGCode = new List<string>();

            foreach (PinInfo pin in ListPin)
            {
                listGCode.Add(Go_Subroutine(pin));
            }
            listGCode.Add(EndProgram());

            foreach (PinInfo pin in ListPin)
            {
                listGCode.Add(Building_Subroutine(pin));
            }
            listGCode.Add(FinishProgram());

            return listGCode;
        }
    }
}
