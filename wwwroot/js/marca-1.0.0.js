


        function PulsarTecla(event) {
            tecla = event.keyCode;

            if (tecla == 107 && event.altKey) {
                document.getElementById("AgregarMarca").click();
            }
        }

        window.onkeydown = PulsarTecla;

        // FUNCION QUE EVITA QUE GUARDE TODO AL PRESIONAR ENTER. (SE USA SOBRE TODO POR EL LECTOR DE CODIGO DE BARRAS).
        window.onkeydown = EvitarEnter;
        function EvitarEnter(e) {
            // averiguamos el código de la tecla pulsada (keyCode para IE y which para Firefox)
            tecla = (document.all) ? e.keyCode : e.which;
            // si la tecla no es 13 devuelve verdadero,  si es 13 devuelve false y la pulsación no se ejecuta
            return (tecla != 13);
        }
    

    
        //*************************************************
        //FUNCIÓN PARA QUITAR EL MSJ DE ERROR EN LA VISTA
        //*************************************************
        function QuitarMensaje() {
            $('#ErrorDeshabilitar').addClass('ocultar');
        }

        document.querySelector("#buscar").onkeyup = function () {
            $TableFilter("#tabla", this.value);
        }

        $TableFilter = function (id, value) {
            var rows = document.querySelectorAll(id + ' tbody tr');

            for (var i = 0; i < rows.length; i++) {
                var showRow = false;

                var row = rows[i];
                //row.style.display = 'none';
                row.classList.add('ocultar');

                for (var x = 0; x < row.childElementCount; x++) {
                    if (row.children[x].textContent.toLowerCase().indexOf(value.toLowerCase().trim()) > -1) {
                        showRow = true;
                        break;
                    }
                }

                if (showRow) {
                    //row.style.display = null;
                    row.classList.remove('ocultar');
                }
            }
        }

        //***********************************
        //FUNCIÓN PARA DESHABILITAR LA MARCA
        //***********************************
        function DeshabilitarMarca(MarcaID) {
            swal({
                title: "ATENCIÓN",
                icon: "warning",
                text: "¿Deshabilitar Marca?",

                dangerMode: true,
                closeOnClickOutside: false,
                closeOnEsc: false,

                buttons: {
                    cancel: {
                        text: "Cancelar",
                        value: null,
                        visible: true,
                        className: "botonRojoSweetAlert",
                        closeModal: true,
                    },
                    confirm: {
                        text: "Aceptar",
                        value: true,
                        visible: true,
                        className: "botonVerdeSweetAlert",
                        closeModal: true
                    }
                },
            })
                .then((willDelete) => {
                    if (willDelete) {
                        $.ajax({
                            type: "POST",
                            url: '../../Marcas/DeshabilitarMarca',
                            data: { id: MarcaID },
                            success: function (data) {
                                QuitarMensaje();
                                if (data == 0) {
                                    $("#ErrorDeshabilitar").removeClass("ocultar");
                                }
                                else {
                                    location.href = "../../Marcas/Index";
                                }
                            },
                            error: function (result) {
                            }
                        });
                    }
                    else { }
                });
        }

        //********************************
        //FUNCIÓN PARA HABILITAR LA MARCA
        //********************************
        function HabilitarMarca(MarcaID) {
            swal({
                title: "ATENCIÓN",
                icon: "warning",
                text: "¿Habilitar Marca?",

                dangerMode: true,
                closeOnClickOutside: false,
                closeOnEsc: false,

                buttons: {
                    cancel: {
                        text: "Cancelar",
                        value: null,
                        visible: true,
                        className: "botonRojoSweetAlert",
                        closeModal: true,
                    },
                    confirm: {
                        text: "Aceptar",
                        value: true,
                        visible: true,
                        className: "botonVerdeSweetAlert",
                        closeModal: true
                    }
                },
            })
                .then((willDelete) => {
                    if (willDelete) {
                        location.href = "../../Marcas/HabilitarMarca/" + MarcaID;
                    }
                });
        }

        window.onload = BuscarMarcas();

        document.addEventListener('DOMContentLoaded', () => {
            $('#buscar').on('keyup', () => {
                if (document.querySelector('#myTable').querySelectorAll('.ocultar').length === document.querySelector('#myTable').childElementCount) $('.agregar-busqueda').removeClass('ocultar');
                else $('.agregar-busqueda').addClass('ocultar');
            });
            let btnGuardar = document.querySelector('#btn-guardar');
            btnGuardar.addEventListener('click', GuardarMarca);
        });

        //*******************************************************
        //FUNCIÓN PARA ABRIR EL MODAL DE CREAR O EDITAR LA MARCA
        //*******************************************************
        function AbrirModal(param = 0) {
            //param === 0: Crear
            //param !== 0: Editar
            //console.log(param, ' Typeof:', typeof param)
            let tituloModal = document.querySelector('#exampleModalScrollableTitle');

            if (param === 0) {
                tituloModal.textContent = 'Marca Nueva';
                $('MarcaNombre').val($('#buscar').val());
                $('#MarcaID').val(0);

            }
            else {
                tituloModal.textContent = 'Editar Marca';
                $('#MarcaNombre').attr('disabled', true);
                $('#MarcaID').val(param);

                BuscarMarca();
            }
            setTimeout(function () { $("#MarcaNombre").focus(); }, 500);
            $('#ModalMarca').modal('show');
        }


        //***************************************************
        //FUNCIÓN PARA BUSCAR LAS MARCAS
        //***************************************************
        function BuscarMarcas() {
            $("#tbody-marcas").empty();
            $.ajax({
                type: "POST",
                url: '../../Marcas/BuscarTodasMarcas',
                data: {},
                success: function (marcasMostrar) {
                    $("#tbody-marcas").empty();
                    $.each(marcasMostrar, function (index, marca) {

                        let claseEliminado = '';
                        let habilitarDeshabilitar = '<td class="text-center tdBotones"><a onclick = "DeshabilitarMarca(' + marca.marcaID + ')" class="btn-tabla" title = "Deshabilitar" ><i class="bi bi-trash3-fill btnTablaEliminar"></i></a ></td>';
                        if (marca.eliminado == true) {
                            claseEliminado = 'fondoRojoTabla';
                            habilitarDeshabilitar = '<td class="text-center tdBotones fondoRojoTabla"><a onclick="HabilitarMarca(' + marca.marcaID + ')" class="btn-tabla" title="Habilitar"><i class="bi bi-check-lg btnTablaHabilitar"></i></a></td>';
                        }

                        $("#tbody-marcas").append('<tr class=' + claseEliminado + '>' +
                            '<td class="' + claseEliminado + '"><a onclick="AbrirModal(' + marca.marcaID + ')" class="elementosTabla" style="cursor:pointer" title="Editar Marca">' + marca.marcaNombre + '</td>' +
                            habilitarDeshabilitar +
                            '</tr>');
                    });
                },
                error: function (err) {
                    console.log(err);
                    swal({
                        title: "ATENCIÓN",
                        icon: "error",
                        text: "Error al visualizar las Marcas.",

                        dangerMode: true,
                        closeOnClickOutside: false,
                        closeOnEsc: false,

                        buttons: {
                            confirm: {
                                text: "Aceptar",
                                value: true,
                                visible: true,
                                className: "botonRojoSweetAlert",
                                closeModal: true
                            }
                        },
                    }).then(res => location.assign('/Marcas/Index'));
                }
            });
        }

        //*******************************************
        //FUNCIÓN PARA GUARDAR Y GUARDAR-EDITAR MARCA
        //*******************************************
        function GuardarMarca() {

            $('#btn-guardar').addClass('ocultar');
            $('#btn-espere-modal').removeClass('ocultar');
            $('#btn-cancelar').attr('disabled', true);

            let marcaNombre = $('#MarcaNombre').val();
            let marcaID = $('#MarcaID').val();

            //console.log('ActividadID:', actividadID, typeof actividadID)
            if (marcaID === "0") {
                $.ajax({
                    url: '../../Marcas/GuardarMarca',
                    type: 'POST',
                    data: { MarcaNombre: marcaNombre },
                    success: function (res) {
                        //console.log(res);
                        res.forEach(i => {
                            if (i === 'correcto') {
                                LimpiarModal();
                                return location.href = '../../Marcas/Index';
                            }
                            if (i.includes('error_MarcaNombre')) {
                                if (i === 'error_MarcaNombre') texto = '*Campo Requerido.';
                                else if (i === 'error_MarcaNombre_existe') texto = 'Ya existe esta Marca.';
                                document.querySelector(`#error_MarcaNombre`).textContent = texto;
                                document.querySelector(`#error_MarcaNombre`).classList.remove('ocultar');
                                LimpiarMsjError();

                                $('#btn-guardar').removeClass('ocultar');
                                $('#btn-espere-modal').addClass('ocultar');
                                $('#btn-cancelar').attr('disabled', false);
                            }

                        });
                    }
                });
            }
            else {
                $.ajax({
                    url: '../../Marcas/GuardarEditarMarca',
                    type: 'POST',
                    data: { MarcaID: marcaID, MarcaNombre: marcaNombre },
                    success: function (res) {
                        //console.log(res);
                        if (res.length > 0) {
                            let texto = '';
                            res.forEach(i => {
                                if (i === 'correcto') {
                                    LimpiarModal();
                                    return location.href = '../../Marcas/Index';
                                }
                                if (i.includes('error_MarcaNombre')) {
                                    if (i === 'error_MarcaNombre') texto = '*Campo Requerido.';
                                    else if (i === 'error_MarcaNombre_existe') texto = 'Ya existe esta Marca.';
                                    document.querySelector(`#error_MarcaNombre`).textContent = texto;
                                    document.querySelector(`#error_MarcaNombre`).classList.remove('ocultar');
                                    LimpiarMsjError();

                                    $('#btn-guardar').removeClass('ocultar');
                                    $('#btn-espere-modal').addClass('ocultar');
                                    $('#btn-cancelar').attr('disabled', false);
                                }
                            });
                        }
                        else {
                            LimpiarModal();
                            $('#btn-guardar').removeClass('ocultar');
                            $('#btn-espere-modal').addClass('ocultar');
                            $('#btn-cancelar').attr('disabled', false);
                        }
                    }
                });
            }
        }

        //**************************************************************
        //FUNCIÓN PARA BUSCAR LA INFO DE LA MARCA CUANDO QUEREMOS EDITAR
        //**************************************************************
        function BuscarMarca() {

            let marcaID = document.querySelector('#MarcaID').value;
            $.ajax({
                url: '../../Marcas/BuscarMarca',
                type: 'POST',
                data: { MarcaID: marcaID },
                success: function (res) {
                    //console.log(res);
                    $('#MarcaNombre').val(res.marcaNombre);
                    $('#MarcaNombre').attr('disabled', false);
                    $('#MarcaID').val(res.marcaID);
                }
            });
        }

        //**************************************************
        //FUNCIÓN PARA LIMPIAR EL MODAL AL CERRAR O CANCELAR
        //**************************************************
        $('#ModalMarca').on('hidden.bs.modal', LimpiarModal);
        function LimpiarModal() {
            $('#ModalMarca').modal('hide');
            $('#MarcaNombre').val('');
            $('#MarcaNombre').attr('disabled', false);
            $('#MarcaID').val(0);

            $('#btn-guardar').removeClass('ocultar');
            $('#btn-espere-modal').addClass('ocultar');
            $('#btn-cancelar').attr('disabled', false);
            document.querySelectorAll('span[id^="error_"]').forEach(i => {
                i.textContent = '';
                i.classList.add('ocultar');
            });
        }

        //************************************************
        //FUNCIÓN PARA LIMPIAR LOS MSJS DE ERROR DEL MODAL
        //************************************************
        function LimpiarMsjError() {
            document.querySelectorAll('span[id^="error_"]').forEach(i => {
                if (i.textContent !== '') {
                    i.previousElementSibling.addEventListener('keyup', () => {
                        i.textContent = '';
                        i.classList.add('ocultar');
                    });
                }
            })
        }

