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
        private const string PATH_FOLLOW = "Database/seguindo.csv";
        private const string PATH = "Database/usuarios.csv";

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

        // =========================================== Mudar de Classe ===================================================== //

        // Determinar a rota
        [Route("Seguir")]
        // Seguir um usuario
        public IActionResult Seguir(int id){
            // ViewBag que contem o ID do usuario logado
            ViewBag.IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            // Conversão de string para int
            int id_UsuarioLogado = int.Parse(ViewBag.IdUsuarioLogado);
            // Todas as linhas do CSV
            List<string> seguindo = usuario.ReadAllLinesCSV(PATH_FOLLOW);
            // Expressão Lambda para validar se o usuario ja esta ou não seguindo aquele perfil
            var JaSeguindo =
            seguindo.Find(
                x =>
                x == $"{id_UsuarioLogado};{id}"
            );
            // Validar se o usuario ja esta seguindo aquele perfil
            if(JaSeguindo != null){
                // Caso sim, ele vai recaregar a pagina
                return LocalRedirect("~/Cadastrar/Listar");
            } 
            // Caso não, ele vai utilizar o metodo Seguir
            usuario.Seguir(id_UsuarioLogado , id);
            // Retornando com uma atualização da pagina
            return LocalRedirect("~/Cadastrar/Listar");
        }

        // Determinar a rota
        [Route("DeixarSeguir")]
        // Deixar de seguir um usuario
        public IActionResult DeixarSeguir(int id){
            // ViewBag que contem o ID do usuario logado
            ViewBag.IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            // Conversão de string para int
            int id_UsuarioLogado = int.Parse(ViewBag.IdUsuarioLogado);
            // Utilizando o metodo Deixar de Seguir
            usuario.DeixarSeguir(id_UsuarioLogado, id);
            // Retornando com uma atualização da pagina
            return LocalRedirect("~/Cadastrar/Listar");
        }
    }
}

