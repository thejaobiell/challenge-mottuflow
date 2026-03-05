namespace MottuFlowApi.DTOs
{
    /// <summary>
    /// DTO usado para autenticação no endpoint /api/v1/auth/login.
    /// </summary>
    public class AuthLoginInputDTO
    {
        /// <example>admin</example>
        public string Username { get; set; } = string.Empty;

        /// <example>123</example>
        public string Password { get; set; } = string.Empty;
    }
}
