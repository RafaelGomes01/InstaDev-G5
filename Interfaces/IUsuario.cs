using System.Collections.Generic;
using back_end_totoal.Models;

namespace back_end_totoal.Interfaces
{
    public interface IUsuario
    {
         void CadastrarUsuario(Usuario usuario);
         void EditarUsuario(int id, Usuario usuario);
         List<Usuario> MostrarUsuario();
         void DeletarUsuario(int id);
         List<Usuario> ListarUsuarios(Usuario usuario);
    }
}