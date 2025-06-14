using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class PatientErweiterungTests
    {
        [Test]
        public void NewPatientErweiterung_HasValidDefaults()
        {
            var erw = new PatientErweiterung();
            Assert.IsNotNull(erw.Notizen);
            Assert.AreEqual(Guid.Empty, erw.PatientId);
        }
    }
}
