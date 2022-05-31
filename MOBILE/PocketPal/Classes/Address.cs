namespace POCKETPAL
{
    public class Address
    {
        //public static string _API       = "https://sbm-apps.dev/POCKETPAL-API";
        //public static string _NEWS      = "https://sbm-apps.dev/POCKETPAL-API";
        //public static string _VMS       = "https://sbm-apps.shimano.id/WEBVISIBUKKUPRD";
        //public static string _EAPPROVAL = "https://sbm-apps.shimano.id/WEBEAPPROVALPRD";
        //public static string _CKS       = "https://sbm-apps.shimano.id/CKS";
        public static string _API = "https://sbm-apps.shimano.id/POCKETPAL-API";
        public static string _NEWS = "https://sbm-apps.shimano.id/POCKETPAL-API";
        public static string _VMS = "https://sbm-apps.shimano.id/WEBVISIBUKKUPRD";
        public static string _EAPPROVAL = "https://sbm-apps.shimano.id/WEBEAPPROVALPRD";
        public static string _CKS = "https://sbm-apps.shimano.id/CKS";

        public static string Api
        {
            get { return _API; }
            set { _API = value; }
        }
        public static string VMS
        {
            get { return _VMS; }
            set { _VMS = value; }
        }
        public static string EAPPROVAL
        {
            get { return _EAPPROVAL; }
            set { _EAPPROVAL = value; }
        }
        public static string News
        {
            get { return _NEWS; }
            set { _NEWS = value; }
        }
        public static string CKS
        {
            get { return _CKS; }
            set { _CKS = value; }
        }
    }
}
