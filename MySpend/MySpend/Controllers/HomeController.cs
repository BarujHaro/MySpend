using Microsoft.AspNetCore.Mvc;
using MySpend.Models.Entities;
using MySpend.Models.ViewModels;
using MySpend.Service;
using System.Diagnostics;


namespace MySpend.Controllers
{
    //páginas generales
    public class HomeController : Controller
    {
        /*
         Es el intermediario entre usuario y logica, recibe peticiones HTTP pide datos y decide que vista mostrar
         */
        //Carga la pagina de inicio
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }


    }
}
