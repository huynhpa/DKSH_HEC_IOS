'use strict';

var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
dkshApp.controller('SearchCallController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob, upload) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        CustomerCode: '', Campaign: '', FromDate: GetCurrentDate(), ToDate: GetCurrentDate(),
        Status: '', ServiceId: '', ReportType: '', MonthOfYear: GetCurrentMonth(), ServiceIdList: '',
        ProjectID: '', contracts: ''
    },
    $scope.ListSearchCall = [];
    $scope.ProjectList = [];
    $scope.EffectiveCampaignHistoryList = [];
    $scope.CampaignList = [];
    $scope.CustomerClassFile = '';
    $scope.SurveyDto = {
        Id: '',ProvinceID: '',CenterID: '',MSBCH: '',EmployeeID: '',SurveyDate: '',CustomerName: '',CustomerAddress: ''
        ,CustomerPhone: '',CustomerAge: '',ServiceTypeID: '',NumberOfContact: '',SuperviseID: '',StartTime: ''
        ,EndTime: '',SurveyTime: '',Cost: '',IsCallBack: '',CallBackDate: '',SurveyStatus: '',CustomField1: '',CustomField2: ''
        ,CustomField3: '',CustomField4: '',CustomField5: '',CustomField6: '',CustomField7: '',CustomField8: '',CustomField9: ''
        ,CustomField10: '',CreatedBy: '',CreatedDate: '',UpdatedBy: '',UpdatedDate: '',IsDeleted: ''
    };
    $scope.UserSession = {
        UserId: '', UserName: '', IsAdmin: '', contracts: '', Extension: ''
    },
    /*========= PAGING =========*/
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['10', '20', '50']
    };
    $scope.callback = function () { },
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['10', '20', '50']
        };
    },
    /*========= PAGING =========*/

    // SEARCH
    $scope.Search = function () {        
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        $scope.SearchParam.ServiceIdList = $scope.SearchParam.ServiceId.toString();
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/Search',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $scope.ListSearchCall = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });        
    },

    $scope.SearchEffectiveCampaignHistory = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        $scope.SearchParam.ServiceIdList = $scope.SearchParam.ServiceId.toString();
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/SearchEffectiveCampaignHistory',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $scope.EffectiveCampaignHistoryList = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
    },


    /*=============== Report Call ===============*/
    $scope.SurveyResult = function () {
        if (IsNullOrEmpty($scope.SearchParam.ServiceId)) {
            ErrorMessage('Please choose at least one campaign');
            return false;
        } else {
            if (!IsNullOrEmpty($scope.SearchParam.ServiceId)) {
                $scope.SearchParam.ServiceIdList = $scope.SearchParam.ServiceId.toString();
            }
            if (IsNullOrEmpty($scope.SearchParam.ServiceId)) {
                $scope.SearchParam.Campaign = 'SurveyResult_' + $scope.SearchParam.FromDate;
            } else {
                $scope.SearchParam.Campaign = $("#ddlServiceTypeID1 option:selected").text();
            }
            $scope.SearchParam.FromDate += ' 01:00 AM';
            $scope.SearchParam.ToDate += ' 11:59 PM';
            angular.element('#Loading').LoadingOverlay("show");
            $http({
                method: 'GET',
                url: '/SearchCall/SurveyResult',
                headers: [{ 'Content-Type': 'application/json' }],
                params: $scope.SearchParam,
                responseType: 'blob'
            }).then(function (response) {
                $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
                $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
                $('#Loading').LoadingOverlay("hide");
                var data = new Blob([response.data], { type: response.data.type });
                FileSaver.saveAs(data, $scope.SearchParam.Campaign + '_' + $scope.SearchParam.FromDate + '.xlsx');
            }, function (response) {
                $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
                $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
                angular.element('#Loading').LoadingOverlay("hide");
                ErrorMessage(response.data);
                console.log(response.data);
                return deferred.reject(response.data);
            });
        }
    },

    $scope.SearchProject = function () {
        $scope.ProjectDto = {
            ID: ''
            , ProjectName: ''
            , IsActive: true
            , IsDeleted: false
        };
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Project/SearchProject',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ProjectDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ProjectList = response.data;           
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    /*========== RESET ===========*/
    $scope.Reset = function () {
        $scope.SearchParam = {
            CustomerCode: '', Campaign: '', FromDate: GetCurrentDate(), ToDate: GetCurrentDate(),
            Status: '', ServiceId: '', ReportType: '', MonthOfYear: GetCurrentMonth(), ServiceIdList: '',
            ProjectID: ''
        };
    },

    /*========== Get Campaign List ==========*/
    $scope.GetCampaignList = function () {
        $http({
            method: 'GET',
            url: '/SearchCall/GetCampaignList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.CampaignList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },
    $scope.OBSurveyParam = {};
    /* delete survey */
    $scope.DeleteSurvey = function (item) {
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Are you sure to delete this survey?',
            buttons: {
                submit: {
                    btnClass: 'btn-success',
                    action: function () {
                        $scope.SurveyDto = item;                        
                        $scope.OBSurveyParam = angular.copy($scope.SurveyDto);
                        console.log("SURVEY DATA: ", $scope.OBSurveyParam);
                        $http({
                            method: 'POST',
                            url: '/DoCampaign/DeleteSurvey',
                            headers: [{ 'Content-Type': 'application/json' }],
                            data: $scope.OBSurveyParam
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code === 'SESSION_EXPIRED_MESSAGE') {
                                $.alert({
                                    title: 'Message!',
                                    type: 'red',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-red',
                                            action: function () {
                                                $window.location.href = '/Login';
                                            }
                                        }
                                    }
                                });
                            } else if (response.data.code === 'SUCCESS') {
                                $.alert({
                                    title: 'Message!',
                                    type: 'green',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-red',
                                            action: function () {
                                                $scope.Search();
                                            }
                                        }
                                    }
                                });
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage("Server is not responding");
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });
        
    },

    /*ViewAgentInCampaign*/
    $scope.ViewAgentInCampaign = function () {
        if (IsNullOrEmpty($scope.SearchParam.ServiceId)) {
            $.alert({
                title: 'Message!',
                type: 'red',
                content: 'Please select a campaign',
                buttons: {
                    ok: {
                        btnClass: 'btn-red',
                        action: function () {
                            $('#ddlServiceTypeID2').focus();
                        }
                    }
                }
            });
        } else {            
            $scope.SearchParam.FromDate += ' 01:00 AM';
            $scope.SearchParam.ToDate += ' 11:59 PM';
            angular.element('#Loading').LoadingOverlay("show");
            $http({
                method: 'GET',
                url: '/Campaign/ViewAgentInCampaign',
                headers: [{ 'Content-Type': 'application/json' }],
                params: $scope.SearchParam,
                responseType: 'blob'
            }).then(function (response) {
                $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
                $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
                $('#Loading').LoadingOverlay("hide");
                var data = new Blob([response.data], { type: response.data.type });
                FileSaver.saveAs(data, 'AgentInCampaign.xlsx');
            }, function (response) {
                $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
                $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
                angular.element('#Loading').LoadingOverlay("hide");
                ErrorMessage('Unexpected error while getting data!!');
                console.log(response.data);
                return deferred.reject(response.data);
            });
        }
    },

    $scope.ReportCall = function () {
        var fileNm = 'CallHistory' + $scope.SearchParam.FromDate + ' to ' + $scope.SearchParam.ToDate + '.xlsx';
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';        
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/ReportCall',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.EffectiveCampaignHistory = function () {
        if (IsNullOrEmpty($scope.SearchParam.ServiceId)) {
            ErrorMessage('Please select at least one campaign');
            return false;
        } else {
            $scope.SearchParam.FromDate += ' 01:00 AM';
            $scope.SearchParam.ToDate += ' 11:59 PM';
            $scope.SearchParam.ServiceIdList = $scope.SearchParam.ServiceId.toString();
            angular.element('#Loading').LoadingOverlay("show");
            $http({
                method: 'GET',
                url: '/SearchCall/EffectiveCampaignHistory',
                headers: [{ 'Content-Type': 'application/json' }],
                params: $scope.SearchParam,
                responseType: 'blob'
            }).then(function (response) {
                $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
                $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
                $('#Loading').LoadingOverlay("hide");
                var data = new Blob([response.data], { type: response.data.type });
                FileSaver.saveAs(data, 'EffectiveCampaign.xlsx');
            }, function (response) {
                $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
                $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
                angular.element('#Loading').LoadingOverlay("hide");
                ErrorMessage(response.data);
                return deferred.reject(response.data);
            });
        }
    },

    $scope.CallCampaignHistory = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        $scope.SearchParam.ServiceIdList = $scope.SearchParam.ServiceId.toString();
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/CallCampaignHistory',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'CampaignHistory ' + $scope.SearchParam.FromDate + ' to ' + $scope.SearchParam.ToDate + '.xlsx');
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.DuplicateCall = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/DuplicateCall',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'DuplicateCall.xlsx');
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.ActiveOutletDaily = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/ActiveOutletDaily',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'ActiveOutletDaily.xlsx');
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.SFEReport = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SearchCall/SFEReport',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'SFEReport.xlsx');
        }, function (response) {
            $scope.SearchParam.FromDate = $scope.SearchParam.FromDate.substr(0, 10);
            $scope.SearchParam.ToDate = $scope.SearchParam.ToDate.substr(0, 10);
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    /*==============================*/
    $scope.ExportResult = function () {
        if (IsNullOrEmpty($scope.SearchParam.ReportType)) {
            ErrorMessage('Please choose report type');
            return false;
        } else {
            if ($scope.SearchParam.ReportType === 'SURVEYRESULT') {
                $scope.SurveyResult();
            } else if ($scope.SearchParam.ReportType === 'CALLHISTORY') {
                $scope.ReportCall();
            } else if ($scope.SearchParam.ReportType === 'AGENTINCAMPAIGN') {
                $scope.ViewAgentInCampaign();
            } else if ($scope.SearchParam.ReportType === 'DUPLICATECALL') {
                $scope.DuplicateCall();
            } else if ($scope.SearchParam.ReportType === 'ACTIVE_OUTLET_DAILY') {
                $scope.ActiveOutletDaily();
            } else if ($scope.SearchParam.ReportType === 'EFFECTIVE_CAMPAIGN_HISTORY') {
                $scope.EffectiveCampaignHistory();
            } else if ($scope.SearchParam.ReportType === 'CALL_CAMPAIGN_HISTORY') {
                $scope.CallCampaignHistory();
            } else if ($scope.SearchParam.ReportType === 'SFE_REPORT') {
                $scope.SFEReport();
            }
        }
    },

    $scope.UploadCustomer = function () {
        if (IsNullOrEmpty($('#txtCustomerClassFile').val())) {
            ErrorMessage('There is no file chosen to upload');
            return false;
        } else {
            $('#Loading').LoadingOverlay("show");
            upload({
                url: '/SearchCall/UploadCustomer',
                method: 'POST',
                data: {
                    aFile: $scope.CustomerClassFile
                },
                params: $scope.SearchParam
            }).then(function (response) {
                $('#Loading').LoadingOverlay("hide");
                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                    SuccessMessage(response.data.message);
                } else {
                    ErrorMessage(response.data.message);
                }
            }, function (response) {
                $('#Loading').LoadingOverlay("hide");
                ErrorMessage(response.data);
                console.error(response.data);
            });
        }
        
    },

    $scope.ProcessActiveOutlet = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'POST',
            url: '/SearchCall/ProcessActiveOutletDaily',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                SuccessMessage(response.data.message);
            } else if (response.data.code === 'ERROR') {
                ErrorMessage(response.data.message);
            }
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
    },

    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {   
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.UserSession.contracts = $('#contracts').val();
        $scope.GetCampaignList();
        $scope.SearchProject();
        $scope.Search();        
    },
    $scope.initPage();

    /*========== INIT PAGE ===========*/

}).filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
}).directive('uploadFile', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var file_uploaded = $parse(attrs.uploadFile);
            element.bind('change', function () {
                scope.$apply(function () {
                    file_uploaded.assign(scope, element[0].files[0]);
                });
            });
        }
    };
}]);