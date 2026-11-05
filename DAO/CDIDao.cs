using DKSHApplication.Common;
using DKSHApplication.DAOBusiness;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class CDIDao
    {
        private GenericRepository<CDIDto> cdiRepository;

        private GenericRepository<CertificationDto> CertificateRepo;

        private GenericRepository<CertificateInvalid> CertificateInvalidRepo;

        private GenericRepository<CdiDashboardDto> CDIDashboardRepo;

        private GenericRepository<CertificationExpiredDto> CertificationExpiredRepo;

        private GenericRepository<CertificationHistoryDto> certificateHistoryRepo;

        private GenericRepository<CDILicensesSummaryDto> licensesRepo;

        private GenericRepository<CustCodeInchargeDto> inchargeRepo;

        private GenericRepository<SetupTimelineDto> timelineRepo;

        private GenericRepository<ChannelDto> channelHistoryRepo;

        private GenericRepository<SummaryChannelHistoryDto> SummaryChannelHistoryRepo;

        private GenericRepository<CustomerShipToDto> shiptoRepo;

        private GenericRepository<ProductKSDBDto> productRepo;

        private CCCToolsEntity cccToolsEntity = null;

        public CDIDao()
        {
            cdiRepository = new GenericRepository<CDIDto>(new CCCToolsEntity());
            CertificateRepo = new GenericRepository<CertificationDto>(new CCCToolsEntity());
            CDIDashboardRepo = new GenericRepository<CdiDashboardDto>(new CCCToolsEntity());
            CertificateInvalidRepo = new GenericRepository<CertificateInvalid>(new CCCToolsEntity());
            CertificationExpiredRepo = new GenericRepository<CertificationExpiredDto>(new CCCToolsEntity());
            certificateHistoryRepo = new GenericRepository<CertificationHistoryDto>(new CCCToolsEntity());
            licensesRepo = new GenericRepository<CDILicensesSummaryDto>(new CCCToolsEntity());
            inchargeRepo = new GenericRepository<CustCodeInchargeDto>(new CCCToolsEntity());
            timelineRepo = new GenericRepository<SetupTimelineDto>(new CCCToolsEntity());
            channelHistoryRepo = new GenericRepository<ChannelDto>(new CCCToolsEntity());
            SummaryChannelHistoryRepo = new GenericRepository<SummaryChannelHistoryDto>(new CCCToolsEntity());
            productRepo = new GenericRepository<ProductKSDBDto>(new CCCToolsEntity());
            shiptoRepo = new GenericRepository<CustomerShipToDto>(new CCCToolsEntity());

            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Search CDI
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<CDIDto> GetSoldToList(CDIDto dto)
        {
            // 
            object[] paramAll = {
                dto.CustCode, dto.CustName, dto.CreateBy, Utils.StringDateFormat(dto.CreateDateFrom),
                Utils.StringDateFormat(dto.CreateDateTo), Utils.StringDateFormat(dto.UpdateDateFrom),
                Utils.StringDateFormat(dto.UpdateDateTo), dto.ExpiredYN,
                Utils.StringDateFormat(dto.DateExpiredFrom), Utils.StringDateFormat(dto.DateExpiredTo),
                dto.CGrpCode, dto.Certification, dto.UserId, dto.IsManual, dto.TDPHCCDCGN_Search,
                dto.TDNLDLT_Search, dto.TDDMCSD_Search, dto.SinhPham_Search, dto.Vaccin_Search, dto.BotoxXeomin_Search,
                dto.DDKXB, dto.DDKXBYN, dto.NotUse, dto.ShipTo
            };
            string paramAllNumber = @" {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13},
                                       {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}";
            // 
            object[] paramCertification = {
                dto.CustCode, dto.CustName, dto.CreateBy, Utils.StringDateFormat(dto.CreateDateFrom),
                Utils.StringDateFormat(dto.CreateDateTo), Utils.StringDateFormat(dto.UpdateDateFrom),
                Utils.StringDateFormat(dto.UpdateDateTo), dto.ExpiredYN,
                Utils.StringDateFormat(dto.DateExpiredFrom), Utils.StringDateFormat(dto.DateExpiredTo),
                dto.GCNDDKKDD, dto.GPP, dto.GDP, dto.GSP, dto.KCB, dto.GCNDDKTC,
                dto.CGrpCode, dto.Certification, dto.UserId, dto.IsManual, dto.TDPHCCDCGN_Search,
                dto.TDNLDLT_Search, dto.TDDMCSD_Search, dto.SinhPham_Search, dto.Vaccin_Search, dto.BotoxXeomin_Search,
                dto.DDKXB, dto.DDKXBYN, dto.NotUse, dto.ShipTo
            };
            string paramCertificationNumber = @" {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8},
                                   {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19},
                                   {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}";
            // Check select condition
            if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && !string.IsNullOrEmpty(dto.Certification) && "1".Equals(dto.ExpiredYN))
            {
                string spQuery = @"[SearchCDIExpired]" + paramCertificationNumber;
                return cdiRepository.ExecuteQuery(spQuery, paramCertification);
            }
            else if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && !string.IsNullOrEmpty(dto.Certification) && "2".Equals(dto.ExpiredYN))
            {
                string spQuery = @"[SearchCDIComingExpire]" + paramCertificationNumber;
                return cdiRepository.ExecuteQuery(spQuery, paramCertification);
            }
            else if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && string.IsNullOrEmpty(dto.Certification) && "1".Equals(dto.ExpiredYN))
            {
                string spQuery = @"[SearchCDIExpiredAll]" + paramAllNumber;
                return cdiRepository.ExecuteQuery(spQuery, paramAll);
            }
            else if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && string.IsNullOrEmpty(dto.Certification) && "2".Equals(dto.ExpiredYN))
            {
                string spQuery = @"[SearchCDIComingExpireAll]" + paramAllNumber;
                return cdiRepository.ExecuteQuery(spQuery, paramAll);
            }
            else if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && !string.IsNullOrEmpty(dto.Certification) && "0".Equals(dto.ExpiredYN))
            {
                string spQuery = @"[SearchCDINotYet]" + paramCertificationNumber;
                return cdiRepository.ExecuteQuery(spQuery, paramCertification);
            }
            else if (!string.IsNullOrEmpty(dto.DateExpiredFrom) && string.IsNullOrEmpty(dto.Certification) && "0".Equals(dto.ExpiredYN))
            {
                string spQuery = @"[SearchCDINotYetAll]" + paramAllNumber;
                return cdiRepository.ExecuteQuery(spQuery, paramAll);
            }
            else if (!string.IsNullOrEmpty(dto.ShipTo))
            {
                object[] param = { dto.ShipTo };
                string spQuery = @"EXEC sp_SearchSoldToByShipTo {0}";
                return cdiRepository.ExecuteQuery(spQuery, param);
            }
            else
            {
                object[] parameter = { dto.CustCode, dto.CustName, dto.CreateBy, Utils.StringDateFormat(dto.CreateDateFrom),
                                   Utils.StringDateFormat(dto.CreateDateTo), Utils.StringDateFormat(dto.UpdateDateFrom),
                                   Utils.StringDateFormat(dto.UpdateDateTo), dto.ExpiredYN,
                                   Utils.StringDateFormat(dto.DateExpiredFrom),Utils.StringDateFormat(dto.DateExpiredTo),
                                   dto.CGrpCode, dto.UserId, dto.IsManual, dto.TDPHCCDCGN_Search,
                                   dto.TDNLDLT_Search, dto.TDDMCSD_Search, dto.SinhPham_Search, dto.Vaccin_Search, dto.BotoxXeomin_Search,
                                   dto.DDKXB, dto.DDKXBYN, dto.NotUse, dto.ShipTo };
                string spQuery = @"[Get_All_CDI] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12},
                                                 {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}";
                return cdiRepository.ExecuteQuery(spQuery, parameter);
            }
        }

        
        /// <summary>
        /// get cdi detail
        /// </summary>
        /// <returns></returns>
        public CDIDto GetCDIDetail(object[] parameter)
        {
            string spQuery = "EXEC [Get_Detail_CDI] {0}";
            return cdiRepository.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Get ShipTo By CustCode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> GetShipToByCustCode(CDIDto dto)
        {
            object[] parameter = { dto.CustCode };
            string spQuery = "EXEC [sp_GetShipToByCustCode] {0}";
            return shiptoRepo.ExecuteQuery(spQuery, parameter);
        }

        /// <summary>
        /// Get Detail License ShipTo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public CustomerShipToDto GetDetailLicenseShipTo(CustomerShipToDto dto)
        {
            object[] parameter = { dto.CustCode, dto.ShipTo };
            string spQuery = "EXEC [sp_GetDetailLicenseShipTo] {0}, {1}";
            return shiptoRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// Insert Customer Ship To
        /// </summary>
        /// <param name="shipto"></param>
        /// <returns></returns>
        public int InsertHECShipTo(CustomerShipToDto shipto)
        {
            object[] param = { shipto.CustCode, shipto.ShipTo, shipto.ShipToName, shipto.SalOrg,
                                shipto.DC, shipto.DV, shipto.CreateBy, shipto.UpdateBy, shipto.Name1 };
            string spQuery = @"EXEC [sp_InsertHECShipTo] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// delete cdi
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UnusedCdi(object[] param)
        {
            int result = 0;
            string spQuery = "EXEC [Unused_CDI] {0}, {1}";
            result = cdiRepository.ExecuteCommand(spQuery, param);
            return result;
        }

        /// <summary>
        /// Update Cdi
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int UpdateCdi(object[] param)
        {
            try
            {
                string spQuery = @"EXEC [UpdateCdi] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, 
                                {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20},
                                {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, 
                                {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, 
                                {41}, {42}, {43}, {44}, {45}, {46}, {47}, {48}, {49}, {50}, 
                                {51}, {52}, {53}, {54}, {55}, {56}, {57}, {58}, {59}, {60},
                                {61}, {62}";
                return cdiRepository.ExecuteCommand(spQuery, param);               
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Add CDI
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int AddCdi(object[] param)
        {
            string spQuery = @"EXEC [AddCdi] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, 
                                {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20},
                                {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, 
                                {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, 
                                {41}, {42}, {43}, {44}, {45}, {46}, {47}, {48}, {49}, {50}, 
                                {51}, {52}, {53}, {54}, {55}, {56}, {57}, {58}, {59}, {60},
                                {61}, {62}, {63}";
            int rs = cdiRepository.ExecuteCommand(spQuery, param);
            return rs;
        }
        
        /// <summary>
        /// Update Tax code from CDIMng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateTaxCodeFromCDIMng(CDIDto dto)
        {
            object[] param = { dto.CustCode, dto.TaxCode };
            string spQuery = @"EXEC [UpdateTaxCodeFromCDIMng] {0}, {1}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update Tax Code To Customer
        /// </summary>
        /// <returns></returns>
        public int UpdateTaxCodeToCustomer()
        {
            object[] param = { };
            string spQuery = @"EXEC [UpdateTaxCodeToCustomer]";
            int rs = cdiRepository.ExecuteCommand(spQuery, param);
            return rs;
        }

        /// <summary>
        /// check customer is exist
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int checkCustomerExist(CDIDto dto)
        {
            var query = (from c in cccToolsEntity.CDIMngs
                         where c.CustCode.Equals(dto.CustCode)
                         select c).Count();
            int cnt = query;
            return cnt;
        }

        /// <summary>
        /// Search Certification
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<CertificationDto> SearchCertification(CertificateParam dto)
        {
            object[] param = { dto.CustCode, dto.Certification, dto.UserId, dto.Status, dto.IsDelete, dto.ExpiredYN };
            string spQuery = "EXEC [SearhCertification] {0}, {1}, {2}, {3}, {4}, {5}";
            return CertificateRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Update Certification
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public QueryResultDto UpdateCertification(object[] param)
        {
            QueryResultDto rs = new QueryResultDto();
            string spQuery = @"EXEC [UpdateCertification] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, 
                                    {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20},
                                    {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}";
            rs.Code = cdiRepository.ExecuteCommand(spQuery, param);
            return rs;
        }

        /// <summary>
        /// Insert Certification
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int InsertCertification(object[] param)
        {
            string spQuery = @"EXEC [InsertCertification] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, 
                                    {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20},
                                    {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// get cdi detail
        /// </summary>
        /// <returns></returns>
        public CertificationDto getCertificate(object[] parameter)
        {
            string spQuery = "[Get_Detail_CDI] {0}";
            return CertificateRepo.ExecuteQuerySingle(spQuery, parameter);
        }

        /// <summary>
        /// CustCodeRegisteredBy
        /// </summary>
        /// <returns></returns>
        public CdiDashboardDto GetCDIDashboard(CDIDashboardParam dto)
        {
            object[] param = { dto.UserId, dto.Dates, dto.Months, dto.FromDate, dto.ToDate, dto.Years, dto.Methods, dto.Client };
            string spQuery = "[CDIDashboard] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";
            return CDIDashboardRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Update condition enough to buy
        /// </summary>
        /// <returns></returns>
        public int IsCertification()
        {
            object[] param = { };
            string spQuery = "EXEC [IsCertification]";
            return CDIDashboardRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// get is certification
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<CertificationDto> GetIsCertification(CertificateParam jsonParam)
        {
            object[] param = { jsonParam.Certification };
            string spQuery = "EXEC [GetIsCertification] {0}";
            return CertificateRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// get certificate expired
        /// </summary>
        /// <returns></returns>
        public CertificateInvalid GetCertificateExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [GetCertificateExpired]";
            return CertificateInvalidRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Get Certificate Coming Expired Soon
        /// </summary>
        /// <returns></returns>
        public CertificateInvalid GetCertificateComingExpiredSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [GetCertificateComingExpiredSoon]";
            return CertificateInvalidRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Search Certificate Invalid
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<CertificationDto> SearchCertificateInvalid(CertificateParam jsonParam)
        {
            object[] param = { jsonParam.CustCode, jsonParam.CGrpCode, jsonParam.Invalid };
            string spQuery = "EXEC [SearchCertificateInvalid] {0}, {1}, {2}";
            return CertificateRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GCNDDKKDD Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> DDKKDDExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [DDKKDDExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GPP Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> GPPExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [GPPExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// GDP Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> GDPExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [GDPExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GSP Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> GSPExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [GSPExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// KCB Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> KCBExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [KCBExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// KSDB Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> KSDBExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [KSDBExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// COETTBYT Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> COETTBYTExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [COETTBYTExpired]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// GCNDDKKDD  Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> DDKKDDComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [DDKKDDComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GPP  Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> GPPComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [GPPComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// GDP  Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> GDPComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [GDPComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// GSP  Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> GSPComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [GSPComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// KCB Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> KCBComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [KCBComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// KSDB Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> KSDBComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [KSDBComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// COETTBYT Coming Expire Soon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationExpiredDto> COETTBYTComingExpireSoon()
        {
            object[] param = { };
            string spQuery = "EXEC [COETTBYTComingExpireSoon]";
            return CertificationExpiredRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Process customer and Required Licenses
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int ProcessCustomer(CDIDto dto)
        {
            object[] param = { };
            string spQuery = "EXEC [ExecutiveProcedureDaily]";
            return CDIDashboardRepo.ExecuteCommand(spQuery, param);
        }


        /// <summary>
        /// Certification History
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<CertificationHistoryDto> CertificationHistory(CDIDto parameters)
        {
            object[] param = { parameters.CustCode };
            string spQuery = "EXEC [sp_CertificationHistory] {0}";
            return certificateHistoryRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Insert certification history
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int InsertCertificationHistory(CDIDto dto)
        {
            object[] param = {
                dto.CDINumber, dto.CustCode, dto.TaxCode, dto.GPKD, dto.GPKDNumber, dto.GPKDName,
                Utils.StringDateFormat(dto.GCNDDKKDDFromDate), Utils.StringDateFormat(dto.GCNDDKKDDToDate),
                dto.GCNDDKKDDStatus, Utils.StringDateFormat(dto.GPPFromDate),
                Utils.StringDateFormat(dto.GPPToDate), dto.GPPStatus, Utils.StringDateFormat(dto.GDPFromDate),
                Utils.StringDateFormat(dto.GDPToDate), dto.GDPStatus, Utils.StringDateFormat(dto.GSPFromDate),
                Utils.StringDateFormat(dto.GSPToDate), dto.GSPStatus,
                Utils.StringDateFormat(dto.KCBFromDate), Utils.StringDateFormat(dto.KCBToDate),
                dto.KCBStatus, dto.Vaccin, dto.SinhPham, dto.Remark, dto.UserId, dto.IsDDKKDD,
                dto.IsGPP, dto.IsGDP, dto.IsGSP, dto.IsKCB, dto.IsCOETTBYT, dto.TDDMCSD, dto.NNKDMBTTBYT,
                dto.TDNLDLT, dto.TDPHCCDCGN , dto.DDKXB_TDPHCCDCGN, dto.DDKXB_TDNLDLT, dto.DDKXB_TDDMCSD,
                dto.DDKXB_SinhPham, dto.DDKXB_Vaccin, dto.IsManual, dto.DMKTCTBotoxBotulinum, dto.DDKM_BotoxXeomin,
                dto.CreateBy,Utils.StringDateFormat(dto.CreateDate),Utils.StringDateFormat(dto.UpdateDate),
                Utils.StringDateFormat(dto.GCNDDKTCFromDate), Utils.StringDateFormat(dto.GCNDDKTCToDate),
                dto.IsGCNDDKTC, dto.CMND, dto.Included, dto.Excluded_TDPHCCDCGN, dto.Excluded_TDNLDLT,
                dto.Excluded_TDDMCSD, dto.RemarkKSDB, dto.IncludedGPP, dto.ExcludedGPP_TDPHCCDCGN, dto.ExcludedGPP_TDNLDLT,
                dto.ExcludedGPP_TDDMCSD, dto.RemarkGPP_KSDB, dto.LicensesYESNO, dto.LicenseGPP, dto.LicenseGDP,
                dto.LicenseTaxCode, dto.LicenseGPKD, dto.LicenseCOETTBYT, dto.LicenseGCNDDKKDD,
                dto.LicenseKCB, dto.LicenseDMKTBotoxXeomin, dto.RX , dto.OTC ,
                dto.EssentialDrugs , dto.DDKXB_RX , dto.DDKXB_OTC, dto.DDKXB_EssentialDrugs
            };
            string spQuery = @"EXEC [sp_InsertCertificationHistory] 
                                {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, 
                                {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20},
                                {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, 
                                {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, 
                                {41}, {42}, {43}, {44}, {45}, {46}, {47}, {48}, {49}, {50}, 
                                {51}, {52}, {53}, {54}, {55}, {56}, {57}, {58}, {59}, {60},
                                {61}, {62}, {63}, {64}, {65}, {66}, {67}, {68}, {69}, {70},
                                {71}, {72}, {73}, {74}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        public int CheckCustCodeInDDKXB(CDIDto dto)
        {
            object[] param = { dto.CustCode, dto.TaxCode, dto.IsManual };
            string spQuery = @"EXEC [CheckCustCodeInDDKXB] {0}, {1}, {2}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Process By Cust Code
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int ProcessByCustCode(CDIDto dto)
        {
            object[] param = { dto.CustCode, dto.TaxCode, dto.IsManual };
            string spQuery = @"EXEC [ProcessByCustCode] {0}, {1}, {2}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Backup data for daily report
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int BackUpCDIMngToDDKXBDaily(CDIDto dto)
        {
            dto.FromDate = DateTime.Now.ToString("yyyy-MM-dd");
            object[] param = { dto.FromDate, dto.UserId };
            string spQuery = @"EXEC [BackUpCDIMngToDDKXBDaily] {0}, {1}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Auto BackUp Daily Report
        /// </summary>
        /// <returns></returns>
        public int AutoBackUpDailyReport()
        {
            object[] param = { };
            string spQuery = @"EXEC [AutoBackUpDailyReport]";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// DDKXB Daily
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<CDIDto> DDKXBDaily(CDIDto obj)
        {
            object[] param = {  obj.TDPHCCDCGN_Search, obj.TDNLDLT_Search, obj.TDDMCSD_Search,
                                obj.SinhPham_Search, obj.Vaccin_Search, obj.BotoxXeomin_Search,
                                obj.DDKXB, obj.DDKXBYN, Utils.StringDateFormat(obj.FromDate) };
            object[] paramAll = { obj.DDKXBYN, Utils.StringDateFormat(obj.FromDate) };
            string spQuery = string.Empty;
            if (string.IsNullOrEmpty(obj.DDKXBYN) && !string.IsNullOrEmpty(obj.DDKXB))
            {
                spQuery = "EXEC [sp_DDKXBDaily] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            }
            else if (!string.IsNullOrEmpty(obj.DDKXBYN) && string.IsNullOrEmpty(obj.DDKXB))
            {
                spQuery = "EXEC [sp_DDKXBDailyYN] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            }
            else if (!string.IsNullOrEmpty(obj.DDKXBYN) && !string.IsNullOrEmpty(obj.DDKXB))
            {
                spQuery = "EXEC [sp_DDKXBDailyDDKXBANDYN] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            }
            else
            {
                spQuery = "EXEC [sp_DDKXBDailyALL] {0}, {1}";
                return cdiRepository.ExecuteQuery(spQuery, paramAll);
            }
            return cdiRepository.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Select Customer NOT IN DDKXB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<CDIDto> SelectCustomerNOTINDDKXB(CDIDto obj)
        {
            object[] param = { obj.TDPHCCDCGN_Search, obj.TDNLDLT_Search, obj.TDDMCSD_Search,
                                obj.SinhPham_Search, obj.Vaccin_Search, obj.BotoxXeomin_Search,
                                obj.DDKXB, obj.DDKXBYN, Utils.StringDateFormat(obj.FromDate) };
            string spQuery = "EXEC [SelectCustomerNOTINDDKXB] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            return cdiRepository.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Required Licences
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<CDIDto> RequiredLicences(CDIDto obj)
        {
            object[] param = { obj.LicensesYESNO, obj.CGrpCode };
            string spQuery = "EXEC [sp_SelectRequiredLicenses] {0}, {1}";
            return cdiRepository.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Required Licences By Customer
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int RequiredLicencesByCustomer(CDIDto dto)
        {
            object[] param = { dto.CustCode };
            string spQuery = @"EXEC [RequiredLicencesByCustomer] {0}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }


        /*========================================= DASHBOARD ==========================================*/
        public IEnumerable<CDILicensesSummaryDto> DashboardCDI()
        {
            object[] param = { };
            string spQuery = "EXEC [DashboardCDI]";
            return licensesRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get General Certification Expired
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CDIDto> CDIDashboardReport(CdiDashboardDto dto)
        {
            string spQuery = string.Empty;
            if (!string.IsNullOrEmpty(dto.LicenseType))
            {
                if (dto.LicenseType.Equals("GPP_EXPIRED"))
                {
                    spQuery = "EXEC [GetGPPExpired]";
                }
                else if (dto.LicenseType.Equals("GDP_EXPIRED"))
                {
                    spQuery = "EXEC [GetGDPExpired]";
                }
                else if (dto.LicenseType.Equals("GCNDDKKDD_EXPIRED"))
                {
                    spQuery = "EXEC [GetGCNDDKKDDExpired]";
                }
                else if (dto.LicenseType.Equals("KCB_EXPIRED"))
                {
                    spQuery = "EXEC [GetKCBExpired]";
                }
                else if (dto.LicenseType.Equals("TOTAL_LICENSES_EXPIRED"))
                {
                    spQuery = "EXEC [GetTotalExpired]";
                }
                else if (dto.LicenseType.Equals("GPP_LACK"))
                {
                    spQuery = "EXEC [GetGPPLack]";
                }
                else if (dto.LicenseType.Equals("GDP_LACK"))
                {
                    spQuery = "EXEC [GetGDPLack]";
                }
                else if (dto.LicenseType.Equals("GCNDDKKDD_LACK"))
                {
                    spQuery = "EXEC [GetGCNDDKKDDLack]";
                }
                else if (dto.LicenseType.Equals("KCB_LACK"))
                {
                    spQuery = "EXEC [GetKCBLack]";
                }
                else if (dto.LicenseType.Equals("GPKD"))
                {
                    spQuery = "EXEC [GetGPKDLack]";
                }
                else if (dto.LicenseType.Equals("TAXCODE"))
                {
                    spQuery = "EXEC [GetTaxCodeLack]";
                }
                else if (dto.LicenseType.Equals("BOTOX_XEOMIN"))
                {
                    spQuery = "EXEC [GetBotoxXeominLack]";
                }
                else if (dto.LicenseType.Equals("TOTAL_LACK_OF_LICENSES"))
                {
                    spQuery = "EXEC [GetTotalLack]";
                }
                else if (dto.LicenseType.Equals("GPP_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetGPPComingExpired]";
                }
                else if (dto.LicenseType.Equals("GDP_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetGDPComingExpired]";
                }
                else if (dto.LicenseType.Equals("GSP_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetGSPComingExpired]";
                }
                else if (dto.LicenseType.Equals("GCNDDKKDD_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetGCNDDKKDDComingExpired]";
                }
                else if (dto.LicenseType.Equals("KCB_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetKCBComingExpired]";
                }
                else if (dto.LicenseType.Equals("GCNDDKTC_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetGCNDDKTCComingExpired]";
                }
                else if (dto.LicenseType.Equals("TOTAL_LICENSES_COMING_EXPIRED"))
                {
                    spQuery = "EXEC [GetTotalLicensesComingExpired]";
                }
                else if (dto.LicenseType.Equals("TAXCODE_VALID"))
                {
                    spQuery = "EXEC [GetTaxCodeValid]";
                }
                else if (dto.LicenseType.Equals("TAXCODE_BLANK"))
                {
                    spQuery = "EXEC [GetTaxCodeBlank]";
                }
                else if (dto.LicenseType.Equals("TAXCODE_NO_PROVIDE"))
                {
                    spQuery = "EXEC [GetTaxCodeNoProvide]";
                }
                else if (dto.LicenseType.Equals("TAXCODE_NO_CONTROL_LICENSES"))
                {
                    spQuery = "EXEC [GetTaxCodeNoControlLicense]";
                }
                else if (dto.LicenseType.Equals("ALL_CUSTOMERS_ACTIVE") || dto.LicenseType.Equals("TOTAL_TAXCODE"))
                {
                    spQuery = "EXEC [GetAllCustomerActive]";
                }
                else if (dto.LicenseType.Equals("ALL_CUSTOMERS_LICENSES_EXPIRED_LACK"))
                {
                    spQuery = "EXEC [GetAllLicenseLack]";
                }
                else if (dto.LicenseType.Equals("ALL_CUSTOMERS_TAXCODE_VALID"))
                {
                    spQuery = "EXEC [GetAllCustomersTaxCodeValid]";
                }
                else if (dto.LicenseType.Equals("PHARMACY_PHARMACYCOMPANY"))
                {
                    spQuery = "EXEC [CustomerByPharmacy_PharmacyComp]";
                }
                else if (dto.LicenseType.Equals("DIANOSTICS"))
                {
                    spQuery = "EXEC [CustomerDiagnostics]";
                }
                else if (dto.LicenseType.Equals("OTHERS"))
                {
                    spQuery = "EXEC [CustomerOthers]";
                }
                else if (dto.LicenseType.Equals("HOSPITAL_CLINIC"))
                {
                    spQuery = "EXEC [CustomerByHospitalDrClinic]";
                }
                else if (dto.LicenseType.Equals("ALL_CUSTOMERS_BLANK_NOPROVIDED"))
                {
                    object[] pr = { };
                    IEnumerable<CDIDto> TaxCodeBlank = cdiRepository.ExecuteQuery("EXEC [GetTaxCodeBlank]", pr);
                    IEnumerable<CDIDto> TaxCodeNoProvided = cdiRepository.ExecuteQuery("EXEC [GetTaxCodeNoProvide]", pr);
                    IEnumerable<CDIDto> TaxCodeBlankNoProvided = TaxCodeBlank.Union(TaxCodeNoProvided);
                    return TaxCodeBlankNoProvided;
                }
                else if (dto.LicenseType.Equals("NORMAL") || dto.LicenseType.Equals("PHOI_HOP") ||
                    dto.LicenseType.Equals("THUOC_DOC") || dto.LicenseType.Equals("THUOC_CAM") ||
                    dto.LicenseType.Equals("SINH_PHAM") || dto.LicenseType.Equals("VACCIN") ||
                    dto.LicenseType.Equals("BUY_BOTOX_XEOMIN") || dto.LicenseType.Equals("ALL_NORMAL_SPECIAL"))
                {
                    object[] parameter = { dto.LicenseType };
                    spQuery = "EXEC [sp_DuDieuKienXuatBan] {0}";
                    return cdiRepository.ExecuteQuery(spQuery, parameter);
                }
                else if (dto.LicenseType.Equals(Constant.ALARM_CERTIFICATION_EXPIRED_DAILY))
                {
                    object[] parameter = { };
                    spQuery = "EXEC [AutoAlarmCertificationExpiredDaily]";
                    return cdiRepository.ExecuteQuery(spQuery, parameter);
                }
                else if (dto.LicenseType.Equals(Constant.ALARM_CERTIFICATION_COMING_EXPIRE_DAILY))
                {
                    object[] parameter = { };
                    spQuery = "EXEC [AutoAlarmCertificationComingExpireSoon]";
                    return cdiRepository.ExecuteQuery(spQuery, parameter);
                }
                else if (dto.LicenseType.Equals("PHARMACY_FULL") || dto.LicenseType.Equals("DIAGNOSTICS_FULL") ||
                    dto.LicenseType.Equals("OTHERS_FULL") || dto.LicenseType.Equals("HOSPITAL_DRCLINIC_FULL") ||
                    dto.LicenseType.Equals("PHARMACY_LACKOFLICENSE") || dto.LicenseType.Equals("DIAGNOSTICS_LACKOFLICENSE") ||
                    dto.LicenseType.Equals("OTHERS_LACKOFLICENSE") || dto.LicenseType.Equals("HOSPITAL_DRCLINIC_LACKOFLICENSE") ||
                    dto.LicenseType.Equals("TOTAL_FULL_CHANNELS") || dto.LicenseType.Equals("TOTAL_LACKOFLICENSE_CHANNELS"))
                {
                    object[] parameter = { dto.LicenseType };
                    spQuery = "EXEC [sp_StatusByLicenses] {0}";
                    return cdiRepository.ExecuteQuery(spQuery, parameter);
                }
            }
            else if (dto.ReportType.Equals("CRT") || dto.ReportType.Equals("UDT"))
            {
                object[] pr = { dto.ReportType, dto.ByUser, Utils.StringDateFormat(dto.FromDate), Utils.StringDateFormat(dto.ToDate) };
                spQuery = "EXEC [ReportUpdateCreateByUser] {0}, {1}, {2}, {3}";
                return cdiRepository.ExecuteQuery(spQuery, pr);
            }
            else if (dto.ReportType.Equals("LICENSES_EXPIRED") || dto.ReportType.Equals("LICENSES_COMING_EXPIRE"))
            {
                object[] pr = { dto.ReportType, dto.ByUser, Utils.StringDateFormat(dto.FromDate), Utils.StringDateFormat(dto.ToDate) };
                spQuery = "EXEC [TotalLicenseExpiredAndComing] {0}";
                return cdiRepository.ExecuteQuery(spQuery, pr);
            }
            else if (dto.ReportType.Equals("FOUR_MONTHS")) {
                object[] pr = { };
                spQuery = "EXEC [sp_GCN_GPP_GDP_4MONTH]";
                return cdiRepository.ExecuteQuery(spQuery, pr);
            }
            object[] param = { };
            return cdiRepository.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Get Timeline IsRun
        /// </summary>
        /// <returns></returns>
        public SetupTimelineDto GetTimelineIsRun()
        {
            object[] param = { };
            string spQuery = "EXEC sp_GetTimelineIsRun";
            return timelineRepo.ExecuteQuerySingle(spQuery, param);
        }

        /// <summary>
        /// Total License Expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CDIDto> TotalLicenseExpired()
        {
            object[] param = { };
            string spQuery = "EXEC [TotalLicenseExpired]";
            return cdiRepository.ExecuteQuery(spQuery, param);
        }


        /// <summary>
        /// Get Email CustCode SE ASM
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustCodeInchargeDto> GetEmailCustCodeSE_ASM()
        {
            object[] param = { };
            string spQuery = "EXEC [GetEmailCustCodeSE_ASM]";
            return inchargeRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Setup Timeline
        /// </summary>
        /// <returns></returns>
        public int SetupTimeline(SetupTimelineDto dto)
        {
            object[] param = { dto.Id, dto.TaskName, Utils.StringDateFormat(dto.Timeline), dto.UpdateBy, dto.IsRun };
            string spQuery = @"EXEC [sp_SetupTimeline] {0}, {1}, {2}, {3}, {4}";
            return cdiRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Summary Channel History
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<SummaryChannelHistoryDto> SummaryChannelHistory(CDIDashboardParam dto)
        {
            object[] param = { dto.Years };
            string spQuery = @"EXEC [sp_SummaryChannelHistory] {0}";
            return SummaryChannelHistoryRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Update Ship To License
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateShipToLicense(CustomerShipToDto obj)
        {
            object[] param = { obj.CustCode, obj.ShipTo, obj.CGrpCode, Utils.StringDateFormat(obj.GCNDDKKDDFromDate), Utils.StringDateFormat(obj.GCNDDKKDDToDate),
                            Utils.StringDateFormat(obj.GPPFromDate), Utils.StringDateFormat(obj.GPPToDate), Utils.StringDateFormat(obj.GDPFromDate),
                            Utils.StringDateFormat(obj.GDPToDate), Utils.StringDateFormat(obj.GSPFromDate),
                            Utils.StringDateFormat(obj.GSPToDate), obj.UpdateBy, obj.TaxCode, obj.GPKD, obj.TDPHCCDCGN, obj.TDNLDLT, obj.TDDMCSD,
                            obj.SinhPham, obj.Vaccin, obj.NNKDMBTTBYT, obj.IsCOETTBYT, obj.IsManual, obj.IsGPP, obj.IsGDP, obj.IsGSP, obj.GPKDNumber,
                            obj.GPKDName, obj.IsDDKKDD, obj.IsKCB,
                            Utils.StringDateFormat(obj.KCBFromDate), Utils.StringDateFormat(obj.KCBToDate), obj.IsGCNDDKTC,
                            Utils.StringDateFormat(obj.GCNDDKTCFromDate), Utils.StringDateFormat(obj.GCNDDKTCToDate), obj.Remark };
            string spQuery = @"EXEC [sp_UpdateShipToLicense] {0}, {1}, {2}, {3}, {4}, {5}
                                    ,{6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}
                                    ,{16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}
                                    ,{25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update DDKXB ShipTo Licenses
        /// </summary>
        /// <param name="shipto"></param>
        /// <returns></returns>
        public int UpdateShipToLicenses(CustomerShipToDto shipto)
        {
            object[] param = {  shipto.CustCode, shipto.ShipTo, shipto.GPKD, shipto.GPKDNumber, shipto.GPKDName,
                    Utils.StringDateFormat(shipto.GCNDDKKDDFromDate), Utils.StringDateFormat(shipto.GCNDDKKDDToDate),
                    Utils.StringDateFormat(shipto.GPPFromDate),
                    Utils.StringDateFormat(shipto.GPPToDate), Utils.StringDateFormat(shipto.GDPFromDate),
                    Utils.StringDateFormat(shipto.GDPToDate), Utils.StringDateFormat(shipto.GSPFromDate),
                    Utils.StringDateFormat(shipto.GSPToDate), Utils.StringDateFormat(shipto.KCBFromDate),
                    Utils.StringDateFormat(shipto.KCBToDate),
                    shipto.Vaccin, shipto.SinhPham, shipto.Remark, shipto.IsDDKKDD,
                    shipto.IsGPP, shipto.IsGDP, shipto.IsGSP, shipto.IsKCB, shipto.IsCOETTBYT,
                    shipto.TDDMCSD, shipto.NNKDMBTTBYT, shipto.TDNLDLT, shipto.TDPHCCDCGN,
                    shipto.DDKXB_TDPHCCDCGN, shipto.DDKXB_TDNLDLT, shipto.DDKXB_TDDMCSD, shipto.DDKXB_SinhPham, shipto.DDKXB_Vaccin,
                    shipto.DMKTCTBotoxBotulinum, shipto.DDKM_BotoxXeomin, shipto.LicensesYESNO,
                    shipto.LicenseGPP, shipto.LicenseGDP, shipto.LicenseTaxCode,
                    shipto.LicenseGPKD, shipto.LicenseCOETTBYT, shipto.LicenseGCNDDKKDD, shipto.LicenseKCB, shipto.LicenseDMKTBotoxXeomin,
                    shipto.UpdateBy };
            string spQuery = @"EXEC [sp_UpdateShipToLicenses]   {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, 
                                                                {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20},
                                                                {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30},
                                                                {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40},
                                                                {41}, {42}, {43}, {44}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Process Customer By ShipTo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ProcessCustomerByShipTo(CustomerShipToDto obj)
        {
            object[] param = { obj.CustCode, obj.ShipTo };
            string spQuery = @"EXEC [ProcessCustomerByShipTo] {0}, {1}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Using this procedure when Code Sold To # Ship To then process by ship to
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ProcessCustomerByShiptoCGrpCode(CustomerShipToDto obj)
        {
            object[] param = { obj.CustCode, obj.ShipTo };
            string spQuery = @"EXEC [ProcessCustomerByShiptoCGrpCode] {0}, {1}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Process Shipto Licenses
        /// </summary>
        /// <returns></returns>
        public int ProcessShiptoLicenses()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_ProcessShiptoLicenses]";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Required Licences ShipTo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int RequiredLicencesShipTo(CustomerShipToDto obj)
        {
            object[] param = { obj.CustCode, obj.ShipTo };
            string spQuery = @"EXEC [RequiredLicencesShipTo] {0}, {1}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Update HECShipTo Into CustomerShipTo
        /// </summary>
        /// <returns></returns>
        public int UpdateHECShipToIntoCustomerShipTo()
        {
            object[] param = { };
            string spQuery = @"EXEC [sp_UpdateHECShipToIntoCustomerShipTo]";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Required Licenses ShipTo CGrpCode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int RequiredLicensesShipToCGrpCode(CustomerShipToDto obj)
        {
            object[] param = { obj.CustCode, obj.ShipTo };
            string spQuery = @"EXEC [sp_RequiredLicensesShipToCGrpCode] {0}, {1}";
            return shiptoRepo.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Search ShipTo
        /// </summary>
        /// <param name="shipto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> SearchShipTo(CustomerShipToDto shipto)
        {
            object[] param = { shipto.CustCode, shipto.ShipTo };
            string spQuery = @"EXEC sp_SearchShipTo {0}, {1}";
            return shiptoRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// ShipTo List
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<CustomerShipToDto> ShipToList(CustomerShipToDto dto)
        {
            object[] param = { dto.CustCode, dto.ShipTo, Utils.StringDateFormat(dto.UpdateDateFrom),
                                Utils.StringDateFormat(dto.UpdateDateTo), dto.UpdateBy, dto.IsManual };
            string spQuery = @"EXEC sp_ShipToList {0}, {1}, {2}, {3}, {4}, {5}";
            return shiptoRepo.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductKSDBDto> GetListProductKSDB()
        {
            object[] param = { };
            string spQuery = @"EXEC sp_GetListProductKSDB";
            return productRepo.ExecuteQuery(spQuery, param);
        }
    }
}