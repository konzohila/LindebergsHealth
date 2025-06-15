using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class FinanzEntitiesTests
    {
        [Test]
        public void NewFinanzEntity_HasValidDefaults()
        {
            var rechnung = new Rechnung();
            Assert.IsFalse(rechnung.IsDeleted);
            Assert.IsNotNull(rechnung.Beschreibung);
            var gehalt = new Gehalt();
            Assert.That(gehalt.Zulagen, Is.EqualTo(0m));
        }
    }
}
