using System.Windows.Controls;
using Calculator.Contracts;
using Calculator.Models;

namespace Calculator.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private decimal _answer;
        private EquationModel _equation;
        private string _status;

        public MainViewModel(IMathService mathService)
        {
            Status = "Initializing...";
            MathService = mathService;
        }

        private IMathService MathService { get; }

        public EquationModel Equation
        {
            get => _equation;
            set
            {
                _equation = value;
                NotifyOfPropertyChange();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyOfPropertyChange();
            }
        }

        public decimal Answer
        {
            get => _answer;
            set
            {
                _answer = value;
                NotifyOfPropertyChange();
            }
        }

        public void Loaded()
        {
            DisplayName = "Calculator";
            Status = "Ready";
        }

        public void ConfigureEquation(object sender)
        {
            if (!(sender is Button button))
            {
                return;
            }

            var input = button.Content.ToString().ToLower();
            if (input == "=")
            {
                Answer = MathService.Calculate(Equation);
                Equation = new EquationModel();
                return;
            }

            Equation = MathService.BuildEquation(Equation, input);
            if (input == "ce")
            {
                Answer = 0;
            }
        }
    }
}