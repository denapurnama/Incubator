;(function ($) {
    $tdk.fnTypeahead_StringFilter = function(strings) {
        return function findMatches(key, cb) {
            var matches = [];
            var keyRegex = new RegExp(key, 'i');
            $.each(strings, function(index, str) {
                if(keyRegex.test(str)) {
                    matches.push({ value: str });
                }
            });

            cb(matches);
        };
    };

    $.fn.tdkComboBox = function (options) {
        var settings = $.extend(true, $.fn.tdkComboBox.defaults, options);
        return this.each(function () {
            $parent = $(this);
            $toggle = $parent.children(".dropdown-toggle").first();
            var dropdown = $parent.children("ul.dropdown-menu").find("a").each(function () {
                $anchor = $(this);
                $anchor.data("value", $anchor.attr("id"));
                $anchor.removeAttr("id");
                $anchor.click(function (e) {
                    e.preventDefault();
                    $this = $(this);
                    $toggle.data("value", $this.data("value"));
                    $toggle.text($this.text() + " ");
                    $toggle.append($("<span/>", { addClass: "caret" }));
                    settings.onOptionClicked.call(this);
                });
            });
        });
    };
    $.fn.tdkComboBox.defaults = {
        onOptionClicked: function () { alert($(this).data("value")); }
    };
    $.fn.tdkComboBox.getSelectedValue = function () {
        $toggle = $(this).children(".dropdown-toggle").first();
        return $toggle.data("value");
    };

    $.fn.tdkDataGrid = function (options) {
        var settings = $.extend(true, $.fn.tdkDataGrid.defaults, options);
        return this.each(function () {
            var container = $(this);
            var table = container.find("table").first();
            var btDeleteRows = container.find("button.ui-grid-delete-rows").first();
            var rowSelectionChecks = container.find("table td.select-column input[type=checkbox]");
            var selectAllRowCheck = container.find("table th.select-column input[type=checkbox]").first(); 
            var downloadDialog = container.find("div.modal.dialog-download").first();           
            var uploadDialog = container.find("div.modal.dialog-upload").first();           

            container.find("button.ui-grid-add").click(function() {                
                settings.OnAddButtonClicked.call();
            });
            rowSelectionChecks.change(function() {
                var moreThenOneChecked = container.find("table td.select-column input:checked").size() > 0;
                btDeleteRows.prop("disabled", !moreThenOneChecked);
                selectAllRowCheck.prop("checked", !moreThenOneChecked);
            });
            selectAllRowCheck.change(function() {
                var checked = $(this).prop("checked");
                rowSelectionChecks.prop("checked", checked);
                btDeleteRows.prop("disabled", !checked);
            });

            container.find("button.ui-grid-download").click(function() {
                downloadDialog.modal();
            });
            container.find("button.ui-grid-upload").click(function() {
                uploadDialog.modal();
            });

            var btUploadDialogBrowse = uploadDialog.find("span.dialog-upload-browse input:file").first();
            var uploadDialogInfo = uploadDialog.find("span.info").first();
            uploadDialogInfo.text("No file selected.");            
            var uploadDialogProgress = uploadDialog.find("div.progress").first();
            var btUploadDialogInit = uploadDialog.find(".dialog-upload-initiate").first();
            btUploadDialogBrowse.change(function() {                                
                var input = $(this);
                var path = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.data("path", path);
                uploadDialogInfo.html("Ready to upload: <em>" + path + "</em>");
                input.val("");                                
            });
            btUploadDialogInit.click(function() {
                var path = btUploadDialogBrowse.data("path");
                uploadDialogInfo.html("Uploading: <em>" + path + "</em>");                
                uploadDialogProgress.fadeIn("fast");
                setTimeout(function() {                    
                    uploadDialogProgress.fadeOut("fast");                    
                    uploadDialogInfo.text("File uploaded, please select another file to upload.");
                }, 2000);
            });

            var confirmDeleteDialog = container.find("div.modal.dialog-delete-confirm").first();  
            container.find("button.ui-grid-delete-rows").click(function() {
                confirmDeleteDialog.modal();
            });
        });
    };
    $.fn.tdkDataGrid.defaults = {
        pageNumber: 1,
        pageSize: 10,
        enableFilterRow: true,
        OnAddButtonClicked: function() { }
    };
})(jQuery);