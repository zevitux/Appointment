using AgendamentoApi.Models;

namespace AgendamentoApi.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId);
    Task<List<Appointment>> GetAllAppointmentsAsync();
    Task<List<Appointment>> GetAppointmentsByUserIdAsync(Guid userId);
    Task<List<Appointment>> GetAppointmentsByOwnershipIdAsync(Guid ownershipId);
    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    Task<Appointment> UpdateAppointmentAsync(Guid appointmentId);
    Task<bool> DeleteAppointmentAsync(Guid appointmentId);
}