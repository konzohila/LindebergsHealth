using NUnit.Framework;
using LindebergsHealth.Domain.Entities;
using System;

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
            Assert.AreEqual(0m, rechnung.Steuerbetrag);
            Assert.AreEqual(0m, rechnung.Gesamtbetrag);
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
            Assert.AreEqual(119m, rechnung.Gesamtbetrag);
        }
    }
}
