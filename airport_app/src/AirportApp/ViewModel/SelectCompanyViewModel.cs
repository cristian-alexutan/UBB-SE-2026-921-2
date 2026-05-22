using System.ComponentModel;
using System.Runtime.CompilerServices;

using AirportApp.WinUI.Utils;

using CommunityToolkit.Mvvm.Input;

namespace AirportApp.ViewModel
{
    public partial class SelectCompanyViewModel : INotifyPropertyChanged
    {
        private readonly ICompanyService companyService;
        private readonly INavigationUtil navigationUtil;

        private ObservableCollection<Company> companies;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Company> Companies
        {
            get => companies;
            set
            {
                if (companies != value)
                {
                    companies = value;
                    OnPropertyChanged();
                }
            }
        }

        public SelectCompanyViewModel(ICompanyService companyService, INavigationUtil navigationUtil)
        {
            this.companyService = companyService;
            this.navigationUtil = navigationUtil;

            LoadCompanies();
        }

        private void LoadCompanies()
        {
            List<Company> availableCompanies = companyService.GetAllCompanies();
            Companies = new ObservableCollection<Company>(availableCompanies);
        }

        public IRelayCommand SelectCompanyCommand => new RelayCommand<Company>(SelectCompany);
        private void SelectCompany(Company company)
        {
            if (company != null)
            {
                navigationUtil.NavigateToCompanyDashboard(company.Id);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
