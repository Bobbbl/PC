using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PC.XAMLFiles
{
    /// <summary>
    /// Interaktionslogik für Portis.xaml
    /// </summary>
    public partial class Portis : UserControl, INotifyPropertyChanged
    {
        public event PortisEventClickedEventHandler PortisClickedEvent;


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
        #endregion


        public Brush IndicatorColor { get; set; } = Brushes.Transparent;
        public string PortName { get; set; } = "Default";
        public int BaudRate { get; set; } = 115200;

        public Brush BackgroundColor { get; set; } = Brushes.White;



        public Portis()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PresentViewModel.CurrentSelectedBaudRate = BaudRate;
            PresentViewModel.CurrentSelectedPortName = PortName;

        }
    }
}
