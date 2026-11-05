using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKSHApplication.Controllers
{
    public class FileStoreController : Controller
    {
        private FileStoreService fileStoreService = null;
        private CCCRequestService cccRequestService = null;
        private RaiseIssueService raiseIssueService = null;
        public FileStoreController() : this(new FileStoreServiceImpl(), new CCCRequestServiceImpl(), new RaiseIssueServiceImpl()) { }

        public FileStoreController(FileStoreService fileStoreService, CCCRequestService cccRequestService, RaiseIssueService raiseIssueService)
        {
            this.fileStoreService = fileStoreService;
            this.cccRequestService = cccRequestService;
            this.raiseIssueService = raiseIssueService;
        }

        /// <summary>
        /// Init page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                IEnumerable<TicketNoDto> ticketList = cccRequestService.GetTicketNoList();
                ViewBag.TicketNoList = ticketList;
                IEnumerable<RaiseIssueDto> IssueNoList = raiseIssueService.GetIssueNoList();
                ViewBag.IssueNoList = IssueNoList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        /// <summary>
        /// Attach File
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFiles(FileStoreDto fileDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            
            var userSession = (UserDto)Session[Constant.USER_SESSION];
            if (userSession != null)
            {
                fileDto.CreateBy = userSession.UserId;
                fileDto.Team = userSession.Team;
                List<string> fileNameList = new List<string>(); 
                //Ensure model state is valid
                if (ModelState.IsValid && fileDto.files != null && fileDto.files.Length > 0)
                {
                    var ticketNo = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/FilesRequest/"));
                    //Utils.CreateDirectory(ticketNo);
                    //iterating through multiple file collection 
                    foreach (HttpPostedFileBase file in fileDto.files)
                    {                        
                        //Checking file is available to save.
                        if (file != null)
                        {
                            try
                            {
                                var InputFileName = fileDto.TicketNo + "_" + Path.GetFileName(file.FileName);
                                var ServerSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/FilesRequest/")) + InputFileName;
                                /* Save file to server folder */
                                file.SaveAs(ServerSavePath);
                                FileStoreDto dto = new FileStoreDto();
                                //assigning file uploaded status to ViewBag for showing message to user.
                                dto.FileName = file.FileName;
                                dto.IssueId = fileDto.IssueId;
                                dto.IssueNo = fileDto.IssueNo;
                                dto.RequestId = fileDto.RequestId;
                                dto.CreateBy = fileDto.CreateBy;
                                dto.FileLocation = ticketNo.ToString();
                                dto.FileType = Path.GetExtension(InputFileName);
                                // SAVE INTO TABLE FileStore
                                result = fileStoreService.InsertFile(dto);
                                result.message = String.Format(Constant.FILE_UPLOAD, fileDto.files.Count().ToString());
                            }
                            catch (Exception ex)
                            {
                                result.code = Constant.ERROR;
                                result.message = ex.Message;
                                throw ex;
                            }
                        }
                        else
                        {
                            result.code = Constant.ERROR;
                            result.message = Constant.NO_FILE_SELECTED;
                        }
                    }
                }
                ViewBag.UploadStatus = result;
                ModelState.Clear();
            }

            return RedirectToAction("Index", "FileStore");
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="FileStoreDto"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownLoadFile(string fileName, string fileType, string ticketNo)
        {
            FilePathResult filePathResult = null;
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(ticketNo))
            {                 
                string fileNm = ticketNo + "_" + fileName;
                string contentType = MimeMapping.GetMimeMapping(fileName);
                string filePath = System.Web.HttpContext.Current.Server.MapPath(Constant.FILE_REQUEST_LOCATION + fileNm);
                filePathResult = new FilePathResult(filePath, contentType);
                return File(filePath, contentType, fileNm);
            }
            else
            {
                return RedirectToAction("Index","Error", filePathResult);
            }
        }


        /// <summary>
        /// Get IssueNo By Ticket
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public JsonResult GetIssueNoByTicket(TicketNoDto dto)
        {
            IEnumerable<RaiseIssueDto> lst = raiseIssueService.GetIssueNoByTicket(dto);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search file
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult SearchFile(FileStoreParam param)
        {
            IEnumerable<FileStoreDto> lst = fileStoreService.SearchFile(param);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

    }
}
