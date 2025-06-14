using System;
using LindebergsHealth.Domain.Entities;
using NUnit.Framework;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void NewBaseEntity_HasValidDefaults()
        {
            var entity = new TestEntity();
            Assert.That(entity.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(entity.IstGelöscht, Is.EqualTo(false));
            Assert.IsNull(entity.GelöschtAm);
            Assert.IsNull(entity.GelöschtVon);
            Assert.IsNull(entity.LöschGrund);
            Assert.IsNotNull(entity.RowVersion);
        }

        [Test]
        public void SoftDelete_SetsProperFields()
        {
            var entity = new TestEntity();
            var userId = Guid.NewGuid();
            entity.SoftDelete(userId, "Test");
            Assert.IsTrue(entity.IstGelöscht);
            Assert.IsNotNull(entity.GelöschtAm);
            Assert.That(entity.GelöschtVon, Is.EqualTo(userId));
            Assert.That(entity.LöschGrund, Is.EqualTo("Test"));
        }

        [Test]
        public void MarkAsModified_SetsFields()
        {
            var entity = new TestEntity();
            var userId = Guid.NewGuid();
            entity.MarkAsModified(userId);
            Assert.IsNotNull(entity.GeändertAm);
            Assert.That(entity.GeändertVon, Is.EqualTo(userId));
        }

        private class TestEntity : BaseEntity { }
    }
}
