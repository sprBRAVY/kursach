// PrintingOrderManager.Application/Services/ClientService.cs
using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

namespace PrintingOrderManager.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        public async Task<ClientDto> GetClientByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) return null;
            return _mapper.Map<ClientDto>(client);
        }

        public async Task AddClientAsync(CreateClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(int id, UpdateClientDto clientDto)
        {
            var existingClient = await _clientRepository.GetByIdAsync(id);
            if (existingClient == null)
            {
                throw new KeyNotFoundException($"Client with ID {id} not found.");
            }
            _mapper.Map(clientDto, existingClient);
            await _clientRepository.UpdateAsync(existingClient);
        }

        public async Task DeleteClientAsync(int id)
        {
            await _clientRepository.DeleteAsync(id);
        }

        public async Task<ClientDto?> GetClientByNameAsync(string name)
        {
            var client = await _clientRepository.GetByNameAsync(name);
            if (client == null) return null;
            return _mapper.Map<ClientDto>(client);
        }
    }
}