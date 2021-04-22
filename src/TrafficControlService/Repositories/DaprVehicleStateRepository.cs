using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TrafficControlService.Models;

namespace TrafficControlService.Repositories
{
    public class DaprVehicleStateRepository : IVehicleStateRepository
    {
        private readonly HttpClient _httpClient;
        private const string DAPR_STORE_NAME = "statestore";

        public DaprVehicleStateRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<VehicleState> GetVehicleStateAsync(string licenseNumber)
        {
            var state = await _httpClient.GetFromJsonAsync<VehicleState>(
                $"http://localhost:3600/v1.0/state/{DAPR_STORE_NAME}/{licenseNumber}");
            return state;
        }

        public async Task SaveVehicleStateAsync(VehicleState vehicleState)
        {
            var state = new[]
            {
                new { 
                    key = vehicleState.LicenseNumber,
                    value = vehicleState
                }
            };

            await _httpClient.PostAsJsonAsync(
                $"http://localhost:3600/v1.0/state/{DAPR_STORE_NAME}",
                state);
        }
    }
}