namespace DietetykAPI.Models.Records
{
    public record MedicalRecommendationRecord(int MedicalRecomendationsId, int dietId, string note, DateTimeOffset date);
}
