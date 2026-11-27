namespace CgbSteps.Api.Helpers;

public static class AppConstants
{
    public const string AdminWebCorsPolicy = "admin-web";
    public const string ReadyHealthTag = "ready";

    public static class DatabaseKeys
    {
        public const string App = "DATABASE_URL";
        public const string Migrations = "MIGRATIONS_DATABASE_URL";
    }

    public static class ErrorCodes
    {
        public const string BadRequest = "bad_request";
        public const string Unauthorized = "unauthorized";
        public const string Forbidden = "forbidden";
        public const string NotFound = "not_found";
        public const string MethodNotAllowed = "method_not_allowed";
        public const string RateLimited = "rate_limited";
        public const string ServerError = "server_error";
        public const string Conflict = "conflict";

        public const string DepartmentNotFound = "department_not_found";
        public const string DepartmentExists = "department_exists";
        public const string DepartmentHasEmployees = "department_has_employees";
        public const string EmployeeNotFound = "employee_not_found";
        public const string EmployeeIdExists = "employee_id_exists";
        public const string WorkEmailExists = "work_email_exists";

        public const string FileMissing = "file_missing";
        public const string FileTooLarge = "file_too_large";
        public const string CsvHeaderInvalid = "csv_header_invalid";
        public const string CsvTooManyRows = "csv_too_many_rows";
        public const string InvalidEmployeeId = "invalid_employee_id";
        public const string InvalidFullName = "invalid_full_name";
        public const string InvalidWorkEmail = "invalid_work_email";
        public const string InvalidDepartment = "invalid_department";
        public const string DuplicateInFile = "duplicate_in_file";

        public const string ActivityDateOutOfRange = "activity_date_out_of_range";
    }
}
