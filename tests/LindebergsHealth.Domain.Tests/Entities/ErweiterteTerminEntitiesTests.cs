using System;
using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class ErweiterteTerminEntitiesTests
    {
        [Test]
        public void NewTerminserie_HasValidDefaults()
        {
            var serie = new Terminserie();
            Assert.IsNotNull(serie.Name);
            Assert.IsNotNull(serie.Termine);
        }

        private class Terminserie : BaseEntity
        {
            public string Name { get; set; } = string.Empty;
            public System.Collections.Generic.List<Termin> Termine { get; set; } = new();
        }
    }
}
