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
    $("#nome").prop('disabled', true);
    $("#desc").prop('disabled', true);
    $("[name=dietaOptions]").prop('disabled', true);
    $("[name=ativoOptions]").prop('disabled', true);
}

function InserirDados(dados) {
    $("#id").val(dados.id);
    $("#nome").val(dados.nome);
    $("#desc").val(dados.descricao);

    if (dados.tipo) {
        $('#inlineRadio1').prop('checked', true);
    } else {
        $('#inlineRadio2').prop('checked', true);
    }
    if (dados.ativo) {
        $('#inlineRadio3').prop('checked', true);
    } else {
        $('#inlineRadio4').prop('checked', true);
    }
}

function CarregarDados() {
    $.ajax({
        url: '/api/Sobremesas/' + id,
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            InserirDados(response);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function EditarDados() {
    var idVal = $("#id").val();
    var nomeVal = $("#nome").val();
    var descicaoVal = $("#desc").val();

    var dietaVal = $('input[name="dietaOptions"]:checked').val() == 1;
    var ativoVal = $('input[name="ativoOptions"]:checked').val() == 1;

    var objeto = {
        nome: nomeVal,
        descricao: descicaoVal,
        tipo: dietaVal,
        ativo: ativoVal
    };

    $.ajax({
        url: '/api/Sobremesas/' + id,
        method: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(objeto),
        success: function (response) {
            //window.close();
            window.location.href = "Sobremesaslst.html";
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function AdicionarDados() {
    var nomeVal = $("#nome").val();
    var descicaoVal = $("#desc").val();

    var dietaVal = $('input[name="dietaOptions"]:checked').val() == 1;
    var ativoVal = $('input[name="ativoOptions"]:checked').val() == 1;

    var objeto = {
        nome: nomeVal,
        descricao: descicaoVal,
        tipo: dietaVal,
        ativo: ativoVal
    };

    $.ajax({
        url: '/api/Sobremesas',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(objeto),
        success: function (response) {
            //window.close();
            window.location.href = "Sobremesaslst.html";
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function Acao() {
    if (tipoOperacao == 'edt') {
        EditarDados();
    }
    else if (tipoOperacao == 'add') {
        AdicionarDados();
    }
}

window.OnPageLoad = function () { }
window.onload = function () {
    if (tipoOperacao == 'ver') {
        $("#btnAcao").hide();
        DesabilitarCampos();
        CarregarDados();
    }
    else if (tipoOperacao == 'edt') {
        CarregarDados();
    }
}