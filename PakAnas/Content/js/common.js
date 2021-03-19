function pad(str, len, pad, dir) {
    if (typeof (len) == "undefined") { var len = 0; }
    if (typeof (pad) == "undefined") { var pad = ' '; }
    if (typeof (dir) == "undefined") { var dir = STR_PAD_RIGHT; }

    if (len + 1 >= str.length) {

        switch (dir) {

            case STR_PAD_LEFT:
                str = Array(len + 1 - str.length).join(pad) + str;
                break;

            case STR_PAD_BOTH:
                var right = Math.ceil((padlen = len - str.length) / 2);
                var left = padlen - right;
                str = Array(left + 1).join(pad) + str + Array(right + 1).join(pad);
                break;

            default:
                str = str + Array(len + 1 - str.length).join(pad);
                break;

        } // switch
    }

    return str;
}

// Message type Message Box Modal [start]
function handleAjaxResult(returnResult, processName, funcSuccess, funcError, isNotShowSuccessMesg, isMesgSuccessNotBlocking, isMesgErrorNotBlocking) {
    if (typeof returnResult.Redirect != "undefined") {
        window.location = returnResult.Redirect;
        return;
    }

    if (typeof returnResult.Result != "undefined" && typeof returnResult.ErrMesgs != "undefined") {
        if (returnResult.Result == returnResult.ValueSuccess) {
            if (!isNotShowSuccessMesg) {
                var mesg = "Process " + processName + " finish successfully";
                if (typeof returnResult.SuccMesgs != "undefined" && returnResult.SuccMesgs != null) {
                    for (i = 0; i < returnResult.SuccMesgs.length; i++) {
                        mesg += "<br/>" + returnResult.SuccMesgs[i];
                    }
                }
                if (typeof returnResult.ProcessId != "undefined" && returnResult.ProcessId != null) {
                    //mesg += "<br/> ProcessId = " + returnResult.ProcessId;
                    mesg += ("<br/> ProcessId = <a href='#' onclick='openNewWindow(\"" + gRootPath + "LogMonitoring?processId=" + returnResult.ProcessId + "\")'>" + returnResult.ProcessId + "</a>");
                }
                $.msgBox({
                    title: "SUCCESS",
                    content: mesg,
                    success: function (result) {
                        if (!isMesgSuccessNotBlocking) {
                            if (funcSuccess && (typeof funcSuccess == "function")) {
                                funcSuccess(returnResult);
                            }
                        }
                    }
                });
            }

            if (isMesgSuccessNotBlocking || isNotShowSuccessMesg) {
                if (funcSuccess && (typeof funcSuccess == "function")) {
                    funcSuccess(returnResult);
                }
            }
        } else {
            if (returnResult.ErrMesgs != null) {
                var errMesg = "Process " + processName + " finish with error : <br/>";
                if (typeof returnResult.ProcessId != "undefined" && returnResult.ProcessId != null) {
                    //errMesg += "ProcessId = " + returnResult.ProcessId + "<br/>";
                    errMesg += ("ProcessId = <a href='#' onclick='openNewWindow(\"" + gRootPath + "LogMonitoring?processId=" + returnResult.ProcessId + "\")'>" + returnResult.ProcessId + "</a><br/>");
                }
                for (i = 0; i < returnResult.ErrMesgs.length; i++) {
                    errMesg += returnResult.ErrMesgs[i] + "<br/>";
                }
                $.msgBox({
                    title: "ERROR",
                    type: "error",
                    content: errMesg,
                    success: function (result) {
                        if (!isMesgErrorNotBlocking) {
                            if (funcError && (typeof funcError == "function")) {
                                funcError(returnResult);
                            }
                        }
                    }
                });
            }

            if (isMesgErrorNotBlocking) {
                if (funcError && (typeof funcError == "function")) {
                    funcError(returnResult);
                }
            }
        }
    } else {
        alert(returnResult);
        window.location = window.location.origin;
    }
}

function handleAjaxResponseError(returnResult, processName, funcError, isMesgErrorNotBlocking) {
    $.msgBox({
        title: "ERROR",
        //type: "error",
        content: "Process " + processName + " finish with error : <br/> Ajax Request Error : <br/> " + JSON.stringify(returnResult),
        success: function (result) {
            if (!isMesgErrorNotBlocking) {
                if (funcError && (typeof funcError == "function")) {
                    funcError(returnResult);
                }
            }
        }
    });

    if (isMesgErrorNotBlocking) {
        if (funcError && (typeof funcError == "function")) {
            funcError(result);
        }
    }
}

function showSuccessMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "SUCCESS",
        content: mesg,
        type: "info",
        success: function (result) {
            if (funcSuccess && (typeof funcSuccess == "function")) {
                funcSuccess(result);
            }
        }
    });
}

function showErrorMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "ERROR",
        content: mesg,
        type: "error",
        success: function (result) {
            if (funcSuccess && (typeof funcSuccess == "function")) {
                funcSuccess(result);
            }
        }
    });
}

function showInfoMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "INFORMATION",
        content: mesg,
        type: "info",
        success: function (result) {
            if (funcSuccess && (typeof funcSuccess == "function")) {
                funcSuccess(result);
            }
        }
    });
}

function showConfirmMesg(mesg, funcSuccess, title) {
    $.msgBox({
        title: title ? title : "Are You Sure",
        content: mesg,
        type: "confirm",
        buttons: [{ value: "Yes" }, { value: "No"}],
        success: function (result) {
            if (result == "Yes") {
                if (funcSuccess && (typeof funcSuccess == "function")) {
                    funcSuccess(result);
                }
            }
        }
    });
}
// Message type Message Box Modal [end]

function handleDownloadResponseError(responseHtml, url) {
    window.location = window.location.origin;
}

function popUpProgressShow() {
    $.blockUI({
        message: '</br></br><div style="font-weight: bold;font-size: 110%;font-family: Sans-Serif;">Loading</div><div style="font-weight: bold;font-size: 110%;font-family: Sans-Serif;">Please wait...</div></br></br><img src="' + gRootPath + 'Content/Images/loading.gif" /></br></br>',
        css: {
            padding: 0,
            margin: 0,
            width: '22%',
            top: '33%',
            left: '39%',
            border: '1px solid black',
            backgroundColor: '#FFFFFF',
            cursor: 'wait'
        },
        overlayCSS: {
            backgroundColor: '#CCCCCC',
            opacity: 0.6,
            cursor: 'wait'
        },
        baseZ: 99990
    });
}

function popUpProgressHide() {
    $.unblockUI();
}

// Message type Growl [start]
function handleAjaxResultGrowl(returnResult, processName, funcSuccess, funcError, isNotShowSuccessMesg) {
    if (typeof returnResult.Redirect != "undefined") {
        window.location = returnResult.Redirect;
        return;
    }

    if (typeof returnResult.Result != "undefined" && typeof returnResult.ErrMesgs != "undefined") {
        if (returnResult.Result == returnResult.ValueSuccess) {
            if (!isNotShowSuccessMesg) {
                var mesg = "Process " + processName + " finish successfully";
                if (typeof returnResult.SuccMesgs != "undefined" && returnResult.SuccMesgs != null) {
                    for (i = 0; i < returnResult.SuccMesgs.length; i++) {
                        mesg += "<br/>" + returnResult.SuccMesgs[i];
                    }
                }
                if (typeof returnResult.ProcessId != "undefined" && returnResult.ProcessId != null) {
                    //mesg += "<br/> ProcessId = " + returnResult.ProcessId;
                    mesg += ("<br/> ProcessId = <a href='#' onclick='openNewWindow(\"" + gRootPath + "LogMonitoring?processId=" + returnResult.ProcessId + "\")'>" + returnResult.ProcessId + "</a>");
                }

                showSuccessMesgGrowl(mesg);
            }

            if (funcSuccess && (typeof funcSuccess == "function")) {
                funcSuccess(returnResult);
            }
        } else {
            if (returnResult.ErrMesgs != null) {
                var errMesg = "Process " + processName + " finish with error : <br/>";
                if (typeof returnResult.ProcessId != "undefined" && returnResult.ProcessId != null) {
                    //errMesg += "ProcessId = " + returnResult.ProcessId + "<br/>";
                    errMesg += ("ProcessId = <a href='#' onclick='openNewWindow(\"" + gRootPath + "LogMonitoring?processId=" + returnResult.ProcessId + "\")'>" + returnResult.ProcessId + "</a><br/>");
                }
                for (i = 0; i < returnResult.ErrMesgs.length; i++) {
                    errMesg += returnResult.ErrMesgs[i] + "<br/>";
                }

                showErrorMesgGrowl(errMesg);
            }

            if (funcError && (typeof funcError == "function")) {
                funcError(returnResult);
            }
        }
    } else {
        alert(returnResult);
        window.location = window.location.origin;
    }
}

function handleAjaxResponseErrorGrowl(returnResult, processName, funcError) {
    var errMesg = "Process " + processName + " finish with error : <br/> Ajax Request Error : <br/> " + JSON.stringify(returnResult);

    showErrorMesgGrowl(errMesg);

    if (funcError && (typeof funcError == "function")) {
        funcError(result);
    }
}

function showSuccessMesgGrowl(mesg) {
    $.bootstrapGrowl(mesg, {
        type: 'success',
        allow_dismiss: true,
        align: 'center',
        delay: 10000
    });
}

function showErrorMesgGrowl(mesg) {
    $.bootstrapGrowl(mesg, {
        type: 'danger',
        allow_dismiss: true,
        align: 'center',
        delay: 10000
    });
}

function showInfoMesgGrowl(mesg) {
    $.bootstrapGrowl(mesg, {
        type: 'info',
        allow_dismiss: true,
        align: 'center', 
        delay: 10000
    });
}

/*
Growl Options
$.bootstrapGrowl("another message, yay!", {
ele: 'body', // which element to append to
type: 'info', // (null, 'info', 'danger', 'success')
offset: {from: 'top', amount: 20}, // 'top', or 'bottom'
align: 'right', // ('left', 'right', or 'center')
width: 250, // (integer, or 'auto')
delay: 4000, // Time while the message will be displayed. It's not equivalent to the *demo* timeOut!
allow_dismiss: true, // If true then will display a cross to close the popup.
stackup_spacing: 10 // spacing between consecutively stacked growls.
});
*/
// Message type Growl [end]

function checkNumeric(evt, obj, bilBulat) {
    keyCode = null;
    if (evt.which) {
        keyCode = evt.which;
    } else if (evt.keyCode) {
        if (evt.charCode == 0)
            return true;
        else
            keyCode = evt.keyCode;
    }

    if (bilBulat == true) {
        if ((48 > keyCode || keyCode > 57) && keyCode != 8)
            return false;

    } else {
        if ((48 > keyCode || keyCode > 57) && keyCode != 8
                    && keyCode != 44 && keyCode != 46)
            return false;
    }
    oldValue = obj.value;
    return true;
}

function removeCommas(str) {
    return (str.replace(/,/g, ''));
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function openNewWindow(urlString) {
    window.open(urlString);
}
