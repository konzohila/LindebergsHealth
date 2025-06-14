using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class TherapieEntitiesTests
    {
        [Test]
        public void NewTherapieEntity_HasValidDefaults()
        {
            var therapie = new Therapie();
            Assert.IsNotNull(therapie.Name);
            Assert.IsNotNull(therapie.Beschreibung);
        }
        
        private class Therapie : BaseEntity
        {
            public string Name { get; set; } = string.Empty;
            public string Beschreibung { get; set; } = string.Empty;
        }
    }
}
