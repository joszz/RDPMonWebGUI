"use strict";

$(function () {
    if ("serviceWorker" in navigator) {
        window.addEventListener("load", function () {
            var baseUri = $("body").data("baseuri");

            navigator.serviceWorker.register(baseUri + "Session/ServiceWorker", { scope: baseUri }).then(function (_registration) {
                // Registration was successful
            }, function (_err) {
                // registration failed :(
            });
        });
    }

    $(".fb-frame").fancybox({
        defaultType: "iframe",
        iframe: {
            css: {
                width: '1000px'
            }
        }
    });
});
