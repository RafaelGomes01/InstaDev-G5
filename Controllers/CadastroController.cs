using System.Collections.Generic;
using System.IO;
using back_end_totoal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end_totoal.Controllers
{   
    // Determinar a rota
    [Route("Cadastrar")]
    public class CadastroController : Controller
    {
        // Instanciando a Classe Usuario
        Usuario usuario = new Usuario();

        [TempData] // Arquivo Temporario
        public string MensagemNome { get; set; }

        [TempData] // Arquivo Temporario
        public string MensagemEmail { get; set; }

        [TempData] // Arquivo Temporario
        public string MensagemUserName { get; set; }

        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";

        // Iniciar a pagina
        public IActionResult Index(){
            return View();
        }

        // Determinar a rota
        [Route("Cadastrar")]
        // Pegar as informações do Front-End e passar para o Back-End
        public IActionResult Cadastrar(IFormCollection form){

            Usuario novoUsuario = new Usuario();

            List<string> linhas = usuario.ReadAllLinesCSV(PATH);

            // Nome
            var userIgual = linhas.Find(x=> x.Split(";")[1] == form["Nome"]);

            // Email
            var emailIgual = linhas.Find(x=> x.Split(";")[5] == form["Email"]);

            // UserName
            var userNameIgual = linhas.Find(x=> x.Split(";")[6] == form["UserName"]);

            if(userIgual != null || emailIgual != null || userNameIgual != null){
                if(userIgual != null){
                    MensagemNome = $"Ja existe um usuario com esse nome";
                }

                else if(emailIgual != null){
                    MensagemEmail = $"Ja existe um usuario com esse email";
                }

                else if(userNameIgual != null){
                     MensagemUserName = $"Ja existe um usuario com esse username";
                }
            }else{
                novoUsuario.IdUsuario = usuario.IdGenerator();
                novoUsuario.Nome = form["Nome"];
                novoUsuario.Email = form["Email"];
                novoUsuario.Foto = "user_padrao.png";
                novoUsuario.UserName = form["UserName"];
                novoUsuario.Senha = form["Senha"];
                usuario.CadastrarUsuario(novoUsuario);
                return LocalRedirect("~/Login");
            }
            
            ViewBag.Usuarios = usuario.MostrarUsuario();
            
            return LocalRedirect("~/Cadastrar");
        }

        // Determinar a rota
        [Route("{id}")]
        // Deletar um usuario
        public IActionResult Excluir(int id){
            // Excluir o usuario dependendo do Metodo
            usuario.DeletarUsuario(id);
            // Adicionar as atualizações a ViewBag
            ViewBag.Usuarios = usuario.MostrarUsuario();
            // Retornar com uma atualização da pagina
            return LocalRedirect("~/Cadastrar/Listar");
        }
    }
}

