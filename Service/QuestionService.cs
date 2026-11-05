using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface QuestionService
    {
        IEnumerable<QuestionDto> LoadServiceDetail(OBSurveyParam param);
        QuestionDto GetQuestionDetail(QuestionDto dto);
        ResultMessageDto InsertQuestion(QuestionDto dto);
        ResultMessageDto UpdateQuestion(QuestionDto dto);
        ResultMessageDto DeleteQuestion(QuestionDto dto);
        IEnumerable<QuestionDto> GetQuestion(QuestionDto dto);
        QuestionDto GetQuestionByID(QuestionDto dto);
        IEnumerable<AnswerTypeDto> AnswerType();
        IEnumerable<AnswerDto> GetAnswerByQuestion(AnswerDto dto);
        AnswerDto GetAnswerByID(AnswerDto dto);
        ResultMessageDto UpdateAnswer(AnswerDto dto);
        ResultMessageDto InsertAnswer(AnswerDto dto);
        ResultMessageDto DeleteAnswer(AnswerDto dto);
    }
}
