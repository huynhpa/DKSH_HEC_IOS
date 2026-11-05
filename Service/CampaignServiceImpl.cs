using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.Common;
using DKSHApplication.DAO;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace DKSHApplication.Service
{
    public class CampaignServiceImpl : CampaignService
    {
        CampaignDao campaignDao = new CampaignDao();

        public ServiceTypeDto CampaignDetail(ServiceTypeDto param)
        {
            ServiceTypeDto result = campaignDao.CampaignDetail(param);
            return result;
        }

        /// <summary>
        /// Delete Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto DeleteCampaign(ServiceTypeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                result.number = campaignDao.DeleteCampaign(dto);
                if (result.number > 0)
                {
                    result.code = Constant.DELETE;
                    result.message = string.Format(Constant.MESSAGE_DELELE, result.number);
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
        /// GetCampaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CampaignDto GetCampaign(CampaignDto dto)
        {
            CampaignDto campaign = new CampaignDto();
            try
            {
                List<ProjectDto> ProjectList = new List<ProjectDto>();
                List<ServiceTypeDto> ServiceTypeList = new List<ServiceTypeDto>();
                ProjectList = campaignDao.ProjectActive().ToList();
                ServiceTypeList = campaignDao.SearchCampaign(dto).ToList();
                campaign.ProjectList = ProjectList;
                campaign.ServiceTypeList = ServiceTypeList;
            }
            catch (Exception ex)
            {
                campaign.code = Constant.ERROR;
                campaign.message = ex.Message;
            }
            return campaign;
        }

        /// <summary>
        /// Insert Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto InsertCampaign(ServiceTypeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                List<string> project_id_lst = new List<string>();
                if (!string.IsNullOrEmpty(dto.ProjectIDList))
                {
                    project_id_lst = dto.ProjectIDList.Split(',').ToList();
                    foreach (string prj in project_id_lst)
                    {
                        dto.ProjectID = Guid.Parse(prj);
                        dto.ID = Guid.NewGuid();
                        result.number = campaignDao.InsertCampaign(dto);
                    }
                }
                else
                {
                    dto.ID = Guid.NewGuid();
                    result.number = campaignDao.InsertCampaign(dto);
                }
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
        /// Search
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<ServiceTypeDto> Search(CampaignDto dto)
        {
            IEnumerable<ServiceTypeDto> ServiceTypeList = campaignDao.SearchCampaign(dto);
            return ServiceTypeList.ToList();
        }

        /// <summary>
        /// Update Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResultMessageDto UpdateCampaign(ServiceTypeDto dto)
        {
            ResultMessageDto result = new ResultMessageDto();
            try
            {
                List<string> project_id_lst = new List<string>();
                if (!string.IsNullOrEmpty(dto.ProjectIDList))
                {
                    project_id_lst = dto.ProjectIDList.Split(',').ToList();
                    foreach (string prj in project_id_lst)
                    {
                        dto.ProjectID = Guid.Parse(prj);
                        result.number = campaignDao.UpdateCampaign(dto);
                    }
                }
                else
                {
                    result.number = campaignDao.UpdateCampaign(dto);
                }
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
        /// ViewAgentInCampaign
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IWorkbook ViewAgentInCampaign(SearchCallDto json)
        {
            IWorkbook workbook;
            string extension = Constant.EXCEL_XLSX;
            var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ExcelTemplate/")) + "AgentInCampaign.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                if (Constant.EXCEL_XLSX.Equals(extension))
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    throw new Exception("This format is not supported");
                }
                
                ISheet sheet = workbook.GetSheet("Agent In Campaign");
                int rowIdx = 1;
                IEnumerable<SearchCallDto> agentInCampaign = campaignDao.ViewAgentInCampaign(json);
                foreach (SearchCallDto item in agentInCampaign)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    row.CreateCell(0).SetCellValue(item.SalemanCode);
                    row.CreateCell(1).SetCellValue(item.EmployeeName);
                    row.CreateCell(2).SetCellValue(item.Agent);
                    row.CreateCell(3).SetCellValue(item.Campaign);
                    rowIdx += 1;
                }
                file.Close();
            }
            return workbook;
        }
    }
}