using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LifeBook.Data;
using LifeBook.Data.Entities;

namespace LifeBook.Controllers
{
    public class ProtocolsController : Controller
    {
        private readonly DataContext _context;

        public ProtocolsController(DataContext context)
        {
            _context = context;
        }

        // GET: Protocols
        public async Task<IActionResult> Index(string portFilter)
        {
            var protocols = from p in _context.Protocols
                            select p;

            if (!String.IsNullOrEmpty(portFilter))
            {
                // Split the input by commas y espacios, si es necesario
                var ports = portFilter.Split(',')
                                      .Select(p => p.Trim())
                                      .Where(p => !string.IsNullOrEmpty(p))
                                      .ToList();

                // Filtrar protocolos que contengan cualquiera de los puertos
                protocols = protocols.Where(p => ports.Any(port => p.Port.Contains(port)));
            }

            return View(await protocols.ToListAsync());
        }



        // GET: Protocols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var protocol = await _context.Protocols
                .FirstOrDefaultAsync(m => m.Id == id);
            if (protocol == null)
            {
                return NotFound();
            }

            return View(protocol);
        }

        // GET: Protocols/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Port")] Protocol protocol)
        {
            // Validar el formato de los puertos
            if (!string.IsNullOrWhiteSpace(protocol.Port) && !protocol.Port.Split(',').All(p => int.TryParse(p.Trim(), out _)))
            {
                ModelState.AddModelError("Port", "Introduce un valor válido para los puertos (números separados por comas).");
            }

            if (ModelState.IsValid)
            {
                _context.Add(protocol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(protocol);
        }


        // GET: Protocols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var protocol = await _context.Protocols.FindAsync(id);
            if (protocol == null)
            {
                return NotFound();
            }
            return View(protocol);
        }

        // POST: Protocols/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Port")] Protocol protocol)
        {
            if (id != protocol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(protocol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProtocolExists(protocol.Id))
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
            return View(protocol);
        }

        // GET: Protocols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var protocol = await _context.Protocols
                .FirstOrDefaultAsync(m => m.Id == id);
            if (protocol == null)
            {
                return NotFound();
            }

            return View(protocol);
        }

        // POST: Protocols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var protocol = await _context.Protocols.FindAsync(id);
            if (protocol != null)
            {
                _context.Protocols.Remove(protocol);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProtocolExists(int id)
        {
            return _context.Protocols.Any(e => e.Id == id);
        }
    }
}
