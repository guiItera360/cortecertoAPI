using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Dominio.Entidades;
using FinalProject360.Repositorio.Interfaces;

namespace FinalProject.Aplicacao
{
    public class ServicoAplicacao : IServicoAplicacao
    {
        private readonly IServicoRepositorio _servicoRepositorio;

        public ServicoAplicacao(IServicoRepositorio servicoRepositorio)
        {
            _servicoRepositorio = servicoRepositorio;
        }

        public async Task<int> Salvar(Servico servico)
        {
            if (servico == null)
                throw new ArgumentException("O serviço não pode ser nulo.");

            ValidarServico(servico);

            return await _servicoRepositorio.Salvar(servico);
        }

        public async Task Atualizar(Servico servico)
        {
            var servicoExistente = await _servicoRepositorio.ObterPorId(servico.ServicoId);
            if (servicoExistente == null)
                throw new Exception("Serviço não encontrado.");

            ValidarServico(servico);

            servicoExistente.Nome = servico.Nome;
            servicoExistente.Preco = servico.Preco;
            servicoExistente.Descricao = servico.Descricao;

            await _servicoRepositorio.Atualizar(servicoExistente);
        }

        public async Task Deletar(int servicoId)
        {
            var servico = await _servicoRepositorio.ObterPorId(servicoId);
            if (servico == null)
                throw new Exception("Serviço não encontrado.");

            servico.Deletar();
            await _servicoRepositorio.Atualizar(servico);
        }

        public async Task Restaurar(int servicoId)
        {
            var servico = await _servicoRepositorio.ObterPorId(servicoId);
            if (servico == null)
                throw new Exception("Serviço não encontrado.");

            servico.Restaurar();
            await _servicoRepositorio.Atualizar(servico);
        }

        public async Task<IEnumerable<Servico>> ListarTodos(bool ativo)
        {
            return await _servicoRepositorio.ListarServicos(ativo);
        }

        public async Task<Servico> ObterPorId(int servicoId)
        {
            var servico = await _servicoRepositorio.ObterPorId(servicoId);
            if (servico == null)
                throw new Exception("Serviço não encontrado.");

            return servico;
        }

        #region Utilitários
        private static void ValidarServico(Servico servico)
        {
            if (string.IsNullOrWhiteSpace(servico.Nome))
                throw new ArgumentException("O nome do serviço é obrigatório.");

            if (servico.Preco <= 0)
                throw new ArgumentException("O preço do serviço deve ser maior que zero.");
        }
        #endregion
    }
}
