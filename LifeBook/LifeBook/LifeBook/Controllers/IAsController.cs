using LifeBook.Data;
using LifeBook.Data.Entities;
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


        // GET: IAsController/Index
        public async Task<IActionResult> Index(int categoryId)
        {
            if (categoryId == 0)
            {
                return RedirectToAction("Index", "Home"); // Redirigir si no se pasa un categoryId válido
            }

            var iasInCategory = await _context.IAs
                                               .Where(ia => ia.CategoryId == categoryId)
                                               .Include(ia => ia.IACategory)
                                               .ToListAsync();

            ViewBag.CategoryId = categoryId;
            return View(iasInCategory);
        }




        // GET: IAsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ia = await _context.IAs
                                    .Include(i => i.IACategory) // Si necesitas cargar la categoría relacionada
                                    .FirstOrDefaultAsync(i => i.Id == id);

            if (ia == null)
            {
                return NotFound();
            }

            return View(ia);
        }


        // GET: IAsController/Create
        [HttpGet]
        public IActionResult Create(int? categoryId)
        {
            // Si categoryId no es nulo, lo asignamos al ViewBag
            ViewBag.CategoryId = categoryId;

            // Obtener las categorías
            var categories = _context.IACategories.ToList();
            ViewBag.IACategories = categories;

            return View();
        }


        // POST: IAsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name, Description, Url")] IA ia, int categoryId)
        {
            if (ModelState.IsValid)
            {
                // Asignar el categoryId a la IA
                ia.CategoryId = categoryId;

                // Guardar la IA en la base de datos
                _context.Add(ia);
                _context.SaveChanges();

                // Redirigir con el categoryId a la lista de IAs
                return RedirectToAction(nameof(Index), new { categoryId = categoryId });
            }

            // Si no es válido, devolver la vista con errores
            return View(ia);
        }








        // GET: IAsController/Edit/5
        public ActionResult Edit(int id)
        {
            var ia = _context.IAs.Find(id); // Aquí debes obtener el objeto IA por su id
            if (ia == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = ia.CategoryId;  // Asigna el categoryId a ViewBag
            return View(ia);
        }


        // POST: IAsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IA ia, int categoryId)
        {
            if (id != ia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualiza la IA en la base de datos
                    _context.Update(ia);
                    _context.SaveChanges(); // Guarda los cambios

                    // Redirige a la página de índice (debe incluir categoryId)
                    return RedirectToAction(nameof(Index), new { categoryId = categoryId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si ocurre una excepción de concurrencia, significa que la IA ya no existe
                    if (!_context.IAs.Any(e => e.Id == ia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si el modelo no es válido, vuelve a la vista de edición con los errores
            return View(ia);
        }



        public IActionResult Delete(int id, int categoryId)
        {
            var iaItem = _context.IAs
                .Include(i => i.IACategory) // Asegúrate de cargar la categoría
                .FirstOrDefault(i => i.Id == id);

            if (iaItem == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = categoryId; // Asegúrate de pasar correctamente el categoryId
            return View(iaItem);
        }




        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id, int categoryId)
        {
            var iaItem = _context.IAs.FirstOrDefault(i => i.Id == id);

            if (iaItem == null)
            {
                return NotFound();
            }

            // Eliminar la IA de la base de datos
            _context.IAs.Remove(iaItem);
            _context.SaveChanges();

            // Redirigir correctamente a la vista Index con el categoryId
            return RedirectToAction(nameof(Index), new { categoryId = categoryId });

        }






    }

}
