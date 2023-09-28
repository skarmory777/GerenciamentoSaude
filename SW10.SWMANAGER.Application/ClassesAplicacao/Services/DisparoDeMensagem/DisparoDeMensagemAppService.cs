
namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem
{
    using Abp.Application.Services.Dto;
    using SW10.SWMANAGER.Helpers;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using SW10.SWMANAGER.Helper;

    public class DisparoDeMensagemAppService : SWMANAGERAppServiceBase, IDisparoDeMensagemAppService
    {
        /// <inheritdoc />
        public async Task<PagedResultDto<DisparoDeMensagemDto>> Listar(IndexFiltroDisparoDeMensagemViewModel input)
        {
            var defaultField = "Id";
            var selectPart = defaultField + @", 
                [DisparoDeMensagem].[DataProgramada],
                [DisparoDeMensagem].[DataInicioDisparo],
                [DisparoDeMensagem].[DataFinalDisparo],
                [DisparoDeMensagem].[Mensagem],
                [DisparoDeMensagem].[Total],
                [DisparoDeMensagem].[TotalEnviado],
                [DisparoDeMensagem].[TotalRecebido]";
            var fromPart = "SisDisparoDeMensagem AS DisparoDeMensagem";
            var wherePart = "AND DisparoDeMensagem.IsDeleted = @deleted AND DisparoDeMensagem.IsSistema = @isSistema";
            return await DataTableHelper
                .CreateDataTable<DisparoDeMensagemDto, IndexFiltroDisparoDeMensagemViewModel>(this)
                .AddDefaultField(defaultField)
                .AddSelectClause(selectPart)
                .AddFromClause(fromPart)
                .AddWhereMethod((filtro, dapperParameters) =>
                {
                    dapperParameters.Add("deleted", false);
                    dapperParameters.Add("isSistema", false);
                    return wherePart;
                }).ExecuteAsync(input);
        }

        /// <inheritdoc />
        public async Task<DisparoDeMensagemDto> CriarOuEditar(DisparoDeMensagemDto input)
        {
            try
            {
                using (var disparoDeMensagemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagem, long>>())
                using (var disparoDeMensagemItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagemItem, long>>())
                {
                    input.Total = input.DisparoDeMensagemItems?.Count ?? 0;

                    var model = DisparoDeMensagemDto.Mapear(input);
                    model.DisparoDeMensagemItems = null;

                    model.DataInicioDisparo = model.DataFinalDisparo.ToNullIfTooEarlyForDb();
                    model.DataFinalDisparo = model.DataFinalDisparo.ToNullIfTooEarlyForDb();
                    
                    input.Id = await disparoDeMensagemRepository.Object.InsertOrUpdateAndGetIdAsync(model);
                    //model.DisparoDeMensagemItems = DisparoDeMensagemItemDto.MapearLista(input.DisparoDeMensagemItems);

                    foreach (var item in input.DisparoDeMensagemItems)
                    {
                        item.DisparoDeMensagemId = input.Id;
                        item.Mensagem = model.Mensagem;
                        item.DataProgramada = model.DataProgramada;
                        item.DataInicioDisparo = item.DataInicioDisparo.ToNullIfTooEarlyForDb();
                        item.DataFinalDisparo = item.DataFinalDisparo.ToNullIfTooEarlyForDb();
                        item.DataRecebimento = item.DataRecebimento.ToNullIfTooEarlyForDb();
                        item.Titulo = model.Titulo;
                        item.DisparoDeMensagemItemTipo = null;
                        item.Id = disparoDeMensagemItemRepository.Object.InsertOrUpdateAndGetId(DisparoDeMensagemItemDto.Mapear(item));
                    }

                    return DisparoDeMensagemDto.MapearEntity(model);
                }
            }
            catch (Exception e)
            {

            }

            return null;

        }

        /// <inheritdoc />
        public async Task ExcluirItem(long disparoDeMensagemId)
        {
            using (var disparoDeMensagemItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagemItem, long>>())
            {
                var item = await disparoDeMensagemItemRepository.Object.FirstOrDefaultAsync(disparoDeMensagemId);

                if (item != null)
                {
                    await disparoDeMensagemItemRepository.Object.DeleteAsync(item.Id);
                }
            }
        }

        /// <inheritdoc />
        public Task<DisparoDeMensagemDto> AdicionarItem(DisparoDeMensagemItemDto input)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public DisparoDeMensagemDto Obter(long id)
        {
            using (var disparoDeMensagemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagem, long>>())
            {
                var disparoDeMensagem = disparoDeMensagemRepository.Object.GetAll()
                    .Include(x => x.DisparoDeMensagemItems)
                    .Include(x => x.DisparoDeMensagemItems.Select(z => z.Pessoa))
                    .Include(x => x.DisparoDeMensagemItems.Select(z => z.DisparoDeMensagemItemTipo))
                    .FirstOrDefault(z => z.Id == id);

                return DisparoDeMensagemDto.MapearEntity(disparoDeMensagem);
            }
        }

        public void Excluir(long id)
        {
            using (var disparoDeMensagemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<DisparoDeMensagem, long>>())
            {
                var disparoDeMensagem = disparoDeMensagemRepository.Object.GetAll().FirstOrDefault(z => z.Id == id);
                if (disparoDeMensagem != null)
                {
                    disparoDeMensagemRepository.Object.Delete(id);
                }
            }
        }

        /// <inheritdoc />
        public async Task<PagedResultDto<IndexDisparoDeMensagemViewModel>> ListarParaDisparo(FiltroDisparoDeMensagemViewModel filterViewModel)
        {
            var selectQuery = new StringBuilder(
                @" DISTINCT Pessoa.Id,
                CONCAT(SisPaciente.DddTelefone1, SisPaciente.Telefone1) Telefone1, 
                CONCAT(SisPaciente.DddTelefone2, SisPaciente.Telefone2) Telefone2,
                CONCAT(SisPaciente.DddTelefone3, SisPaciente.Telefone3) Telefone3, 
                CONCAT(SisPaciente.DddTelefone4, SisPaciente.Telefone4) Telefone4, 
                Pessoa.Email AS Email1,
                Pessoa.Email2,
                Pessoa.Email3,
                Pessoa.Email4,
                Pessoa.NomeCompleto,
                Pessoa.Nascimento,");
            var fromQuery = new StringBuilder(@" SisPessoa AS Pessoa INNER JOIN SisPaciente ON SisPaciente.SisPessoaId = Pessoa.Id");


            var whereQuery = new StringBuilder(@"
                AND Pessoa.IsDeleted = @deleted 
                AND Pessoa.FisicaJuridica = 'F' 
                AND Pessoa.NomeCompleto IS NOT NULL 
                AND (CONCAT(SisPaciente.DddTelefone1, SisPaciente.Telefone1) IS NOT NULL 
                    OR CONCAT(SisPaciente.DddTelefone2, SisPaciente.Telefone2) IS NOT NULL 
                    OR CONCAT(SisPaciente.DddTelefone3, SisPaciente.Telefone3) IS NOT NULL 
                    OR CONCAT(SisPaciente.DddTelefone4, SisPaciente.Telefone4) IS NOT NULL)
                AND (
                    SisPaciente.Email IS NOT NULL 
                    OR SisPaciente.Email2 IS NOT NULL 
                    OR SisPaciente.Email3 IS NOT NULL 
                    OR SisPaciente.Email4 IS NOT NULL)
            ");

            if (filterViewModel.PacienteId.HasValue)
            {
                whereQuery.Append(@" AND SisPaciente.Id = @PacienteId ");
            }

            if ((filterViewModel.PaisId.HasValue && filterViewModel.PaisId != 0)
                || (filterViewModel.EstadoId.HasValue && filterViewModel.EstadoId != 0)
                || (filterViewModel.CidadeId.HasValue && filterViewModel.CidadeId != 0)
                || !string.IsNullOrEmpty(filterViewModel.Bairro))
            {
                fromQuery.Append(@" INNER JOIN SisEndereco ON SisEndereco.PesssoaId = Pessoa.Id");
                whereQuery.Append(@" AND SisEndereco.IsDeleted = @deleted ");
            }

            if (filterViewModel.SexoId.HasValue)
            {
                whereQuery.Append(@" AND Pessoa.SexoId = @SexoId ");
            }

            if (filterViewModel.PaisId.HasValue)
            {
                selectQuery.Append(" SisPais.Nome AS Pais,");
                fromQuery.Append(@" INNER JOIN SisPais ON SisPais.Id = SisEndereco.PaisId ");
                whereQuery.Append(@" AND SisEndereco.PaisId = @PaisId ");
            }

            if (filterViewModel.EstadoId.HasValue)
            {
                selectQuery.Append(" SisEstado.Nome AS Estado,");
                fromQuery.Append(@" INNER JOIN SisEstado ON SisEstado.Id = SisEndereco.EstadoId ");
                whereQuery.Append(@" AND SisEndereco.EstadoId = @EstadoId ");
            }

            if (filterViewModel.CidadeId.HasValue)
            {
                selectQuery.Append(" SisCidade.Nome AS Cidade,");
                fromQuery.Append(@" INNER JOIN SisCidade ON SisEstado.Id = SisEndereco.CidadeId ");
                whereQuery.Append(@" AND SisEndereco.CidadeId = @CidadeId ");
            }

            if (!string.IsNullOrEmpty(filterViewModel.Bairro))
            {
                whereQuery.Append(@" AND SisEndereco.Bairro like '%'+ @Bairro +'%' ");
            }

            if (filterViewModel.NascimentoInicio.HasValue && filterViewModel.NascimentoFinal.HasValue)
            {
                whereQuery.Append(@" AND Pessoa.Nascimento BETWEEN @NascimentoInicio AND @NascimentoFinal ");
            }

            if (filterViewModel.HabilitarFiltroAtendimento)
            {
                selectQuery.Append(@"
                    AteAtendimento.Codigo AS Atendimento,
                    AteAtendimento.DataRegistro AS DataAtendimento,
                    SisUnidadeOrganizacional.Descricao AS UnidadeAtendimento,
                    AteAtendimentoStatus.Descricao AS StatusAtendimento,
                    DataAlta AS DataAltaAtendimento,");
                fromQuery.Append(@"
                    INNER JOIN AteAtendimento ON AteAtendimento.SisPacienteId = SisPaciente.Id
                    INNER JOIN SisUnidadeOrganizacional ON AteAtendimento.SisUnidadeOrganizacionalId = SisUnidadeOrganizacional.Id
                    INNER JOIN AteAtendimentoStatus On AteAtendimento.AteAtendimentoStatusId =  AteAtendimentoStatus.Id ");

                whereQuery.Append(@" AND SisPaciente.IsDeleted = @deleted AND  AteAtendimento.IsDeleted = @deleted ");

                if (filterViewModel.UltimoAtendimento)
                {
                    if (filterViewModel.UnidadeOrganizacionalId.HasValue)
                    {
                        fromQuery.Append(@" INNER JOIN (
                        SELECT Max(AteAtendimento.DataRegistro) AS DataRegistro, AteAtendimento.SisPacienteId 
                        FROM AteAtendimento WHERE AteAtendimento.IsDeleted = @deleted AND AteAtendimento.SisUnidadeOrganizacionalId = @UnidadeOrganizacionalId
                        GROUP BY AteAtendimento.SisPacienteId) AS Ultimo
                        ON Ultimo.DataRegistro = AteAtendimento.DataRegistro AND AteAtendimento.SisPacienteId =
                            Ultimo.SisPacienteId");
                    }
                    else
                    {
                        fromQuery.Append(@" INNER JOIN (
                        SELECT Max(AteAtendimento.DataRegistro) AS DataRegistro, AteAtendimento.SisPacienteId 
                        FROM AteAtendimento WHERE AteAtendimento.IsDeleted = @deleted
                        GROUP BY AteAtendimento.SisPacienteId) AS Ultimo
                        ON Ultimo.DataRegistro = AteAtendimento.DataRegistro AND AteAtendimento.SisPacienteId =
                            Ultimo.SisPacienteId");
                    }
                }


                if (filterViewModel.ConvenioId.HasValue)
                {
                    whereQuery.Append(@" AND AteAtendimento.SisConveniolId = @ConvenioId ");
                }

                if (filterViewModel.PlanoId.HasValue)
                {
                    whereQuery.Append(@" AND AteAtendimento.SisPlanoId = @PlanoId ");
                }


                if (filterViewModel.AtendimentoInicio.HasValue && filterViewModel.AtendimentoFinal.HasValue)
                {
                    whereQuery.Append(@" AND AteAtendimento.DataRegistro BETWEEN @AtendimentoInicio AND @AtendimentoFinal ");
                }

                if (filterViewModel.AtendimentoAltaInicio.HasValue && filterViewModel.AtendimentoAltaFinal.HasValue)
                {
                    whereQuery.Append(@" AND AteAtendimento.DataAlta BETWEEN @AtendimentoAltaInicio AND @AtendimentoAltaFinal ");
                }

                if (filterViewModel.UnidadeOrganizacionalId.HasValue)
                {
                    whereQuery.Append(" AND AteAtendimento.SisUnidadeOrganizacionalId = @UnidadeOrganizacionalId ");
                }

                if (filterViewModel.StatusAtendimentoId.HasValue)
                {
                    whereQuery.Append(" AND AteAtendimento.AteAtendimentoStatusId = @UnidadeOrganizacionalId ");
                }

                //TODO: Fazer o vinculo no ateAtendimento para a internação vinda da emergencia.
                if (filterViewModel.NaoInternado)
                {
                    whereQuery.Append(" AND SisPaciente.Id NOT IN (select SisPacienteId FROM AteAtendimento Where AteMotivoAltaId  = 15  AND IsDeleted = @deleted)");
                }
            }

            //TODO: Fazer o vinculo no sisPaciente quando ha alta com obito
            if (filterViewModel.NaoEnviarPacienteObito)
            {
                whereQuery.Append(" AND SisPaciente.Id NOT IN (select SisPacienteId FROM AteAtendimento WHERE  AteMotivoAltaId IN ( SELECT ID FROM AssMotivoAlta WHERE AssMotivoAltaTipoAltaId = 3)  AND IsDeleted = @deleted)");
            }

            var helper =  DataTableHelper.CreateDataTable<IndexDisparoDeMensagemViewModel, FiltroDisparoDeMensagemViewModel>(this);
            selectQuery.Length -= 1;
            
            helper.AddDefaultField("Pessoa.Id");
            helper.AddSelectClause(selectQuery.ToString());
            helper.AddFromClause(fromQuery.ToString());
            helper.EnablePagination(!filterViewModel.AllRows);
            helper.AddWhereMethod((input, dapperParameters) =>
                {
                    dapperParameters.Add("deleted", false);
                    return whereQuery.ToString();
                });
            

            var result = await helper.ExecuteAsync(filterViewModel);

            foreach (var item in result.Items)
            {
                if (item.Nascimento.HasValue)
                {
                    item.Idade = FuncoesGlobais.ObterIdadeCompleto(item.Nascimento.Value);
                }

                if (!string.IsNullOrEmpty(item.Telefone1))
                {
                    item.Telefone1 = string.Join(" <br/> ", FormataTelefone(item.Telefone1));
                }

                if (!string.IsNullOrEmpty(item.Telefone2))
                {
                    item.Telefone2 = string.Join(" <br/> ", FormataTelefone(item.Telefone2));
                }

                if (!string.IsNullOrEmpty(item.Telefone3))
                {
                    item.Telefone3 = string.Join(" <br/> ", FormataTelefone(item.Telefone3));
                }

                if (!string.IsNullOrEmpty(item.Telefone4))
                {
                    item.Telefone4 = string.Join(" <br/> ", FormataTelefone(item.Telefone4));
                }
            }

            return result;

        }

        static List<string> FormataTelefone(string telefone)
        {
            var result = new List<string>();
            if (telefone.Contains("/"))
            {
                var telefoneSplit = telefone.Split('/');
                foreach (var telefoneIndex in telefoneSplit)
                {
                    var test = FormatTelefoneAction(telefoneIndex);
                    if (!string.IsNullOrEmpty(test))
                    {
                        result.Add(test);
                    }
                }
            }
            else
            {
                var test = FormatTelefoneAction(telefone);
                if (!string.IsNullOrEmpty(test))
                {
                    result.Add(test);
                }
            }

            return result;
        }

        static string FormatTelefoneAction(string telefone)
        {
            var result = new StringBuilder();

            foreach (char c in telefone)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                result.Append(c);
            }

            return FormatPhoneNumber(result.ToString());
        }

        static string FormatPhoneNumber8Digits(long number)
        {
            return number.ToString(@"0000-0000");
        }

        static string FormatPhoneNumber9Digits(long number)
        {
            return number.ToString(@"00000-0000");
        }

        static string FormatPhoneNumber10Digits(long number)
        {
            return number.ToString(@"(00) 0000-0000");
        }

        static string FormatPhoneNumber11Digits(long number)
        {
            return number.ToString(@"(00) 00000-0000");
        }

        static string FormatPhoneNumber(string number)
        {
            if (number.Length == 8)
            {
                return FormatPhoneNumber8Digits(long.Parse(number));
            }
            else if (number.Length == 9)
            {
                return FormatPhoneNumber9Digits(long.Parse(number));
            }
            else if (number.Length == 10)
            {
                return FormatPhoneNumber10Digits(long.Parse(number));
            }
            else if (number.Length == 11)
            {
                return FormatPhoneNumber11Digits(long.Parse(number));
            }
            return null;
        }

    }
}
