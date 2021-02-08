using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Prototipo_BackEnd.Models;
using System.IO;
using System;
using back_end_totoal.Models;
using System.Collections.Generic;
using back_end_totoal.Controllers;

namespace Prototipo_BackEnd.Controllers
{
    [Route("Feed")]
    public class FeedController : Controller
    {

        public bool Dark = true;
        
        

        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";
        
        Publicacao publicacaoModel = new Publicacao();
        Comentario comentario = new Comentario();
        Usuario usuario = new Usuario();
        LoginController loginController = new LoginController();

        [Route("Listar")]
        public IActionResult Index(IFormCollection form)
        {
            
            ViewBag.Publicacoes = publicacaoModel.ReadAll();
            ViewBag.Comentario = comentario.ReadAll();
            ViewBag.Usuarios = usuario.MostrarUsuario();
                    
            ViewBag.IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");
            ViewBag.NascimentoLogado = HttpContext.Session.GetString("_NascimentoLogado");
            ViewBag.NomeLogado = HttpContext.Session.GetString("_NomeLogado");
            ViewBag.EmailLogado = HttpContext.Session.GetString("_EmailLogado");
            ViewBag.UserNomeLogado = HttpContext.Session.GetString("_UserNameLogado");
            ViewBag.SenhaLogado = HttpContext.Session.GetString("_SenhaLogado");

            ViewBag._IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");           

            return View();
        }

        [Route("Publicar")]
        public IActionResult Publicar(IFormCollection form)
        {
            Publicacao novaPublicacao = new Publicacao();

            novaPublicacao.IdPublicacao = publicacaoModel.IdGenerator();
            novaPublicacao.IdUsuario = int.Parse(HttpContext.Session.GetString("_IdUsuarioLogado"));
            novaPublicacao.NomeUsuario = HttpContext.Session.GetString("_UserNameLogado");
            novaPublicacao.ImagemUsuario = HttpContext.Session.GetString("_FotoLogado");
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
            ViewBag.IdPublicacao = int.Parse(form["IdPublicacao"]);
            novoComentario.IdPublicacao = ViewBag.IdPublicacao;

            novoComentario.UserName = HttpContext.Session.GetString("_UserNameLogado");
            
            comentario.CriarComentario(novoComentario);
            return LocalRedirect("~/Feed/Listar");
        }

        [Route("Curtir")]
        public IActionResult Curtir(IFormCollection form){
            int idPub = int.Parse(form["IdPublicacao2"]);
            int idUser = int.Parse(HttpContext.Session.GetString("_IdUsuarioLogado"));
            
            List<string> likes = usuario.ReadAllLinesCSV(PATH_LIKES);

            var like = likes.Find(x=> x == $"{idPub};{idUser}");

            if(like != null){
                publicacaoModel.CancelLike(idPub, idUser);
                
            }else{
                publicacaoModel.Like(idPub, idUser);
                ViewBag.TotalLikes = publicacaoModel.TotalLikes(idPub);
                Console.WriteLine($"2 - {ViewBag.TotalLikes}");
            }
            
            return LocalRedirect("~/Feed/Listar");
        }

        // Determinar a rota
        [Route("Seguir")]
        // Seguir um usuario
        public IActionResult Seguir(int id){
            // ViewBag que contem o ID do usuario logado
            ViewBag.IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            // Conversão de string para int
            int id_UsuarioLogado = int.Parse(ViewBag.IdUsuarioLogado);
            // Todas as linhas do CSV
            List<string> seguindo = usuario.ReadAllLinesCSV(PATH_SEGUINDO);
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
            return LocalRedirect("~/Feed/Listar");
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
            return LocalRedirect("~/Feed/Listar");
        }
    }
}