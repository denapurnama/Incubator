using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;

namespace PakAnas.Commons.Constants
{
    public class CommonConstant
    {
        public const string SYSTEM_TYPE_SPTT_APPROVER_TYPE = "SPTT_APPROVER_TYPE";

        public enum DATA_LOCK_ACTION_TYPE { SDLAT_EDIT, SDLAT_REVISE, SDLAT_CREATE };

        // Module [start]
        public const int MODULE_ID_INTERFACE_OUT = 5;
        // Module [end]
        // Upload data type, size
        public const string SUFE_EMPLOYEE_INPUT = "SUFE_EMPLOYEE_INPUT";
        public const string SUFS_EMPLOYEE_INPUT = "SUFS_EMPLOYEE_INPUT";
        // Upload data type, size

        // Upload attachment type, size
        public const string SUFE_EMPLOYEE_INPUT_ATTACHMENT =
            "SUFE_EMPLOYEE_INPUT_ATTACHMENT";
        public const string SUFS_EMPLOYEE_INPUT_ATTACHMENT =
            "SUFS_EMPLOYEE_INPUT_ATTACHMENT";
        // Upload attachment type, size

        // Upload attachment type, size
        //Download attachment
        public const string SYS_FILE_LOC_TEMP_FOLDER = "SF_TEMP_FOLDER";
        public const string SYS_FILE_LOC_MAIN_FOLDER = "SF_MAIN_FOLDER";
        //Download attachment

        // Upload data type, size
        public const string SUFE_ORG_INPUT = "SUFE_ORG_INPUT";
        public const string SUFS_ORG_INPUT = "SUFS_ORG_INPUT";
         //Upload data type, size

        ////Upload attachment type, size
        //public const string SUFE_PAKANAS_INPUT_ATTACHMENT =
        //    "SUFE_PAKANAS_INPUT_ATTACHMENT";
        //public const string SUFS_PAKANAS_INPUT_ATTACHMENT =
        //    "SUFS_PAKANAS_INPUT_ATTACHMENT";
        ////Upload attachment typr, size

        // Function [start]
        public const int FUNCTION_ID_ITF_FILE_OUT_CALENDAR = 5001;
        public const int FUNCTION_ID_ITF_FILE_OUT_COST_CENTER = 5002;
        public const int FUNCTION_ID_ITF_FILE_OUT_CYCLE_TIME = 5003;
        public const int FUNCTION_ID_ITF_FILE_OUT_MANPOWER = 5004;
        public const int FUNCTION_ID_ITF_FILE_OUT_STANDARD_TIME = 5005;
        public const int FUNCTION_ID_ITF_FILE_OUT_TARGET = 5006;
        public const int FUNCTION_ID_ITF_FILE_OUT_AWH = 5007;
        public const int FUNCTION_ID_ITF_FILE_OUT_GPQ = 5008;
        // Function [end]

        public const string SCREEN_MODE_ADD = "ADD";
        public const string SCREEN_MODE_EDIT = "EDIT";

        public const string SCREEN_DATE_FORMAT = "dd/MM/yyyy";
        public const string SCREEN_DATE_PICKER_DATE_FORMAT = "dd M yyyy";
        public const string SCREEN_YEAR_FORMAT = "yyyy";
        public const string SCREEN_DATE_PICKER_YEAR_FORMAT = "yyyy";
        //public const string SCREEN_DATE_FORMAT_JS = "dd/mm/yyyy";
        public const string SCREEN_DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        public const string SCREEN_MONEY_FORMAT = "#,##0.00##";
        public const string SCREEN_DECIMAL_FORMAT = "#,##0.###";
        public const string SCREEN_MONTH_FORMAT = "MM-yyyy";
        public const string DB_MONTH_FORMAT = "yyyyMM";

        public const string TEXT_ALL = "--All--";
        public const string TEXT_CHOOSE = "--Choose--";
        public const string TEXT_EMPTY = "";

        // Business Trip
        public const string BUSINESS_TRIP_DOMESTIC = "DOMESTIC";
        public const string BUSINESS_TRIP_OVERSEAS = "OVERSEAS";

        // Yes No
        public const string CHOICE_YES = "Y";
        public const string CHOICE_NO = "N";

        public const string CHOICE_YES_STR = "Yes";
        public const string CHOICE_NO_STR = "No";

        // Currency Cd
        public const string CURRENCY_CD_IDR = "IDR";
        public const string CURRENCY_CD_USD = "USD";
        public const string CURRENCY_CD_JPY = "JPY";

        // Purpose
        public const string PURPOSE_PROJECT = "PROJECT";
        public const string PURPOSE_NON_PROJECT = "NON-PROJECT";


        public const string CANCEL_FINISH_FLAG_YES = "Y";
        public const string CANCEL_FINISH_FLAG_NO = "N";

        //VISA LOCK WBS STATUS
        public const string LOCK_WBS_STATUS_YES = "Y";
        public const string LOCK_WBS_STATUS_NO = "N";

        //VISA DRAFT REJECT FLAG
        public const string DRAFT_REJECT_FLAG_YES = "Y";
        public const string DRAFT_REJECT_FLAG_NO = "N";

        public const string BUTTON_SEARCH = "SEARCH";
        public const string BUTTON_EDIT = "EDIT";
        public const string BUTTON_SAVE = "SAVE";

        // System Type [start]
        
        // System Type [end]

        // System Cd [start]
        

        //FTP SERVER KEY
        public const string FTP_SERVER_CD_AP = "FTP_AP";
        public const string FTP_SERVER_CD_WH = "FTP_WH";
        public const string FTP_SERVER_FTP_APD = "FTP_APD";
        public const string FTP_SERVER_FTP_WHD = "FTP_WHD";
        public const string FTP_SERVER_ITF_FILE_OUT = "ITF_FILE_OUT";

        //FTP SERVER FILE NAME
        public const string FTP_SERVER_FILE_NAME_AP = "AP_";
        public const string FTP_SERVER_FILE_NAME_WH = "WH_";

        //FTP SERVER FORMAT FILE
        public const string FTP_SERVER_FORMAT_FILE = ".csv";

        // Template for download local part
        public const string TEMPLATE_EXCEL_DIR = "/Content/Documents/";

        public const int KATASHIKI_MODULE_ID =4;
        public const int KATASHIKI_FUNCTION_ID = 1005;
        public const int SUCCESS = 0; 
        public const int ERROR_BUSINESS  =1;
        public const int ERROR_UNHANDLED = 2;
        public const int ERROR_COMMON =3;
        public const int INPROGRESS = 4;

        public const string MESSAGE_SUCCESS_ID ="MPCS00003INF";
        public const string MESSAGE_ERROR_ID = "MPCS00008ERR";
        public const string MESSAGE_START_ID  = "MPCS00002INF";
        public const string MESSAGE_FINISH_ID = "MPCS00003INF";

        public const string GETSUDO_INTERFACE_PART_LIST = "GETSUDO_INTERFACE_PART_LIST";
        public const string GETSUDO_INTERFACE_PART_LIST_ENGINE = "PROD_TYPE_ENGINE";

        public static object SUFE_PAKANAAS_ { get; set; }
    }
}
