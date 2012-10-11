using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ILicenseplacePopup
/// </summary>
/// 
namespace TEST
{
    public interface ILicensePlatePopup
    {
        //public ILicensePlacePopup() { }

        void SelectLicensePlate(string licensePrefix, string licenseProvince, string licenseNo, int n = 0);

    }

    public class XViewModel : ILicensePlatePopup
    {
        public string LicensePlate { get; set; }

        public string LicensePlate2 { get; set; }

        public void SelectLicensePlate(string licensePrefix, string licenseProvince, string licenseNo, int n = 0)
        {
            LicensePlate = licensePrefix + " " + licenseProvince + " " + licenseNo;
        }
    }

    public class YViewModel : ILicensePlatePopup
    {
        string LicensePrefix { get; set; }
        string LicenseProvice { get; set; }
        string LicenseNo { get; set; }

        string LicensePrefix2 { get; set; }
        string LicenseProvice2 { get; set; }
        string LicenseNo2 { get; set; }

        public void SelectLicensePlate(string licensePrefix, string licenseProvince, string licenseNo, int n = 0)
        {
            LicensePrefix = licensePrefix;
            LicenseProvice = licenseProvince;
            LicenseNo = licenseNo;
        }
    }

}