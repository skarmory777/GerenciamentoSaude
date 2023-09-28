using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(Leito))]
    public class LeitoDto : CamposPadraoCRUDDto
    {
        public string DescricaoResumida { get; set; }

        public string LeitoAih { get; set; }

        public string Ramal { get; set; }

        public int? Sexo { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public long? TipoAcomodacaoId { get; set; }

        public long? TabelaItemTissId { get; set; }

        public long? TabelaItemSusId { get; set; }

        public long? LeitoStatusId { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public bool Extra { get; set; }

        public bool HospitalDia { get; set; }

        public bool Ativo { get; set; }

        public virtual UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public virtual TipoAcomodacaoDto TipoAcomodacao { get; set; }

        public virtual TabelaDominioDto TabelaDominio { get; set; }

        public virtual LeitoStatusDto LeitoStatus { get; set; }

        public string Paciente { get; set; }

        public long? AtendimentoId { get; set; }

        public static LeitoDto MapearFromCore(Leito leito)
        {

            var leitoDto = new LeitoDto();

            leitoDto.Id = leito.Id;
            leitoDto.LeitoAih = leito.LeitoAih;
            leitoDto.Ramal = leito.Ramal;
            leitoDto.Sexo = leito.Sexo;
            leitoDto.UnidadeOrganizacionalId = leito.UnidadeOrganizacionalId;
            leitoDto.TipoAcomodacaoId = leito.TipoAcomodacaoId;
            leitoDto.TabelaItemTissId = leito.TabelaItemTissId;
            leitoDto.TabelaItemSusId = leito.TabelaItemSusId;
            leitoDto.LeitoStatusId = leito.LeitoStatusId;
            leitoDto.DataAtualizacao = leito.DataAtualizacao;
            leitoDto.Extra = leito.Extra;
            leitoDto.HospitalDia = leito.HospitalDia;
            leitoDto.Ativo = leito.Ativo;
            leitoDto.Codigo = leito.Codigo;
            leitoDto.Descricao = leito.Descricao;

            if (leito.UnidadeOrganizacional != null)
            {
                leitoDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.MapearFromCore(leito.UnidadeOrganizacional);
            }

            leitoDto.TipoAcomodacao = leito.TipoAcomodacao?.MapTo<TipoAcomodacaoDto>();
            leitoDto.TabelaDominio = leito.TabelaDominio?.MapTo<TabelaDominioDto>();
            leitoDto.LeitoStatus = leito.LeitoStatus?.MapTo<LeitoStatusDto>();

            return leitoDto;
        }

        #region Mapeamento

        public static Leito Mapear(LeitoDto leitoDto)
        {
            Leito leito = new Leito();

            leito.Ativo = leitoDto.Ativo;
            leito.Codigo = leitoDto.Codigo;
            leito.CreationTime = leitoDto.CreationTime;
            leito.CreatorUserId = leitoDto.CreatorUserId;
            leito.DataAtualizacao = leitoDto.DataAtualizacao;
            leito.DeleterUserId = leitoDto.DeleterUserId;
            leito.DeletionTime = leitoDto.DeletionTime;
            leito.Descricao = leitoDto.Descricao;
            leito.DescricaoResumida = leitoDto.DescricaoResumida;
            leito.Extra = leitoDto.Extra;
            leito.HospitalDia = leitoDto.HospitalDia;
            leito.Id = leitoDto.Id;
            leito.IsDeleted = leitoDto.IsDeleted;
            leito.IsSistema = leitoDto.IsSistema;
            leito.LastModificationTime = leitoDto.LastModificationTime;
            leito.LastModifierUserId = leitoDto.LastModifierUserId;
            leito.LeitoAih = leitoDto.LeitoAih;
            leito.LeitoStatusId = leitoDto.LeitoStatusId;
            leito.Ramal = leitoDto.Ramal;
            leito.Sexo = leitoDto.Sexo;
            leito.TabelaItemSusId = leitoDto.TabelaItemSusId;
            leito.TabelaItemTissId = leitoDto.TabelaItemTissId;
            leito.TipoAcomodacaoId = leitoDto.TipoAcomodacaoId;
            leito.UnidadeOrganizacionalId = leitoDto.UnidadeOrganizacionalId;

            return leito;

        }

        public static LeitoDto Mapear(Leito leito)
        {
            if (leito == null)
            {
                return null;
            }

            LeitoDto leitoDto = MapearBase<LeitoDto>(leito);

            leitoDto.Ativo = leito.Ativo;
            leitoDto.Codigo = leito.Codigo;
            leitoDto.CreationTime = leito.CreationTime;
            leitoDto.CreatorUserId = leito.CreatorUserId;
            leitoDto.DataAtualizacao = leito.DataAtualizacao;
            leitoDto.DeleterUserId = leito.DeleterUserId;
            leitoDto.DeletionTime = leito.DeletionTime;
            leitoDto.Descricao = leito.Descricao;
            leitoDto.DescricaoResumida = leito.DescricaoResumida;
            leitoDto.Extra = leito.Extra;
            leitoDto.HospitalDia = leito.HospitalDia;
            leitoDto.Id = leito.Id;
            leitoDto.IsDeleted = leito.IsDeleted;
            leitoDto.IsSistema = leito.IsSistema;
            leitoDto.LastModificationTime = leito.LastModificationTime;
            leitoDto.LastModifierUserId = leito.LastModifierUserId;
            leitoDto.LeitoAih = leito.LeitoAih;
            leitoDto.LeitoStatusId = leito.LeitoStatusId;
            leitoDto.Ramal = leito.Ramal;
            leitoDto.Sexo = leito.Sexo;
            leitoDto.TabelaItemSusId = leito.TabelaItemSusId;
            leitoDto.TabelaItemTissId = leito.TabelaItemTissId;
            leitoDto.TipoAcomodacaoId = leito.TipoAcomodacaoId;
            leitoDto.UnidadeOrganizacionalId = leito.UnidadeOrganizacionalId;

            if (leito.TipoAcomodacao != null)
            {
                leitoDto.TipoAcomodacao = TipoAcomodacaoDto.Mapear(leito.TipoAcomodacao);
            }

            if (leito.LeitoStatus != null)
            {
                leitoDto.LeitoStatus = LeitoStatusDto.Mapear(leito.LeitoStatus);
            }
            return leitoDto;

        }

        public static List<LeitoDto> Mapear(List<Leito> entityList)
        {
            var dtoList = new List<LeitoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }

        #endregion

    }


}
