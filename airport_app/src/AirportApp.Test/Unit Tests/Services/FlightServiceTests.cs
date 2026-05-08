using Moq;

namespace AirportApp.Test.Unit_Tests.Services;

public class FlightServiceTests
{
    private const string FirstFlightNumber = "AA100";
    private const string SecondFlightNumber = "BB200";
    private const int NumberOfFlights = 2;
    private const int NegativeFlightId = -1;
    private const int ValidFlightId = 1;
    private const int InvalidFlightId = 99;
    private const int InvalidRouteId = 0;
    private const int ValidRouteId = 5;
    private const int ValidRunwayId = 3;
    private const int ValidGateId = 1;
    private static readonly DateTime FlightDate = new DateTime(2024, 5, 1);
    private static readonly DateTime NewFlightDate = new DateTime(2025, 6, 15);
    private static readonly DateTime NewFlightDate2 = new DateTime(2025, 12, 1);
    private const int NewRunwayId = 7;
    private const int NewGateId = 9;

    [Fact]
    public void GetAll_ShouldReturnAllFlights_Always()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flights = new List<Flight>
        {
            new Flight { FlightNumber = FirstFlightNumber },
            new Flight { FlightNumber = SecondFlightNumber }
        };
        mockFlightRepo.Setup(getAllFlights => getAllFlights.GetAllFlights()).Returns(flights);

        var flightService = new FlightService(mockFlightRepo.Object);
        var result = flightService.GetAllFlights();

        Assert.Equal(NumberOfFlights, result.Count);
        Assert.Equal(flights, result);
    }

    [Fact]
    public void GetById_ShouldReturnNull_ForInvalidId()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);

        Assert.Null(flightService.GetFlightById(NegativeFlightId));
    }

    [Fact]
    public void GetById_ShouldReturnFlight_WhenFound()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flight = new Flight { FlightNumber = FirstFlightNumber };
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(flight);

        var flightService = new FlightService(mockFlightRepo.Object);
        var result = flightService.GetFlightById(ValidFlightId);

        Assert.Equal(flight, result);
    }

    [Fact]
    public void GetByRoute_ShouldReturnEmptyList_ForInvalidRouteId()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);

        var result = flightService.GetFlightsByRouteId(InvalidRouteId);

        Assert.Empty(result);
        mockFlightRepo.Verify(doesNotCallRepository => doesNotCallRepository.GetFlightsByRouteId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void GetByRoute_ShouldReturnFlights_WhenFound()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flights = new List<Flight> { new Flight { FlightNumber = FirstFlightNumber } };
        mockFlightRepo.Setup(getFlights => getFlights.GetFlightsByRouteId(ValidRouteId)).Returns(flights);

        var flightService = new FlightService(mockFlightRepo.Object);
        var result = flightService.GetFlightsByRouteId(ValidRouteId);

        Assert.Equal(flights, result);
    }

    [Fact]
    public void AddFlight_ShouldThrow_ForNullFlightNumber()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);
        Assert.Throws<ArgumentException>(() => flightService.AddFlight(null, ValidRouteId,
            DateTime.Now, ValidRunwayId, ValidGateId));
    }

    [Fact]
    public void AddFlight_ShouldThrow_ForEmptyFlightNumber()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);
        Assert.Throws<ArgumentException>(() => flightService.AddFlight(string.Empty, ValidRouteId,
            DateTime.Now, ValidRunwayId, ValidGateId));
    }

    [Fact]
    public void AddFlight_ShouldThrow_ForWhitespaceFlightNumber()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);
        Assert.Throws<ArgumentException>(() => flightService.AddFlight(" ", ValidRouteId,
            DateTime.Now, ValidRunwayId, ValidGateId));
    }

    [Fact]
    public void AddFlight_ShouldThrow_ForInvalidRouteId()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => flightService.AddFlight(FirstFlightNumber, InvalidRouteId, DateTime.Now, ValidRunwayId, ValidGateId));
    }

    [Fact]
    public void AddFlight_ShouldWork_ForValidData()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockFlightRepo.Setup(addFlight => addFlight.AddFlight(It.IsAny<Flight>())).Returns(ValidFlightId);

        var flightService = new FlightService(mockFlightRepo.Object);
        var result = flightService.AddFlight(FirstFlightNumber, ValidRouteId, DateTime.Now, ValidRunwayId, ValidGateId);

        Assert.Equal(ValidFlightId, result);
        mockFlightRepo.Verify(addFlight => addFlight.AddFlight(It.IsAny<Flight>()), Times.Once);
    }

    [Fact]
    public void UpdateFlight_ShouldThrow_WhenFlightNotFound()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(InvalidFlightId)).Returns((Flight)null);

        var flightService = new FlightService(mockFlightRepo.Object);

        Assert.Throws<InvalidOperationException>(() => flightService.UpdateFlight(InvalidFlightId));
    }

    [Fact]
    public void UpdateFlight_ShouldUpdateOnlyDate_WhenFlightNumberIsNotProvided()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flight = new Flight { FlightNumber = FirstFlightNumber, Date = FlightDate };
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(flight);

        var flightService = new FlightService(mockFlightRepo.Object);
        flightService.UpdateFlight(ValidFlightId, date: NewFlightDate);
        Assert.Equal(NewFlightDate, flight.Date);
        Assert.Equal(FirstFlightNumber, flight.FlightNumber);
        mockFlightRepo.Verify(callsRepositoryToUpdateflight => callsRepositoryToUpdateflight.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void UpdateFlight_ShouldUpdateOnlyFlightNumber_WhenDateIsNotProvided()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flight = new Flight { FlightNumber = FirstFlightNumber, Date = FlightDate };
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(flight);
        var flightService = new FlightService(mockFlightRepo.Object);
        flightService.UpdateFlight(ValidFlightId, flightNumber: SecondFlightNumber);

        Assert.Equal(SecondFlightNumber, flight.FlightNumber);
        Assert.Equal(FlightDate, flight.Date);
        mockFlightRepo.Verify(callsRepostirotyToUpdateFlight => callsRepostirotyToUpdateFlight.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void UpdateFlight_ShouldUpdateOnlyRunwayId_WhenOtherFieldsAreNotProvided()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flight = new Flight { FlightNumber = FirstFlightNumber, Runway = new Runway { Id = ValidRunwayId } };
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(flight);
        var flightService = new FlightService(mockFlightRepo.Object);
        flightService.UpdateFlight(ValidFlightId, runwayId: NewRunwayId);

        Assert.Equal(NewRunwayId, flight.Runway.Id);
        Assert.Equal(FirstFlightNumber, flight.FlightNumber);
        mockFlightRepo.Verify(callsRepositoryToUpdateflight => callsRepositoryToUpdateflight.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void UpdateFlight_ShouldUpdateOnlyGateId_WhenOtherFieldsAreNotProvided()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flight = new Flight { FlightNumber = FirstFlightNumber, Gate = new Gate { Id = ValidGateId } };
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(flight);
        var flightService = new FlightService(mockFlightRepo.Object);
        flightService.UpdateFlight(ValidFlightId, gateId: NewGateId);

        Assert.Equal(NewGateId, flight.Gate.Id);
        Assert.Equal(FirstFlightNumber, flight.FlightNumber);
        mockFlightRepo.Verify(callsRepositoryToUpdateFlight => callsRepositoryToUpdateFlight.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void UpdateFlight_ShouldUpdateAllFields_WhenAllFieldsAreProvided()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flight = new Flight
        {
            FlightNumber = FirstFlightNumber,
            Date = FlightDate,
            Runway = new Runway { Id = ValidRunwayId },
            Gate = new Gate { Id = ValidGateId }
        };
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(flight);

        var flightService = new FlightService(mockFlightRepo.Object);
        var newDate = NewFlightDate2;
        flightService.UpdateFlight(ValidFlightId, newDate, SecondFlightNumber, NewRunwayId, NewGateId);
        Assert.Equal(newDate, flight.Date);
        Assert.Equal(SecondFlightNumber, flight.FlightNumber);
        Assert.Equal(NewRunwayId, flight.Runway.Id);
        Assert.Equal(NewGateId, flight.Gate.Id);
        mockFlightRepo.Verify(callsRepositoryToUpdateFlight => callsRepositoryToUpdateFlight.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void DeleteFlight_Should_Throw_For_Invalid_Id()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        var flightService = new FlightService(mockFlightRepo.Object);

        Assert.Throws<InvalidOperationException>(() => flightService.DeleteFlightUsingId(InvalidFlightId));
    }

    [Fact]
    public void DeleteFlight_ShouldThrow_WhenFlightNotFound()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockFlightRepo.Setup(getNoFlight => getNoFlight.GetFlightById(InvalidFlightId)).Returns((Flight)null);

        var flightService = new FlightService(mockFlightRepo.Object);

        Assert.Throws<InvalidOperationException>(() => flightService.DeleteFlightUsingId(InvalidFlightId));
    }

    [Fact]
    public void DeleteFlight_ShouldCall_RepoForValidId()
    {
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockFlightRepo.Setup(getFlight => getFlight.GetFlightById(ValidFlightId)).Returns(new Flight());

        var flightService = new FlightService(mockFlightRepo.Object);
        flightService.DeleteFlightUsingId(ValidFlightId);

        mockFlightRepo.Verify(callsRepositoryToDeleteFlight => callsRepositoryToDeleteFlight.DeleteFlightUsingId(ValidFlightId), Times.Once);
    }
}
