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

            IQueryable<Attack> query = _context.Attacks
                .Include(a => a.So)       // Incluye el sistema operativo
                .Include(a => a.Tools);   // Incluye las herramientas relacionadas

            if (soId != null)
            {
                // Filtrar ataques por el SoId
                query = query.Where(a => a.SoId == soId);
            }

            var attacks = await query.ToListAsync();

            return View(attacks);
        }




        // GET: Attacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Attacks
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

            var attack = await _context.Attacks.Include(a => a.So).FirstOrDefaultAsync(a => a.Id == id);
            if (attack == null)
            {
                return NotFound();
            }

            return View(attack); // Se pasa el SoId como parte del objeto attack
        }

        // POST: Attacks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,VideoUrl")] Attack attack)
        {
            if (id != attack.Id)
            {
                return NotFound();
            }

            var existingAttack = await _context.Attacks.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (existingAttack == null)
            {
                return NotFound();
            }

            attack.SoId = existingAttack.SoId;

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
                return RedirectToAction(nameof(Index), new { soId = attack.SoId });
            }

            return View(attack);
        }

        // Verificar si el ataque existe
        private bool AttackExists(int id)
        {
            return _context.Attacks.Any(e => e.Id == id);
        }


        // GET: Attacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Attacks
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
    var attack = await _context.Attacks.FindAsync(id);
    if (attack != null)
    {
        _context.Attacks.Remove(attack);
        await _context.SaveChangesAsync();
    }
    
    return RedirectToAction(nameof(Index), new { soId = attack.SoId });
}

    }
}
