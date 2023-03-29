const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/push")
    .configureLogging(signalR.LogLevel.Error)
    .build();

// получение сообщения от сервера
hubConnection.on('Notify', function (message) {

    var elem = document.getElementById('notificationText');
    console.log(message);
    elem.innerHTML = message;
    $("#myModal").modal();
});

hubConnection.start();