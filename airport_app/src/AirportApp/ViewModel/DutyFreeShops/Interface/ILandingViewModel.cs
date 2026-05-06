using System;
using System.Windows.Input;

namespace AirportApp.ViewModel.DutyFreeShops.Interface
{
    public interface ILandingViewModel
    {
        bool IsRoleSelected { get; }
        string ErrorMessage { get; }
        ICommand SelectAdminCommand { get; }
        ICommand SelectClientCommand { get; }
    }
}
