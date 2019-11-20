using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PC
{
    class ConsoleViewModel : BaseViewModel
    {
        #region Static INotifyPropertyChanged

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string PropName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(PropName);
                handler(typeof(ConsoleViewModel), e);
            }
        }

        #endregion

        private static bool _Verbose = false;
        public static bool Verbose
        {
            get
            {
                return _Verbose;
            }
            set
            {
                if (_Verbose != value)
                {
                    _Verbose = value;
                    RaiseStaticPropertyChanged(nameof(Verbose));
                }
            }
        }

        public ICommand ClearCommand { get; set; }

        private async Task Clear()
        {
            await Task.Run(() =>
            {
                ConsoleContent = string.Empty;

                if (PresentViewModel.Device != null)
                    PresentViewModel.Device.SendReceiveBuffer.Clear();
            });

        }

        private static string _ConsoleContent = string.Empty;
        public static string ConsoleContent
        {
            get
            {
                return _ConsoleContent;
            }
            set
            {
                _ConsoleContent = value;
                RaiseStaticPropertyChanged(nameof(ConsoleContent));
            }
        }


        public ConsoleViewModel()
        {
            ClearCommand = new RelayCommand(() => Clear());
        }

    }
}
