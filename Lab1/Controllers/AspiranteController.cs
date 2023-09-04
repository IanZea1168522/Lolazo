using Microsoft.AspNetCore.Mvc;
using Lab1.Models;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Lab1.Controllers
{
    public class AspiranteController : Controller
    {
        public static AVL arbol = new AVL();
        public static List<Aspirante> listaAspi = new List<Aspirante>();
        public IActionResult Index()
        {
            return View("Index");
        }
        [Route("carga")]
        public IActionResult SubirDatos(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                ViewBag.Error = "Seleccione un archivo CSV válido.";
                return View("SubirDatos", listaAspi);
            }

            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                var tempList = new List<Aspirante>(); // Crear una lista temporal para almacenar objetos
                if (listaAspi.Count() > 0)
                {
                    listaAspi.Clear();
                }
                if(arbol.raiz != null)
                {
                    arbol.raiz = null;
                }

                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();
                    var partes = linea.Split(';');

                    if (partes.Length != 2)
                    {
                        continue;
                    }

                    var instruccion = partes[0].Trim();
                    var json = partes[1].Trim();

                    try
                    {
                        JObject jsonData = JObject.Parse(json);
                        Aspirante aspirante = new Aspirante
                        {
                            nombre = (string)jsonData["name"],
                            dpi = (string)jsonData["dpi"],
                            nacimiento = (string)jsonData["datebirth"],
                            direccion = (string)jsonData["address"],
                        };

                        switch (instruccion.ToUpper())
                        {
                            case "INSERT":
                                arbol.Insertar(aspirante); 
                                break;

                            case "DELETE":
                                arbol.BuscaElimina(aspirante);
                                break;

                            case "PATCH":
                                arbol.actual(aspirante);
                                break;

                            default:
                                return View("Error");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        return View("Error");
                    }
                }
                listaAspi = arbol.listaOrdenada();
            }

            return RedirectToAction("SubirDatos");
        }
        [HttpPost]
        [Route("buscarNom")]
        public IActionResult encontrado(string nombre)
        {
            if(nombre == null)
            {
                return View("ErrorBuscar");
            }
            List<Aspirante> aspirantesEncontrados = new List<Aspirante>();
            aspirantesEncontrados = arbol.busqueda(nombre);
            if(aspirantesEncontrados == null)
            {
                return View("ErrorBuscar");
            }
            return View("Encontrado" , aspirantesEncontrados);
        }
    }
}
