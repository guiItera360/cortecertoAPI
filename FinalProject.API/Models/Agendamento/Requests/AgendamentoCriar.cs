namespace FinalProject.API.Models.Agendamento.Requests;

public class AgendamentoCriar{
    public int ServicoId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime Data { get; set; }
}