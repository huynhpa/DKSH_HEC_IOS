using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class OBSDashboardDao
    {
        private GenericRepository<OBSSummaryDto> dashboardRepo;
        private GenericRepository<SummaryByCampaignDto> summaryCampaignRepo;
        private GenericRepository<SummaryByAgentDto> summaryAgentRepo;
        private GenericRepository<AgentByLeaderDto> AgentRepo;
        private GenericRepository<SFEPerformanceDashboardDto> SFEPerformanceDashboardRepo;
        private GenericRepository<ComboChartDto> ComboChartRepo;
        private GenericRepository<GaugeChartDto> GaugeChartRepo;
        private GenericRepository<ContactsDto> contactRepo;
        private GenericRepository<TradeSalesDto> trade_salesRepo;
        private OBSEntity obsEntity;

        public OBSDashboardDao()
        {
            obsEntity = new OBSEntity();
            dashboardRepo = new GenericRepository<OBSSummaryDto>(new OBSEntity());
            summaryCampaignRepo = new GenericRepository<SummaryByCampaignDto>(new OBSEntity());
            summaryAgentRepo = new GenericRepository<SummaryByAgentDto>(new OBSEntity());
            AgentRepo = new GenericRepository<AgentByLeaderDto>(new OBSEntity());
            ComboChartRepo = new GenericRepository<ComboChartDto>(new OBSEntity());
            GaugeChartRepo = new GenericRepository<GaugeChartDto>(new OBSEntity());
            SFEPerformanceDashboardRepo = new GenericRepository<SFEPerformanceDashboardDto>(new OBSEntity());
            contactRepo = new GenericRepository<ContactsDto>(new OBSEntity());
            trade_salesRepo = new GenericRepository<TradeSalesDto>(new OBSEntity());
        }

        internal AgentByLeaderDto GetAgentInfo(OBSDashboardDto dto)
        {
            object[] param = { dto.AgentExtension };
            string spQuery = @"SELECT [AgentExtension]
                                  ,[AgentName]
                                  ,[LeaderName]
                                  ,[LeaderExtension]
                                  ,[SalemanCode]
                                  ,[LeaderEmail]
                                  ,[AgentEmail]
                                  ,[SFACode]
                                  ,[AgentContract]
                                  ,[Team]
                                  ,[Note]
                                  ,[EmpId]
                                  ,[EmpLevels]
                              FROM [OB].[dbo].[AgentByLeader]
                              where AgentExtension = {0}";
            return AgentRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ComboChartDto> ComboChart(OBSDashboardDto dto)
        {
            object[] param = { dto.Month, dto.AgentExtension };
            string spQuery = @"exec sp_ComboChart {0},{1}";
            return ComboChartRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<GaugeChartDto> GauChart(OBSDashboardDto dto)
        {
            object[] param = { dto.Month, dto.AgentExtension, dto.LeaderExtension };
            string spQuery = @"exec sp_GauChart {0},{1},{2}";
            return GaugeChartRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<ComboChartDto> ComboChartSum(OBSDashboardDto dto)
        {
            object[] param = { dto.Month, dto.LeaderExtension };
            string spQuery = @"exec sp_ComboChartSum {0},{1}";
            return ComboChartRepo.ExecuteQueryOBS(spQuery, param);
        }


        /// <summary>
        /// OBS Dashboard Summary
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        internal OBSSummaryDto OBSDashboardSummary(OBSDashboardDto pr)
        {
            object[] param = { pr.Agent };
            string spQuery = @"exec sp_OBSDashboardSummary {0}";
            return dashboardRepo.ExecuteQuerySingleOBS(spQuery, param);
        }

        /// <summary>
        /// Summary By Campaign
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SummaryByCampaignDto> SummaryByCampaign(OBSDashboardDto dto)
        {
            object[] param = { dto.Month };
            string spQuery = @"exec sp_SummaryByCampaign {0}";
            return summaryCampaignRepo.ExecuteQueryOBS(spQuery, param);
        }

        /// <summary>
        /// Summary By Agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<SummaryByAgentDto> SummaryByAgent(OBSDashboardDto dto)
        {
            object[] param = { dto.Month, dto.FromDate, dto.ToDate, dto.TypeReport, dto.ServiceType };
            string spQuery = @"exec sp_SummaryByAgent {0},{1},{2},{3},{4}";
            return summaryAgentRepo.ExecuteQueryOBS(spQuery, param);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<AgentByLeaderDto> SupsList()
        {
            object[] param = { };
            string query = @"SELECT [AgentExtension]
                                  ,[AgentName]
                                  ,[LeaderName]
                                  ,[LeaderExtension]
                                  ,[SalemanCode]
                                  ,[LeaderEmail]
                                  ,[AgentEmail]
                                  ,[SFACode]
                                  ,[AgentContract]
                                  ,[Team]
                                  ,[Note]
                                  ,[EmpId]
                                  ,[EmpLevels]
                              FROM [OB].[dbo].[AgentByLeader]
                              where EmpLevels IN ('LEADER','MANAGER')
                            order by AgentExtension,AgentName";
            return AgentRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<AgentByLeaderDto> SaleRepBySup(AgentByLeaderDto dto)
        {
            object[] param = { dto.LeaderExtension };
            string query = @"SELECT [AgentExtension]
                                  ,[AgentName]
                                  ,[LeaderName]
                                  ,[LeaderExtension]
                                  ,[SalemanCode]
                                  ,[LeaderEmail]
                                  ,[AgentEmail]
                                  ,[SFACode]
                                  ,[AgentContract]
                                  ,[Team]
                                  ,[Note]
                                  ,[EmpId]
                                  ,[EmpLevels]
                              FROM [OB].[dbo].[AgentByLeader]
                              where (LeaderExtension IN ({0}) OR AgentExtension = {0})
                            order by AgentExtension,AgentName";
            return AgentRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// RefreshPerformanceData
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int RefreshPerformanceData(OBSDashboardDto dto)
        {
            object[] param = { Utils.StringDateFormat(dto.interval_day), Utils.StringDateFormat(dto.interval_day_end), dto.AgentExtension };
            string query = @"
                        DECLARE @interval_day as date = {0}
                        DECLARE @interval_day_end as date = {1}
                        DECLARE @salerep_code as varchar(25)
                        DECLARE @agent_extension as varchar(25)
                        SET @agent_extension = {2}
                        DECLARE @sfa_code as varchar(25)
                        DECLARE @actual_call as decimal = 0
                        DECLARE @actual_sale as decimal = 0
                        DECLARE @actual_coverage as decimal = 0
                        DECLARE @active_customer as decimal = 0
                        DECLARE @coveration_rate as decimal = 0
			
                        DECLARE agent_cursor CURSOR FOR		
	                        SELECT [interval_day]
			                        ,[salerep_code]
			                        ,[agent_extension]
			                        ,[sfa_code]
	                        FROM [dbo].[SFAPerformanceDashboard]
	                        WHERE interval_day between @interval_day and @interval_day_end
			                and agent_extension = @agent_extension
                        OPEN agent_cursor
                        FETCH NEXT FROM agent_cursor
                        INTO @interval_day,@salerep_code,@agent_extension,@sfa_code
                        WHILE @@FETCH_STATUS = 0  
	                        begin
		                        -- @actual_call : Count successful call by agent
		                        SET @actual_call = (
								                        SELECT COUNT(S.ID)
								                        FROM SurveyVote	S
									                        LEFT JOIN AgentByLeader A ON A.EmpId = S.EmployeeID
								                        WHERE 
									                        A.AgentExtension = @agent_extension
									                        AND FORMAT(S.SurveyDate, 'yyyy-MM-dd') = @interval_day
									                        AND S.IsDeleted = 0
									                        AND S.SurveyStatus = '1'
							                        )
		                        --@actual_coverage : Distinct count number of customer having successful call
		                        SET @actual_coverage = (
									                        SELECT COUNT(DISTINCT S.CustomerPhone)
									                        FROM SurveyVote	S
										                        LEFT JOIN AgentByLeader A ON A.EmpId = S.EmployeeID
									                        WHERE 
										                        A.AgentExtension = @agent_extension
										                        AND FORMAT(S.SurveyDate, 'yyyy-MM-dd') = @interval_day
										                        AND S.IsDeleted = 0
										                        AND S.SurveyStatus = '1'
								                        )
					
		                        -- @active_customer: Count distinct customer having order by salesman code (source from sales data)
		                        SET @active_customer = (
									                        SELECT COUNT(DISTINCT CustomerCode)
									                        FROM CCCTools.dbo.T_Sales S
									                        WHERE S.SalesMan IN (@salerep_code)
										                        and (S.BU = 'HEC') AND FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd') = @interval_day
								                        )
		                        -- @actual_sale : Sum of net sales by salesman code 
		                        SET @actual_sale = (
								                        SELECT SUM(CAST(S.NetSales as decimal))
								                        FROM CCCTools.dbo.T_Sales S
								                        WHERE S.SalesMan IN (@salerep_code)
									                        and (S.BU = 'HEC') AND FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd') = @interval_day
							                        )

		                        -- @coveration_rate: Display Active customer by day & Coverage by day
		                        -- Compare:  AC/ Coverage = x%
		                        SET @coveration_rate = (CASE WHEN @actual_coverage > 0 THEN CAST(((@active_customer/@actual_coverage) * 100) as decimal) ELSE NULL END)

		                        UPDATE [SFAPerformanceDashboard]
		                        SET actual_call = @actual_call,
			                        --target_call = (case when @target_call is not null then @target_call else target_call end),
			                        actual_sale = @actual_sale, 
			                        --target_sale = (case when @target_sale is not null then @target_sale else target_sale end),
			                        actual_coverage = @actual_coverage, 
			                        --target_coverage = (case when @target_coverage is not null then @target_coverage else target_coverage end), 
			                        active_customer = @active_customer, 
			                        coveration_rate = @coveration_rate
		                        WHERE [interval_day] = @interval_day
			                        and [agent_extension] = @agent_extension
					
		                        -- next loop
		                        FETCH NEXT FROM agent_cursor
		                        INTO @interval_day,@salerep_code,@agent_extension,@sfa_code
	                        end
                        CLOSE agent_cursor;
                        DEALLOCATE agent_cursor;
                    ";
            return SFEPerformanceDashboardRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveDataPerformanceByAgent(OBSDashboardDto dto)
        {
            object[] param = { dto.AgentExtension, dto.Month };
            string query = @"
                delete from SFAPerformanceDashboard where [agent_extension] = {0} and format([interval_day],'MM-yyyy') = {1}
                DECLARE @interval as date
                --declare @cnt as int = 0
                --set @cnt = (SELECT COUNT(*) FROM SFAPerformanceDashboard WHERE [agent_extension] = {0} and format([interval_day],'MM-yyyy') = {1})
                declare @year int = FORMAT(GETDATE(),'yyyy'), @month int = FORMAT(GETDATE(),'MM');
                DECLARE interval_cursor CURSOR FOR		
                WITH numbers
                as
                (
	                select 1 as value
	                UNION ALL
	                select value + 1 from numbers
	                where value + 1 <= DAY(EOMONTH(datefromparts(@year,@month,1)))
                )
                SELECT datefromparts(@year,@month,numbers.value) Datum FROM numbers
                --IF @cnt = 0
		        --BEGIN
                OPEN interval_cursor
                FETCH NEXT FROM interval_cursor
                INTO @interval
                WHILE @@FETCH_STATUS = 0  
	                begin
		                INSERT INTO [dbo].[SFAPerformanceDashboard]
				                ([interval_day]
				                ,[salerep_code]
				                ,[salerep_name]
				                ,[agent_extension]
				                ,[sfa_code])
		                SELECT @interval,
			                SalemanCode,
			                AgentName,
			                AgentExtension,
			                SFACode
		                FROM AgentByLeader
			                WHERE AgentExtension = {0}
					
		                -- next loop
		                FETCH NEXT FROM interval_cursor
		                INTO @interval
	                end
                --END
                CLOSE interval_cursor;
                DEALLOCATE interval_cursor;";
            return SFEPerformanceDashboardRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveSFAPerformanceDashboard(SFEPerformanceDashboardDto dto)
        {
            object[] param = { dto.target_call,dto.target_sale, dto.target_coverage };
            string spQuery = @"exec sp_SaveSFAPerformanceDashboard {0},{1},{2}";
            return SFEPerformanceDashboardRepo.ExecuteCommandOBS(spQuery, param);
        }


        internal int InsertContact()
        {
            object[] param = { };
            string spQuery = @"INSERT INTO [dbo].[contacts]
                                        ([cust_code]
                                        ,[cust_name]
                                        ,[addrs]
                                        ,[district]
                                        ,[province]
                                        ,[region]
                                        ,[channel]
                                        ,[cgrp_code]
                                        ,[cgrp_name]
                                        ,[email_addr]
                                        ,[update_date]
                                        ,[update_by]
                                        ,[VNZ0Code]
                                        ,[Tel])
                                SELECT [CustCode]
                                    ,[CustName]
                                    ,[Addr]
                                    ,[District]
                                    ,[Province]
                                    ,[Region]
                                    ,[Channel]
                                    ,[CGrpCode]
                                    ,[CGrpName]
                                    ,[Email]
	                                ,format([LastUpdatedate],'yyyy-MM-dd')
	                                ,'sys'
	                                ,[VNZ0Code]
                                    ,[Tel]
                                FROM [CCCTools].[dbo].[Customer]
                                where [CustCode] not in (select [cust_code] from contacts)
                                and CHARINDEX('NOT USE', CustName) = 0

                            -- sync phone number from imind
                            INSERT INTO [dbo].[contact_phone]
                                       ([person_name]
                                       ,[phone_number]
                                       ,[cust_code]
                                       ,contact_source)
                            SELECT [person_name]
                                  ,[phone_number]
                                  ,[custcode]
                                  ,'iMind'
                              FROM [OB].[dbo].[Imind_Contract_Phones] P
                            where NOT EXISTS (select phone_number from contact_phone where phone_number = P.[phone_number])

                            INSERT INTO OB.[dbo].[CustomerCode]
                                       ([CustCode]
                                       ,[VNZ0Code]
                                       ,[BravoCode]
                                       ,[addr_detail]
                                       ,[district_id]
                                       ,[province_id])
                            SELECT C.CustCode, D.VNZ0Code, D.BravoCode, C.Addr, C.District, C.Province
                            FROM CCCTools.dbo.Customer C
	                            LEFT JOIN CCCTools.dbo.CDIMng D ON D.CustCode = C.CustCode
                            where C.CustCode NOT IN (SELECT CustCode FROM CustomerCode)                                                        
                        ";
            return contactRepo.ExecuteCommandOBS(spQuery, param);
        }

        /// <summary>
        /// SaveTradeSales
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int SaveTradeSales(TradeSalesDto obj)
        {
            string table_name = "[OB].[dbo].[Trade_Sales_" + DateTime.Now.ToString("yyyy") + "]";
            object[] param = { obj.InvoiceDate };
            string query = @"
                        DECLARE @myTableVariable TABLE (dates varchar(20))
                        declare @invoice_date as varchar(20)
                        declare @saturday as varchar(20)
                        declare @sunday as varchar(20)
                        set @invoice_date = format(DATEADD(day, -1, CAST(GETDATE() AS date)), 'yyyy-MM-dd')
                        print 'Add date into tablevariable '
                        insert into @myTableVariable values(@invoice_date)
                        declare @date_week as varchar(20)
                        set @date_week = DATENAME(DW, @invoice_date)
                        --print @date_week
                        if @date_week = 'Monday'
                        begin
	                        set @sunday = format(DATEADD(day, -1, @invoice_date), 'yyyy-MM-dd')
	                        set @saturday = format(DATEADD(day, -2, @invoice_date), 'yyyy-MM-dd')
	                        print 'Add date into tablevariable '
	                        insert into @myTableVariable values(@sunday)
	                        insert into @myTableVariable values(@saturday)
	                        --print @saturday
	                        --print DATENAME(DW, @saturday)
                        end

                        declare @existed as int = 0
                        declare @inv_day as varchar(20)
                        declare _daycursor cursor for
                        select * from @myTableVariable
                        OPEN _daycursor
                        FETCH NEXT FROM _daycursor
                        INTO @inv_day

                        while @@FETCH_STATUS = 0
                        BEGIN
	                        set @existed = (select count(*) from Trade_Sales_2022 where FORMAT(CAST(InvoiceDate AS date),'yyyy-MM-dd') = @inv_day)
	                        if @existed = 0
		                        begin
			                        INSERT INTO " + table_name + @"
				                        ([ClientCode]
				                        ,[ClientName]
				                        ,[WHS]
				                        ,[ProductID]
				                        ,[ProductName]
				                        ,[CustomerCode]
				                        ,[CustomerName]
				                        ,[Province]
				                        ,[Channel]
				                        ,[Region]
				                        ,[DistrictName]
				                        ,[InvoiceDate]
				                        ,[SalesMan]
				                        ,[Qty]
				                        ,[NetSales]
				                        ,[NetSalesVAT]
				                        ,[Reason]
				                        ,[BU]
				                        ,[Month]
				                        ,[Year]
				                        ,[SalesOrg]
				                        ,[CompanyCode]
				                        ,[OrderType]
				                        ,[DeviceCode]
				                        ,[DeviceName])
			                        SELECT [ClientCode]
				                        ,[ClientName]
				                        ,[WHS]
				                        ,[ProductID]
				                        ,[ProductName]
				                        ,[CustomerCode]
				                        ,[CustomerName]
				                        ,[Province]
				                        ,[Channel]
				                        ,[Region]
				                        ,[DistrictName]
				                        ,[InvoiceDate]
				                        ,[SalesMan]
				                        ,SUM(CAST([Qty] as decimal))
				                        ,SUM(CAST([NetSales] as decimal))
				                        ,SUM(CAST([NetSalesVAT] as decimal))
				                        ,[Reason]
				                        ,[BU]
				                        ,[Month]
				                        ,[Year]
				                        ,[SalesOrg]
				                        ,[CompanyCode]
				                        ,[OrderType]
				                        ,[DeviceCode]
				                        ,[DeviceName]
			                        FROM CCCTools.dbo.T_Sales S
			                        where S.Reason = 'Sales'
				                        and FORMAT(CAST(InvoiceDate AS date),'yyyy-MM-dd') = @inv_day
			                        group by [ClientCode]
				                        ,[ClientName]
				                        ,[WHS]
				                        ,[ProductID]
				                        ,[ProductName]
				                        ,[CustomerCode]
				                        ,[CustomerName]
				                        ,[Province]
				                        ,[Channel]
				                        ,[Region]
				                        ,[DistrictName]
				                        ,[InvoiceDate]
				                        ,[SalesMan]
				                        ,[Reason]
				                        ,[BU]
				                        ,[Month]
				                        ,[Year]
				                        ,[SalesOrg]
				                        ,[CompanyCode]
				                        ,[OrderType]
				                        ,[DeviceCode]
				                        ,[DeviceName]

			                        update " + table_name + @"
				                        set Reason = 'Return'
			                        where CAST([Qty] as decimal) < 0
				                        AND FORMAT(CAST(InvoiceDate AS date),'yyyy-MM-dd') = @inv_day

			                        update " + table_name + @"
				                        set S_ClientCode = ClientCode, S_ClientName = ClientName
			                        where (S_ClientCode is null or S_ClientCode = '')
				                        AND FORMAT(CAST(InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day
    
			                        update S
			                        set S.S_ClientCode = G.S_ClientCode, 
				                        S.S_ClientName = G.S_ClientName
			                        from " + table_name + @" S
				                        join [OB].[dbo].T_ProdMatching G 
					                        ON G.ProductID = S.ProductID
			                        WHERE FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day
                                
			                        update S
				                        set S.S_ClientCode = G.S_ClientCode, 
					                        S.S_ClientName = G.S_ClientName
			                        from " + table_name + @" S 
				                        join (select * from [OB].[dbo].T_ProdMatching where (ProductID is null or ProductID = '')) G
				                        ON G.ClientCode = S.ClientCode
			                        WHERE FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd') = @inv_day


			                        update S
				                        set S.SBL = O.SBL
			                        from " + table_name + @" S
				                        join [OB].[dbo].[T_OmniSBL] O ON O.S_ClientCode = S.S_ClientCode
			                        WHERE FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day

			                        update " + table_name + @"
			                        set INV_Date = CAST(InvoiceDate as decimal), IVN_Day = DAY(CAST(InvoiceDate as date))
			                        where FORMAT(CAST(InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day

			                        -- update exclusion by product id
			                        update S
				                        set S.Exclusion = E.Exclude
			                        from " + table_name + @" S
				                        join T_Exclusion E ON E.ProductID = S.ProductID
					                        and (E.ProductID is not null and E.ProductID <> '' ) and (E.CustomerCode is null or E.CustomerCode = '')
			                        where FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day			                        

                                    update S    
		                                set S.Exclusion = E.Exclude
                                    from " + table_name + @" S    
	                                left join [OB].[dbo].T_Exclusion E 
                                        ON concat(S.ProductID, S.CustomerCode) like E.IND 
                                    where FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day

                                    update S    set S.Exclusion = E.Exclude
                                        from " + table_name + @" S    
	                                left join [OB].[dbo].T_Exclusion E 
                                        ON CONCAT(S.S_ClientCode,S.CustomerCode) = E.IND 
                                    where FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day

			                        -- update ais product
			                        update S 
				                        set S.ATC1 = A.ATC1,
					                        S.ATC2 = A.ATC2,
					                        S.ATC3 = A.ATC3, 
					                        S.BSClass = A.BSClass, 
					                        S.BrandName = A.BrandName,
					                        S.Organic_YTD = A.Organic_YTD,
					                        S.Organic_MAT = A.Organic_MAT
			                        from " + table_name + @" S
			                        join T_AISProduct A on A.ProductCode = S.ProductID
			                        where FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day


			                        -- update AIS customer
			                        update S
				                        SET S.[ChannelMOM] = A.[ChannelMOM],
					                        S.[Omni_Chain_Brand] = A.[Omni_Chain_Brand],
					                        S.[Omni_Chain_Group] = A.[Omni_Chain_Group],
					                        S.OmniClass = A.OmniClass
			                        from " + table_name + @" S
				                        JOIN T_AISCustomer A ON S.CustomerCode = A.Kunnr
			                        where FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd')  = @inv_day

			                        -- update touchpoint
			                        update S 
				                        set S.Touchpoint = A.[Touchpoint], 
					                        S.[Resources] = A.[Resources]
			                        from " + table_name + @" S
				                        join T_SalesTouchpoint A on A.SalesMan = S.SalesMan
			                        where FORMAT(CAST(S.InvoiceDate AS date),'yyyy-MM-dd') = @inv_day

			                        update A set A.SFACode = b.SFACode, A.RepName = b.RepName, A.FLM = c.FLMName , A.SLM = c.SLMName
			                        from " + table_name + @" A left join T_SalesMapping b on A.CustomerCode = b.CustomerCode
				                        left join T_OmniFFMaster c on b.SFACode = c.SFACode
			                        where A.SFACode is null or A.SFACode = ''
			                        and FORMAT(CAST(A.InvoiceDate AS date),'yyyy-MM-dd') = @inv_day

			                        update " + table_name + @"
				                        set SalemanName = [Resources], Team = [Resources]
			                        where SalemanName is null or SalemanName like ''
				                        and FORMAT(CAST(InvoiceDate AS date),'yyyy-MM-dd') = @inv_day
	
	                        end
	                        else
	                        begin
		                        print 'Data already existed ' + @inv_day
	                        end
	                        -- end IF Statement
	                        -- fetch loop
	                        FETCH NEXT FROM _daycursor
	                        INTO @inv_day
                        END

                        CLOSE _daycursor;
                        DEALLOCATE _daycursor;
                            ";
            return trade_salesRepo.ExecuteCommandOBS(query, param);
        }
    }
}