using CgbSteps.Api.Models.Dtos;

namespace CgbSteps.Api.Services.Interfaces;

public interface ISystemService
{
    SystemInfoResponse GetInfo();
}
