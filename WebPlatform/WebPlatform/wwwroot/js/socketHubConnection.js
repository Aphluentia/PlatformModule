

function onMessageReceived(message, conns) {
    $.ajax({
        type: "GET",
        url: "/Modules/AddModule",
        data: { port: message.split("-")[1], appType: message.split("-")[0], conns: conns },
        success: function (data) {
            $("#ModulesPartial").html(data);
        }
    });
}