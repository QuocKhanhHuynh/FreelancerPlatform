using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FreelancerPlatform.ApiService
{
    public class SystemRecommendationApiService : ISystemRecommendationApiService
    {
        private readonly IHttpClientFactory _httpContextFactory;
        private readonly IConfiguration _configuration;

        public SystemRecommendationApiService(IHttpClientFactory httpContextFactory, IConfiguration configuration)
        {
            _httpContextFactory = httpContextFactory;
            _configuration = configuration;
        }
        public async Task BuildSystemRecommendation()
        {
            var httpClient = _httpContextFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["AIAddress"]);
            await httpClient.GetAsync($"/api/trainai");
        }

        public async Task<List<int>> GetFreelancerRecommendation(int freelancerId)
        {
            var httpClient = _httpContextFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["AIAddress"]);

            // Gọi API
            var response = await httpClient.GetAsync($"/api/recommendFeelancer/{freelancerId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RecommendationResponse>(jsonResponse);
                return data.Result;  // Trả về danh sách số nguyên trong `Result`
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }

        public async Task<List<int>> GetRecommendation(int jobId)
        {
            var httpClient = _httpContextFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["AIAddress"]);

            // Gọi API
            var response = await httpClient.GetAsync($"/api/recommend/{jobId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<RecommendationResponse>(jsonResponse);
                return data.Result;  // Trả về danh sách số nguyên trong `Result`
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }

    }

    public class RecommendationResponse
    {
        public List<int> Result { get; set; }
    }
}
