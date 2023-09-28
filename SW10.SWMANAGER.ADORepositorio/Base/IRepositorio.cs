using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ADORepositorio.Base
{
    public interface IRepositorio<T> : IDisposable where T : class
    {
        Task<int> Inserir(T input);

        Task Alterar(T input);

        Task Excluir(long id);

        Task<T> Obter(long id);

        Task<ICollection<T>> Listar();
    }
}
