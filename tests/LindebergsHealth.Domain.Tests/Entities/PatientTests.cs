using NUnit.Framework;
using LindebergsHealth.Domain.Entities;
using System;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class PatientTests
    {
        [Test]
        public void NewPatient_HasValidDefaults()
        {
            var patient = new Patient();
            Assert.IsFalse(patient.IsDeleted);
            Assert.IsNotNull(patient.Versicherungsnummer);
            Assert.IsNotNull(patient.Email);
            Assert.AreEqual(default(DateTime), patient.CreatedAt);
            Assert.AreEqual(default(DateTime), patient.ModifiedAt);
        }

        [Test]
        public void Patient_CanBeMarkedAsDeleted()
        {
            var patient = new Patient();
            patient.IsDeleted = true;
            Assert.IsTrue(patient.IsDeleted);
        }

        [Test]
        public void Patient_EmailValidation()
        {
            var patient = new Patient { Email = "test@example.com" };
            Assert.That(patient.Email, Does.Contain("@"));
        }
    }
}
