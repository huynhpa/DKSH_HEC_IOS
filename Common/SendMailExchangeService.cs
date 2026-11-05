using DKSHApplication.DTO;
using System;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Autodiscover;
using System.IO;
using System.Web;

namespace DKSHApplication.Common
{
    public class SendMailExchangeService
    {
        /// <summary>
        /// Send mail exchange outlook
        /// </summary>
        /// <param name="sendMailDto"></param>
        /// <returns></returns>
        public static ResultMessageDto SendMailSMTP(SendMailDto sendMailDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            ExchangeService myservice = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            myservice.Credentials = new WebCredentials(sendMailDto.fromAddress, sendMailDto.fromPassword);
            try
            {
                string serviceUrl = "https://outlook.office365.com/ews/exchange.asmx";
                myservice.Url = new Uri(serviceUrl);
                EmailMessage emailMessage = new EmailMessage(myservice);
                emailMessage.Subject = sendMailDto.title;
                emailMessage.Body = new MessageBody( sendMailDto.content);
                if (sendMailDto.recipientList != null && sendMailDto.recipientList.Count() > 0)
                {
                    foreach (string recipient in sendMailDto.recipientList)
                    {
                        emailMessage.ToRecipients.Add(recipient);
                    }
                }
                if (sendMailDto.ccAddress != null && sendMailDto.ccAddress.Count() > 0)
                {
                    foreach (string cc in sendMailDto.ccAddress)
                    {
                        emailMessage.CcRecipients.Add(cc);
                    }
                }
                if (sendMailDto.attachment != null && sendMailDto.attachment.Count() > 0)
                {
                    foreach (string item in sendMailDto.attachment)
                    {
                        emailMessage.Attachments.AddFileAttachment(item);
                    }
                }
                emailMessage.Importance = Importance.High;
                emailMessage.SendAndSaveCopy();
                result.flag = true;
                result.code = Constant.SUCCESS;
                result.message = Constant.MESSAGE_SEND_SUCCESS;
            }
            catch (UnauthorizedAccessException au)
            {
                result.flag = false;
                result.code = Constant.ERROR;
                result.message = au.Message;
            }
            catch (SmtpException exception)
            {
                result.flag = false;
                result.code = Constant.ERROR;
                result.message = exception.Message;
            }
            catch (AutodiscoverRemoteException exception)
            {
                result.flag = false;
                result.code = Constant.ERROR;
                result.message = exception.Message;
            }
            return result;
        }

        /// <summary>
        /// Read Mail Specificate
        /// </summary>
        public static ResultMessageDto ReadMailSpecificate(ReadMailDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (dto != null && !string.IsNullOrEmpty(dto.Email) && !string.IsNullOrEmpty(dto.Password))
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new WebCredentials(dto.Email, dto.Password);
                try
                {
                    string serviceUrl = "https://outlook.office365.com/ews/exchange.asmx";
                    service.Url = new Uri(serviceUrl);
                    EmailMessage emailMessage = new EmailMessage(service);
                    service.AutodiscoverUrl(dto.EmailDiscover);
                    ItemView view = new ItemView(10);
                    FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(10));
                    string currDate = DateTime.Now.ToString("ddMMyyyy");
                    var folder = Path.Combine(HttpContext.Current.Server.MapPath("~/FilesUpload/FetchEmail/"));
                    bool exists = Directory.Exists(folder + currDate);
                    string saveDir = folder + currDate + "/";
                    if (!exists) {
                        Directory.CreateDirectory(saveDir);
                    }
                    if (findResults != null && findResults.Items != null && findResults.Items.Count > 0)
                    {
                        ReadMailDto rm = null;
                        foreach (Item item in findResults.Items)
                        {
                            EmailMessage message = EmailMessage.Bind(service, item.Id, new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments, ItemSchema.HasAttachments));
                            foreach (Microsoft.Exchange.WebServices.Data.Attachment attachment in message.Attachments)
                            {
                                if (attachment is FileAttachment)
                                {
                                    rm = new ReadMailDto();
                                    FileAttachment fileAttachment = attachment as FileAttachment;
                                    fileAttachment.Load();
                                    rm.FileName = fileAttachment.Name;
                                    result.readMailDto.Add(rm);
                                    using (FileStream fs = new FileStream(saveDir + attachment.Name, FileMode.Create))
                                    {
                                        using (BinaryWriter w = new BinaryWriter(fs))
                                        {
                                            w.Write(fileAttachment.Content);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException au)
                {
                    result.flag = false;
                    result.code = Constant.ERROR;
                    result.message = au.Message;
                }
                catch (SmtpException exception)
                {
                    result.flag = false;
                    result.code = Constant.ERROR;
                    result.message = exception.Message;
                }
                catch (AutodiscoverRemoteException exception)
                {
                    result.flag = false;
                    result.code = Constant.ERROR;
                    result.message = exception.Message;
                }
            }
            else
            {
                result.flag = false;
                result.code = Constant.ERROR;
                result.message = "Couldn't login into this account";
            }
            return result;
        }
    }
}