using System.Collections.Generic;
using System.IO;

namespace back_end_totoal.Models
{
    public class Comentario : InstaDevBase
    {
        public int IdComentario { get; set; }
        public string Mensagem { get; set; }
        public int IdUsuario { get; set; }
        public int IdPublicacao { get; set; }
        public string UserName {get; set;}

        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";

        public Comentario(){
            CreateFolderAndFile(PATH_COMENTARIOS, PATH_SEGUINDO);
        }

        public void CriarComentario(Comentario comentario){
            string[] linhas = {PrepareCSVLineComentario(comentario)};
            File.AppendAllLines(PATH_COMENTARIOS, linhas);
        }

        public string PrepareCSVLineComentario(Comentario comentario){
            return $"{comentario.IdComentario};{comentario.Mensagem};{comentario.IdUsuario};{comentario.IdPublicacao};{comentario.UserName}";
        }

        public List<Comentario> ReadAll(){
            // Criar uma lista com os usuarios
            List<Comentario> comentarios = new List<Comentario>();
            // Pegar as linhas do CSV
            string[] linhas = File.ReadAllLines(PATH_COMENTARIOS);
            // Foreach para listar os usuarios
            foreach (var item in linhas){
                string[] linha = item.Split(";");

                Comentario comentario = new Comentario();
                comentario.IdComentario = int.Parse(item.Split(";")[0]);
                comentario.Mensagem = item.Split(";")[1];
                comentario.IdUsuario = int.Parse(item.Split(";")[2]);
                comentario.IdPublicacao = int.Parse(item.Split(";")[3]);
                comentario.UserName = item.Split(";")[4];
                
                // Adicionando os dados ao usuario
                comentarios.Add(comentario);
            }
            // Retornando a Lista
            return comentarios;
        }

        public void EditarComentario(int id, Comentario comentario){
            // Pegar todas as linhas do CSV
            List<string> comentarios = ReadAllLinesCSV(PATH_COMENTARIOS);
            // Remover a linha aonde o ID for igual ao requerido
            comentarios.RemoveAll(x => int.Parse(x.Split(";")[0]) == id);
            // Adicionar com o dado Alterado
            comentarios.Add(PrepareCSVLineComentario(comentario));
            // Reescrever no arquivo CSV
            RewriteCSV(PATH_COMENTARIOS, comentarios);
        }
        
        // Deletar um usuario no CSV
        public void DeletarComentario(int id){
            // Lendo as linhas do CSV
            List<string> comentarios = ReadAllLinesCSV(PATH_COMENTARIOS);
            // Removendo aquela que for requerida pelo ID
            comentarios.RemoveAll(x => x.Split(";")[0] == id.ToString());
            // Reescrever com o arquivo deletado
            RewriteCSV(PATH_COMENTARIOS, comentarios);
        }

        public int IdGenerator(){
            string[] linhas = File.ReadAllLines(PATH_COMENTARIOS);
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
    }
}