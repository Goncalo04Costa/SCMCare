/*PDS
Daniela Brito nº25591
Daniela Pereira nº25988
Sofia Carvalho nº25991
Diogo Fernandes nº26008
Gonçalo Costa nº26024
LESI-IPCA */

const params = new URLSearchParams(window.location.search);
let tipoUtilizador = params.get('usr');

function CarregarDados() {	
    $.ajax({
        url: '/api/Consultas',
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            CriarTabela(response);
        },
        error: function(xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function AplicarFiltros() {
    var idMin = $("#id_de").val();
    var idMax = $("#id_ate").val();
    var hospitaisId = $("#hosp_id").val();
    var utentesId = $("#utnt_id").val();
    var funcionariosId = $("#func_id").val();
    var dataMin = $("#data_de").val();
    var dataMax = $("#data_ate").val();

    $.ajax({
        url: '/api/Consultas',
        method: 'GET',
        data: {
            idMin: idMin,
            idMax: idMax,
            hospitaisId: hospitaisId,
            utentesId: utentesId,
            funcionariosId: funcionariosId,
            dataMin: dataMin,
            dataMax: dataMax
        },
        dataType: 'json',
        success: function (response) {
            CriarTabela(response);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function CriarTabela(dados) {
    var tabela = document.getElementById('tabela');
    tabela.innerHTML = ''; // Limpa a tabela antes de criar novamente

    var tabelaHead = tabela.createTHead().insertRow(0);
    var tabelaBody = tabela.createTBody();

    var colunas = [
        { nome: 'Id', classe: 'col-1' },
        { nome: 'Utente', classe: 'col-5' },
        { nome: 'Hospital', classe: 'col-2' },
        { nome: 'Data', classe: 'col-2' },
        { nome: 'Estado', classe: 'col-1' },
        { nome: 'Opções', classe: 'col-1' }
    ];

    colunas.forEach(tituloColuna => {
        var th = document.createElement('th');
        var titulo = document.createTextNode(tituloColuna.nome);
        th.appendChild(titulo);
        th.className = tituloColuna.classe;
        tabelaHead.appendChild(th);
    });

    // Verifica se há dados
    if (dados && dados.length > 0) {
        dados.forEach(item => {
            var linha = tabelaBody.insertRow();

            var c1 = linha.insertCell(0);
            var c2 = linha.insertCell(1);
            var c3 = linha.insertCell(2);
            var c4 = linha.insertCell(3);
            var c5 = linha.insertCell(4);
            var c6 = linha.insertCell(5);

            var botoes = '<a title="Ver" style="cursor:pointer" onClick="ver(' + item.id + ');"><span class="material-icons">visibility</span></a>';
            botoes += '<a title="Remover" style="cursor:pointer" onClick="remover(' + item.id + ');"><span class="material-icons">highlight_off</span></a>';

            c1.innerHTML = item.id;
            c2.innerHTML = item.utente;
            c3.innerHTML = item.hospital;
            c4.innerHTML = item.data;
            if (item.responsavelId) {
                c5.innerHTML = '<span class="material-icons" title="Tem acompanhante">check_circle</span>';
            }
            else if (item.funcionarioId) {
                c5.innerHTML = '<span class="material-icons" title="Funcionário ' + item.funcionarioId+' como acompanhante">check_circle</span>';
            }
            else {
                c5.innerHTML = '<span style="cursor:pointer" onclick=abrirModal(\'' + (item.id + "," + item.data.split('T')[0]) + '\'); class="material-icons" title="Utente sem acompanhante">cancel</span>';
            }
            c6.innerHTML = botoes;
        });
    } else {
        // Caso não haja dados, exibe uma mensagem na tabela
        var linha = tabelaBody.insertRow();
        var celula = linha.insertCell();
        celula.colSpan = colunas.length;
        celula.innerHTML = "Nenhum dado disponível";
    }
}

function adicionar(id) {
    window.open("../PN2/Consultaedt.html?opr=add", "_self");
}

function ver(id) {
    window.open("../PN2/Consultaedt.html?opr=ver&id=" + id, "_self");
}

function dadosDropdownFuncionarios(dados) {
    const input = document.getElementById('func1');
    const dropdown = document.getElementById('func2');

    input.addEventListener('input', function () {
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
            suggestion.setAttribute('data-value', item.funcionarioID);

            suggestion.addEventListener('click', function () {
                input.value = item.nome;
                input.setAttribute('data-value', item.funcionarioID);
                dropdown.innerHTML = '';
                dropdown.classList.remove('show');
            });

            dropdown.appendChild(suggestion);
        });

        dropdown.classList.add('show');
    });

    document.addEventListener('click', function (event) {
        if (!dropdown.contains(event.target) && event.target !== input) {
            dropdown.innerHTML = '';
            dropdown.classList.remove('show');
        }
    });
}

function abrirModalFuncionario(id) {
    $.ajax({
        url: '/api/Funcionarios',
        method: 'GET',
        data: {
            historico0: true,
            historico1: false
        },
        dataType: 'json',
        success: function (response) {
            dadosDropdownFuncionarios(response);

            $("#idModalF").val(id);
            $('#modalFunc').modal('show');
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function abrirModalResponsavel(id, data) {
    $("#idModalR").val(id);
    $("#dataModalR").val(data);

    $('#modalResp').modal('show');
}


function abrirModal(dados, dados2) {
    //tipoUtilizador = "1";// !!! teste funcionário
    tipoUtilizador = "0";// !!! teste responsável
    if (tipoUtilizador == 1) {
        abrirModalFuncionario(dados.toString().split(',')[0]);
    }
    else if (tipoUtilizador == 0) {
        var teste = dados.toString().split(',');
        abrirModalResponsavel(dados.toString().split(',')[0], dados.toString().split(',')[1]);
    }
    else {
        alert("Erro a tentar abrir o modal.");
    }
}

function Atualiza(objeto) {

    $.ajax({
        url: '/api/Consultas/' + objeto.id,
        method: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(objeto),
        success: function (response) {
            AplicarFiltros();
            $('#modalFunc').modal('hide');
            $('#modalResp').modal('hide');
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });

}

function voltarMenu() {
    window.location.href = "https://localhost:7134/menu.html";
}

function Acao() {
    var idVal;
    var idVal2;

    if (tipoUtilizador == 0) {
        idVal = $("#idModalR").val();
        idVal2 = 1 // !!! Teste id do responsável atual
    }
    else if (tipoUtilizador == 1) {
        idVal = $("#idModalF").val();
        idVal2 = $("#func1").data("value");
    }
    else {
        return;
    }

    $.ajax({
        url: '/api/Consultas/' + idVal,
        method: 'GET',
        contentType: 'application/json',
        success: function (response) {
            var dataVal = response[0].data;
            var descicaoVal = response[0].descricao;
            var hospitalVal = response[0].hospitalId;
            var utenteVal = response[0].utenteId;

            var funcionariosIdVal;
            var responsaveisIdVal;

            if (tipoUtilizador == 0) {
                funcionariosIdVal = null;
                responsaveisIdVal = idVal2;
            }
            else if (tipoUtilizador == 1) {
                funcionariosIdVal = idVal2;
                responsaveisIdVal = null;
            }
            else {
                return;
            }
            var objeto = {
                id: idVal,
                data: dataVal,
                descricao: descicaoVal,
                hospitaisId: hospitalVal,
                utentesId: utenteVal,
                funcionariosId: funcionariosIdVal,
                responsaveisId: responsaveisIdVal
            };

            Atualiza(objeto);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
            return;
        }
    });
}

function remover(id) {
    $.ajax({
        url: '/api/Consultas/'+id,
        method: 'DELETE',
        dataType: 'json',
        success: function (response) {
            console.log("Consulta apagada com sucesso.");
            CarregarDados();
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

window.OnPageLoad = function () { }
window.onload = function () {
    CarregarDados();
}