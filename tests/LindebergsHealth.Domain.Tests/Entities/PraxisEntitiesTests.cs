using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class PraxisEntitiesTests
    {
        [Test]
        public void NewPraxisEntity_HasValidDefaults()
        {
            var praxis = new Praxis();
            Assert.IsNotNull(praxis.Name);
            Assert.IsNotNull(praxis.Adresse);
        }
        
        private class Praxis : BaseEntity
        {
            public string Name { get; set; } = string.Empty;
            public Adresse Adresse { get; set; } = new Adresse();
        }
    }
}
