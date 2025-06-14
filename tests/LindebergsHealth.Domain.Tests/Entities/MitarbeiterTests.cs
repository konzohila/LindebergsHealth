using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class MitarbeiterTests
    {
        [Test]
        public void NewMitarbeiter_HasValidDefaults()
        {
            var mitarbeiter = new Mitarbeiter();
            Assert.IsFalse(mitarbeiter.IsDeleted);
            Assert.IsNotNull(mitarbeiter.Vorname);
            Assert.IsNotNull(mitarbeiter.Nachname);
            Assert.IsNotNull(mitarbeiter.Email);
        }

        [Test]
        public void Mitarbeiter_CanBeMarkedAsDeleted()
        {
            var mitarbeiter = new Mitarbeiter();
            mitarbeiter.IsDeleted = true;
            Assert.IsTrue(mitarbeiter.IsDeleted);
        }
    }
}
