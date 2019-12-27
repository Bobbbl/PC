using HelixToolkit.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PC
{
    public class ToolbarViewModel : BaseViewModel
    {
        #region Static INotifyPropertyChanged
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string PropName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
            {
                handler(typeof(ToolbarViewModel), new PropertyChangedEventArgs(PropName));
            }
        }
        #endregion


        public int StepSizeJog { get; set; } = 1;

        public int FeedRateJog { get; set; } = 100;

        public int SpindelSpeed { get; set; } = 1000;

        public bool SpindelIsOn { get; set; }

        public string CustomLineContent { get; set; }

        public double StreamProgress { get; set; } = 0;

        public string CNCFileContent { get; set; }
        public ObservableCollection<string> CNCFileContentArray { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Point3D> SegmentList { get; set; } = new ObservableCollection<Point3D>();


        private static bool _IsConnected = false;
        public static bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                if (value != _IsConnected)
                {
                    _IsConnected = value;
                    RaiseStaticPropertyChanged("IsConnected");
                }

            }
        }


        private GlobalKeyboardHook _GlobalKeyboardHook;


        #region Commands
        public ICommand SetZeroCommand { get; set; }
        public ICommand HomingCommand { get; set; }
        public ICommand UnlockCommand { get; set; }
        public ICommand XMinusCommand { get; set; }
        public ICommand XPlusCommand { get; set; }
        public ICommand YPlusCommand { get; set; }
        public ICommand YMinusCommand { get; set; }
        public ICommand ZPlusCommand { get; set; }
        public ICommand ZMinusCommand { get; set; }
        public ICommand SpindelCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand SendLineButtonCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public async Task SetZero()
        {
            await PresentViewModel.Device.SendSetZero();
        }

        public async Task Homing()
        {
            await PresentViewModel.Device.DoHoming();
        }

        public async Task Unlock()
        {
            await PresentViewModel.Device.KillAlarm();
        }

        public async Task XMinus()
        {
            await PresentViewModel.Device.RelativeJogX(-StepSizeJog, FeedRateJog);
        }

        public async Task XPlus()
        {
            await PresentViewModel.Device.RelativeJogX(StepSizeJog, FeedRateJog);
        }

        public async Task YPlus()
        {
            await PresentViewModel.Device.RelativeJogY(StepSizeJog, FeedRateJog);
        }

        public async Task YMinus()
        {
            await PresentViewModel.Device.RelativeJogY(-StepSizeJog, FeedRateJog);
        }

        public async Task ZPlus()
        {
            await PresentViewModel.Device.RelativeJogZ(StepSizeJog, FeedRateJog);
        }

        public async Task ZMinus()
        {
            await PresentViewModel.Device.RelativeJogZ(-StepSizeJog, FeedRateJog);
        }

        public async Task Spindel()
        {
            await PresentViewModel.Device.SetSpindleSpeed(SpindelSpeed, "clockwise");
        }

        public async Task Reset()
        {
            await PresentViewModel.Device.SendSoftReset();
        }

        public async Task SendLine()
        {
            CNCMessage m = new CNCMessage() { Message = CustomLineContent };
            await PresentViewModel.Device.SendCustomMessage(m);
        }

        public async Task Load()
        {

            #region Read File Content

            OpenFileDialog odialog = new OpenFileDialog();
            if (odialog.ShowDialog() == true)
            {
                using (StreamReader sr = File.OpenText(odialog.FileName))
                {
                    CNCFileContent = string.Empty;
                    CNCFileContentArray = new ObservableCollection<string>(Regex.Split(await sr.ReadToEndAsync(), "\r\n|\r|\n"));


                    int e = 0;
                    foreach (string item in CNCFileContentArray)
                    {
                        CNCFileContent += item + '\n';

                        if (e > 1000)
                            break;

                        e++;
                    }
                }
            }

            #endregion

            PlotViewModel.PointList.Clear();
            PlotViewModel.LineList.Clear();

            await Task.Run(() =>
            {

                #region Get Points

                List<Point3D> coll = new List<Point3D>();

                Point3D currentcoor = new Point3D(0, 0, 0);

                Point3D lastcoor = new Point3D(0, 0, 0);


                string xstr = string.Empty, ystr = string.Empty, zstr = string.Empty;
                double x, y, z;

                string currmovmodstr = "g90";
                SelectedPlane currplane = SelectedPlane.XY;

                foreach (string item in CNCFileContentArray)
                {

                    // Remove Comments
                    var ite = Regex.Replace(item, @"\(.*?\)", "");

                    // check for global/relativ instruction
                    string ret = Regex.Match(ite, @"(G90)|(G91)").Groups[0].Value.ToLower();
                    if (ret.ToLower().Contains("g90"))
                    {
                        currmovmodstr = "g90";
                    }
                    else if (ret.ToLower().Contains("g91"))
                        currmovmodstr = "g91";

                    ret = Regex.Match(ite, @"(G17)|(G18)|(G19)").Groups[0].Value.ToLower();
                    //var gr = Regex.Match(ite, @"(G17)|(G18)|(G19)").Groups;
                    if (ret.ToLower().Contains("g17"))
                    {
                        currplane = SelectedPlane.XY;
                    }
                    else if (ret.ToLower().Contains("g18"))
                    {
                        currplane = SelectedPlane.ZX;
                    }
                    else if (ret.ToLower().Contains("g19"))
                    {
                        currplane = SelectedPlane.YZ;
                    }

                    // Find possible X
                    xstr = Regex.Match(ite, @"X(-?[0-9]*.[0-9]*)").Groups[1].Value;
                    ystr = Regex.Match(ite, @"Y(-?[0-9]*.[0-9]*)").Groups[1].Value;
                    zstr = Regex.Match(ite, @"Z(-?[0-9]*.[0-9]*)").Groups[1].Value;

                    // Save last coordinate
                    lastcoor.X = currentcoor.X;
                    lastcoor.Y = currentcoor.Y;
                    lastcoor.Z = currentcoor.Z;


                    if (!string.IsNullOrEmpty(xstr) || !string.IsNullOrEmpty(ystr) || !string.IsNullOrEmpty(zstr))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(xstr))
                            {
                                x = double.Parse(xstr);
                                if (currmovmodstr == "g90")
                                    currentcoor.X = x;
                                else
                                    currentcoor.Y += x;
                            }
                            else
                            {
                                x = currentcoor.X;
                            }
                        }
                        catch (FormatException ex)
                        {
                            x = currentcoor.X;
                        }
                        try
                        {
                            if (!string.IsNullOrEmpty(ystr))
                            {
                                y = double.Parse(ystr);
                                if (currmovmodstr == "g90")
                                    currentcoor.Y = y;
                                else
                                    currentcoor.Y += y;
                            }
                            else
                            {
                                y = currentcoor.Y;
                            }
                        }
                        catch (FormatException ex)
                        {
                            y = currentcoor.Y;
                        }
                        try
                        {
                            if (!string.IsNullOrEmpty(zstr))
                            {
                                z = double.Parse(zstr);
                                if (currmovmodstr == "g90")
                                    currentcoor.Z = z;
                                else
                                    currentcoor.Z += z;
                            }
                            else
                            {
                                z = currentcoor.Z;
                            }

                        }
                        catch (FormatException ex)
                        {
                            z = currentcoor.Z;
                        }



                        var p = new Point3D();
                        p.X = currentcoor.X;
                        p.Y = currentcoor.Y;
                        p.Z = currentcoor.Z;

                        /*
                        * Usage
                        * G2 Xnnn Ynnn Innn Jnnn Ennn Fnnn (Clockwise Arc)
                        * G3 Xnnn Ynnn Innn Jnnn Ennn Fnnn (Counter-Clockwise Arc)
                        * Parameters
                        * Xnnn The position to move to on the X axis
                        * Ynnn The position to move to on the Y axis
                        * Innn The point in X space from the current X position to maintain a constant distance from
                        * Jnnn The point in Y space from the current Y position to maintain a constant distance from
                        * Ennn The amount to extrude between the starting point and ending point
                        * Fnnn The feedrate per minute of the move between the starting point and ending point (if supplied)
                        */
                        //Check if a line or a arc
                        var g = Regex.Match(ite, @"([G][2]|[G][3]) ").Groups;
                        string arcstr = Regex.Match(ite, @"([G][2]|[G][3]) ").Groups[0].Value.ToLower();
                        Point3D end;
                        Point3D center;
                        Point3D start;
                        List<Point3D> plist = new List<Point3D>();
                        if (!string.IsNullOrEmpty(arcstr))
                        {
                            string istr = Regex.Match(ite, @"I(-?[0-9]*.[0-9]*)").Groups[1].Value;
                            string jstr = Regex.Match(ite, @"J(-?[0-9]*.[0-9]*)").Groups[1].Value;
                            string rstr = Regex.Match(ite, @"R(-?[0-9]*.[0-9]*)").Groups[1].Value;

                            double I = 0, J = 0, R = 0;

                            if (!string.IsNullOrEmpty(istr) || !string.IsNullOrEmpty(jstr))
                            {
                                I = double.Parse(istr);
                                J = double.Parse(jstr);
                            }
                            else if (!string.IsNullOrEmpty(rstr))
                            {
                                R = double.Parse(rstr);
                            }

                            if (!string.IsNullOrEmpty(istr) || !string.IsNullOrEmpty(jstr))
                            {
                                start = new Point3D(lastcoor.X, lastcoor.Y, lastcoor.Z);
                                end = new Point3D(currentcoor.X, currentcoor.Y, currentcoor.Z);
                                center = new Point3D(lastcoor.X + I, lastcoor.Y + J, lastcoor.Z);

                                plist = Arc(start, end, center, 10, arcstr.ToLower().Contains("g2"), currplane);
                            }
                            else if (!string.IsNullOrEmpty(rstr))
                            {
                                start = new Point3D(lastcoor.X, lastcoor.Y, lastcoor.Z);
                                end = new Point3D(currentcoor.X, currentcoor.Y, currentcoor.Z);
                                plist = ArcR(start, end, 10, arcstr.ToLower().Contains("g2"), currplane);
                            }

                        }

                        foreach (var il in plist)
                        {
                            coll.Add(il);
                        }
                        coll.Add(new Point3D(currentcoor.X, currentcoor.Y, currentcoor.Z));
                    }



                }

                List<Point3D> llist = new List<Point3D>();

                Point3D firstpoint, secondpoint;

                for (int i = 0; i < coll.Count - 1; i++)
                {
                    firstpoint = coll[i];
                    secondpoint = coll[i + 1];

                    // Add Line

                    llist.Add(firstpoint);
                    llist.Add(secondpoint);
                }

                PlotViewModel.LineList = llist;
                PlotViewModel.PointList = coll;


                #endregion

            });



        }

        public double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        public Vector RotateVector2d(double x, double y, double angle)
        {
            double[] result = new double[2];
            result[0] = x * Math.Cos(angle) - y * Math.Sin(angle);
            result[1] = x * Math.Sin(angle) + y * Math.Cos(angle);
            return new Vector(result[0], result[1]);
        }

        public double SignedAngle1(Vector a, Vector b)
        {
            return Math.Atan2(b.Y, b.X) - Math.Atan2(a.Y, a.X);
        }

        public double SignedAngle2(Vector a, Vector b)
        {
            return Math.Atan2(a.X * b.Y - a.Y * b.X, a.X * b.X + a.Y * b.Y);
        }

        public List<Point3D> ArcR(Point3D Start, Point3D End, int NumPoints, bool Clockwise, SelectedPlane Plane = SelectedPlane.XY)
        {
            var list = new List<Point3D>(NumPoints);


            /* Create Vectors */
            Vector ortsvectorcenter = new Vector();
            Vector ortsvectorstart = new Vector();
            Vector ortsvectorend = new Vector();

            switch (Plane)
            {
                case SelectedPlane.XY:
                    ortsvectorstart = new Vector(Start.X, Start.Y);
                    ortsvectorend = new Vector(End.X, End.Y);
                    break;
                case SelectedPlane.ZX:
                    // ZX
                    ortsvectorstart = new Vector(Start.X, Start.Z);
                    ortsvectorend = new Vector(End.X, End.Z);
                    break;
                case SelectedPlane.YZ:
                    // YZ
                    ortsvectorstart = new Vector(Start.Y, Start.Z);
                    ortsvectorend = new Vector(End.Y, End.Z);
                    break;
                default:
                    ortsvectorstart = new Vector(Start.X, Start.Y);
                    ortsvectorend = new Vector(End.X, End.Y);
                    break;
            }

            var vectorstartend = ortsvectorend - ortsvectorstart;
            ortsvectorcenter = ortsvectorstart + vectorstartend * 0.5;

            // Vectors for use
            Vector start = ortsvectorstart - ortsvectorcenter;
            Vector end = ortsvectorend - ortsvectorcenter;

            /* Calculate Angle between the start and the end vector beginning at the rotation
             * center point*/

            double alpha = 0;
            alpha = SignedAngle2(start, end);



            // If alpha <0 then it is in clockwise direction. 
            // If alpha > 0 then it is in counterclockwise direction
            if (Clockwise)
            {
                if (alpha > 0)
                {
                    alpha = Math.PI * 2 - alpha;
                    alpha *= -1;
                }
            }
            else
            {
                if (alpha < 0)
                    alpha = Math.PI * 2 + alpha;
            }

            if (alpha == 0)
                return list;

            /*Rotate vector step by step. For this we calculate an vector(array) with angles which should be used to rotate the vector to*/
            var anglesteps = alpha / (NumPoints); // -1 'cause start is already included by for - loop
            Vector ortsvectorrotated = new Vector();

            if (alpha < 0)
            {
                for (double w = 0; w >= alpha; w += anglesteps)
                {
                    Vector coor = RotateVector2d(start.X, start.Y, w);
                    ortsvectorrotated = coor + ortsvectorcenter;
                    switch (Plane)
                    {
                        case SelectedPlane.XY:
                            list.Add(new Point3D(ortsvectorrotated.X, ortsvectorrotated.Y, Start.Z));
                            break;
                        case SelectedPlane.ZX:
                            list.Add(new Point3D(ortsvectorrotated.X, Start.Y, ortsvectorrotated.Y));
                            break;
                        case SelectedPlane.YZ:
                            list.Add(new Point3D(Start.X, ortsvectorrotated.X, ortsvectorrotated.Y));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                for (double w = 0; w <= alpha; w += anglesteps)
                {
                    Vector coor = RotateVector2d(start.X, start.Y, w);
                    ortsvectorrotated = coor + ortsvectorcenter;
                    switch (Plane)
                    {
                        case SelectedPlane.XY:
                            list.Add(new Point3D(ortsvectorrotated.X, ortsvectorrotated.Y, Start.Z));
                            break;
                        case SelectedPlane.ZX:
                            list.Add(new Point3D(ortsvectorrotated.X, Start.Y, ortsvectorrotated.Y));
                            break;
                        case SelectedPlane.YZ:
                            list.Add(new Point3D(Start.X, ortsvectorrotated.X, ortsvectorrotated.Y));
                            break;
                        default:
                            break;
                    }
                }
            }







            return list;

        }

        public List<Point3D> Arc(Point3D Start, Point3D End, Point3D Center, int NumPoints, bool clockwise, SelectedPlane Plane = SelectedPlane.XY)
        {
            var list = new List<Point3D>(NumPoints);

            // TODO: Check R same for both

            /* Create Vectors */
            Vector ortsvectorcenter = new Vector();
            Vector ortsvectorstart = new Vector();
            Vector ortsvectorend = new Vector();

            /*Select Plane*/

            // XY
            // lokal vectors

            switch (Plane)
            {
                case SelectedPlane.XY:
                    ortsvectorcenter = new Vector(Center.X, Center.Y);
                    ortsvectorstart = new Vector(Start.X, Start.Y);
                    ortsvectorend = new Vector(End.X, End.Y);
                    break;
                case SelectedPlane.ZX:
                    // ZX
                    ortsvectorcenter = new Vector(Center.X, Center.Z);
                    ortsvectorstart = new Vector(Start.X, Start.Z);
                    ortsvectorend = new Vector(End.X, End.Z);
                    break;
                case SelectedPlane.YZ:
                    // YZ
                    ortsvectorcenter = new Vector(Center.Y, Center.Z);
                    ortsvectorstart = new Vector(Start.Y, Start.Z);
                    ortsvectorend = new Vector(End.Y, End.Z);
                    break;
                default:
                    ortsvectorcenter = new Vector(Center.X, Center.Y);
                    ortsvectorstart = new Vector(Start.X, Start.Y);
                    ortsvectorend = new Vector(End.X, End.Y);
                    break;
            }



            // Vectors for use
            Vector start = ortsvectorstart - ortsvectorcenter;
            Vector end = ortsvectorend - ortsvectorcenter;

            /* Calculate Angle between the start and the end vector beginning at the rotation
             * center point*/

            double alpha = 0;
            alpha = SignedAngle2(start, end);




            // If alpha <0 then it is in clockwise direction. 
            // If alpha > 0 then it is in counterclockwise direction
            if (clockwise)
            {
                if (alpha > 0)
                {
                    alpha = Math.PI * 2 - alpha;
                    alpha *= -1;
                }
                else if (alpha == 0)
                    alpha = Math.PI * -2;
            }
            else
            {
                if (alpha < 0)
                    alpha = Math.PI * 2 + alpha;
                else if (alpha == 0)
                    alpha = Math.PI * 2;

            }

            if (alpha == 0)
                return list;


            /*Rotate vector step by step. For this we calculate an vector(array) with angles which should be used to rotate the vector to*/
            var anglesteps = alpha / (NumPoints); // -1 'cause start is already included by for - loop
            Vector ortsvectorrotated = new Vector();

            if (alpha < 0)
            {
                for (double w = 0; w >= alpha; w += anglesteps)
                {
                    Vector coor = RotateVector2d(start.X, start.Y, w);
                    ortsvectorrotated = coor + ortsvectorcenter;
                    switch (Plane)
                    {
                        case SelectedPlane.XY:
                            list.Add(new Point3D(ortsvectorrotated.X, ortsvectorrotated.Y, Start.Z));
                            break;
                        case SelectedPlane.ZX:
                            list.Add(new Point3D(ortsvectorrotated.X, Start.Y, ortsvectorrotated.Y));
                            break;
                        case SelectedPlane.YZ:
                            list.Add(new Point3D(Start.X, ortsvectorrotated.X, ortsvectorrotated.Y));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                for (double w = 0; w <= alpha; w += anglesteps)
                {
                    Vector coor = RotateVector2d(start.X, start.Y, w);
                    ortsvectorrotated = coor + ortsvectorcenter;
                    switch (Plane)
                    {
                        case SelectedPlane.XY:
                            list.Add(new Point3D(ortsvectorrotated.X, ortsvectorrotated.Y, Start.Z));
                            break;
                        case SelectedPlane.ZX:
                            list.Add(new Point3D(ortsvectorrotated.X, Start.Y, ortsvectorrotated.Y));
                            break;
                        case SelectedPlane.YZ:
                            list.Add(new Point3D(Start.X, ortsvectorrotated.X, ortsvectorrotated.Y));
                            break;
                        default:
                            break;
                    }
                }
            }


            return list;
        }

        public async Task Send()
        {
            PresentViewModel.Device.CurrentMode = CommModes.SendMode;

            CNCMessage message0 = new CNCMessage() { Message = CNCFileContentArray[0] };
            CNCMessage answer = new CNCMessage();
            CNCMessage posrequmessage = new CNCMessage();
            CNCMessage posmessage = new CNCMessage();
            CNCMessage rmessage = new CNCMessage() { Message = string.Empty };

            CNC_Device Device = PresentViewModel.Device;
            CNCInterface Interface = Device.Interface;

            long count = 0;
            long maxcount = CNCFileContentArray.Count;
            int okcount = 0;
            float[] r = new float[3];
            int wcount = 0;
            int recd = 0;

            await Task.Run(async () =>
            {

                foreach (var item in CNCFileContentArray)
                {

                    // Cancel Routine
                    if (CancelSend)
                    {
                        CancelSend = false;
                        StreamProgress = 1;
                        PresentViewModel.Device.CurrentMode = CommModes.DefaultMode;
                        return;
                    }

                    message0.Message = item;

                    PresentViewModel.Device.Interface.SendMessage(message0, false);
                    okcount++;

                    posrequmessage = PresentViewModel.Device.Protokoll.GetCurrentXYZMessage();
                    Interface.SendMessage(posrequmessage, false);
                    okcount++;


                    while (okcount > 0)
                    {

                        if (CancelSend)
                        {
                            CancelSend = false;
                            StreamProgress = 1;
                            PresentViewModel.Device.CurrentMode = CommModes.DefaultMode;
                            await PresentViewModel.Device.SendSoftReset();
                            Thread.Sleep(1000);
                            await PresentViewModel.Device.KillAlarm();
                            return;
                        }

                        rmessage = new CNCMessage();
                        rmessage = Interface.ReceiveMessage(100, false);

                        if (rmessage.Message.Contains("ok") || rmessage.Message.Contains("error"))
                        {
                            okcount--;

                        }
                        else if (rmessage.Message.Contains("WPos"))
                        {
                            try
                            {
                                r[0] = Convert.ToSingle(Regex.Match(rmessage.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[2].Value, CultureInfo.InvariantCulture);
                                r[1] = Convert.ToSingle(Regex.Match(rmessage.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[3].Value, CultureInfo.InvariantCulture);
                                r[2] = Convert.ToSingle(Regex.Match(rmessage.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[4].Value, CultureInfo.InvariantCulture);


                                PresentViewModel.Device.CurrentX = r[0];
                                PresentViewModel.Device.CurrentY = r[1];
                                PresentViewModel.Device.CurrentZ = r[2];
                            }
                            catch (Exception ex)
                            {
                                r[0] = (float)PresentViewModel.CurrentX;
                                r[1] = (float)PresentViewModel.CurrentY;
                                r[2] = (float)PresentViewModel.CurrentZ;
                            }


                            recd++;

                        }

                        if (wcount > 2 && recd != 0)
                        {
                            Interface.SendMessage(posrequmessage, false);
                            okcount++;
                            wcount = 0;
                            recd = 0;
                        }
                        wcount++;

                    }




                    StreamProgress = (double)count / (double)maxcount;

                    count++;

                }

            });

            StreamProgress = 1;
            PresentViewModel.Device.CurrentMode = CommModes.DefaultMode;

        }

        private bool CancelSend = false;
        public async Task Cancel()
        {
            CancelSend = true;
            PresentViewModel.Device.CurrentMode = CommModes.DefaultMode;

        }

        #endregion


        #region Constructor ToolbarViewModel
        public ToolbarViewModel()
        {
            SetZeroCommand = new RelayCommand(async () => await SetZero());
            (SetZeroCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            HomingCommand = new RelayCommand(async () => await Homing());
            (HomingCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            UnlockCommand = new RelayCommand(async () => await Unlock());
            (UnlockCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            XMinusCommand = new RelayCommand(async () => await XMinus());
            (XMinusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            XPlusCommand = new RelayCommand(async () => await XPlus());
            (XPlusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            YPlusCommand = new RelayCommand(async () => await YPlus());
            (YPlusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            YMinusCommand = new RelayCommand(async () => await YMinus());
            (YMinusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            ZPlusCommand = new RelayCommand(async () => await ZPlus());
            (ZPlusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            ZMinusCommand = new RelayCommand(async () => await ZMinus());
            (ZMinusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            SpindelCommand = new RelayCommand(async () => await Spindel());
            (SpindelCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            ResetCommand = new RelayCommand(async () => await Reset());
            (ResetCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            SendLineButtonCommand = new RelayCommand(async () => await SendLine());
            (SendLineButtonCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            LoadCommand = new RelayCommand(async () => await Load());

            SendCommand = new RelayCommand(async () => await Send());
            (SendCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            CancelCommand = new RelayCommand(async () => await Cancel());
            //(CancelCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;


            StaticPropertyChanged += ToolbarViewModel_StaticPropertyChanged;


            // Global Keyboard Hooks
            _GlobalKeyboardHook = new GlobalKeyboardHook();
            _GlobalKeyboardHook.KeyboardPressed += _GlobalKeyboardHook_KeyboardPressed;


        }
        #endregion

        public bool WhenSendButtonEnabled()
        {
            if (PresentViewModel.Device == null)
                return false;

            if (IsConnected && (PresentViewModel.Device.CurrentMode == CommModes.DefaultMode))
                return true;
            else
                return false;
        }

        private void _GlobalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                try
                {
                    var focusedelementtype = Keyboard.FocusedElement.GetType();
                    if (focusedelementtype != typeof(System.Windows.Controls.TextBox) &&
                   focusedelementtype != typeof(HelixToolkit.Wpf.CameraController))
                    {
                        if (Keyboard.IsKeyDown(Key.Left))
                        {
                            if (XMinusCommand.CanExecute(null))
                                XMinusCommand.Execute(null);
                        }
                        if (Keyboard.IsKeyDown(Key.Right))
                        {
                            if (XPlusCommand.CanExecute(null))
                                XPlusCommand.Execute(null);
                        }
                        if (Keyboard.IsKeyDown(Key.Down))
                        {
                            if (YMinusCommand.CanExecute(null))
                                YMinusCommand.Execute(null);
                        }
                        if (Keyboard.IsKeyDown(Key.Up))
                        {
                            if (YPlusCommand.CanExecute(null))
                                YPlusCommand.Execute(null);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    return;
                }


            }
        }

        private void ToolbarViewModel_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsConnected):
                    if (IsConnected)
                    {
                        PortViewModel.PortisList.ForEach(porti => porti.IndicatorColor = Brushes.Transparent);

                        if (PresentViewModel.Device == null)
                        {
                            return;
                        }

                        XAMLFiles.Portis p = PortViewModel.PortisList.Find(
                            porti =>
                            PresentViewModel.Device.Interface.Portname == porti.PortName);
                        p.IndicatorColor = Brushes.ForestGreen;
                    }
                    else
                    {
                        PortViewModel.PortisList.ForEach(porti => porti.IndicatorColor = Brushes.Transparent);
                    }

                    (SetZeroCommand as RelayCommand).FireCanExecuteChanged();
                    (HomingCommand as RelayCommand).FireCanExecuteChanged();
                    (UnlockCommand as RelayCommand).FireCanExecuteChanged();
                    (XMinusCommand as RelayCommand).FireCanExecuteChanged();
                    (XPlusCommand as RelayCommand).FireCanExecuteChanged();
                    (YPlusCommand as RelayCommand).FireCanExecuteChanged();
                    (YMinusCommand as RelayCommand).FireCanExecuteChanged();
                    (ZPlusCommand as RelayCommand).FireCanExecuteChanged();
                    (ZMinusCommand as RelayCommand).FireCanExecuteChanged();
                    (SpindelCommand as RelayCommand).FireCanExecuteChanged();
                    (ResetCommand as RelayCommand).FireCanExecuteChanged();
                    (SendLineButtonCommand as RelayCommand).FireCanExecuteChanged();
                    (SendCommand as RelayCommand).FireCanExecuteChanged();

                    break;
                default:
                    break;
            }
        }
    }
}
