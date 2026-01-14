using DietetykAPI.Models.Entities;

namespace DietetykAPI.Models.Records
{
    public record RegisterVisitRecord(DateTimeOffset Date, int EmployeeId, string CustomerPesel);
}
