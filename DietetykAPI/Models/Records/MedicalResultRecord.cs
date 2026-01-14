using NuGet.Versioning;

namespace DietetykAPI.Models.Records
{
    public record MedicalResultRecord(int MedicalResultId, double weight, double height,double waistLine, double bodyFat,double sugarLevel, double bmi, DateTime date);
}
