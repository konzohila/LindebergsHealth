using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

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
            Assert.That(erw.PatientId, Is.EqualTo(Guid.Empty));
        }
    }
}
