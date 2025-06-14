using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class LookupEntitiesTests
    {
        [Test]
        public void LookupEntity_Defaults_AreValid()
        {
            var lookup = new LookupEntity();
            Assert.IsNotNull(lookup.Name);
            Assert.IsFalse(lookup.IsDeleted);
        }
        
        private class Geschlecht
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = string.Empty;
            public bool IsDeleted { get; set; }
        }
    }
}
