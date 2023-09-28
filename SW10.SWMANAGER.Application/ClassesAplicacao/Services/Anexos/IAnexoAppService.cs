using SW10.SWMANAGER.ClassesAplicacao.Anexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Anexos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Anexos
{
    public interface IAnexoAppService
    {
        Task<Anexo> InserirAnexo(AnexoDto anexoDto);
        Task CriarRelacionamento(AnexoListaDto anexoListaDto);
        Task ExcluirAnexo(string objectKey);
        Task<List<AnexoDto>> ListarAnexos(Guid anexoListaId);
        Task<List<AnexoDto>> ListarAnexosPelaOrigem(long origemAnexoId, string origemAnexoTabela);
    }
}
