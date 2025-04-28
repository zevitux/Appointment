using AgendamentoApi.Data;
using AgendamentoApi.Models;
using AgendamentoApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoApi.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<IAppointmentRepository> _logger;

    public AppointmentRepository(AppDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId)
    {
        try
        {
            return await _context.Appointments.FirstOrDefaultAsync(y => y.Id == appointmentId);
        }
        catch (Exception)
        {
            _logger.LogError("Appointment with id {appointmentId} not found", appointmentId);
            throw;
        }
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync()
    {
        try
        {
            return await _context.Appointments.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Appointments not found");
            throw;
        }
    }

    public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(Guid userId)
    {
        try
        {
            return await _context.Appointments
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Appointments not found");
            throw;
        }
    }

    public async Task<List<Appointment>> GetAppointmentsByOwnershipIdAsync(Guid ownershipId)
    {
        try
        {
            return await _context.Appointments
                .Where(x => x.OwnershipId == ownershipId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Appointments not found");
            throw;
        }
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        try
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
        catch (Exception)
        {
            _logger.LogError("Error creating appointment");
            throw;
        }
    }

    public async Task<Appointment> UpdateAppointmentAsync(Guid appointmentId)
    {
        try
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointmentId);
            if (existingAppointment == null)
                throw new KeyNotFoundException($"Appointment with Id: {appointmentId} doesn't exist");

            _context.Entry(existingAppointment).CurrentValues.SetValues(appointmentId);
            await _context.SaveChangesAsync();
            return existingAppointment;
        }
        catch (Exception)
        {
            _logger.LogError("Error updating appointment");
            throw;
        }
    }

    public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
    {
        try
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                _logger.LogError("Appointment with Id: {appointmentId} doesn't exist", appointmentId);
                return false;
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            _logger.LogError("Error deleting appointment with ID {AppointmentId}", appointmentId);
            throw;
        }
    }
}