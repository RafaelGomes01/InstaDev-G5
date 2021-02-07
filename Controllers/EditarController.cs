using System;
using System.Collections.Generic;
using System.IO;
using back_end_totoal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end_totoal.Controllers
{
    [Route("Editar")]
    
    public class Editar : Controller
    {
        Usuario usuario = new Usuario();
        
        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";

        [Route("Listar")]
        public IActionResult Index()
        {
            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");
            ViewBag.UserNameLogado = HttpContext.Session.GetString("_UserName");
            ViewBag.NomeLogado = HttpContext.Session.GetString("_NomeLogado");
            ViewBag.EmailLogado = HttpContext.Session.GetString("_EmailLogado");
            ViewBag.IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            
            bool redirecionamentoLogado = false;

            if(ViewBag.IdUsuarioLogado != null){
                redirecionamentoLogado = true;
            }

            if(redirecionamentoLogado == false){
                return LocalRedirect("~/Login");
            }
            
            return View();
        }

        [Route("EditarPerfil")]
        public IActionResult EditarPerfil(IFormCollection form){
            Usuario novoUsuario = new Usuario();

            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");

            string nome = form["Nome"];
            string username = form["UserName"];
            string email = form["Email"]; 
            
            // Upload
            if(form.Files.Count > 0){
                var file = form.Files[0];
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Usuarios");

                if(!Directory.Exists(folder)){Directory.CreateDirectory(folder);}

                var path = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot/img/", folder, file.FileName);

                using(var stream = new FileStream(path, FileMode.Create)){
                    file.CopyTo(stream);
                }

                novoUsuario.Foto = file.FileName;
            }else{
                novoUsuario.Foto = ViewBag.FotoLogado;
            }

            novoUsuario.IdUsuario = int.Parse(HttpContext.Session.GetString("_IdUsuarioLogado"));
            novoUsuario.Nome = nome;
            novoUsuario.Email = email;
            novoUsuario.UserName = username;
            novoUsuario.Senha = HttpContext.Session.GetString("_SenhaLogado");

            usuario.EditarUsuario(novoUsuario.IdUsuario, novoUsuario);
            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");

            return LocalRedirect("~/Perfil");
        }

        public IActionResult ExcluirPerfil(){
            int id = int.Parse(HttpContext.Session.GetString("_IdUsuarioLogado"));
            usuario.DeletarUsuario(id);
            return LocalRedirect("~/Cadastrar");
        }
    }
}