namespace uofi_itp_directory_data.DataModels {

    public enum ActivityTypeEnum {
        NotListed, Publication, Presentation, Education, Award, Link, Other = 99
    }

    public enum AreaTypeEnum {
        NotListed, System, Campus, College, Research, Other = 99
    }

    public enum LogTypeEnum {
        NotListed, SecuritySetting, Area, Office, Employee, JobProfile, Other = 99
    }

    public enum OfficeTypeEnum {
        NotListed, General, IT, HR, Business, Facilities, Communications, Marketing, Academic, Research, StudentSupport, Advancement, Other = 99
    }
}