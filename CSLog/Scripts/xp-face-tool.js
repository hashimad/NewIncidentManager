/*document.addEventListener("contextmenu", function (e) {
        e.preventDefault();
    }, false);*/
var interval = null;
$(document).on('ready', function () {
   // interval = setInterval(updateWallteBalance, 10000);
});


//function updateWallteBalance() {
//    $.ajax({
//        type: "POST",
//        headers: { "cache-control": "no-cache" },
//        url: '../Account/GetBalance',
//        async: true,
//        timeout: 120000,
//        success: function (data, status, xhr) {
//            if (!data.IsAuthenticated) {
//                if (interval != null) {
//                    clearInterval(interval);
//                }

//            }
//            if (data.IsSuccessful) {
//                $("#walletBalance").html("Deposit Balance ₦ " + data.Balance);
//                $("#withdrawableBalance").html("Withdrawable Balance ₦ " + data.WithdrawableBalance);
//            }
//        },
//        error: function (xhr, status, error) {
//            //ErrorMessage(error);
//            return false;
//        }
//    });
//}
function validateSession(data) {
    if (!data.IsAuthenticated) {
        location.href = "/login";
    }
}
function format_money(strVal) {
    return numeral(strVal).format('0,0');
}
function ErrorMessage(msg, id) {
    sweetAlert("", msg, "error");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}

function SuccessMessage(msg, id) {
    sweetAlert("", msg, "success");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}
function InfoMessage(msg, id) {
    sweetAlert("", msg, "info");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}
function InlineErrorMessage(msg, id) {
    if (id == undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-danger "><a href="#" class="close" data-dismiss="alert" aria-label="close">×</a><strong>' + msg + '</strong></div>';
        $("#" + id).html(html);
    }
}
function InlineSuccessMessage(msg, id) {
    if (id == undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-success "><a href="#" class="close" data-dismiss="alert" aria-label="close">×</a><strong>' + msg + '</strong></div>';
        $("#" + id).html(html);
    }
}

function ClearInlineError(id) {
    $("#" + id).html("");
}
function adjustSideBar() {
    if ($('*').hasClass('leftMenuCloneWrap'))
        $('.leftMenuCloneWrap').remove();

    if ($("#sp-left .sp-module .sp-module-content > ul").is(".nav", ".menu") && window.innerWidth <= 991) {
        var cloneTitle = $("#sp-left .sp-module .sp-module-title").html();;
        var cloneListArray = [];
        $("#sp-left .sp-module .sp-module-content > ul.nav.menu li").each(function () {
            cloneListArray.push([$(this).find('a').attr('href'), $(this).find('a').html()]);
        });

        var selectCloneListMenu = "";
        var count = 0;
        jQuery.each(cloneListArray, function (key, value) {
            if (count == 0) {
                if (value[0].indexOf('#') == -1) {
                    selectCloneListMenu += "<div class='leftMenuCloneWrap'><div class='leftMenuCloneInnerWrap'><div class='cloneTitle'>" + cloneTitle + "</div>        <div class='cloneSelector'><select id='leftMenuClone' name='leftMenuClone' onchange='if(this.value) window.location.href=this.value;'>";
                }
                else {
                    selectCloneListMenu += "<div class='leftMenuCloneWrap'><div class='leftMenuCloneInnerWrap'><div class='cloneTitle'>" + cloneTitle + "</div>        <div class='cloneSelector'><select id='leftMenuClone' name='leftMenuClone' onchange='if(this.value){ window.location.href=this.value; location.reload();}'>";
                }
            }
            count++;
            selectCloneListMenu += "<option value='" + value[0] + "'>" + value[1] + "</option>";
        });
        selectCloneListMenu += "</select></div></div></div>";

        $('#sp-main-body #sp-component').prepend(selectCloneListMenu);
        selectCloneListMenu = "";
        $("#leftMenuClone option[value='" + $("#sp-left .sp-module .sp-module-content > ul.nav.menu li.current a").attr('href') + "']").attr("selected", "selected");
        $("#sp-left").hide();
    }
    else {
        $("#sp-left > .sp-module:first-child").show();
        $("#sp-left").show();
    }
    if (window.innerWidth <= 991 && window.innerWidth >= 768) {
        $("#sp-right").css("width", "100%");
    }
    else {
        $("#sp-right").css("width", "");
    }

}

function fix_chars2(textBox) {
    //textBox.value = textBox.value.replace(/[@'&_,%`"*#|<>;]/g, "");
    var strVal;
    var strVal1;
    var strVal2;
    var dot;
    var i;
    var strComma;
    strVal2 = "";
    strComma = "";
    strVal1 = "";
    strVal = textBox.value;
    dot = 0;
    for (i = 0; i < strVal.length; i++) {
        if (strVal.substring(i, i + 1).indexOf('.') > -1) {
            dot = dot + 1;
        }
        if ((strVal.substring(i, i + 1).indexOf('0') > -1) || (strVal.substring(i, i + 1).indexOf("1") > -1) || (strVal.substring(i, i + 1).indexOf("2") > -1) || (strVal.substring(i, i + 1).indexOf("3") > -1) || (strVal.substring(i, i + 1).indexOf("4") > -1) || (strVal.substring(i, i + 1).indexOf("5") > -1) || (strVal.substring(i, i + 1).indexOf("6") > -1) || (strVal.substring(i, i + 1).indexOf("7") > -1) || (strVal.substring(i, i + 1).indexOf("8") > -1) || (strVal.substring(i, i + 1).indexOf("9") > -1) || ((strVal.substring(i, i + 1).indexOf('.') > -1) && dot <= 1)) {
            strVal1 = strVal1 + strVal.substring(i, i + 1);
        }
    }
    if ((strVal1.indexOf('.') == 0)) {
        strVal1 = "0" + strVal1;
    }
    if (strVal1.indexOf('.') > 0) {
        if (((strVal1.length) - (strVal1.indexOf('.') + 1)) > 2) {
            strVal1 = strVal1.substring(0, strVal1.indexOf('.') + 3);
        }
    }

    strVal = "";
    if (strVal1.indexOf('.') != -1) {

        strVal = strVal1.substring(strVal1.indexOf('.'), strVal1.indexOf('.') + 3);
        strVal1 = strVal1.substring(0, strVal1.indexOf('.'));
    }
    //	    	        alert(strVal1.indexOf('.'));

    while (strVal1.length > 0) {
        if (strVal1.length > 3) {
            strVal2 = strVal1.substring(strVal1.length - 3, strVal1.length) + strComma + strVal2;
            strVal1 = strVal1.substring(0, strVal1.length - 3);
            strComma = ",";
        }
        else {
            strVal2 = strVal1 + strComma + strVal2;
            strVal1 = "";
        }
    }

    //	    	    if (strVal.length>0){
    //	    	    strVal= strVal;
    //	    	    }

    if (strVal2.indexOf('.') > 0) {
        strVal2 = strVal2.substring(0, strVal2.indexOf('.'));
        alert(strVal2);
    }

    textBox.value = strVal2 + strVal;
}

    return false;
}