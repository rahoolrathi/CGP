using System.Reflection;
using CgbSteps.Application.Abstractions;

namespace CgbSteps.Api.Infrastructure;

internal sealed class AssemblyApplicationInfo : IApplicationInfo
{
    public string Version { get; } =
        typeof(AssemblyApplicationInfo).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion.Split('+')[0]
        ?? "0.0.0";
}
