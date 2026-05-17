using Moq;

namespace AirportApp.Test.Unit_Tests.Services;

public class RunwayServiceTests
{
    private const int DefaultId = 1;
    private const int ValidHandleTime = 10;
    private const int UpdatedHandleTime = 30;
    private const string DefaultRunwayName = "R1";
    private const int DefaultHandleTime = 10;
    private const string OldRunwayName = "OldName";
    private const int OldHandleTime = 5;
    private const int DefaultHandleTime2 = 15;
    private const string DefaultRunwayName2 = "R2";
    private const string UpdatedRunwayName = "UpdatedName";
    private const int CreatedId = 7;
    private const string NonNumericHandleTimeText = "abc";
    private const int InvalidId = -1;
    private const int NonExistentId = 999;
    private const int NegativeHandleTime = -5;
    private const int NewRunwayId = 0;

    [Fact]
    public void GetAllRunways_ShouldReturnAllRunways_Always()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();

        var runways = new List<Runway>
        {
            new Runway { Name = DefaultRunwayName, HandleTime = DefaultHandleTime },
            new Runway { Name = DefaultRunwayName2, HandleTime = DefaultHandleTime2 }
        };

        mockRunwayRepo.Setup(getallrunways => getallrunways.GetAllRunways()).Returns(runways);

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);
        var result = runwayService.GetAllRunways();

        Assert.Equal(runways.Count, result.Count);
        Assert.Equal(runways, result);
    }

    [Fact]
    public void GetRunwayById_ShouldBeNull_ForInvalidId()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Null(runwayService.GetRunwayById(InvalidId));
    }

    [Fact]
    public void GetRunwayById_ShouldBeNull_WhenRunwayNotFound()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();

        mockRunwayRepo.Setup(getNullInsteadOfrunway => getNullInsteadOfrunway.GetRunwayById(NonExistentId)).Returns((Runway)null);

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Null(runwayService.GetRunwayById(NonExistentId));
    }

    [Fact]
    public void GetRunwayById_ShouldReturnRunway_WhenRunwayExists()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var runway = new Runway { Name = DefaultRunwayName, HandleTime = ValidHandleTime };

        mockRunwayRepo.Setup(getDefaultRunway => getDefaultRunway.GetRunwayById(DefaultId)).Returns(runway);

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);
        var result = runwayService.GetRunwayById(DefaultId);

        Assert.Equal(runway, result);
    }

    [Fact]
    public void GetRunwayById_ShouldThrow_WhenRepoThrows()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockRunwayRepo.Setup(throwExceptionInsteadOfGivingRunway => throwExceptionInsteadOfGivingRunway.GetRunwayById(It.IsAny<int>())).Throws<Exception>();

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Throws<Exception>(() => runwayService.GetRunwayById(DefaultId));
    }

    [Fact]
    public void AddRunway_ShouldThrow_ForNullName()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => runwayService.AddRunway(null, ValidHandleTime));
    }

    [Fact]
    public void AddRunway_ShouldThrow_ForEmptyName()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => runwayService.AddRunway(string.Empty, ValidHandleTime));
    }

    [Fact]
    public void AddRunway_ShouldThrow_ForInvalidHandleTime()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => runwayService.AddRunway(DefaultRunwayName, NegativeHandleTime));
    }

    [Fact]
    public void AddRunway_ShouldWork_WhenValidInput()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();

        mockRunwayRepo.Setup(addProvidedrunwayAndGetItsId => addProvidedrunwayAndGetItsId.AddRunway(It.IsAny<Runway>())).Returns(CreatedId);

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);
        var result = runwayService.AddRunway(DefaultRunwayName, ValidHandleTime);

        Assert.Equal(CreatedId, result);
        mockRunwayRepo.Verify(callsRepoToAddRunway => callsRepoToAddRunway.AddRunway(It.Is<Runway>(runway =>
            runway.Name == DefaultRunwayName &&
            runway.HandleTime == ValidHandleTime)), Times.Once);
    }

    [Fact]
    public void UpdateRunway_ShouldThrow_WhenRunwayNotFound()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockRunwayRepo.Setup(getNullInsteadOfRunway => getNullInsteadOfRunway.GetRunwayById(DefaultId)).Returns((Runway)null);

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.Throws<InvalidOperationException>(() => runwayService.UpdateRunway(DefaultId, UpdatedRunwayName));
    }

    [Fact]
    public void UpdateRunway_ShouldUpdateAllFields_WhenBothAreProvided()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var oldRunway = new Runway { Name = OldRunwayName, HandleTime = OldHandleTime };

        mockRunwayRepo.Setup(getOldRunway => getOldRunway.GetRunwayById(DefaultId)).Returns(oldRunway);

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);
        runwayService.UpdateRunway(DefaultId, UpdatedRunwayName, UpdatedHandleTime);

        Assert.Equal(UpdatedRunwayName, oldRunway.Name);
        Assert.Equal(UpdatedHandleTime, oldRunway.HandleTime);
        mockRunwayRepo.Verify(callsRepositoryToUpdateRunway => callsRepositoryToUpdateRunway.UpdateRunway(oldRunway), Times.Once);
    }

    [Fact]
    public void DeleteRunway_ShouldCallRepo_ForValidId()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockRunwayRepo.Setup(getDefaultRunway => getDefaultRunway.GetRunwayById(DefaultId)).Returns(new Runway { Name = DefaultRunwayName });

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);
        runwayService.DeleteRunwayUsingId(DefaultId);

        mockRunwayRepo.Verify(callsRepositoryToDeleteDefaultRunway => callsRepositoryToDeleteDefaultRunway.DeleteRunwayUsingId(DefaultId), Times.Once);
    }

    [Fact]
    public void SaveRunway_ShouldThrow_ForInvalidHandleTimeText()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        const string nonNumericValue = NonNumericHandleTimeText;
        Assert.Throws<ArgumentException>(() => runwayService.SaveRunway(0, DefaultRunwayName, nonNumericValue));
    }

    [Fact]
    public void SaveRunway_ShouldCallAdd_WhenIdIsZero()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        string handleTimeStr = ValidHandleTime.ToString();

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);
        runwayService.SaveRunway(NewRunwayId, DefaultRunwayName, handleTimeStr);

        mockRunwayRepo.Verify(callsRepositoryToAddRunway => callsRepositoryToAddRunway.AddRunway(It.IsAny<Runway>()), Times.Once);
    }

    [Fact]
    public void HasFlights_ShouldReturnTrue_WhenFlightsExist()
    {
        var mockRunwayRepo = new Mock<IRunwayRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();

        mockFlightRepo.Setup(getDefaultFlight => getDefaultFlight.GetFlightsByRunwayId(DefaultId))
                      .Returns(new List<Flight> { new Flight() });

        var runwayService = new RunwayService(mockRunwayRepo.Object, mockFlightRepo.Object);

        Assert.True(runwayService.HasFlights(DefaultId));
    }
}