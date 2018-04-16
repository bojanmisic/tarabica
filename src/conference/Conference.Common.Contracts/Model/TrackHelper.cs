namespace Conference.Common.Contracts.Model
{
    using System;

    public static class TrackHelper
    {
        public static string GetTitleForTrack(string code)
        {
            switch (code)
            {
                case "ALM":
                    return "Application Lifecycle Management";
                case "APM":
                    return "Agile i upravljanje projektima";
                case "CLD":
                    return "Cloud, Azure App Development";
                case "DBI":
                    return "Data Platform & Bussiness Intelligence";
                case "DEV":
                    return "Desktop Languages, Frameworks, Developer Tools";
                case "DVC":
                    return "Windows 10, Windows 10 Mobile & Mobile Services";
                case "DYN":
                    return "Dynamics";
                case "GAM":
                    return "Game Development";
                case "SES":
                    return "SharePoint, Office 365 & Enterprise Social";
                case "SRV":
                    return "Windows Server, Networks, Cloud Platform and Modern Datacenter";
                case "WEB":
                    return "Web Development";
                case "UX":
                    return "Korisničko iskustvo";
                case "MSC":
                    return "Other";
                default:
                    return string.Empty;
            }
        }

        public static string GetColorForTrack(string code)
        {
            switch (code)
            {
                case "ALM":
                    return "#304ffe";
                case "APM":
                    return "#ffc400";
                case "CLD":
                    return "#f50057";
                case "DBI":
                    return "#00e5ff";
                case "DEV":
                    return "#37474f";
                case "DVC":
                    return "#d500f9";
                case "DYN":
                    return "#ff3d00";
                case "GAM":
                    return "#00e676";
                case "SES":
                    return "#ff1744";
                case "SRV":
                    return "#651fff";
                case "WEB":
                    return "#ff9100";
                case "UX":
                    return "#1de9b6";
                case "MSC":
                    return "#ffea00";
                default:
                    return "#000000";
            }
        }

        public static string GetImageUrlForTrack(string code)
        {
            return $"/Assets/Tracks/{code}.png";
        }
    }
}
