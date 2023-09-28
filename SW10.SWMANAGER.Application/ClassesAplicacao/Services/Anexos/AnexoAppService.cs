using Abp.AutoMapper;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Anexos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Anexos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Anexos
{
    public class AnexoAppService : SWMANAGERAppServiceBase, IAnexoAppService
    {
        private readonly IRepository<Anexo, long> _anexoRepository;
        private readonly IRepository<Lancamento, long> _lancamentoRepository;
        private readonly IRepository<Documento, long> _documentoRepository;
        private readonly IRepository<Paciente, long> _pacienteRepository;

        public AnexoAppService(
            IRepository<Anexo, long> anexoRepository,
            IRepository<Lancamento, long> lancamentoRepository,
            IRepository<Documento, long> documentoRepository,
            IRepository<Paciente, long> pacienteRepository)
        {
            _anexoRepository = anexoRepository;
            _lancamentoRepository = lancamentoRepository;
            _documentoRepository = documentoRepository;
            _pacienteRepository = pacienteRepository;
        }

        public async Task<Anexo> InserirAnexo(AnexoDto anexoDto)
        {
            var novoAnexo = new Anexo()
            {
                AnexoListaId = anexoDto.AnexoListaId,
                BucketName = anexoDto.BucketName,
                FileName = anexoDto.FileName,
                Key = anexoDto.Key
            };

            var objectSaved = await _anexoRepository.InsertAsync(novoAnexo);

            return objectSaved;
        }

        public async Task CriarRelacionamento(AnexoListaDto anexoListaDto)
        {
            if (anexoListaDto.OrigemAnexoTabela.Equals("finlancamento"))
            {
                var lancamento = await _lancamentoRepository.FirstOrDefaultAsync(Convert.ToInt64(anexoListaDto.OrigemAnexoId));
                lancamento.AnexoListaId = anexoListaDto.AnexoListaId;
                await _lancamentoRepository.UpdateAsync(lancamento);
            }

            if (anexoListaDto.OrigemAnexoTabela.Equals("findocumento"))
            {
                var documento = await _documentoRepository.FirstOrDefaultAsync(Convert.ToInt64(anexoListaDto.OrigemAnexoId));
                documento.AnexoListaId = anexoListaDto.AnexoListaId;
                await _documentoRepository.UpdateAsync(documento);
            }

            if (anexoListaDto.OrigemAnexoTabela.Equals("sispaciente"))
            {
                var paciente = await _pacienteRepository.FirstOrDefaultAsync(Convert.ToInt64(anexoListaDto.OrigemAnexoId));
                paciente.AnexoListaId = anexoListaDto.AnexoListaId;
                await _pacienteRepository.UpdateAsync(paciente);
            }
        }

        private async Task ExcluirRelacionamento(Guid anexoListaId, string objectKey)
        {
            if (objectKey.Contains("finlancamento"))
            {
                var lancamento = await _lancamentoRepository.FirstOrDefaultAsync(x => x.AnexoListaId.Value.Equals(anexoListaId));
                lancamento.AnexoListaId = null;
                await _lancamentoRepository.UpdateAsync(lancamento);
            }

            if (objectKey.Contains("findocumento"))
            {
                var documento = await _documentoRepository.FirstOrDefaultAsync(x => x.AnexoListaId.Value.Equals(anexoListaId));
                documento.AnexoListaId = null;
                await _documentoRepository.UpdateAsync(documento);
            }

            if (objectKey.Contains("sispaciente"))
            {
                var paciente = await _pacienteRepository.FirstOrDefaultAsync(x => x.AnexoListaId.Value.Equals(anexoListaId));
                paciente.AnexoListaId = null;
                await _pacienteRepository.UpdateAsync(paciente);
            }
        }

        public async Task ExcluirAnexo(string objectKey)
        {
            
            var anexo = await _anexoRepository.FirstOrDefaultAsync(x => x.Key.Equals(objectKey));
            var anexosPorListaId = await _anexoRepository.CountAsync(x => x.AnexoListaId.Equals(anexo.AnexoListaId));

            if (anexosPorListaId.Equals(1))
                await ExcluirRelacionamento(anexo.AnexoListaId, objectKey);

            await _anexoRepository.DeleteAsync(anexo);
        }

        public async Task<List<AnexoDto>> ListarAnexos(Guid anexoListaId)
        {
            var anexosDto = new List<AnexoDto>();
            var anexos = await _anexoRepository.GetAllListAsync(x => x.AnexoListaId.Equals(anexoListaId));

            if (anexos != null && anexos.Count > 0)
                anexosDto = anexos.MapTo<List<AnexoDto>>();

            return anexosDto;
        }

        public async Task<List<AnexoDto>> ListarAnexosPelaOrigem(long origemAnexoId, string origemAnexoTabela)
        {
            var anexosDto = new List<AnexoDto>();

            switch (origemAnexoTabela)
            {
                case "finlancamento":
                    var lancamento = await _lancamentoRepository.SingleAsync(x => x.Id.Equals(origemAnexoId));

                    if (lancamento?.AnexoListaId != null)
                        anexosDto = await ListarAnexos(lancamento.AnexoListaId.Value);

                    break;

                case "findocumento":
                    var documento = await _documentoRepository.SingleAsync(x => x.Id.Equals(origemAnexoId));

                    if (documento?.AnexoListaId != null)
                        anexosDto = await ListarAnexos(documento.AnexoListaId.Value);

                    break;

                case "sispaciente":
                    var paciente = await _pacienteRepository.SingleAsync(x => x.Id.Equals(origemAnexoId));

                    if (paciente?.AnexoListaId != null)
                        anexosDto = await ListarAnexos(paciente.AnexoListaId.Value);

                    break;
            }

            return anexosDto;
        }
    }
}
