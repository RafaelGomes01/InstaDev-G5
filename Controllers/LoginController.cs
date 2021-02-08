using System;
using System.Collections.Generic;
using back_end_totoal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end_totoal.Controllers
{
    // Rota padrão para a pagina login
    [Route("Login")]
    public class LoginController : Controller
    {
        // Instaciamento das classes
        Usuario usuarioModel = new Usuario();
        Comentario comentario = new Comentario();

        // Atributos da Classe
        [TempData] // Arquivo Temporario
        public string Mensagem { get; set; }        

        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";

        // Index para chama a pagina
        public IActionResult Index(){
            ViewBag._IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            ViewBag.Comentario = comentario.ReadAll();
            return View();
        }

        // Rota para o metodo logar
        [Route("Logar")]
        // Logar em uma conta
        public IActionResult Logar(IFormCollection form){
            // Ler todas as linhas do CSV
            List<string> usuarios = usuarioModel.ReadAllLinesCSV(PATH);
            // Validar se a senha e o usuario estão corretos
            var logado =
            usuarios.Find(
                x =>
                x.Split(";")[6] == form["UserName"] &&
                x.Split(";")[7] == form["Senha"]
            );
            // Validar se os arquivos de UserName e Senha foram armazenadas
            if(logado != null){
                // Definir um sessão para um usuario
                HttpContext.Session.SetString("_IdUsuarioLogado", logado.Split(";")[0]);
                HttpContext.Session.SetString("_NomeLogado", logado.Split(";")[1]);
                HttpContext.Session.SetString("_FotoLogado", logado.Split(";")[2]);
                HttpContext.Session.SetString("_NascimentoLogado", logado.Split(";")[3]);
                HttpContext.Session.SetString("_UserName", logado.Split(";")[6]);
                HttpContext.Session.SetString("_EmailLogado", logado.Split(";")[5]);
                HttpContext.Session.SetString("_UserNameLogado", logado.Split(";")[6]);
                HttpContext.Session.SetString("_SenhaLogado", logado.Split(";")[7]);

                // Retornando para o "Feed"
                return LocalRedirect("~/Feed/Listar");
            }
            // Caso ele não aceite os dados de UserName e Senha, ele passa essa mensagem
            Mensagem = "Dados Incorretos, Tente novamente";
            // E redireciona para a pagina de login para tentar novamente
            return LocalRedirect("~/Login");
        }

        // Rota para o metodo de Logout
        [Route("Logout")]
        // Logout de um usuario
        public IActionResult Logout(){
            // Finalizar a sessão de um usuario
            HttpContext.Session.Remove("_UserName");

            HttpContext.Session.Remove("_IdUsuarioLogado");
            HttpContext.Session.Remove("_NomeLogado");
            HttpContext.Session.Remove("_FotoLogado");
            HttpContext.Session.Remove("_NascimentoLogado");
            HttpContext.Session.Remove("_EmailLogado");
            HttpContext.Session.Remove("_SeguindoLogado");
            HttpContext.Session.Remove("_SeguidoresLogado");
            HttpContext.Session.Remove("_UserNameLogado");
            HttpContext.Session.Remove("_SenhaLogado");

            // E redireciona para a pagina de login
            return LocalRedirect("~/Login");
        }
    }
}