using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC_701_ProyectoG4_Horarios.DAL;

namespace SC_701_ProyectoG4_Horarios.Controllers
{
    public class ReservacionesController : Controller
    {
        private readonly HorariosContext _context;

        public ReservacionesController(HorariosContext context)
        {
            _context = context;
        }

        // GET: Reservaciones
        public async Task<IActionResult> Index()
        {
            var horariosContext = _context.Reservaciones.Include(r => r.Aula).Include(r => r.Clase).Include(r => r.Profesor).Include(r => r.UsuarioCreacion).Include(r => r.UsuarioModificacion);
            return View(await horariosContext.ToListAsync());
        }

        // GET: Reservaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservacion = await _context.Reservaciones
                .Include(r => r.Aula)
                .Include(r => r.Clase)
                .Include(r => r.Profesor)
                .Include(r => r.UsuarioCreacion)
                .Include(r => r.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }

        // GET: Reservaciones/Create
        public IActionResult Create()
        {
            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre");
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Id");
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Email");
            ViewData["UsuarioModificacionId"] = new SelectList(_context.Usuarios, "Id", "Email");
            return View();
        }

        // POST: Reservaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AulaId,ProfesorId,ClaseId,Fecha,HoraInicio,HoraFin,UsuarioCreacionId,FechaCreacion,UsuarioModificacionId,FechaModificacion")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Id", reservacion.ClaseId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Email", reservacion.UsuarioCreacionId);
            ViewData["UsuarioModificacionId"] = new SelectList(_context.Usuarios, "Id", "Email", reservacion.UsuarioModificacionId);
            return View(reservacion);
        }

        // GET: Reservaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservacion = await _context.Reservaciones.FindAsync(id);
            if (reservacion == null)
            {
                return NotFound();
            }
            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Id", reservacion.ClaseId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Email", reservacion.UsuarioCreacionId);
            ViewData["UsuarioModificacionId"] = new SelectList(_context.Usuarios, "Id", "Email", reservacion.UsuarioModificacionId);
            return View(reservacion);
        }

        // POST: Reservaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AulaId,ProfesorId,ClaseId,Fecha,HoraInicio,HoraFin,UsuarioCreacionId,FechaCreacion,UsuarioModificacionId,FechaModificacion")] Reservacion reservacion)
        {
            if (id != reservacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservacionExists(reservacion.Id))
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
            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Id", reservacion.ClaseId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Email", reservacion.UsuarioCreacionId);
            ViewData["UsuarioModificacionId"] = new SelectList(_context.Usuarios, "Id", "Email", reservacion.UsuarioModificacionId);
            return View(reservacion);
        }

        // GET: Reservaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservacion = await _context.Reservaciones
                .Include(r => r.Aula)
                .Include(r => r.Clase)
                .Include(r => r.Profesor)
                .Include(r => r.UsuarioCreacion)
                .Include(r => r.UsuarioModificacion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return View(reservacion);
        }

        // POST: Reservaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservacion = await _context.Reservaciones.FindAsync(id);
            if (reservacion != null)
            {
                _context.Reservaciones.Remove(reservacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservacionExists(int id)
        {
            return _context.Reservaciones.Any(e => e.Id == id);
        }
    }
}
