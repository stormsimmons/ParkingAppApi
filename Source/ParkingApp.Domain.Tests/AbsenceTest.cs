using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.Domain.Enums;

namespace ParkingApp.Domain.Tests
{
    [TestClass]
    public class AbsenceTest
    {
        private DateTime _searchPeriodStartDate;
        private DateTime _searchPeriodEndDateToday;
        private DateTime _searchPeriodEndDateNextWeek;

        [TestInitialize]
        public void Initialize()
        {
            _searchPeriodStartDate = new DateTime(2017, 7, 21);

            _searchPeriodEndDateToday = _searchPeriodStartDate.AddDays((int)SearchDateCriteria.Today);
            _searchPeriodEndDateNextWeek = _searchPeriodStartDate.AddDays((int)SearchDateCriteria.Week);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceStartDateIsToday_ReturnsTrue()
        {
            Absence absenceToday = new Absence() { StartDate = new DateTime(2017, 7, 21), EndDate = new DateTime(2017, 7, 27), ReservedBy = null };

            Assert.IsTrue(absenceToday.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateToday));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceStartDateIsNotToday_ReturnsFalse()
        {
            Absence absenceToday = new Absence() { StartDate = new DateTime(2017, 7, 22), EndDate = new DateTime(2017, 7, 27), ReservedBy = null };

            Assert.IsFalse(absenceToday.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateToday));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceIsInNextWeekSearchPeriod_ReturnsTrue()
        {
            Absence absenceInNextWeek = new Absence() { StartDate = new DateTime(2017, 7, 24), EndDate = new DateTime(2017, 7, 27), ReservedBy = null };

            Assert.IsTrue(absenceInNextWeek.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateNextWeek));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceIsAfterNextWeekSearchPeriod_ReturnsFalse()
        {
            Absence absenceInNextWeek = new Absence() { StartDate = new DateTime(2017, 7, 30), EndDate = new DateTime(2017, 7, 31), ReservedBy = null };

            Assert.IsFalse(absenceInNextWeek.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateNextWeek));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceIsBeforeNextWeekSearchPeriod_ReturnsFalse()
        {
            Absence absenceInNextWeek = new Absence() { StartDate = new DateTime(2017, 7, 13), EndDate = new DateTime(2017, 7, 20), ReservedBy = null };

            Assert.IsFalse(absenceInNextWeek.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateNextWeek));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceStartsBeforeNextWeekSearchPeriodAndEndsWithinPeriod_ReturnsTrue()
        {
            Absence absenceInNextWeek = new Absence() { StartDate = new DateTime(2017, 7, 13), EndDate = new DateTime(2017, 7, 24), ReservedBy = null };

            Assert.IsTrue(absenceInNextWeek.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateNextWeek));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceStartsWithinNextWeekSearchPeriodAndEndsAfterPeriod_ReturnsTrue()
        {
            Absence absenceInNextWeek = new Absence() { StartDate = new DateTime(2017, 7, 26), EndDate = new DateTime(2017, 8, 4), ReservedBy = null };

            Assert.IsTrue(absenceInNextWeek.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateNextWeek));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void IsValidFor_AbsenceSpansEntireNextWeekSearchPeriod_ReturnsTrue()
        {
            Absence absenceInNextWeek = new Absence() { StartDate = new DateTime(2017, 7, 13), EndDate = new DateTime(2017, 8, 4), ReservedBy = null };

            Assert.IsTrue(absenceInNextWeek.IsValidFor(_searchPeriodStartDate, _searchPeriodEndDateNextWeek));
        }
    }
}
