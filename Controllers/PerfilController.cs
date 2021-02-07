using System;
using System.Collections.Generic;
using System.IO;
using back_end_totoal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prototipo_BackEnd.Controllers;
using Prototipo_BackEnd.Models;

namespace back_end_totoal.Controllers
{
    [Route("Perfil")]
    public class PerfilController : Controller 
    {
        // Atributos da classe
        private const string PATH = "Database/usuarios.csv";
        private const string PATH_PUBLICACOES = "Database/publicacao.csv";
        private const string PATH_COMENTARIOS = "Database/comentarios.csv";
        private const string PATH_LIKES = "Database/likes.csv";
        private const string PATH_SEGUINDO = "Database/seguindo.csv";
        
        Usuario usuario = new Usuario();
        Publicacao publicacaoModel = new Publicacao();
        Comentario comentario = new Comentario();



        public IActionResult Index(){
            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");

            List<string> usuarios = usuario.ReadAllLinesCSV(PATH);

            string IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            ViewBag.Publicacoes = publicacaoModel.ReadAll(int.Parse(IdUsuarioLogado));

            ViewBag._IdUsuarioLogado = HttpContext.Session.GetString("_IdUsuarioLogado");
            bool redirecionamentoLogado = false;

            if(ViewBag._IdUsuarioLogado != null){
                redirecionamentoLogado = true;
            }

            if(redirecionamentoLogado == false){
                return LocalRedirect("~/Login");
            }

            var logado =
            usuarios.Find(
                x =>
                x.Split(";")[0] == IdUsuarioLogado
            );
            
            string followBase = usuario.SeguidoresAndSeguindo(int.Parse(IdUsuarioLogado));

            string seguindo = followBase.Split(";")[0];
            string seguidores = followBase.Split(";")[1];

            HttpContext.Session.SetString("_NomeLogado", logado.Split(";")[1]);
            HttpContext.Session.SetString("_FotoLogado", logado.Split(";")[2]);
            HttpContext.Session.SetString("_SeguindoLogado", seguindo);
            HttpContext.Session.SetString("_SeguidoresLogado", seguidores);
            HttpContext.Session.SetString("_EmailLogado", logado.Split(";")[5]);
            HttpContext.Session.SetString("_UserNameLogado", logado.Split(";")[6]);
            HttpContext.Session.SetString("_SenhaLogado", logado.Split(";")[7]);

            string totalPubl = publicacaoModel.TotalPublicacoes(int.Parse(IdUsuarioLogado));

            HttpContext.Session.SetString("_TotalPublicacoes", totalPubl);
            
            ViewBag.TotalPublicacoes = HttpContext.Session.GetString("_TotalPublicacoes");

            ViewBag.NomeLogado       = HttpContext.Session.GetString("_NomeLogado");
            ViewBag.SeguindoLogado   = HttpContext.Session.GetString("_SeguindoLogado");
            ViewBag.SeguidoresLogado = HttpContext.Session.GetString("_SeguidoresLogado");
            ViewBag.UserNameLogado   = HttpContext.Session.GetString("_UserNameLogado");
            ViewBag.FotoLogado = HttpContext.Session.GetString("_FotoLogado");
            return View();
        }
    }
}