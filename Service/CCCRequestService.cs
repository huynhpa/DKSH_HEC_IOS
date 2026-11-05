using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface CCCRequestService
    {
        /// <summary>
        /// SEARCH REQUEST
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<CCCRequestDto> SearchRequest(CCCRequestParam param);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<TicketNoDto> GetTicketNoList();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        CCCRequestDto GetTicketNo(CCCRequestParam jsonParam);

        /// <summary>
        /// get detail
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        CCCRequestDto GetRequestDetail(CCCRequestParam jsonParam);

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto InsertRequest(CCCRequestDto jsonParam);

        /// <summary>
        /// update
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto UpdateRequest(CCCRequestDto jsonParam);

        /// <summary>
        /// IsDelete = 1
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto DeleteRequest(CCCRequestDto jsonParam);

        /// <summary>
        /// Approval
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto Approval(CCCRequestDto jsonParam);
    }
}
