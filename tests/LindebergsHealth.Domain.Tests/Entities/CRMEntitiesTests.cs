using NUnit.Framework;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class CRMEntitiesTests
    {
        [Test]
        public void NewCRMEntity_HasValidDefaults()
        {
            var dokument = new Dokument();
            Assert.IsNotNull(dokument.Titel);
            Assert.IsNotNull(dokument.Dateipfad);
        }
        
        private class Dokument : BaseEntity
        {
            public string Titel { get; set; } = string.Empty;
            public string Dateipfad { get; set; } = string.Empty;
        }
    }
}
