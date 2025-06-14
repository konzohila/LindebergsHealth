using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

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
            Assert.AreEqual("Deutschland", adresse.Land);
            Assert.IsEmpty(adresse.Strasse);
            Assert.IsEmpty(adresse.Hausnummer);
        }
    }
}
