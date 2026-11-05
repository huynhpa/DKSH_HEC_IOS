'use strict';

var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'ngSanitize', 'lr.upload'])
dkshApp.controller('RecordsController', function ($scope, $http, $location, $window, $log, $q, upload, FileSaver, Blob, $sce, $sanitize) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        calldate: GetLastDayFromNow(1)
        , calldate_to: GetLastDayFromNow(1)
        , destination: ''
        , RecordID: ''
        , duration: ''
        , disposition: ''
        , recordingfile: ''
        , cnum: ''
        , server_record: ''
        , LeaderExtension: ''
        , contracts: ''
        , MonitorDateFrom: GetCurrentDate()
        , MonitorDateTo: GetCurrentDate()
        , AgentExtension: ''
        , report_type: ''
    };
    $scope.MonitorHistory = {
        MonitorID: ''
        , MonitorBy: ''
        , MonitorDateTime: ''
        , CreateBy: ''
        , CreateDate: ''
        , TotalListening: ''
        , TotalMark: 0
        , EvalID: ''
        , RecordID: ''
        , CriterialID: ''
        , calldate: ''
        , uniqueid: ''
        , destination: ''
        , duration: ''
        , disposition: ''
        , recordingfile: ''
        , cnum: ''
        , contracts: ''
        , server_record: ''
        , LeaderEmail: ''
        , FullName: ''
        , recordlink: ''
        , calldateYear: ''
        , calldateMon: ''
        , calldateDay: ''
        , EvaluationResult: []
        , IsUpdate: false
        , Campaign: ''
        , RecordLink: ''
    };
    $scope.TeleOrgchartDto = {
        AgentExtension: ''
        , AgentName: ''
        , LeaderName: ''
        , LeaderExtension: ''
        , SalemanCode: ''
        , LeaderEmail: ''
        , AgentEmail: ''
        , SFACode: ''
        , AgentContract: ''
        , Team: ''
        , IsActive: ''
        , Note: ''
        , IsUpdate: false
        , GroupID: ''
        , EmpLevels: ''
        , WindowsUser: ''
    }
    $scope.ListAgentByLeader = [];
    $scope.ListMonitorHistory = [];
    $scope.EvaluationCall = [];
    $scope.RecordFile = '';
    $scope.ListRecords = [];
    $scope.AgentList = [];
    $scope.EvaluationMark = [];
    $scope.ListTeleOrgChart = [];
    $scope.CriterialEvaluationCall = {
        WELCOME: [],
        CUSTOMER_RELATIONSHIP: [],
        CONTENT: [],
        FEELING: [],
        FINISH: []
    };
    $scope.EvaluationDataParam = {
        EvalMark: []
    };
    /*========= PAGING =========*/
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    };
    $scope.callback = function () { }
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    };
    /*========= PAGING =========*/

    /*========================== Search record =======================*/
    $scope.Search = function () {
        if (IsNullOrEmpty($scope.SearchParam.server_record)) {
            ErrorMessage('Please select server');
            return false;
        } else {
            angular.element('#Loading').LoadingOverlay("show");
            $http({
                method: 'GET',
                url: '/Records/SearchRecord',
                headers: [{ 'Content-Type': 'application/json' }],
                params: $scope.SearchParam
            }).then(function (response) {
                angular.element('#Loading').LoadingOverlay("hide");
                deferred.resolve(response);
                $scope.ListRecords = response.data;
                $scope.totalItems = response.data.length;
                $scope.SetPaging();
            }, function (response) {
                angular.element('#Loading').LoadingOverlay("hide");
                ErrorMessage(response.data);
                return deferred.reject(response.data);
            });
        }
    },

    $scope.GetProjectList = function () {
        $http({
            method: 'GET',
            url: '/SearchCall/GetProjectList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ProjectList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /*========================== Search monitor history =======================*/
    $scope.SearchMonitorHistory = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/MonitorForm/SearchMonitorHistory',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.ListMonitorHistory = response.data;
            $scope.SetPaging();
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.SearchTeleOrgchart = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Records/SearchTeleOrgchart',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.ListTeleOrgChart = response.data;
            $scope.SetPaging();
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.GetTeleOrgChartDetail = function (item) {
        $scope.TeleOrgchartDto = item;
        $scope.TeleOrgchartDto.IsUpdate = true;
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Records/GetTeleOrgChartDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.TeleOrgchartDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.TeleOrgchartDto = response.data;
            
            $('#mdTeleSalesOrgChart').modal({
                backdrop: 'static',
                keyboard: false
            });
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.NewTeleOrgChart = function () {
        $scope.TeleOrgchartDto = {
            AgentExtension: ''
            , AgentName: ''
            , LeaderName: ''
            , LeaderExtension: ''
            , SalemanCode: ''
            , LeaderEmail: ''
            , AgentEmail: ''
            , SFACode: ''
            , AgentContract: ''
            , Team: ''
            , IsActive: true
            , Note: ''
            , IsUpdate: false
        };
        $('#mdTeleSalesOrgChart').modal({
            backdrop: 'static',
            keyboard: false
        });
    },

    $scope.SaveTeleSalesOrgChart = function () {
        if (IsNullOrEmpty($scope.TeleOrgchartDto.AgentExtension)) {
            ErrorMessage('Please input AgentExtension');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.AgentName)) {
            ErrorMessage('Please input AgentName');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.AgentEmail)) {
            ErrorMessage('Please input AgentEmail');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.EmpLevels)) {
            ErrorMessage('Please input Levels');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.AgentContract)) {
            ErrorMessage('Please input AgentContract');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.LeaderExtension)) {
            ErrorMessage('Please input LeaderExtension');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.LeaderName)) {
            ErrorMessage('Please input LeaderName');
            return false;
        } if (IsNullOrEmpty($scope.TeleOrgchartDto.LeaderEmail)) {
            ErrorMessage('Please input LeaderEmail');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'info',
                content: 'Do you want to save?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            $http({
                                method: 'POST',
                                url: '/Records/SaveTeleSaleOrgChart',
                                headers: [{ 'Content-Type': 'application/json' }],
                                params: $scope.TeleOrgchartDto
                            }).then(function (response) {
                                deferred.resolve(response);
                                if (response.data.code === 'SUCCESS') {
                                    $.alert({
                                        title: 'Message!',
                                        type: 'info',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'bg-green-active',
                                                action: function () {
                                                    $('#mdTeleSalesOrgChart').modal('hide');
                                                    $scope.TeleOrgchartDto.IsUpdate = false;
                                                    $scope.SearchTeleOrgchart();
                                                }
                                            }
                                        }
                                    });
                                } else if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                } else if (response.data.code === 'SESSION_EXPIRED_MESSAGE') {
                                    $.alert({
                                        title: 'Message!',
                                        type: 'info',
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
                                }
                            }, function (response) {
                                ErrorMessage(response.data);
                                return deferred.reject(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
        
    },

    $scope.DeleteTeleSaleOrgChart = function (item) {
        $.confirm({
            title: 'Confirm!',
            type: 'red',
            content: 'Do you want to delete?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $scope.TeleOrgchartDto = item;
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/Records/DeleteTeleSaleOrgChart',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.TeleOrgchartDto
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code === 'SUCCESS') {
                                $.alert({
                                    title: 'Message!',
                                    type: 'info',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'bg-green-active',
                                            action: function () {
                                                $scope.SearchTeleOrgchart();
                                            }
                                        }
                                    }
                                });
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage(response.data);
                            return deferred.reject(response.data);
                        });
                        return deferred.promise;
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });
    };

    $scope.GetAgentByLeader = function () {
        $http({
            method: 'GET',
            url: '/MonitorForm/GetAgentByLeader',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListAgentByLeader = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    };


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

    /*======================== Save record =======================*/
    $scope.SaveRecord = function () {
        if (IsNullOrEmpty($('#txtRecordFile').val())) {
            ErrorMessage('There is no file chosen to upload');
            return false;
        } else {
            $('#Loading').LoadingOverlay("show");
            upload({
                url: '/Records/SaveRecord',
                method: 'POST',
                data: {
                    aFile: $scope.RecordFile
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
                                    $('#mdCustomer').modal('hide');
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
    };

    /*================== Fetch data from Issabel 10.210.148.36 ==================*/
    $scope.FetchRecordIssabel = function () {
        if (IsNullOrEmpty($scope.SearchParam.server_record)) {
            ErrorMessage('Please select server');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'red',
                content: 'This action shall delete all records from <strong>' + $scope.SearchParam.calldate + '</strong> to <strong>' + $scope.SearchParam.calldate_to + '</strong> to reload data from server 36. Do you want to continue ? ',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            angular.element('#Loading').LoadingOverlay("show");
                            deferred.notify();
                            $http({
                                method: 'POST',
                                url: '/Records/FetchRecordIssabel',
                                headers: [{ 'Content-Type': 'application/json' }],
                                params: $scope.SearchParam
                            }).then(function (response) {
                                deferred.resolve(response);
                                angular.element('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS') {
                                    SuccessMessage(response.data.message);
                                } else if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                ErrorMessage(response.data);
                                angular.element('#Loading').LoadingOverlay("hide");
                                return deferred.reject(response.data);
                            });
                            return deferred.promise;
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    },

    $scope.Report = function () {
        if ($scope.SearchParam.report_type === 'RECORD') {
            $scope.RecordExport();
        } else if ($scope.SearchParam.report_type === 'TALK_TIME') {
            $scope.TalkTime();
        }
    };            
    

    $scope.RecordExport = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = "RecordExport-" + $scope.SearchParam.calldate + " to " + $scope.SearchParam.calldate_to + ".xlsx";
        $http({
            method: 'GET',
            url: '/Records/RecordExport',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            console.error(response.data);
            return deferred.reject(response.data);
        });
    };

    $scope.TalkTime = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = "TalkTime-" + $scope.SearchParam.calldate + " to " + $scope.SearchParam.calldate_to + ".xlsx";
        $http({
            method: 'GET',
            url: '/Records/TalkTime',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            console.error(response.data);
            return deferred.reject(response.data);
        });
    };

    $scope.MonitorHistoryExport = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = "MonitorHistory-" + $scope.SearchParam.MonitorDateFrom + " to " + $scope.SearchParam.MonitorDateTo + ".xlsx";
        $http({
            method: 'GET',
            url: '/MonitorForm/MonitorHistoryExport',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            console.error(response.data);
            return deferred.reject(response.data);
        });
    };


    /*============== get agent ==============*/
    $scope.GetAgents = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Agent/SearchAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.AssignmentDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.AgentList = response.data;
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    };


    /*============ EVALUATION CALL ============*/
    $scope.GetRecordInfoToMonitor = function (item) {
        angular.element('#Loading').LoadingOverlay("show");
        $scope.MonitorHistory.RecordLink = item.RecordLink;
        $scope.MonitorHistory = item;
        console.log('Monitor data: ', $scope.MonitorHistory);
        deferred.notify();
        $http({
            method: 'GET',
            url: '/MonitorForm/GetRecordInfoToMonitor',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.MonitorHistory
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            console.log("Record Info: ", response.data);            
            $scope.MonitorHistory = response.data;
            if (IsNullOrEmpty($scope.MonitorHistory.destination)) {
                $scope.MonitorHistory.destination = item.destination;
            } if (IsNullOrEmpty($scope.MonitorHistory.duration)) {
                $scope.MonitorHistory.duration = item.duration;
            } if (IsNullOrEmpty($scope.MonitorHistory.cnum)) {
                $scope.MonitorHistory.cnum = item.cnum;
            } if (IsNullOrEmpty($scope.MonitorHistory.uniqueid)) {
                $scope.MonitorHistory.uniqueid = item.uniqueid;
            } if (IsNullOrEmpty($scope.MonitorHistory.RecordID)) {
                $scope.MonitorHistory.RecordID = item.RecordID;
            } if (IsNullOrEmpty($scope.MonitorHistory.RecordLink)) {
                $scope.MonitorHistory.RecordLink = item.RecordLink;
            }
            $('#mdEvaluationCall').modal({
                backdrop: 'static',
                keyboard: false
            });
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    };

    $scope.GetMonitorDetail = function (item) {        
        $scope.MonitorHistory = item;
        deferred.notify();
        $http({
            method: 'GET',
            url: '/MonitorForm/GetMonitorDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.MonitorHistory
        }).then(function (response) {
            deferred.resolve(response);
            $scope.MonitorHistory = response.data;
            $scope.MonitorHistory.IsUpdate = true;
            $scope.BindEvaluationData(); // binding evaluation data detail into elements value
            console.log("Result Eval:", $scope.MonitorHistory);
            $('#mdEvaluationCall').modal({
                backdrop: 'static',
                keyboard: false
            });
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    };

    $scope.DeleteMonitor = function (item) {
        $.confirm({
            title: 'Confirm!',
            type: 'red',
            content: 'Do you want to delete?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $scope.MonitorHistory = item;
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/Records/DeleteMonitor',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.MonitorHistory
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code === 'SUCCESS') {
                                SuccessMessage(response.data.message);
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage('Unexpected error while saving data!!');
                            return deferred.reject(response.data);
                        });
                        return deferred.promise;
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });

    };

    $scope.CriterialEvaluationCallList = function () {
        deferred.notify();
        $http({
            method: 'GET',
            url: '/MonitorForm/CriterialEvaluationCall',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.MonitorHistory
        }).then(function (response) {
            deferred.resolve(response);
            if (!IsNullOrEmpty(response.data)) {
                //$scope.CriterialEvaluationCall.WELCOME = response.data.WELCOME;
                //$scope.CriterialEvaluationCall.CUSTOMER_RELATIONSHIP = response.data.CUSTOMER_RELATIONSHIP;
                //$scope.CriterialEvaluationCall.CONTENT = response.data.CONTENT;
                //$scope.CriterialEvaluationCall.FEELING = response.data.FEELING;
                //$scope.CriterialEvaluationCall.FINISH = response.data.FINISH;
                $scope.CriterialEvaluationCall = response.data;
            }
            console.log("Criterial Eval: ", $scope.CriterialEvaluationCall);
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    };

    $scope.EvaluationMarkList = function () {
        deferred.notify();
        $http({
            method: 'GET',
            url: '/MonitorForm/EvaluationMark',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.MonitorHistory
        }).then(function (response) {
            deferred.resolve(response);
            $scope.EvaluationMark = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    };

    /*=================== Save Evaluation Call ==================*/
    $scope.BindEvaluationData = function () {
        var EvaluationResult = $scope.MonitorHistory.EvaluationResult;
        if (!IsNullOrEmpty(EvaluationResult)) {
            for (var i = 0; i < EvaluationResult.length; i++) {
                if (EvaluationResult[i].CriterialID === $scope.CriterialEvaluationCall[i].CriterialID) {
                    $('#optionMark_' + i + '_' + $scope.CriterialEvaluationCall[i].CriterialID).val(EvaluationResult[i].EvalID);
                    $('#txtNote_' + i + '_' + $scope.CriterialEvaluationCall[i].CriterialID).val(EvaluationResult[i].Note);
                }
            }
        }
    };

    $scope.GetEvaluationData = function () {
        $scope.EvaluationDataParam = { EvalMark: [] };
        $scope.MonitorHistory.TotalMark = 0;
        if (!IsNullOrEmpty($scope.CriterialEvaluationCall)) {
            for (var i = 0; i < $scope.CriterialEvaluationCall.length; i++) {
                var mark = $('#optionMark_' + i + '_' + $scope.CriterialEvaluationCall[i].CriterialID).val();
                var note = $('#txtNote_' + i + '_' + $scope.CriterialEvaluationCall[i].CriterialID).val();
                $scope.EvaluationDataParam.EvalMark.push(
                    {
                        CriterialID: $scope.CriterialEvaluationCall[i].CriterialID,
                        Mark: mark,
                        Note: note
                    }
                );
                if (mark === 'EXCELLENT') {
                    $scope.MonitorHistory.TotalMark += 4;
                } else if (mark === 'GOOD') {
                    $scope.MonitorHistory.TotalMark += 3;
                } else if (mark === 'NORMAL') {
                    $scope.MonitorHistory.TotalMark += 2;
                } else if (mark === 'NEED_IMPROVEMENT') {
                    $scope.MonitorHistory.TotalMark += 1;
                } else {
                    $scope.MonitorHistory.TotalMark += 0;
                }
            }
        }
        console.log("Data eval: ", $scope.EvaluationDataParam.EvalMark);
        console.log("Monitor info: ", $scope.MonitorHistory);
    };

    $scope.SaveEvaluationCall = function () {
        if (IsNullOrEmpty($scope.MonitorHistory.calldate)) {
            ErrorMessage('Calldate is required');
            return false;
        }
        if (IsNullOrEmpty($scope.MonitorHistory.cnum)) {
            ErrorMessage('Agent is required');
            return false;
        }
        if (IsNullOrEmpty($scope.MonitorHistory.destination)) {
            ErrorMessage('Destination is required');
            return false;
        }
        if (IsNullOrEmpty($scope.MonitorHistory.duration)) {
            ErrorMessage('Duration is required');
            return false;
        }
        else {
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            $scope.GetEvaluationData();// get evaluation data into object
                            angular.element('#Loading').LoadingOverlay("show");
                            $http({
                                method: 'POST',
                                url: '/MonitorForm/SaveEvaluationCall',
                                headers: [{ 'Content-Type': 'application/json' }],
                                data: $scope.EvaluationDataParam,
                                params: $scope.MonitorHistory
                            }).then(function (response) {
                                angular.element('#Loading').LoadingOverlay("hide");
                                deferred.resolve(response);
                                if (response.data.code === 'SUCCESS') {
                                    $.alert({
                                        title: 'Message!',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'bg-green-active',
                                                action: function () {
                                                    $scope.ClearModel();
                                                    $scope.MonitorHistory.IsUpdate = false;
                                                    $('#mdEvaluationCall').modal('hide');
                                                    $scope.SearchMonitorHistory();
                                                }
                                            }
                                        }
                                    });
                                } else if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                } else if (response.data.code === 'SESSION_EXPIRED_MESSAGE') {
                                    $.alert({
                                        title: 'Message!',
                                        type: 'info',
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
                                }
                            }, function (response) {
                                angular.element('#Loading').LoadingOverlay("hide");
                                ErrorMessage(response.data);
                                return deferred.reject(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    };

    $scope.TeleOrgchartTab = function () {
        $scope.GetProjectList();
        $scope.SearchTeleOrgchart();
    },

    /*============= END EVAL ==============*/
    $scope.ClearModel = function () {
        $("select[id^='optionMark_']").val('');
        $("input[id^='txtNote_']").val('');
        $scope.MonitorHistory = {
            MonitorID: ''
            , MonitorBy: ''
            , MonitorDateTime: ''
            , CreateBy: ''
            , CreateDate: ''
            , TotalListening: ''
            , TotalMark: 0
            , EvalID: ''
            , RecordID: ''
            , CriterialID: ''
            , calldate: ''
            , uniqueid: ''
            , destination: ''
            , duration: ''
            , disposition: ''
            , recordingfile: ''
            , cnum: ''
            , contracts: ''
            , server_record: ''
            , FullName: ''
            , recordlink: ''
            , calldateYear: ''
            , calldateMon: ''
            , calldateDay: ''
            , EvaluationResult: []
            , IsUpdate: false
            , Campaign: ''
        };
    }

    /*========== RESET ===========*/
    $scope.Reset = function () {
        $scope.SearchParam = {
            calldate: GetLastDayFromNow(1)
            , calldate_to: GetLastDayFromNow(1)
            , destination: ''
            , RecordID: ''
            , duration: ''
            , disposition: ''
            , recordingfile: ''
            , cnum: ''
            , server_record: ''
            , LeaderExtension: ''
            , contracts: ''
            , MonitorDateFrom: GetCurrentDate()
            , MonitorDateTo: GetCurrentDate()
            , AgentExtension: ''
        };
        
    };
    /*========== INIT PAGE ===========*/
    $scope.UserSession = { UserId: '', UserName: '', IsAdmin: '', Extension: '', IsTeleSaleLeader: '' };
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.UserSession.Extension = $('#Extension').val();
        $scope.UserSession.IsTeleSaleLeader = $('#IsTeleSaleLeader').val();
        $scope.SearchTeleOrgchart();
        $scope.EvaluationMarkList();
        $scope.CriterialEvaluationCallList();
        $scope.GetLeaderList();
        $scope.GetAgentByLeader();
    };
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