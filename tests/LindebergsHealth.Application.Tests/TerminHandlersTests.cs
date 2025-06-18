using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LindebergsHealth.Application.Termine.Commands;
using LindebergsHealth.Application.Termine.Dto;
using LindebergsHealth.Application.Termine.Queries;
using LindebergsHealth.Application.Termine;
using LindebergsHealth.Domain.Termine;
using LindebergsHealth.Domain.Entities;
using Mapster;

namespace LindebergsHealth.Application.Tests
{
    public class TerminHandlersTests
    {
        public TerminHandlersTests()
        {
            // Mapster-Konfiguration einmalig registrieren
            LindebergsHealth.Application.MapsterRegistration.RegisterMappings();
        }

        [Fact]
        public async Task CreateTerminHandler_CreatesTermin_AndReturnsDetailDto()
        {
            var repoMock = new Mock<ITermineRepository>();
            repoMock.Setup(r => r.CreateTerminAsync(It.IsAny<Termin>())).ReturnsAsync((Termin t) => t);
            var handler = new CreateTerminHandler(repoMock.Object);
            var dto = new CreateTerminDto
            {
                Titel = "Test",
                Beschreibung = "Desc",
                Datum = DateTime.Today,
                DauerMinuten = 30
            };
            var command = new CreateTerminCommand(dto);
            var result = await handler.Handle(command, CancellationToken.None);
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Test", result.Titel);
            Xunit.Assert.Equal("Desc", result.Beschreibung);
            repoMock.Verify(r => r.CreateTerminAsync(It.IsAny<Termin>()), Times.Once);
        }

        [Fact]
        public async Task GetAllTermineHandler_ReturnsMappedDtos()
        {
            var termine = new List<Termin>
            {
                new Termin { Id = Guid.NewGuid(), Titel = "A", Datum = DateTime.Today, DauerMinuten = 10 },
                new Termin { Id = Guid.NewGuid(), Titel = "B", Datum = DateTime.Today.AddDays(1), DauerMinuten = 20 }
            };
            var repoMock = new Mock<ITermineRepository>();
            repoMock.Setup(r => r.GetAllTermineAsync()).ReturnsAsync(termine);
            var handler = new GetAllTermineHandler(repoMock.Object);
            var result = await handler.Handle(new GetAllTermineQuery(), CancellationToken.None);
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.Contains(result, t => t.Titel == "A");
            Xunit.Assert.Contains(result, t => t.Titel == "B");
        }

        [Fact]
        public async Task GetTerminByIdHandler_ReturnsMappedDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var termin = new Termin { Id = id, Titel = "X", Beschreibung = "Y", Datum = DateTime.Today, DauerMinuten = 15 };
            var repoMock = new Mock<ITermineRepository>();
            repoMock.Setup(r => r.GetTerminByIdAsync(id)).ReturnsAsync(termin);
            var handler = new GetTerminByIdHandler(repoMock.Object);
            var result = await handler.Handle(new GetTerminByIdQuery(id), CancellationToken.None);
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("X", result.Titel);
            Xunit.Assert.Equal("Y", result.Beschreibung);
        }

        [Fact]
        public async Task GetTerminByIdHandler_ReturnsNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            var repoMock = new Mock<ITermineRepository>();
            repoMock.Setup(r => r.GetTerminByIdAsync(id)).ReturnsAsync((Termin)null!);
            var handler = new GetTerminByIdHandler(repoMock.Object);
            var result = await handler.Handle(new GetTerminByIdQuery(id), CancellationToken.None);
            Xunit.Assert.Null(result);
        }
        [Fact]
        public async Task CreateTerminHandler_ThrowsException_WhenRepositoryFails()
        {
            var repoMock = new Mock<ITermineRepository>();
            repoMock.Setup(r => r.CreateTerminAsync(It.IsAny<Termin>())).ThrowsAsync(new Exception("DB-Fehler"));
            var handler = new CreateTerminHandler(repoMock.Object);
            var dto = new CreateTerminDto { Titel = "Fehler", Datum = DateTime.Today, DauerMinuten = 10 };
            var command = new CreateTerminCommand(dto);
            await Xunit.Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public void Mapster_Maps_UpdateTerminDto_To_Termin_Correctly()
        {
            var dto = new UpdateTerminDto
            {
                Id = Guid.NewGuid(),
                Titel = "Update",
                Beschreibung = "UpdateDesc",
                Datum = DateTime.Today,
                DauerMinuten = 45,
                MitarbeiterId = Guid.NewGuid(),
                PatientId = Guid.NewGuid(),
                RaumId = Guid.NewGuid(),
                KategorieId = Guid.NewGuid()
            };
            var termin = dto.Adapt<Termin>();
            Xunit.Assert.Equal(dto.Id, termin.Id);
            Xunit.Assert.Equal(dto.Titel, termin.Titel);
            Xunit.Assert.Equal(dto.Beschreibung, termin.Beschreibung);
            Xunit.Assert.Equal(dto.Datum, termin.Datum);
            Xunit.Assert.Equal(dto.DauerMinuten, termin.DauerMinuten);
        }

        [Fact]
        public void TerminListDto_IsImmutable_Record()
        {
            var dto = new TerminListDto
            {
                Id = Guid.NewGuid(),
                Titel = "RecordTest",
                Datum = DateTime.Now,
                DauerMinuten = 25
            };
            // Versuch, Property zu ändern (sollte nicht gehen, da init-only)
            // dto.Titel = "Test2"; // Compilerfehler, daher kommentiert
            var dto2 = dto with { Titel = "Test2" };
            Xunit.Assert.NotEqual(dto.Titel, dto2.Titel);
            Xunit.Assert.Equal(dto.Id, dto2.Id);
        }

        [Fact]
        public void Mapster_CustomMapping_CanBeConfigured()
        {
            // Beispiel für Custom-Mapping: PatientName aus PatientId
            TypeAdapterConfig<Termin, TerminListDto>.NewConfig()
                .Map(dest => dest.PatientName, src => src.PatientId != null ? "DummyPatient" : null);
            var termin = new Termin { Id = Guid.NewGuid(), Titel = "MitPatient", Datum = DateTime.Today, DauerMinuten = 10, PatientId = Guid.NewGuid() };
            var dto = termin.Adapt<TerminListDto>();
            Xunit.Assert.Equal("DummyPatient", dto.PatientName);
        }
    }
}

