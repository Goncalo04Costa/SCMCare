/*PDS
Daniela Brito nº25591
Daniela Pereira nº25988
Sofia Carvalho nº25991
Diogo Fernandes nº26008
Gonçalo Costa nº26024
LESI-IPCA */

const params = new URLSearchParams(window.location.search);
const tipoUtilizador = params.get('usr');

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
                c5.innerHTML = '<span style="cursor:pointer" class="material-icons" title="Funcionário ' + item.funcionarioId+' como acompanhante">check_circle</span>';
            }
            else {
                c5.innerHTML = '<span style="cursor:pointer" class="material-icons" title="Utente sem acompanhante">cancel</span>';
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