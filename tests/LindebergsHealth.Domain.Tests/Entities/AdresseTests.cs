using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class AdresseTests
    {
        [Test]
        public void NewAdresse_HasValidDefaults()
        {
            var adresse = new Adresse();
            Assert.IsFalse(adresse.IsDeleted);
            Assert.That(adresse.Land, Is.EqualTo("Deutschland"));
            Assert.IsEmpty(adresse.Strasse);
            Assert.IsEmpty(adresse.Hausnummer);
        }
    }
}
