
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

var s = 1;
connection.on("ReceiveMessage", function (user, message) {
    var div = document.createElement("div");
    div.className = "container bg-white rounded-pill border text-dark p-2 my-2 rounded";
    div.style = "display:inline-block;";   
    div.textContent = `${user} says ${message}`;
    document.getElementById("messagesList").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage",message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.addEventListener('DOMContentLoaded', function () {
    var chatItems = document.querySelectorAll('.chat-item');
    var chatNameElement = document.getElementById('chatName');

    chatItems.forEach(function (item) {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            var chatName = this.getAttribute('data-chat-name');
            chatNameElement.textContent = chatName;

            // Optionally collapse the chat list on smaller screens after a chat is selected
            var chatList = document.getElementById('chatList');
            if (window.innerWidth < 768) {
                var collapse = new bootstrap.Collapse(chatList, {
                    toggle: true
                });
            }
        });
    });
});