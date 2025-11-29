// PrintingOrderManager.Application.Services/EquipmentService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMapper _mapper;

        public EquipmentService(IEquipmentRepository equipmentRepository, IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync()
        {
            var equipment = await _equipmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EquipmentDto>>(equipment);
        }

        public IQueryable<EquipmentDto> GetEquipmentQueryable()
        {
            return _equipmentRepository.GetQueryable()
                .ProjectTo<EquipmentDto>(_mapper.ConfigurationProvider);
        }

        public async Task<EquipmentDto> GetEquipmentByIdAsync(int id)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            return equipment == null ? null : _mapper.Map<EquipmentDto>(equipment);
        }

        public async Task AddEquipmentAsync(CreateEquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);
            await _equipmentRepository.AddAsync(equipment);
        }

        public async Task UpdateEquipmentAsync(int id, UpdateEquipmentDto equipmentDto)
        {
            var existingEquipment = await _equipmentRepository.GetByIdAsync(id);
            if (existingEquipment == null)
                throw new KeyNotFoundException($"Equipment with ID {id} not found.");
            _mapper.Map(equipmentDto, existingEquipment);
            await _equipmentRepository.UpdateAsync(existingEquipment);
        }

        public async Task DeleteEquipmentAsync(int id)
        {
            await _equipmentRepository.DeleteAsync(id);
        }

        public async Task<EquipmentDto?> GetEquipmentByNameAsync(string name)
        {
            var equipment = await _equipmentRepository.GetByNameAsync(name);
            return equipment == null ? null : _mapper.Map<EquipmentDto>(equipment);
        }

        public async Task<EquipmentDto?> GetEquipmentByModelAsync(string model)
        {
            var equipment = await _equipmentRepository.GetAllAsync();
            var filtered = equipment.FirstOrDefault(e => e.Model != null &&
                e.Model.Contains(model, StringComparison.OrdinalIgnoreCase));
            return filtered == null ? null : _mapper.Map<EquipmentDto>(filtered);
        }
    }
}