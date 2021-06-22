'use strict';

const conexion = new signalR.HubConnectionBuilder()
                            .withUrl("/chathub")
                            .build();

conexion.on("GetMessage", (message) => {
    let li = document.createElement("li");
    li.textContent = `${message.user} - ${message.content}`;
    document.getElementById("lstMessages").appendChild(li);
});

document.getElementById("btnConnect").addEventListener("click", (e) => {
    if (conexion.state === signalR.HubConnectionState.Disconnected) {
        conexion.start().then(() => {
            let li = document.createElement('li');
            li.textContent = `Conexion Exitosa.`;
            document.getElementById("lstMessages").appendChild(li);
            document.getElementById("btnConnect").value = "Desconectar";
            document.getElementById("txtUser").setAttribute("disabled", "true");
            document.getElementById("btnEnviar").removeAttribute("disabled");
            let txtUser = document.getElementById("txtUser").value;
            let txtRoom = document.getElementById("txtRoom").value;
            const objMessage = {
                user: txtUser,
                content: "",
                room: txtRoom
            };
            conexion.invoke("SendMessage", objMessage)
                .catch((error) => {
                    console.error(error);
                });
            conexion.stream("CounterAsync").subcribe({
                next: (item) => { document.getElementById("lblDuration").innerHTML = item },
                complete: (item) => { document.getElementById("lblDuration").innerHTML = "Se acabo el tiempo" },
                error: (error) => { console.error(error) },
            });
            document.getElementById("btnSend").removeAttribute("disabled");
        }).catch((error) => {
            console.error(error);
        });
    } else if (conexion.state === signalR.HubConnectionState.Connected) {
        conexion.stop();
        let li = document.createElement('li');
        let txtUser = document.getElementById("txtUser").value;
        li.textContent = `${txtUser} Ha salido del chat.`;
        document.getElementById("lstMessages").appendChild(li);
        document.getElementById("btnConnect").value = "Conectar";
        document.getElementById("txtUser").removeAttribute("disabled");
        document.getElementById("txtUser").value = "";
    }
});

document.getElementById("btnSend").addEventListener("click", (e) => {
    e.preventDefault();
    if (conexion.state !== signalR.HubConnectionState.Connected) {
        return;
    }
    let txtUser = document.getElementById("txtUser").value;
    let txtMessage = document.getElementById("txtMessage").value;
    let txtRoom = document.getElementById("txtRoom").value;
    const objMessage = {
        user: txtUser,
        content: txtMessage,
        room: txtRoom
    };
    conexion.invoke("SendMessage", objMessage)
            .catch((error) => {
                console.error(error);
            });
    document.getElementById("txtMessage").value = "";
});