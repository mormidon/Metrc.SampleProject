// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


(function() {
    //'use strict';

    let root = window,
        metrc = root.metrc = root.metrc || {};
    metrc.$ = metrc.$ || root.jQuery;
    metrc.$.fn = metrc.$.fn || {};
    metrc.kendo = metrc.kendo || {};

    // ========================================
    // startSpinner : Shows a "loading" spinner
    // ========================================

    metrc.startSpinner = metrc.startSpinner || function () {
        let $body = metrc.$(document.body),
            $data = $body.data();

        // try/catch in case there is a browser that doesn't support blur(). Remove try/catch after 2018-01-01.
        // JN: Commented out try/catch on 2018-08-21. Restore if problems found.
        //try {
        document.activeElement.blur();
        //} catch (e) { }

        if (typeof $data.spinner !== 'undefined') {
            return false;
        }

        if ($body.children('#spinnerBackground').length === 0) {
            metrc.$('<div id="spinnerBackground">')
                .css({
                    backgroundColor: '#000',
                    position: 'fixed',
                    top: 0,
                    bottom: 0,
                    left: 0,
                    right: 0,
                    opacity: 0.6,
                    zIndex: 20000
                })
                .appendTo($body);
        }

        let options = {
            lines: 11, // The number of lines to draw
            length: 30, // The length of each line
            width: 11, // The line thickness
            radius: 30, // The radius of the inner circle
            corners: 1, // Corner roundness (0..1)
            rotate: 0, // The rotation offset
            direction: 1, // 1: clockwise, -1: counterclockwise
            color: '#FFF', // #rgb or #rrggbb or array of colors
            speed: 1, // Rounds per second
            trail: 60, // Afterglow percentage
            shadow: true, // Whether to render a shadow
            hwaccel: true, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: 'auto', // Top position relative to parent in px
            left: 'auto' // Left position relative to parent in px
        };
        $data.spinner = new Spinner(options).spin($body.get(0));

        return true;
    };

    // ===============================
    // stopSpinner : Stops the spinner
    // ===============================

    metrc.stopSpinner = metrc.stopSpinner || function () {
        let $body = metrc.$(document.body),
            $spinnerBackground = $body.children('#spinnerBackground'),
            $data = $body.data();

        if (typeof $data.spinner !== 'undefined') {
            $data.spinner.stop();
            delete $data.spinner;
        }
        if ($spinnerBackground.length !== 0) {
            $spinnerBackground.remove();
        }
    };

    // =================================================================
    // serverErrors : Opens an error alert with useful error information
    // =================================================================

    metrc.serverErrors = metrc.serverErrors || function (errorResponse) {
        let alertTemplate =
            '<div class="alert alert-error">' +
            '<a class="close" data-dismiss="alert">×</a>' +
            '<p>{{message}} {{exceptionTypeTemplate}}</p>' +
            '{{exceptionMessageTemplate}}' +
            '{{stackTraceTemplate}}' +
            '</div>',
            exceptionTypeTemplate =
                '({{exceptionType}})',
            exceptionMessageTemplate =
                '<p>{{exceptionMessage}}</p>',
            stackTraceTemplate =
                '<ul class="js-panelbar-new">' +
                '<li>' +
                '<span class="muted"><small>(view details)</small></span>' +
                '<div><pre style="max-height: 400px; overflow-y: scroll;">{{stackTrace}}</pre></div>' +
                '</li>' +
                '</ul>',

            message = typeof errorResponse === 'string' ? errorResponse : errorResponse.Message || errorResponse.responseText,
            exceptionType = errorResponse.ExceptionType || errorResponse.ClassName || null,
            exceptionMessage = errorResponse.ExceptionMessage || null,
            stackTrace = errorResponse.StackTrace || errorResponse.RemoteStackTraceString || errorResponse.StackTraceString || null,

            result = alertTemplate
                .replace('{{message}}', message || 'An error has occurred (' + (new Date()).toString() + ')')
                .replace('{{exceptionTypeTemplate}}', exceptionType ? exceptionTypeTemplate.replace('{{exceptionType}}', exceptionType) : '')
                .replace('{{exceptionMessageTemplate}}', exceptionMessage ? exceptionMessageTemplate.replace('{{exceptionMessage}}', exceptionMessage) : '')
                .replace('{{stackTraceTemplate}}', stackTrace ? stackTraceTemplate.replace('{{stackTrace}}', stackTrace) : '');

        metrc.$('#user-alerts').append(result);
        metrc.$('.js-panelbar-new')
            .removeClass('js-panelbar-new')
            .addClass('js-panelbar')
            .kendoPanelBar();
    };

    // ==============================================================================
    // messageAlerts : Opens an alert with a message, and optionally, a type of alert
    // ==============================================================================

    metrc.messageAlerts = metrc.messageAlerts || function (message, type) {
        if (!type) type = 'info';

        let template =
            '<div class="alert alert-{{type}}">' +
                '<a class="close" data-dismiss="alert">×</a>' +
                '<p>{{message}}</p>' +
                '</div>';

        let result = template
            .replace('{{type}}', type)
            .replace('{{message}}', message);

        metrc.$('#user-alerts').append(result);
    };

    // ==================================================================================
    // errorResponseHandler : TODO Description
    // ==================================================================================

    metrc.errorResponseHandler = metrc.errorResponseHandler || function (response) {
        let errorResponse = response.responseJSON || response;

        console.dir(errorResponse);
        switch (response.status) {
        case 0:
            // If the error is 0, ignore it...
            break;
        case 400:
        case 500:
            metrc.serverErrors(errorResponse);
            break;
        case 401:
            // If the session has expired, reload the window and the user will be redirected to the log-in page
            location.reload();
            break;
        default:
            metrc.messageAlerts(response.statusText, 'error');
        }
    };

    // =======================================================
    // submitData : Helper for submitting data to the server
    // -------------------------------------------------------
    // Options:
    //  - url: the URL
    //  - method: HTTP method to use (default: 'POST')
    //  - data: data to pass on along with the request
    //  - dataType: data type name
    //  - contentType: content type name of data going across
    //  - success: event handler to call on success
    //  - error: event handler to call on error
    //  - always: event handler to always call, whether successful or not
    //  - useSpinner: boolean whether to use the UI spinner (default: true)
    // =======================================================

    metrc.submitData = metrc.submitData || function (options) {
        options = metrc.$.extend(true, {
            url: '',
            method: 'POST',
            success: function () { },
            error: metrc.errorResponseHandler,
            useSpinner: true
        }, options || {});


        let usingSpinner = options.useSpinner ? metrc.startSpinner() : false;
        metrc.$.ajax({
                contentType: options.contentType,
                data: options.data,
                dataType: options.dataType,
                url: options.url,
                type: options.method
            })
            .done(options.success)
            .fail(options.error)
            .always(function () {
                if (typeof options.always === 'function') {
                    options.always();
                }
                if (usingSpinner) {
                    metrc.stopSpinner();
                }
            });
    };

    // =================================================================
    // submitJson : Helper for submitting JSON to the server
    // -----------------------------------------------------------------
    // Options: same options as submitData, except the following:
    //  - data: anything in "data" will be stringified (JSON.stringify)
    //  - dataType: set to "json"
    //  - contentType: set to "application/json"
    // =================================================================

    metrc.submitJson = metrc.submitJson || function (options) {
        options = metrc.$.extend(true, options || {}, {
            contentType: 'application/json',
            dataType: 'json'
        });
        if (typeof options.data !== 'undefined' && options.data !== null && typeof options.data !== 'string') {
            options.data = JSON.stringify(options.data);
        }

        metrc.submitData(options);
    };

    // =========================================
    // kendo.clientDataSource : TODO Description
    // ----------------------------------------------------------------------
    // Options:
    //  - data : DataSource data
    //  - model : DataSource schema model
    //  - pageSize : number of rows to display
    //  - sort : default sort to apply
    //  - dataChangedHandler : function to handle data change events
    // =========================================

    metrc.kendo.clientDataSource = metrc.kendo.clientDataSource || function (options) {
        options = options || {};

        return new kendo.data.DataSource({
            data: options.data || [],
            schema: {
                model: options.model,
                type: 'json'
            },
            change: options.dataChangedHandler,
            pageSize: options.pageSize || 20,
            sort: options.sort
        });
    };

    // ======================================================================
    // kendo.serverDataSource :
    // ----------------------------------------------------------------------
    // Options:
    //  - url : the URL
    //  - data : data to pass on along with the request
    //  - model : DataSource schema model
    //  - pageSize : number of rows to display
    //  - sort : default sort to apply
    //  - serverAggregates : submit aggregates to be calculated by the server
    //  - serverFiltering : submit filters to the server
    //  - serverGrouping : submit grouping requests to the server
    //  - serverPaging : perform row paging on the server
    //  - serverSorting : perform columng sorting on the server
    // ======================================================================

    metrc.kendo.serverDataSource = metrc.kendo.serverDataSource || function(options) {
        options = options || {};

        if (!options.pageSize || options.pageSize < 0) options.pageSize = 20;
        if (typeof options.serverAggregates === 'undefined' || options.serverAggregates === null) options.serverAggregates = true;
        if (typeof options.serverFiltering === 'undefined' || options.serverFiltering === null) options.serverFiltering = true;
        if (typeof options.serverGrouping === 'undefined' || options.serverGrouping === null) options.serverGrouping = true;
        if (typeof options.serverPaging === 'undefined' || options.serverPaging === null) options.serverPaging = true;
        if (typeof options.serverSorting === 'undefined' || options.serverSorting === null) options.serverSorting = true;

        return new kendo.data.DataSource({
            transport: {
                read: function (readOptions) {
                    let optionsData;
                    if (typeof options.data === 'function') {
                        optionsData = options.data();
                    } else {
                        optionsData = options.data;
                    }
                    metrc.submitJson({
                        url: options.url || '',
                        method: 'GET',
                        data: { data: optionsData, request: readOptions.data },
                        success: function (result) {
                            readOptions.success(result);
                        },
                        useSpinner: false
                    });
                }
            },
            schema: {
                model: options.model || { fields: {} },
                type: 'json',
                data: 'Data',
                errors: 'Errors',
                total: 'Total'
            },
            autoSync: false,
            batch: false,
            pageSize: options.pageSize,
            sort: options.sort,
            serverAggregates: options.serverAggregates,
            serverFiltering: options.serverFiltering,
            serverGrouping: options.serverGrouping,
            serverPaging: options.serverPaging,
            serverSorting: options.serverSorting
        });
    };

    // ==================================================================================
    // gridModal : Opens a modal window with a form and reloads the grid after submission
    // ----------------------------------------------------------------------------------
    //  - title : The title of the modal
    //  - formUrl : The URL of the form
    //  - formQuery : Query parameters for the form URL
    //  - formSelector : Selector for the form
    //  - submitUrl : The URL to submit to via AJAX
    //  - $grid : jQuery of grid, or array of grids, to be refreshed after closing the modal
    // ==================================================================================

    metrc.kendo.modalWindow = metrc.kendo.modalWindow || function (title, formUrl, formQuery, formSelector, submitUrl, $grid, success, openHandler, requestType) {
        if (metrc.kendo.modalWindowOpened) return null;
        metrc.kendo.modalWindowOpened = true;

        title = title || '';
        formUrl = formUrl || '';
        formQuery = formQuery || {};
        formSelector = formSelector || 'form';
        submitUrl = submitUrl || '';
        success = success || function () { };
        requestType = requestType || 'GET';

        let usingSpinner = metrc.startSpinner(),
            $element = metrc.$(document.createElement('div')),
            destroyModalWindow = function () {
                setTimeout(function () { $element.data('kendoWindow').destroy(); }, 1000);
                delete metrc.kendo.modalWindowOpened;
            };

        $element.kendoWindow({
            // config
            actions: ['Close'],
            draggable: true,
            modal: true,
            resizable: false,
            title: title,
            // events
            open: openHandler,
            activate: function () {
                if (usingSpinner) {
                    metrc.stopSpinner();
                }

                metrc.$(formSelector)
                    .find('input[type=text]:not([readonly]),select,textarea:not([readonly])')
                    .filter(':visible:enabled:first')
                    .focus(); // Set focus to first enabled and visible input in form
            },
            refresh: function () {
                let me = this;

                $element.find(formSelector).metrcActivateAjaxForm({
                    url: submitUrl,
                    type: 'POST',
                    success: function (e) {
                        me.close();
                        if ($grid instanceof Array) {
                            for (let i = 0, icnt = $grid.length; i < icnt; i++) {
                                $grid[i].data('kendoGrid').dataSource.read();
                            }
                        }
                        else if ($grid) {
                            $grid.data('kendoGrid').dataSource.read();
                        }
                        success(e);
                    }
                });
                $element.find('#cancel').click(function () { me.close(); });

                me.center();
                me.open();
            },
            close: destroyModalWindow,
            error: function (e) {
                if (usingSpinner) {
                    metrc.stopSpinner();
                }

                destroyModalWindow();
                metrc.errorResponseHandler(e.xhr);
            }
        });

        let kWindow = $element.data('kendoWindow');
        kWindow.refresh({
            url: formUrl,
            data: formQuery,
            traditional: true,
            type: requestType
        });

        return $element;
    };

    // ====================================================================================
    // $.metrcActivateAjaxForm : Sets up validation and AJAX-style submission for a form
    // ------------------------------------------------------------------------------------
    // Options : Options hash passed to $.ajax
    // ====================================================================================

    metrc.$.fn.metrcActivateAjaxForm = metrc.$.fn.metrcActivateAjaxForm || function (options) {
        // Fail if nothing is selected.
        if (!this.length) {
            console.log('metrcActivateAjaxForm: No element selected.');
            return this;
        }

        let ajaxOptions = metrc.$.extend(true, {
            error: metrc.errorResponseHandler
        }, options);

        this.each(function () {
            let $this = metrc.$(this);
            $this.submit(function () {
                $this.metrcAjaxSubmit(ajaxOptions);
                return false;
            });
        });

        return this;
    };


    // ===================================================
    // $.metrcAjaxSubmit : Submits a form via AJAX
    // ---------------------------------------------------
    // Options: same options as submitJson, except the following:
    //  - url: if not specified, will be taken from the form's action attribute
    //  - method: if not specified, will be taken from the form's method attribute
    // ===================================================

    metrc.$.fn.metrcAjaxSubmit = metrc.$.fn.metrcAjaxSubmit || function (options) {
        // Fail if nothing is selected.
        if (!this.length) {
            window.log('metrcAjaxSubmit: No element selected.');
            return this;
        }

        let $form = this,

            // Grab the URL from the form element.
            action = $form.attr('action'),
            url = options.url || typeof action === 'string'
                ? metrc.$.trim(action)
                : window.location.href || '',
            method = options.method || $form.attr('method') || 'POST',

            // Serialize the form values
            data = $form.serializeObject().model,

            // Build the final options hash.
            ajaxOptions = metrc.$.extend(true, {
                url: url,
                type: method,
                data: data
            }, options);

        // Finally, submit the data.
        metrc.submitJson(ajaxOptions);
    };

    // ==================================
    // metrcGridIds : TODO Description
    // ==================================

    metrc.$.fn.metrcGridIds = metrc.$.fn.metrcGridIds || function (idField) {
        let ids = [],
            kendoGrid = metrc.$(this).data('kendoGrid');

        idField = idField || 'Id';
        kendoGrid.select().each(function () {
            let item = kendoGrid.dataItem(metrc.$(this));
            ids.push(item[idField]);
        });

        return ids;
    };
})();