namespace AirportApp.Data.Services
{
    public class GateService(
        IGateRepository gateRepository,
        IFlightRepository flightRepository) : IGateService
    {
        private const string EmptyGateNameErrorMessage = "The gate name cannot be empty.";

        private const string CriticalDeleteWarningTemplate = "CRITICAL: Gate '{0}' has flights assigned. Deleting it will remove ALL associated flights. Proceed?";
        private const string StandardDeleteWarningTemplate = "Are you sure you want to delete gate '{0}'?";

        public List<Gate> GetAllGates()
        {
            return gateRepository.GetAllGates();
        }

        public Gate? GetGateById(int gateId)
        {
            if (gateId <= 0)
            {
                return null;
            }

            return gateRepository.GetGateById(gateId);
        }

        public int AddGate(string gateName)
        {
            if (string.IsNullOrWhiteSpace(gateName))
            {
                throw new ArgumentException(EmptyGateNameErrorMessage, nameof(gateName));
            }

            Gate newGate = new Gate
            {
                Name = gateName
            };

            return gateRepository.AddGate(newGate);
        }

        public void UpdateGate(int gateId, string? updatedGateName = null)
        {
            Gate? existingGate = gateRepository.GetGateById(gateId);

            if (existingGate == null)
            {
                return;
            }

            if (updatedGateName != null)
            {
                if (string.IsNullOrWhiteSpace(updatedGateName))
                {
                    throw new ArgumentException(EmptyGateNameErrorMessage, nameof(updatedGateName));
                }

                existingGate.Name = updatedGateName;
            }

            gateRepository.UpdateGate(existingGate);
        }

        public void DeleteGateUsingId(int gateId)
        {
            if (gateId > 0)
            {
                gateRepository.DeleteGateUsingId(gateId);
            }
        }

        public void SaveGate(int gateId, string gateName)
        {
            if (gateId == 0)
            {
                this.AddGate(gateName);
            }
            else
            {
                this.UpdateGate(gateId, gateName);
            }
        }

        public bool HasFlights(int gateId)
        {
            List<Flight> associatedFlights = flightRepository.GetFlightsByGateId(gateId);

            return associatedFlights.Count > 0;
        }

        public string GetDeleteWarningMessage(int gateId)
        {
            Gate? gate = this.GetGateById(gateId);

            if (gate == null)
            {
                return string.Empty;
            }

            bool gateHasAssignedFlights = this.HasFlights(gateId);

            if (gateHasAssignedFlights)
            {
                return string.Format(CriticalDeleteWarningTemplate, gate.Name);
            }

            return string.Format(StandardDeleteWarningTemplate, gate.Name);
        }
    }
}