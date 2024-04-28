using Modelos;
using WebApplication1.DTOs;

namespace WebApplication1.Interface
{
    public interface IUserFuncionarioRepository
    {
        Task<UserFuncionario> Incluir(UserFuncionario usuario);

        Task<UserFuncionario> Alterar(UserFuncionario usuario);

        Task<UserFuncionario> Excluir(int id);

        Task<UserFuncionario> SelecionarAsync(int id);
        Task<IEnumerable<UserFuncionario>> SelecionarTodosAsync();
    }
}
