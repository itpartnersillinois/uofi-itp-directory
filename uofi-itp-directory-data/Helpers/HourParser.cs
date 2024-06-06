using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Helpers {

    public static class HourParser {

        private static readonly Dictionary<DayOfWeek, string> _dayOfWeekTranslator = new() {
            { DayOfWeek.Sunday, "Su" },
            { DayOfWeek.Monday, "M" },
            { DayOfWeek.Tuesday, "Tu" },
            { DayOfWeek.Wednesday, "W" },
            { DayOfWeek.Thursday, "Th" },
            { DayOfWeek.Friday, "F" },
            { DayOfWeek.Saturday, "Sa" }
        };

        public static string GetEmployeeHourString(this List<EmployeeHour> hours) {
            var returnValue = "";
            var comparisonValue = "";
            var startValue = "";
            var endValue = "";
            foreach (var hour in hours.OrderBy(h => h.Day))
                if (string.IsNullOrEmpty(comparisonValue)) {
                    startValue = _dayOfWeekTranslator[hour.Day];
                    endValue = _dayOfWeekTranslator[hour.Day];
                    comparisonValue = hour.OutputText;
                } else if (comparisonValue == hour.OutputText) {
                    endValue = _dayOfWeekTranslator[hour.Day];
                } else {
                    if (startValue == endValue && !string.IsNullOrWhiteSpace(comparisonValue)) {
                        returnValue += $"{startValue} {comparisonValue}; ";
                    } else if (!string.IsNullOrWhiteSpace(comparisonValue)) {
                        returnValue += $"{startValue}-{endValue} {comparisonValue}; ";
                    }
                    startValue = _dayOfWeekTranslator[hour.Day];
                    endValue = _dayOfWeekTranslator[hour.Day];
                    comparisonValue = hour.OutputText;
                }
            if (startValue == endValue && !string.IsNullOrWhiteSpace(comparisonValue)) {
                returnValue += $"{startValue} {comparisonValue}.";
            } else if (!string.IsNullOrWhiteSpace(comparisonValue)) {
                returnValue += $"{startValue}-{endValue} {comparisonValue}.";
            }
            return returnValue.TrimEnd([' ', ';']);
        }

        public static string GetOfficeHourString(this List<OfficeHour> hours, string overrideTextString, bool includeHoliday) {
            if (!string.IsNullOrWhiteSpace(overrideTextString)) {
                return overrideTextString;
            }
            var returnValue = "";
            var comparisonValue = "";
            var startValue = "";
            var endValue = "";
            foreach (var hour in hours.OrderBy(h => h.Day))
                if (string.IsNullOrEmpty(comparisonValue)) {
                    startValue = _dayOfWeekTranslator[hour.Day];
                    endValue = _dayOfWeekTranslator[hour.Day];
                    comparisonValue = hour.OutputText;
                } else if (comparisonValue == hour.OutputText) {
                    endValue = _dayOfWeekTranslator[hour.Day];
                } else {
                    if (startValue == endValue && !string.IsNullOrWhiteSpace(comparisonValue)) {
                        returnValue += $"{startValue} {comparisonValue}; ";
                    } else if (!string.IsNullOrWhiteSpace(comparisonValue)) {
                        returnValue += $"{startValue}-{endValue} {comparisonValue}; ";
                    }
                    startValue = _dayOfWeekTranslator[hour.Day];
                    endValue = _dayOfWeekTranslator[hour.Day];
                    comparisonValue = hour.OutputText;
                }
            if (startValue == endValue && !string.IsNullOrWhiteSpace(comparisonValue)) {
                returnValue += $"{startValue} {comparisonValue}.";
            } else if (!string.IsNullOrWhiteSpace(comparisonValue)) {
                returnValue += $"{startValue}-{endValue} {comparisonValue}.";
            }
            if (includeHoliday) {
                returnValue += " Closed on University Holidays.";
            }

            return returnValue.TrimEnd([' ', ';']);
        }
    }
}