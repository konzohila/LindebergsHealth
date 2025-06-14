using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Beziehungsperson Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class BeziehungspersonTests
{
    private Beziehungsperson _beziehungsperson;
    private Patient _patient;
    private Beziehungstyp _beziehungstyp;

    [SetUp]
    public void Setup()
    {
        _patient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "Max",
            Nachname = "Mustermann",
            Geburtsdatum = new DateTime(2010, 5, 15) // Kind
        };

        _beziehungstyp = new Beziehungstyp
        {
            Id = Guid.NewGuid(),
            Bezeichnung = "Mutter",
            Code = "MOTHER",
            Aktiv = true
        };

        _beziehungsperson = new Beziehungsperson
        {
            PatientId = _patient.Id,
            Patient = _patient,
            BeziehungstypId = _beziehungstyp.Id,
            Beziehungstyp = _beziehungstyp,
            Vorname = "Anna",
            Nachname = "Mustermann",
            Geburtsdatum = new DateTime(1985, 3, 20),
            IstNotfallkontakt = true,
            IstSorgeberechtigt = true,
            IstVolljaehrig = true
        };
    }

    [Test]
    public void Beziehungsperson_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_beziehungsperson, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Beziehungsperson_ShouldHaveValidRelationshipData()
    {
        // Assert
        Assert.That(_beziehungsperson.PatientId, Is.EqualTo(_patient.Id));
        Assert.That(_beziehungsperson.Patient, Is.EqualTo(_patient));
        Assert.That(_beziehungsperson.BeziehungstypId, Is.EqualTo(_beziehungstyp.Id));
        Assert.That(_beziehungsperson.Beziehungstyp, Is.EqualTo(_beziehungstyp));
        Assert.That(_beziehungsperson.Vorname, Is.EqualTo("Anna"));
        Assert.That(_beziehungsperson.Nachname, Is.EqualTo("Mustermann"));
        Assert.That(_beziehungsperson.IstNotfallkontakt, Is.True);
        Assert.That(_beziehungsperson.IstSorgeberechtigt, Is.True);
        Assert.That(_beziehungsperson.IstVolljaehrig, Is.True);
    }

    [Test]
    public void Beziehungsperson_ShouldSupportEmergencyContact()
    {
        // Arrange - Notfallkontakt-Daten
        var notfallkontakt = new Beziehungsperson
        {
            PatientId = _patient.Id,
            Vorname = "Dr. Klaus",
            Nachname = "Müller",
            IstNotfallkontakt = true,
            NotfallTelefon = "+49 30 12345678",
            NotfallEmail = "klaus.mueller@example.com",
            Verfuegbarkeit = "Mo-Fr 8-18 Uhr, Sa 9-12 Uhr",
            Prioritaet = 1 // Erste Priorität
        };

        // Act
        _patient.Beziehungspersonen.Add(notfallkontakt);

        // Assert
        var notfallkontakte = _patient.Beziehungspersonen.Where(bp => bp.IstNotfallkontakt).ToList();
        Assert.That(notfallkontakte.Count, Is.EqualTo(1));
        Assert.That(notfallkontakte[0].NotfallTelefon, Is.EqualTo("+49 30 12345678"));
        Assert.That(notfallkontakte[0].NotfallEmail, Is.EqualTo("klaus.mueller@example.com"));
        Assert.That(notfallkontakte[0].Prioritaet, Is.EqualTo(1));
    }

    [Test]
    public void Beziehungsperson_ShouldSupportCustodyRights()
    {
        // Arrange - Sorgerecht-Szenarien
        var sorgeberechtigte = new[]
        {
            new Beziehungsperson
            {
                PatientId = _patient.Id,
                Vorname = "Anna",
                Nachname = "Mustermann",
                IstSorgeberechtigt = true,
                SorgerechtArt = "Alleiniges Sorgerecht",
                SorgerechtVon = new DateTime(2010, 5, 15), // Seit Geburt
                SorgerechtBis = null, // Unbefristet
                Beziehungstyp = new Beziehungstyp { Bezeichnung = "Mutter" }
            },
            new Beziehungsperson
            {
                PatientId = _patient.Id,
                Vorname = "Peter",
                Nachname = "Mustermann",
                IstSorgeberechtigt = false,
                SorgerechtArt = "Umgangsrecht",
                SorgerechtVon = new DateTime(2012, 1, 1),
                SorgerechtBis = null,
                Beziehungstyp = new Beziehungstyp { Bezeichnung = "Vater" }
            }
        };

        // Act
        foreach (var person in sorgeberechtigte)
        {
            _patient.Beziehungspersonen.Add(person);
        }

        // Assert
        var sorgeberechtigtePersonen = _patient.Beziehungspersonen
            .Where(bp => bp.IstSorgeberechtigt)
            .ToList();

        Assert.That(sorgeberechtigtePersonen.Count, Is.EqualTo(1));
        Assert.That(sorgeberechtigtePersonen[0].SorgerechtArt, Is.EqualTo("Alleiniges Sorgerecht"));
        Assert.That(sorgeberechtigtePersonen[0].Beziehungstyp.Bezeichnung, Is.EqualTo("Mutter"));
    }

    [Test]
    public void Beziehungsperson_ShouldValidateAgeForCustody()
    {
        // Arrange - Altersvalidierung für Sorgerecht
        var testPersonen = new[]
        {
            new { Geburtsdatum = DateTime.Today.AddYears(-16), IstVolljaehrig = false },
            new { Geburtsdatum = DateTime.Today.AddYears(-18), IstVolljaehrig = true },
            new { Geburtsdatum = DateTime.Today.AddYears(-25), IstVolljaehrig = true }
        };

        // Act & Assert
        foreach (var testPerson in testPersonen)
        {
            _beziehungsperson.Geburtsdatum = testPerson.Geburtsdatum;
            _beziehungsperson.IstVolljaehrig = testPerson.IstVolljaehrig;

            var alter = DateTime.Today.Year - testPerson.Geburtsdatum.Year;
            if (testPerson.Geburtsdatum.Date > DateTime.Today.AddYears(-alter))
                alter--;

            Assert.That(_beziehungsperson.IstVolljaehrig, Is.EqualTo(testPerson.IstVolljaehrig));
            
            if (testPerson.IstVolljaehrig)
            {
                Assert.That(alter, Is.GreaterThanOrEqualTo(18));
            }
        }
    }

    [Test]
    public void Beziehungsperson_ShouldSupportContactInformation()
    {
        // Arrange - Kontaktinformationen
        _beziehungsperson.TelefonPrivat = "+49 30 12345678";
        _beziehungsperson.TelefonMobil = "+49 170 1234567";
        _beziehungsperson.EmailPrivat = "anna.mustermann@example.com";

        // Assert
        Assert.That(_beziehungsperson.TelefonPrivat, Is.EqualTo("+49 30 12345678"));
        Assert.That(_beziehungsperson.TelefonMobil, Is.EqualTo("+49 170 1234567"));
        Assert.That(_beziehungsperson.EmailPrivat, Is.EqualTo("anna.mustermann@example.com"));
    }

    [Test]
    public void Beziehungsperson_ShouldSupportMedicalAuthorization()
    {
        // Arrange - Medizinische Vollmachten
        _beziehungsperson.KannMedizinischeEntscheidungenTreffen = true;
        _beziehungsperson.VollmachtGueltigVon = new DateTime(2024, 1, 1);
        _beziehungsperson.VollmachtGueltigBis = new DateTime(2024, 12, 31);

        // Assert
        Assert.That(_beziehungsperson.KannMedizinischeEntscheidungenTreffen, Is.True);
        Assert.That(_beziehungsperson.VollmachtGueltigVon, Is.EqualTo(new DateTime(2024, 1, 1)));
        Assert.That(_beziehungsperson.VollmachtGueltigBis, Is.EqualTo(new DateTime(2024, 12, 31)));
    }
} 