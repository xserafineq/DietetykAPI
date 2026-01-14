using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietetykAPI.Models.Records
{
    public record EmployeeRecord(int EmployeeId, string firstName, string lastName, string email, string password);
}
