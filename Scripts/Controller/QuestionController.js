'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'ngSanitize'])
dkshApp.controller('QuestionController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob, $sce, $sanitize) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = { ServiceTypeID: '', CreatedDate: '', CreatedBy: '' };
    $scope.ListQuestion = [];
    $scope.ListCampaign = [];
    $scope.AnswerTypeList = [];
    $scope.ListAnswer = [];
    $scope.isUpdateQuestion = false;
    $scope.isUpdateAnswer = false;
    $scope.isNewQuestion = false;
    $scope.isViewQuestion = false;
    $scope.isNewAnswer = false;
    $scope.UserSession = { UserId: '', UserName: '', IsAdmin: '' };
    $scope.CampaignName = '';
    $scope.QuestionDto = {
        ID: ''
        , QuestionText: ''
        , QuestionTextNonHTML: ''
        , QuestionCode: ''
        , QuestionTypeCode: ''
        , CreatedBy: ''
        , CreatedDate: ''
        , UpdatedBy: ''
        , UpdatedDate: ''
        , IsDeleted: ''
        , ServiceID: ''
        , ServiceTypeID: ''
        , ServiceName: ''
        , OrderIndex: ''
        , IsCampaignDetail: false
    };
    $scope.AnswersDto = {
        ID: ''
        , AnswerCode: ''
        , QuestionID: ''
        , ContentAnswer: ''
        , GroupCode: ''
        , AnswerTypeCode: ''
        , Mark: ''
        , OrderIndex: ''
        , AnswerValue: '1'
        , MinValue: '1'
        , MaxValue: '10'
        , CheckOther: ''
        , CreatedBy: ''
        , CreatedDate: ''
        , UpdatedBy: ''
        , UpdatedDate: ''
        , IsDeleted: ''
        , IsActive: ''
        , CampaignDetailId: ''
        , CampaignDescription: ''
        , ServiceTypeID: ''
    };
    $scope.CampaignDetailDto = {
        ServiceTypeId: ''
        , CampaignDescription: ''
        , CreateDate: ''
        , CreateBy: ''
        , CampaignDetailId: ''
        , QuestionId: ''
        , AnswerId: ''
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
    },
    /*========= PAGING =========*/

    /*============== Answer type ==============*/
    $scope.AnswerType = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Question/AnswerType',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.AnswerTypeList = response.data;
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /* Delete question */
    $scope.DeleteQuestion = function (item) {
        $scope.QuestionDto.ID = item;
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Do you want to delete ?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        angular.element('#Loading').LoadingOverlay("show");
                        $http({
                            method: 'POST',
                            url: '/Question/DeleteQuestion',
                            headers: [{ 'Content-Type': 'application/json' }],
                            data: $scope.QuestionDto
                        }).then(function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
                            deferred.resolve(response);
                            if (response.data.code === 'SUCCESS') {
                                $.alert({
                                    title: 'Message',
                                    type: 'green',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-green',
                                            action: function () {
                                                $scope.GetQuestion();
                                            }
                                        }
                                    }
                                });
                            } else if (response.data.code === 'INFO') {
                                InfoMessage(response.data.message);
                            } else {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
                            ErrorMessage('Unexpected error while delete. Please contact admin!!');
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });
    },

    /* DeleteAnswer */
    $scope.DeleteAnswer = function (item) {
        $scope.AnswersDto.ID = item;
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Do you want to delete ?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        angular.element('#Loading').LoadingOverlay("show");
                        $http({
                            method: 'POST',
                            url: '/Question/DeleteAnswer',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.AnswersDto
                        }).then(function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
                            deferred.resolve(response);
                            if (response.data.code === 'SUCCESS') {
                                $.alert({
                                    title: 'Message',
                                    type: 'green',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-green',
                                            action: function () {
                                                $scope.GetAnswerByQuestion($scope.AnswersDto.QuestionID);
                                            }
                                        }
                                    }
                                });
                            } else if (response.data.code === 'INFO') {
                                InfoMessage(response.data.message);
                            } else {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
                            ErrorMessage('Unexpected error while delete. Please contact admin!!');
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });
    },

    /*========== Get Question List ==========*/
    $scope.GetQuestion = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Question/GetQuestion',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            if (response.data != null) {
                $scope.ListQuestion = response.data;
                $scope.totalItems = response.data.length;
            } else {
                $scope.totalItems = 0;
            }
            $scope.SetPaging();
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /*=========== GET QUESTION BY ID =============*/
    $scope.GetQuestionByID = function (QuestionID) {
        $scope.QuestionDto = {};
        $scope.QuestionDto.ID = QuestionID;
        $scope.CampaignName = $('#ddlCampaign option:selected').text();
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Question/GetQuestionByID',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.QuestionDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            if (response.data !== null) {
                $scope.QuestionDto = response.data;
                if ($scope.isUpdateQuestion) {
                    $('#txtQuestionText').summernote('code', $scope.QuestionDto.QuestionText);
                }
                $('#mdQuestionInfo').modal({ backdrop: 'static' }, 'show');
            }
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /*================ Get answer by question ================*/
    $scope.GetAnswerByQuestion = function (QuestionID) {
        $scope.AnswersDto.QuestionID = QuestionID;
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Question/GetAnswerByQuestion',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.AnswersDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            if (response.data != null) {
                $scope.ListAnswer = response.data;
            }
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /*================= Get Answer By ID ==================*/
    $scope.GetAnswerByID = function (ID) {
        $scope.isUpdateAnswer = true;
        $scope.AnswersDto.ID = ID;
        $http({
            method: 'GET',
            url: '/Question/GetAnswerByID',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.AnswersDto
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data != null) {
                $scope.AnswersDto = response.data;
                $('#txtCampaignDescription').summernote('code', response.data.CampaignDescription);
            }
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    // Get question info to update 
    $scope.GetQuestionInfoUpdate = function (QuestionID) {
        $scope.AnswersDto.QuestionID = QuestionID;
        $scope.isViewQuestion = false;
        $scope.isUpdateQuestion = true;
        $scope.AnswerType();
        $scope.GetAnswerByQuestion(QuestionID);
        $scope.GetQuestionByID(QuestionID);
    },

    $scope.ViewQuestionDetail = function (QuestionID) {
        $scope.AnswersDto.QuestionID = QuestionID;
        $scope.isUpdateQuestion = false;
        $scope.isViewQuestion = true;
        $scope.isNewQuestion = false;
        $scope.GetQuestionByID(QuestionID);
        $scope.GetAnswerByQuestion(QuestionID);
    },

    $scope.Cancel = function () {
        $scope.isUpdateQuestion = false;
        $scope.isViewQuestion = false;
        $scope.isNewQuestion = false;
    },

    $scope.ResetCampaignText = function () {
        $('#txtCampaignDescription').summernote('code', '');
    },
    /*==========================Submit Question Info=========================*/
    $scope.SubmitQuestionInfo = function () {
        if (IsNullOrEmpty($scope.QuestionDto.ServiceTypeID)) {
            ErrorMessage('<strong>Campaign</strong> is required');
            return false;
        } if (IsNullOrEmpty($scope.QuestionDto.QuestionCode)) {
            ErrorMessage('<strong>Question code</strong> is required');
            return false;
        } if (IsNullOrEmpty($scope.QuestionDto.OrderIndex)) {
            ErrorMessage('<strong>Order index</strong> is required');
            return false;
        } if (IsNullOrEmpty($('#txtQuestionText').val()) || IsNullOrEmpty($('#txtQuestionText').summernote('code'))) {
            ErrorMessage('<strong>Question content</strong> is required');
            return false;
        } else {
            $scope.QuestionDto.QuestionText = $('#txtQuestionText').summernote('code');
            $scope.QuestionDto.QuestionTextNonHTML = $("#txtQuestionText").summernote('code').replace(/<\/?[^>]+(>|$)/g, "");
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            if ($scope.isUpdateQuestion) {
                                $scope.UpdateQuestion();
                            } else if ($scope.isNewQuestion) {
                                $scope.InsertQuestion();
                            }
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    },
    /*================ Update question ================*/
    $scope.UpdateQuestion = function () {
        $http({
            method: 'POST',
            url: '/Question/UpdateQuestion',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.QuestionDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.isUpdateQuestion = false;
            if (response.data.code === 'SUCCESS') {
                SuccessMessage(response.data.message);
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
    },

    /*================== Insert Question ==================*/
    $scope.InsertQuestion = function () {
        $http({
            method: 'POST',
            url: '/Question/InsertQuestion',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.QuestionDto
        }).then(function (response) {
            deferred.resolve(response);
            if (!IsNullOrEmpty(response.data.Question)) {
                $scope.QuestionDto = response.data.Question;
                $scope.AnswersDto.QuestionID = $scope.QuestionDto.ID;
            }
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message!',
                    type: 'info',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green-leaf',
                            action: function () {
                                $scope.ResetModel();
                                $('#mdQuestionInfo').modal('hide');
                            }
                        },
                        continue: {
                            btnClass: 'bg-olive',
                            action: function () {
                                $scope.ResetModel();
                                $scope.GetQuestion();
                            }
                        },
                        answer: {
                            btnClass: 'bg-orange',
                            action: function () {
                                $scope.NewAnswer();
                                $('#txtQuestionCode').val('');
                                $('#txtOrderIndex').val('');
                                $('#txtQuestionText').val('');
                                $('#txtQuestionText').summernote('code', '');
                                $("#tabQuestion").removeClass('active');
                                $("#QuestionTab").removeClass('active');
                                $("#tabAnswer").addClass('active');
                                $("#AnswerTab").addClass('active');
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
    },

    /*============== Submit answer information ===============*/
    $scope.SubmitAnswersInfo = function () {
        if (IsNullOrEmpty($scope.AnswersDto.QuestionID)) {
            ErrorMessage('There is no question selected');
            return false;
        }
        if (IsNullOrEmpty($scope.AnswersDto.AnswerCode)) {
            ErrorMessage('<strong>Answer code</strong> is required');
            return false;
        } if (IsNullOrEmpty($scope.AnswersDto.AnswerTypeCode)) {
            ErrorMessage('<strong>Answer Type Code</strong> is required');
            return false;
        } if (IsNullOrEmpty($scope.AnswersDto.OrderIndex)) {
            ErrorMessage('<strong>Order Index</strong> is required');
            return false;
        } if (!IsNullOrEmpty($scope.QuestionDto.IsCampaignDetail) && $scope.QuestionDto.IsCampaignDetail == true
            && IsNullOrEmpty($('#txtCampaignDescription').summernote('code'))) {
            ErrorMessage('<strong>Campaign Detail</strong> is required');
            return false;
        } else
            $scope.AnswersDto.CampaignDescription = $('#txtCampaignDescription').summernote('code');
        $scope.AnswersDto.ServiceTypeID = $scope.QuestionDto.ServiceTypeID;
        console.log("Answer Dto: ", $scope.AnswersDto);
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Do you want to save ?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        if (!$scope.isNewAnswer) {
                            $scope.UpdateAnswer();
                        } else {
                            $scope.InsertAnswer();
                        }
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });
    },



    /*=============== UPDATE ANSWER ==============*/
    $scope.UpdateAnswer = function () {
        $http({
            method: 'POST',
            url: '/Question/UpdateAnswer',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.AnswersDto
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.isNewAnswer = false;
                                $scope.isUpdateAnswer = false;
                                $scope.AnswersDto.CampaignDescription = '';
                                $scope.GetAnswerByQuestion($scope.AnswersDto.QuestionID);
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
    },

    /*================= INSERT ANSWER ==================*/
    $scope.InsertAnswer = function () {
        $http({
            method: 'POST',
            url: '/Question/InsertAnswer',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.AnswersDto
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.NewAnswer();
                                $scope.GetAnswerByQuestion($scope.AnswersDto.QuestionID);
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
    },
    /*========== RESET ===========*/
    $scope.Reset = function () {
        $scope.SearchParam = { ServiceTypeID: '', CreatedDate: '', CreatedBy: '' };
    },

    /* New Answer */
    $scope.NewAnswer = function () {
        if (IsNullOrEmpty($scope.AnswersDto.QuestionID)) {
            ErrorMessage('There is no question selected');
            return false;
        } else {
            $scope.isNewAnswer = true;
            $scope.isUpdateAnswer = false;
            $scope.AnswersDto = {
                ID: ''
                , AnswerCode: ''
                , QuestionID: $scope.AnswersDto.QuestionID
                , ContentAnswer: ''
                , GroupCode: ''
                , AnswerTypeCode: ''
                , Mark: ''
                , OrderIndex: ''
                , AnswerValue: '1'
                , MinValue: '1'
                , MaxValue: '10'
                , CheckOther: ''
                , CreatedBy: ''
                , CreatedDate: ''
                , UpdatedBy: ''
                , UpdatedDate: ''
                , IsDeleted: false
                , IsActive: ''
            };
        }        
    },

    /* Get Campaign List */
    $scope.GetCampaignList = function () {
        $http({
            method: 'GET',
            url: '/SearchCall/GetCampaignList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListCampaign = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.ResetModel = function () {
        $scope.ListAnswer = [];
        $scope.isUpdateAnswer = false;
        $scope.isUpdateQuestion = false;
        $scope.isNewAnswer = true;
        $scope.isNewQuestion = true;
        $scope.isViewQuestion = false;
        $('#txtQuestionCode').val('');
        $('#txtOrderIndex').val('');
        $('#txtQuestionText').val('');
        $('#txtQuestionText').summernote('code', '');
        $("#tabQuestion").addClass('active');
        $("#QuestionTab").addClass('active');
        $("#tabAnswer").removeClass('active');
        $("#AnswerTab").removeClass('active');
        $scope.QuestionDto = {
            ID: ''
            , QuestionText: ''
            , QuestionCode: ''
            , QuestionTypeCode: ''
            , CreatedBy: ''
            , CreatedDate: ''
            , UpdatedBy: ''
            , UpdatedDate: ''
            , IsDeleted: ''
            , ServiceID: ''
            , ServiceTypeID: $scope.SearchParam.ServiceTypeID
            , ServiceName: ''
            , OrderIndex: ''
            , IsCampaignDetail: false
        };
        $scope.AnswersDto = {
            ID: ''
            , AnswerCode: ''
            , QuestionID: ''
            , ContentAnswer: ''
            , GroupCode: ''
            , AnswerTypeCode: ''
            , Mark: ''
            , OrderIndex: ''
            , AnswerValue: ''
            , MinValue: ''
            , MaxValue: ''
            , CheckOther: ''
            , CreatedBy: ''
            , CreatedDate: ''
            , UpdatedBy: ''
            , UpdatedDate: ''
            , IsDeleted: ''
            , IsActive: ''
            , CampaignDetailId: ''
        };
    },


    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.SearchParam.ServiceTypeID = $('#CampaignID').val();
        $scope.SetPaging();
        $scope.GetCampaignList();
        $scope.GetQuestion();
        $scope.AnswerType();
    },
    $scope.initPage();

    /*========== INIT PAGE ===========*/

}).filter('cut', function () {
    return function (value, wordwise, max, tail) {
        if (!value) return '';

        max = parseInt(max, 10);
        if (!max) return value;
        if (value.length <= max) return value;

        value = value.substr(0, max);
        if (wordwise) {
            var lastspace = value.lastIndexOf(' ');
            if (lastspace !== -1) {
                //Also remove . and , so its gives a cleaner result.
                if (value.charAt(lastspace - 1) === '.' || value.charAt(lastspace - 1) === ',') {
                    lastspace = lastspace - 1;
                }
                value = value.substr(0, lastspace);
            }
        }
        return value + (tail || ' …');
    };
}).filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
});