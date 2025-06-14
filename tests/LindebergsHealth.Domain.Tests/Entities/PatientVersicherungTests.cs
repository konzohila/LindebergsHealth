using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class PatientVersicherungTests
    {
        [Test]
        public void NewPatientVersicherung_HasValidDefaults()
        {
            var vers = new PatientVersicherung();
            Assert.IsFalse(vers.IsDeleted);
            Assert.IsNotNull(vers.Versicherungsname);
            Assert.IsNotNull(vers.Versicherungsnummer);
        }
    }
}
