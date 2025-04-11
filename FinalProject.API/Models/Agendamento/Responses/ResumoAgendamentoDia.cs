namespace FinalProject.API.Models.Agendamento.Responses;

public class AgendamentoDiaResponse
{
    public int AgendamentoId { get; set; }
    public string NomeCliente { get; set; }
    public string NomeServico { get; set; }
    public string Horario { get; set; }
    public int Status { get; set; }
}
