using Microsoft.AspNetCore.Mvc.Infrastructure;
using Modelos;
using WebApplication1.DTOs;
using WebApplication1.Interface;
using AutoMapper;


namespace WebApplication1.Servicos
{
    public class UserFServices : IUserFuncionarioService
    {
        private readonly IUserFuncionarioRepository _repository;
        private readonly IMapper _mapper;

        public UserFServices(IUserFuncionarioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserFDTO> Alterar(UserFDTO userftdo)
        {
            var user = _mapper.Map<UserFuncionario>(userftdo);
            var useralterado = await _repository.Alterar(user);
            return _mapper.Map<UserFDTO>(useralterado);
        }

        public  async Task<UserFDTO> Excluir(int id)
        {
            var user = await _repository.Excluir(id);
            return _mapper.Map<UserFDTO>(user);
        }

        public async Task<UserFDTO> Incluir(UserFDTO userftdo)
        {
            var user = _mapper.Map<UserFuncionario>(userftdo);
            var userincluido = await _repository.Incluir(user);
            return _mapper.Map<UserFDTO>(userincluido);
        }

        public  async Task<UserFDTO> SelecionarAsync(int id)
        {
            var user = await _repository.SelecionarAsync(id);
            return _mapper.Map<UserFDTO>(user);
        }

        public  async Task<IEnumerable<UserFDTO>> SelecionarTodosAsync()
        {
            var users = await _repository.SelecionarTodosAsync();
            return _mapper.Map<IEnumerable<UserFDTO>>(users);
        }
    }
}
