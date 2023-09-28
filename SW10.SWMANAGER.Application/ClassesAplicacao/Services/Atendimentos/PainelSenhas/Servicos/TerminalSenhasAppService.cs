using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Servicos
{

    using Abp.Auditing;
    using Abp.Dependency;

    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Impressora;

    public class TerminalSenhasAppService : SWMANAGERAppServiceBase, ITerminalSenhasAppService
    {
        private readonly IIocManager _iocManager;

        public TerminalSenhasAppService(IIocManager iocManager)
        {
            this._iocManager = iocManager;
        }

        [UnitOfWork]
        public async Task<SenhaIndex> GerarSenha(long filaId)
        {
            //SenhaIndex senhaIndex = null;


            SenhaIndex senhaIndex = null;
            using (var unitOfWorkManager = this._iocManager.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var filaRepository = this._iocManager.ResolveAsDisposable<IRepository<Fila, long>>())
            using (var senhaRepository = this._iocManager.ResolveAsDisposable<IRepository<Senha, long>>())
            using (var senhaMovimentacaoRepository = this._iocManager.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var fila = await filaRepository.Object
                                      .GetAll()
                                      //.AsNoTracking()
                                      .Include(i => i.Empresa)
                                      .FirstOrDefaultAsync(w => w.Id == filaId).ConfigureAwait(false);

                //var ultimaSenha = _senhaRepository.GetAll()
                //                                  .Where(w => w.FilaId == filaId)
                //                                  .ToList()
                //                                  .LastOrDefault();

                var numeroSenha = 0;
                if (fila.UltimaSenha == 0)
                {
                    numeroSenha = fila.UltimaSenha = fila.NumeroInicial;
                    fila.UltimaZera = DateTime.Now;
                }
                else
                {
                    if (fila.UltimaSenha < fila.NumeroFinal)//   && !(((DateTime)fila.HoraZera).TimeOfDay < DateTime.Now.TimeOfDay && (fila.UltimaZera.Date < DateTime.Now.Date  ) ))
                    {
                        numeroSenha = ++fila.UltimaSenha;
                    }
                    else
                    {
                        numeroSenha = fila.UltimaSenha = fila.NumeroInicial;
                        fila.UltimaZera = DateTime.Now;
                    }
                }


                await filaRepository.Object.UpdateAsync(fila).ConfigureAwait(false);

                var senha = new Senha
                {
                    Numero = numeroSenha,
                    DataHora = DateTime.Now,
                    FilaId = filaId
                };


                var senhaId = await senhaRepository.Object.InsertAndGetIdAsync(senha).ConfigureAwait(false);

                var senhaMov = new SenhaMovimentacao
                {
                    SenhaId = senhaId,
                    TipoLocalChamadaId = fila.TipoLocalChamadaInicialId,
                    DataHora = DateTime.Now
                };


                await senhaMovimentacaoRepository.Object.InsertAndGetIdAsync(senhaMov).ConfigureAwait(false);

                unitOfWork.Complete();
                await unitOfWork.CompleteAsync().ConfigureAwait(false);

                senhaIndex = new SenhaIndex
                {
                    NumeroSenha = senha.Numero,
                    TipoLocalChamada = fila.Descricao,
                    Data = senhaMov.DataHora,
                    Hospital = fila.Empresa?.NomeFantasia
                };
            }
            //Criar Impressão da senha

            return senhaIndex;
        }


        public async Task<SenhaIndex> GerarSenhaEImprimir(long filaId, string printerName)
        {
            var senha = await this.GerarSenha(filaId).ConfigureAwait(false);

            await this.ImprimirSenha(senha.TipoLocalChamada, senha.NumeroSenha, senha.Hospital, printerName, filaId).ConfigureAwait(false);

            return senha;
        }

        [UnitOfWork(false)]
        private async Task ImprimirSenha(string tipoLocalChamada, int numero, string hospital, string printerName, long filaId)
        {
            using (var impressoraAppService = this._iocManager.ResolveAsDisposable<IImpressoraArquivosAppService>())
            using (var modeloTextoAppService = this._iocManager.ResolveAsDisposable<IModeloTextoAppService>())
            using (var filaRepository = this._iocManager.ResolveAsDisposable<IRepository<Fila, long>>())
            {
                var fila = await filaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == filaId).ConfigureAwait(false);

                if (fila.IsNaoImprimeSenha)
                {
                    return;
                }

                var modelo = await modeloTextoAppService.Object.ObterPorTipoAsync((long)EnumTipoModelo.TerminalSenha).ConfigureAwait(false);

                if (modelo == null)
                {
                    throw new Exception("Não há modelo de impressão");
                }

                var texto = modelo.Texto
                    .MergeTexto("NomeHospital", hospital)
                    .MergeTexto("Numero", numero.ToString())
                    .MergeTexto("Fila", tipoLocalChamada)
                    .MergeTexto("Data", DateTime.Now.ToString("dd/MM/yyyy"));

                var uuidPdf = $"_TerminalDeSenha-{Guid.NewGuid()}.pdf";

                var file = modelo.GerarPdf(texto);

                impressoraAppService.Object.EnviarParaImprimir(printerName, file, uuidPdf, fila.QtdImpressaoSenha);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public List<FilaTerminalIndex> ListarFilasDisponiveis()
        {
            try
            {
                var filasIndex = new List<FilaTerminalIndex>();

                using (var _filaRepository = this._iocManager.ResolveAsDisposable<IRepository<Fila, long>>())
                {

                    var diaSemana = DateTime.Now.DayOfWeek;

                    var filas = _filaRepository.Object.GetAll().AsNoTracking().Where(
                            w => w.IsAtivo && !w.IsNaoImprimeSenha && ((diaSemana == DayOfWeek.Sunday && w.IsDomingo)
                                                                       || (diaSemana == DayOfWeek.Monday && w.IsSegunda)
                                                                       || (diaSemana == DayOfWeek.Tuesday && w.IsTerca)
                                                                       || (diaSemana == DayOfWeek.Wednesday && w.IsQuarta)
                                                                       || (diaSemana == DayOfWeek.Thursday && w.IsQuinta)
                                                                       || (diaSemana == DayOfWeek.Friday && w.IsSexta)
                                                                       || (diaSemana == DayOfWeek.Saturday && w.IsSabado)))
                        .ToList();

                    foreach (var fila in filas)
                    {
                        filasIndex.Add(new FilaTerminalIndex { FilaId = fila.Id, DescricaoFila = fila.Descricao, Cor = fila.Cor });
                    }

                    return filasIndex;
                }
            }
            catch(Exception e)
            {
                this.Logger.Error(e.StackTrace);
                throw;
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public List<FilaTerminalIndex> ListarFilasDisponiveis(long? tipoLocalChamadaId)
        {
            try
            {
                using (var _filaRepository = this._iocManager.ResolveAsDisposable<IRepository<Fila, long>>())
                {

                    var diaSemana = DateTime.Now.DayOfWeek;
                    var query = _filaRepository.Object.GetAll().AsNoTracking().Where(
                            w => w.IsAtivo && !w.IsNaoImprimeSenha && ((diaSemana == DayOfWeek.Sunday && w.IsDomingo)
                                                                       || (diaSemana == DayOfWeek.Monday && w.IsSegunda)
                                                                       || (diaSemana == DayOfWeek.Tuesday && w.IsTerca)
                                                                       || (diaSemana == DayOfWeek.Wednesday && w.IsQuarta)
                                                                       || (diaSemana == DayOfWeek.Thursday && w.IsQuinta)
                                                                       || (diaSemana == DayOfWeek.Friday && w.IsSexta)
                                                                       || (diaSemana == DayOfWeek.Saturday && w.IsSabado)));
                    if (tipoLocalChamadaId.HasValue)
                    {
                        query = query.Where(x => x.TipoLocalChamadaInicialId == tipoLocalChamadaId);
                    }

                    return query.ToList().Select(fila => new FilaTerminalIndex { FilaId = fila.Id, DescricaoFila = fila.Descricao, Cor = fila.Cor }).ToList();
                }
            }
            catch (Exception e)
            {
                this.Logger.Error(e.StackTrace);
                throw;
            }

        }



        public async Task ChamarSenha(long tipoLocalChamada, long localChamadaId, long senhaMovimentacaoId)
        {
            using (var unitOfWorkManager = this._iocManager.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var painelTipoLocalChamadaRepository = this._iocManager.ResolveAsDisposable<IRepository<PainelTipoLocalChamada, long>>())
            using (var senhaMovimentacaoRepository = this._iocManager.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
            using (var senhaMovimentacaoPainelRepository = this._iocManager.ResolveAsDisposable<IRepository<SenhaMovimentacaoPainel, long>>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var senhaMovimentacao = await senhaMovimentacaoRepository.Object
                                                                .GetAll()
                                                                .Include(x => x.Senha)
                                                                .FirstOrDefaultAsync(w => w.Id == senhaMovimentacaoId).ConfigureAwait(false);


                var paineisTipoLocalChamadas = await painelTipoLocalChamadaRepository.Object.GetAll().AsNoTracking()
                                                                              .Where(w => w.TipoLocalChamadaId == tipoLocalChamada
                                                                                       && w.PainelId != null
                                                                                       && !w.Painel.IsDeleted)
                                                                              .ToListAsync().ConfigureAwait(false);
                foreach (var item in paineisTipoLocalChamadas)
                {
                    var senhaMovimentacaoPainel =
                        new SenhaMovimentacaoPainel
                        {
                            IsMostra = true,
                            PainelId = (long)item.PainelId,
                            SenhaMovimentacaoId = senhaMovimentacaoId
                        };
                    await senhaMovimentacaoPainelRepository.Object.InsertAndGetIdAsync(senhaMovimentacaoPainel).ConfigureAwait(false);
                }

                senhaMovimentacao.LocalChamadaId = localChamadaId;
                senhaMovimentacao.DataHoraFinal = DateTime.Now;


                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();
            }
        }
    }
}
