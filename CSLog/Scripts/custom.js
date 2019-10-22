function showAjaxLoading() {
    $("#AjaxLoading").css('display', 'flex');
}
function hideAjaxLoading() {
    $("#AjaxLoading").css('display', 'none');
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
function isPhoneNumber(phone) {
    var regex = /^[0]\\d{10}$/;
    return regex.test(phone);
}

function InlineErrorMessage(msg, id) {
    if (id === undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-danger "><a href="#" class="close" data-dismiss="alert" aria-label="close">×</a><strong>' + msg + '</strong></div>';
        $("#" + id).html(html);
    }
}

function isJson(item) {
    item = typeof item !== "string"
        ? JSON.stringify(item)
        : item;

    try {
        item = JSON.parse(item);
    } catch (e) {
        return false;
    }

    if (typeof item === "object" && item !== null) {
        return true;
    }

    return false;
}

var start = moment()
var end = moment();
function cb(start, end) {
    //alert(start);
    $('#sDate').val(start.format('DD/MM/YYYY'));
    $('#eDate').val(end.format('DD/MM/YYYY'));
    $('#reportrange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
}


var dalWiz = (function () {
    var titleStore = "#titleStore";
    var pagecontainer = "#page-container";
    var setActive = function () {
        var clickedElement = $(this);
        var activeElement = clickedElement.siblings("a.getr.active").first();
        activeElement.removeClass("active");
        clickedElement.addClass("active");
    };
    var updateUI = function (data, pageUrl, phost, IsPop) {
        var elem = this;
        if (!phost) phost = pagecontainer;
        $(phost).html(data);
        if (IsPop === false && pageUrl !== window.location) {
            var pageTitle = $(titleStore).val();
            document.title = pageTitle;
            var modUrl = pageUrl.replace("?r=1", "").replace("&r=1", "");
            window.history.pushState({ pageUrl: modUrl }, pageTitle, modUrl);
        }
        setActive.call(elem);
        hideAjaxLoading();
    };
    var updatePanel = function (data, phost) {
        if (!phost) phost = pagecontainer;
        $(phost).html(data);
        hideAjaxLoading();
    };
    return {
        getPage: function (IsPop) {
            var elem = this;
            var pageUrl = $(this).attr("href");
            var phost = $(this).attr("data-host");
            if (!pageUrl) pageUrl = $(this).attr("data-url");
            if (pageUrl.indexOf("#") === -1) {
                if (pageUrl.indexOf("?") === -1) pageUrl += "?r=1"; else pageUrl += "&r=1";
                return $.ajax({
                    url: pageUrl,
                    method: "GET"
                }).fail(function (data) {
                    //alert(data.statusText);
                    hideAjaxLoading();
                    updatePanel(data.responseText, phost);
                }).done(function (data) {
                    updateUI.call(elem, data, pageUrl, phost, IsPop);
                    $('.dataTable').DataTable();
                    $('#reportrange').daterangepicker({
                        startDate: start,
                        endDate: end,
                        opens: 'left',
                        autoApply: true,
                        ranges: {
                            'Today': [moment(), moment()],
                            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                            'This Month': [moment().startOf('month'), moment().endOf('month')],
                            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                        }
                    }, cb);
                    cb(start, end);
                });
            }
        },
        postData: function (IsPop) {
            var $elem = $(this);
            var frm = $elem.closest("form");
            var params = frm.serialize();
            var pageUrl = frm.attr("action");
            var phost = $elem.attr("data-host");
            if (!pageUrl) pageUrl = $elem.attr("data-url");
            if (pageUrl.indexOf("?") === -1) pageUrl += "?r=1"; else pageUrl += "&r=1";
            return $.ajax({
                url: pageUrl,
                data: params,
                method: "POST"
            }).fail(function (data) {
                //alert(data.statusText);
                $('#dvModal').modal('hide');
                hideAjaxLoading();
                updatePanel(data.responseText, phost);
                
            }).done(function (data) {
                if (isJson(data)) {
                    InlineErrorMessage(data.Error ? data.Error : "Unknow error occured. Please try again later!", "dvError");
                }
                else {
                    updatePanel(data, phost);
                    $('#dvModal').modal('hide');
                }
                hideAjaxLoading();
                $('.dataTable').DataTable();
                //$('#dvModal').modal('hide');
            });
        }
    };
})();

$(document).ready(function () {

    $(document.body).on('keypress', ".digits", function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $.ajaxSetup({
        beforesend: function (request) {
            request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        }
    });

    $(window).on("popstate", function () {
        var st = event.state;
        if (!st) return false;
        var elem = document.querySelector("a.getr[href='" + st.pageUrl + "']");
        if (!elem) return false;
        dalWiz.getPage.call(elem, true);
    });

    $('.dataTable').DataTable({
        //searching: false
        //paging: false,
        //info: false
    });

    $(document).on("click", ".getr", function (e) {
        e.preventDefault();
        showAjaxLoading();
        dalWiz.getPage.call(this, false);
    });
    $(document).on("click", ".postr", function (e) {
        e.preventDefault();
        showAjaxLoading();
        dalWiz.postData.call(this, false);
    });
    

    $(document).on("click", "a[data-modal]", function (e) {
        showAjaxLoading();
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        $('#modalContent').load(this.href, function () {
            $('#dvModal').modal('show', {
                backdrop: 'static',
                keyboard: false,
            });
            hideAjaxLoading();
        });

        return false;
    });


    //$(document).on("focusin", ".date", function () {
    //    $(this).datepicker({
    //        format: 'dd/mm/yyyy',
    //        autoclose: true
    //    });
    //});

    $(document).on("focus", ".date", function (e) {
        $(this).datepicker({
            format: "dd/mm/yyyy",
            daysOfWeekHighlighted: "0",
            autoclose: true,
            todayHighlight: true,
            changeMonth: true
        });
    });


    $(document).on("click", ".btn-T", function () {
        var mclass = $(this).attr("data-class");
        //$(".dropdown-con").hide();
        $("." + mclass).toggle();
    });

    $(document).on("click", ".tr-show", function () {
        var mclass = $(this).attr("data-showTR");
        $("." + mclass).toggle();
    });

    $(document).on("focusout", ".phone", function () {
        var phone = $(this).val();
        if (!isPhoneNumber(phone)) {
            $(".phoneValidation").text("Invalid Phone number");
        }
    });

    $(document).on("focusout", ".email", function () {
        var email = $(this).val();
        if (!isEmail(email)) {
            if (email !== "") {
                $(".emailValidation").text("Invalid email address");
            }
        }
    });

    $(document).on("focusin", ".phone", function () {
        $(".phoneValidation").text("");
    });

    $(document).on("focusin", ".email", function () {
        $(".emailValidation").text("");
    });

    $(document).on("click", ".mItem", function () {
        $(".mItem").removeClass("mItemMenu");
        $(this).addClass("mItemMenu");
    });

    $(document).on("click", ".mInnerItem", function () {
        $(".mInnerItem").removeClass("mItemInnerMenu");
        $(this).addClass("mItemInnerMenu");
    });

    $(document).on("click", ".main-menu", function () {
        var item = $(this).attr("data-item");
        $("." + item).toggle();
    });

    // Add minus icon for collapse element which is open by default
    $(".collapse.show").each(function () {
        $(this).prev(".card-header").find(".fa").addClass("fa-minus").removeClass("fa-plus");
    });

    // Toggle plus minus icon on show hide of collapse element
    $(".collapse").on('show.bs.collapse', function () {
        $(this).prev(".card-header").find(".fa").removeClass("fa-plus").addClass("fa-minus");
    }).on('hide.bs.collapse', function () {
        $(this).prev(".card-header").find(".fa").removeClass("fa-minus").addClass("fa-plus");
    });

    $('#dvModal').on('shown.bs.modal', function () {
        $('.modal-dialog input:visible:enabled:first').trigger('focus');
    });


});