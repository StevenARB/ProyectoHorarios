using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SC_701_ProyectoG4_Horarios.DAL;
using SC_701_ProyectoG4_Horarios.Models;

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
        [Authorize(Roles = "Profesor, Admin")]
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
        [Authorize(Roles = "Profesor, Admin")]
        public IActionResult Create()
        {
            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre");
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion");
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento");
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["UsuarioModificacionId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Reservaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Profesor, Admin")]
        //public async Task<IActionResult> Create([Bind("Id,AulaId,ProfesorId,ClaseId,Fecha,HoraInicio,HoraFin")] Reservacion reservacion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Verificar si ya existe una reserva para la misma aula en el mismo horario
        //        var solapamientos = await _context.Reservaciones
        //            .Where(r => r.AulaId == reservacion.AulaId && r.Fecha == reservacion.Fecha)
        //            .AnyAsync(r =>
        //                (reservacion.HoraInicio < r.HoraFin && reservacion.HoraFin > r.HoraInicio)
        //            );

        //        if (solapamientos)
        //        {
        //            ModelState.AddModelError("", "Ya existe una reserva para esta aula en el mismo intervalo de tiempo.");
        //            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
        //            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
        //            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
        //            return View(reservacion);
        //        }

        //        reservacion.UsuarioCreacionId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Asignar ID del usuario logueado
        //        reservacion.FechaCreacion = DateTime.Now; // Asignar la fecha actual

        //        // Asignar los valores de UsuarioModificacionId y FechaModificacion iniciales
        //        reservacion.UsuarioModificacionId = reservacion.UsuarioCreacionId;
        //        reservacion.FechaModificacion = reservacion.FechaCreacion;

        //        _context.Add(reservacion);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
        //    ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
        //    ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
        //    return View(reservacion);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Profesor, Admin")]
        public async Task<IActionResult> Create([Bind("Id,AulaId,ProfesorId,ClaseId,Fecha,HoraInicio,HoraFin")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                // Verificar que la fecha y hora de la reserva no sean anteriores a la fecha y hora actual
                var fechaHoraActual = DateTime.Now;
                var fechaHoraReserva = new DateTime(
                    reservacion.Fecha.Year,
                    reservacion.Fecha.Month,
                    reservacion.Fecha.Day,
                    reservacion.HoraInicio.Hour,
                    reservacion.HoraInicio.Minute,
                    0
                );

                if (fechaHoraReserva < fechaHoraActual)
                {
                    ModelState.AddModelError("", "La fecha y hora de la reserva no pueden ser anteriores a la fecha y hora actual.");
                    ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
                    ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
                    ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
                    return View(reservacion);
                }

                // Verificar si ya existe una reserva para la misma aula en el mismo horario
                var solapamientos = await _context.Reservaciones
                    .Where(r => r.AulaId == reservacion.AulaId && r.Fecha == reservacion.Fecha)
                    .AnyAsync(r =>
                        (reservacion.HoraInicio < r.HoraFin && reservacion.HoraFin > r.HoraInicio)
                    );

                if (solapamientos)
                {
                    ModelState.AddModelError("", "Ya existe una reserva para esta aula en el mismo intervalo de tiempo.");
                    ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
                    ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
                    ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
                    return View(reservacion);
                }

                reservacion.UsuarioCreacionId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Asignar ID del usuario logueado
                reservacion.FechaCreacion = DateTime.Now; // Asignar la fecha actual

                // Asignar los valores de UsuarioModificacionId y FechaModificacion iniciales
                reservacion.UsuarioModificacionId = reservacion.UsuarioCreacionId;
                reservacion.FechaModificacion = reservacion.FechaCreacion;

                _context.Add(reservacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
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
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
            ViewData["UsuarioCreacionId"] = new SelectList(_context.Usuarios, "Id", "Nombre", reservacion.UsuarioCreacionId);
            ViewData["UsuarioModificacionId"] = new SelectList(_context.Usuarios, "Id", "Nombre", reservacion.UsuarioModificacionId);
            return View(reservacion);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,AulaId,ProfesorId,ClaseId,Fecha,HoraInicio,HoraFin")] Reservacion reservacion)
        //{
        //    if (id != reservacion.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Cargar la reservación existente desde la base de datos
        //            var existingReservacion = await _context.Reservaciones.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        //            if (existingReservacion == null)
        //            {
        //                return NotFound();
        //            }

        //            // Mantener los valores originales de UsuarioCreacionId y FechaCreacion
        //            reservacion.UsuarioCreacionId = existingReservacion.UsuarioCreacionId;
        //            reservacion.FechaCreacion = existingReservacion.FechaCreacion;

        //            // Verificar si ya existe una reserva para la misma aula en el mismo horario (excluyendo la reserva actual)
        //            var solapamientos = await _context.Reservaciones
        //                .Where(r => r.AulaId == reservacion.AulaId && r.Fecha == reservacion.Fecha && r.Id != reservacion.Id)
        //                .AnyAsync(r =>
        //                    (reservacion.HoraInicio < r.HoraFin && reservacion.HoraFin > r.HoraInicio)
        //                );

        //            if (solapamientos)
        //            {
        //                ModelState.AddModelError("", "Ya existe una reserva para esta aula en el mismo intervalo de tiempo.");
        //                ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
        //                ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
        //                ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
        //                return View(reservacion);
        //            }

        //            // Actualizar UsuarioModificacionId y FechaModificacion
        //            reservacion.UsuarioModificacionId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Asignar ID del usuario logueado
        //            reservacion.FechaModificacion = DateTime.Now; // Asignar la fecha actual

        //            // Actualizar la entidad en el contexto
        //            _context.Update(reservacion);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReservacionExists(reservacion.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
        //    ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
        //    ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
        //    return View(reservacion);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AulaId,ProfesorId,ClaseId,Fecha,HoraInicio,HoraFin")] Reservacion reservacion)
        {
            if (id != reservacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cargar la reservación existente desde la base de datos
                    var existingReservacion = await _context.Reservaciones.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                    if (existingReservacion == null)
                    {
                        return NotFound();
                    }

                    // Mantener los valores originales de UsuarioCreacionId y FechaCreacion
                    reservacion.UsuarioCreacionId = existingReservacion.UsuarioCreacionId;
                    reservacion.FechaCreacion = existingReservacion.FechaCreacion;

                    // Verificar que la fecha y hora de la reserva no sean anteriores a la fecha y hora actual
                    var fechaHoraActual = DateTime.Now;
                    var fechaHoraReserva = new DateTime(
                        reservacion.Fecha.Year,
                        reservacion.Fecha.Month,
                        reservacion.Fecha.Day,
                        reservacion.HoraInicio.Hour,
                        reservacion.HoraInicio.Minute,
                        0
                    );

                    if (fechaHoraReserva < fechaHoraActual)
                    {
                        ModelState.AddModelError("", "La fecha y hora de la reserva no pueden ser anteriores a la fecha y hora actual.");
                        ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
                        ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
                        ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
                        return View(reservacion);
                    }

                    // Verificar si ya existe una reserva para la misma aula en el mismo horario (excluyendo la reserva actual)
                    var solapamientos = await _context.Reservaciones
                        .Where(r => r.AulaId == reservacion.AulaId && r.Fecha == reservacion.Fecha && r.Id != reservacion.Id)
                        .AnyAsync(r =>
                            (reservacion.HoraInicio < r.HoraFin && reservacion.HoraFin > r.HoraInicio)
                        );

                    if (solapamientos)
                    {
                        ModelState.AddModelError("", "Ya existe una reserva para esta aula en el mismo intervalo de tiempo.");
                        ViewData["AulaId"] = new SelectList(_context.Aulas, "Id", "Nombre", reservacion.AulaId);
                        ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
                        ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
                        return View(reservacion);
                    }

                    // Actualizar UsuarioModificacionId y FechaModificacion
                    reservacion.UsuarioModificacionId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Asignar ID del usuario logueado
                    reservacion.FechaModificacion = DateTime.Now; // Asignar la fecha actual

                    // Actualizar la entidad en el contexto
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
            ViewData["ClaseId"] = new SelectList(_context.Clases, "Id", "Descripcion", reservacion.ClaseId);
            ViewData["ProfesorId"] = new SelectList(_context.Profesores, "Id", "Departamento", reservacion.ProfesorId);
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

        public async Task<IActionResult> Historial()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            // Obtener las reservaciones pasadas con información del usuario que creó y modificó
            var reservacionesPasadas = await _context.Reservaciones
                .Where(r => r.Fecha < hoy)
                .Include(r => r.Aula)
                .Include(r => r.Clase)
                .Include(r => r.Profesor)
                .Include(r => r.UsuarioCreacion)       // Incluir usuario de creación
                .Include(r => r.UsuarioModificacion)   // Incluir usuario de modificación
                .ToListAsync();

            // Obtener las reservaciones futuras con información del usuario que creó y modificó
            var reservacionesFuturas = await _context.Reservaciones
                .Where(r => r.Fecha >= hoy)
                .Include(r => r.Aula)
                .Include(r => r.Clase)
                .Include(r => r.Profesor)
                .Include(r => r.UsuarioCreacion)       // Incluir usuario de creación
                .Include(r => r.UsuarioModificacion)   // Incluir usuario de modificación
                .ToListAsync();

            var viewModel = new HistorialViewModel
            {
                ReservacionesPasadas = reservacionesPasadas,
                ReservacionesFuturas = reservacionesFuturas
            };

            return View(viewModel);
        }



    }
}
