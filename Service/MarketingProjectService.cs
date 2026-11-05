using DKSHApplication.DB;
using DKSHApplication.DTO;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DKSHApplication.Service
{
    public interface MarketingProjectService
    {
        /// <summary>
        /// Search
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        IEnumerable<MarketingProjectDto> Search(MarketingProjectDto json);

        /// <summary>
        /// Get Project By Id
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        MarketingProjectDto GetProjectById(MarketingProjectDto json);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto Insert(MarketingProjectDto json);

        /// <summary>
        /// Get Project Name List
        /// </summary>
        /// <returns></returns>
        IEnumerable<MarketingProjectDto> GetProjectNameList();

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto Update(MarketingProjectDto json);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto Delete(MarketingProjectDto json);

        /// <summary>
        /// Get Saleman Code
        /// </summary>
        /// <returns></returns>
        IEnumerable<SalemanCodeDto> GetSalemanCode(SalemanCodeDto json);

        /// <summary>
        /// Team List
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> TeamList();

        /// <summary>
        /// Get Channel
        /// </summary>
        /// <returns></returns>
        IEnumerable<ChannelDto> GetChannel();

        /// <summary>
        /// Get Region
        /// </summary>
        /// <returns></returns>
        IEnumerable<RegionDto> GetRegion();

        /// <summary>
        /// Process Project
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        ResultMessageDto ProcessProject(IEnumerable<MarketingProjectDto> jsonArr, string currDate, string userId);

        /// <summary>
        /// Get Project Selected
        /// </summary>
        /// <param name="projectList"></param>
        /// <returns></returns>
        IEnumerable<MarketingProjectDto> GetProjectSelected(ProjectProcessParam projectList);

        /// <summary>
        /// Get Marketing Project By Project Number
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        IEnumerable<MarketingDataRawDto> GetMarketingProjectByProjectNumber(MarketingProjectDto projectDto);

        /// <summary>
        /// Export Data Pick
        /// </summary>
        /// <param name="projectDataPick"></param>
        /// <returns></returns>
        IWorkbook ExportDataPick(IEnumerable<MarketingDataRawDto> projectDataPick, MarketingProjectDto projectDto);

        /// <summary>
        /// Count Data Raw By Project Number
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<DataRawResult> CountDataRawByProjectNumber(ProjectProcessParam ProjectNumberList);

        /// <summary>
        /// Project Over View
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<MarketingProjectDto> ProjectOverView(MarketingProjectDto param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultMessageDto UpdateDataPick(HttpPostedFileBase aFile, MarketingProjectDto dto);
    }
}
