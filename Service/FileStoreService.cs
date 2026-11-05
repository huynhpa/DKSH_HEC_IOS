using DKSHApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSHApplication.Service
{
    public interface FileStoreService
    {
        /// <summary>
        /// GetFileByTicket
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        IEnumerable<FileStoreDto> GetFileByTicket(FileStoreDto jsonParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        IEnumerable<FileStoreDto> GetFileByIssue(FileStoreDto jsonParam);

        /// <summary>
        /// InsertFile
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        ResultMessageDto InsertFile(FileStoreDto jsonParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileDto"></param>
        /// <returns></returns>
        ResultMessageDto UploadFiles(FileStoreDto fileDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<FileStoreDto> SearchFile(FileStoreParam param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        FileStoreDto DownloadFile(FileStoreDto jsonParam);
    }
}
