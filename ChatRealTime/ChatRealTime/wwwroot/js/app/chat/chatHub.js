'use strict';

const conexion = new signalR.HubConnectionBuilder()
                            .withUrl("/chathub")
                            .build();

conexion.on("GetMessage", (message) => {
    let li = document.createElement("li");
    li.textContent = `${message.user} - ${message.content}`;
    document.getElementById("lstMessages").appendChild(li);
});

conexion.start()
        .then(() => {
            let li = document.createElement('li');
            li.textContent = `Bienvenido al chat`;
            document.getElementById("lstMessages").appendChild(li);
        })
        .catch((error) => {
            console.error(error);
 })

document.getElementById("btnSend").addEventListener("click", (e) => {
    e.preventDefault();
    let txtUser = document.getElementById("txtUser").value;
    let txtMessage = document.getElementById("txtMessage").value;
    const objMessage = {
        user: txtUser,
        content: txtMessage
    };
    conexion.invoke("SendMessage", objMessage)
            .catch((error) => {
                console.error(error);
            });
    txtMessage = "";
});

document.getElementById("btnClear").addEventListener("click", (e) => {
    e.preventDefault();
    document.getElementById("lstMessages").innerHTML = "";
    document.getElementById("txtMessage").innerHTML = "";
    let li = document.createElement('li');
    li.textContent = `Bienvenido al chat`;
    document.getElementById("lstMessages").appendChild(li);
});