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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,URL")] Tool tool, IFormFile documentFile, int attackId)
        {
            var attack = await _context.Attacks.Include(a => a.So).FirstOrDefaultAsync(a => a.Id == attackId);
            if (attack == null)
            {
                return NotFound();
            }

            // Asignar manualmente los campos SoId y AttackId
            tool.AttackId = attackId;
            tool.SoId = attack.SoId;

            // Manejar la subida del archivo sin depender de ModelState
            if (documentFile != null && documentFile.Length > 0)
            {
                var fileName = Path.GetFileName(documentFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentFile.CopyToAsync(stream);
                }

                tool.DocumentPath = $"/documents/{fileName}";
            }
            else
            {
                // Si no se sube un documento, asignar un valor vacío o nulo
                tool.DocumentPath = string.Empty; // O null si permites DocumentPath como nullable
            }

            // Guardar el registro en la base de datos sin depender de ModelState
            try
            {
                _context.Add(tool);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Manejar cualquier error de base de datos aquí, si es necesario
                // Por ejemplo, loggear el error o redirigir a una vista de error
                return BadRequest("Error al crear la herramienta: " + ex.Message);
            }

            // Redirigir al índice una vez guardado el registro
            return RedirectToAction(nameof(Index), new { attackId = attackId });
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,URL,DocumentPath")] Tool tool, IFormFile documentFile)
        {
            if (id != tool.Id)
            {
                return NotFound();
            }

            // Obtener la herramienta existente
            var existingTool = await _context.Tools
                .AsNoTracking()
                .Include(t => t.Attack) // Incluimos la relación con Attack
                .Include(t => t.So)     // Incluimos la relación con So
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTool == null)
            {
                return NotFound();
            }

            // Asegúrate de mantener los valores de SoId y AttackId
            tool.SoId = existingTool.SoId;
            tool.AttackId = existingTool.AttackId;

            // Manejo del archivo si es que se carga uno nuevo
            if (documentFile != null && documentFile.Length > 0)
            {
                var fileName = Path.GetFileName(documentFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentFile.CopyToAsync(stream);
                }

                tool.DocumentPath = $"/documents/{fileName}";
            }
            else
            {
                // Mantener el DocumentPath existente si no se carga uno nuevo
                tool.DocumentPath = existingTool.DocumentPath;
            }

            // Validar si el modelo es válido
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
                // Eliminar el archivo del sistema si existe
                if (!string.IsNullOrEmpty(tool.DocumentPath))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", tool.DocumentPath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                _context.Tools.Remove(tool);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { attackId = tool.AttackId });
        }


        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }


        [HttpPost]
        public async Task<IActionResult> UploadDocument(int id, IFormFile documentFile)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }

            if (documentFile != null && documentFile.Length > 0)
            {
                var fileName = Path.GetFileName(documentFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentFile.CopyToAsync(stream);
                }

                tool.DocumentPath = $"/documents/{fileName}";
                _context.Update(tool);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { attackId = tool.AttackId });
        }


        public IActionResult GetDocument(string filePath)
        {
            // Obtiene la ruta física completa del archivo
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

            // Verifica si el archivo existe
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(); // Retorna un error si el archivo no existe
            }

            // Obtiene la extensión del archivo
            var fileExtension = Path.GetExtension(fullPath).ToLowerInvariant();

            // Define el tipo MIME según la extensión del archivo
            var mimeType = fileExtension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                _ => "application/octet-stream" // Valor por defecto si no coincide con ninguna extensión conocida
            };

            // Lee el archivo desde el sistema de archivos
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            // Retorna el archivo con el tipo MIME correspondiente
            return File(fileBytes, mimeType);
        }


    }
}
