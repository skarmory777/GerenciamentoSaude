using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturamentoItemAtendimento.dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturamentoItemAtendimento
{
    public class FaturamentoItemAtendimentoManager : DomainService, IFaturamentoItemAtendimentoManager
    {
        public async Task AdicionaItemAsync(FaturamentoItemAtendimentoDto dto)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                await faturamentoItemAtendimentoRepository.Object.InsertOrUpdateAsync(FaturamentoItemAtendimentoDto.Mapear(dto));
            }
        }

        public void AdicionaItem(FaturamentoItemAtendimentoDto dto)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                faturamentoItemAtendimentoRepository.Object.InsertOrUpdate(FaturamentoItemAtendimentoDto.Mapear(dto));
            }
        }

        public async Task AdicionaItemAsync(IEnumerable<FaturamentoItemAtendimentoDto> dtoList)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                foreach (var dto in dtoList)
                {
                    await faturamentoItemAtendimentoRepository.Object.InsertOrUpdateAsync(FaturamentoItemAtendimentoDto.Mapear(dto));    
                }
            }
        }

        public void AdicionaItem(IEnumerable<FaturamentoItemAtendimentoDto> dtoList)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                foreach (var dto in dtoList)
                {
                    faturamentoItemAtendimentoRepository.Object.InsertOrUpdate(FaturamentoItemAtendimentoDto.Mapear(dto));    
                }
            }
        }

        public async Task RemoverItemAsync(FaturamentoItemAtendimentoDto dto)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                var items = await faturamentoItemAtendimentoRepository.Object.GetAll().Where(x=> x.Entidade == dto.Entidade && x.EntidadeId ==dto.EntidadeId).ToListAsync();

                foreach (var item in items)
                {
                    await faturamentoItemAtendimentoRepository.Object.DeleteAsync(item);
                }
            }
        }

        public void RemoverItem(FaturamentoItemAtendimentoDto dto)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                var items = faturamentoItemAtendimentoRepository.Object.GetAll().Where(x=> x.Entidade == dto.Entidade && x.EntidadeId ==dto.EntidadeId).ToList();

                foreach (var item in items)
                {
                    faturamentoItemAtendimentoRepository.Object.Delete(item);
                }
            }
        }

        public async Task RemoverItemAsync(IEnumerable<FaturamentoItemAtendimentoDto> dtoList)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                var items = await faturamentoItemAtendimentoRepository.Object.GetAll().Where(x=> dtoList.Any(dtoListx => dtoListx.Entidade == x.Entidade && dtoListx.EntidadeId == x.EntidadeId)).ToListAsync();

                foreach (var item in items)
                {
                    await faturamentoItemAtendimentoRepository.Object.DeleteAsync(item);
                }
            }
        }

        public void RemoverItem(IEnumerable<FaturamentoItemAtendimentoDto> dtoList)
        {
            using (var faturamentoItemAtendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento, long>>())
            {
                var items = faturamentoItemAtendimentoRepository.Object.GetAll().Where(x=> dtoList.Any(dtoListx => dtoListx.Entidade == x.Entidade && dtoListx.EntidadeId == x.EntidadeId)).ToList();

                foreach (var item in items)
                {
                    faturamentoItemAtendimentoRepository.Object.Delete(item);
                }
            }
        }
    }

    public interface IFaturamentoItemAtendimentoManager : IDomainService
    {
        Task AdicionaItemAsync(FaturamentoItemAtendimentoDto dto);
        
        void AdicionaItem(FaturamentoItemAtendimentoDto dto);
        
        Task AdicionaItemAsync(IEnumerable<FaturamentoItemAtendimentoDto> dtoList);
        
        void AdicionaItem(IEnumerable<FaturamentoItemAtendimentoDto> dtoList);
        
        Task RemoverItemAsync(FaturamentoItemAtendimentoDto dto);
        
        void RemoverItem(FaturamentoItemAtendimentoDto dto);
        
        Task RemoverItemAsync(IEnumerable<FaturamentoItemAtendimentoDto> dtoList);
        
        void RemoverItem(IEnumerable<FaturamentoItemAtendimentoDto> dtoList);
    }
}