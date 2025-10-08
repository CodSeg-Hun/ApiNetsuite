using ApiNetsuite.Modelo;
using System.Threading.Tasks;

namespace ApiNetsuite.Repositorio.IRepository
{
    public interface IUsuariosSQLServer
    {
        Task<UsuarioAPI> DameUsuario(LoginAPI login);
    }
}
