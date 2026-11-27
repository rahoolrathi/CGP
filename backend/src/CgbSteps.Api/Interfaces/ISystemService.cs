using CgbSteps.Api.Models.Dtos;

namespace CgbSteps.Api.Interfaces;

public interface ISystemService
{
    SystemInfoResponse GetInfo();
}
