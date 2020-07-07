﻿using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using shop_giay_server.Controllers;
using shop_giay_server.data;
using System;
using shop_giay_server;
using System.Collections.Generic;
using System.Linq;

namespace shop_giay_server._Controllers
{
    public class CustomerController : GeneralController<Customer, CustomerDTO>
    {
        private readonly IAuthRepository _authRepo;

        public CustomerController(IAsyncRepository<Customer> repo, ILogger<CustomerController> logger, IMapper mapper, IAuthRepository authRepo, DataContext context)
            : base(repo, logger, mapper, context)
        {
            _authRepo = authRepo;
        }


        #region Client API


        //
        //  CREATE CUSTOMER API
        //
        [HttpPost]
        [Route("client/customer/register")]
        public async Task<IActionResult> ClientAddCustomer([FromBody] BodyCreateCustomer model)
        {
            // Get username
            if (model.username == null)
            {
                model.username = model.email;
            }

            // Get gender 
            var gender = 1;
            if (model.gender.ToLower() == "nam" || model.gender.ToLower() == "male")
            {
                gender = 0;
            }

            // Validate
            var existed = await _repository.ExistWhere(c => c.Email == model.email
                    || c.PhoneNumber == model.phoneNumber
                    || c.Username == model.username);

            if (existed)
            {
                return Ok(ResponseDTO.BadRequest("Khách hàng đã tồn tại."));
            }

            // Create pass
            byte[] passwordHash, passwordSalt;
            SecurityHelper.CreatePasswordHash(model.password, out passwordHash, out passwordSalt);

            var customer = new Customer
            {
                DeleteFlag = false,
                Username = model.username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = model.name,
                Gender = gender,
                Point = 0,
                PhoneNumber = model.phoneNumber,
                Email = model.email,
                DateOfBirth = new DateTime(), // todo: fix datetime format
                CustomerTypeId = model.customerTypeId
            };

            // Create customer 
            var result = await _repository.Add(customer);

            // Ad-hoc: erase password 
            result.PasswordHash = null;
            result.PasswordSalt = null;
            return Ok(ResponseDTO.Ok(result));
        }


        //
        //  CUSTOMER GET INFO
        //
        [HttpGet]
        [Route("client/customer/getInfo")]
        public async Task<IActionResult> ClientGetCustomerInfo()
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername))
            {
                return Ok(ResponseDTO.BadRequest());
            }

            var customer = await _repository.FirstOrDefault(c => c.Username == sessionUsername);
            if (customer == null)
            {
                return Ok(ResponseDTO.NotFound());
            }

            return Ok(ResponseDTO.Ok(new
            {
                Id = customer.Id,
                customer.Name,
                DateOfBirth = customer.DateOfBirth.ToString("o"),
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Gender = customer.Gender
            }));
        }


        //
        //  CUSTOMER GET ADDRESSES
        //
        [HttpGet]
        [Route("client/customer/getAddresses")]
        public async Task<IActionResult> ClientGetCustomerAddress()
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername))
            {
                return Ok(ResponseDTO.BadRequest());
            }

            var customer = await _repository.FirstOrDefault(c => c.Username == sessionUsername);
            if (customer == null)
            {
                return Ok(ResponseDTO.NotFound());
            }

            var listResult = new List<dynamic>();
            foreach (var address in customer.Addresses)
            {
                listResult.Add(ConvertToResponseAddressDTO(address, customer.Id));
            }
            return Ok(ResponseDTO.Ok(listResult));
        }


        //
        // ADD ADDRESS
        //
        [Route("client/[controller]/add-address")]
        [HttpPost]
        public async Task<IActionResult> ClientAddAddress([FromBody] BodyAddressDTO dto)
        {
            var customer = await GetCustomer();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest("Invalid customers' username."));
            }

            var newAddress = new Address()
            {
                City = dto.city,
                District = dto.district,
                Ward = dto.ward,
                Street = dto.street,
                CustomerId = customer.Id,
                RecipientName = dto.name,
                RecipientPhoneNumber = dto.phoneNumber
            };

            customer.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            var listResponse = new List<object>();
            foreach (var add in customer.Addresses)
            {
                listResponse.Add(ConvertToResponseAddressDTO(add, customer.Id));
            }
            return Ok(ResponseDTO.Ok(listResponse));
        }


        //
        // UPDATE ADDRESS
        //
        [Route("client/[controller]/update-address")]
        [HttpPost]
        public async Task<IActionResult> ClientUpdateAddress([FromBody] BodyAddressDTO dto)
        {
            var customer = await GetCustomer();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest("Invalid customers' username."));
            }

            var updateAddress = customer.Addresses.FirstOrDefault(c => c.Id == dto.id);
            if (updateAddress != null)
            {
                updateAddress.City = dto.city;
                updateAddress.District = dto.district;
                updateAddress.Ward = dto.ward;
                updateAddress.Street = dto.street;
                updateAddress.CustomerId = customer.Id;
                updateAddress.RecipientName = dto.name;
                updateAddress.RecipientPhoneNumber = dto.phoneNumber;
            }
            else
            {
                return Ok(ResponseDTO.NotFound("Invalid ID for updated address."));
            }

            await _context.SaveChangesAsync();

            var listResponse = new List<object>();
            foreach (var add in customer.Addresses)
            {
                listResponse.Add(ConvertToResponseAddressDTO(add, customer.Id));
            }
            return Ok(ResponseDTO.Ok(listResponse));
        }


        //
        // CHANGE PASSWORD
        //
        [Route("client/[controller]/changePassword")]
        [HttpPost]
        public async Task<IActionResult> ClientChangePassword([FromBody] ChangePasswordDTO model)
        {
            var customer = await GetCustomer();
            if (customer == null)
            {
                return Ok(ResponseDTO.BadRequest("Invalid customers' username."));
            }

            // Check password
            if (!SecurityHelper.VerifyPassword(model.oldPassword, customer.PasswordHash, customer.PasswordSalt))
            {
                return Ok(ResponseDTO.BadRequest("Old password is not correct."));
            }

            // Update password
            byte[] passwordHash, passwordSalt;
            SecurityHelper.CreatePasswordHash(model.newPassword, out passwordHash, out passwordSalt);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();
            return Ok(ResponseDTO.OkEmpty());
        }


        #endregion


        #region Admin API


        // Customer Update
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO dto)
        {
            if (id == dto.Id)
            {
                return BadRequest(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }
            var entity = _mapper.Map<Customer>(dto);
            entity.Id = id;

            var updatedItem = await _repository.Update(entity);
            if (updatedItem == null)
            {
                return BadRequest(ResponseDTO.BadRequest("Item ID is not existed."));
            }
            return Ok(ResponseDTO.Ok(entity));
        }


        // address update
        [Route("admin/[controller]/{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressDTO dto)
        {
            if (id == dto.Id)
            {
                return BadRequest(ResponseDTO.BadRequest("URL ID and Item ID does not matched."));
            }
            var entity = _mapper.Map<Address>(dto);
            entity.Id = id;

            var updatedItem = _context.Addresses.Update(entity);
            if (updatedItem == null)
            {
                return BadRequest(ResponseDTO.BadRequest("Item ID is not existed."));
            }
            return Ok(ResponseDTO.Ok(entity));
        }


        #endregion


        #region HELPER METHODS

        public async Task<Customer> GetCustomer()
        {
            var sessionUsername = HttpContext.Session.GetString(SessionConstant.Username);
            if (string.IsNullOrEmpty(sessionUsername))
            {
                return null;
            }

            var customer = await _repository.FirstOrDefault(c => c.Username == sessionUsername);
            return customer;
        }

        public object ConvertToResponseAddressDTO(Address address, int customerId)
        {
            return new
            {
                Id = address.Id,
                CustomerId = customerId,
                City = address.City,
                address.District,
                address.Ward,
                address.Street
            };
        }

        #endregion

    }


    public class BodyCreateCustomer
    {
        public string username { get; set; } = null;
        public int customerTypeId { get; set; } = 1;
        public DateTime dateOfBirth { get; set; } = new DateTime();

        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string password { get; set; }

    }

    public class SecurityHelper
    {
        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }

    public class BodyAddressDTO
    {
        public int id { get; set; } = 0;
        public string city { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string street { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
    }

    public class ChangePasswordDTO
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
