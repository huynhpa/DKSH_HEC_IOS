using DKSHApplication.Common;
using DKSHApplication.DAO;
using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DKSHApplication.Service
{
    public class FileStoreServiceImpl : FileStoreService
    {
        FileStoreDao fileDao = new FileStoreDao();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public FileStoreDto DownloadFile(FileStoreDto jsonParam)
        {
            FileStoreDto dto = fileDao.DownloadFile(jsonParam);
            return dto;
            throw new NotImplementedException();
        }

        /// <summary>
        /// get file by issue
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<FileStoreDto> GetFileByIssue(FileStoreDto jsonParam)
        {
            IEnumerable<FileStoreDto> fileList = fileDao.GetFileByIssue(jsonParam);
            return fileList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get File By Ticket
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public IEnumerable<FileStoreDto> GetFileByTicket(FileStoreDto jsonParam)
        {
            IEnumerable<FileStoreDto> fileList = fileDao.GetFileByTicket(jsonParam);
            return fileList;
            throw new NotImplementedException();
        }

        /// <summary>
        /// insert file
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public ResultMessageDto InsertFile(FileStoreDto jsonParam)
        {
            ResultMessageDto result = new ResultMessageDto();
            result.number = fileDao.InsertFile(jsonParam);
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search File
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<FileStoreDto> SearchFile(FileStoreParam param)
        {
            IEnumerable<FileStoreDto> lstFile = fileDao.SearchFile(param);
            return lstFile;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload files process
        /// </summary>
        /// <param name="fileDto"></param>
        /// <returns></returns>
        public ResultMessageDto UploadFiles(FileStoreDto fileDto)
        {
            ResultMessageDto result = new ResultMessageDto();
            return result;
            throw new NotImplementedException();
        }
    }
}