using DKSHApplication.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.Common;

namespace DKSHApplication.DAO
{
    public class RecordsDao
    {
        private GenericRepository<Records46Dto> recordRepo;
        private GenericRepository<TeleSaleOrgChartDto> TeleSaleOrgChartRepo;
        private OBSEntity obsEntity = null;

        public RecordsDao()
        {
            obsEntity = new OBSEntity();
            recordRepo = new GenericRepository<Records46Dto>(new OBSEntity());
            TeleSaleOrgChartRepo = new GenericRepository<TeleSaleOrgChartDto>(new OBSEntity());
        }

        /// <summary>
        /// search record_ggm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<Records46Dto> SearchRecord(Records46Dto dto)
        {
            object[] param = { dto.cnum, Utils.StringDateFormat(dto.calldate), Utils.StringDateFormat(dto.calldate_to)
                    , dto.disposition, dto.destination, dto.RecordID, dto.server_record };
            string query = "EXEC sp_SearchRecord {0},{1},{2},{3},{4},{5},{6}";
            return recordRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// SearchTeleOrgchart
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<TeleSaleOrgChartDto> SearchTeleOrgchart(TeleSaleOrgChartDto dto)
        {
            object[] param = { dto.AgentExtension, dto.LeaderExtension };
            string query = "EXEC sp_SearchTeleOrgchart {0},{1}";
            return TeleSaleOrgChartRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// GetTeleOrgChartDetail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public TeleSaleOrgChartDto GetTeleOrgChartDetail(TeleSaleOrgChartDto dto)
        {
            object[] param = { dto.AgentExtension, dto.LeaderExtension };
            string query = "EXEC sp_GetTeleOrgChartDetail {0},{1}";
            return TeleSaleOrgChartRepo.ExecuteQuerySingleOBS(query, param);
        }

        /// <summary>
        /// save record_ggm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int SaveRecordFromServer46(Records46Dto dto)
        {
            object[] param = { dto.calldate, dto.destination, dto.duration, dto.disposition,
                                dto.cnum, dto.recordingfile, dto.uniqueid, dto.contracts };
            string query = "EXEC sp_SaveRecordFromServer46 {0},{1},{2},{3},{4},{5},{6},{7}";
            return recordRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// Record Export
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<Records46Dto> RecordExport(Records46Dto dto)
        {
            object[] param = { dto.cnum, Utils.StringDateFormat(dto.calldate), Utils.StringDateFormat(dto.calldate_to) };
            string query = "EXEC sp_RecordsReport {0},{1},{2}";
            return recordRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// Delete Record 46
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteRecord46(Records46Dto dto)
        {
            object[] param = { Utils.StringDateFormat(dto.calldate), Utils.StringDateFormat(dto.calldate_to), dto.cnum };
            string query = "EXEC sp_DeleteRecord46 {0},{1},{2}";
            return recordRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// SaveTeleSaleOrgChart
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveTeleSaleOrgChart(TeleSaleOrgChartDto dto)
        {
            object[] param = { dto.AgentExtension
                                ,dto.AgentName
                                ,dto.LeaderName
                                ,dto.LeaderExtension
                                ,dto.SalemanCode
                                ,dto.LeaderEmail
                                ,dto.AgentEmail
                                ,dto.SFACode
                                ,dto.AgentContract
                                ,dto.Team
                                ,dto.IsActive
                                ,dto.Note 
                                ,dto.IsUpdate
                                ,dto.EmpLevels
                                ,dto.GroupID
                                ,dto.WindowsUser
                            };
            string query = "EXEC sp_SaveTeleSaleOrgChart {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}";
            return TeleSaleOrgChartRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// DeleteTeleSaleOrgChart
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int DeleteTeleSaleOrgChart(TeleSaleOrgChartDto dto)
        {
            object[] param = { dto.AgentExtension, dto.LeaderExtension };
            string query = "EXEC sp_DeleteTeleSaleOrgChart {0},{1}";
            return recordRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// TalkTime
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<Records46Dto> TalkTime(Records46Dto dto)
        {
            object[] param = { dto.calldate, dto.calldate_to };
            string query = @"SELECT format(T.calldate,'yyyy-MM-dd') as calldate
                                ,T.duration
                                ,T.cnum
	                            ,A.AgentName 
	                            ,G.GroupName
	                            ,A.AgentContract
	                            ,a.LeaderName
                                ,T.[login]
                                ,T.logoff
                                ,CAST(T.total_call as VARCHAR) as total_call
                                ,T.zalo_call
                                ,CAST(T.actual_target as VARCHAR) as actual_target
                                ,CAST(T.active_customer as VARCHAR) as active_customer
                            FROM OB.dbo.TalkTimeDaily T
                                left outer join AgentByLeader A on A.AgentExtension = T.cnum
                                left outer join AgentGroup G on G.GroupCode = A.Team
                            where FORMAT(T.calldate, 'yyyy-MM-dd') between {0} and {1}
                            order by A.Team, T.cnum, T.calldate desc
                        ";
            return recordRepo.ExecuteQueryOBS(query, param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal int SaveTalkTimeDaily(Records46Dto dto)
        {
            object[] param = { dto.calldate };
            string query = @"
                            DELETE FROM TalkTimeDaily where format(calldate,'yyyy-MM-dd') = {0}
                            INSERT INTO [dbo].[TalkTimeDaily]
                                        ([calldate]
                                        ,[duration]
                                        ,[cnum])
                            SELECT FORMAT(CAST(R.calldate AS DATE), 'yyyy-MM-dd') AS calldate,
                                CONVERT(varchar, DATEADD(ms, SUM(CAST(R.duration AS decimal)) * 1000, 0), 114) AS duration,
                                R.cnum
                            FROM dbo.Records46 AS R
                            WHERE (CHARINDEX('s', LOWER(R.destination)) = 0)
                            AND (CHARINDEX('i', LOWER(R.destination)) = 0)
                            AND (R.disposition = 'ANSWERED')
                            AND R.duration > 0
                            AND (FORMAT(CAST(R.calldate AS DATE), 'yyyy-MM-dd') = {0})
                            AND (R.cnum IN
                                (SELECT AgentExtension
                                FROM dbo.AgentByLeader))

                            GROUP BY FORMAT(CAST(R.calldate AS DATE), 'yyyy-MM-dd'),
                                R.cnum

	                        declare @cnum as varchar(10)
	                        declare @login_time as varchar(20)
	                        declare @logoff_time as varchar(20)
	                        declare @total_call as decimal = 0
	                        declare @actual_target as decimal = 0
	                        declare @active_customer as decimal = 0

	                        declare _calltime cursor for
	                        select cnum
	                        from TalkTimeDaily
	                        where FORMAT(CAST(calldate AS DATE), 'yyyy-MM-dd') = {0}

	                        OPEN _calltime
	                        FETCH NEXT FROM _calltime
	                        INTO @cnum

	                        while @@FETCH_STATUS = 0
	                        BEGIN
		                        set @login_time = (SELECT TOP 1 FORMAT(CAST(R.calldate AS DATETIME), 'HH:mm:ss tt') AS calldate
							                        FROM dbo.Records46 AS R
						                        WHERE (CHARINDEX('s', LOWER(R.destination)) = 0)
							                        AND (CHARINDEX('i', LOWER(R.destination)) = 0)
							                        AND (R.disposition = 'ANSWERED')
							                        AND R.duration > 0
							                        AND FORMAT(CAST(R.calldate AS DATE), 'yyyy-MM-dd') = {0}
							                        AND R.cnum = @cnum
						                        ORDER BY FORMAT(CAST(R.calldate AS DATETIME), 'HH:mm:ss tt') asc)

		                        set @logoff_time = (SELECT TOP 1 FORMAT(CAST(R.calldate AS DATETIME), 'HH:mm:ss tt') AS calldate
							                        FROM dbo.Records46 AS R
						                        WHERE (CHARINDEX('s', LOWER(R.destination)) = 0)
							                        AND (CHARINDEX('i', LOWER(R.destination)) = 0)
							                        AND (R.disposition = 'ANSWERED')
							                        AND R.duration > 0
							                        AND FORMAT(CAST(R.calldate AS DATE), 'yyyy-MM-dd') = {0}
							                        AND R.cnum = @cnum
						                        ORDER BY FORMAT(CAST(R.calldate AS DATETIME), 'HH:mm:ss tt') desc)
		
		                        set @total_call = (select count(S.ID) from SurveyVote S 
			                        LEFT JOIN HrEmployee E ON S.EmployeeID = E.ID 
			                        where E.AgentNumber = @cnum and FORMAT(S.SurveyDate,'yyyy-MM-dd') = {0}
				                        and S.SurveyStatus = '1')

		                        set @actual_target = (case when @total_call > 0 then (@total_call / 40) * 100 else 0 end)

		                        SET @active_customer = (
									                        SELECT COUNT(DISTINCT S.CustomerPhone)
									                        FROM SurveyVote	S
										                        LEFT JOIN AgentByLeader A ON A.EmpId = S.EmployeeID
									                        WHERE 
										                        A.AgentExtension = @cnum
										                        AND FORMAT(S.SurveyDate, 'yyyy-MM-dd') = {0}
										                        AND S.IsDeleted = 0
										                        AND S.SurveyStatus = '1'
                                                                AND S.CustomerPhone <> '' and S.CustomerPhone is not null
								                        )


		                        update TalkTimeDaily
		                        set [login] = @login_time, logoff = @logoff_time, total_call = @total_call, actual_target = @actual_target, active_customer = @active_customer
			                        where FORMAT(CAST(calldate AS DATE), 'yyyy-MM-dd') = {0}
			                        and cnum = @cnum

		                        FETCH NEXT FROM _calltime
		                        INTO @cnum
	                        END
	
                        CLOSE _calltime;
                        DEALLOCATE _calltime;
                ";
            return recordRepo.ExecuteCommandOBS(query, param);
        }

        /// <summary>
        /// CheckExtension
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        internal IEnumerable<TeleSaleOrgChartDto> CheckExtension(TeleSaleOrgChartDto dto)
        {
            object[] param = { dto.AgentExtension, dto.AgentContract };
            string query = @"SELECT A.[AgentExtension]
		                            ,A.[AgentName]
		                            ,A.[LeaderName]
		                            ,A.[LeaderExtension]
		                            ,A.[SalemanCode]
		                            ,A.[LeaderEmail]
		                            ,A.[AgentEmail]
		                            ,A.[SFACode]
		                            ,A.[AgentContract]
		                            ,A.Team
		                            ,A.[IsActive]
		                            ,A.EmpLevels
		                            ,A.[EmpId]
		                            ,A.[Note]
		                            ,E.WindowsUser
		                            ,E.GroupID
		                            ,FORMAT(A.UpdateDate, 'dd-MM-yyyy HH:mm:ss') as UpdateDate
	                            FROM [OB].[dbo].[AgentByLeader] A
		                            LEFT JOIN AgentGroup G ON G.GroupCode = A.Team
		                            LEFT JOIN hrEmployee E ON E.ID = A.EmpId
                              where A.AgentExtension = {0} AND AgentContract = {1}";
            return TeleSaleOrgChartRepo.ExecuteQueryOBS(query, param);
        }
    }
}