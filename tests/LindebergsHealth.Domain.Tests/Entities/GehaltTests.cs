using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class GehaltTests
    {
        [Test]
        public void NewGehalt_HasValidDefaults()
        {
            var gehalt = new Gehalt();
            Assert.AreEqual(0m, gehalt.Zulagen);
            Assert.AreEqual(0m, gehalt.Abzuege);
            Assert.AreEqual(0m, gehalt.Nettogehalt);
            Assert.AreEqual(0m, gehalt.Steuern);
            Assert.AreEqual(0m, gehalt.Sozialversicherung);
        }

        [Test]
        public void Gehalt_NettogehaltKorrektBerechnet()
        {
            var gehalt = new Gehalt
            {
                Zulagen = 200m,
                Abzuege = 50m,
                Steuern = 30m,
                Sozialversicherung = 20m
            };
            gehalt.Nettogehalt = gehalt.Zulagen - gehalt.Abzuege - gehalt.Steuern - gehalt.Sozialversicherung;
            Assert.AreEqual(100m, gehalt.Nettogehalt);
        }
    }
}
