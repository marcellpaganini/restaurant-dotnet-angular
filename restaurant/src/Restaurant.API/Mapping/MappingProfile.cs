using AutoMapper;
using Restaurant.Application.DTOs;
using Restaurant.Domain.Entities;

namespace Restaurant.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<ClientDto, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();

        CreateMap<Meal, MealDto>();
        CreateMap<MealDto, MealDto>();
        CreateMap<CreateMealDto, Meal>();
        CreateMap<UpdateMealDto, Meal>();

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItemDto>();
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, OrderDto>();
    }
}
