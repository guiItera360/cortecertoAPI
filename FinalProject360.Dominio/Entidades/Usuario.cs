using System.Text.Json.Serialization;
using FinalProject360.Dominio.Enumeradores;

namespace FinalProject360.Dominio.Entidades;

public class Usuario
{
    #region Atributos | Propriedades

    // O ID do Usuario, gerado automaticamente pelo banco de dados.
    public int UsuarioId { get; set; }

    // O nome do usuario. Deve ter entre 3 e 100 caracteres.
    public string Nome { get; set; }

    // O número de telefone do usuario. Deve ser um número com entre 10 e 15 caracteres
    public string Email { get; set; }

    // A senha do usuario, não há regras estabelecidas
    public string Senha { get; set; }

    // Indica se o usuario está ativo ou não. Se estiver inativo, o usuario não aparecerá nas operações normais.
    public bool StatusAtivo { get; set; } = true;

    // A data em que o usuario foi cadastrado. Este campo é preenchido automaticamente no momento da criação.
    public DateTime DataCadastro { get; set; }

    // A lista de agendamentos realizados pelo usuario.
    [JsonIgnore]
    public List<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    // A categoria do usuario.
    public CategoriaUsuario Categoria { get; set; }

    #endregion

    #region Métodos

    /// <summary>
    /// Marca o usuario como inativo. 
    /// Este método simula um "soft delete", onde o usuario é desativado, mas os dados permanecem no banco.
    /// </summary>
    public void Deletar()
    {
        StatusAtivo = false;
    }

    /// <summary>
    /// Restaura o usuario para o estado ativo.
    /// Este método permite que um usuario, antes deletado (inativo), seja reativado.
    /// </summary>
    public void Restaurar()
    {
        StatusAtivo = true;
    }

    #endregion
}