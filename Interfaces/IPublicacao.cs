using System.Collections.Generic;
using Prototipo_BackEnd.Models;

namespace Prototipo_BackEnd.Interfaces
{
    public interface IPublicacao
    {
         // CRUD
        void Create(Publicacao p);
        List<Publicacao> ReadAll();
        void Update(Publicacao e);
        void Delete(int idPublicacao);
        void Like(int idPublicacao, int idUsuario);
        
    }
}