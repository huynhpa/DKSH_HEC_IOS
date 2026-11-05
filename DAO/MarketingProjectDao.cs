using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class MarketingProjectDao
    {
        private GenericRepository<MarketingProjectDto> projectRepo;

        private GenericRepository<SalemanCodeDto> salemanCodeRepo;

        private GenericRepository<ProductsGroupDto> productGroupRepo;

        private GenericRepository<CustCodeByProductDto> custCodeByProductRepo;

        public CCCToolsEntity cccToolsEntity = null;

        public MarketingProjectDao()
        {
            projectRepo = new GenericRepository<MarketingProjectDto>(new CCCToolsEntity());
            salemanCodeRepo = new GenericRepository<SalemanCodeDto>(new CCCToolsEntity());
            productGroupRepo = new GenericRepository<ProductsGroupDto>(new CCCToolsEntity());
            custCodeByProductRepo = new GenericRepository<CustCodeByProductDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search Marketing Project
        /// </summary>
        /// <param name="paramDto"></param>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> SearchMarketingProject(MarketingProjectDto paramDto)
        {
            object[] param = { paramDto.ProjectName, paramDto.CampaignName, Utils.StringDateFormat(paramDto.ValidFrom),
                               Utils.StringDateFormat(paramDto.ValidTo), paramDto.IsDelete };
            string spQuery = "EXEC [MarketingProjectList] {0}, {1}, {2}, {3}, {4}";
            return projectRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Project By Id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MarketingProjectDto GetProjectById(MarketingProjectDto dto)
        {
            object[] param = { dto.Id };
            string spQuery = "EXEC [GetProjectById] {0}";
            return projectRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int Insert(MarketingProjectDto dto)
        {
            object[] param = { dto.ProjectName, dto.ProjectDescription, dto.ProjectLeader, dto.CampaignType, dto.CampaignName,
                               Utils.StringDateFormat(dto.ValidFrom), Utils.StringDateFormat(dto.ValidTo), dto.ProductGroup, dto.Products, dto.MinTarget,
                               dto.MaxTarget, dto.ResourceSale, dto.Status, dto.Remark, dto.Channel,
                               dto.Customers, dto.Province, dto.Region, dto.MinAmount, dto.MinQuantity, dto.CreateBy, dto.UpdateBy,
                               dto.InExCludedCust, dto.InExCludedRegion, dto.InExCludedDistrict, dto.InExCludedChannel, dto.ProductList,
                               dto.ProjectNumber, dto.IsSelectedResource, dto.ProductExtension, dto.CustomerExtension, dto.PONumber,
                               dto.TargetFollow, dto.InExCludedProvince, dto.IsFOC
                             };
            string spQuery = @"EXEC [InsertMarketingProject] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10},
                                                             {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22},
                                                             {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}";
            return projectRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int Update(MarketingProjectDto dto)
        {
            object[] param = { dto.ProjectName, dto.ProjectDescription, dto.ProjectLeader, dto.CampaignType, dto.CampaignName,
                               Utils.StringDateFormat(dto.ValidFrom), Utils.StringDateFormat(dto.ValidTo),
                               dto.ProductGroup, dto.Products, dto.MinTarget,
                               dto.MaxTarget, dto.ResourceSale, dto.Status, dto.Remark, dto.Channel,
                               dto.Customers, dto.Province, dto.Region, dto.MaxQuantity, dto.MinQuantity, dto.UpdateBy,
                               dto.IsDelete, dto.Id, dto.InExCludedCust, dto.InExCludedRegion,
                               dto.InExCludedDistrict, dto.InExCludedChannel, dto.ProductList, dto.IsSelectedResource,
                               dto.ProductExtension, dto.CustomerExtension, dto.PONumber, dto.TargetFollow, dto.InExCludedProvince,
                               dto.IsFOC };
            string spQuery = @"EXEC [UpdateMarketingProject] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10},
                                                             {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22},
                                                             {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}";
            return projectRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int Delete(MarketingProjectDto dto)
        {
            try
            {
                var query = (from p in cccToolsEntity.MarketingProjects
                             where p.Id == dto.Id
                             select p).First();
                query.IsDelete = true;
                int count = cccToolsEntity.SaveChanges();
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Saleman Code
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SalemanCodeDto> GetSalemanCode(SalemanCodeDto json)
        {
            object[] param = { json.SalemanCode, json.FullName, json.Team };
            string spQuery = "EXEC [GetSalemanCode] {0}, {1}, {2}";
            return salemanCodeRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Team
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> TeamList()
        {
            try
            {
                var query = (from t in cccToolsEntity.SalemanCodes
                             select t.Team).Distinct();
                return query.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Channel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ChannelDto> GetChannel()
        {
            try
            {
                var query = (from c in cccToolsEntity.Channels
                             select new ChannelDto()
                             {
                                 Id = c.Id,
                                 Channel = c.Channel
                             });
                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Region
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RegionDto> GetRegion()
        {
            try
            {
                var query = (from r in cccToolsEntity.Regions
                             select new RegionDto()
                             {
                                 Code = r.Code,
                                 RegionName = r.RegionName
                             });
                return query.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Project Selected
        /// </summary>
        /// <param name="projectList"></param>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> GetProjectSelected(ProjectProcessParam projectList)
        {
            object[] param = { projectList.ProjectIdList };
            string spQuery = @"EXEC [GetProjectSelected] {0}";
            return projectRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// Get Cust Group Code By Channel
        /// </summary>
        /// <param name="projectList"></param>
        /// <returns></returns>
        public IEnumerable<CGrpCodeDto> GetCustGroupCodeByChannel(ChannelDto channel)
        {
            List<string> channelList = new List<string>();
            if (!string.IsNullOrEmpty(channel.Channel))
            {                
                channelList = channel.Channel.Split(',').ToList();
                for (int i = 0; i < channelList.Count(); i++)
                {
                    if (channelList[i].Equals(string.Empty))
                    {
                        channelList.RemoveAt(i);
                        break;
                    }
                }
            }            
            try
            {
                var query = (from c in cccToolsEntity.CGrpCodes                             
                             select new CGrpCodeDto {
                                 CGroupCode = c.CGroupCode,
                                 CGroupName = c.CGroupName,
                                 Channel = c.Channel
                             }).Where(t => channelList.Contains(t.Channel)).OrderBy(x => x.Channel);
                return query.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Project Name List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> GetProjectNameList()
        {
            try
            {
                var query = (from p in cccToolsEntity.MarketingProjects
                             select new MarketingProjectDto {
                                 Id = p.Id,
                                 ProjectNumber = p.ProjectNumber,
                                 ProjectName = p.ProjectName
                             }).ToList();
                return query;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Project Over View
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MarketingProjectDto> ProjectOverView(MarketingProjectDto obj)
        {
            object[] param = { obj.ProjectStatus };
            string spQuery = "EXEC [ProjectOverView] {0}";
            return projectRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Update Project Status
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int UpdateProjectStatus(MarketingProjectDto pr)
        {
            object[] param = { pr.ProjectStatus, pr.ProjectNumber };
            string spQuery = @"EXEC [UpdateProjectStatus] {0}, {1}";
            return projectRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Insert Product Group
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int InsertProductGroup(ProductsGroupDto pr)
        {
            object[] param = { pr.ProductGroup, pr.ProductCode, pr.ProductName, pr.ProjectNumber };
            string spQuery = @"EXEC [sp_InsertProductGroup] {0}, {1}, {2}, {3}";
            return projectRepo.ExecuteCommand(spQuery, param);
        }


        /// <summary>
        /// Delete Product Group
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int DeleteProductGroup(MarketingProjectDto pr)
        {
            object[] param = { pr.ProjectNumber };
            string spQuery = @"EXEC [sp_DeleteProductGroup] {0}";
            return projectRepo.ExecuteCommand(spQuery, param);
        }



        /// <summary>
        /// Insert Cust Code By Product
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int InsertCustCodeByProduct(CustCodeByProductDto pr)
        {
            object[] param = { pr.CustCode, pr.ProductGroup, pr.ProjectNumber, pr.InExcluded };
            string spQuery = @"EXEC [sp_InsertCustCodeByProduct] {0}, {1}, {2}, {3}";
            return custCodeByProductRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Check Cust Code By Product
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public int CheckCustCodeByProduct(CustCodeByProductDto pr)
        {
            object[] param = { pr.CustCode, pr.ProductGroup, pr.ProjectNumber };
            string spQuery = @"EXEC [sp_CheckCustCodeByProduct] {0}, {1}, {2}";
            return custCodeByProductRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteDataRawByProject(MarketingProjectDto dto)
        {
            object[] param = { dto.ProjectNumber };
            string spQuery = @"EXEC [sp_DeleteDataPick] {0}";
            return custCodeByProductRepo.ExecuteCommand(spQuery, param);
            
        }
    }
}