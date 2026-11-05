'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver'])
    .controller('CDIDashboardController', function ($scope, $http, $location, $window, $log, $rootScope, $filter, $q, FileSaver, Blob) {
    var deferred = $q.defer();
    var d = new Date();
    $scope.CDIDashboardModel = {
        CustCodeByUser: '',
        LicenseType: '',
        Years: '',
        FromMonth: '',
        ToMonth:'',
        Month: '',
        CurrentChannel: ''
    };
    $scope.TotalMonths = {
        TotalJanuary: 0,
        TotalFebruary: 0,
        TotalMarch: 0,
        TotalApril: 0,
        TotalMay: 0,
        TotalJune: 0,
        TotalJuly: 0,
        TotalAugust: 0,
        TotalSeptember: 0,
        TotalOctorber: 0,
        TotalNovember: 0,
        TotalDecember: 0
    }
    $scope.LicensesSummary = {
        TotalCustomerPharmacy: '',
        TotalCustomerDianostics: '',
        TotalCustomerOthers:'',
        TotalCustomerHospitalClinic: '',
        TotalCustomerActive: '',
        TotalCustCodeLackLicense: '',
        TotalGPPExpired: '',
        TotalGPPMissed: '',
        TotalGDPExpired: '',
        TotalGDPMissed: '',
        TotalTaxCodeMissed: '',
        TotalGPKDMissed: '',
        TotalCOETTBYTMissed: '',
        TotalGCNDDKKDDExpired: '',
        TotalGCNDDKKDDMissed: '',
        TotalKCBExpired: '',
        TotalKCBMissed: '',
        TotalDMKTBotoxXeominMissed: '',
        TotalGPPComingExpired: '',
        TotalGDPComingExpired: '',
        TotalGSPComingExpired: '',
        TotalGCNDDKKDDComingExpired: '',
        TotalKCBComingExpired: '',
        TotalGCNDDKTCComingExpired: '',
        TotalLicenseComingExpired: '',
        TotalTaxCodeValid: '',
        TotalTaxCodeInValid: '',
        TaxCodeNoProvide: '',
        TotalTaxCodeBlank: '',
        TaxCodeNoControlLicense: '',
        TotalTaxCode: '',
        TotalLicensesExpired: '',
        TotalLackOfLicenses: '',
        TotalNormal: '',
        TotalBuyPhoiHop: '',
        TotalBuyThuocDoc: '',
        TotalBuyThuocCam: '',
        TotalBuyBotox: '',
        TotalBuySinhPham: '',
        TotalBuyVaccin: '',
        TotalNormalSpecial: '',
        TotalFullPharmacy: '',
        TotalFullDiagnostics: '',
        TotalFullOthers: '',
        TotalFullHospitalDrClinic: '',
        TotalLackOfLicensePharmacy: '',
        TotalLackOfLicenseDiagnostics: '',
        TotalLackOfLicenseOthers: '',
        TotalLackOfLicenseHospitalDrClinic: '',
        TotalFullChannels: '',
        TotalLackOfLicenseChannels: ''
    }

    $scope.ChannelCustomer = [];
    
    $scope.extensionArr = [{ key: 'xls', value: 'xls' }, { key: 'xlsx', value: 'xlsx' }];
    $scope.extension = '';
    $scope.SummaryChannel = [];
    /*========== GET CUSTOMER CODE BY DAY / MONTH / YEAR ===========*/
    $scope.GetCDIDashboard = function () {
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/DashboardCDI/GetCDIDashboard',
            headers: [{ 'Content-Type': 'application/json' }],
            params: null
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            if (!IsNullOrEmpty(response.data.LicensesSummaryDto)) {
                $scope.LicensesSummary = response.data.LicensesSummaryDto[0];
                $scope.LicensesSummary.TotalLicenseComingExpired = $scope.LicensesSummary.TotalGPPComingExpired +
                    $scope.LicensesSummary.TotalGDPComingExpired + $scope.LicensesSummary.TotalGSPComingExpired +
                    $scope.LicensesSummary.TotalGCNDDKKDDComingExpired + $scope.LicensesSummary.TotalKCBComingExpired +
                    $scope.LicensesSummary.TotalGCNDDKTCComingExpired;
                $scope.LicensesSummary.TotalTaxCodeInValid = $scope.LicensesSummary.TotalTaxCodeBlank +
                    $scope.LicensesSummary.TaxCodeNoProvide;
                $scope.LicensesSummary.TotalTaxCode = $scope.LicensesSummary.TotalCustomerActive;
                //$scope.LicensesSummary.TotalLicensesExpired = $scope.LicensesSummary.TotalGPPExpired + $scope.LicensesSummary.TotalGDPExpired
                //    + $scope.LicensesSummary.TotalGCNDDKKDDExpired + $scope.LicensesSummary.TotalKCBExpired;

                $scope.LicensesSummary.TotalFullChannels = $scope.LicensesSummary.TotalFullPharmacy + $scope.LicensesSummary.TotalFullDiagnostics
                    + $scope.LicensesSummary.TotalFullOthers
                    + $scope.LicensesSummary.TotalFullHospitalDrClinic;
                $scope.LicensesSummary.TotalLackOfLicenseChannels = $scope.LicensesSummary.TotalLackOfLicensePharmacy
                    + $scope.LicensesSummary.TotalLackOfLicenseDiagnostics
                    + $scope.LicensesSummary.TotalLackOfLicenseOthers
                    + $scope.LicensesSummary.TotalLackOfLicenseHospitalDrClinic;
                //$scope.LicensesSummary.TotalLackOfLicenses = $scope.LicensesSummary.TotalDMKTBotoxXeominMissed + $scope.LicensesSummary.TotalTaxCodeMissed
                //    + $scope.LicensesSummary.TotalGPKDMissed + $scope.LicensesSummary.TotalKCBMissed
                //    + $scope.LicensesSummary.TotalGCNDDKKDDMissed + $scope.LicensesSummary.TotalGDPMissed
                //    + $scope.LicensesSummary.TotalGPPMissed;
                // LOAD GOOGLE CHART 
                $scope.LoadPieChartChannels(); // Customers by channels
                $scope.LoadBarChartDDKXB(); // Du dieu kien xuat ban
                $scope.LoadStackedChartChannel(); // Status licenses by channels
                $scope.ChannelCustomer = response.data.Channel;
            }
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    }

    $scope.Export = function (LicenseType) {
        var fileName = '';
        if (LicenseType === 'GPP_EXPIRED') {
            fileName = 'GPPEXPIRED.xlsx';
        } else if (LicenseType === 'GDP_EXPIRED') {
            fileName = 'GDPEXPIRED.xlsx';
        } else if (LicenseType === 'GCNDDKKDD_EXPIRED') {
            fileName = 'GCNDDKKDDEXPIRED.xlsx';
        } else if (LicenseType === 'KCB_EXPIRED') {
            fileName = 'KCBEXPIRED.xlsx';
        } else if (LicenseType === 'TOTAL_LICENSES_EXPIRED') {
            fileName = 'TOTAL_LICENSES_EXPIRED.xlsx';
        } else if (LicenseType === 'GPP_LACK') {
            fileName = 'GPPLACK.xlsx';
        } else if (LicenseType === 'GDP_LACK') {
            fileName = 'GDPLACK.xlsx';
        } else if (LicenseType === 'GCNDDKKDD_LACK') {
            fileName = 'GCNDDKKDDLACK.xlsx';
        } else if (LicenseType === 'KCB_LACK') {
            fileName = 'KCBLACK.xlsx';
        } else if (LicenseType === 'GPKD') {
            fileName = 'GPKD.xlsx';
        } else if (LicenseType === 'TAXCODE') {
            fileName = 'TAXCODE.xlsx';
        } else if (LicenseType === 'BOTOX_XEOMIN') {
            fileName = 'BOTOX_XEOMIN.xlsx';
        } else if (LicenseType === 'TOTAL_LACK_OF_LICENSES') {
            fileName = 'TOTAL_LACK_OF_LICENSES.xlsx';
        } else if (LicenseType === 'GPP_COMING_EXPIRED') {
            fileName = 'GPP_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'GDP_COMING_EXPIRED') {
            fileName = 'GDP_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'GSP_COMING_EXPIRED') {
            fileName = 'GSP_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'GCNDDKKDD_COMING_EXPIRED') {
            fileName = 'GCNDDKKDD_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'KCB_COMING_EXPIRED') {
            fileName = 'KCB_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'GCNDDKTC_COMING_EXPIRED') {
            fileName = 'GCNDDKTC_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'TOTAL_LICENSES_COMING_EXPIRED') {
            fileName = 'TOTAL_LICENSES_COMING_EXPIRED.xlsx';
        } else if (LicenseType === 'TAXCODE_VALID') {
            fileName = 'TAXCODE_VALID.xlsx';
        } else if (LicenseType === 'TAXCODE_BLANK') {
            fileName = 'TAXCODE_BLANK.xlsx';
        } else if (LicenseType === 'TAXCODE_NO_PROVIDE') {
            fileName = 'TAXCODE_NO_PROVIDE.xlsx';
        } else if (LicenseType === 'TAXCODE_NO_CONTROL_LICENSES') {
            fileName = 'TAXCODE_NO_CONTROL_LICENSES.xlsx';
        } else if (LicenseType === 'ALL_CUSTOMERS_ACTIVE') {
            fileName = 'ALL_CUSTOMERS_ACTIVE.xlsx';
        } else if (LicenseType === 'TOTAL_TAXCODE') {
            fileName = 'TOTAL_TAXCODE.xlsx';
        } else if (LicenseType === 'ALL_CUSTOMERS_TAXCODE_VALID') {
            fileName = 'ALL_CUSTOMERS_TAXCODE_VALID.xlsx';
        } else if (LicenseType === 'ALL_CUSTOMERS_LICENSES_EXPIRED_LACK') {
            fileName = 'ALL_CUSTOMERS_LICENSES_EXPIRED_LACK.xlsx';
        } else if (LicenseType === 'ALL_CUSTOMERS_BLANK_NOPROVIDED') {
            fileName = 'ALL_CUSTOMERS_BLANK_NOPROVIDED.xlsx';
        } else if (LicenseType === 'PHARMACY_PHARMACYCOMPANY') {
            fileName = 'PHARMACY_PHARMACYCOMPANY.xlsx';
        } else if (LicenseType === 'DIANOSTICS') {
            fileName = 'DIANOSTICS.xlsx';
        } else if (LicenseType === 'OTHERS') {
            fileName = 'OTHERS.xlsx';
        } else if (LicenseType === 'NORMAL') {
            fileName = 'NORMAL.xlsx';
        } else if (LicenseType === 'PHOI_HOP') {
            fileName = 'PHOI_HOP.xlsx';
        } else if (LicenseType === 'THUOC_DOC') {
            fileName = 'THUOC_DOC.xlsx';
        } else if (LicenseType === 'THUOC_CAM') {
            fileName = 'THUOC_CAM.xlsx';
        } else if (LicenseType === 'SINH_PHAM') {
            fileName = 'SINH_PHAM.xlsx';
        } else if (LicenseType === 'VACCIN') {
            fileName = 'VACCIN.xlsx';
        } else if (LicenseType === 'BUY_BOTOX_XEOMIN') {
            fileName = 'BUY_BOTOX_XEOMIN.xlsx';
        } else if (LicenseType === 'ALL_NORMAL_SPECIAL') {
            fileName = 'ALL_NORMAL_SPECIAL.xlsx';
        } else if (LicenseType === 'TOTAL_FULL_CHANNELS') {
            fileName = 'TOTAL_FULL_CHANNELS.xlsx';
        } else if (LicenseType === 'PHARMACY_FULL') {
            fileName = 'PHARMACY_FULL.xlsx';
        } else if (LicenseType === 'DIAGNOSTICS_FULL') {
            fileName = 'DIAGNOSTICS_FULL.xlsx';
        } else if (LicenseType === 'OTHERS_FULL') {
            fileName = 'OTHERS_FULL.xlsx';
        } else if (LicenseType === 'HOSPITAL_DRCLINIC_FULL') {
            fileName = 'HOSPITAL_DRCLINIC_FULL.xlsx';
        } else if (LicenseType === 'TOTAL_LACKOFLICENSE_CHANNELS') {
            fileName = 'TOTAL_LACKOFLICENSE_CHANNELS.xlsx';
        } else if (LicenseType === 'PHARMACY_LACKOFLICENSE') {
            fileName = 'PHARMACY_LACKOFLICENSE.xlsx';
        } else if (LicenseType === 'DIAGNOSTICS_LACKOFLICENSE') {
            fileName = 'DIAGNOSTICS_LACKOFLICENSE.xlsx';
        } else if (LicenseType === 'OTHERS_LACKOFLICENSE') {
            fileName = 'OTHERS_LACKOFLICENSE.xlsx';
        } else if (LicenseType === 'HOSPITAL_DRCLINIC_LACKOFLICENSE') {
            fileName = 'HOSPITAL_DRCLINIC_LACKOFLICENSE.xlsx';
        }
        $scope.CDIDashboardModel.LicenseType = LicenseType;
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/DashboardCDI/Export',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CDIDashboardModel,
            responseType: 'blob'
        }).then(function (response) {
            console.log(response.data.type);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileName);
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }

    /*===================================== BEGIN GOOGLE CHART ========================================*/
    $scope.LoadPieChartChannels = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(CustomerChannelChart);

        function CustomerChannelChart() {

            var data = google.visualization.arrayToDataTable([
                ['Customers', 'Channels'],
                ['Pharmacy & P.Company', $scope.LicensesSummary.TotalCustomerPharmacy],
                ['Diagnostics', $scope.LicensesSummary.TotalCustomerDianostics],
                ['Others', $scope.LicensesSummary.TotalCustomerOthers],
                ['Hospital & Dr. Clinic', $scope.LicensesSummary.TotalCustomerHospitalClinic]
            ]);

            var options = {
                //width: '800',
                //height: '500'
            };

            var chart = new google.visualization.PieChart(document.getElementById('PieChartChannel'));

            chart.draw(data, options);
        }
    }

    $scope.LoadBarChartDDKXB = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(DDKXBBarChart);

        function DDKXBBarChart() {
            var data = new google.visualization.DataTable();
            var data = google.visualization.arrayToDataTable([
                ['Điều kiện xuất bán', 'Quantity', { role: 'style' }, { role: 'annotation' }],
                ['Hàng thường', $scope.LicensesSummary.TotalNormal, '#00a65a', $scope.LicensesSummary.TotalNormal],
                ['Thuốc dạng phối hợp có chứa dược chất gây nghiện', $scope.LicensesSummary.TotalBuyPhoiHop, '#dd4b39', $scope.LicensesSummary.TotalBuyPhoiHop],
                ['Thuốc độc, nguyên liệu độc làm thuốc', $scope.LicensesSummary.TotalBuyThuocDoc, '#dd4b39', $scope.LicensesSummary.TotalBuyThuocDoc],
                ['Thuốc dược chất thuộc danh mục bị cấm sử dụng', $scope.LicensesSummary.TotalBuyThuocCam, '#dd4b39', $scope.LicensesSummary.TotalBuyThuocCam],
                ['All (0), (1), (2), (3)', $scope.LicensesSummary.TotalNormalSpecial, '#dd4b39', $scope.LicensesSummary.TotalNormalSpecial],
                ['Vaccin', $scope.LicensesSummary.TotalBuyVaccin, '#3c8dbc', $scope.LicensesSummary.TotalBuyVaccin],
                ['Botox | Xeomin (Botulinum)', $scope.LicensesSummary.TotalBuyBotox, '#3c8dbc', $scope.LicensesSummary.TotalBuyBotox],
            ]);

            var options = {
                legend: { position: 'none' },
                width: '650',
                height: '400',
                //hAxis: {
                //    title: 'Quantity customers ' + $filter('number')($scope.LicensesSummary.TotalCustomerActive, 0),
                //},
                //vAxis: {
                    
                //}
            };

            var chart = new google.visualization.BarChart(document.getElementById('BarChartDDKXB'));

            chart.draw(data, options);
        }
    }

    $scope.LoadStackedChartChannel = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(StatusChannelsStackedChart);

        function StatusChannelsStackedChart() {
            var data = new google.visualization.DataTable();
            var data = google.visualization.arrayToDataTable([
                ['Channel', 'Full Licenses', 'Lack of Licenses'],
                ['Pharmacy & Pharmacy Co.', $scope.LicensesSummary.TotalFullPharmacy, $scope.LicensesSummary.TotalLackOfLicensePharmacy],
                ['Diagnostics', $scope.LicensesSummary.TotalFullDiagnostics, $scope.LicensesSummary.TotalLackOfLicenseDiagnostics], 
                ['Others', $scope.LicensesSummary.TotalFullOthers, $scope.LicensesSummary.TotalLackOfLicenseOthers],
                ['Hospital & Dr. Clinic', $scope.LicensesSummary.TotalFullHospitalDrClinic,
                    $scope.LicensesSummary.TotalLackOfLicenseHospitalDrClinic]
            ]);

            var options = {
                legend: { position: 'none' },
                colors: ['#109618', '#DC3912'],
                width: '650',
                height: '400',
                isStacked: 'percent'
            };

            var chart = new google.visualization.ColumnChart(document.getElementById('StackedChartChannels'));

            chart.draw(data, options);
        }
    }
    /*===================================== END GOOGLE CHART ========================================*/
    
    /*===== Send Mail To Supervisor =====*/
    $scope.SendMailToSuperVisor = function () {
        $http({
            method: 'GET',
            url: '/DashboardCDI/SendMailToSuperVisor',
            headers: [{ 'Content-Type': 'application/json' }],
            params: {}
        }).then(function (response) {
            if (response.data.code === 'SUCCESS') {
                SuccessMessage(response.data.message);
            } else if (response.data.code === 'ERROR') {
                ErrorMessage(response.data.message);
            }
        }, function (response) {
            console.log(response.data);
            return deferred.reject(response.data);
        });
    };
    /*====================== SummaryChannelHistory ======================*/
    $scope.SummaryChannelHistory = function () {
        $scope.ClearTotal();
        $http({
            method: 'GET',
            url: '/DashboardCDI/SummaryChannelHistory',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CDIDashboardModel,
        }).then(function (response) {
            $scope.SummaryChannel = response.data;
            if ($scope.SummaryChannel != null && $scope.SummaryChannel.length > 0) {
                for (var i = 0; i < $scope.SummaryChannel.length; i++) {
                    $scope.TotalMonths.TotalJanuary += parseInt($scope.SummaryChannel[i].January);
                    $scope.TotalMonths.TotalFebruary += parseInt($scope.SummaryChannel[i].February);
                    $scope.TotalMonths.TotalMarch += parseInt($scope.SummaryChannel[i].March);
                    $scope.TotalMonths.TotalApril += parseInt($scope.SummaryChannel[i].April);
                    $scope.TotalMonths.TotalMay += parseInt($scope.SummaryChannel[i].May);
                    $scope.TotalMonths.TotalJune += parseInt($scope.SummaryChannel[i].June);
                    $scope.TotalMonths.TotalJuly += parseInt($scope.SummaryChannel[i].July);
                    $scope.TotalMonths.TotalAugust += parseInt($scope.SummaryChannel[i].August);
                    $scope.TotalMonths.TotalSeptember += parseInt($scope.SummaryChannel[i].September);
                    $scope.TotalMonths.TotalOctorber += parseInt($scope.SummaryChannel[i].Octorber);
                    $scope.TotalMonths.TotalNovember += parseInt($scope.SummaryChannel[i].November);
                    $scope.TotalMonths.TotalDecember += parseInt($scope.SummaryChannel[i].December);
                }
            }
            
        }, function (response) {
            console.log(response.data);
            return deferred.reject(response.data);
        });
    };

    /*====================== View Channel Detail ======================*/
    $scope.ViewChannelHistoryDetail = function (mon, item) {
        $('#Loading').LoadingOverlay("show");
        $scope.CDIDashboardModel.ToMonth = mon + '-' + $scope.CDIDashboardModel.Years;
        $scope.CDIDashboardModel.CurrentChannel = item.NewChannel;
        var fileName = $scope.CDIDashboardModel.ToMonth + '-' + item.NewChannel + '.xlsx';
        $http({
            method: 'GET',
            url: '/DashboardCDI/ViewDetailChannel',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CDIDashboardModel,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileName);
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }

    /*====================== View Channel Detail ======================*/
    $scope.ViewTotalEveryMonth = function (mon) {
        //$scope.ClearTotal();
        $('#Loading').LoadingOverlay("show");
        $scope.CDIDashboardModel.ToMonth = mon + '-' + $scope.CDIDashboardModel.Years;
        var fileName = $scope.CDIDashboardModel.ToMonth + '.xlsx';
        $http({
            method: 'GET',
            url: '/DashboardCDI/ViewTotalEveryMonth',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CDIDashboardModel,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileName);
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage("Unexpected error while loading data");
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }

    $scope.CompareAnyMonth = function (mon) {
        if (IsNullOrEmpty($scope.CDIDashboardModel.FromMonth) || IsNullOrEmpty($scope.CDIDashboardModel.ToMonth)) {
            WarningMessage('Please choose two months');
        }
        else {
            $('#Loading').LoadingOverlay("show");
            var fileName = $scope.CDIDashboardModel.FromMonth + '_' + $scope.CDIDashboardModel.ToMonth + '.xlsx';
            $http({
                method: 'GET',
                url: '/DashboardCDI/CompareAnyMonth',
                headers: [{ 'Content-Type': 'application/json' }],
                params: $scope.CDIDashboardModel,
                responseType: 'blob'
            }).then(function (response) {
                $('#Loading').LoadingOverlay("hide");
                var data = new Blob([response.data], { type: response.data.type });
                FileSaver.saveAs(data, fileName);
            }, function (response) {
                $('#Loading').LoadingOverlay("hide");
                ErrorMessage("Unexpected error while loading data");
                console.log(response.data);
                return deferred.reject(response.data);
            });
        }
    }

    $scope.ClearCompare = function () {
        $scope.CDIDashboardModel.FromMonth = '';
        $scope.CDIDashboardModel.ToMonth = '';
        $scope.CDIDashboardModel.CurrentChannel = '';
    }

    $scope.ClearTotal = function () {
        $scope.TotalMonths = {
            TotalJanuary: 0,
            TotalFebruary: 0,
            TotalMarch: 0,
            TotalApril: 0,
            TotalMay: 0,
            TotalJune: 0,
            TotalJuly: 0,
            TotalAugust: 0,
            TotalSeptember: 0,
            TotalOctorber: 0,
            TotalNovember: 0,
            TotalDecember: 0
        }
    }
    /*===== INIT PAGE =====*/
    $scope.initPage = function () {
        $scope.CDIDashboardModel.Years = d.getFullYear();
        $scope.GetCDIDashboard();
        $scope.SummaryChannelHistory();
        //$scope.SendMailToSuperVisor();
    }
    $scope.initPage();
    /*===== INIT PAGE =====*/

})