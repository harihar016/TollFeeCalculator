using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TollFeeCalculator.Tests
{
    [TestClass]
    public class TollCalculatorTests
    {
        [TestMethod]
        public void GetTollFeeForNoTollFreeVehicle()
        {
            DateTime[] date = { new DateTime(2018, 08, 17, 6, 25, 0), new DateTime(2018, 08, 17, 7, 40, 0), new DateTime(2018, 08, 17, 15, 40, 0), new DateTime(2018, 08, 17, 15, 55, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 44);
        }

        [TestMethod]
        public void GetZeroTollFeeForTollFreeVehicle()
        {
            DateTime[] date = { new DateTime(2018, 08, 17, 6, 25, 0), new DateTime(2018, 08, 17, 7, 40, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Motorbike(), date);
            Assert.AreEqual(tollFee, 0);
        }

        [TestMethod]
        public void GetMaxTollFee()
        {
            DateTime[] date = { new DateTime(2018, 08, 17, 6, 25, 0), new DateTime(2018, 08, 17, 7, 40, 0), new DateTime(2018, 08, 17, 8, 55, 0), new DateTime(2018, 08, 17, 15, 40, 0), new DateTime(2018, 08, 17, 17, 40, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 60);
        }

        [TestMethod]
        public void GetZeroTollFeeForTollFreeDateTime()
        {
            DateTime[] date = { new DateTime(2018, 08, 17, 19, 25, 0), new DateTime(2018, 08, 17, 20, 40, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 0);
        }

        [TestMethod]
        public void GetZeroTollFeeForTollFreeDate()
        {
            DateTime[] date = { new DateTime(2018, 11, 1, 19, 25, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 0);
        }

        [TestMethod]
        public void GetZeroTollFeeForHoliday()
        {
            DateTime[] date = { new DateTime(2018, 08, 18, 19, 25, 0) };
            TollCalculator toll = new TollCalculator();
            int tollFee = toll.GetTollFee(new Car(), date);
            Assert.AreEqual(tollFee, 0);
        }
    }
}
