using Guybrush.SmartHome.Client.Data;
using Guybrush.SmartHome.Client.Data.Base;
using Guybrush.SmartHome.Client.Data.Managers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Guybrush.SmartHome.Client.UWP.ViewModels
{
    public class ConditionsViewModel : Observable
    {
        public ObservableCollection<ConditionViewModel> Conditions { get; set; }
        private ClientConditionManager _conditionManager;
        public ConditionsViewModel()
        {
            Conditions = new ObservableCollection<ConditionViewModel>();
        }
        private bool _isNewEnabled;

        public bool IsNewEnabled
        {
            get { return _isNewEnabled; }
            set { _isNewEnabled = value; OnPropertyChanged(); }
        }
        public async Task LoadConditions()
        {
            Conditions.Clear();
            _conditionManager = Context.Current.ConditionManager;
            if (_conditionManager != null)
            {
                if (_conditionManager.IsConnected)
                {
                    var conds = await _conditionManager.GetConditions();
                    foreach (var cond in conds)
                    {
                        Conditions.Add(new ConditionViewModel()
                        {
                            SourceDeviceType = cond.SourceDeviceType,
                            SourceDeviceName = cond.SourceDeviceName,
                            TargetDeviceName = cond.TargetDeviceName,
                            RequiredValue = cond.RequiredValue,
                            ConditionType = cond.ConditionType,
                            TargetValue = cond.TargetValue
                        });
                    }
                }
                else
                {

                    Conditions.Clear();
                }
            }
        }

        private ConditionViewModel _selectedCondition;
        public ConditionViewModel SelectedCondition
        {
            get { return _selectedCondition; }
            set
            {
                if (_selectedCondition != value)
                {
                    _selectedCondition = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsConditionSelected));
                }

            }
        }

        public bool IsConditionSelected
        {
            get { return SelectedCondition != null; }
        }

        private bool _isControlActive;

        public bool IsControlActive
        {
            get { return _isControlActive; }
            set { _isControlActive = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsControlDisabled)); }
        }

        public bool IsControlDisabled
        {
            get { return !_isControlActive; }
            set { IsControlDisabled = !value; OnPropertyChanged(); OnPropertyChanged(nameof(IsControlActive)); }
        }


    }
}
