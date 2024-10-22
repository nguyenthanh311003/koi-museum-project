namespace KoiMuseum.Common
{
    public class Const
    {
        #region Error Codes

        public static int ERROR_EXCEPTION = -4;
        public static string APIEndPoint = "https://localhost:7028/api/";

        #endregion
        #region Status Codes

        public static string APPROVE_STATUS = "APPROVE";
        public static string CHECKIN_STATUS = "CHECKIN";
        public static string CANCEL_STATUS = "CANCEL";
        public static string REJECT_STATUS = "REJECT";


        #endregion

        #region Success Codes

        public static int SUCCESS_CREATE_CODE = 1;
        public static string SUCCESS_CREATE_MSG = "Save data success";
        public static int SUCCESS_READ_CODE = 1;
        public static string SUCCESS_READ_MSG = "Get data success";
        public static int SUCCESS_UPDATE_CODE = 1;
        public static string SUCCESS_UPDATE_MSG = "Update data success";
        public static int SUCCESS_DELETE_CODE = 1;
        public static string SUCCESS_DELETE_MSG = "Delete data success";

        #endregion

        #region Fail code

        public static int FAIL_CREATE_CODE = -1;
        public static string FAIL_CREATE_MSG = "Save data fail";
        public static int FAIL_READ_CODE = -1;
        public static string FAIL_READ_MSG = "Get data fail";
        public static int FAIL_UPDATE_CODE = -1;
        public static string FAIL_UPDATE_MSG = "Update data fail";
        public static int FAIL_DELETE_CODE = -1;
        public static string FAIL_DELETE_MSG = "Delete data fail";

        #endregion

        #region Warning Code

        public static int WARNING_NO_DATA_CODE = 4;
        public static string WARNING_NO_DATA_MSG = "No data";
        public static int WARNING_STATUS_CHANGE_CODE = 5;  // New code added
        public static string WARNING_STATUS_CHANGE_MSG = "Status change warning";  // Optional message

        #endregion

        public static string PAID_STATUS = "PAID";
        public static string PENDING_STATUS = "PENDING";
    }
}
