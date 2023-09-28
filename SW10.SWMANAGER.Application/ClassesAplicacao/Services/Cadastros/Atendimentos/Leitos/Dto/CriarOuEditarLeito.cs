using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(Leito))]
    public class CriarOuEditarLeito : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        public string DescricaoResumida { get; set; }

        public string LeitoAih { get; set; }

        public string Ramal { get; set; }

        public int? Sexo { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }

        public long? TipoAcomodacaoId { get; set; }

        public long? TabelaItemTissId { get; set; }

        public long? TabelaItemSusId { get; set; }

        public long? LeitoStatusId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataAtualizacao { get; set; }

        public bool Extra { get; set; }

        public bool HospitalDia { get; set; }

        public bool Ativo { get; set; }

        [ForeignKey("UnidadeOrganizacionalId")]
        public virtual UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("TipoAcomodacaoId")]
        public virtual TipoAcomodacaoDto TipoAcomodacao { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominioDto TabelaDominio { get; set; }

        [ForeignKey("LeitoStatusId")]
        public virtual LeitoStatusDto LeitoStatus { get; set; }

        public static Leito Mapear(CriarOuEditarLeito dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<Leito>(dto);
            entity.DescricaoResumida = dto.DescricaoResumida;
            entity.LeitoAih = dto.LeitoAih;
            entity.Ramal = dto.Ramal;
            entity.Sexo = dto.Sexo;
            entity.UnidadeOrganizacionalId = dto.UnidadeOrganizacionalId;
            entity.TipoAcomodacaoId = dto.TipoAcomodacaoId;
            entity.TabelaItemTissId = dto.TabelaItemTissId;
            entity.TabelaItemSusId = dto.TabelaItemSusId;
            entity.LeitoStatusId = dto.LeitoStatusId;
            entity.DataAtualizacao = dto.DataAtualizacao;
            entity.Extra = dto.Extra;
            entity.HospitalDia = dto.HospitalDia;
            entity.Ativo = dto.Ativo;
            entity.UnidadeOrganizacional = dto.UnidadeOrganizacional;
            entity.TipoAcomodacao = TipoAcomodacaoDto.Mapear(dto.TipoAcomodacao);
            entity.TabelaDominio = TabelaDominioDto.Mapear(dto.TabelaDominio);
            entity.LeitoStatus = LeitoStatusDto.Mapear(dto.LeitoStatus);

            return entity;
        }
    }
}