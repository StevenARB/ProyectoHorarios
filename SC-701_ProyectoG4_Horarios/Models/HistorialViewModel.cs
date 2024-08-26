using SC_701_ProyectoG4_Horarios.DAL;

namespace SC_701_ProyectoG4_Horarios.Models
{
    public class HistorialViewModel
    {
        public List<Reservacion> ReservacionesPasadas { get; set; }
        public List<Reservacion> ReservacionesFuturas { get; set; }
    }
}
