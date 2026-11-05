'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
.controller('CallPlanController', function ($scope, $http, $location, $window, $log, $rootScope, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.ListCallPlan = [];
    $scope.AllCallPlan = '';
    $scope.CallPlanFile = '';
    $scope.ProjectList = [];
    $scope.CampaignByCustomerFile = '';
    $scope.DailyInTimeSummaryFile = '';
    $scope.ActiveCampaignList = [];
    $scope.CampaignFocus = [];
    $scope.CallPlanFocusCampaign = [];
    $scope.ListLimitedCustomer = [];
    $scope.LeaderList = [];
    $scope.LimitedCustomerFile = '';
    $scope.LimitedCustomerDto = {
        salesman_code: ''
        , FullName: ''
        , extension: ''
        , limit_from: ''
        , limit_to: ''
        , cust_code: ''
        , import_date: ''
        , leader_saleman: ''
        , leader_ext: ''
    };
    $scope.CallPlanDto = {
        route_date: GetCurrentDate(), route_date_end: GetCurrentDate(), salesman_code: '',
        CallDate: '', CallDateTo: '', ServiceCode: '', ServiceTypeId: '', summary_month: GetCurrentMonth(),
        ProjectId: '', CustCode: '', FocusMonth: GetCurrentMonth(), action_type: '', remove_type: ''
    };
    $scope.CallPlanByCampaign = {
        ID: ''
        , ServiceTypeId: ''
        , ServiceCode: ''
        , ServiceName: ''
        , salesman_code: ''
        , GroupName: ''
        , CustCode: ''
        , CreateDate: ''
        , CreateBy: ''
        , CallDate: ''
        , CallDateEnd: ''
        , ProjectName: ''
    },

    $scope.SearchCallPlan = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/CallPlan/SearchCallPlan',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListCallPlan = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },  
    /* check | uncheck all */
    $scope.CheckAll = function () {
        if ($('#chkAll').prop('checked') === false) {
            for (var i = 0; i < $scope.ListCallPlan.length; i++) {
                $("[id^='callplanid_]").prop('checked', false);
            }
        } else {
            for (var j = 0; j < $scope.ListCallPlan.length; j++) {
                $("[id^='callplanid_']").prop('checked', true);
            }
        }
    },

    $scope.AllActiveCampaign = function () {
        $scope.CampaignFocus.length = 0;
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/CallPlan/AllActiveCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ActiveCampaignList = response.data;
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.GetProject = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Campaign/GetCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.ProjectList = response.data.ProjectList;
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.SearchCallPlanByCampaign = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/CallPlan/SearchCallPlanByCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.CampaignFocus = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.SearchDailyInTimeSummary = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/CallPlan/SearchDailyInTimeSummary',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.DailyInTimeSummary = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.ProcessCallPlan = function () {
        $scope.CallPlanByCampaign.CallDate = $scope.CallPlanDto.CallDate;
        $scope.CallPlanByCampaign.CallDateEnd = $scope.CallPlanDto.CallDateTo;
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'This action shall import call plan into assignment and auto pick to call daily. Begin <strong>' + $scope.CallPlanByCampaign.CallDate + '</strong>, end <strong>' + $scope.CallPlanByCampaign.CallDateEnd +'</strong> ?',
            buttons: {
                submit: {
                    btnClass: 'bg-green-active',
                    action: function () {                        
                        angular.element('#Loading').LoadingOverlay("show");
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/CallPlan/ProcessCallPlan',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.CallPlanByCampaign
                        }).then(function (response) {
                            deferred.resolve(response);
                            angular.element('#Loading').LoadingOverlay("hide");
                            if (response.data.code === 'SUCCESS') {
                                SuccessMessage(response.data.message);
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
                            return deferred.reject(response.data);
                        });
                        return deferred.promise;
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });        
    },


    $scope.GetCallPlan = function () {
        $scope.CallPlanByCampaign.CallDate = $scope.CallPlanDto.CallDate;
        $scope.CallPlanByCampaign.CallDateEnd = $scope.CallPlanDto.CallDateTo;
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Get Call Plan of <strong>' + GetCurrentMonth() + '</strong>?',
            buttons: {
                submit: {
                    btnClass: 'bg-green-active',
                    action: function () {
                        angular.element('#Loading').LoadingOverlay("show");
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/CallPlan/GetCallPlan',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.CallPlanDto
                        }).then(function (response) {
                            deferred.resolve(response);
                            angular.element('#Loading').LoadingOverlay("hide");
                            if (response.data.code === 'SUCCESS') {
                                SuccessMessage(response.data.message);
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
                            return deferred.reject(response.data);
                        });
                        return deferred.promise;
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });
    },


    $scope.CallPlanTemplate = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/CallPlan/CallPlanTemplate',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto,
            responseType: 'blob'
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'CallPlanTemplate.xlsx');
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting template file!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },


    $scope.CampaignByCustomerTemplate = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/CallPlan/CampaignByCustomerTemplate',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto,
            responseType: 'blob'
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'CampaignByCustomerTemplate.xlsx');
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting template file!!');
            return deferred.reject(response.data);
        });
    },

    $scope.ExportCampaignFocus = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = 'CampaignFocus_' + $scope.CallPlanDto.FocusMonth + '.xlsx';
        $http({
            method: 'GET',
            url: '/CallPlan/ExportCampaignFocus',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto,
            responseType: 'blob'
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting template file!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.ImportCampaignByCustomer = function () {
        if (IsNullOrEmpty($('#txtCampaignByCustomer').val())) {
            ErrorMessage('There is no file to upload');
            return false;
        } if (IsNullOrEmpty($scope.CallPlanDto.action_type)) {
            ErrorMessage('Please select action type');
            return false;
        } if (!IsNullOrEmpty($scope.CallPlanDto.action_type) && $scope.CallPlanDto.action_type === '1'
                    && IsNullOrEmpty($scope.CallPlanDto.remove_type)) {
            ErrorMessage('Please select remove & replace type');
            return false;
        } if ($scope.CallPlanDto.remove_type === '0' && IsNullOrEmpty($scope.CallPlanDto.ServiceTypeId)) {
            ErrorMessage('Please select campaign');
            return false;
        } else {
            var confr = '';
            if (!IsNullOrEmpty($scope.CallPlanDto.remove_type)) {
                confr = 'Data uploaded <strong>BEFORE</strong> in <strong class="text-red">' + $scope.CallPlanDto.FocusMonth + ' of campaign ' + $("#ddlCampaign option:selected").text() + '</strong> shall be delete and could not revert. Do you want to save?';
            } else {
                confr = 'Upload will be take a few minutes. Do you want to save?';
            }
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: confr,
                buttons: {
                    submit: {
                        btnClass: 'btn-success',
                        action: function () {
                            $('#Loading').LoadingOverlay("show");
                            upload({
                                url: '/CallPlan/ImportCampaignByCustomer',
                                method: 'POST',
                                data: {
                                    aFile: $scope.CampaignByCustomerFile
                                },
                                params: $scope.CallPlanDto
                            }).then(function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                                    $.alert({
                                        title: 'Message',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-green',
                                                action: function () {
                                                    $scope.SearchCallPlanByCampaign();
                                                }
                                            }
                                        }
                                    });
                                } else {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                ErrorMessage("The wait operation timed out");
                                console.error(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });            
        }
    },

    $scope.GetLeaderList = function () {
        $http({
            method: 'GET',
            url: '/MonitorForm/GetLeaderList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.LeaderList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    };

    $scope.SearchLimitedCustomer = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/CallPlan/SearchLimitedCustomer',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListLimitedCustomer = response.data;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.ImportLimitedCustomer = function () {
        if (IsNullOrEmpty($('#txtLimitedCustomerFile').val())) {
            ErrorMessage('There is no file to upload');
            return false;
        } if (IsNullOrEmpty($scope.CallPlanDto.limit_from)) {
            ErrorMessage('Please select limit from');
            return false;
        } if (IsNullOrEmpty($scope.CallPlanDto.limit_to)) {
            ErrorMessage('Please select limit to');
            return false;
        } else {
            var confr = 'Upload will be take a few minutes. Do you want to save?';            
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: confr,
                buttons: {
                    submit: {
                        btnClass: 'btn-success',
                        action: function () {
                            $('#Loading').LoadingOverlay("show");
                            upload({
                                url: '/CallPlan/ImportLimitedCustomer',
                                method: 'POST',
                                data: {
                                    aFile: $scope.LimitedCustomerFile
                                },
                                params: $scope.LimitedCustomerDto
                            }).then(function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                                    $.alert({
                                        title: 'Message',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-green',
                                                action: function () {
                                                    $scope.SearchDailyInTimeSummary();
                                                }
                                            }
                                        }
                                    });
                                } else {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                ErrorMessage(response.data);
                                console.error(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    },

    $scope.RouteToPhoneNumber = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'POST',
            url: '/CallPlan/RouteToPhoneNumber',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallPlanDto
        }).then(function (response) {
            deferred.resolve(response);
            angular.element('#Loading').LoadingOverlay("hide");
            if (response.data.code === 'SUCCESS') {
                SuccessMessage(response.data.message);
            } else if (response.data.code === 'ERROR') {
                ErrorMessage(response.data.message);
            }
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.ImportDailyInTimeSummary = function () {
        if (IsNullOrEmpty($('#txtDailyInTimeSummary').val())) {
            ErrorMessage('There is no file to upload');
            return false;
        } if (IsNullOrEmpty($scope.CallPlanDto.summary_month)) {
            ErrorMessage('Please select month');
            return false;
        } else {
            var confr = 'Upload will be take a few minutes. Do you want to save?';
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: confr,
                buttons: {
                    submit: {
                        btnClass: 'btn-success',
                        action: function () {
                            $('#Loading').LoadingOverlay("show");
                            upload({
                                url: '/CallPlan/ImportDailyInTimeSummary',
                                method: 'POST',
                                data: {
                                    aFile: $scope.DailyInTimeSummaryFile
                                },
                                params: $scope.CallPlanDto
                            }).then(function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                                    $.alert({
                                        title: 'Message',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-green',
                                                action: function () {
                                                    $scope.SearchDailyInTimeSummary();
                                                }
                                            }
                                        }
                                    });
                                } else {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                ErrorMessage(response.data);
                                console.error(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    },
    /* Upload CallPlan */
    $scope.UploadCallPlan = function () {
        if (IsNullOrEmpty($('#txtCallPlan').val())) {
            ErrorMessage('No file chosen to upload');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-success',
                        action: function () {
                            $('#Loading').LoadingOverlay("show");
                            upload({
                                url: '/CallPlan/UploadCallPlan',
                                method: 'POST',
                                data: {
                                    aFile: $scope.CallPlanFile
                                }
                            }).then(function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                                    $.alert({
                                        title: 'Message',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-green',
                                                action: function () {                                                
                                                    $scope.SearchCallPlan();
                                                }
                                            }
                                        }
                                    });
                                } else {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                ErrorMessage("The wait operation timed out");
                                console.error(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    },

    $scope.Clear = function () {
        $scope.CallPlanDto = {
            route_date: GetCurrentDate(), route_date_end: GetCurrentDate(), salesman_code: '',
            CallDate: '', CallDateTo: '', ServiceCode: '', ServiceTypeId: '',
            ProjectId: '', cust_code: '', FocusMonth: GetCurrentMonth()
        };
    },

    /*=========== PAGINATION=========== */
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    },
    $scope.callback = function () { },
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    },
    /*=========== PAGINATION=========== */
    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        $scope.GetProject();
        //$scope.SearchCallPlan();
    },
    $scope.initPage();
    /* ===== INIT PAGE ====== */
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