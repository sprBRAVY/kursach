// PrintingOrderManager.Application/Mappings/MappingProfile.cs
using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг Client
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientName))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts));
            CreateMap<CreateClientDto, Client>();
            CreateMap<UpdateClientDto, Client>().ReverseMap();

            // Маппинг Equipment
            CreateMap<Equipment, EquipmentDto>()
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.EquipmentName))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.Specifications));
            CreateMap<CreateEquipmentDto, Equipment>();
            CreateMap<UpdateEquipmentDto, Equipment>().ReverseMap();

            // Маппинг Order
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.ClientName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Status, opt => opt.Ignore());
            CreateMap<UpdateOrderDto, Order>().ReverseMap();

            // Маппинг OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
                .ForMember(dest => dest.WorkerFullName, opt => opt.MapFrom(src => src.Worker != null ? src.Worker.WorkerFullName : null))
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Equipment != null ? src.Equipment.EquipmentName : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.Cost, opt => opt.Ignore()); // ← ИГНОРИРУЕМ

            CreateMap<UpdateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.Cost, opt => opt.Ignore());

            // Маппинг Payment
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<UpdatePaymentDto, Payment>().ReverseMap();

            // Маппинг Service
            CreateMap<Service, ServiceDto>();
            CreateMap<CreateServiceDto, Service>();
            CreateMap<UpdateServiceDto, Service>().ReverseMap();

            // Маппинг Worker
            CreateMap<Worker, WorkerDto>();
            CreateMap<CreateWorkerDto, Worker>();
            CreateMap<UpdateWorkerDto, Worker>().ReverseMap();
        }
    }
}