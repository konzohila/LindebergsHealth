using NUnit.Framework;
using LindebergsHealth.Domain.Entities;
using System;

namespace LindebergsHealth.Domain.Tests.Entities
{
    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void NewBaseEntity_HasValidDefaults()
        {
            var entity = new TestEntity();
            Assert.AreNotEqual(Guid.Empty, entity.Id);
            Assert.AreEqual(false, entity.IstGelöscht);
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
            Assert.AreEqual(userId, entity.GelöschtVon);
            Assert.AreEqual("Test", entity.LöschGrund);
        }

        [Test]
        public void MarkAsModified_SetsFields()
        {
            var entity = new TestEntity();
            var userId = Guid.NewGuid();
            entity.MarkAsModified(userId);
            Assert.IsNotNull(entity.GeändertAm);
            Assert.AreEqual(userId, entity.GeändertVon);
        }

        private class TestEntity : BaseEntity { }
    }
}
