using FinalProject360.Dominio.Enumeradores;

namespace FinalProject.API.Models.Agendamento.Requests;

public class AgendamentoAtualizar{
    public int AgendamentoId { get; set; }
    public int ServicoId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime Data { get; set; }
    public StatusAgendamento Status { get; set; }
}