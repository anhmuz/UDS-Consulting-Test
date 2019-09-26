using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTest
{
    [TestClass]
    public class WorkDayCalculatorTests
    {
        [TestMethod]
        public void TestNoWeekEnd()
        {
            DateTime startDate = new DateTime(2014, 12, 1);
            int count = 10;

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, null);

            Assert.AreEqual(startDate.AddDays(count-1), result);
        }

        [TestMethod]
        public void TestNoWeekendData()
        {
            DateTime startDate = new DateTime(2014, 12, 1);
            int count = 10;

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, new WeekEnd[0]);

            Assert.AreEqual(startDate.AddDays(count - 1), result);
        }

        [TestMethod]
        public void TestNormalPath()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 5;
            WeekEnd[] weekends = new WeekEnd[1]
            {
                new WeekEnd(new DateTime(2017, 4, 23), new DateTime(2017, 4, 25))
            }; 

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.IsTrue(result.Equals(new DateTime(2017, 4, 28)));
        }

        [TestMethod]
        public void TestWeekendAfterEnd()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 5;
            WeekEnd[] weekends = new WeekEnd[2]
            {
                new WeekEnd(new DateTime(2017, 4, 23), new DateTime(2017, 4, 25)),
                new WeekEnd(new DateTime(2017, 4, 29), new DateTime(2017, 4, 29))
            };

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.IsTrue(result.Equals(new DateTime(2017, 4, 28)));
        }

        [TestMethod]
        public void TestUnorderedWeekends()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 6;
            WeekEnd[] weekends = new []
            {
                new WeekEnd(new DateTime(2017, 4, 26), new DateTime(2017, 4, 28)),
                new WeekEnd(new DateTime(2017, 4, 23), new DateTime(2017, 4, 24))
            };

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.AreEqual(result, new DateTime(2017, 5, 1));
        }

        [TestMethod]
        public void TestImposedWeekends()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 6;
            WeekEnd[] weekends = new []
            {
                new WeekEnd(new DateTime(2017, 4, 23), new DateTime(2017, 4, 24)),
                new WeekEnd(new DateTime(2017, 4, 24), new DateTime(2017, 4, 25))
            };

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.AreEqual(result, new DateTime(2017, 4, 29));
        }

        [TestMethod]
        public void TestOneDayWeekend()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 3;
            WeekEnd[] weekends = new []
            {
                new WeekEnd(new DateTime(2017, 4, 23), new DateTime(2017, 4, 23))
            };

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.AreEqual(result, new DateTime(2017, 4, 24));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidDayCount()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 0;
            WeekEnd[] weekends = new []
            {
                new WeekEnd(new DateTime(2017, 4, 23), new DateTime(2017, 4, 25))
            };
            new WorkDayCalculator().Calculate(startDate, count, weekends);
        }

        [TestMethod]
        public void TestInvalidWeekendInterval()
        {
            DateTime startDate = new DateTime(2017, 4, 21);
            int count = 5;
            WeekEnd[] weekends = new []
            {
                new WeekEnd(new DateTime(2017, 4, 24), new DateTime(2017, 4, 23))
            };

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.AreEqual(new DateTime(2017, 4, 25), result);
        }

        [TestMethod]
        public void TestHighLoad()
        {
            DateTime startDate = new DateTime(2017, 4, 1);
            int count = 5;
            WeekEnd[] weekends = new WeekEnd[100];
            for (int i = 0; i < 100; i++)
            {
                weekends[i] = new WeekEnd(startDate.AddDays(i), startDate.AddDays(i));
            }

            DateTime result = new WorkDayCalculator().Calculate(startDate, count, weekends);

            Assert.AreEqual(new DateTime(2017, 7, 14), result);
        }
    }
}
