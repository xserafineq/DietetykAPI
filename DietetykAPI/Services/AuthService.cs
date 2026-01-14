using DietetykAPI.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService
{
    private readonly string _jwtKey;

    public AuthService(IConfiguration configuration)
    {
        _jwtKey = configuration["ApiSettings:MyApiKey"];
    }

    public string GenerateJwt(Employee employee)
    {
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", employee.EmployeeId.ToString()),           
                new Claim("firstName", employee.firstName),              
                new Claim("lastName", employee.lastName),
                new Claim("isadmin", employee.isadmin ? "1" : "0")
            }),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
