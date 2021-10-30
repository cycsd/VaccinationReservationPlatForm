"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var time = new Date();
    var hour = time.getHours();
    var min = time.getMinutes();
    var conversationHeight = $("#conversation").get(0).scrollHeight;
    if (min < 10) {
        min = "0" + min;
    }
    if (hour > 12) {
        hour = "下午" + (hour - 12);
    }
    else {
        hour = "上午" + hour;
    }
    var date = time.getFullYear().toString() + "/" + (time.getMonth() + 1).toString() + "/" + time.getDate().toString() + " " + hour.toString() + ":" + min.toString();
    $("#messagesList").append('<li><div class="chat-block"><div class="message-text">' + user + " 說: " + '<br/>' + msg + '</div><div class="message-time pull-right">' + date + '</div></div></li>').scrollTop($("#messagesList").prop('scrollHeight'));
});

connection.on("ReceiveEnterMessage", function (user) {
    var userName = user;
    var time = new Date();
    var hour = time.getHours();
    var min = time.getMinutes();
    if (min < 10) {
        min = "0" + min;
    }
    if (hour > 12) {
        hour = "下午" + (hour - 12);
    }
    else {
        hour = "上午" + hour;
    }
    var date = time.getFullYear().toString() + "/" + (time.getMonth() + 1).toString() + "/" + time.getDate().toString() + " " + hour.toString() + ":" + min.toString();
    $("#messagesList").append('<li><div class="chat-block"><div class="message-text">訊息:' + '<br/>' + userName + '</div><div class="message-time pull-right">' + date + '</div></div></li>').scrollTop($("#messagesList").prop('scrollHeight'));
});



connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    var user = document.getElementById("entermsg").value;
    connection.invoke("EnterMessage", user).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).then(function () {
        document.getElementById("messageInput").value = "";
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});





