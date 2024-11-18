using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IOfferService
    {
        Task<ServiceResult> CreateOfferAsync(OfferCreateRequest request);
        Task<ServiceResult> DeleteOfferAsync(int id);
        Task<List<OfferQuicckViewModel>> GetAllOffer();
        Task<bool> CheckIsOffer(int jobId, int freelancerId);

    }
}
