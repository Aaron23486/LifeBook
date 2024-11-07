using LifeBook.Data;
using LifeBook.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeBook.Controllers
{
    public class IACategoriesController : Controller
    {
        private readonly DataContext _context;

        // Inyectar el contexto de datos en el constructor
        public IACategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: IACategoriesController
        public async Task<IActionResult> Index()
        {
            // Obtener todas las categorías de IA con sus IAs asociadas
            var categories = await _context.IACategories
                                            .Include(c => c.IAs)  // Incluir la colección de IAs
                                            .ToListAsync();

            // Pasar la lista de categorías a la vista
            return View(categories);
        }

        // GET: IACategoriesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Buscar la categoría por ID
            var iACategory = await _context.IACategories
                                            .Include(c => c.IAs) // Incluir IAs asociadas
                                            .FirstOrDefaultAsync(c => c.Id == id);
            if (iACategory == null)
            {
                return NotFound(); // Si no se encuentra, retornar un error 404
            }

            return View(iACategory);
        }

        // GET: IACategoriesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IACategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IACategory iACategory)
        {
            if (ModelState.IsValid)
            {
                _context.IACategories.Add(iACategory);
                await _context.SaveChangesAsync(); // Usar SaveChangesAsync para asincronía
                return RedirectToAction(nameof(Index));
            }
            return View(iACategory);
        }

        // GET: IACategoriesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var iACategory = await _context.IACategories.FindAsync(id);
            if (iACategory == null)
            {
                return NotFound(); // Si no se encuentra, retornar un error 404
            }

            return View(iACategory);
        }

        // POST: IACategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IACategory iACategory)
        {
            if (id != iACategory.Id)
            {
                return NotFound(); // Verificar que el ID coincide
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iACategory);
                    await _context.SaveChangesAsync(); // Usar SaveChangesAsync para asincronía
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IACategoryExists(iACategory.Id))
                    {
                        return NotFound(); // Si no existe la categoría, retornar un error 404
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(iACategory);
        }

        // GET: IACategoriesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var iACategory = await _context.IACategories
                                            .FirstOrDefaultAsync(c => c.Id == id);
            if (iACategory == null)
            {
                return NotFound(); // Si no se encuentra, retornar un error 404
            }

            return View(iACategory);
        }

        // POST: IACategoriesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iACategory = await _context.IACategories.FindAsync(id);
            if (iACategory != null)
            {
                _context.IACategories.Remove(iACategory);
                await _context.SaveChangesAsync(); // Usar SaveChangesAsync para asincronía
            }
            return RedirectToAction(nameof(Index));
        }

        private bool IACategoryExists(int id)
        {
            return _context.IACategories.Any(e => e.Id == id);
        }
    }
}
