using DKSHApplication.Common;
using DKSHApplication.DB;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKSHApplication.DAO
{
    public class FileStoreDao
    {
        private GenericRepository<FileStoreDto> fileRepository;
        private CCCToolsEntity cccToolsEntity = null;


        public FileStoreDao()
        {
            this.fileRepository = new GenericRepository<FileStoreDto>(new CCCToolsEntity());
            cccToolsEntity = new CCCToolsEntity();
        }

        /// <summary>
        /// Get File By Ticket
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<FileStoreDto> GetFileByTicket(FileStoreDto dto)
        {
            object[] param = { dto.RequestId };
            string spQuery = @"EXEC [GetFileByTicket] {0}";
            IEnumerable<FileStoreDto> lst = fileRepository.ExecuteQuery(spQuery, param);
            return lst;        
        }

        /// <summary>
        /// Get File By Ticket
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<FileStoreDto> GetFileByIssue(FileStoreDto dto)
        {
            object[] param = { dto.IssueId };
            string spQuery = @"EXEC [GetFileByIssue] {0}";
            IEnumerable<FileStoreDto> lst = fileRepository.ExecuteQuery(spQuery, param);
            return lst;
        }

        /// <summary>
        /// Insert list file
        /// </summary>
        /// <param name="listFileDto"></param>
        /// <returns></returns>
        public int InsertFile(FileStoreDto dto)
        {
            object[] param = { dto.FileName, dto.FileLocation, dto.RequestId, dto.CreateBy, dto.FileType, dto.IssueId, dto.IssueNo };
            string spQuery = @"EXEC [InsertFile] {0}, {1}, {2}, {3}, {4}, {5}, {6}";
            return fileRepository.ExecuteCommand(spQuery, param);
        }

        /// <summary>
        /// Search file
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<FileStoreDto> SearchFile(FileStoreParam p)
        {
            object[] param = { p.FileName, p.RequestId, p.IssueNo, p.IssueId };
            string spQuery = @"EXEC [SearchFile] {0}, {1}, {2}, {3}";
            return fileRepository.ExecuteQuery(spQuery, param);
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public FileStoreDto DownloadFile(FileStoreDto obj)
        {
            object[] param = { obj.Id };
            string spQuery = @"EXEC [GetFileToDownload] {0}";
            return fileRepository.ExecuteQuerySingle(spQuery, param);
        }
    }
}