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
    public class ToolsController : Controller
    {
        private readonly DataContext _context;

        public ToolsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? attackId)
        {
            // Guardar attackId y soId en ViewBag para usarlos en la vista
            var attack = await _context.Attacks.Include(a => a.So).FirstOrDefaultAsync(a => a.Id == attackId);

            if (attack == null)
            {
                return NotFound();
            }

            ViewBag.CurrentAttackId = attackId; // Guardar el ID del ataque
            ViewBag.SoId = attack.SoId; // Guardar el ID del sistema operativo

            // Filtrar herramientas por el attackId
            var filteredTools = _context.Tools
                .Where(t => t.AttackId == attackId)
                .Include(t => t.Attack)
                .Include(t => t.So);

            return View(await filteredTools.ToListAsync());
        }




        // GET: Tools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools
                .Include(t => t.Attack)
                .Include(t => t.So)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // GET: Tools/Create
        public IActionResult Create(int attackId)
        {
            var attack = _context.Attacks.Include(a => a.So).FirstOrDefault(a => a.Id == attackId);
            if (attack == null)
            {
                return NotFound();
            }

            ViewData["AttackId"] = attackId; // Pasar el attackId a la vista
            ViewData["SoId"] = attack.SoId; // Pasar el SoId del ataque asociado
            ViewBag.AttackName = attack.Name; // Pasar el nombre del ataque para mostrarlo en la vista
            ViewBag.SoName = attack.So.Name;  // Pasar el nombre del SO para mostrarlo en la vista

            return View();
        }


        // POST: Tools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Tool tool, int attackId)
        {
            var attack = await _context.Attacks.Include(a => a.So).FirstOrDefaultAsync(a => a.Id == attackId);
            if (attack == null)
            {
                return NotFound();
            }

            tool.AttackId = attackId;
            tool.SoId = attack.SoId;

            if (ModelState.IsValid)
            {
                _context.Add(tool);
                await _context.SaveChangesAsync();
                // Redirigir a la vista de herramientas del ataque creado
                return RedirectToAction("Index", new { attackId = attackId });
            }

            ViewData["AttackId"] = attackId;
            ViewData["SoId"] = attack.SoId;
            return View(tool);
        }






        // GET: Tools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools
                .Include(t => t.Attack)   // Incluimos la relación con Attack
                .Include(t => t.So)       // Incluimos la relación con So
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Tool tool)
        {
            if (id != tool.Id)
            {
                return NotFound();
            }

            var existingTool = await _context.Tools
                .AsNoTracking()
                .Include(t => t.Attack)
                .Include(t => t.So)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTool == null)
            {
                return NotFound();
            }

            // Mantener el SoId y AttackId originales
            tool.SoId = existingTool.SoId;
            tool.AttackId = existingTool.AttackId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolExists(tool.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirigir a la lista de herramientas del ataque
                return RedirectToAction(nameof(Index), new { attackId = tool.AttackId });
            }

            return View(tool);
        }



        // GET: Tools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools
                .Include(t => t.Attack)
                .Include(t => t.So)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool != null)
            {
                _context.Tools.Remove(tool);
                await _context.SaveChangesAsync();
            }

            // Redirigir a la lista de herramientas del ataque
            return RedirectToAction(nameof(Index), new { attackId = tool.AttackId });
        }


        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
