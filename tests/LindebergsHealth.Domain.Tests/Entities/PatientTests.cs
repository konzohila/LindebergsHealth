using System;
using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

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
            Assert.That(patient.CreatedAt, Is.EqualTo(default(DateTime)));
            Assert.That(patient.ModifiedAt, Is.EqualTo(default(DateTime)));
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
