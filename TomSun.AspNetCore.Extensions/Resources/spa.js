/// <reference path="../lib/jquery/dist/jquery.js" />

var site = site || {};
site.baseUrl = site.baseUrl || "";

String.prototype.format = function () {
    var str = this;
    for (var i = 0; i < arguments.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        str = str.replace(reg, arguments[i]);
    }
    return str;
}

var getAbsoluteUrl = function(relativeUrl) {
    var url = site.baseUrl + relativeUrl;
    return url;
}
var getDefaultValue = function(value, defaultValue) {
    return typeof value !== 'undefined' ? value : defaultValue;
}
var loadSpaContainer = function (spaElement, relativeUrl, ajaxType, ajaxData) {
    ajaxType = getDefaultValue(ajaxType, "get");
    ajaxData = getDefaultValue(ajaxData, "");


    var url = getAbsoluteUrl(relativeUrl);
    var avoidCaching = $(spaElement).attr("spa-no-cache");

    var content = $(spaElement).find("#contentContainer");
    var loadContent = $(spaElement).find("#loadContainer");
    var idleContent = $(spaElement).find("#idleContainer");

    $(content).css('display', 'none');
    $(idleContent).css('display', 'none');
    $(loadContent).css('display', 'inline');

    $.ajax({
        type: ajaxType,
        data: ajaxData,
        url: url,
        cache: !avoidCaching,
        dataType: "html",
        success: function (data) {
            $(content).html(data);
            $(loadContent).css('display', 'none');
            $(content).css('display', 'inline');
        },
        error: function (textStatus, errorThrown) {
            var errorMessage =
                "Failed to load SPA container with url '{0}'. HTTP Error text was '{1}', the error code was '{2}'"
                    .format(url, textStatus.statusText, textStatus.status);

            $(content).html(errorMessage);
            $(loadContent).css('display', 'none');
            $(content).css('display', 'inline');
        }
    });
}

var onSpaButtonClicked=function(button) {
    var componentId = $(button).attr("spa-target");
    var relativeUrl = $(button).attr("spa-url");
    var asyncComponent = $(document).find("#" + componentId);
    loadSpaContainer(asyncComponent, relativeUrl);
}

var onSpaFormSubmit = function (form, relativePostUrl) {
    var componentId = $(form).attr("spa-target");
    var asyncComponent = $(document).find("#" + componentId);
    var formData = $(form).serialize();
    loadSpaContainer(asyncComponent, relativePostUrl, 'post', formData);
}

$(document).ready(function (e) {
    // locate each partial section.
    // if it has a URL set, load the contents into the area.

    $(".spa-container").each(function (index, item) {
        var relativeUrl = $(item).attr("spa-url");
        if (relativeUrl) {
         
            loadSpaContainer($(item), relativeUrl);
        }
    });

});
