$(document).ready(function () {

    /** start - event for tree grid with table grid **/
    initTableToggle();
    $(".rowtoggle").click(function () {
        $(this).toggleClass("rowexpand", "");
        $(this).toggleClass("rowcollapse", "");

        var p1 = $(this).parentsUntil("tr");
        var p2 = $(p1).parent()
        $(p2).toggleClass("tb-collapse", "");
        $(p2).toggleClass("tb-expand", "");
        setTableToogle(p2);
    });
    /** end - event for tree grid with table grid **/


    /** start - check all checkbox **/
    $("#checkall").click(function () {
        $(".check").prop("checked", $("#checkall").is(":checked"));
    });
    /** end - check all checkbox **/

    /** use class _datepicker to generate datepicker on input text element **/
    $("._datepicker").datepicker({
        autoclose: true,
        format: 'dd-mm-yyyy'
    });

    /** use class _monthpicker to generate month picker on input text element **/
    $("._monthpicker").datepicker({
        autoclose: true,
        format: 'mm-yyyy',
        minViewMode: 'months'
    });

    /** use class _yearpicker to generate year picker on input text element **/
    $("._yearpicker").datepicker({
        autoclose: true,
        format: 'yyyy',
        minViewMode: 'years'
    });
});


/** initialize table tree **/
function initTableToggle() {
    $(".tb-expand td:first-child").prepend('<span class="rowtoggle rowexpand"><i class="fa fa-minus-square-o fa-fw"></i></span> ');
    $(".tb-collapse td:first-child").prepend('<span class="rowtoggle rowcollapse"><i class="fa fa-plus-square-o fa-fw"></i></span> ');

    var header = $(".tb-collapse");
    header.each(function () {
        $(this).next().toggleClass("hide", "");
    });

}

/** set table tree toggle action 
param: object parent**/
function setTableToogle(p) {
    $(".rowexpand").html('<i class="fa fa-minus-square-o fa-fw"></i> ');
    $(".rowcollapse").html('<i class="fa fa-plus-square-o fa-fw"></i> ');

    $(p).next().toggleClass("hide", "");
}

/** action table tree fo toggle all 
param: 1 = collapse; 0 = expand; **/
function tableToggleAll(aToggle) {

    if (aToggle == 1) {
        $("tr.tb-expand").next().toggleClass("hide", "");
        $("tr.tb-expand").addClass("tb-collapse");
        $("tr.tb-expand").removeClass("tb-expand");

        $(".rowtoggle.rowexpand").toggleClass("rowcollapse", "");
        $(".rowtoggle.rowcollapse").removeClass("rowexpand");
    } else {
        $("tr.tb-collapse").next().toggleClass("hide", "");
        $("tr.tb-collapse").addClass("tb-expand");
        $("tr.tb-collapse").removeClass("tb-collapse");

        $(".rowtoggle.rowcollapse").toggleClass("rowexpand", "");
        $(".rowtoggle.rowexpand").removeClass("rowcollapse");
    }

    $(".rowexpand").html('<i class="fa fa-minus-square-o fa-fw"></i> ');
    $(".rowcollapse").html('<i class="fa fa-plus-square-o fa-fw"></i> ');
}

/**
* desc : Number only format that trigger on pressing keyboard
* 
* author  Argyaputri
* date    17 Nov 2014
* param   {Event} evt
* use     onkeypress="javascript: isNumberOnly(event);"
*/
function isNumberOnly(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

/**
* desc : Number format from pressing keyboard
*          (,) is separator, (.) is decimal
* 
* author  Argyaputri
* date    01 March 2010 5:55 PM
* param   {Object} obj
* use     onkeyup="javascript: numberFormat(this);"
*/
function numberFormat(obj) {
    var a = obj.value;
    var b = a.replace(/[^\d+\..]/g, "");
    var dtNumber = b.split('.');
    var numberInt = dtNumber[0];
    var numberVal = "";
    var intLength = numberInt.length;
    var j = 0;

    if (dtNumber.length == 1) {
        numberDec = "";
    } else {
        numberDec = ".";
    }

    for (n = 1; n < dtNumber.length; n++) {
        numberDec += dtNumber[n];
    }

    for (i = intLength; i > 0; i--) {
        j = j + 1;
        if (((j % 3) == 1) && (j != 1)) {
            numberVal = numberInt.substr(i - 1, 1) + "," + numberVal;
        } else {
            numberVal = numberInt.substr(i - 1, 1) + numberVal;
        }
    }

    obj.value = numberVal + numberDec;
}

//Preloader table function
function showLoading(page) {
    var preloader = page.find("#preloader")
    var wait_div = page.find("#wait")
    preloader.show();
    wait_div.show();
}

function hideLoading(page) {
    var preloader = page.find("#preloader")
    var wait_div = page.find("#wait")
    preloader.hide();
    wait_div.hide();
}
