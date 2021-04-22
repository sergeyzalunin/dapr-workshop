using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dapr.Client;
using TrafficControlService.Models;

namespace TrafficControlService.Repositories
{
    public class DaprVehicleStateRepository : IVehicleStateRepository
    {
        private readonly DaprClient _daprClient;
        private const string DAPR_STORE_NAME = "statestore";

        public DaprVehicleStateRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }
        public async Task<VehicleState> GetVehicleStateAsync(string licenseNumber)
        {
            return await _daprClient.GetStateAsync<VehicleState>(
                DAPR_STORE_NAME, licenseNumber);
        }

        public async Task SaveVehicleStateAsync(VehicleState vehicleState)
        {
            await _daprClient.SaveStateAsync(
                DAPR_STORE_NAME, vehicleState.LicenseNumber, vehicleState);
        }
    }
}