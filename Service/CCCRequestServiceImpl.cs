using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class CCCRequestServiceImpl : CCCRequestService
    {
        CCCRequestDao requestDao = new CCCRequestDao();
        UserDao userDao = new UserDao();
        FileStoreDao fileDao = new FileStoreDao();
        /// <summary>
        /// SEARCH REQUEST
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<CCCRequestDto> SearchRequest(CCCRequestParam param)
        {
            IEnumerable<CCCRequestDto> listRequest = requestDao.SearchRequest(param);
            return listRequest;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketNoDto> GetTicketNoList()
        {
            IEnumerable<TicketNoDto> ticketList = requestDao.GetTicketNoList();
            return ticketList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get TicketNo
        /// </summary>
        /// <returns></returns>
        public CCCRequestDto GetTicketNo(CCCRequestParam jsonPr)
        {
            CCCRequestDto cccRequestDto = new CCCRequestDto();
            int randomNo = Utils.GenerateRandomNo();
            cccRequestDto.TicketNo = String.Format("{0}-{1}-{2}", jsonPr.Team, jsonPr.UserId, randomNo);
            return cccRequestDto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public CCCRequestDto GetRequestDetail(CCCRequestParam jsonParam)
        {
            CCCRequestDto cccRequestDto = new CCCRequestDto();
            cccRequestDto = requestDao.GetRequestDetail(jsonParam);
            return cccRequestDto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert request
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto InsertRequest(CCCRequestDto jsonParam)
        {
            ResultMessageDto resultMessgeDto = new ResultMessageDto();
            if (jsonParam != null)
            {
                if (jsonParam.IsManager == true)
                {
                    jsonParam.Approval = Constant.REQUEST_APPROVED;
                }
                else
                {
                    jsonParam.Approval = Constant.REQUEST_AWAITING;
                }
                resultMessgeDto.number = requestDao.InsertRequest(jsonParam);
                // GET MANAGER BY TEAM
                IEnumerable<UserDto> userDto = userDao.GetManagerByTeam(jsonParam.Team);
                // SEND MAIL
                object[] param = new object[4];
                param[0] = jsonParam.TicketNo;
                param[1] = jsonParam.Title;
                param[2] = jsonParam.Email;
                foreach (UserDto u in userDto)
                {
                    param[3] = u.UserName;
                    Utils.SendMail(Constant.TITLE_NEW_REQUEST, String.Format(Constant.CONFIRM_NEW_REQUEST, param), u.Email, true, string.Empty);
                }
                if (resultMessgeDto.number > 0)
                {
                    resultMessgeDto.code = Constant.INSERT;
                    resultMessgeDto.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    resultMessgeDto.number = 0;
                    resultMessgeDto.code = Constant.ERROR;
                    resultMessgeDto.message = Constant.MESSAGE_ERROR;
                }
                return resultMessgeDto;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update request
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateRequest(CCCRequestDto jsonParam)
        {
            ResultMessageDto resultMessgeDto = new ResultMessageDto();
            if (jsonParam != null)
            {
                resultMessgeDto.number = requestDao.UpdateRequest(jsonParam);
                if (resultMessgeDto.number > 0)
                {
                    resultMessgeDto.code = Constant.UPDATE;
                    resultMessgeDto.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    resultMessgeDto.number = 0;
                    resultMessgeDto.code = Constant.ERROR;
                    resultMessgeDto.message = Constant.MESSAGE_ERROR;
                }
                return resultMessgeDto;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// delete request
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteRequest(CCCRequestDto jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.number = requestDao.DeleteRequest(jsonParam);
            if (result.number == 1)
            {
                result.code = Constant.DELETE;
                result.message = Constant.MESSAGE_DELELE;
                CCCRequestParam dto = new CCCRequestParam();
                result.RequestList = requestDao.SearchRequest(dto);
            }
            else
            {
                result.code = Constant.EXISTED;
                result.message = String.Format(Constant.NOT_EXIST, jsonParam.TicketNo);
            }
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Approved OR Rejected Request
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto Approval(CCCRequestDto jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            if (jsonParam != null)
            {
                UserDto userDto = userDao.GetEmailByUserId(jsonParam.CreateBy);
                FileStoreDto fileDto = new FileStoreDto();
                fileDto.RequestId = jsonParam.Id;
                IEnumerable<FileStoreDto> fileList = fileDao.GetFileByTicket(fileDto);
                result.number = requestDao.ApprovalRequest(jsonParam);
                if (result.number > 0)
                {
                    result.code = Constant.UPDATE;
                    result.message = Constant.MESSAGE_SUCCESS;
                    object[] format = new object[4];
                    format[0] = jsonParam.Approval;
                    format[1] = jsonParam.TicketNo;
                    format[2] = jsonParam.Title;
                    string content = string.Empty;
                    if (jsonParam.Approval.Equals(Constant.REQUEST_APPROVED))
                    {
                        content = Constant.APPROVAL_CONTENT;
                        foreach (FileStoreDto item in fileList)
                        {
                            jsonParam.FileLocation += item.FileName + "<br>";
                        }
                        format[3] = jsonParam.FileLocation;
                    }
                    else if (jsonParam.Approval.Equals(Constant.REQUEST_REJECTED))
                    {
                        content = Constant.REJECTED_CONTENT;
                        format[3] = jsonParam.Reason;
                    }
                    Utils.SendMail(string.Format(Constant.APPROVAL, format[0]), string.Format(content, format), userDto.Email, true, string.Empty);
                }
            }
            return result;
            throw new NotImplementedException();
        }
    }
}