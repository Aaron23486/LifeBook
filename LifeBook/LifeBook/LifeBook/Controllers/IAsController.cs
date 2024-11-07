using LifeBook.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeBook.Controllers
{
    public class IAsController : Controller
    {

        private readonly DataContext _context;

        // Inyectar el contexto de datos en el constructor
        public IAsController(DataContext context)
        {
            _context = context;
        }
        // GET: IAController/Index
        public async Task<IActionResult> Index(int categoryId)
        {
            // Obtener todas las IAs que pertenecen a la categoría seleccionada
            var iasInCategory = await _context.IAs
                                               .Where(ia => ia.CategoryId == categoryId)
                                               .Include(ia => ia.IACategory) // Asegurarse de incluir la categoría al cargar las IAs
                                               .ToListAsync();

            // Pasar la lista de IAs a la vista
            return View(iasInCategory);
        }

        // GET: IAsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IAsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IAsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IAsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IAsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IAsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IAsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
