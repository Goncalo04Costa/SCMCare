/*PDS
Daniela Brito nº25591
Daniela Pereira nº25988
Sofia Carvalho nº25991
Diogo Fernandes nº26008
Gonçalo Costa nº26024
LESI-IPCA */

const params = new URLSearchParams(window.location.search);
const tipoOperacao = params.get('opr');
const id = params.get('id');

function DesabilitarCampos() {
	$("#data").prop('disabled', true);
	$("#utn1").prop('disabled', true);
	$("#hosp1").prop('disabled', true);
	$("#desc").prop('disabled', true);
}

function CarregarDadosCampos() {
	$.ajax({
		url: '/api/Utentes',
		method: 'GET',
		data: {
			historico0: true,
			historico1: false
		},
		dataType: 'json',
		success: function (response) {
			dadosDropdownUtentes(response);
		},
		error: function (xhr, status, error) {
			console.error(xhr.responseText);
		}
	});

	$.ajax({
		url: '/api/Hospitais',
		method: 'GET',
		data: {
			ativo0: false,
			ativo1: true
		},
		dataType: 'json',
		success: function (response) {
			dadosDropdownHospitais(response);
		},
		error: function (xhr, status, error) {
			console.error(xhr.responseText);
		}
	});
}

function CarregarDados() {
	$.ajax({
		url: '/api/Consultas/' + id,
		method: 'GET',
		dataType: 'json',
		success: function (response) {
			InserirDados(response[0]);
		},
		error: function (xhr, status, error) {
			console.error(xhr.responseText);
		}
	});
}

function InserirDados(dados) {
	$("#id").val(dados.id);
	$("#data").val(dados.data.split('T')[0]);

	$("#utn1").val(dados.utente);
	//$("#utn1").setAttribute('data-value', dados.utenteId);
	$("#hosp1").val(dados.hospital);
	//$("#hosp1").setAttribute('data-value', dados.hospitalId);

	$("#desc").val(dados.descricao);
}


function dadosDropdownUtentes(dados){
	const input = document.getElementById('utn1');
	const dropdown = document.getElementById('utn2');

	input.addEventListener('input', function() {
	  const searchTerm = input.value.toLowerCase();
	  dropdown.innerHTML = '';

	  if (searchTerm.length === 0) {
		dropdown.classList.remove('show');
		return;
	  }

	  const filteredData = dados.filter(item => item.nome.toLowerCase().includes(searchTerm));
	  
	  if (filteredData.length === 0) {
		dropdown.classList.remove('show');
		return;
	  }

	  filteredData.forEach(item => {
		const suggestion = document.createElement('a');
		suggestion.classList.add('list-group-item', 'list-group-item-action', 'dropdown-auto');
		suggestion.textContent = item.nome;
		suggestion.setAttribute('data-value', item.id);

		suggestion.addEventListener('click', function() {
		  input.value = item.nome;
		  input.setAttribute('data-value', item.id);
		  dropdown.innerHTML = '';
		  dropdown.classList.remove('show');
		});

		dropdown.appendChild(suggestion);
	  });

	  dropdown.classList.add('show');
	});

	document.addEventListener('click', function(event) {
	  if (!dropdown.contains(event.target) && event.target !== input) {
		dropdown.innerHTML = '';
		dropdown.classList.remove('show');
	  }
	});
}


function dadosDropdownHospitais(dados){
	const input = document.getElementById('hosp1');
	const dropdown = document.getElementById('hosp2');

	input.addEventListener('input', function() {
	  const searchTerm = input.value.toLowerCase();
	  dropdown.innerHTML = '';

	  if (searchTerm.length === 0) {
		dropdown.classList.remove('show');
		return;
	  }

	  const filteredData = dados.filter(item => item.nome.toLowerCase().includes(searchTerm));
	  
	  if (filteredData.length === 0) {
		dropdown.classList.remove('show');
		return;
	  }

	  filteredData.forEach(item => {
		const suggestion = document.createElement('a');
		suggestion.classList.add('list-group-item', 'list-group-item-action', 'dropdown-auto');
		suggestion.textContent = item.nome;
		suggestion.setAttribute('data-value', item.id);

		suggestion.addEventListener('click', function() {
		  input.value = item.nome;
		  input.setAttribute('data-value', item.id);
		  dropdown.innerHTML = '';
		  dropdown.classList.remove('show');
		});

		dropdown.appendChild(suggestion);
	  });

	  dropdown.classList.add('show');
	});

	document.addEventListener('click', function(event) {
	  if (!dropdown.contains(event.target) && event.target !== input) {
		dropdown.innerHTML = '';
		dropdown.classList.remove('show');
	  }
	});
}

function Acao() {
	var id = $("#id").val();
	var dataVal = $("#data").val();
	var utenteVal = $("#utn1").data("value");
	var hospitalVal = $("#hosp1").data("value");
	var descricaoVal = $("#desc").val();

	var objeto = {
		data: dataVal,
		descricao: descricaoVal,
		hospitaisId: hospitalVal,
		utentesId: utenteVal,
		funcionariosId: null,
		responsaveisId: null
	};

	$.ajax({
		url: '/api/Consultas',
		method: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(objeto),
		success: function (response) {
			//window.close();
			window.location.href = "Consultalst.html";
		},
		error: function (xhr, status, error) {
			console.error(xhr.responseText);
		}
	});
}


window.onload = function () {
	if (tipoOperacao == 'ver') {
		$("#btnAcao").hide();
		DesabilitarCampos();
		CarregarDados();
	}
	else if (tipoOperacao == 'add') {
		CarregarDadosCampos();
	}
}