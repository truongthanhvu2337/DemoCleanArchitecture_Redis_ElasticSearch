﻿using AutoMapper;
using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCase.Customers.Command.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        private readonly IMapper mapper;

        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IUserRepo userRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            this.mapper = mapper;
        }

        public async Task<APIResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepo.GetById(request.id);
            if (result == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "Not found",
                    Data = null,
                };
            }

            result.Name = request.Name;
            result.Address = request.Address;
            await _userRepo.UpdateCustomer(result);
            await _unitOfWork.SaveChangesAsync();
            var updatedUsers = mapper.Map<CustomerResponseDto>(result);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "ok ngon",
                Data = updatedUsers,
            };
        }
    }
}
