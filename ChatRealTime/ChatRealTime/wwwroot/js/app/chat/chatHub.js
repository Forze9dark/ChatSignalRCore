'use strict';

const conexion = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

conexion.on("GetMessage", (user, message) => {
    let li = document.createElement("li");
    li.textContent = `${user} - ${message}`;
    document.getElementById("lstMessages").appendChild(li);
});

conexion.start().then(() => {
    let li = document.createElement('li');
    li.textContent = `Bienvenido al chat`;
    document.getElementById("lstMessages").appendChild(li);
}).catch((error) => {
    console.error(error);
})

document.getElementById("btnSend").addEventListener("click", (e) => {
    let user = document.getElementById("txtUser").value;
    let message = document.getElementById("txtMessage").value;
    conexion.invoke("SendMessage", user, message).catch((error) => {
        console.error(error);
    });
    user.value = "";
    message.value = "";
    event.preventDefault();
});