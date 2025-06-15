using System;
using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class KontaktTests
    {
        [Test]
        public void NewKontakt_HasValidDefaults()
        {
            var kontakt = new Kontakt();
            Assert.IsFalse(kontakt.IsDeleted);
            Assert.IsEmpty(kontakt.Wert);
            Assert.IsFalse(kontakt.IstHauptkontakt);
        }
    }
}
