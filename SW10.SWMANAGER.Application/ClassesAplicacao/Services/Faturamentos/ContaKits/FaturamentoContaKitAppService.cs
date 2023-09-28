using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Helpers;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits
{
    public class FaturamentoContaKitAppService : SWMANAGERAppServiceBase, IFaturamentoContaKitAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoContaKit, long> _contaRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<FaturamentoKit, long> _faturamentoKitRepository;
        private readonly IRepository<FaturamentoConta, long> _faturamentoContaRepository;
        private readonly IFaturamentoContaItemAppService _faturamentoContaItemAppService;

        public FaturamentoContaKitAppService(
            IRepository<FaturamentoContaKit, long> contaRepository
           , IUnitOfWorkManager unitOfWorkManager
            , IRepository<FaturamentoKit, long> faturamentoKitRepository
            , IRepository<FaturamentoConta, long> faturamentoContaRepository
            , IFaturamentoContaItemAppService faturamentoContaItemAppService
            )
        {
            _contaRepository = contaRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _faturamentoKitRepository = faturamentoKitRepository;
            _faturamentoContaRepository = faturamentoContaRepository;
            _faturamentoContaItemAppService = faturamentoContaItemAppService;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoContaKitDto>> Listar(ListarFaturamentoContaKitsInput input)
        {
            var contarContaKits = 0;
            List<FaturamentoContaKit> contas;
            List<FaturamentoContaKitDto> contasDtos = new List<FaturamentoContaKitDto>();
            try
            {
                var query = _contaRepository
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro)
                    ;

                contarContaKits = await query
                    .CountAsync();

                contas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                contasDtos = contas
                    .MapTo<List<FaturamentoContaKitDto>>();

                return new PagedResultDto<FaturamentoContaKitDto>(contarContaKits, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoContaKitContaMedicaDto>> ListarParaContaMedica(FaturamentoContaKitFilterDto input)
        {
            var query = _contaRepository
                    .GetAll()
                    .Include(x => x.FaturamentoKit)
                    .Where(x => x.FaturamentoContaId == input.ContaMedicaId);

            var contarContaKits = await query.CountAsync();

            var contas = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();
            var users = await UserManager.Users.AsNoTracking().ToListAsync();
            var contasDtos = contas.Select(x => FaturamentoContaKitContaMedicaDto.Mapear(x)).ToList();
            contasDtos.ForEach(dto =>
            {
                dto.Usuario = users.FirstOrDefault(x => x.Id == dto.UsuarioId)?.FullName;
            });
            return new PagedResultDto<FaturamentoContaKitContaMedicaDto>(contarContaKits, contasDtos);
        }


        public async Task<IResultDropdownList<long>> ListarDropdownKitContaMedica(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<DropdownInput>()
                .EnableDistinct()
                .AddIdField("FatPacote.FaturamentoItemId")
                .AddTextField("CONCAT(FatItem.Codigo, ' - ', FatItem.Descricao)")
                .AddFromClause("FatPacote INNER JOIN FatItem ON FatPacote.FaturamentoItemId = FatItem.Id AND FatItem.IsDeleted = 0")
                .AddWhereClause(@"FaturamentoContaId = @filtro 
                    AND (
                        @search IS NULL
	                    OR FatItem.Descricao like '%' + @search + '%'
	                    OR  FatItem.Codigo like '%' + @search + '%'
	                    OR  FatItem.CodAmb like '%' + @search + '%'
	                    OR  FatItem.CodCbhpm like '%' + @search + '%'
	                    OR  FatItem.CodTuss like '%' + @search + '%'
	                    OR  FatItem.DescricaoTuss like '%' + @search + '%'
                    )")
                .AddOrderByClause("CONCAT(FatItem.Codigo, ' - ', FatItem.Descricao) ASC")
                .ExecuteAsync(dropdownInput);
        }

        public async Task<PagedResultDto<FaturamentoContaKitViewModel>> ListarVM(ListarFaturamentoContaKitsInput input)
        {
            var contarContaKits = 0;
            List<FaturamentoContaKit> contas;
            List<FaturamentoContaKitViewModel> contasDtos = new List<FaturamentoContaKitViewModel>();
            try
            {
                var query = _contaRepository
                    .GetAll()
                    .Include(i => i.FaturamentoKit)
                    .Include(i => i.Turno)
                    .Include(i => i.Medico)
                    .Include(i => i.Medico.SisPessoa)
                    .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro)
                    ;

                contarContaKits = await query
                    .CountAsync();

                contas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                foreach (var ck in contas)
                {
                    var contaKit = new FaturamentoContaKitViewModel();
                    contaKit.Id = ck.Id;
                    contaKit.Codigo = ck.FaturamentoKit?.Codigo;
                    contaKit.Descricao = ck.FaturamentoKit?.Descricao;
                    contaKit.FaturamentoContaId = ck.FaturamentoContaId;
                    contaKit.FaturamentoKitId = ck.FaturamentoKitId;
                    contaKit.Data = ck.Data;
                    contaKit.Qtde = ck.Qtde;
                    contaKit.CentroCustoId = ck.CentroCustoId;
                    contaKit.CentroCustoDescricao = ck.CentroCusto != null ? ck.CentroCusto.Descricao : string.Empty;
                    contaKit.TurnoId = ck.TurnoId;
                    contaKit.TurnoDescricao = ck.Turno?.Descricao;
                    //contaKit.TipoLeitoId           = ck.TipoLeitoId;
                    //contaKit.TipoLeitoDescricao    = ck.TipoLeito != null ? ck.TipoLeito.Descricao : string.Empty;

                    contaKit.TipoLeitoId = ck.TipoAcomodacaoId;
                    contaKit.TipoLeitoDescricao = ck.TipoAcomodacao != null ? ck.TipoAcomodacao.Descricao : string.Empty;

                    contaKit.HoraIncio = ck.HoraIncio;
                    contaKit.HoraFim = ck.HoraFim;
                    contaKit.MedicoId = ck.MedicoId;
                    contaKit.MedicoNome = ck.Medico != null ? ck.Medico.NomeCompleto : string.Empty;

                    contasDtos.Add(contaKit);
                }

                return new PagedResultDto<FaturamentoContaKitViewModel>(contarContaKits, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> CriarOuEditar(FaturamentoContaKitDto input)
        {
            try
            {
                using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
                using (var contaMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var fatKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoKit, long>>())
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var fatKit = await fatKitRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == input.FaturamentoKitId);
                    var contaMedica = await contaMedicaRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == input.FaturamentoContaId);
                    var contaKit = new FaturamentoContaKit
                    {
                        Id = input.Id,
                        FaturamentoKitId = input.FaturamentoKitId,
                        Codigo = input.Codigo,
                        Descricao = input.Descricao,
                        FaturamentoContaId = input.FaturamentoContaId,
                        Data = input.Data,
                        Qtde = input.Qtde,
                        CentroCustoId = input.CentroCustoId,
                        TurnoId = input.TurnoId,
                        // contaKit.TipoLeitoId           = input.TipoLeitoId;
                        TipoAcomodacaoId = input.TipoLeitoId,
                        HoraIncio = input.HoraIncio,
                        HoraFim = input.HoraFim,
                        MedicoId = input.MedicoId
                    };


                    if (input.Id.Equals(0))
                    {
                        contaKit.Id = await _contaRepository.InsertAndGetIdAsync(contaKit);

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.KitIncluidoContaMedica(fatKit?.Descricao, contaMedica?.Codigo, (await this.GetCurrentUserAsync()).FullName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaKit,
                            typeof(FaturamentoContaKit).FullName, contaKit.Id, typeof(FaturamentoConta).FullName,
                            contaKit.FaturamentoContaId));
                    }
                    else
                    {
                        await _contaRepository.UpdateAsync(contaKit);
                    }


                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                    return contaKit.Id;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoContaKitDto input)
        {
            try
            {
                await _contaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<DefaultReturn<DefaultReturnBool>> RemoverKit(long contaMedicaId, long contaKitId)
        {
            using (var contaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var contaKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaKit, long>>())
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            {
                var result = new DefaultReturn<DefaultReturnBool>()
                {
                    ReturnObject = new DefaultReturnBool()
                };
                var contaMedica = await contaAppService.Object.Obter(contaMedicaId);
                var contaKit = await contaKitRepository.Object.GetAll()
                    .Include(x => x.FaturamentoKit)
                    .FirstOrDefaultAsync(x => x.Id == contaKitId && x.FaturamentoContaId == contaMedicaId);
                if (contaKit == null)
                {
                    result.Errors.Add(ErroDto.Criar("", "Não é possivel localizar o kit para excluir"));
                    result.ReturnObject.Sucesso = false;
                    return result;
                }




                var items = (await contaAppService.Object.ListarItems(new FaturamentoContaItemTableFilterDto()
                {
                    ContaMedicaId = contaMedicaId,
                    EnablePaginate = false
                })).Items;
                items = items.Where(x => x.FaturamentoContakitId == contaKitId).ToList();

                await contaKitRepository.Object.DeleteAsync(contaKit);

                var userName = (await this.GetCurrentUserAsync()).FullName;
                await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.KitRemovidoContaMedica(contaKit.FaturamentoKit?.Descricao, contaMedica.Codigo, userName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem, typeof(FaturamentoConta).FullName, contaMedicaId));



                if (items.Any())
                {
                    await contaItemAppService.Object.ExcluirItens(contaMedicaId, items.Select(x => x.Id).ToList());
                }

                result.ReturnObject.Sucesso = true;

                return result;
            }
        }

        public async Task ExcluirVM(long id)
        {
            try
            {
                await _contaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoContaKitDto> Obter(long id)
        {
            try
            {
                var query = await _contaRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var conta = query.MapTo<FaturamentoContaKitDto>();

                return conta;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoContaKitViewModel> ObterViewModel(long id)
        {
            try
            {
                var query = await _contaRepository
                    .GetAll()
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.CentroCusto)
                    .Include(m => m.Turno)
                    //.Include(m => m.TipoLeito)
                    .Include(m => m.TipoAcomodacao)
                    .Include(m => m.FaturamentoKit)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var contaKit = new FaturamentoContaKitViewModel();
                contaKit.Id = query.Id;
                contaKit.Codigo = query.Codigo;
                contaKit.Descricao = query.Descricao;
                contaKit.FaturamentoContaId = query.FaturamentoContaId;
                contaKit.Data = query.Data;
                contaKit.Qtde = query.Qtde;
                contaKit.CentroCustoId = query.CentroCustoId;
                contaKit.CentroCustoDescricao = query.CentroCusto != null ? query.CentroCusto.Descricao : string.Empty;
                contaKit.TurnoId = query.TurnoId;
                contaKit.TurnoDescricao = query.Turno != null ? query.Turno.Descricao : string.Empty;
                //contaKit.TipoLeitoId             = query.TipoLeitoId;
                //contaKit.TipoLeitoDescricao      = query.TipoLeito != null ? query.TipoLeito.Descricao : string.Empty;
                contaKit.TipoLeitoId = query.TipoAcomodacaoId;
                contaKit.TipoLeitoDescricao = query.TipoAcomodacao != null ? query.TipoAcomodacao.Descricao : string.Empty;

                contaKit.HoraIncio = query.HoraIncio;
                contaKit.HoraFim = query.HoraFim;
                contaKit.MedicoId = query.MedicoId;
                contaKit.MedicoNome = query.Medico != null ? query.Medico.NomeCompleto : string.Empty;
                contaKit.FaturamentoKitId = query.FaturamentoKitId;
                contaKit.FaturamentoKitDescricao = query.FaturamentoKit != null ? query.FaturamentoKit.Descricao : string.Empty;

                return contaKit;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

    }


    public class FaturamentoContaKitViewModel
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public long? FaturamentoContaId { get; set; }
        public DateTime? Data { get; set; }
        public float Qtde { get; set; }
        public long? CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }
        public long? TurnoId { get; set; }
        public string TurnoDescricao { get; set; }
        public long? TipoLeitoId { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public DateTime? HoraIncio { get; set; }
        public DateTime? HoraFim { get; set; }
        public long? MedicoId { get; set; }
        public string MedicoNome { get; set; }
        public long? FaturamentoKitId { get; set; }
        public string FaturamentoKitDescricao { get; set; }
    }
}
