// PrintingOrderManager.Application/Services/ServiceService.cs
using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

namespace PrintingOrderManager.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllWithUsageAsync();
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<ServiceDto> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) return null;
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task AddServiceAsync(CreateServiceDto serviceDto)
        {
            var service = _mapper.Map<Service>(serviceDto);
            await _serviceRepository.AddAsync(service);
        }

        public async Task UpdateServiceAsync(int id, UpdateServiceDto serviceDto)
        {
            var existingService = await _serviceRepository.GetByIdAsync(id);
            if (existingService == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            _mapper.Map(serviceDto, existingService);
            await _serviceRepository.UpdateAsync(existingService);
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _serviceRepository.DeleteAsync(id);
        }

        public async Task<ServiceDto?> GetServiceByNameAsync(string name)
        {
            var service = await _serviceRepository.GetByNameAsync(name);
            if (service == null) return null;
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<IEnumerable<ServiceDto>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var services = await _serviceRepository.GetByPriceRangeAsync(minPrice, maxPrice);
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }
    }
}