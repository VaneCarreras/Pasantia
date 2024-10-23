window.onload = ResetearBtn();

function ResetearBtn() {
    $("#login-submit").removeClass("ocultar");
    $("#loader").addClass("ocultar");
}

function FiltrarEmail() {
    let email = $("#Email").val();
    if (email == "") {
        $('#email-invalido-login').removeClass('ocultar');
    }
    else {
        $('#email-invalido-login').addClass('ocultar');
    }
}
function FiltrarContrasena() {
    let password = $("#Password").val();
    if (password == "") {
        $('#password-invalido').removeClass('ocultar');
    }
    else {
        $('#password-invalido').addClass('ocultar');
    }
}

function IniciarSesion() {

    var email = $('#Email').val();
    var password = $('#Password').val();
    $('.login-invalid').addClass('ocultar');
    $('#email-invalido-login').addClass('ocultar');
    $('#password-invalido').addClass('ocultar');

    let control = true;

    console.log(email);
    console.log(password);
    if (email == '') {
        $('#email-invalido-login').removeClass('ocultar');
        control = false;
        return;
    }
    if (password == '') {
        $('#password-invalido').removeClass('ocultar');
        control = false;
        return;
    }

    if (control) {
        $('.fa fa-exclamation-circle').addClass('ocultar');
        $("#login-submit").addClass("ocultar");
        $("#loader").removeClass("ocultar");
    }
}

window.onload = function () {
    $('[data-toggle="tooltip"]').tooltip();
};