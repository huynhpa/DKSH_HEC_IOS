using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Ionic.Zip;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NPOI.SS.UserModel;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Web;
using Ionic.Zlib;
using System.Linq;
using System.Timers;
using DKSHApplication.DTO;
using System.Text.RegularExpressions;

namespace DKSHApplication.Common
{
    public static class Utils
    {
        private static string YYYY_MM_DD = "yyyy-mm-dd";
        private static string DDMMYYYY = "dd-mm-yyyy";
        private static string DDMMYYYY_1 = "dd/mm/yyyy";
        private static string DDMMYYYYHHMM = "dd-MM-yyyy hh:mm";
        private static string YYYYMMDDHHMM = "yyyy-MM-dd hh:mm";
        private static string YYYYMM = "yyyyMM";
        private static string YYYY_MM = "yyyy-MM";
        private static string MMYYYY = "MM-yyyy";
        static Timer timer;

        /// <summary>
        /// string to date format yyyy-mm-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string StringDateFormat(string date)
        {
            try
            {
                if (!string.IsNullOrEmpty(date) && date.Length >= 10)
                {
                    DateTime dt = DateTime.ParseExact(date, DDMMYYYY, CultureInfo.InvariantCulture);
                    return dt.ToString(YYYY_MM_DD);
                }
            }
            catch(Exception ex)
            {
                date = ex.Message;
            }
            
            return date;
        }

        /// <summary>
        /// convert string MM-yyyy to yyyy-MM
        /// </summary>
        /// <param name="mon"></param>
        /// <returns></returns>
        public static string StringMonthFormat(string mon)
        {
            try
            {
                if (!string.IsNullOrEmpty(mon) && mon.Length >= 6)
                {
                    DateTime dt = DateTime.ParseExact(mon, MMYYYY, CultureInfo.InvariantCulture);
                    return dt.ToString(YYYY_MM);
                }
            }
            catch (Exception ex)
            {
                mon = ex.Message;
            }
            return mon;
        }

        /// <summary>
        /// String Date Format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string StringDateFormat1(string date)
        {
            try
            {
                if (!string.IsNullOrEmpty(date) && date.Length >= 10)
                {
                    DateTime dt = DateTime.ParseExact(date, DDMMYYYY_1, CultureInfo.InvariantCulture);
                    return dt.ToString(YYYY_MM_DD);
                }
            }
            catch (Exception ex)
            {
                date = ex.Message;
            }

            return date;
        }

        /// <summary>
        /// Start day of week monday
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            string firstDay = cultureInfo.DateTimeFormat.GetDayName(DayOfWeek.Monday);
            DateTime firstDayInWeek = dayInWeek.Date.AddDays(1);
            while (!firstDayInWeek.DayOfWeek.ToString().Equals(firstDay))
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }
            return firstDayInWeek;
        }


        /// <summary>
        /// string to date format yyyy-mm-dd hh:mm tt
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string StringDateFormatHHMMSS(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime dt = DateTime.ParseExact(date, DDMMYYYYHHMM, CultureInfo.InvariantCulture);
                return dt.ToString(YYYYMMDDHHMM);
            }
            return date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertToYYYYMM(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                DateTime dt = DateTime.ParseExact(date, "MM-yyyy", CultureInfo.InvariantCulture);
                return dt.ToString(YYYYMM);
            }
            return date;
        }


        /// <summary>
        /// Convert String To Date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDate(string date, string format)
        {
            DateTime dt = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
            string d = dt.ToString(format);
            DateTime rs = DateTime.ParseExact(d, format, CultureInfo.InvariantCulture);
            return rs;
        }

        /// <summary>
        /// String To Date DDMMYYYYHHMM
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime StringToDateDDMMYYYYHHMM(string date, string format)
        {
            DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
            string d = dt.ToString(format);
            DateTime rs = DateTime.ParseExact(d, format, CultureInfo.InvariantCulture);
            return rs;
        }

        /// <summary>
        /// String To Date DDMMYYYYHHMM
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime StringToDateDDMMYYYYHHMMSS(string date, string format)
        {
            DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string d = dt.ToString(format);
            DateTime rs = DateTime.ParseExact(d, format, CultureInfo.InvariantCulture);
            return rs;
        }

        /// <summary>
        /// Convert Time
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ConvertTime(string t)
        {
            string span = DateTime.Now.TimeOfDay.ToString();
            if (!string.IsNullOrEmpty(t))
            {
                string temp = t.Substring(0, 2);
                if (temp.Equals("12"))
                {
                    t = t.Replace("AM", "PM");
                    span = DateTime.ParseExact(t, "h:mm tt", CultureInfo.InvariantCulture).ToString();
                }
                else
                {
                    DateTime dateTime = DateTime.ParseExact(t, "hh:mm tt", CultureInfo.InvariantCulture);
                    span = dateTime.TimeOfDay.ToString();
                }
            }
            return span;
        }

        /// <summary>
        /// Convert Format Time
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ConvertFormatTime(string t)
        {
            string span = DateTime.Now.TimeOfDay.ToString();
            if (!string.IsNullOrEmpty(t))
            {
                if (t.Substring(0, 2).Equals("12"))
                {
                    span = DateTime.ParseExact(t, "h:mm tt", CultureInfo.InvariantCulture).ToString();
                }
                else
                {
                    DateTime dateTime = DateTime.ParseExact(t, "HH:mm tt", CultureInfo.InvariantCulture);
                    span = dateTime.TimeOfDay.ToString();
                }
                
            }
            return span;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string CreateSalt(int size)
        {
            var cryptoServiceProvider = new RNGCryptoServiceProvider();
            var buffer = new byte[size];

            cryptoServiceProvider.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] GetByteArray(string inputString)
        {
            return Encoding.UTF8.GetBytes(inputString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetPasswordHashAndSalt(string message)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = GetByteArray(message);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            return GetString(resultBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultBytes"></param>
        /// <returns></returns>
        private static string GetString(byte[] resultBytes)
        {
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// get current date
        /// </summary>
        /// <returns></returns>
        public static DateTime Now()
        {
            DateTime now = DateTime.Today;
            return now;
        }

        /// <summary>
        /// Get Current Date with format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Now(string format)
        {
            DateTime now = DateTime.Today;
            return now.ToString(format);
        }

        /// <summary>
        /// Random Ticket Number
        /// </summary>
        /// <returns></returns>
        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 99999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        /// <summary>
        /// Create Random Password
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// Convert string to date YYYY-MM-DD
        /// </summary>
        /// <returns></returns>
        public static DateTime? StringToDate(string strDate)
        {
            if (!string.IsNullOrEmpty(strDate))
            {
                DateTime dateTime = DateTime.ParseExact(strDate, YYYY_MM_DD, CultureInfo.InvariantCulture);
                return dateTime.Date;
            }
            return null;
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="sendToMail"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool SendMail(string title, string content, string sendToMail, bool cc, string attachment)
        {
            var fromAddress = new MailAddress("noreplydkshtools@gmail.com", "CCC Tools");
            var toAddress = new MailAddress(sendToMail);
            const string fromPassword = "ubpvbmiywjgivejl";
            string subject = title;
            string body = content;
            MailAddress addressCC = new MailAddress("huynh.anh.pham@dksh.com");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.High
            })
            {
                if (cc)
                {
                    message.CC.Add(addressCC);
                }
                if (!string.IsNullOrEmpty(attachment))
                {
                    Attachment attach = new Attachment(attachment);
                    message.Attachments.Add(attach);
                }
                smtp.Send(message);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="sendToMail"></param>
        /// <param name="ccMail"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static bool SendMailMultipleAttacment(string title, string content, string sendToMail, List<string> ccMail, List<string> attachment)
        {
            var fromAddress = new MailAddress("noreplydkshtools@gmail.com", "CCC Tools");
            var toAddress = new MailAddress(sendToMail);
            const string fromPassword = "ubpvbmiywjgivejl";
            string subject = title;
            string body = content;
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.High
            })
            {
                if (ccMail.Count() > 0)
                {
                    foreach (string item in ccMail)
                    {
                        MailAddress addressCC = new MailAddress(item);
                        message.CC.Add(addressCC);
                    }
                }
                if (attachment.Count() > 0)
                {
                    foreach (string att in attachment)
                    {
                        Attachment attach = new Attachment(att);
                        message.Attachments.Add(attach);
                    }
                    
                }
                message.Bcc.Add(Constant.HUYNH_ANH_PHAM_DKSH);
                smtp.Send(message);
                return true;
            }
        }

        /// <summary>
        /// Send mail with multiple attachment
        /// </summary>
        /// <param name="sendMailDto"></param>
        /// <returns></returns>
        public static bool SendMailMultipleAttachment(SendMailDto sendMailDto)
        {
            var fromAddress = new MailAddress("noreplydkshtools@gmail.com", "CCC Tools");
            const string fromPassword = "ubpvbmiywjgivejl";
            string subject = sendMailDto.title;
            string body = sendMailDto.content;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress.Address, sendMailDto.recipient)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.High
            })
            {
                message.From = fromAddress;
                if (sendMailDto.recipientList != null && sendMailDto.recipientList.Count() > 0)
                {
                    foreach (string to in sendMailDto.recipientList)
                    {
                        message.To.Add(to);
                    }
                }
                if (sendMailDto.ccAddress != null && sendMailDto.ccAddress.Count() > 0)
                {
                    foreach (string item in sendMailDto.ccAddress)
                    {
                        MailAddress addressCC = new MailAddress(item);
                        message.CC.Add(addressCC);
                    }
                }
                if (sendMailDto.attachment != null && sendMailDto.attachment.Count() > 0)
                {
                    foreach (string att in sendMailDto.attachment)
                    {
                        Attachment attach = new Attachment(att);
                        message.Attachments.Add(attach);
                    }
                }
                smtp.Send(message);
                return true;
            }
        }

        /// <summary>
        /// <para>You need assign these values in below (data type in string)</para>
        /// <para>1. Title</para>
        /// <para>2. Content</para>
        /// <para>3. Sender email</para>
        /// <para>4. Sender password</para>
        /// <para>5. Receiver</para>
        /// <para>6. CC and BCC if have</para>
        /// <para>7. List file attachment</para>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="sendToMail"></param>
        /// <param name="cc"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static ResultMessageDto SendMailOutlookWithSMTP(SendMailDto sendMailDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            var fromAddress = new MailAddress(sendMailDto.fromAddress, sendMailDto.displayName);
            var toAddress = new MailAddress(sendMailDto.recipient);
            string fromPassword = sendMailDto.fromPassword;
            string subject = sendMailDto.title;
            string body = sendMailDto.content;
            
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                })
                {
                    if (sendMailDto.cc == true)
                    {
                        foreach (string ccMail in sendMailDto.ccAddress)
                        {
                            MailAddress addressCC = new MailAddress(ccMail);
                            message.CC.Add(addressCC);
                        }
                    }
                    if (sendMailDto.bcc == true)
                    {
                        foreach (string bccMail in sendMailDto.bccAddress)
                        {
                            MailAddress addressBCC = new MailAddress(bccMail);
                            message.Bcc.Add(addressBCC);
                        }
                    }
                    if (sendMailDto.attachment.Count() > 0)
                    {
                        foreach (string att in sendMailDto.attachment)
                        {
                            Attachment attach = new Attachment(att);
                            message.Attachments.Add(attach);
                        }
                    }
                    smtp.Send(message);
                    result.flag = true;
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SEND_SUCCESS;
                    return result;
                }
            }
            catch(SmtpFailedRecipientException ex)
            {
                result.flag = false;
                result.code = Constant.ERROR;
                result.message = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Get Formatted Cell Value
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="eval"></param>
        /// <returns></returns>
        public static string GetFormattedCellValue(ICell cell, IFormulaEvaluator eval = null)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue;

                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            DateTime date = cell.DateCellValue;
                            ICellStyle style = cell.CellStyle;
                            // Excel uses lowercase m for month whereas .Net uses uppercase
                            string format = style.GetDataFormatString().Replace('m', 'M');
                            return date.ToString(format);
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString();
                        }

                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "true" : "false";

                    case CellType.Formula:
                        if (eval != null)
                            return GetFormattedCellValue(eval.EvaluateInCell(cell));
                        else
                            return cell.CellFormula;

                    case CellType.Error:
                        return FormulaError.ForInt(cell.ErrorCellValue).String;
                }
            }
            // null or blank cell, or unknown cell type
            return string.Empty;
        }


        /// <summary>
        /// Read File TXT Line By Line
        /// </summary>
        /// <param name="textFilePath"></param>
        /// <returns></returns>
        public static StringBuilder ReadFileTXTLineByLine(string textFilePath)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int counter = 0;
            string line = string.Empty;
            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(textFilePath);
            while ((line = file.ReadLine()) != null)
            {
                stringBuilder.Append(line);
                if (!file.EndOfStream)
                {
                    stringBuilder.Append(Constant.COMMA);
                }                
                counter++;
            }

            file.Close();
            return stringBuilder;
        }

        /// <summary>
        /// Set Value And Format Date
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        /// <param name="formatId"></param>
        public static void SetValueAndFormatDate(IWorkbook workbook, ICell cell, DateTime value)
        {
            //set value for the cell
            if (value != null)
            {
                cell.SetCellValue(value);
            }
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("dd-M-yyyy");
            cell.CellStyle = cellStyle;
        }

        /// <summary>
        /// CheckExcelFormat
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool CheckExcelFormat(string extension)
        {
            try
            {
                bool rs = false;
                if (!string.IsNullOrEmpty(extension))
                {
                    if (extension.Equals(Constant.EXCEL_XLS) || extension.Equals(Constant.EXCEL_XLSX))
                    {
                        rs = true;
                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Send Mail OutLook with Attachment
        /// </summary>
        /// <returns></returns>
        public static bool SendMailOutlookWithInterop(string email, string filePath, string subject, string bodyHtml)
        {
            bool flag = false;
            try
            {                
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = bodyHtml;
                //Add an attachment.
                String sDisplayName = "Attachment";
                int iPosition = oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                Outlook.Attachment oAttach = oMsg.Attachments.Add(filePath, iAttachType, iPosition, sDisplayName);
                //Subject line
                oMsg.Subject = subject;
                oMsg.Importance = Outlook.OlImportance.olImportanceHigh;
                // Add a recipient.
                Outlook.Recipients oRecips = oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = oRecips.Add(email);
                oRecip.Resolve();
                // Send.
                //oMsg.Send();
                flag = true;
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }
            catch (Exception ex)
            {
                flag = false;
                Console.WriteLine(ex);
            }
            return flag;
        }

        /// <summary>
        /// Create Documentation Zip File
        /// </summary>
        /// <param name="documentIdentifier"></param>
        /// <param name="zipDestinationPath"></param>
        /// <param name="documentPaths"></param>
        /// <returns></returns>
        public static string CreateZipFile(int documentIdentifier, string zipDestinationPath, IList<string> documentPaths)
        {
            string zipped = "";
            if (documentPaths.Count > 0)
            {
                using (ZipFile loanZip = new ZipFile())
                {
                    zipped = string.Format("{0}.zip", zipDestinationPath);
                    loanZip.AddFiles(documentPaths, false, string.Empty);
                    loanZip.Save(zipped);
                }
            }
            return zipped;
        }

        /// <summary>
        /// Write Log
        /// </summary>
        public static void WriteLog(string text, string filePath)
        {
            // This text is added only once to the file.
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                string createText = "[RUN PROCESS AND BACK UP] : " + Environment.NewLine;
                File.WriteAllText(filePath, createText);
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            string appendText = text + Environment.NewLine;
            File.AppendAllText(filePath, appendText);

            // Open the file to read from.
            string readText = File.ReadAllText(filePath);
        }


        public static IEnumerable<T> FindSandwichedItem<T>(this IEnumerable<T> items, Predicate<T> matchFilling)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (matchFilling == null)
                throw new ArgumentNullException("matchFilling");

            return FindSandwichedItemImpl(items, matchFilling);
        }

        public static IEnumerable<T> FindSandwichedItemImpl<T>(IEnumerable<T> items, Predicate<T> matchFilling)
        {
            using (var iter = items.GetEnumerator())
            {
                T previous = default(T);
                while (iter.MoveNext())
                {
                    if (matchFilling(iter.Current))
                    {
                        yield return previous;
                        yield return iter.Current;
                        if (iter.MoveNext())
                            yield return iter.Current;
                        else
                            yield return default(T);
                        yield break;
                    }
                    previous = iter.Current;
                }
            }
            // If we get here nothing has been found so return three default values
            yield return default(T); // Previous
            yield return default(T); // Current
            yield return default(T); // Next
        }

        /// <summary>
        /// Schedule Timer
        /// </summary>
        public static void ScheduleTimer()
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 17, 0, 0, 0); //Specify your scheduled time HH,MM,SS [8am and 42 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            timer = new Timer(tickTime);
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            timer.Start();
        }

        /// <summary>
        /// Timer Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("### Timer Stopped ### \n");
            timer.Stop();
            Console.WriteLine("### Scheduled Task Started ### \n\n");
            Console.WriteLine("Performing scheduled task\n");
            Console.WriteLine("### Task Finished ### \n\n");
            ScheduleTimer();
        }

        /// <summary>
        /// Get Monday In Range Dates
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public static List<string> GetMondayInRangeDates(DateTime start_date, DateTime end_date)
        {
            List<string> days_list = new List<string>();
            for (DateTime date = start_date; date <= end_date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    days_list.Add(date.ToShortDateString());
                }
            }
            return days_list;
        }

        /// <summary>
        /// replace special characters to empty in string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSpecialChar(string str)
        {
            try
            {
                str = str.Replace(".","").Replace("+", "").Replace(",", "").Replace("&", "");
                return str;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// check string is date
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckStringIsDate(string str)
        {
            DateTime dateTime = new DateTime();
            try
            {
                dateTime = DateTime.ParseExact(str, "dd/MM/yyyy", null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get Max String
        /// </summary>
        /// <param name="data"></param>
        /// <param name="MaxLength"></param>
        /// <returns></returns>
        public static string GetMaxString(string data, int MaxLength)
        {
            if (string.IsNullOrEmpty(data)) return "";
            string str = data;
            if (data.Length > MaxLength) { 
                str = data.Substring(0, MaxLength - 1);
            }
            return str;
        }

        /// <summary>
        /// Check string Is Guid
        /// </summary>
        /// <param name="GuidId"></param>
        /// <returns></returns>
        public static bool IsGuid(string GuidId)
        {
            if (string.IsNullOrEmpty(GuidId))
                return false;
            string parttern = "########-####-####-####-############";
            string guid = GuidId.ToLower();

            Regex reg = new Regex(parttern.Replace("#", "[a-z0-9]"));
            if (reg.IsMatch(guid) && guid.Length == parttern.Length)
                return true;
            return false;
        }

        /// <summary>
        /// Distinct IEnumerable by object
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// create folder
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFolder(string path)
        {
            try
            {
                if (Directory.Exists(path) == true)
                { }
                else
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// WriteFileLog
        /// </summary>
        /// <param name="serPath"></param>
        /// <param name="message"></param>
        public static void WriteFileLog(string serPath, string message)
        {
            try
            {
                // create folder write logs
                CreateFolder(serPath);
                DateTime now = DateTime.Now;
                string date = string.Format("{0}{1}{2}", now.Year, now.Month, now.Day);
                string datetime = string.Format("{0}:{1}:{2}", new object[] { now.Hour, now.Minute, now.Second });
                string message_full = "[" + now.ToString("yyyyMMdd") + "]" + " " + "[" + datetime + "] [" + "] [Message: " + message + "]";
                FileStream strFile = new FileStream(serPath + date + ".txt", FileMode.Append);
                StreamWriter strWrite = new StreamWriter(strFile, Encoding.UTF8);
                strWrite.WriteLine("*****************");
                strWrite.WriteLine(message_full);
                strWrite.WriteLine("*****************");
                strWrite.Flush();
                strWrite.Close();
                strFile.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


    }
}