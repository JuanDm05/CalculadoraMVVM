using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CalculadoraMVVM.VistaModelo
{
    class VMcalcular : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

      



        private string _display = "0";
        private double _numeroUno, _numeroDos;
        private string _operador;
        private bool _isNumeroUnoSet = false;

        private string _selectedOperator;
        public string SelectedOperator
        {
            get => _selectedOperator;
            set
            {
                if (_selectedOperator != value)
                {
                    _selectedOperator = value;
                    OnPropertyChanged(nameof(SelectedOperator));
                }
            }
        }
        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged(nameof(Display));
            }
        }

        public ICommand AddNumberCommand { get; }
        public ICommand SetOperationCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ClearCommand { get; }

        public VMcalcular()
        {
            AddNumberCommand = new Command<string>(AgregarNumero);
            SetOperationCommand = new Command<string>(EstablecerOperacion);
            CalculateCommand = new Command(Calcular);

            ClearCommand = new Command(Limpiar);
        }

        private void AgregarNumero(string numero)
        {
            SelectedOperator = null;

            if (numero == "." && Display.Contains("."))
            {
                return;
            }

            if (Display == "0" && numero != ".")
                Display = numero;
            else
                Display += numero;
        }

        private void EstablecerOperacion(string operacion)
        {
            SelectedOperator = operacion;
           
            if (!_isNumeroUnoSet)
            {
                _numeroUno = Convert.ToDouble(Display);
                _isNumeroUnoSet = true;
                Display = "0";
            }
            _operador = operacion;
        }

        private void Calcular()
        {
            if (!_isNumeroUnoSet) return;

            if (!double.TryParse(Display, out _numeroDos))
            {
                Display = "Error";
                return;
            }

            double resultado = 0;
            switch (_operador)
            {
                case "+":
                    resultado = _numeroUno + _numeroDos;
                    break;
                case "-":
                    resultado = _numeroUno - _numeroDos;
                    break;
                case "*":
                    resultado = _numeroUno * _numeroDos;
                    break;
                case "/":
                    if (_numeroDos != 0)
                        resultado = _numeroUno / _numeroDos;
                    else
                    {
                        Display = "Error";
                        return;
                    }
                    break;
            }

            Display = resultado.ToString();
            _isNumeroUnoSet = false;
        }


        private void Limpiar()
        {
            Display = "0";
            _numeroUno = 0;
            _numeroDos = 0;
            _operador = string.Empty;
            _isNumeroUnoSet = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
