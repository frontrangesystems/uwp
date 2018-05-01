using Caliburn.Micro;

namespace Calculator.Models
{
    public class EquationModel : PropertyChangedBase
    {
        private string _operator;

        private string _source1;

        private string _source2;

        public decimal Argument1
        {
            get
            {
                decimal.TryParse(Source1, out var number);
                return number;
            }
        }

        public decimal Argument2
        {
            get
            {
                decimal.TryParse(Source2, out var number);
                return number;
            }
        }

        public string Operator
        {
            get => _operator;
            set
            {
                _operator = value;
                NotifyOfPropertyChange();
            }
        }

        public string EquationText => $"{Source1} {Operator} {Source2}".Trim();

        public string Source1
        {
            get => _source1;
            set
            {
                _source1 = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(Argument1));
                NotifyOfPropertyChange(nameof(EquationText));
            }
        }

        public string Source2
        {
            get => _source2;
            set
            {
                _source2 = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(Argument2));
                NotifyOfPropertyChange(nameof(EquationText));
            }
        }
    }
}