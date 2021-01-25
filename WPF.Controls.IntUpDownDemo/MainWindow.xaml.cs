using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WPF.Controls.IntUpDownDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!storage.Equals(value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        private int _value=3;
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private int _minValue=1;
        public int MinValue
        {
            get => _minValue;
            set => SetProperty(ref _minValue, value);
        }

        private int _maxValue=9;
        public int MaxValue
        {
            get => _maxValue;
            set => SetProperty(ref _maxValue, value);
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

    }
}
