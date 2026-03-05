using MottuFlow.Hateoas;

namespace MottuFlow.Hateoas
{
    public class ArucoTagResource : ResourceBase
    {
        public new int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int IdMoto { get; set; }
    }
}