using System;
using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class RechnungTests
    {
        [Test]
        public void NewRechnung_HasValidDefaults()
        {
            var rechnung = new Rechnung();
            Assert.IsFalse(rechnung.IsDeleted);
            Assert.IsNotNull(rechnung.Beschreibung);
            Assert.That(rechnung.Steuerbetrag, Is.EqualTo(0m));
            Assert.That(rechnung.Gesamtbetrag, Is.EqualTo(0m));
        }

        [Test]
        public void Rechnung_BetragUndSteuer_KorrekteBerechnung()
        {
            var rechnung = new Rechnung
            {
                Betrag = 100m,
                Steuerbetrag = 19m
            };
            rechnung.Gesamtbetrag = rechnung.Betrag + rechnung.Steuerbetrag;
            Assert.That(rechnung.Gesamtbetrag, Is.EqualTo(119m));
        }
    }
}
