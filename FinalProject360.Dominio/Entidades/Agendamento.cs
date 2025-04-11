using FinalProject360.Dominio.Enumeradores;
namespace FinalProject360.Dominio.Entidades;

public class Agendamento{
    public int AgendamentoId { get; set; }
    public int UsuarioId { get; set; }
    public int ServicoId { get; set; }
    public DateTime DataHora { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
    
    // Relacionamentos EfCore
    public Usuario Usuario { get; set; }
    public Servico Servico { get; set; }
    public List<Usuario> Usuarios { get; set; }
    public List<Servico> Servicos { get; set; }

    public void Deletar ()
    {
        Status = StatusAgendamento.Cancelado;
    }

    public void Confirmar ()
    {
        Status = StatusAgendamento.Confirmado;
    }

}