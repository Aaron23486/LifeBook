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
    public class AttacksController : Controller
    {
        private readonly DataContext _context;

        public AttacksController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? soId)
        {
            ViewBag.SoId = soId; // Pasar SoId a la vista

            if (soId == null)
            {
                // Si no se pasa SoId, mostrar todos los ataques
                var dataContext = _context.Atacks.Include(a => a.So);
                return View(await dataContext.ToListAsync());
            }
            else
            {
                // Filtrar ataques por el SoId
                var filteredAttacks = _context.Atacks
                    .Where(a => a.SoId == soId)
                    .Include(a => a.So);
                return View(await filteredAttacks.ToListAsync());
            }
        }



        // GET: Attacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Atacks
                .Include(a => a.So)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attack == null)
            {
                return NotFound();
            }

            return View(attack);
        }

        // GET: Attacks/Create
        public IActionResult Create(int soId)
        {
            ViewData["SoId"] = soId; // Pasar el SoId a la vista para asociar el ataque
            return View();
        }

        // POST: Attacks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,VideoUrl")] Attack attack, int soId)
        {
            // Asigna el SoId al ataque
            attack.SoId = soId;

            if (ModelState.IsValid)
            {
                _context.Add(attack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { soId = soId });
            }
            ViewData["SoId"] = soId;
            return View(attack);
        }






        // GET: Attacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Atacks.FindAsync(id);
            if (attack == null)
            {
                return NotFound();
            }
            ViewData["SoId"] = new SelectList(_context.Sos, "Id", "Id", attack.SoId);
            return View(attack);
        }

        // POST: Attacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,VideoUrl,SoId")] Attack attack)
        {
            if (id != attack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttackExists(attack.Id))
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
            ViewData["SoId"] = new SelectList(_context.Sos, "Id", "Id", attack.SoId);
            return View(attack);
        }

        // GET: Attacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Atacks
                .Include(a => a.So)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attack == null)
            {
                return NotFound();
            }

            return View(attack);
        }

        // POST: Attacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attack = await _context.Atacks.FindAsync(id);
            if (attack != null)
            {
                _context.Atacks.Remove(attack);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttackExists(int id)
        {
            return _context.Atacks.Any(e => e.Id == id);
        }
    }
}
