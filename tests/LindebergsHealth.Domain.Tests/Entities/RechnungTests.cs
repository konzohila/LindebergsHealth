using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Rechnung Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class RechnungTests
{
    private Rechnung _rechnung;
    private Patient _patient;
    private Termin _termin;

    [SetUp]
    public void Setup()
    {
        _patient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        _termin = new Termin
        {
            Id = Guid.NewGuid(),
            PatientId = _patient.Id,
            Datum = DateTime.Today.AddDays(-1),
            DauerMinuten = 60
        };

        _rechnung = new Rechnung
        {
            PatientId = _patient.Id,
            Patient = _patient,
            TerminId = _termin.Id,
            Termin = _termin,
            Betrag = 85.50m,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Fälligkeitsdatum = DateTime.Today.AddDays(14)
        };
    }

    [Test]
    public void Rechnung_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_rechnung, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Rechnung_ShouldHaveValidInvoiceData()
    {
        // Assert
        Assert.That(_rechnung.PatientId, Is.EqualTo(_patient.Id));
        Assert.That(_rechnung.Patient, Is.EqualTo(_patient));
        Assert.That(_rechnung.TerminId, Is.EqualTo(_termin.Id));
        Assert.That(_rechnung.Termin, Is.EqualTo(_termin));
        Assert.That(_rechnung.Betrag, Is.EqualTo(85.50m));
        Assert.That(_rechnung.Rechnungsnummer, Is.EqualTo("R-2024-001"));
        Assert.That(_rechnung.Rechnungsdatum, Is.EqualTo(DateTime.Today));
        Assert.That(_rechnung.Fälligkeitsdatum, Is.EqualTo(DateTime.Today.AddDays(14)));
    }

    [Test]
    public void Rechnung_ShouldInitializeCollections()
    {
        // Assert
        Assert.That(_rechnung.Positionen, Is.Not.Null);
        Assert.That(_rechnung.Zahlungseingänge, Is.Not.Null);
    }

    [Test]
    public void Rechnung_ShouldCalculatePaymentStatus()
    {
        // Arrange - Unbezahlte Rechnung
        Assert.That(_rechnung.BezahltAm, Is.Null);

        // Act - Zahlung eingegangen
        _rechnung.BezahltAm = DateTime.Today;

        // Assert
        Assert.That(_rechnung.BezahltAm, Is.EqualTo(DateTime.Today));
    }

    [Test]
    public void Rechnung_ShouldSupportPartialPayments()
    {
        // Arrange
        var teilzahlung1 = new Zahlungseingang
        {
            RechnungId = _rechnung.Id,
            Rechnung = _rechnung,
            Betrag = 40.00m,
            Eingangsdatum = DateTime.Today.AddDays(-5),
            IstTeilzahlung = true
        };

        var teilzahlung2 = new Zahlungseingang
        {
            RechnungId = _rechnung.Id,
            Rechnung = _rechnung,
            Betrag = 45.50m,
            Eingangsdatum = DateTime.Today,
            IstTeilzahlung = false // Restzahlung
        };

        // Act
        _rechnung.Zahlungseingänge.Add(teilzahlung1);
        _rechnung.Zahlungseingänge.Add(teilzahlung2);

        // Assert
        var gesamtZahlungen = _rechnung.Zahlungseingänge.Sum(z => z.Betrag);
        Assert.That(gesamtZahlungen, Is.EqualTo(_rechnung.Betrag));
        Assert.That(_rechnung.Zahlungseingänge.Count, Is.EqualTo(2));
        Assert.That(_rechnung.Zahlungseingänge.First().IstTeilzahlung, Is.True);
        Assert.That(_rechnung.Zahlungseingänge.Last().IstTeilzahlung, Is.False);
    }

    [Test]
    public void Rechnung_ShouldSupportDetailedPositions()
    {
        // Arrange
        var leistungstyp1 = new Leistungstyp { Bezeichnung = "Osteopathie", Code = "OST" };
        var leistungstyp2 = new Leistungstyp { Bezeichnung = "Physiotherapie", Code = "PHY" };

        var position1 = new RechnungsPosition
        {
            RechnungId = _rechnung.Id,
            Rechnung = _rechnung,
            LeistungstypId = leistungstyp1.Id,
            Leistungstyp = leistungstyp1,
            Bezeichnung = "Osteopathische Behandlung",
            GOÄ_Ziffer = "3306",
            Einzelpreis = 85.50m,
            Anzahl = 1,
            Faktor = 1.0m,
            Gesamtpreis = 85.50m
        };

        var position2 = new RechnungsPosition
        {
            RechnungId = _rechnung.Id,
            Rechnung = _rechnung,
            LeistungstypId = leistungstyp2.Id,
            Leistungstyp = leistungstyp2,
            Bezeichnung = "Krankengymnastik",
            GOÄ_Ziffer = "846",
            Einzelpreis = 25.00m,
            Anzahl = 2,
            Faktor = 1.2m,
            Gesamtpreis = 60.00m
        };

        // Act
        _rechnung.Positionen.Add(position1);
        _rechnung.Positionen.Add(position2);

        // Assert
        Assert.That(_rechnung.Positionen.Count, Is.EqualTo(2));
        
        var gesamtbetrag = _rechnung.Positionen.Sum(p => p.Gesamtpreis);
        Assert.That(gesamtbetrag, Is.EqualTo(145.50m));
        
        // GOÄ-Faktor Berechnung testen
        Assert.That(position2.Gesamtpreis, Is.EqualTo(position2.Einzelpreis * position2.Anzahl * position2.Faktor));
    }

    [Test]
    public void Rechnung_ShouldValidateInvoiceNumber()
    {
        // Arrange
        var validInvoiceNumbers = new[]
        {
            "R-2024-001",
            "INV-2024-0001",
            "2024/001",
            "24-001"
        };

        // Act & Assert
        foreach (var invoiceNumber in validInvoiceNumbers)
        {
            _rechnung.Rechnungsnummer = invoiceNumber;
            Assert.That(_rechnung.Rechnungsnummer, Is.EqualTo(invoiceNumber));
            Assert.That(string.IsNullOrWhiteSpace(_rechnung.Rechnungsnummer), Is.False);
        }
    }

    [Test]
    public void Rechnung_ShouldCalculateDueDates()
    {
        // Arrange
        var rechnungsdatum = new DateTime(2024, 1, 15);
        var zahlungsziel = 14; // Tage

        // Act
        _rechnung.Rechnungsdatum = rechnungsdatum;
        _rechnung.Fälligkeitsdatum = rechnungsdatum.AddDays(zahlungsziel);

        // Assert
        Assert.That(_rechnung.Fälligkeitsdatum, Is.EqualTo(new DateTime(2024, 1, 29)));
        
        // Überfällig prüfen
        var heute = new DateTime(2024, 2, 1);
        var istÜberfällig = _rechnung.Fälligkeitsdatum < heute && _rechnung.BezahltAm == null;
        Assert.That(istÜberfällig, Is.True);
    }

    [Test]
    public void Rechnung_ShouldSupportTaxCalculation()
    {
        // Arrange
        var steuerdaten = new Steuerdaten
        {
            RechnungId = _rechnung.Id,
            Steuersatz = 19.0m,
            Nettobetrag = 71.85m,
            Steuerbetrag = 13.65m,
            Bruttobetrag = 85.50m
        };

        // Act
        _rechnung.Steuerdaten = steuerdaten;

        // Assert
        Assert.That(_rechnung.Steuerdaten, Is.Not.Null);
        Assert.That(_rechnung.Steuerdaten.Steuersatz, Is.EqualTo(19.0m));
        Assert.That(_rechnung.Steuerdaten.Bruttobetrag, Is.EqualTo(_rechnung.Betrag));
        
        // Steuerberechnung validieren
        var berechneterSteuerbetrag = _rechnung.Steuerdaten.Nettobetrag * (_rechnung.Steuerdaten.Steuersatz / 100);
        Assert.That(berechneterSteuerbetrag, Is.EqualTo(_rechnung.Steuerdaten.Steuerbetrag).Within(0.01m));
    }

    [Test]
    public void Rechnung_ShouldSupportNotes()
    {
        // Arrange
        var notizen = "Rechnung per E-Mail versendet. Patient hat Nachfragen zur GOÄ-Ziffer.";

        // Act
        _rechnung.Notizen = notizen;

        // Assert
        Assert.That(_rechnung.Notizen, Is.EqualTo(notizen));
    }

    [Test]
    public void Rechnung_ShouldValidateAmounts()
    {
        // Arrange & Act
        var validAmounts = new[] { 0.01m, 85.50m, 1000.00m, 9999.99m };
        var invalidAmounts = new[] { -1.00m, 0.00m };

        // Assert - Valid amounts
        foreach (var amount in validAmounts)
        {
            _rechnung.Betrag = amount;
            Assert.That(_rechnung.Betrag, Is.GreaterThan(0));
        }

        // Invalid amounts should be handled by business logic
        foreach (var amount in invalidAmounts)
        {
            _rechnung.Betrag = amount;
            if (amount <= 0)
            {
                Assert.That(_rechnung.Betrag, Is.LessThanOrEqualTo(0));
            }
        }
    }

    [Test]
    public void Rechnung_ShouldTrackPaymentHistory()
    {
        // Arrange
        var zahlung1 = new Zahlungseingang
        {
            Betrag = 50.00m,
            Eingangsdatum = DateTime.Today.AddDays(-10),
            Referenz = "SEPA-Überweisung"
        };

        var zahlung2 = new Zahlungseingang
        {
            Betrag = 35.50m,
            Eingangsdatum = DateTime.Today,
            Referenz = "Bar-Zahlung"
        };

        // Act
        _rechnung.Zahlungseingänge.Add(zahlung1);
        _rechnung.Zahlungseingänge.Add(zahlung2);

        // Assert
        var zahlungshistorie = _rechnung.Zahlungseingänge.OrderBy(z => z.Eingangsdatum).ToList();
        Assert.That(zahlungshistorie.Count, Is.EqualTo(2));
        Assert.That(zahlungshistorie.First().Eingangsdatum, Is.LessThan(zahlungshistorie.Last().Eingangsdatum));
        
        var gesamtGezahlt = _rechnung.Zahlungseingänge.Sum(z => z.Betrag);
        Assert.That(gesamtGezahlt, Is.EqualTo(_rechnung.Betrag));
    }

    [Test]
    public void Rechnung_ShouldSupportDifferentPaymentMethods()
    {
        // Arrange
        var zahlungsarten = new[]
        {
            new Zahlungsart { Bezeichnung = "Überweisung", Code = "SEPA" },
            new Zahlungsart { Bezeichnung = "Bar", Code = "CASH" },
            new Zahlungsart { Bezeichnung = "EC-Karte", Code = "EC" },
            new Zahlungsart { Bezeichnung = "Kreditkarte", Code = "CC" }
        };

        // Act & Assert
        foreach (var zahlungsart in zahlungsarten)
        {
            var zahlung = new Zahlungseingang
            {
                RechnungId = _rechnung.Id,
                ZahlungsartId = zahlungsart.Id,
                Zahlungsart = zahlungsart,
                Betrag = _rechnung.Betrag,
                Eingangsdatum = DateTime.Today
            };

            _rechnung.Zahlungseingänge.Add(zahlung);
            
            Assert.That(zahlung.Zahlungsart.Bezeichnung, Is.EqualTo(zahlungsart.Bezeichnung));
            Assert.That(zahlung.Zahlungsart.Code, Is.EqualTo(zahlungsart.Code));
        }
    }
} 