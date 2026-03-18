using NUnit.Framework;
using YourCompany.YourPackage;

namespace YourCompany.YourPackage.Tests.Runtime
{
    public class YourUtilityTests
    {
        [Test]
        public void Clamp01_ValueAboveOne_ReturnsOne()
        {
            Assert.AreEqual(1f, YourUtility.Clamp01(1.5f));
        }

        [Test]
        public void Clamp01_ValueBelowZero_ReturnsZero()
        {
            Assert.AreEqual(0f, YourUtility.Clamp01(-0.5f));
        }

        [Test]
        public void Clamp01_ValueInRange_ReturnsSameValue()
        {
            Assert.AreEqual(0.5f, YourUtility.Clamp01(0.5f));
        }
    }
}
