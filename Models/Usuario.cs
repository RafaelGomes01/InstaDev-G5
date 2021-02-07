using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using back_end_totoal.Interfaces;
using Microsoft.AspNetCore.Http;

namespace back_end_totoal.Models
{
    public class Usuario : InstaDevBase, IUsuario
    {
        // Atributos da Classe
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Seguidores = 0;
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Senha { get; set; }
        
        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";



        // Criar a paste e arquivo
        public Usuario(){
            CreateFolderAndFile(PATH, PATH_SEGUINDO);
        }

        // Adicionar um usuario no arquivo CSV
        public void CadastrarUsuario(Usuario usuario){
            // Criar linhas com os objetos que retornaram da classe PrepareCSVLine
            string[] linhas = {PrepareCSVLine(usuario)};
            File.AppendAllLines(PATH, linhas);
        }

        // Retornar uma linha preparada para ser adicionada no CSV
        public string PrepareCSVLine(Usuario usuario){
            // Retornar uma string com todos os atributos da classe
            return $"{usuario.IdUsuario};{usuario.Nome};{usuario.Foto};{usuario.DataNascimento};{usuario.Seguidores};{usuario.Email};{usuario.UserName};{usuario.Senha}";
        }

        // Listar todos os Usuarios
        public int IdGenerator(){
            string[] linhas = File.ReadAllLines(PATH);
            int IdUsuario = 0;
            bool loop = true;
            
            foreach (var item in linhas)
            {
                do{
                    if(IdUsuario == int.Parse(item.Split(";")[0])){
                        IdUsuario++;
                    }else{loop = false;}
                }while(loop == true);
            }

            foreach (var item in linhas)
            {
                do{
                    if(IdUsuario == int.Parse(item.Split(";")[0])){
                        IdUsuario++;
                    }else{loop = false;}
                }while(loop == true);
            }

            return IdUsuario;
        }

        // Listar todos os atributos dos usuarios
        public List<Usuario> MostrarUsuario(){
            // Criar uma lista com os usuarios
            List<Usuario> usuarios = new List<Usuario>();
            // Pegar as linhas do CSV
            string[] linhas = File.ReadAllLines(PATH);
            // Foreach para listar o usuario
            foreach (var item in linhas){
                string[] linha = item.Split(";");
                Usuario usuario = new Usuario();
                usuario.IdUsuario = Int32.Parse(linha[0]);
                usuario.Nome = linha[1];
                usuario.Foto = linha[2];
                usuario.DataNascimento = DateTime.Parse(linha[3]);
                usuario.Seguidores = int.Parse(linha[4]);
                usuario.Email = linha[5];
                usuario.UserName = linha[6];
                usuario.Senha = linha[7];
                // Adicionando os dados ao usuario
                usuarios.Add(usuario);
            }
            // Retornando a Lista
            return usuarios;
        }

        // Atualizar um dado no CSV
        public void EditarUsuario(int id, Usuario usuario){
            // Pegar todas as linhas do CSV
            List<string> linhas = ReadAllLinesCSV(PATH);
            // Remover a linha aonde o ID for igual ao requerido
            linhas.RemoveAll(x => int.Parse(x.Split(";")[0]) == id);
            // Adicionar com o dado Alterado
            linhas.Add(PrepareCSVLine(usuario));
            // Reescrever no arquivo CSV
            RewriteCSV(PATH, linhas);
        }

        // Deletar um usuario no CSV
        public void DeletarUsuario(int id){
            // Lendo as linhas do CSV
            List<string> linhas = ReadAllLinesCSV(PATH);
            // Removendo aquela que for requerida pelo ID
            linhas.RemoveAll(x => x.Split(";")[0] == id.ToString());
            // Reescrever com o arquivo deletado
            RewriteCSV(PATH, linhas);
        }

        // Listar todos os Usuarios
        public List<Usuario> ListarUsuarios(Usuario usuario){
            // Criar uma lista com os usuarios
            List<Usuario> usuarios = new List<Usuario>();
            // Pegar as linhas do CSV
            string[] linhas = File.ReadAllLines(PATH);
            // Foreach para listar os usuarios
            foreach (var item in linhas){
                string[] linha = item.Split(";");
                usuario.Foto = linha[3];
                usuario.Seguidores = Int32.Parse(linha[5]);
                usuario.UserName = linha[7];
                // Adicionando os dados ao usuario
                usuarios.Add(usuario);
            }
            // Retornando a Lista
            return usuarios;
        }

        // Adicionar um seguidor a um usuario logado
        public void Seguir(int id_UsuarioLogado, int id_UsuarioSeguindo){
            // Linha que vai ser adicionada no CSV
            string[] follow = {PrepareCSVLineFollow(id_UsuarioLogado, id_UsuarioSeguindo)};

            // Adicionar Linha no CSV
            File.AppendAllLines(PATH_SEGUINDO, follow);
        }

        // Preparar a linha para ser adicionada no CSV Seguindo 
        public string PrepareCSVLineFollow(int id_UsuarioLogado, int id_UsuarioSeguindo){
            // Retornar a linha correta para a adição
            return $"{id_UsuarioLogado};{id_UsuarioSeguindo}";
        }

        // Remover um seguidor de um usuario logado
        public void DeixarSeguir(int id_UsuarioLogado, int id_UsuarioSeguindo){
            // Adicionar uma lista que vai ler todos as linhas do CSV
            List<string> seguindo = ReadAllLinesCSV(PATH_SEGUINDO);
            // Preparar a linha que vai ser removida
            string prepareCSVLineRemove = $"{id_UsuarioLogado};{id_UsuarioSeguindo}";
            // Utilizando uma expressão lambda para remover os usuarios
            seguindo.RemoveAll(x => x == prepareCSVLineRemove);
            // Reescrever o CSV atualizado
            RewriteCSV(PATH_SEGUINDO, seguindo); 
        }

        // Listar Perfil do usuario Logado
        public string SeguidoresAndSeguindo(int id){
            string[] linhas = File.ReadAllLines(PATH_SEGUINDO);
            int seguindo = 0;
            int seguidores = 0;

            foreach (var item in linhas)
            {
                if(int.Parse(item.Split(";")[0]) == id){
                    seguindo++;
                }
            }

            foreach (var item in linhas)
            {
                if(int.Parse(item.Split(";")[1]) == id){
                    seguidores++;
                }
            }

            return $"{seguindo};{seguidores}";
        }

        public void AlterarFoto(int userId, string image){
            List<string> linhas = ReadAllLinesCSV(PATH);
            var user = linhas.Find(x=> int.Parse(x.Split(";")[0]) == userId);

            Usuario usuarioEditado = new Usuario();

            usuarioEditado.IdUsuario = int.Parse(user.Split(";")[0]);
            usuarioEditado.Nome = user.Split(";")[1];
            usuarioEditado.Foto = image;
            usuarioEditado.DataNascimento = DateTime.Parse(user.Split(";")[3]);
            usuarioEditado.Seguidores = 0;
            usuarioEditado.Email = user.Split(";")[5];
            usuarioEditado.UserName = user.Split(";")[6];
            usuarioEditado.Senha = user.Split(";")[7];


            EditarUsuario(userId, usuarioEditado);
        }
    }
}