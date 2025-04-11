using FinalProject360.Dominio.Entidades;
using FinalProject360.Dominio.Enumeradores;

namespace FinalProject.API.Models.Agendamento.Responses;

public class AgendamentoResposta{
    public int AgendamentoId { get; set; }
    public Servico Servico { get; set; }
    public Usuario Usuario { get; set; }
    public DateTime Data { get; set; }
    public StatusAgendamento Status { get; set; }
}