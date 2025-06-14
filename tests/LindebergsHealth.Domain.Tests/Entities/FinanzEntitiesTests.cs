using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

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
            Assert.AreEqual(0m, gehalt.Zulagen);
        }
    }
}
