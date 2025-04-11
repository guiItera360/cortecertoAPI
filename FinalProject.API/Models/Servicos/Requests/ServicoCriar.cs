using FinalProject360.Dominio.Entidades;

namespace FinalProject.API.Models.Servicos.Requests
{
    public class ServicoCriar
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
    }
}