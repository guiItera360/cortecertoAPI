namespace FinalProject360.Repositorio.StoredProcedures.DTOs;
public class ResumoFaturamentoDto
{
    public decimal Faturamento_Previsto { get; set; }
    public decimal Faturamento_Realizado { get; set; }
    public decimal PerdaOuPotencial => Faturamento_Previsto - Faturamento_Realizado;
}
