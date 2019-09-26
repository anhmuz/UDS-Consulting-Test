using System;
using System.Collections.Generic;

namespace CSharpTest
{
    public class WorkDayCalculator : IWorkDayCalculator
    {
        private HashSet<DateTime> MakeWeekendsHashSet(WeekEnd[] weekEnds)
        {
            var result = new HashSet<DateTime>();
            foreach (WeekEnd weekEnd in weekEnds)
            {
                if  (weekEnd.EndDate < weekEnd.StartDate)
                {
                    continue;
                }
                var currentDay = weekEnd.StartDate;
                while (currentDay <= weekEnd.EndDate)
                {
                    result.Add(currentDay);
                    currentDay = currentDay.AddDays(1);
                }
            }
            return result;
        }

        public DateTime Calculate(DateTime startDate, int dayCount, WeekEnd[] weekEnds)
        {
            if (dayCount <= 0)
            {
                throw new ArgumentException(string.Format("Invalid day count: {0}", dayCount));
            }
            if (weekEnds == null || weekEnds.Length == 0)
            {
                return startDate.AddDays(dayCount - 1);
            }

            var weekendsHashSet = MakeWeekendsHashSet(weekEnds);

            var currentDay = startDate;
            var count = 0;
            while (true)
            {
                if (!weekendsHashSet.Contains(currentDay))
                {
                    ++count;
                    if (count == dayCount)
                    {
                        break;
                    }
                }
                currentDay = currentDay.AddDays(1);
            }
            return currentDay;
        }
    }
}
