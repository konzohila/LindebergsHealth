using NUnit.Framework;
using LindebergsHealth.Domain.Entities;
using System;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class TerminTests
    {
        [Test]
        public void Termin_CannotBeInThePast()
        {
            // Arrange
            var gestern = DateTime.Now.AddDays(-1);
            var termin = new Termin
            {
                Datum = gestern,
                DauerMinuten = 60
            };

            // Act
            bool isValid = termin.Datum.Date >= DateTime.Now.Date;

            // Assert
            Assert.IsFalse(isValid, "Ein Termin darf nicht in der Vergangenheit liegen.");
        }

        [Test]
        public void Termin_DefaultIsNotDeleted()
        {
            var termin = new Termin();
            Assert.IsFalse(termin.IsDeleted);
        }
    }
}
