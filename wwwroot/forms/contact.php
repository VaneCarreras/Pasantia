<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Obtener los datos del formulario
    $name = $_POST['name'];
    $email = $_POST['email'];
    $message = $_POST['message'];

    // Validar los campos (opcional, pero recomendado)
    if (!empty($name) && !empty($email) && !empty($message) && filter_var($email, FILTER_VALIDATE_EMAIL)) {
        // Configuración del correo
        $destinatario = "vanecarreras91@gmail.com";  // Cambia esto a tu dirección de correo
        $asunto = "Nuevo mensaje de contacto de: $name";
        
        $contenido = "Name: $name\n";
        $contenido .= "Email: $email\n";
        $contenido .= "Message:\n$message\n";
        
        

        // Enviar el correo
        if (mail($destinatario, $asunto, $contenido)) {
            echo "El mensaje se envió correctamente.";
        } else {
            echo "Error al enviar el mensaje.";
        }
    } else {
        echo "Por favor, rellena todos los campos correctamente.";
    }
}
?>
