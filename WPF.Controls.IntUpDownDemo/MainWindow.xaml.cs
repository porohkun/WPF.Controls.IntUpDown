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

        public int MinValue { get; private set; } = 1;


        public int MaxValue { get; private set; } = 9;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

    }
}
