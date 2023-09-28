using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Visitantes;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto
{
    [AutoMap(typeof(Visitante))]
    public class VisitanteDto : CamposPadraoCRUDDto
    {

        public string Nome { get; set; }
        public string Documento { get; set; }
        public bool IsAcompanhante { get; set; }
        public bool IsVisitante { get; set; }
        public bool IsMedico { get; set; }
        public bool IsEmergencia { get; set; }
        public bool IsInternado { get; set; }
        public bool IsFornecedor { get; set; }
        public bool IsSetor { get; set; }
        public byte[] Foto { get; set; }
        public string FotoMimeType { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public long AteId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }
        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }
        public long? FornecedorId { get; set; }
        public SisFornecedorDto Fornecedor { get; set; }
        public bool IsFinalizar { get; set; }
        public string NomePacinete { get; set; }

        public static VisitanteDto MapearFormCore(Visitante visitante)
        {
            var visitanteDto = new VisitanteDto
            {
                Id = visitante.Id,
                Nome = visitante.Nome,
                Documento = visitante.Documento,
                IsAcompanhante = visitante.IsAcompanhante,
                IsVisitante = visitante.IsVisitante,
                IsMedico = visitante.IsMedico,
                IsEmergencia = visitante.IsEmergencia,
                IsInternado = visitante.IsInternado,
                IsFornecedor = visitante.IsFornecedor,
                IsSetor = visitante.IsSetor,
                Foto = visitante.Foto,
                FotoMimeType = visitante.FotoMimeType,
                DataEntrada = visitante.DataEntrada,
                DataSaida = visitante.DataSaida,
                UnidadeOrganizacionalId = visitante.UnidadeOrganizacionalId,
                UnidadeOrganizacional = visitante.UnidadeOrganizacional != null ? UnidadeOrganizacionalDto.Mapear(visitante.UnidadeOrganizacional) : null,//?.MapTo<UnidadeOrganizacionalDto>(),
                AtendimentoId = visitante.AtendimentoId,

                Atendimento = visitante.Atendimento != null ? AtendimentoDto.Mapear(visitante.Atendimento) : null,//?.MapTo<AtendimentoDto>(),
                LeitoId = visitante.LeitoId
            };

            if (visitante.Leito != null)
            {
                visitanteDto.Leito = LeitoDto.MapearFromCore(visitante.Leito);
            }

            visitanteDto.FornecedorId = visitante.FornecedorId;
            visitanteDto.Fornecedor = visitante.Fornecedor?.MapTo<SisFornecedorDto>();
            visitanteDto.IsFinalizar = false;

            return visitanteDto;
        }

        public static Visitante MapearParaCore(VisitanteDto visitanteDto)
        {
            var visitante = new Visitante();

            visitante.Id = visitanteDto.Id;
            visitante.Nome = visitanteDto.Nome;
            visitante.Documento = visitanteDto.Documento;
            visitante.IsAcompanhante = visitanteDto.IsAcompanhante;
            visitante.IsVisitante = visitanteDto.IsVisitante;
            visitante.IsMedico = visitanteDto.IsMedico;
            visitante.IsEmergencia = visitanteDto.IsEmergencia;
            visitante.IsInternado = visitanteDto.IsInternado;
            visitante.IsFornecedor = visitanteDto.IsFornecedor;
            visitante.IsSetor = visitanteDto.IsSetor;
            visitante.Foto = visitanteDto.Foto;
            visitante.FotoMimeType = visitanteDto.FotoMimeType;
            visitante.DataEntrada = visitanteDto.DataEntrada;
            visitante.DataSaida = visitanteDto.DataSaida;
            //visitante.AteId = 
            visitante.UnidadeOrganizacionalId = visitanteDto.UnidadeOrganizacionalId;
            //visitante.UnidadeOrganizacional = visitanteDto.UnidadeOrganizacional.MapTo<UnidadeOrganizacional>();
            visitante.AtendimentoId = visitanteDto.AtendimentoId;
            //visitante.Atendimento = visitanteDto.Atendimento.MapTo<Atendimento>();
            visitante.LeitoId = visitanteDto.LeitoId;
            //visitante.Leito = visitanteDto.Leito.MapTo<Leito>();
            visitante.FornecedorId = visitanteDto.FornecedorId;
            //visitante.Fornecedor = visitanteDto.Fornecedor.MapTo<SisFornecedorDto>();
            //visitante.IsFinalizar = false;

            return visitante;
        }
    }

}
