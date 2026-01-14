using DietetykAPI.Models.Entities;

namespace DietetykAPI.Models.Records
{
    public record VisitRecord(int VisitId, DateTimeOffset Date, int EmployeeId,
        string CustomerPesel,string status, Customer Customer, MedicalRecomendations Recomendation);
}
