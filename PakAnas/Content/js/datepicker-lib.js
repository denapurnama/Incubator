$(document).ready(function () {

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
