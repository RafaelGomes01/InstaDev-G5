using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstaDev.Controllers
{
    public class ValidacaoController : Controller
    {   

        [TempData]
        public string Mensagem { get; set; }

        public IActionResult Index(){
            return View();
        }

        [Route("Validar")]
        public IActionResult Validar(IFormCollection form){
            int codEnviado = int.Parse(HttpContext.Session.GetString("CodeUsuarioCadastrando"));

            int codRecebido = int.Parse(form["code"]);

            if(codRecebido == codEnviado){
                return LocalRedirect("~/Login");

            }else{
                Mensagem = "Codigo errado, tente novamente";
                return LocalRedirect("~/Validacao");

            }
        }
    }
}