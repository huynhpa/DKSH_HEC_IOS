using MP.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Common
{
    public class Constant
    {
        public const string HUYNH_ANH_PHAM_DKSH = "huynh.anh.pham@dksh.com";

        public const string CSKH_VIMEDIMEX_BD_MAIL_ADDRESS = "cskh-cdi@vimedimex.com";

        public const string CSKH_VIMEDIMEX_BD_PASSWORD = "*vime1234";

        public const string HUYNH_ANH_PHAM_DKSH_PASS = "*DKSH2018";

        public const string RESET_PASSWORD_AGENT = "*dksh123";

        public const string COMMA = ",";

        public const string HYPHEN = "-";

        public const string YYYYMMDDHHMM = "yyyy-MM-dd hh:mm tt";
        public const string YYYYMMDDHHMMSS = "yyyy-MM-dd HH:mm:ss";

        public const string DDMYYYY = "dd-mm-yyyy";

        public const string DDMYYYY_HHMMSS = "dd-mm-yyyy hh:mm:ss";
        public const string DDMYYYYHHMMSS_slash = "dd/mm/yyyy hh:mm:ss";

        public const string MMDDYYYY = "MM/dd/yyyy";

        public const string TRIMESTER_I = "I";

        public const string TRIMESTER_II = "II";

        public const string TRIMESTER_III = "III";

        public const string TRIMESTER_IV = "IV";

        public const string TeleMarketing = "TeleMarketing";

        public const string DOCTOR_ABBOTT = "HCP";

        public const string TIMELINE = "TIMELINE";

        public const string HALF = "HALF";

        public const string FULL = "FULL";

        public const string NO_TIMELINE_IS_RUN_MESSAGE = "There is no timeline setup to run OR Timeline has expired";

        public const string TIMLINE_SENT_SUCCESS_MESSAGE = "An email has been sent announcement licenses expired and coming expire to Supervisor with Timeline {0}";

        public const string CONNECTION_CDI = @"server=SGN1CC078;database=CDI;uid=sa;pwd=HpaSEV057";

        public const string CONNECTION_OBS = @"server=10.210.128.70;database=outbound;uid=sa;pwd=123456";

        public const string CONNECTION_TLM = "Data Source=FOREVER\\SQLEXPRESS;Initial Catalog=TeleMarketing;User ID=sa;Password=HpaEXE057";

        public const string CONNECT_SERVER_36 = @"server=10.210.128.36;database=iminduser;password=iminduser";

        public const string PATH_FILE_REQUEST = @"D:/1.Huynh Pham Anh/Request_CCC/";

        public const string TEMP_LOCATION = @"E:/DKSH_Requirement/CCCRequest";

        public const string CHUNG_TU_KHACH_HANG = @"\\vn.dksh.net\Public\SAIGON-GROUP\SGN-CDI\CHUNG TU KHACH HANG\";
        public const string CLICK_2_CALL_VIME = @"https://10.210.148.36/clicktocalldksh/doAction.php";
        public const string CLICK_2_CALL_GIGAMED = @"https://gigamed.voip247.vn/clicktocallgigamed/doAction.php";
        public const string ACTION_AUTOCALL = "autocall";
        public const string CLICK_2_CALL_CODE_VIME = @"as7dkf8ie2343it26fg32kaken43f123fdkjkek";
        public const string CLICK_2_CALL_CODE_GIGAMED = @"as7dkf8ie2343it26fg32kasdfd3f123fdkjkek";
        public const string MESSAGE_SUCCESS = "Save successfull !";
        public const string MESSAGE_PROCESS_SUCCESS = "Process successfull !";

        public const string EXTENSION_EXISTED = "Extension {0} with contract {1} already existed";

        public const string MESSAGE_SUCCESS_NEXT_ASSIGN = "Save successfull ! Do you want to go NEXT ASSIGN ?";

        public const string CUSTOMER_CODE_EXIST = "Customer code <b class=\"text-red\">{0}</b> has been registered !";
        public const string CUSTOMER_HAS_BEEN_LIMITED_INTIME = "Customer code <b class=\"text-red\">{0}</b> has been limited to call in time from {1} to {2} !";

        public const string MESSAGE_DELELE = "{0} rows have been deleted successfull !";

        public const string MESSAGE_NO_ROW_DELELE = "There are no rows have been deleted";

        public const string MESSAGE_ERROR = "Unexpected error when saving data.";

        public const string MESSAGE_ERROR_CONVERT = "{0}. Please check data input type date";

        public const string USER_EXIST = "<b>{0}</b> has been registered !";

        public const string REGISTERED = "<b>{0}</b> has been registered by <b>{1}</b>";

        public const string EXISTED = "EXISTED";

        public const string UNSUCCESS = "UNSUCCESS";

        public const string SUCCESS = "SUCCESS";

        public const string SUCCESS_NEXT_ASSIGN = "SUCCESS_NEXT_ASSIGN";

        public const string HTTP_MESSAGE_OK = "200 OK";

        public const string CALLBACK = "CALL BACK";

        public const string INFO = "INFO";

        public const string MESSAGE_INFO = "{0} does not exist or deleted";

        public const string YYYY_MM_DD = "yyyy-mm-dd";

        public const string YYYYMMDD = "yyyyMMdd";

        public const string USER_SESSION = "USER_SESSION";

        public const string SESSION_OK = "SESSION_OK";

        public const string SESSION_EXPIRED_MESSAGE = "Session has been expired. Please sign in to start your session";

        public const string SEND_PASSWORD = "A reset password has been sent to your email. Please check mail and login to change password.";

        public const string CUSTCODE_404 = "Customer code not found";

        public const string MESSAGE_CONVERT_SUCCESS = "Convert PDF success";

        public const string TEAM_CDI = "CDI";

        public const string INSERT = "INSERT";

        public const string UPDATE = "UPDATE";

        public const string DELETE = "DELETE";

        public const string SESSION_EXPIRED_CODE = "SESSION_EXPIRED_MESSAGE";

        public const string ERROR = "ERROR";
        public const string RECORD_NOT_FOUND = "";

        public const string ERROR_SQL = "ERROR_SQL";

        public const string FILE_NOT_EXIST = "File does not existed or deleted";

        public const string PROCESS_CUSTOMER_ERROR = "No rows has been updated";

        public const string OUT_OFF_MEMORY = "{0}. The error occurred may be file has exceeded limit rows. Please separate several times to upload";

        public const string NO_ROW_SAVE = "There are no rows saved !";

        public const string PHONE_NUMBER_NOT_EXISTED = "Số điện thoại này không có trong hệ thống !";

        public const string ERROR_NO_DATA_CALL = @"There are no <b class='text-red'>Data Call</b> found. 
                                                    Please upload Data Call of <b class='text-red'>{0}</b>";

        public const string ERROR_NO_SALEMAN = "There are no saleman code found. Please upload saleman code";

        public const string NO_DATA_SALE_FOUND = @"There are no data sale in <b class='text-red'>{0}</b>. Please upload data sale";

        public const string FILE_INCORRECT_FORMAT = "This file extension is not supported";

        public const int SQL_ERROR_CODE = 9999;

        public const string MESSAGE_PRODUCT_CUSTOMER_NULL = "Products OR Customers hasn't been imported";

        public const string DEPARTMENT = "Customer Care Center";

        public const string MANAGER = "MNG";

        public const string SUPPORT = "Support";

        public const string ROLE_ADMIN = "ADMIN";

        public const string ROLE_USER = "USER";

        public const string RESET_PASSWORD = "RESET_PASSWORD";

        public const string UPLOAD = "UPLOAD";

        public const string FILE_UPLOAD = "{0} files uploaded successfully!";

        public const string UPLOAD_SUCCESS = "Uploaded successfully!";

        public const string FILE_UPLOAD_SUCCESS = "File has been uploaded";

        public const string RESET_PASSWORD_MESSAGE = "Reset password successfully. Please sign in to continue your session !";

        public const string NO_FILE_SELECTED = "There are no file to uploaded !";

        public const string FILE_EXTENSION_INVALID = "There are no file to uploaded. Please check file name does not contain dot (.)!";

        public const string CSUS_PROCESS_MESSAGE = "Process successfully !";

        public const string ZIP_EXT = ".zip";

        public const string FILE_REQUEST_LOCATION = "~/FilesRequest/";

        public const string NOT_EXIST = "<b>{0}</b> has been deleted or not exist";

        public const string EXCEL_TEMPLATE_PATH = "~/ExcelTemplate/";

        public const string CERTIFICATION_EXPIRED = "Certification_Expired.xlsx";

        public const string CERTIFICATION_EXPIRED_DAILY = "Certification_Expired_{0}.xlsx";

        public const string CERTIFICATION_COMING_EXPIRE_DAILY = "Certification_Coming_Expire_{0}.xlsx";

        public const string CERTIFICATION_COMING_EXPIRE_SOON = "Certification_ComingExpireSoon.xlsx";

        public const string CDI_GCNDDKKDD = "DDKKDD";

        public const string CDI_GPP = "GPP";

        public const string CDI_GDP = "GDP";

        public const string CDI_GSP = "GSP";

        public const string CDI_KCB = "KCB";

        public const string CDI_KSDB = "KSDB";

        public const string CDI_COE_TTBYT = "COE_TTBYT";

        public const string EXCEL_XLSX = "xlsx";

        public const string EXCEL_XLS = "xls";

        public const string XLSX_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public const string XLS_CONTENT_TYPE = "application/vnd.ms-excel";

        public const string ZIP_CONTENT_TYPE = "application/zip";

        public const string PDF_CONTENT_TYPE = "Application/pdf";

        public const string MARKETING_TEMPLATE = "MarketingTemplate.zip";

        public const string TITLE_NEW_REQUEST = "New ticket await approval";

        public const string DAILY_REPORT = "Daily Report";

        public const string REPORT_G950 = "G950";

        public const string CERTIFICATION = "Certification";

        public const string SHIP_TO_LICENSES = "SHIP_TO_LICENSES";

        public const string SHIP_TO_LICENSES_TEMPLATE = "ShipToLicenses.xlsx";

        public const string DAILY_REPORT_TEMPLATE = "DailyReport.xlsx";

        public const string CERTIFICATION_TEMPLATE = "CDIReport.xlsx";

        public const string REPORT_G950_TEMPLATE = "Template_Cai_chan_thuoc_Doc_&_Cam.xlsx";

        public const string CONFIRM_NEW_REQUEST = @"<div style='font-style: Arial; font-size:10pt'><p>
                                            <span style='font-weight: bold;'>Dear {3}</span>
                                            <br><h4>Your team member just raised ticket.</h4><br>
                                            <span style='font-weight: bold;'><span style='color: rgb(206, 0, 0);'>- Ticket No :</span> </span>{0}<br>
                                            <span style='font-weight: bold; color: rgb(206, 0, 0);'>- Title : </span>{1}<br>
                                            <span style='font-weight: bold; color: rgb(206, 0, 0);'>- Registered by user :</span> {2}&nbsp;
                                            <br>&nbsp;Please <b style='color: rgb(57, 132, 198);'><a href='http://10.210.148.38:8098' target=_blank'>Sign In</a></b>
                                            &nbsp; and approval before development.&nbsp;<br></p><p><br></p><p>
                                            <span style='color: rgb(206, 0, 0); font-weight: bold;'>
                                            Thank for choosing CCC Application<br>Best Regard &amp; Sincerely</span>
                                            </p></div>";

        public const string ACTIVED_USER = "ACTIVED";

        public const string MESSAGE_ACTIVED_USER = "User has been actived";

        public const string REQUEST_APPROVED = "APPROVED";

        public const string REQUEST_REJECTED = "REJECTED";

        public const string REQUEST_AWAITING = "AWAITING";

        public const string APPROVAL = "Your ticket has been {0}";

        public const string TITLE_USER_ACTIVED = "Your account actived";

        public const string CONTENT_USER_ACTIVED = @"<div style='font-style: Arial; font-size:10pt'><p><span style='color: rgb(206, 0, 0); font-weight: bold;'>Your account has been actived by {0}</span></p>
                            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>Your information in below:</span></p>
                            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>- User Id : {1}</span></p>
                            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>- User Name : {2}</span></p>
                            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>- Email : {3}</span></p>
                            <p>Please <a href='http://10.210.148.38:8098' target='_blank'><b style='color: rgb(57, 132, 198);'>Sign In</b></a> to start your session&nbsp; and update your profile</p>
                            <p><span style='font-weight: bold;'><br></span></p>
                            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>Thanks for choosing CCC Application</span></p>
                            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>Best Regard &amp; Sincerely</span></p></div>";

        public const string APPROVAL_CONTENT = @"<div style='font-style: Arial; font-size:10pt'><p><b style='color: rgb(206, 0, 0);'>
                                                Your ticket has been {0}</b></p><p><b>- Ticket No : {1}</b></p>
                                                <p><b>- Title : {2}</b></p><p><b>- File name : {3}</b></p><p><br></p><p><br></p>
                                                <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>
                                                Thank for choosing CCC Application<br>Best Regard &amp; Sincerely</span>
                                                </p></div>";
        public const string REJECTED_CONTENT = @"<div style='font-style: Arial; font-size:10pt'><p><b style='color: rgb(206, 0, 0);'>
                                                Your ticket has been {0}</b></p><p><b>- Ticket No : {1}</b></p>
                                                <p><b>- Title : {2}</b></p><p><b>- Reason : {3}</b></p><p><br></p><p><br></p>
                                                <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>
                                                Thank for choosing CCC Application<br>Best Regard &amp; Sincerely</span>
                                                </p></div>";
        public const string TITLE_USER_REGISTER = "New user your team has registed";

        public const string CONTENT_USER_REGISTER = @"<div style='font-style: Arial; font-size:10pt'><p><span style='font-weight: bold; color: rgb(206, 0, 0);'>New user just registered, the information in below :</span></p>
                            <p><span style='font-weight: bold; color: rgb(206, 0, 0);'>- User Id : {0}</span></p>
                            <p><span style='font-weight: bold; color: rgb(206, 0, 0);'>- User name : {1}</span></p>
                            <p><span style='font-weight: bold; color: rgb(206, 0, 0);'>- Full name : {2}</span></p>
                            <p><span style='font-weight: bold; color: rgb(206, 0, 0);'>- Email : {3}</span></p>
                            <p><span style='font-weight: bold; color: rgb(206, 0, 0);'>- Team : {4}</span></p>
                            <p>Please <a href='http://10.210.148.38:8098' target='_blank'><b style='color: rgb(206, 0, 0);'>Sign In</b></a> and <span style='color: rgb(206, 0, 0); font-weight: bold;'>Active</span></p>
                            <p><br></p>
                            <p><span style='font-weight: bold; color: rgb(206, 0, 0);'>Thank for choosing CCC Application<br>Best Regard &amp; Sincerely</span></p></div>";


        public const string CER_GCNDDKKDD = "GCNDDKKDD";

        public const string CER_GPP = "GPP";

        public const string CER_GDP = "GDP";

        public const string CER_GSP = "GSP";

        public const string CER_KCB = "KCB";

        public const string CER_GCNDDKTC = "GCNDDKTC";

        public const string CER_KSDB = "KSDB";

        public const string CER_KSDBToxic = "KSDBToxic";

        public const string CER_COETTBYT = "COETTBYT";

        public const string TDPHCCDCGN = "TDPHCCDCGN"; // Thuốc dạng phối hợp có chứa chất gây nghiện

        public const string TDNLDLT = "TDNLDLT"; // Thuốc độc, nguyên liệu độc làm thuốc

        public const string TDDMCSD = "TDDMCSD"; // Thuốc dược chất thuộc danh mục bị cấm sử dụng

        public const string SINHPHAM = "SINHPHAM";

        public const string VACCIN = "VACCIN";

        public const string BOTOXXEOMIN = "BOTOXXEOMIN";

        public const string ALL = "ALL";

        public const string INCLUDED = "INCLUDED";

        public const string EXCLUDED = "EXCLUDED";

        public const string SELECTED = "SELECTED";

        public const string INHOUSE = "Inhouse";

        public const string VA05 = "VA05";

        public const string PHARMACY_COMPANY = "Phar. Company";

        public const string PC_PRIVATE_COMPANY = "PRIVATE COMPANY";

        public const string PC_STATE_COMPANY = "STATE COMPANY";

        public const string PC_WHOLESALER_COMPANY = "WHOLESALER";

        public const string PC_WSALER_PHARMACY = "W'SALER - PHARMACY";

        public const string PROCESS_MESSAGE = "{0} rows have been saved successfully !!";

        public const string SEND_MESSAGE = "{0} rows have been sent successfully !!";

        public const string PROCESS_FAILED = "Process failed!!";

        public const string BACKUP_FAILED = "Opps, Something went wrong. Please contact admin!!";

        public const string FAILED = "FAILED";

        public const string COMMING_EXPIRE = "SẮP HẾT HẠN ({0} NGÀY)";

        public const string EXPIRED = "HẾT HẠN";

        public const string GET_CELL_ERROR = "Error get cell value at column {0}";

        public const string PROCESS_CUSTOMER = "All customers have been updated !";

        public const string BACKUP_DDKXB = "All data have been saved !";

        public const string TXT_FILE = "txt";

        public const string VA05_OB = "OB";

        public const string VA05_TELE = "TELE";

        public const string QUALIFIED = "QUALIFIED";

        public const string UNQUALIFIED = "UN-QUALIFIED";

        public const string REASON = "Sales";

        public const string WAITING = "WAITING";

        public const string INPROGRESS = "INPROGRESS";

        public const string CLOSED = "CLOSED";

        public const string OUTLET = "OUTLET";

        public const string PUSHSALE = "PUSHSALE";


        /* Transfer notification */
        public const string TITLE_TRANSFER = "Quý khách vui lòng chuyển khoản số tiền trên theo thông tin sau:";

        public const string TEN_TAI_KHOAN = "     - Tên tài khoản:";

        public const string SO_TAI_KHOAN = "     - Số tài khoản:";

        public const string NGAN_HANG = "     - Ngân hàng:";

        public const string VN65_BANK_ACCOUNT = "9552226";//"119000008888";

        public const string VN65_BANK_NAME = "Ngân Hàng TNHH một thành viên ANZ (Việt Nam) CN HCM";

        public const string VN65_COMPANY = "CÔNG TY CỔ PHẦN DƯỢC - THIẾT BỊ Y TẾ ĐÀ NẴNG";

        public const string VN66_BANK_ACCOUNT = "";

        public const string VN66_BANK_NAME = "";

        public const string VN66_COMPANY = "";

        public const string VN62_BANK_ACCOUNT = "091-582767-002";

        public const string VN62_COMPANY = "CN Công Ty TNHH DKSH Việt Nam tại TP Đà Nẵng";

        public const string VN62_BANK_NAME = "Ngân hàng TNHH Một Thành Viên HSBC (Việt Nam), Thành Phố Hồ Chí Minh";

        public const string VN63_BANK_ACCOUNT = "002.139707.001";

        public const string VN63_COMPANY = "CN Công ty TNHH DKSH Việt Nam tại Hà Nội";

        public const string VN63_BANK_NAME = "Ngân Hàng TNHH MTV HSBC Việt Nam";

        public const string VN63_COMPANY_ADDRESS = "Tầng 10 Tòa nhà CEO – Lô HH2-1 Phạm Hùng, Từ Liêm , Hà Nội";

        public const string VN63_ADDRESS = "     - Địa chỉ:";

        public const string VN64_COMPANY = "Công ty TNHH Một Thành Viên Vimedimex Bình Dương";

        public const string VN64_BANK_ACCOUNT = "6339627";

        public const string VN64_BANK_NAME = "ANZ – Chi nhánh Thành phố Hồ Chí Minh";

        public const string VN61_BANK_ACCOUNT = "6339402";

        public const string VN61_BANK_NAME = "Ngan Hang TNHH MTV ANZ Viet Nam - CN HCM";

        public const string VN61_COMPANY = "CONG TY TNHH DKSH VIET NAM";

        public const string UY_NHIEM_CHI = "Sau khi chuyển tiền Quý khách vui lòng fax Ủy nhiệm chi cho chúng tôi:";

        public const string FREE_FAX = "       - Fax (miễn cước): +8428 6256 0667";

        public const string TOLL_FREE_1800545402 = "Và liên hệ với chúng tôi thông qua số điện thoại (miễn cước): 1800 54 54 02 để xác nhận thông tin.";

        public const string VN62_TOLL_FREE = "Và liên hệ với chúng tôi thông qua số điện thoại (miễn cước): 0511 389 3659 để xác nhận thông tin.";

        public const string ANNOUNCEMENT_LINE01 = @"Chúng tôi sẽ tiến hành giao hàng đến Quý khách theo chuyến giao hàng gần nhất kể từ khi nhận được xác nhận của Ngân hàng về việc thanh toán của Quý khách.";

        //public const string ANNOUNCEMENT_LINE02 = " kể từ khi nhận được xác nhận của Ngân hàng về việc thanh toán của Quý khách.";

        public const string NOTE_LINE01 = "Lưu ý:";
        public const string NOTE_LINE02 = "      - Quý khách vui lòng chuyển khoản cho chúng tôi ngay khi nhận được thông báo này.";
        public const string NOTE_LINE03 = "      - Trong vòng 04 ngày làm việc kể từ ngày xuất hóa đơn, ";
        public const string NOTE_LINE04 = "        nếu chúng tôi chưa nhận được xác nhận của Ngân hàng về việc thanh toán của Quý khách,";
        public const string NOTE_LINE05 = "        chúng tôi buộc lòng sẽ hủy hóa đơn theo qui định của Công ty mà không cần thông báo lại với Quý khách.";
        public const string NOTE_INFO = @"      - Khi thực hiện giao dịch Quý khách ghi rõ tên đơn vị mua hàng và số hóa đơn thanh toán.";

        public const string NOTE_LINE03_PROVINCE = "      - Trong vòng 05 ngày làm việc kể từ ngày xuất hóa đơn, ";

        public const string THANK_YOU = "Trân trọng cảm ơn Quý khách.";

        public const string DA_NANG_DATETIME = "Đà Nẵng, ngày {0} tháng {1} năm {2}";

        public const string HCMC_DATETIME = "TPHCM, ngày {0} tháng {1} năm {2}";

        public const string HANOI_DATETIME = "Hà Nội, ngày {0} tháng {1} năm {2}";

        /// <summary>
        /// DKSH HCM
        /// </summary>
        public const string VN61 = "VN61";

        /// <summary>
        /// DKSH VN tại DN
        /// </summary>
        public const string VN62 = "VN62";

        /// <summary>
        /// DKSH VIỆT NAM TẠI HÀ NỘI
        /// </summary>
        public const string VN63 = "VN63";

        /// <summary>
        /// Vimedimex Binh Duong
        /// </summary>
        public const string VN64 = "VN64";

        /// <summary>
        /// Duoc va TBYT DN
        /// </summary>
        public const string VN65 = "VN65";

        public const string VN66 = "VN66";

        public const string VN61_PROVINCE = "VN61Province";

        public const string VN64_PROVINCE = "VN64Province";

        public const string PROVINCE = "Province";

        public const string PAYMENT_TERM_01 = "no 01 ngay";

        public const string TRANSFER_NOTIFICATE_ZIP = "TransferNotification.zip";

        public const string TRANSFER_NOTIFICATE_TEMPLATE = "TransferNotification.xlsx";

        public const string TRANSFER_NOTIFICATE_PROVINCE_TEMPLATE = "TemplateVA05.xlsx";

        public const string PDF_EXTENSION = ".pdf";

        public const string XLSX_EXTENSION = ".xlsx";

        public const string MESSAGE_SEND_SUCCESS = "Sent successfully !";

        public const string MESSAGE_NO_SCRIPT = "No script for reject {0}";

        public const string MESSAGE_SEND_FAILED = "Sent failed !";

        public const string NON_NOTIFY_PENDING = "This product is not notify pending";

        public const string TRANSFER_NOTIFI_SUBJECT = "THƯ TBCK - {0}";

        public const string TRANSFER_NOTIFI_BODY = @"<h5><span style='font-family: Arial; font-size:11pt;'>Kính gửi: Quý khách hàng,</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Chúng tôi trân trọng gửi đến Quý khách thông tin hóa đơn tài chính cho các đơn hàng đã được đặt theo yêu cầu.</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Quý khách vui lòng kiểm tra và phản hồi cho chúng tôi ngay khi phát hiện có bất kì sai sót trên hóa đơn.</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Đối với các đơn hàng theo hình thức thanh toán là chuyển khoản, Quý khách vui lòng thanh toán cho chúng tôi theo số tài khoản sau:</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>- Tên tài khoản:</span>&nbsp;&nbsp;<span style='font-family: Arial; font-size:11pt; font-weight: bold; color: #0099ff;'>{0}</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>- Số tài khoản:</span>&nbsp;&nbsp;<span style='font-family: Arial; font-size:11pt;font-weight: bold; color: #0099ff;'>{1}</span></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>- Tên ngân hàng:</span>&nbsp;&nbsp;<span style='font-family: Arial; font-size:11pt;font-weight: bold; color: #0099ff;'>{2}</span></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Để thuận tiện cho việc theo dõi đơn hàng, Quý khách vui lòng Fax hoặc scan Ủy nhiệm chi cho chúng tôi theo số: +8428 6256 0667 (miễn cước) hoặc email: customercare.vn@dksh.com</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Chúng tôi sẽ tiến hành chuyển hàng đến Quý khách theo tuyến giao hàng gần nhất ngay khi nhận được thông tin xác nhận từ Ngân hàng.</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'><label style='color: #ff0000;'>Lưu ý: </label>Quý khách vui lòng thực hiện chuyển khoản trong vòng 4 ngày kể từ ngày nhận được thông báo này. Sau thời gian trên, nếu chúng tôi chưa nhận được xác nhận từ Ngân hàng về việc thanh toán của Quý khách, chúng tôi buộc lòng hủy hóa đơn theo qui định mà không cần phải thông báo cho Quý khách.</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Nếu Quý khách có thắc mắc liên quan đến đơn hàng, hàng hóa, hóa đơn vui lòng liên hệ với chúng tôi thông qua số điện thoại: 1800.54.54.02 (miễn cước)</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'>Công ty TNHH DKSH VN rất hân hạnh được phục vụ Quý khách và mong tiếp tục nhận được sự ủng hộ nhiệt tình của Quý khách.</span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'><span style='font-weight: bold;'><br></span></span></h5>
                    <h5><span style='font-family: Arial; font-size:11pt;'><span style='font-weight: bold;'>Trân
                        trọng cảm ơn và kính chào.</span><br></span>
                    </h5>";

        public const string NOT_USE = "NOT USE";

        public const string PROCESS_CDI_LOG = "[LOG PROCESS CUSTOMER AND BACK UP DATA FOR DAILY REPORT] : {0}";

        public const string REQUIRED_LICENSES = "RequiredLicenses.xlsx";

        public const string LICENSES_SUMMARY = "LicenseSummary.xlsx";

        public const string LICENSES_HISTORY = "LicenseHistory.xlsx";

        public const string LICENSE_EXPIRED_TO_SALE = "LicenseExpiredToSale.xlsx";

        public const string CHANNEL_HISTORY = "ChannelHistory.xlsx";

        public const string FULL_LICENSE = "FULL LICENSE";

        public const string LACK_OF_LICENSE = "LACK OF LICENSE";

        public const string NO_CONTROL_LICENSE = "NO CONTROL LICENSE";

        public const string SELLING_QUOTA_XLSX = "SellingQuota.xlsx";

        public const string SELLING_QUOTA = "SellingQuota";

        public const string BACKUP_DATA_DAILY_REPORT_TITLE = "[Please do not reply] BACKUP DATA FOR DAILY REPORT";

        public const string BACKUP_DATA_DAILY_REPORT_CONTENT = @"<p><b style='color: rgb(206, 0, 0);'>Dear CDI Team</b></p>
            <p>- DATA FOR DAILY REPORT HAS BEEN BACK UP BY <span style='font-weight: bold; color: rgb(206, 0, 0);'>{0}</span> AT 
            <span style='font-weight: bold; color: rgb(206, 0, 0);'>{1}</span></p>";

        public const string ALARM_CERTIFICATION_EXPIRED_TITLE = "[Please do not reply] Alarm certification expired";

        public const string ALARM_CERTIFICATION_EXPIRED_AND_COMING_EXPIRE_TITLE = "[Please do not reply] Alarm certification EXPIRED and COMING EXPIRE";

        public const string ALARM_CERTIFICATION_EXPIRED_DAILY = "ALARM_CERTIFICATION_EXPIRED_DAILY";

        public const string ALARM_CERTIFICATION_COMING_EXPIRE_DAILY = "ALARM_CERTIFICATION_COMING_EXPIRE_DAILY";

        public const string CERTIFICATION_EXPIRED_COMING_EXPIRE_DAILY_REPORT_CONTENT = @"<p><b style='color: rgb(206, 0, 0);'>Dear CDI Team</b></p>
            <p>- FIND FILES IN ATTACHMENT TO GET LIST CUSTOMERS HAVE <b style='color: rgb(206, 0, 0);'>LICENSES EXPIRED AND COMING EXPIRE</b> 
            TODAY AUTO SENT BY <span style='font-weight: bold; color: rgb(206, 0, 0);'>{0}</span> AT 
            <span style='font-weight: bold; color: rgb(206, 0, 0);'>{1}</span></p>
            <p><span style='color: rgb(206, 0, 0); font-weight: bold;'>Thank for choosing CCC Application</span></p>";

        public const string CERTIFICATION_COMING_EXPIRE_DAILY_REPORT_CONTENT = @"<p><b style='font-family:Arial; color: rgb(206, 0, 0);'>Dear CDI Team</b></p>
            <p class='font-family:Arial; '>- FIND FILE IN ATTACHMENT TO GET LIST CUSTOMERS HAVE LICENSES COMING EXPIRE TODAY AUTO SENT BY <span style='font-family:Arial; font-weight: bold; color: rgb(206, 0, 0);'>{0}</span> AT 
            <span style='font-family:Arial; font-weight: bold; color: rgb(206, 0, 0);'>{1}</span></p>
            <p><b style='font-family:Arial; color: rgb(107, 165, 74);'>Mail also sent success to  GTS's Supervisor</b></p>";

        public const string SENT = "SENT";

        public const string NOT_SENT = "NOT SENT";

        public const string ALARM_CERTIFICATION_DAILY = "ALARM_CERTIFICATION_DAILY";

        public const string SEND_MAIL_GTS_TELESALE = "SEND_MAIL_GTS_TELESALE";

        public const string MESSAGE_AUTO_ALARM = "Auto alarm certificate EXPIRED & COMING EXPIRE have been sent your email. Please check.";

        public const string PHAN_THANH_KINH_MAIL = "kinh.thanh.phan@dksh.com";

        public const string CDI_EMAIL = "cdi.vietnam@dksh.com";

        public const string NGUYEN_THUY_TUYET_VAN_MAIL = "van.thuytuyet.nguyen@dksh.com";

        public const string PHARMACY_VIME_MAIL = "pharmacy@vimedimexbinhduong.com";

        public const string THO_THI_NGUYEN_MAIL = "tho.thi.nguyen@dksh.com";

        public const string NGOC_HONG_LUONG_MAIL = "ngoc.hong.luong@dksh.com";

        public const string TRIET_VIET_TRAN_MAIL = "triet.viet.tran@dksh.com";

        public const string UYEN_XUANPHUONG_NGUYEN_MAIL = "uyen.xuanphuong.nguyen@dksh.com";

        public const string TELESALE_TEAM1_EMAIL = "telesales.team1.vn@dksh.com";

        public const string TELESALE_TEAM_EMAIL = "Telesales.Team.VN@dksh.com";

        public const string TELESALE_TEAM2_EMAIL = "telesales.team2.vn@dksh.com";

        public const string TELESALE_TEAM3_EMAIL = "";

        public const string THUY_THITHANH2_LE_EMAIL = "thuy.thithanh2.le@dksh.com";

        public const string TELESALE_HECA_MAIL = "tele.sales.heca@dksh.com";

        public const string THUY_THITHANH2_NGUYEN_EMAIL = "thuy.thithanh2.nguyen@dksh.com";

        public const string TRAM_VUAI_TRA_EMAIL = "tram.vuai.tra@dksh.com";

        public const string KIM_THI_THIEN_TRAN_EMAIL = "kim.thithien.tran@dksh.com";

        public const string VAN_THIBICH_NGO_EMAIL = "van.thibich.ngo@dksh.com";

        public const string NGOC_NHATHANH_NGUYEN_MAIL = "ngoc.nhathanh.nguyen@vimedimexbinhduong.com";

        public const string DAO_THIANH_NGUYEN_MAIL = "Dao.thianh.nguyen@dksh.com";

        public const string CCC_PROVINCE_VIME_MAIL = "CCC.Province@vimedimexbinhduong.com";

        public const string CCC_PROVINCE_SGN_MAIL = "ccc.province.sgn@dksh.com";

        public const string THU_THIMINH_LE = "thu.thiminh.le@dksh.com";

        public const string DUONG_THITHUY_PHAN = "duong.thithuy.phan@vimedimexbinhduong.com";

        public const string CCC_HOSPITAL = "CCC.Hospital@vimedimexbinhduong.com";

        public const string CCC_MD = "CCC.MD-Diag@vimedimexbinhduong.com";

        public const string PENDING_ORDER_HOSPITAL_PASSWORD = "*vime12345";

        public const string PENDING_ORDER_MEDICAL_PASSWORD = "*thang10";

        public const string SUPERVISOR_GTS_TITLE = @"Danh sách khách hàng có chứng từ hết hạn HIGH IMPORTANT!";

        public const string SUPERVISOR_GTS_BODY = @"<p><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:&quot;Arial&quot;;mso-ansi-language:EN-US;mso-fareast-language:
                                    EN-US;mso-bidi-language:AR-SA'><b>Dear Pharmacy team</b><br></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Đính kèm là danh sách
                                    khách có chứng từ hết hạn. Các anh/chị vui lòng thông báo đến khách hàng và hổ
                                    trợ&nbsp;</span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Arial;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>cập nhật chứng từ mới
                                    nhất giúp.<br></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Arial;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'><b style='color: rgb(206, 0, 0);'>Chứng từ gửi về gồm:</b><br>1.&nbsp;</span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Arial;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>GPKD (Giấy phép kinh
                                    doanh) / MST<br>2.&nbsp;</span><span style='font-family: Arial, sans-serif; font-size: 10pt;'>GCN (Giấy chứng nhận đủ điều kiện kinh doanh dược)<br>3.&nbsp;</span><span style='font-family: Arial, sans-serif; font-size: 10pt;'>GPP/GDP<br></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Arial;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'><b style='color: rgb(206, 0, 0);'>Hình thức gửi:&nbsp;</b><br></span><span style='font-family: Arial, sans-serif; font-size: 10pt;'>1.&nbsp;</span><span style='font-family: Arial, sans-serif; font-size: 10pt;'>Email: </span><a href='mailto:cskh-cdi@vimedimexbinhduong.com' style='font-family: Arial, sans-serif; font-size: 10pt; background-color: rgb(255, 255, 255);'>cskh-cdi@vimedimexbinhduong.com</a><br>2.&nbsp;<span style='font-family: Arial, sans-serif; font-size: 10pt;'>Zalo/Viber: <b>0931 848
                                    978<br></b></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Arial;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'><b><span style='color: rgb(206, 0, 0);'>Timeline: {0}</span><br></b></span><span style='font-family: Arial, sans-serif; font-size: 10pt;'><b><br><br></b></span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    mso-fareast-font-family:Arial;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                    EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'><b>Cảm ơn các anh/chị<br></b></span><span style='font-family: Arial, sans-serif; font-size: 10pt;'><b><br></b></span></p><p class='MsoNormal'><b><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    color:#00B050'>CDI Team<br>VIMEDIMEX BINH DUONG ONE MEMBER CO., LTD<o:p></o:p></span></b></p><p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    color:#1F497D;text-transform:uppercase'>18 L1-2 VSIP II, street 3,
                                    VIETNAM-SINGAPORE INDUSTRIAL PARK 2,<o:p></o:p></span></p><p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    color:#1F497D;text-transform:uppercase'>Hoa Phu ward, Thu Dau Mot CITY, Binh
                                    Duong province<o:p></o:p></span></p><p class='MsoNormal'><b><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,sans-serif;
                                    color:#00B050;text-transform:uppercase'>rep office<o:p></o:p></span></b></p><p class='MsoNormal'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif;color:#1F497D'>Viettel Complex, Floor 6A2<o:p></o:p></span></p><p class='MsoNormal'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif;color:#1F497D'>285 Cach Mang Thang Tam<o:p></o:p></span></p><p class='MsoNormal'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif;color:#1F497D'>Ward 12, District 10, Ho Chi Minh
                                    City, Vietnam<o:p></o:p></span></p><p class='MsoNormal'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif;color:#1F497D'>Tel: 028 38134427<o:p></o:p></span></p><p>





                                    </p><p class='MsoNormal'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif;color:#1F497D'>Mobile +84 9</span><span lang='VI' style='font-size:10.0pt;font-family:&quot;Arial&quot;,sans-serif;color:#1F497D;
                                    mso-ansi-language:VI'>78 107 908<o:p></o:p></span></p><p class='MsoNormal' style='margin-left:1.5in'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif'><o:p></o:p></span></p><p class='MsoNormal' style='margin-left:1.5in'><span style='font-size:10.0pt;
                                    font-family:&quot;Arial&quot;,sans-serif'><o:p></o:p></span></p>";

        public const string MAIL_CONTENT_PROVINCE_TRANSFER_NOTIFICATION = @"<p class=MsoNormal style=mso-margin-top-alt:auto;margin-bottom:12.0pt;
            text-autospace:none><b><span style=font-family:&quot;Arial&quot;,sans-serif;color:#CE0000>Kính
            gửi Quý khách hàng,</span></b><o:p></o:p></p><p class=MsoNormal style=mso-margin-top-alt:auto;margin-bottom:12.0pt;
            text-autospace:none><b><span style=font-family:&quot;Arial&quot;,sans-serif>Công ty
            TNHH&nbsp;MTV&nbsp;Vimedimex Bình Dương / Trung tâm chăm sóc khách hàng Công ty
            TNHH&nbsp;DKSH&nbsp;Việt Nam</span></b><span style=font-family:&quot;Arial&quot;,sans-serif>
            xin chân thành cảm ơn sự ủng hộ nhiệt tình của Quý khách.</span><o:p></o:p></p><p class=MsoNormal style=mso-margin-top-alt:auto;margin-bottom:12.0pt;
            text-autospace:none><span style=font-family:&quot;Arial&quot;,sans-serif>Với mong muốn
            mang đến mọi điều kiện thuận lợi cho Quý khách trong việc đặt hàng hóa, chúng
            tôi xin được gửi đến Quý khách các thông tin có giá trị và hữu ích với hy vọng
            sẽ nhận được sự quan tâm của Quý khách.</span><o:p></o:p></p><p>


            </p><p><span style=font-family:&quot;Arial&quot;,sans-serif>Nếu có thắc mắc về các thông
            tin trên, Quý khách vui lòng liên lạc với Trung tâm dịch vụ chăm sóc khách hàng
            của Vimedimex&nbsp; theo số điện thoại miễn cước : <b>1800 54 54 02<br></b><br><b>Thông
            báo chuyển khoản trong file đính kèm</b></span><br>
            <br>
            <b><span style=font-family:&quot;Arial&quot;,sans-serif>Trân trọng cảm ơn Quý khách. Kính
            chúc Quý khách nhiều sức khỏe</span></b><o:p></o:p></p>";

        public const string MAIL_CONTENT_TRANSFER_NOTIFICATION = @"<p></p>
            <p class='MsoNormal' style='mso-margin-top-alt:auto;margin-bottom:12.0pt;' text-autospace:none=''>
               <span style='color: rgb(8, 82, 148);'>
                  <b><span style='font-family: Arial, sans-serif;'>Kính gửi Quý Công Ty,</span></b>
                  <o:p></o:p>
               </span>
            </p>
            <font face='Arial, sans-serif'><b style='color: rgb(8, 82, 148);'>DKSH xin gửi thư thông báo chuyển khoản, Quý công ty vui lòng chuyển khoản đúng số tiền trong thư đính kèm</b></font>
            <p><span style='color: rgb(8, 82, 148);'><br></span></p>
            <p class='MsoNormal' style='mso-margin-top-alt:auto;margin-bottom:12.0pt;' text-autospace:none=''><span style='font-family: Arial, sans-serif; color: rgb(8, 82, 148);'>- Khi thực hiện giao dịch Quý khách ghi rõ tên đơn vị mua hàng và số Hóa đơn thanh toán<br>- Quý khách vui lòng chuyển khoảng cho chúng tôi ngay khi nhận được thông báo này.<br>- Trong vòng 04 ngày làm việc kể từ ngày xuất hóa đơn, nếu chúng tôi chưa nhận được xác nhận của Ngân hàng về việc thanh toán của Quý khách, chúng tôi buộc lòng sẽ hủy hóa đơn theo quy định của Công ty mà không cần thông báo lại với Quý khách</span></p>
            <p>
               <span style='color: rgb(8, 82, 148);'><br>
               <b style=''><span style='font-family: Arial, sans-serif;'>Trân trọng cảm ơn Quý khách !</span></b></span>
               <o:p></o:p>
            </p>
            <p></p>";

        public const string MESSAGE_QUESTION_EXISTED = "Question has been regitered";

        public const string CUSTOMER_SURVER_REPORT_XLSX = "CustomerSurveyReport.xlsx";

        public const string SUB_TOTAL_SURVEY = "Sub-total mark";

        public const string HOSPITAL = "HOSPITAL";

        public const string MEDICAL = "MEDICAL";

        public const string PIADMIN = "PIADMIN";

        public const string PRODUCT = "PRODUCT";

        public const string SALEREP = "SALEREP";

        public const string EMAIL = "EMAIL";

        public const string TEMPLATE_PENDING_ORDERS_CUSTOMER = "PendingOrdersCustomer.xlsx";

        public const string TEMPLATE_PENDING_ORDERS_NON_CUSTOMER = "PendingOrdersNonCustomer.xlsx";

        public const string SEND_MAIL_WITH_STATUS = "{0} emails have been sent successfully. {1} emails have sent failed";

        /* script pending orders */
        public const string KINH_GUI_ANH_CHI = @"<p><span style='font-size: 11pt; line-height: 107%; font-family: Arial, sans-serif; color: rgb(8, 49, 57);'><b>Kính gửi Anh/Chị,</b></span><br></p>";

        public const string KINH_GUI_QUY_KHACH = @"<p><span style='font-size: 11pt; line-height: 107%; font-family: Arial, sans-serif; color: rgb(8, 49, 57);'><b>Kính gửi Quý Khách,</b></span><br></p>";

        public const string CHAN_THANH_CAM_ON = @"<p><b style='font-size: 11pt; line-height: 107%; font-family: Arial,color: rgb(66, 66, 66);'>Chân thành cảm ơn</b><br></p>";

        public const string SCRIPT_PENDING_1 = @"<h5><font face='Arial'><b>Kính gửi Anh/chị,</b></font></h5>
            <h5><font face='Arial'>Vimedimex trân trọng xin thông báo đến anh/chị, hiện tại sản
                phẩm bên dưới đang chờ hàng nên tạm thời chưa giao hàng cho khách hàng, sẽ ghi
                nhận đơn hàng chờ, anh/chị vui lòng thông báo cho Bệnh Viện / Khách hàng giúp.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
	                mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
	                Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
                </p>
                <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
	                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
                </p>
                <p class='MsoNormal' style='margin-bottom:12.0pt'>
		                <i style=''><b style='font-family: Arial;'>
		                Fax:
		                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		                +8428 6256 0667</b><br></i><span style='color: inherit;'><br><font face='Arial'><b>Trân trọng kính chào</b></font></span></p>
            </h5>";
        public const string SCRIPT_PENDING_2 = @"<h5><font face='Arial'><b>Kính gửi Quý Khách hàng,</b></font></h5>
            <h5><font face='Arial'>Vimedimex trân trọng xin thông báo , hiện tại sản phẩm bên dưới đang chờ hàng nên tạm thời chưa giao hàng cho Quý khách, chúng tôi đã ghi nhận đơn hàng chờ, sẽ thông báo đến Quý Khánh hàng ngay khi có hàng đầy đủ. Mong Quý khách hàng thông cảm.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
	                mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
	                Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
                </p>
                <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
	                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
                </p>
                <p class='MsoNormal' style='margin-bottom:12.0pt'>
	                <span style='font-family: Arial;'>
		                <b><i>
		                Fax:
		                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		                +8428 6256 0667</i></b>
		                <o:p></o:p>
	                </span>
                </p>
                <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
                Trân trọng kính chào</span>
                </p>
            </h5>";

        public const string SCRIPT_PENDING_3 = @"<h5><font face='Arial'><b>Kính gửi Anh/chị,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo đến anh/chị, hiện tại sản phẩm bên dưới đang chờ hàng nên tạm thời chưa giao hàng cho khách hàng, sẽ ghi nhận đơn hàng chờ, anh/chị vui lòng thông báo cho Bệnh Viện / Khách hàng giúp.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
	              mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
	              Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
               </p>
               <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
	              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
               </p>
               <p class='MsoNormal' style='margin-bottom:12.0pt'>
	              <span style='font-family: Arial;'>
		             <b><i>
		             Fax:
		             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		             +8428 6256 0667</i></b>
		             <o:p></o:p>
	              </span>
               </p>
               <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
               Trân trọng kính chào</span>
               </p>
            </h5>";

        public const string SCRIPT_PENDING_4 = @"<h5><font face='Arial'><b>Kính gửi Quý Khách hàng,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại sản phẩm bên dưới chúng tôi không còn phân phối nữa, nên đơn hàng này sẽ được hủy, chân thành cảm ơn và cám sự ủng hộ của Quý Khách hàng trong thời gian qua.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
	              mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
	              Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
               </p>
               <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
	              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
               </p>
               <p class='MsoNormal' style='margin-bottom:12.0pt'>
	              <span style='font-family: Arial;'>
		             <b><i>
		             Fax:
		             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		             +8428 6256 0667</i></b>
		             <o:p></o:p>
	              </span>
               </p>
               <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
               Trân trọng kính chào</span>
               </p>
            </h5>";

        public const string SCRIPT_PENDING_5 = @"<h5><font face='Arial'><b>Kính gửi Anh/Chị,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại các đơn hàng này còn vướng vấn đề công nợ nên tạm thời chưa giao hàng được, anh/chị vui lòng thông báo cho KH để giải quyết công nợ sớm. Chân thành cảm ơn.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
	              mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
	              Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
               </p>
               <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
	              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
               </p>
               <p class='MsoNormal' style='margin-bottom:12.0pt'>
	              <span style='font-family: Arial;'>
		             <b><i>
		             Fax:
		             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		             +8428 6256 0667</i></b>
		             <o:p></o:p>
	              </span>
               </p>
               <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
               Trân trọng kính chào</span>
               </p>
            </h5>";

        public const string SCRIPT_PENDING_6 = @"<h5><font face='Arial'><b>Kính gửi Quý Khách,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại các đơn hàng này còn vướng vấn đề công nợ nên tạm thời chưa giao hàng được, mong quý khách hàng thông cảm và giải quyết công nợ sớm. Chân thành cảm ơn.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
		            mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
		            Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
	            </p>
	            <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
		            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
	            </p>
	            <p class='MsoNormal' style='margin-bottom:12.0pt'>
		            <span style='font-family: Arial;'>
			            <b><i>
			            Fax:
			            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			            +8428 6256 0667</i></b>
			            <o:p></o:p>
		            </span>
	            </p>
	            <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
	            Trân trọng kính chào</span>
	            </p>
            </h5>";

        public const string SCRIPT_PENDING_7 = @"<h5><font face='Arial'><b>Kính gửi Anh/chị,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại sản phẩm bên dưới đang chờ hàng nên tạm thời chưa giao hàng cho khách hàng, sẽ ghi nhận đơn hàng chờ,anh/chị vui lòng thông báo cho khách hàng. Chân thành cảm ơn.</font>
            </h5>
            <h5><p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
	              mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
	              Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
               </p>
               <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
	              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
               </p>
               <p class='MsoNormal' style='margin-bottom:12.0pt'>
	              <span style='font-family: Arial;'>
		             <b><i>
		             Fax:
		             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		             +8428 6256 0667</i></b>
		             <o:p></o:p>
	              </span>
               </p>
               <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
               Trân trọng kính chào</span>
               </p>
            </h5>";

        public const string SCRIPT_PENDING_8 = @"<h5><font face='Arial'><b>Kính gửi Quý Khách,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại sản phẩm bên dưới đang chờ hàng nên tạm thời chưa giao hàng cho Quý khách, chúng tôi sẽ ghi nhận đơn hàng chờ,mong quý khách hàng thông cảm.</font></h5><h5>
            <p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
              mắc xin vui lòng liên hệ</i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
              Trung tâm chăm sóc khách hàng số miễn cước:</i></b></p><p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
            </p>
            <p class='MsoNormal' style='margin-bottom:12.0pt'>
              <span style='font-family: Arial;'>
	             <b><i>
	             Fax:
	             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	             +8428 6256 0667</i></b>
	             <o:p></o:p>
              </span>
            </p>
            <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
            Trân trọng kính chào</span>
            </p>
            </h5>";

        public const string SCRIPT_PENDING_9 = @"<h5><font face='Arial'><b>Kính gửi Anh/Chị,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại các đơn hàng này còn vướng vấn đề công nợ nên tạm thời chưa giao hàng được, anh/chị vui lòng thông báo cho KH để giải quyết công nợ sớm.</font></h5><h5>
            <p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
              mắc xin vui lòng liên hệ</i></b><b style='color: inherit;'><i style='font-family: Arial;'><br>Tổng Đài
              Trung tâm chăm sóc khách hàng số miễn cước:</i></b></p><p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
            </p>
            <p class='MsoNormal' style='margin-bottom:12.0pt'>
              <span style='font-family: Arial;'>
	             <b><i>
	             Fax:
	             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	             +8428 6256 0667</i></b>
	             <o:p></o:p>
              </span>
            </p>
            <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
            Trân trọng kính chào</span>
            </p>
            </h5>";

        public const string SCRIPT_PENDING_10 = @"<h5><font face='Arial'><b>Kính gửi Quý Khách,</b></font></h5><h5><font face='Arial'>
            Vimedimex trân trọng xin thông báo, hiện tại các đơn hàng này còn vướng vấn đề công nợ nên tạm thời chưa giao hàng được, mong quý khách hàng thông cảm và giải quyết công nợ sớm.</font></h5><h5>
            <p class='MsoNormal'><b><i style='font-family: Arial;'>Mọi thắc
              mắc xin vui lòng liên hệ<br></i></b><b style='color: inherit;'><i style='font-family: Arial;'>Tổng Đài
              Trung tâm chăm sóc khách hàng số miễn cước:</i></b>
            </p>
            <p class='MsoNormal'><b style='font-family: Arial; color: inherit;'><i>Tollfree:
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1800 54 54 02&nbsp;</i></b><br>
            </p>
            <p class='MsoNormal' style='margin-bottom:12.0pt'>
              <span style='font-family: Arial;'>
	             <b><i>
	             Fax:
	             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	             +8428 6256 0667</i></b>
	             <o:p></o:p>
              </span>
            </p>
            <p class='MsoNormal'><span style='font-family: Arial; font-weight: bold;'>
            Trân trọng kính chào</span>
            </p>
            </h5>";

        public const string PENDING_ORDER_TITLE = "Vimedimex thông báo đơn hàng tạm thời không giao được";

        public const string MESSAGE_DELELE_DATA_SALE = "{0} rows have been deleted !";

        public const string PENDING_ORDER_INFOR = "Detail <p class='text-blue'>{0}</p>";

        public const string VIMEDIMEXBINHDUONG_COMPANY = "CHI NHÁNH CÔNG TY CỔ PHẦN Y DƯỢC PHẨM VIMEDIMEX BÌNH DƯƠNG";

        public const string DKSH_CUSTOMER_CARE_CENTER = @"TRUNG TÂM CHĂM SÓC KHÁCH HÀNG - SĐT MIỄN CƯỚC 1800545402 - 
                                                          FAX MIỄN CƯỚC 1800545420 CTY TNHH DKSH VN";

        public const string SALES_PROMOTION_OBJECT = @"KÍNH GỬI QUÝ KHÁCH HÀNG {0} CHƯƠNG TRÌNH KHUYỀN MÃI";
        public const string SALES_PROMOTION_REPORT = "SalesPromotionReport.xlsx";
        public const string SALES_PROMOTION_BY_ENTITY = "ExportByEntity.xlsx";
        public const string DATA_ANALYSIC = "DataAnalysic";


        // OUTBOUND CONSTANT
        public const string OBS_ACCOUNT_NOT_EXIST = "Account does not exist.";
        public const string OBS_ACCOUNT_LOCKED = "This account was locked";
        public const string OBS_PASSWORD_INCORRECT = "Incorrect password";
        public const string ASTERISK_SERVER_VIME = "10.210.148.36";//"10.210.148.46";
        public const string ASTERISK_USER_VIME = "popup";//"admin";
        public const string ASTERISK_PASSWORD_VIME = "popupamipassword2016";//"1133557799";//"popupamipassword2016";//"Thubay145";
        public const int ASTERISK_PORT_VIME = 5038;

        public const string ASTERISK_SERVER_GIGA = "119.82.130.112";//gigamed.voip247.vn
        public const string ASTERISK_USER_GIGA = "admin1";//"amiconnect";//"admin";
        public const string ASTERISK_PASSWORD_GIGA = "AdminCallCentertoyota2014";//"amiconnectGigamed@2021";
        public const int ASTERISK_PORT_GIGA = 5038;


        public const string QUICK_CALLING = "QuickCalling";
        public const string CALL_PLAN = "CallPlan";
        public const string OUT_CALL_PLAN = "OutCallPlan";
        public const string SYSTEM_AUTO = "SYSTEM_AUTO";
        public const string MESSAGE_NOT_IN_CALLING_TIME = "Not in calling time for customer: {0}";
        public const string MESSAGE_NO_PHONE_NUMBER_FOR_CODE = "There is no phone number for code: {0}";
        public const string MESSAGE_NO_CONTACT = "There is no contact name";
        public const string MESSAGE_NO_PHONE = "There is no phone number";
        // FEEDBACK STATUS
        public const string FEEDBACK_STATUS_CALLBACK = "1";
        public const string FEEDBACK_STATUS_NORMAL = "0";
        public const string FEEDBACK_STATUS_PROCESS = "2";
        public const string FEEDBACK_STATUS_TRANFER = "3";
        // MESSAGE
        public const string EDIT_CALL_PERMISSION_MESSAGE = @"Please edit the call survey from 【 Search Call 】 screen";
        public const string PHONE_NUMBER_ALREADY_SURVEY_AGENT = @"Số điện thoại này đã được khảo sát bởi danh số {0} ngày {1}, vui lòng chọn số khác";
        public const string PHONE_NUMBER_ALREADY_SURVEY_MOBI_INTERNET = @"Số điện thoại này đã được khảo sát ở dịch vụ MobiInternet {0} ngày {1}, vui lòng chọn số khác";
        public const string PHONE_NUMBER_ALREADY_SURVEY_FAST_CONNECT = @"Số điện thoại này đã được khảo sát ở dịch vụ FastConnect {0} ngày {1}, vui lòng chọn số khác";

        // ERROR PHONE NUMBER FORMAT
        public const string ERROR_SAVE_SURVEY = @"Số điện thoại này nhập sai định dạng(định dạng đúng 9xxxxxxxx(9 chữ số) hoặc 1xxxxxxxxx(10 chữ số)). Vui lòng nhập lại";

        public const string SERVICE_TYPE_EMPTY = @"Vui lòng chọn loại dịch vụ";
        // QUESTION CODE
        public const string QUESTION_CODE_EXISTED = @"Question code is existed";

        public const string ROLE_IN_USE = @"Role is in use, can not delete";
        public const string ROLE_NAME_EXISTED = @"Role Name Existed";
        public const string CHANGE_PASSWORD_SUCCESS = @"Change password successfully";
        public const string NO_PASSWORD_CHANGED = @"No password changed";
        public const string RESET_PASSWORD_AGENT_MESSAGE = "User {0} has been reset password sucessfully. New password is {1}";
        public const string USER_NAME_EXIST = @"User name or extension {0} has been registered";
        public const string BLANK_IMAGE = @"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAACWCAYAAABkW7XSAAAEYklEQVR4Xu3UAQkAAAwCwdm/9HI83BLIOdw5AgQIRAQWySkmAQIEzmB5AgIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlACBB1YxAJfjJb2jAAAAAElFTkSuQmCC";

        public const string INDICATOR_FOCUS = "1";
        public const string INDICATOR_CALLPLAN = "2";
        public const string INDICATOR_QUICKCALL = "0";

        public const string EMPTY_SCRIPT = "EMPTY_SCRIPT";
        public const string ERROR_EMPTY_SCRIPT = "Please load at least one script to save";

        public const string EMAIL_TITLE_MONITOR = "Monitor result agent {0}";
        public const string EMAIL_CONTENT_MONITOR = @"
            <p class='MsoNormal' style='text-align:justify'>
               <span lang='VI' style='font-size: 10pt; line-height: 107%; font-family: &quot;Segoe UI&quot;, sans-serif;'>
	              Xin chào,
		             <b><o:p></o:p></b>                 
               </span>
            </p>
            <p class='MsoNormal' style='text-align:justify'>
               <span lang='VI' style='font-size: 10pt; line-height: 107%; font-family: &quot;Segoe UI&quot;, sans-serif;'>
	              Đây là kết quả đánh giá giao dịch của&nbsp;<b>{0}</b></span></p><p class='MsoNormal' style='text-align:justify'><font face='Segoe UI, sans-serif'><span style='font-size: 13.3333px;'><b>Total mark: {1}<br>Destination: {2}<br>Call date: {3}</b></span></font></p>
            <p class='MsoNormal' style='text-align:justify'>
               <span lang='VI' style='font-size: 10pt; line-height: 107%; font-family: &quot;Segoe UI&quot;, sans-serif;'>
	              Vui lòng truy cập vào đường dẫn bên
	              dưới để xem chi tiết đánh giá giao dịch này, những kĩ năng cần cải thiện và tiến
	              hành huấn luyện cho nhân viên của bạn.</span></p><p class='MsoNormal' style='text-align:justify'><span lang='VI' style='line-height: 107%;'><font face='Segoe UI, sans-serif'><span style='font-size: 13.3333px;'>http://10.210.128.70:8888/Records</span></font></span></p>
            <p class='MsoNormal' style='text-align:justify'>
               <i><span style='font-size:10.0pt;line-height:107%;font-family:&quot;Segoe UI&quot;,sans-serif;
	              mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;color:#0000CC;
	              mso-ansi-language:EN-US;mso-fareast-language:EN-US;mso-bidi-language:AR-SA'>Many thanks and best regards,</span></i><br>
            </p>
        ";
        public const string GIGAMED = "GIGAMED";

        
    }
}