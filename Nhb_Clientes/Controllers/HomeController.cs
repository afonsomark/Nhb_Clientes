using Microsoft.AspNetCore.Mvc;
using Nhb_Clientes.Models;
using System.Diagnostics;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System.Web;
using ISession = NHibernate.ISession;

namespace Nhb_Clientes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISession _session;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            using (ISession session = Nhb_Helper.OpenSession())
            {
                var clientes = session.Query<Clientes>().ToList();
                return View(clientes);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Clientes());
        }

        [HttpPost]
        public ActionResult Create(Clientes cliente)
        {
            try
            {
                using (ISession session = Nhb_Helper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(cliente);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (ISession session = Nhb_Helper.OpenSession())
            {
                var clientes = session.Get<Clientes>(id);
                return View(clientes);
            }
        }

        // GET: Home/Edit/{Id}
        public ActionResult Edit(int id)
        {
            using (ISession session = Nhb_Helper.OpenSession())
            {
                var cliente = session.Get<Clientes>(id);
                return View(cliente);
            }
        }

        // POST: Home/Edit/{Id}
        [HttpPost]
        public ActionResult Edit(int id, Clientes cliente)
        {
            try
            {
                using (ISession session = Nhb_Helper.OpenSession())
                {
                    var clienteAlterado = session.Get<Clientes>(id);
                    clienteAlterado.Nome = cliente.Nome;
                    clienteAlterado.Sexo = cliente.Sexo;
                    clienteAlterado.Endereco = cliente.Endereco;
                    clienteAlterado.Telefone1 = cliente.Telefone1;
                    clienteAlterado.Telefone2 = cliente.Telefone2;
                    clienteAlterado.Telefone3 = cliente.Telefone3;
                    clienteAlterado.FoneValido = cliente.FoneValido;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(clienteAlterado);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/{Id}
        public ActionResult Delete(int id)
        {
            using (ISession session = Nhb_Helper.OpenSession())
            {
                var cliente = session.Get<Clientes>(id);
                return View(cliente);
            }
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Clientes cliente)
        {
            try
            {
                using (ISession session = Nhb_Helper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(cliente);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
