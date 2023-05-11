"use strict";

//Conexion
var connection = new signalR.HubConnectionBuilder()
    .withUrl(urlFor("GenerateHub"), { transport: signalR.HttpTransportType.WebSockets })
	.configureLogging(signalR.LogLevel.Information)
	.build();


// Acciones recibidas del Hub
connection.on("CreatedUser", function (connectionId) {
	$('#connectionId').val(connectionId);
});

connection.on("FinishGenerateCode", function (data) {
	//console.log("FinishGenerateCode", data);
	//console.log("data.error", data.error);
	//console.log("data.success", data.success);
	if (data.error.length > 0) {
		var Errores = '<ul>';
		for (var i = 0; i < data.error.length; i++) {
			Errores += '<li>' + data.error[i] + '</li>';
		}
		Errores += '</ul>';
		$('#ErrorModalGenerating').html(Errores);
		$('#ErrorModal').modal('show');
	}
	if (data.success) {
		saveAsFile(data.nameFile, data.file);
	}
	$('#ButtonGenerating').hide();
	$('#ButtonGenerate').show();
});

var tableActual = "";
var index = 0;

connection.on("ReceiveProgressGenerate", function (table, template, percentage) {
	//console.log(percentage);
	if (tableActual != table) {
		tableActual = table;
		index++;
		$('#ProgressGenerateCode').prepend('<dt>' + index + '. ' + tableActual + '</dt>');
	}
	$('#ProgressGenerateCode').prepend("<dd>" + template + "</dd>");
	$('#ProgressBarGenerate').html(percentage + "%");
	$("#ProgressBarGenerate").attr("aria-valuenow", percentage);
	$("#ProgressBarGenerate").css("width", percentage + "%");
	//console.log("ReceiveProgressGenerate", table, template, percentage);
	
});


// Configuracion
StartConection();

async function start() {
	try {
		await StartConection();
		console.log("connected");
	} catch (err) {
		console.log(err);
		setTimeout(() => start(), 5000);
	}
};

connection.onclose(async () => {
	await start();
});

function StartConection() {
	connection.start().then(function () {
		connection.invoke("CreateUser")
			.catch(function (err) {
				alert("No pudo crear usuario temporal. | " +err.toString());
				return console.error(err.toString());
			});
	}).catch(function (err) {
		alert("Servidor desconectado. | " + err.toString());
		return console.error(err.toString());
	});
}

// Funciones 

function GenerateCode() {

	$('#ButtonGenerating').show();
	$('#ButtonGenerate').hide();

	//console.log("GenerateCode");
	$('#ProgressGenerateCode').html("");
	$('#ProgressBarGenerate').html("0%");
	$("#ProgressBarGenerate").attr("aria-valuenow", "0");
	$("#ProgressBarGenerate").css("width", "0%");

	var NumberConnection = $('#Conexion').dxSelectBox('instance').option("value");
	var FrameworkActual = $('#Framework').dxSelectBox('instance').option("value");
	var TipoOrigenActual = $('#TipoOrigen').dxSelectBox('instance').option("value");
	var NombreRoe = $('#IdNombreRoe').val();
	var PrefijoRoe = $('#ProyectoArea').dxSelectBox('instance').option("value");  //$('#IdPrefijoRoe').val();
	var Consulta = $('#TextAreaConsulta').val();
	var Tables = $('#Tables').dxTreeList('instance').getSelectedRowsData();
	var Templates = $('#Templates').dxTreeList('instance').getSelectedRowsData();
	var UserId = $('#connectionId').val();

	var JsonTables = JSON.stringify(Tables);
	var JsonTemplates = JSON.stringify(Templates);

	connection.invoke("GenerateCode", JsonTables, JsonTemplates, NumberConnection,
		FrameworkActual, TipoOrigenActual, NombreRoe, PrefijoRoe, Consulta, UserId)
		.catch(function (err) {
			alert("Se perdio la conexion con el servidor. Se intentara reconectar pero si el problema persiste recargar la pagina.");
			$('#ButtonGenerating').hide();
			$('#ButtonGenerate').show();
			StartConection();
			return console.error(err.toString());
	});
}


// Funcion para descargar un archivo desde MVC Net Core
function saveAsFile(filename, file) {
	var link = document.createElement('a');
	link.download = filename;
	link.href = urlFor("/Home/DownloadFile" + "?path=" + file + "&name=" + filename);
	console.log(link);
	document.body.appendChild(link);
	link.click();
	document.body.removeChild(link);
}
