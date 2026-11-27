using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CgbSteps.Api.Helpers;

/// <summary>Rules for employee fields, shared by the API and the CSV import.</summary>
public static partial class EmployeeRules
{
    public const string EmployeeIdPattern = "^[A-Za-z0-9-]{1,20}$";
    public const int FullNameMinLength = 2;
    public const int FullNameMaxLength = 200;
    public const int WorkEmailMaxLength = 254;
    public const int DepartmentNameMaxLength = 100;

    private static readonly EmailAddressAttribute EmailValidator = new();

    public static string NormalizeEmployeeId(string value) => value.Trim().ToUpperInvariant();

    [SuppressMessage("Globalization", "CA1308", Justification = "Emails are stored and compared in lower case.")]
    public static string NormalizeEmail(string value) => value.Trim().ToLowerInvariant();

    public static bool IsValidEmployeeId(string value) => EmployeeIdRegex().IsMatch(value);

    public static bool IsValidFullName(string value) =>
        value.Length is >= FullNameMinLength and <= FullNameMaxLength;

    public static bool IsValidEmail(string value) =>
        value.Length <= WorkEmailMaxLength && EmailValidator.IsValid(value);

    public static bool IsValidDepartmentName(string value) =>
        value.Length is >= 2 and <= DepartmentNameMaxLength;

    [GeneratedRegex(EmployeeIdPattern)]
    private static partial Regex EmployeeIdRegex();
}
