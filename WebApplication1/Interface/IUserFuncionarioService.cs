using Modelos;
using WebApplication1.DTOs;

namespace WebApplication1.Interface
{
    

        public interface IUserFuncionarioService
        {
            Task<UserFDTO> Alterar(UserFDTO userfdto);
            Task<UserFDTO> Excluir(int id);
            Task<UserFDTO> Incluir(UserFDTO userfdto);
            Task<UserFDTO> SelecionarAsync(int id);
            Task<IEnumerable<UserFDTO>> SelecionarTodosAsync();
        }
    
}
