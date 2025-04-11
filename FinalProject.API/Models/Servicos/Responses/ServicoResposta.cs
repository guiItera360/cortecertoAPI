using FinalProject360.Dominio.Entidades;

namespace FinalProject.API.Models.Servicos.Responses
{
    public class ServicoResposta
    {
        public int ServicoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Observacao { get; set; }
        public bool Status_Ativo { get; set; }
    }
}