using FinalProject360.Dominio.Entidades;

namespace FinalProject.API.Models.Servicos.Requests
{
    public class ServicoAtualizar
    {
        public int ServicoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Observação { get; set; }
    }
}