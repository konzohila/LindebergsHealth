using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class GehaltTests
    {
        [Test]
        public void NewGehalt_HasValidDefaults()
        {
            var gehalt = new Gehalt();
            Assert.That(gehalt.Zulagen, Is.EqualTo(0m));
            Assert.That(gehalt.Abzuege, Is.EqualTo(0m));
            Assert.That(gehalt.Nettogehalt, Is.EqualTo(0m));
            Assert.That(gehalt.Steuern, Is.EqualTo(0m));
            Assert.That(gehalt.Sozialversicherung, Is.EqualTo(0m));
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
            Assert.That(gehalt.Nettogehalt, Is.EqualTo(100m));
        }
    }
}
