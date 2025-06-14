using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Gehalt Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class GehaltTests
{
    private Gehalt _gehalt;
    private Mitarbeiter _mitarbeiter;

    [SetUp]
    public void Setup()
    {
        _mitarbeiter = new Mitarbeiter
        {
            Id = Guid.NewGuid(),
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        _gehalt = new Gehalt
        {
            MitarbeiterId = _mitarbeiter.Id,
            Mitarbeiter = _mitarbeiter,
            GueltigAb = new DateTime(2024, 1, 1),
            Grundgehalt = 4500.00m,
            Zulagen = 300.00m,
            Stundenlohn = 0.00m, // Festanstellung
            IstStundenlohn = false,
            Sozialversicherungspflichtig = true,
            Steuerklasse = 1
        };
    }

    [Test]
    public void Gehalt_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_gehalt, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Gehalt_ShouldHaveValidSalaryData()
    {
        // Assert
        Assert.That(_gehalt.MitarbeiterId, Is.EqualTo(_mitarbeiter.Id));
        Assert.That(_gehalt.Mitarbeiter, Is.EqualTo(_mitarbeiter));
        Assert.That(_gehalt.GueltigAb, Is.EqualTo(new DateTime(2024, 1, 1)));
        Assert.That(_gehalt.Grundgehalt, Is.EqualTo(4500.00m));
        Assert.That(_gehalt.Zulagen, Is.EqualTo(300.00m));
        Assert.That(_gehalt.IstStundenlohn, Is.False);
        Assert.That(_gehalt.Sozialversicherungspflichtig, Is.True);
        Assert.That(_gehalt.Steuerklasse, Is.EqualTo(1));
    }

    [Test]
    public void Gehalt_ShouldCalculateTotalSalary()
    {
        // Arrange
        _gehalt.Grundgehalt = 4500.00m;
        _gehalt.Zulagen = 300.00m;
        _gehalt.Provisionen = 150.00m;
        _gehalt.Sonderzahlungen = 200.00m;

        // Act
        var bruttogehalt = _gehalt.Grundgehalt + _gehalt.Zulagen + _gehalt.Provisionen + _gehalt.Sonderzahlungen;

        // Assert
        Assert.That(bruttogehalt, Is.EqualTo(5150.00m));
    }

    [Test]
    public void Gehalt_ShouldSupportHourlyWage()
    {
        // Arrange - Stundenlohn-Mitarbeiter
        var stundenlohnGehalt = new Gehalt
        {
            MitarbeiterId = _mitarbeiter.Id,
            Stundenlohn = 25.50m,
            IstStundenlohn = true,
            Sozialversicherungspflichtig = true
        };

        // Act
        var wochenStunden = 40;
        var wochenLohn = stundenlohnGehalt.Stundenlohn * wochenStunden;
        var monatsLohn = wochenLohn * 4.33m; // Durchschnittliche Wochen pro Monat

        // Assert
        Assert.That(stundenlohnGehalt.IstStundenlohn, Is.True);
        Assert.That(stundenlohnGehalt.Stundenlohn, Is.EqualTo(25.50m));
        Assert.That(wochenLohn, Is.EqualTo(1020.00m));
        Assert.That(monatsLohn, Is.EqualTo(4416.60m).Within(0.01m));
    }

    [Test]
    public void Gehalt_ShouldValidateTaxClasses()
    {
        // Arrange - Deutsche Steuerklassen
        var validTaxClasses = new[] { 1, 2, 3, 4, 5, 6 };

        // Act & Assert
        foreach (var taxClass in validTaxClasses)
        {
            _gehalt.Steuerklasse = taxClass;
            Assert.That(_gehalt.Steuerklasse, Is.EqualTo(taxClass));
            Assert.That(_gehalt.Steuerklasse, Is.InRange(1, 6));
        }
    }

    [Test]
    public void Gehalt_ShouldCalculateGrossSalary()
    {
        // Arrange - Verschiedene Gehaltskomponenten
        _gehalt.Grundgehalt = 4000.00m;
        _gehalt.Zulagen = 500.00m; // Schichtzulage, Weihnachtsgeld anteilig
        _gehalt.Provisionen = 200.00m; // Leistungsprämie
        _gehalt.Sonderzahlungen = 100.00m; // Überstundenvergütung
        _gehalt.Sachbezuege = 50.00m; // Firmenwagen, Handy

        // Act
        var bruttogehalt = _gehalt.Grundgehalt + _gehalt.Zulagen + _gehalt.Provisionen + 
                          _gehalt.Sonderzahlungen + _gehalt.Sachbezuege;

        // Assert
        Assert.That(bruttogehalt, Is.EqualTo(4850.00m));
    }

    [Test]
    public void Gehalt_ShouldCalculateNetSalary()
    {
        // Arrange - Bruttolohn und Abzüge
        var bruttolohn = 4850.00m;
        var steuerklasse = 1;
        var kinderfreibetraege = 0;

        // Vereinfachte Berechnung (echte Berechnung wäre komplexer)
        var lohnsteuer = bruttolohn * 0.14m; // ca. 14% Lohnsteuer
        var solidaritaetszuschlag = lohnsteuer * 0.055m; // 5,5% Soli
        var kirchensteuer = lohnsteuer * 0.08m; // 8% Kirchensteuer (Bayern)
        
        var rentenversicherung = bruttolohn * 0.093m; // 9,3% RV
        var arbeitslosenversicherung = bruttolohn * 0.012m; // 1,2% ALV
        var krankenversicherung = bruttolohn * 0.073m; // 7,3% KV
        var pflegeversicherung = bruttolohn * 0.01525m; // 1,525% PV

        var gesamtAbzuege = lohnsteuer + solidaritaetszuschlag + kirchensteuer +
                           rentenversicherung + arbeitslosenversicherung + 
                           krankenversicherung + pflegeversicherung;

        var nettolohn = bruttolohn - gesamtAbzuege;

        // Act
        _gehalt.Bruttolohn = bruttolohn;
        _gehalt.Lohnsteuer = lohnsteuer;
        _gehalt.Solidaritaetszuschlag = solidaritaetszuschlag;
        _gehalt.Kirchensteuer = kirchensteuer;
        _gehalt.Rentenversicherung = rentenversicherung;
        _gehalt.Arbeitslosenversicherung = arbeitslosenversicherung;
        _gehalt.Krankenversicherung = krankenversicherung;
        _gehalt.Pflegeversicherung = pflegeversicherung;
        _gehalt.Nettolohn = nettolohn;

        // Assert
        Assert.That(_gehalt.Bruttolohn, Is.EqualTo(4850.00m));
        Assert.That(_gehalt.Nettolohn, Is.LessThan(_gehalt.Bruttolohn));
        Assert.That(_gehalt.Nettolohn, Is.GreaterThan(3000.00m)); // Plausibilitätsprüfung
        
        var abzugsquote = (gesamtAbzuege / bruttolohn) * 100;
        Assert.That(abzugsquote, Is.InRange(35, 45)); // Typische Abzugsquote in Deutschland
    }

    [Test]
    public void Gehalt_ShouldSupportSalaryHistory()
    {
        // Arrange - Gehaltshistorie
        var gehaltshistorie = new[]
        {
            new Gehalt
            {
                MitarbeiterId = _mitarbeiter.Id,
                GueltigAb = new DateTime(2022, 1, 1),
                Grundgehalt = 4000.00m,
                Zulagen = 200.00m
            },
            new Gehalt
            {
                MitarbeiterId = _mitarbeiter.Id,
                GueltigAb = new DateTime(2023, 1, 1),
                Grundgehalt = 4200.00m,
                Zulagen = 250.00m
            },
            new Gehalt
            {
                MitarbeiterId = _mitarbeiter.Id,
                GueltigAb = new DateTime(2024, 1, 1),
                Grundgehalt = 4500.00m,
                Zulagen = 300.00m
            }
        };

        // Act
        foreach (var gehalt in gehaltshistorie)
        {
            _mitarbeiter.Gehaelter.Add(gehalt);
        }

        // Assert
        var sortierteHistorie = _mitarbeiter.Gehaelter.OrderBy(g => g.GueltigAb).ToList();
        Assert.That(sortierteHistorie.Count, Is.EqualTo(3));
        
        // Gehaltsentwicklung prüfen
        var entwicklung2023 = ((sortierteHistorie[1].Grundgehalt - sortierteHistorie[0].Grundgehalt) / 
                              sortierteHistorie[0].Grundgehalt) * 100;
        var entwicklung2024 = ((sortierteHistorie[2].Grundgehalt - sortierteHistorie[1].Grundgehalt) / 
                              sortierteHistorie[1].Grundgehalt) * 100;
        
        Assert.That(entwicklung2023, Is.EqualTo(5.0m).Within(0.1m)); // 5% Erhöhung
        Assert.That(entwicklung2024, Is.EqualTo(7.14m).Within(0.1m)); // ~7% Erhöhung
    }

    [Test]
    public void Gehalt_ShouldSupportBonusCalculation()
    {
        // Arrange - Bonusberechnung
        var jahresgrundgehalt = _gehalt.Grundgehalt * 12;
        var leistungsbonus = 0.10m; // 10% Leistungsbonus
        var weihnachtsgeld = _gehalt.Grundgehalt * 0.5m; // Halbes Monatsgehalt
        var urlaubsgeld = _gehalt.Grundgehalt * 0.5m; // Halbes Monatsgehalt

        // Act
        var jahresbonus = jahresgrundgehalt * leistungsbonus;
        var gesamtSonderzahlungen = jahresbonus + weihnachtsgeld + urlaubsgeld;
        var jahresGesamtVergütung = jahresgrundgehalt + gesamtSonderzahlungen;

        // Assert
        Assert.That(jahresbonus, Is.EqualTo(5400.00m)); // 10% von 54.000€
        Assert.That(weihnachtsgeld, Is.EqualTo(2250.00m));
        Assert.That(urlaubsgeld, Is.EqualTo(2250.00m));
        Assert.That(gesamtSonderzahlungen, Is.EqualTo(9900.00m));
        Assert.That(jahresGesamtVergütung, Is.EqualTo(63900.00m));

        // Dokumentation
        var bonusBerechnung = $@"
BONUSBERECHNUNG {DateTime.Now.Year}:
Jahresgrundgehalt: {jahresgrundgehalt:C}
Leistungsbonus (10%): {jahresbonus:C}
Weihnachtsgeld: {weihnachtsgeld:C}
Urlaubsgeld: {urlaubsgeld:C}
Gesamt Sonderzahlungen: {gesamtSonderzahlungen:C}
JAHRESGESAMTVERGÜTUNG: {jahresGesamtVergütung:C}";

        _gehalt.BonusBerechnung = bonusBerechnung;
        Assert.That(_gehalt.BonusBerechnung, Contains.Substring("BONUSBERECHNUNG"));
    }

    [Test]
    public void Gehalt_ShouldSupportOvertimeCalculation()
    {
        // Arrange - Überstundenberechnung
        var normalStunden = 40; // Wochenstunden
        var geleisteteStunden = 45; // Tatsächlich gearbeitete Stunden
        var überstunden = geleisteteStunden - normalStunden;
        var stundenlohn = _gehalt.Grundgehalt / (normalStunden * 4.33m); // Monatlicher Stundenlohn
        var überstundenZuschlag = 0.25m; // 25% Zuschlag
        var überstundenVergütung = überstunden * stundenlohn * (1 + überstundenZuschlag);

        // Act
        _gehalt.Ueberstunden = überstunden;
        _gehalt.UeberstundenVerguetung = überstundenVergütung;

        // Assert
        Assert.That(_gehalt.Ueberstunden, Is.EqualTo(5));
        Assert.That(stundenlohn, Is.EqualTo(25.98m).Within(0.01m));
        Assert.That(_gehalt.UeberstundenVerguetung, Is.EqualTo(162.38m).Within(0.01m));
    }

    [Test]
    public void Gehalt_ShouldSupportEmployerCosts()
    {
        // Arrange - Arbeitgeberkosten
        var bruttolohn = 4850.00m;
        
        // Arbeitgeberanteile Sozialversicherung
        var agAnteilRV = bruttolohn * 0.093m; // 9,3% Rentenversicherung
        var agAnteilALV = bruttolohn * 0.012m; // 1,2% Arbeitslosenversicherung
        var agAnteilKV = bruttolohn * 0.073m; // 7,3% Krankenversicherung
        var agAnteilPV = bruttolohn * 0.01525m; // 1,525% Pflegeversicherung
        var unfallversicherung = bruttolohn * 0.017m; // 1,7% Unfallversicherung
        var insolvenzgeld = bruttolohn * 0.0006m; // 0,06% Insolvenzgeld

        var gesamtArbeitgeberkosten = bruttolohn + agAnteilRV + agAnteilALV + 
                                     agAnteilKV + agAnteilPV + unfallversicherung + insolvenzgeld;

        // Act
        _gehalt.ArbeitgeberAnteilRV = agAnteilRV;
        _gehalt.ArbeitgeberAnteilALV = agAnteilALV;
        _gehalt.ArbeitgeberAnteilKV = agAnteilKV;
        _gehalt.ArbeitgeberAnteilPV = agAnteilPV;
        _gehalt.Unfallversicherung = unfallversicherung;
        _gehalt.Insolvenzgeld = insolvenzgeld;
        _gehalt.GesamtArbeitgeberkosten = gesamtArbeitgeberkosten;

        // Assert
        Assert.That(_gehalt.GesamtArbeitgeberkosten, Is.GreaterThan(bruttolohn));
        
        var arbeitgeberkostenFaktor = _gehalt.GesamtArbeitgeberkosten / bruttolohn;
        Assert.That(arbeitgeberkostenFaktor, Is.InRange(1.20m, 1.25m)); // Typisch 20-25% Aufschlag
    }

    [Test]
    public void Gehalt_ShouldSupportPayrollExport()
    {
        // Arrange - Lohnbuchhaltungsexport
        var lohnbuchhaltungsDaten = @"
LOHNBUCHHALTUNG EXPORT:
Personalnummer: MA001
Name: Schmidt, Dr. Anna
Abrechnungsmonat: März 2024
Steuerklasse: 1
Kinderfreibeträge: 0

BEZÜGE:
Grundgehalt: 4.500,00 €
Zulagen: 300,00 €
Provisionen: 150,00 €
Sonderzahlungen: 200,00 €
Sachbezüge: 50,00 €
BRUTTO GESAMT: 5.200,00 €

ABZÜGE:
Lohnsteuer: 728,00 €
Solidaritätszuschlag: 40,04 €
Kirchensteuer: 58,24 €
Rentenversicherung: 483,60 €
Arbeitslosenversicherung: 62,40 €
Krankenversicherung: 379,60 €
Pflegeversicherung: 79,30 €
ABZÜGE GESAMT: 1.831,18 €

NETTO AUSZAHLUNG: 3.368,82 €";

        // Act
        _gehalt.LohnbuchhaltungsExport = lohnbuchhaltungsDaten;

        // Assert
        Assert.That(_gehalt.LohnbuchhaltungsExport, Contains.Substring("LOHNBUCHHALTUNG EXPORT"));
        Assert.That(_gehalt.LohnbuchhaltungsExport, Contains.Substring("BEZÜGE"));
        Assert.That(_gehalt.LohnbuchhaltungsExport, Contains.Substring("ABZÜGE"));
        Assert.That(_gehalt.LohnbuchhaltungsExport, Contains.Substring("NETTO AUSZAHLUNG"));
    }

    [Test]
    public void Gehalt_ShouldValidateMinimumWage()
    {
        // Arrange - Mindestlohn Deutschland 2024
        var mindestlohn = 12.41m; // Euro pro Stunde (Stand 2024)
        var wochenStunden = 40;
        var monatsStunden = wochenStunden * 4.33m;
        var mindestMonatsLohn = mindestlohn * monatsStunden;

        // Act
        var istMindestlohnEingehalten = _gehalt.IstStundenlohn ? 
            _gehalt.Stundenlohn >= mindestlohn :
            _gehalt.Grundgehalt >= mindestMonatsLohn;

        // Assert
        Assert.That(mindestMonatsLohn, Is.EqualTo(2151.89m).Within(0.01m));
        Assert.That(_gehalt.Grundgehalt, Is.GreaterThan(mindestMonatsLohn));
        Assert.That(istMindestlohnEingehalten, Is.True);
    }

    [Test]
    public void Gehalt_ShouldSupportVacationPayCalculation()
    {
        // Arrange - Urlaubsgeldberechnung
        var jahresUrlaub = 30; // Tage
        var arbeitsTageProJahr = 220; // 5 Tage/Woche * 44 Wochen
        var tagesLohn = (_gehalt.Grundgehalt * 12) / arbeitsTageProJahr;
        var urlaubsGeld = tagesLohn * jahresUrlaub;

        // Act
        _gehalt.UrlaubsTage = jahresUrlaub;
        _gehalt.TagesLohn = tagesLohn;
        _gehalt.UrlaubsGeld = urlaubsGeld;

        // Assert
        Assert.That(_gehalt.UrlaubsTage, Is.EqualTo(30));
        Assert.That(_gehalt.TagesLohn, Is.EqualTo(245.45m).Within(0.01m));
        Assert.That(_gehalt.UrlaubsGeld, Is.EqualTo(7363.64m).Within(0.01m));
    }

    [Test]
    public void Gehalt_ShouldSupportSickPayCalculation()
    {
        // Arrange - Krankengeldberechnung
        var krankheitsTage = 5;
        var lohnfortzahlungTage = Math.Min(krankheitsTage, 42); // Max. 6 Wochen Lohnfortzahlung
        var tagesLohn = _gehalt.Grundgehalt / 30; // Vereinfacht: Monat = 30 Tage
        var lohnfortzahlung = lohnfortzahlungTage * tagesLohn;

        // Act
        _gehalt.KrankheitsTage = krankheitsTage;
        _gehalt.LohnfortzahlungsTage = lohnfortzahlungTage;
        _gehalt.Lohnfortzahlung = lohnfortzahlung;

        // Assert
        Assert.That(_gehalt.KrankheitsTage, Is.EqualTo(5));
        Assert.That(_gehalt.LohnfortzahlungsTage, Is.EqualTo(5));
        Assert.That(_gehalt.Lohnfortzahlung, Is.EqualTo(750.00m));
    }
} 