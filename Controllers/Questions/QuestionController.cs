using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DKSHApplication.Common;
using DKSHApplication.DTO;
using DKSHApplication.Service;


namespace DKSHApplication.Controllers.Questions
{
    public class QuestionController : Controller
    {

        private QuestionService questionService;

        public QuestionController() : this(new QuestionServiceImpl())
        { }
        public QuestionController(QuestionService questionService)
        {
            this.questionService = questionService;
        }
        //
        // GET: /Questions/

        public ActionResult Index(string campaignId)
        {
            if (Session[Constant.USER_SESSION] != null)
            {
                if (!string.IsNullOrEmpty(campaignId))
                {
                    ViewBag.CampaignID = campaignId;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /// <summary>
        /// GetQuestionDetail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetQuestionDetail(QuestionDto dto)
        {
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            QuestionDto question = questionService.GetQuestionDetail(dto);
            var jsonResult = Json(question, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        

        /// <summary>
        /// Get Question List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetQuestion(QuestionDto dto)
        {
            IEnumerable<QuestionDto> questions = questionService.GetQuestion(dto);
            var jsonResult = Json(questions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetQuestionByID(QuestionDto dto)
        {
            QuestionDto questions = questionService.GetQuestionByID(dto);
            var jsonResult = Json(questions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Insert Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertQuestion(QuestionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.CreatedBy = usersession.UserName;
                dto.UpdatedBy = usersession.UserName;
                result = questionService.InsertQuestion(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// update question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateQuestion(QuestionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.CreatedBy = usersession.UserName;
                dto.UpdatedBy = usersession.UserName;
                result = questionService.UpdateQuestion(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Load answers type list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AnswerType()
        {
            IEnumerable<AnswerTypeDto> answerType = questionService.AnswerType();
            var jsonResult = Json(answerType, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteQuestion(QuestionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.UpdatedBy = usersession.UserName;
                result = questionService.DeleteQuestion(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteAnswer(AnswerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.UpdatedBy = usersession.UserName;
                result = questionService.DeleteAnswer(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Answer By Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetAnswerByQuestion(AnswerDto dto)
        {            
            IEnumerable<AnswerDto> result = questionService.GetAnswerByQuestion(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAnswerByID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetAnswerByID(AnswerDto dto)
        {
            AnswerDto result = questionService.GetAnswerByID(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateAnswer(AnswerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.UpdatedBy = usersession.UserName;
                result = questionService.UpdateAnswer(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert Answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertAnswer(AnswerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            MemberShipDto usersession = (MemberShipDto)Session[Constant.USER_SESSION];
            if (usersession != null)
            {
                dto.CreatedBy = usersession.UserName;
                dto.UpdatedBy = usersession.UserName;
                result = questionService.InsertAnswer(dto);
            }
            else
            {
                result.code = Constant.SESSION_EXPIRED_CODE;
                result.message = Constant.SESSION_EXPIRED_MESSAGE;
            }            
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
