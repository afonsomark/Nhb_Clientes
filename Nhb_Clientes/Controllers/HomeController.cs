using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nhb_Clientes.Models;
using Nhb_Clientes.Models.Caching;
using NHibernate;
using NuGet.Protocol;
using System.Diagnostics;
using ISession = NHibernate.ISession;

namespace Nhb_Clientes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISession _session;
        private readonly ICachingService _cache;

        public HomeController(ILogger<HomeController> logger, ICachingService cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Info()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var clienteCache = await _cache.GetAsync("clientes");
            var clientes = new List<Clientes>();

            if (!string.IsNullOrEmpty(clienteCache))
            {
                clientes = JsonConvert.DeserializeObject<List<Clientes>>(clienteCache);
                return View(clientes);
            }
            else
            {
                using (ISession session = Nhb_Helper.OpenSession())
                {
                    List<Clientes> listaClientes = session.Query<Clientes>().ToList();
                    await _cache.SetAsync("clientes", JsonConvert.SerializeObject(listaClientes));
                    return View(listaClientes);
                }
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

                        List<Clientes> listaClientes = session.Query<Clientes>().ToList();
                        _cache.SetAsync("clientes", JsonConvert.SerializeObject(listaClientes));
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            var clienteCache = await _cache.GetAsync(id.ToString());
            Clientes? cliente;
            if (!string.IsNullOrEmpty(clienteCache))
            {
                cliente = JsonConvert.DeserializeObject<Clientes>(clienteCache);
                return View(cliente);
            }
            else
            {
                using (ISession session = Nhb_Helper.OpenSession())
                {
                    cliente = session.Get<Clientes>(id);
                    await _cache.SetAsync(id.ToString(), JsonConvert.SerializeObject(cliente));
                    return View(cliente);
                }
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
                    clienteAlterado.Tel_Ativo = cliente.Tel_Ativo;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(clienteAlterado);
                        transaction.Commit();

                        List<Clientes> listaClientes = session.Query<Clientes>().ToList();
                        _cache.SetAsync("clientes", JsonConvert.SerializeObject(listaClientes));
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

                        List<Clientes> listaClientes = session.Query<Clientes>().ToList();
                        _cache.SetAsync("clientes", JsonConvert.SerializeObject(listaClientes));
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
