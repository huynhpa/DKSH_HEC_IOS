
// Check string is null or empty
var IsNullOrEmpty = function (value) {
    return (!value || value == undefined || value == "" || value.length == 0);
}

// Format date
var formatString = function (format) {
    var pieces = format.split('-'),
        year = parseInt(pieces[2]),
        month = parseInt(pieces[1]),
        day = parseInt(pieces[0]),
        date = new Date(year, month - 1, day);
    return date;
}

var copyClipboard = function (elementId) {
    var copyText = document.getElementById("#" + elementId);
    copyText.select();
    copyText.setSelectionRange(0, 99999)
    document.execCommand("copy");
}

var ValidatePhoneNumber = function (input_str)
{
    var re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    console.log("Validate phone number: ", re.test(input_str));
    return re.test(input_str);
}

/*===== Calculate number of days between 2 date =====*/
var DateDiff = function (toDate) {
    console.log("toDate ", toDate);
    var dt = new Date();
    var currDate = dt.getDate() + '-' + (dt.getMonth() + 1) + '-' + dt.getFullYear();
    console.log("currDate ",currDate);
    var now = formatString(currDate);
    var endDate = formatString(toDate);
    var timeDiff = endDate.getTime() - now.getTime();
    var diffDays = Math.round(timeDiff / (1000 * 3600 * 24));
    console.log("diffDays ",diffDays);
    return diffDays;
}

//==================== SUCCESS MESSAGE
var SuccessMessage = function (message) {
    $.alert({
        title: 'Message!',
        type: 'green',
        content: '<strong>'+message+'</strong>',
        typeAnimated: true,
        buttons: {
            ok: {
                btnClass: 'btn-green'
            }
        }
    });
}
var WarningMessage = function (message) {
    $.alert({
        title: 'Message!',
        type: 'orange',
        content: '<strong>' + message + '</strong>',
        buttons: {
            ok: {
                btnClass: 'btn-warning'
            }
        }
    });
}
var InfoMessage = function (message) {
    $.alert({
        title: 'Message!',
        type: 'aqua',
        content: '<strong>' + message + '</strong>',
        buttons: {
            ok: {
                btnClass: 'btn-info'
            }
        }
    });
}

var CheckLengthDate = function (date) {
    if (date.length < 10) {
        $.alert({
            title: 'Alert!',
            type: 'red',
            content: "Data format error, please check date format",
            buttons: {
                ok: {
                    btnClass: 'btn-red'
                }
            }
        });
    }
}

//==================== ERROR MESSAGE
var ErrorMessage = function (message) {
    $.alert({
        title: 'Alert!',
        type: 'red',        
        content: '<strong>' + message + '</strong>',
        buttons: {
            ok: {
                btnClass: 'btn-red'
            }
        }
    });
}

//==================== GET CONTENT TYPE
var GenerateFileType = function (fileExtension) {
    var FileType = '';
    switch (fileExtension.toLowerCase()) {
        case ".doc":
        case ".docx":
            FileType = "application/msword";
            break;
        case ".xls":
        case ".xlsx":
            FileType = "application/vnd.ms-excel";
            break;
        case ".pps":
        case ".ppt":
            FileType = "application/vnd.ms-powerpoint";
            break;
        case ".txt":
            FileType = "text/plain";
            break;
        case ".rtf":
            FileType = "application/rtf";
            break;
        case ".pdf":
            FileType = "application/pdf";
            break;
        case ".msg":
        case ".eml":
            FileType = "application/vnd.ms-outlook";
            break;
        case ".gif":
        case ".bmp":
        case ".png":
        case ".jpg":
            FileType = "image/JPEG";
            break;
        case ".dwg":
            FileType = "application/acad";
            break;
        case ".zip":
            FileType = "application/x-zip-compressed";
            break;
        case ".rar":
            FileType = "application/x-rar-compressed";
            break;
    }
    return FileType;
}

var GetCurrentDate = function () {
    var date = new Date();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    if (month < 10) {
        month = '0' + month;
    } if (day < 10) {
        day = '0' + day;
    }
    var today = day + '-' + month + '-' + date.getFullYear();
    var now = today.toString();
    return now;
}

var GetCurrentMonth = function () {
    var date = new Date();
    var month = date.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }
    var today = month + '-' + date.getFullYear();
    var mon = today.toString();
    return mon;
}

var GetLastDayFromNow = function (numOfDay) {
    var date = new Date();
    var month = date.getMonth() + 1;
    var day = (date.getDate() == 1 ? date.getDate() : (date.getDate() - numOfDay));
    if (month < 10) {
        month = '0' + month;
    } if (day < 10) {
        day = '0' + day;
    }
    var today = day + '-' + month + '-' + date.getFullYear();
    var now = today.toString();
    return now;
}

var GetFirstDayCurrentDate = function () {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth() + 1;
    var firstDay = new Date(y, m, 1);
    if (m < 10) {
        m = '0' + m;
    }
    firstDay = '01' + '-' + m + '-' + y;
    return firstDay.toString();
}

var GetLastDayCurrentDate = function () {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth() + 1;
    var lastDay = new Date(y, m, 0);
    if (m < 10) {
        m = '0' + m;
    }
    var d = lastDay.getDate();
    var rs = d + '-' + m + '-' + y;
    return rs.toString();
}

var MessageAlert = function (messageType) {    
    if (messageType === 'INFO') {
        //$("#info-alert").fadeTo(120000, 200).slideUp(500, function () {
        //    $("#info-alert").slideUp(500);
        //}); 
        $("#info-alert").show('fast');
        $("#success-alert").hide();
        $("#danger-alert").hide();
    } else if (messageType === 'SUCCESS') {
        //$("#success-alert").fadeTo(120000, 200).slideUp(500, function () {
        //    $("#success-alert").slideUp(500);
        //});
        $("#success-alert").show('fast');
        $("#info-alert").hide();        
        $("#danger-alert").hide();
    } else if (messageType === 'ERROR') {
        //$("#danger-alert").fadeTo(120000, 200).slideUp(500, function () {
        //    $("#danger-alert").slideUp(500);
        //});
        $("#danger-alert").show('fast');
        $("#info-alert").hide();
        $("#success-alert").hide();
        
    }    
}
var HideMessage = function () {
    $("#danger-alert").hide();
    $("#info-alert").hide();
    $("#success-alert").hide();
}