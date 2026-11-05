using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using MP.Core.Library;

namespace DKSHApplication.Service
{
    public class QuestionServiceImpl : QuestionService
    {
        QuestionDao questionDao = new QuestionDao();
        AnswerDao answerDao = new AnswerDao();
        ServiceQuestionsDao serviceQuestionsDao = new ServiceQuestionsDao();

        public IEnumerable<QuestionDto> LoadServiceDetail(OBSurveyParam param)
        {
            IEnumerable<QuestionDto> result = questionDao.LoadServiceDetail(param);
            return result;
        }

        /// <summary>
        /// Get Question Detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public QuestionDto GetQuestionDetail(QuestionDto dto)
        {
            QuestionDto question = questionDao.GetQuestionByID(dto);
            return question;
        }

        /// <summary>
        /// Insert Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertQuestion(QuestionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                if (!CheckQuestionCodeExist(dto))
                {
                    dto.ID = Guid.NewGuid();
                    result.number = questionDao.InsertQuestion(dto);
                    result.number += SaveServiceQuestions(dto);
                    result.Question = dto;
                    if (result.number > 0)
                    {
                        result.code = Constant.SUCCESS;
                        result.message = Constant.MESSAGE_SUCCESS;
                    }
                    else
                    {
                        result.code = Constant.ERROR;
                        result.message = Constant.NO_ROW_SAVE;
                    }
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.QUESTION_CODE_EXISTED;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            
            return result;
        }

        /// <summary>
        /// UpdateQuestion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateQuestion(QuestionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = questionDao.UpdateQuestion(dto);
                result.number += SaveServiceQuestions(dto);
                result.Question = dto;
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// DeleteQuestion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteQuestion(QuestionDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = questionDao.DeleteQuestion(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
                }
                else
                {
                    result.code = Constant.INFO;
                    result.message = Constant.MESSAGE_INFO;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Get Question
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<QuestionDto> GetQuestion(QuestionDto dto)
        {
            IEnumerable<QuestionDto> result = questionDao.GetQuestion(dto);
            return result;
        }

        /// <summary>
        /// GetQuestionByID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public QuestionDto GetQuestionByID(QuestionDto dto)
        {
            QuestionDto result = questionDao.GetQuestionByID(dto);
            return result;
        }

        /// <summary>
        /// Insert / Update Service Questions
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private int SaveServiceQuestions(QuestionDto dto)
        {
            int flag = 0;
            if (dto.ID != Guid.Empty)
            {
                ServiceQuestionsDto obj = new ServiceQuestionsDto();
                obj.ServiceTypeID = FGuid.ToGuid(dto.ServiceTypeID);
                obj.QuestionID = FGuid.ToGuid(dto.ID);
                obj.OrderIndex = dto.OrderIndex;
                ServiceQuestionsDto tmp = serviceQuestionsDao.GetByQuestionID(obj);
                if (tmp == null)
                {
                    obj.ID = Guid.NewGuid();
                    obj.CreatedBy = dto.CreatedBy;
                    obj.UpdatedBy = dto.UpdatedBy;
                    obj.IsDeleted = false;
                    flag = serviceQuestionsDao.InsertServiceQuestions(obj);
                }
                else
                {
                    obj.ID = tmp.ID;
                    obj.UpdatedBy = dto.UpdatedBy;
                    obj.IsDeleted = tmp.IsDeleted;
                    flag = serviceQuestionsDao.UpdateServiceQuestions(obj);
                }
            }
            return flag;
        }

        /// <summary>
        /// AnswerType
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AnswerTypeDto> AnswerType()
        {
            IEnumerable<AnswerTypeDto> result = questionDao.AnswerType();
            return result;
        }

        /// <summary>
        /// GetAnswerByQuestion
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<AnswerDto> GetAnswerByQuestion(AnswerDto dto)
        {
            IEnumerable<AnswerDto> result = answerDao.GetByQuestion(dto);
            return result;
        }

        /// <summary>
        /// get answer detail by id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public AnswerDto GetAnswerByID(AnswerDto dto)
        {
            AnswerDto result = answerDao.GetAnswerByID(dto);
            return result;
        }


        /// <summary>
        /// Update answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateAnswer(AnswerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = answerDao.UpdateAnswer(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            
            return result;
        }

        /// <summary>
        /// Insert answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertAnswer(AnswerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                dto.ID = Guid.NewGuid();
                result.number = answerDao.InsertAnswer(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = Constant.MESSAGE_SUCCESS;
                }
                else
                {
                    result.code = Constant.ERROR;
                    result.message = Constant.NO_ROW_SAVE;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Delete Answer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteAnswer(AnswerDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = answerDao.DeleteAnswer(dto);
                if (result.number > 0)
                {
                    result.code = Constant.SUCCESS;
                    result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
                }
                else
                {
                    result.code = Constant.INFO;
                    result.message = Constant.MESSAGE_INFO;
                }
            }
            catch (Exception ex)
            {
                result.code = Constant.ERROR;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Check Question Code Exist
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private bool CheckQuestionCodeExist(QuestionDto dto)
        {
            bool flag = false;
            IEnumerable<QuestionDto> cntQuestion = questionDao.CheckQuestionCodeExist(dto);
            if (cntQuestion.Count() > 0)
            {
                flag = true;
            }
            return flag;
        }
    }
}