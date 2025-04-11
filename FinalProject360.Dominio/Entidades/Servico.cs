using System.Text.Json.Serialization;

namespace FinalProject360.Dominio.Entidades;
public class Servico
{
    public int ServicoId { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool StatusAtivo { get; set; } = true;
    public string? Descricao { get; set; } // Campo opcional
    [JsonIgnore]
    public List<Agendamento> ServicosAgendados { get; set; } = new List<Agendamento>();

    public void AlterarPreco(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new ArgumentException("O preço do serviço deve ser maior que zero.");
            
        Preco = novoPreco;
    }

    public void AlterarDescricao(string? novaDescricao)
    {
        Descricao = novaDescricao; // Permite alterar ou remover a descrição
    }

    public void Deletar()
    {
        StatusAtivo = false;
    }

    public void Restaurar()
    {
        StatusAtivo = true;
    }
}
