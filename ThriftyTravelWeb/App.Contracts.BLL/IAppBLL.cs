using App.Contracts.BLL.Services;

namespace App.Contracts.BLL;

public interface IAppBLL
{
    ITripService Trips { get; }
}