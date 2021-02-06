using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Prototipo_BackEnd.Models;
using System.IO;
using System;
using back_end_totoal.Models;
using System.Collections.Generic;

namespace Prototipo_BackEnd.Controllers
{
    [Route("Feed")]
    public class FeedController : Controller
    {
        private const string PATH_COMENTARIO = "Database/comentarios.csv";
        private const string PATH = "Database/usuarios.csv";
        Publicacao publicacaoModel = new Publicacao();
        Comentario comentario = new Comentario();
        Usuario usuario = new Usuario();

        [Route("Listar")]
        public IActionResult Index()
        {
            ViewBag.UserPost = publicacaoModel.UsuarioPostagem(publicacaoModel.IdUsuario);
            ViewBag.Publicacoes = publicacaoModel.ReadAll();
            ViewBag.Comentario = comentario.ReadAll();

            ViewBag.IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");
            ViewBag.NascimentoLogado = HttpContext.Session.GetString("_NascimentoLogado");
            ViewBag.NomeLogado = HttpContext.Session.GetString("_UserName");
            ViewBag.EmailLogado = HttpContext.Session.GetString("_EmailLogado");
            ViewBag.UserNomeLogado = HttpContext.Session.GetString("_UserNameLogado");
            ViewBag.SenhaLogado = HttpContext.Session.GetString("_SenhaLogado");

            ViewBag._IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");

            bool redirecionamentoLogado = false;

            if(ViewBag._IdUsuarioLogado != null){
                redirecionamentoLogado = true;
            }

            if(redirecionamentoLogado == false){
                return LocalRedirect("~/Login");
            }

            return View();
        }

        [Route("Publicar")]
        public IActionResult Publicar(IFormCollection form)
        {
            Publicacao novaPublicacao = new Publicacao();

            novaPublicacao.IdPublicacao = publicacaoModel.IdGenerator();
            novaPublicacao.IdUsuario = int.Parse(HttpContext.Session.GetString("_IdUsuarioLogado"));
            novaPublicacao.NomeUsuario = HttpContext.Session.GetString("_UserNameLogado");
            novaPublicacao.Legenda = form["Legenda"];
            novaPublicacao.LocalizacaoUsuario = form["Localizacao"];
            novaPublicacao.Likes = 0;

            if(form.Files.Count > 0){
                var file = form.Files[0];
                var folder = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot/img/Publicacoes" );

                if(!Directory.Exists(folder)){
                    Directory.CreateDirectory(folder);
                }

                var PATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/", folder, file.FileName);

                using (var stream = new FileStream(PATH,FileMode.Create)){
                    file.CopyTo(stream);
                }
        
                

                novaPublicacao.Imagem = file.FileName;
            }else{
                novaPublicacao.Imagem = "post.png";
            }

            publicacaoModel.Create(novaPublicacao);

            ViewBag.Publicacoes = publicacaoModel.ReadAll();
            
            return LocalRedirect ("~/Feed/Listar");
        }

        [Route("{id}")]
        public IActionResult Delete(int id){
            publicacaoModel.Delete(id);

            ViewBag.Publicacoes = publicacaoModel.ReadAll();

            return LocalRedirect("~/Feed/Listar");
        }

        [Route("Comentar")]
        public IActionResult Comentar(IFormCollection form){
            
            Comentario novoComentario = new Comentario();

            novoComentario.IdComentario = comentario.IdGenerator();
            novoComentario.Mensagem = form["Comentario"];
            novoComentario.IdUsuario = int.Parse(HttpContext.Session.GetString("_IdUsuarioLogado"));
            novoComentario.IdPublicacao = int.Parse(form["IdPublicacao"]);

            novoComentario.UserName = HttpContext.Session.GetString("_UserNameLogado");
            
            comentario.CriarComentario(novoComentario);
            return LocalRedirect("~/Feed/Listar");
        }
    }
}