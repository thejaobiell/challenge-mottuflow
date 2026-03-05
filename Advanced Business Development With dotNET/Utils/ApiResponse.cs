namespace MottuFlowApi.Utils
{
    /// <summary>
    /// Representa uma resposta padronizada da API com suporte a mensagens, dados e status.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica se a operação foi bem-sucedida.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensagem descritiva sobre o resultado da operação.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Dados retornados pela API (quando aplicável).
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Código HTTP associado à resposta (opcional).
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Timestamp UTC da resposta — útil para logs e auditoria.
        /// </summary>
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        // Retorno padrão de sucesso
        public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso.", int? statusCode = 200) =>
            new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };

        // Retorno padrão de falha
        public static ApiResponse<T> Fail(string message, T? data = default, int? statusCode = 400) =>
            new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
    }
}
