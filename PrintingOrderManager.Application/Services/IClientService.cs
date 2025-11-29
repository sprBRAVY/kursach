// PrintingOrderManager.Application/Services/IClientService.cs
using PrintingOrderManager.Core.DTOs;

namespace PrintingOrderManager.Application.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto> GetClientByIdAsync(int id);
        Task AddClientAsync(CreateClientDto clientDto);
        Task UpdateClientAsync(int id, UpdateClientDto clientDto);
        Task DeleteClientAsync(int id);
        Task<ClientDto?> GetClientByNameAsync(string name);
    }
}