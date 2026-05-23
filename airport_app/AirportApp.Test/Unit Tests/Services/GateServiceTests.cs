using Moq;

namespace AirportApp.Test.Unit_Tests.Services;

public class GateServiceTests
{
    private const string FirstGateName = "A1";
    private const string SecondGateName = "B2";
    private const int ValidGateId = 1;
    private const int InvalidGateId = 99;
    private const int NegativeGateId = -1;
    private const int NumberOfGates = 2;
    private const int InexistentGateId = 0;

    [Fact]
    public void GetAllGates_ShouldReturnAllGates_Always()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gates = new List<Gate>
        {
            new Gate { Name = FirstGateName },
            new Gate { Name = SecondGateName }
        };

        mockGateRepo.Setup(getAllGates => getAllGates.GetAllGates()).Returns(gates);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        var result = gateService.GetAllGates();

        Assert.Equal(NumberOfGates, result.Count);
        Assert.Equal(gates, result);
    }

    [Fact]
    public void GetGateById_ShouldReturnNull_ForInvalidId()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.Null(gateService.GetGateById(InvalidGateId));
    }

    [Fact]
    public void GetGateById_ShouldReturnGate_WhenFound()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gate = new Gate { Name = FirstGateName };
        mockGateRepo.Setup(getGate => getGate.GetGateById(ValidGateId)).Returns(gate);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        var result = gateService.GetGateById(ValidGateId);
        Assert.Equal(gate, result);
    }

    [Fact]
    public void AddGate_ShouldThrow_ForNullName()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => gateService.AddGate(null));
    }

    [Fact]
    public void AddGate_ShouldThrow_ForEmptyName()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => gateService.AddGate(string.Empty));
    }

    [Fact]
    public void AddGate_ShouldThrow_ForWhitespaceName()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => gateService.AddGate(" "));
    }

    [Fact]
    public void AddGate_ShouldWork_ForValidData()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockGateRepo.Setup(addGate => addGate.AddGate(It.IsAny<Gate>())).Returns(ValidGateId);

        var service = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        var result = service.AddGate(FirstGateName);

        Assert.Equal(ValidGateId, result);
        mockGateRepo.Verify(callsRepositoryToAddGate => callsRepositoryToAddGate.AddGate(It.IsAny<Gate>()), Times.Once);
    }

    [Fact]
    public void UpdateGate_ShouldDoNothing_IfGateNotFound()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockGateRepo.Setup(getNullInsteadOfGate => getNullInsteadOfGate.GetGateById(ValidGateId)).Returns((Gate)null);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        gateService.UpdateGate(ValidGateId, SecondGateName);

        mockGateRepo.Verify(callsRepositorytoUpdateGate => callsRepositorytoUpdateGate.UpdateGate(It.IsAny<Gate>()), Times.Never);
    }

    [Fact]
    public void UpdateGate_ShouldThrow_ForWhitespaceNewName()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockGateRepo.Setup(getGate => getGate.GetGateById(ValidGateId)).Returns(new Gate { Name = FirstGateName });

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => gateService.UpdateGate(ValidGateId, " "));
    }

    [Fact]
    public void UpdateGate_ShouldThrow_ForEmptyNewName()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockGateRepo.Setup(getGate => getGate.GetGateById(1)).Returns(new Gate { Name = FirstGateName });

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.Throws<ArgumentException>(() => gateService.UpdateGate(1, string.Empty));
    }

    [Fact]
    public void UpdateGate_ShouldUpdateName_ForValidData()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gate = new Gate { Name = FirstGateName };
        mockGateRepo.Setup(getGate => getGate.GetGateById(ValidGateId)).Returns(gate);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        gateService.UpdateGate(ValidGateId, SecondGateName);
        Assert.Equal(SecondGateName, gate.Name);
        mockGateRepo.Verify(callsRepositoryToUpdateGate => callsRepositoryToUpdateGate.UpdateGate(gate), Times.Once);
    }

    [Fact]
    public void UpdateGate_ShouldCallRepo_EvenWhenNoChanges()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gate = new Gate { Name = FirstGateName };
        mockGateRepo.Setup(getGate => getGate.GetGateById(ValidGateId)).Returns(gate);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        gateService.UpdateGate(ValidGateId);
        Assert.Equal(FirstGateName, gate.Name);
        mockGateRepo.Verify(callsRepositoryToUpdateGate => callsRepositoryToUpdateGate.UpdateGate(gate), Times.Once);
    }

    [Fact]
    public void DeleteShould_NotCallRepo_ForInvalidId()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        gateService.DeleteGateUsingId(NegativeGateId);

        mockGateRepo.Verify(doesNotCallRepositorytoDeleteGate => doesNotCallRepositorytoDeleteGate.DeleteGateUsingId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Delete_ShouldCallRepo_ForValidId()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        gateService.DeleteGateUsingId(ValidGateId);

        mockGateRepo.Verify(callsRepositorytoDeleteGate => callsRepositorytoDeleteGate.DeleteGateUsingId(ValidGateId), Times.Once);
    }

    [Fact]
    public void SaveGate_ShouldCallAdd_WhenIdIsZero()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockGateRepo.Setup(addGate => addGate.AddGate(It.IsAny<Gate>())).Returns(ValidGateId);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        gateService.SaveGate(InexistentGateId, SecondGateName);

        mockGateRepo.Verify(callsRepositoryToAddGate => callsRepositoryToAddGate.AddGate(It.Is<Gate>(newGate => newGate.Name == SecondGateName)), Times.Once);
        mockGateRepo.Verify(doesNotCallRepositoryToUpdateGate => doesNotCallRepositoryToUpdateGate.UpdateGate(It.IsAny<Gate>()), Times.Never);
    }

    [Fact]
    public void SaveGate_ShouldCallUpdate_WhenIdIsNonZero()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        var gate = new Gate { Name = FirstGateName };
        mockGateRepo.Setup(getGate => getGate.GetGateById(ValidGateId)).Returns(gate);

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);
        gateService.SaveGate(ValidGateId, SecondGateName);
        mockGateRepo.Verify(callsRepositoryToUpdateGate => callsRepositoryToUpdateGate.UpdateGate(It.Is<Gate>(newGate => newGate.Name == SecondGateName)), Times.Once);
        mockGateRepo.Verify(doesNotCallRepositoryToAddGate => doesNotCallRepositoryToAddGate.AddGate(It.IsAny<Gate>()), Times.Never);
    }

    [Fact]
    public void HasFlights_ShouldReturnTrue_WhenFlightsExist()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockFlightRepo.Setup(getFlights => getFlights.GetFlightsByGateId(ValidGateId)).Returns(new List<Flight> { new Flight() });

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.True(gateService.HasFlights(ValidGateId));
    }

    [Fact]
    public void HasFlights_ShouldReturnFalse_WhenNoFlightsExist()
    {
        var mockGateRepo = new Mock<IGateRepository>();
        var mockFlightRepo = new Mock<IFlightRepository>();
        mockFlightRepo.Setup(getNoFlights => getNoFlights.GetFlightsByGateId(ValidGateId)).Returns(new List<Flight>());

        var gateService = new GateService(mockGateRepo.Object, mockFlightRepo.Object);

        Assert.False(gateService.HasFlights(ValidGateId));
    }
}
