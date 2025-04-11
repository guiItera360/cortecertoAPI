namespace FinalProject360.Repositorio.StoredProcedures.DTOs;

public class AgendamentosDiaDto
{
    public int AgendamentoId { get; set; }
    public string NomeCliente { get; set; }
    public string NomeServico { get; set; }
    public string Horario { get; set; } // formato HH:mm
    public int Status { get; set; }
}
