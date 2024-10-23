var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
//add to route the userId

//add to route the userId
connection.on("ReceiveMessage", function (user, message) {
    var div = document.createElement("div");
    div.className = "container bg-white rounded-pill border text-dark p-2 my-2 rounded";
    div.style = "display:inline-block;";
    div.textContent = `${user} says ${message}`;
    document.getElementById("messagesList").appendChild(div);
});

document.getElementById("testings").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.send("SendPrivateMessage","asd",message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
