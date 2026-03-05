namespace MottuFlow.Hateoas
{
    public class FuncionarioResource : ResourceBase
    {
        public new int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Cargo { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }  
}