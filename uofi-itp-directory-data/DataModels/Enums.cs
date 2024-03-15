﻿namespace uofi_itp_directory_data.DataModels {

    public enum ActivityTypeEnum {
        Publication, Presentation, Education, Award, Link, Committee
    }

    public enum AreaTypeEnum {
        Unlisted, System, Campus, College, Research, Other = 99
    }

    public enum LocationTypeEnum {
        None, Office, Remote, Other = 99
    }

    public enum LogTypeEnum {
        Unlisted, SecuritySetting, Area, Office, Employee, JobProfile, Other = 99
    }

    public enum OfficeTypeEnum {
        Unlisted, General, Information_Technology, Human_Resources, Finance, Facilities, Academic, Student_Support, Research, Business_Operations, Communications, Marketing, Advancement, Other = 99
    }

    public enum ProfileCategoryTypeEnum {
        None, Faculty, Staff, Graduate_Student, Undergraduate_Student, Post__Doctorate, Affiliate_Faculty, Affiliate_Staff, Emeritus, Other = 99
    }

    public enum ProfileDisplayEnum {
        Middle_SLASH_Default = 3, Top_of_List = 5, Bottom_of_List = 1, Not_Displayed = 0
    }
}