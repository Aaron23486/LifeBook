﻿using System;
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

        // GET: Tools
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Tools.Include(t => t.Attack).Include(t => t.So);
            return View(await dataContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["AttackId"] = new SelectList(_context.Atacks, "Id", "Id");
            ViewData["SoId"] = new SelectList(_context.Sos, "Id", "Id");
            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SoId,AttackId")] Tool tool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttackId"] = new SelectList(_context.Atacks, "Id", "Id", tool.AttackId);
            ViewData["SoId"] = new SelectList(_context.Sos, "Id", "Id", tool.SoId);
            return View(tool);
        }

        // GET: Tools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            ViewData["AttackId"] = new SelectList(_context.Atacks, "Id", "Id", tool.AttackId);
            ViewData["SoId"] = new SelectList(_context.Sos, "Id", "Id", tool.SoId);
            return View(tool);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SoId,AttackId")] Tool tool)
        {
            if (id != tool.Id)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttackId"] = new SelectList(_context.Atacks, "Id", "Id", tool.AttackId);
            ViewData["SoId"] = new SelectList(_context.Sos, "Id", "Id", tool.SoId);
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

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool != null)
            {
                _context.Tools.Remove(tool);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
