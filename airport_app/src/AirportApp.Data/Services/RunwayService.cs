namespace AirportApp.Data.Services
{
    public class RunwayService(
        IRunwayRepository runwayRepository,
        IFlightRepository flightRepository) : IRunwayService
    {
        private const string EmptyRunwayNameErrorMessage = "The runway name cannot be empty.";
        private const string RunwayNotFoundErrorMessage = "Runway with Id {0} does not exist in the system.";

        private const string CriticalDeleteWarningTemplate = "CRITICAL: Runway '{0}' has flights assigned. Deleting it will remove ALL associated flights. Proceed?";
        private const string StandardDeleteWarningTemplate = "Are you sure you want to delete runway '{0}'?";

        public List<Runway> GetAllRunways()
        {
            return runwayRepository.GetAllRunways();
        }

        public Runway? GetRunwayById(int runwayId)
        {
            if (runwayId <= 0)
            {
                return null;
            }

            return runwayRepository.GetRunwayById(runwayId);
        }

        public int AddRunway(string runwayName, int handleTime)
        {
            if (string.IsNullOrWhiteSpace(runwayName))
            {
                throw new ArgumentException(EmptyRunwayNameErrorMessage, nameof(runwayName));
            }

            if (handleTime <= 0)
            {
                throw new ArgumentException("The handle time must be a positive number greater than zero.", nameof(handleTime));
            }

            Runway newRunway = new Runway
            {
                Name = runwayName,
                HandleTime = handleTime
            };

            return runwayRepository.AddRunway(newRunway);
        }

        public void UpdateRunway(int runwayId, string? newName = null, int? newHandleTime = null)
        {
            Runway? existingRunway = runwayRepository.GetRunwayById(runwayId);

            if (existingRunway == null)
            {
                throw new InvalidOperationException(string.Format(RunwayNotFoundErrorMessage, runwayId));
            }

            if (newName != null)
            {
                if (string.IsNullOrWhiteSpace(newName))
                {
                    throw new ArgumentException(EmptyRunwayNameErrorMessage, nameof(newName));
                }

                existingRunway.Name = newName;
            }

            if (newHandleTime != null)
            {
                if (newHandleTime <= 0)
                {
                    throw new ArgumentException("The handle time must be a positive number greater than zero.", nameof(newHandleTime));
                }

                existingRunway.HandleTime = newHandleTime.Value;
            }

            runwayRepository.UpdateRunway(existingRunway);
        }

        public void DeleteRunwayUsingId(int runwayId)
        {
            if (runwayRepository.GetRunwayById(runwayId) == null)
            {
                throw new InvalidOperationException(string.Format(RunwayNotFoundErrorMessage, runwayId));
            }

            runwayRepository.DeleteRunwayUsingId(runwayId);
        }

        public void SaveRunway(int runwayId, string runwayName, string handleTimeText)
        {
            if (!int.TryParse(handleTimeText, out int handleTime) || handleTime <= 0)
            {
                throw new ArgumentException("Handle time must be a valid positive numeric value.");
            }

            if (runwayId == 0)
            {
                this.AddRunway(runwayName, handleTime);
            }
            else
            {
                this.UpdateRunway(runwayId, runwayName, handleTime);
            }
        }

        public bool HasFlights(int runwayId)
        {
            List<Flight> associatedFlights = flightRepository.GetFlightsByRunwayId(runwayId);

            return associatedFlights.Count > 0;
        }

        public string GetDeleteWarningMessage(int runwayId)
        {
            Runway? runway = this.GetRunwayById(runwayId);

            if (runway == null)
            {
                return string.Empty;
            }

            bool runwayHasAssignedFlights = this.HasFlights(runwayId);

            if (runwayHasAssignedFlights)
            {
                return string.Format(CriticalDeleteWarningTemplate, runway.Name);
            }

            return string.Format(StandardDeleteWarningTemplate, runway.Name);
        }
    }
}