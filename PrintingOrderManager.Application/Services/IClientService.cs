// PrintingOrderManager.Application.Services/IClientService.cs
using PrintingOrderManager.Core.DTOs;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        IQueryable<ClientDto> GetClientsQueryable(); // ← ДОБАВЛЕНО
        Task<ClientDto> GetClientByIdAsync(int id);
        Task AddClientAsync(CreateClientDto clientDto);
        Task UpdateClientAsync(int id, UpdateClientDto clientDto);
        Task DeleteClientAsync(int id);
        Task<ClientDto?> GetClientByNameAsync(string name);
    }
}