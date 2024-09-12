using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LifeBook.Data;
using LifeBook.Data.Entities;

namespace LifeBook.Controllers
{
    public class SosController : Controller
    {
        private readonly DataContext _context;

        public SosController(DataContext context)
        {
            _context = context;
        }

        // GET: Sos
        public async Task<IActionResult> Index()
        {
            var sos = await _context.Sos
                .Include(s => s.Attacks) // Incluye la propiedad de navegación Attacks
                .ToListAsync();

            return View(sos);
        }


        // GET: Sos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var so = await _context.Sos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (so == null)
            {
                return NotFound();
            }

            return View(so);
        }

        // GET: Sos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] So so)
        {
            if (ModelState.IsValid)
            {
                _context.Add(so);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(so);
        }





        // GET: Sos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var so = await _context.Sos.FindAsync(id);
            if (so == null)
            {
                return NotFound();
            }
            return View(so);
        }

        // POST: Sos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] So so)
        {
            if (id != so.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(so);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoExists(so.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(so);
        }


        // GET: Sos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var so = await _context.Sos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (so == null)
            {
                return NotFound();
            }

            return View(so);
        }

        // POST: Sos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var so = await _context.Sos
                .Include(s => s.Attacks) // Incluye los ataques relacionados
                .ThenInclude(a => a.Tools) // Incluye las herramientas relacionadas con los ataques
                .FirstOrDefaultAsync(s => s.Id == id);

            if (so != null)
            {
                // Eliminar todas las herramientas asociadas a cada ataque del So
                foreach (var attack in so.Attacks)
                {
                    _context.Tools.RemoveRange(attack.Tools);
                }

                // Eliminar todos los ataques asociados al So
                _context.Attacks.RemoveRange(so.Attacks);

                // Eliminar el So
                _context.Sos.Remove(so);

                // Guardar cambios
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool SoExists(int id)
        {
            return _context.Sos.Any(e => e.Id == id);
        }
    }
}
