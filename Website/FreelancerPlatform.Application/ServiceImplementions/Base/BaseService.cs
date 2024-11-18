using AutoMapper;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions.Base
{
    public class BaseService<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<T> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
