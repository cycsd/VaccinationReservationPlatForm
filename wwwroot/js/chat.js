"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    var time = new Date();
    var date = time.getFullYear() + "/" + (time.getMonth() + 1) + "/" + time.getDate() + " " + time.getHours() + "點" + time.getMinutes() + "分" + time.getSeconds() + "秒";
    
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = user + "說:" + msg +"時間:" + date;
    /*li.textContent = `${user} 說: ${msg}  時間:${date}`;*/
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
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