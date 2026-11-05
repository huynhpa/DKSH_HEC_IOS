using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DKSHApplication.DTO;
using DKSHApplication.Common;
using DKSHApplication.DAO;

namespace DKSHApplication.Service
{
    public class ServiceTypeServiceImpl : ServiceTypeService
    {
        ServiceTypeDao serviceTypeDao = new ServiceTypeDao();
        HrEmployeeDao hrEmployeeDao = new HrEmployeeDao();

        /// <summary>
        /// GetActiveServiceType
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ServiceTypeDto> GetActiveServiceType(ServiceTypeDto dto)
        {
            hrEmployeeDto employeeDto = new hrEmployeeDto();
            employeeDto.WindowsUser = dto.WindowUser;
            employeeDto = hrEmployeeDao.GetEmployeeByUserName(employeeDto);
            dto.ProjectID = employeeDto.GroupID;
            IEnumerable<ServiceTypeDto> result = serviceTypeDao.GetActiveServiceType(dto);
            return result;
        }

        /// <summary>
        /// get service type by id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ServiceTypeDto GetServiceTypeById(ServiceTypeDto dto)
        {
            ServiceTypeDto result = serviceTypeDao.GetServiceTypeByID(dto);
            return result;
        }

    }
}