using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using Pasantia.Data;
using Pasantia.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Pasantia.Controllers{
public class ContactoController : Controller
{
    // Este método manejará el POST del formulario de contacto
    [HttpPost]
    public IActionResult EnviarCorreo(string name, string email, string message)
    {
        // Validar los campos
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(message) )
        // && IsValidEmail(email)
        {
            try
            {
                // Crear el mensaje MIME
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(name, email));  // Remitente
                emailMessage.To.Add(new MailboxAddress("Vane Carreras", "vanecarreras91@gmail.com"));  // Destinatario
                emailMessage.Subject = $"Nuevo mensaje de contacto de: {name}";
                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Nombre: {name}\nEmail: {email}\nMensaje:\n{message}"
                };

                // Configurar el cliente SMTP usando MailKit
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);  // Conectar con TLS
                    smtpClient.Authenticate("vanecarreras91@gmail.com", "3558");  // Autenticar
                    smtpClient.Send(emailMessage);  // Enviar el correo
                    smtpClient.Disconnect(true);  // Desconectar
                }

                ViewBag.Message = "El mensaje se envió correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error al enviar el mensaje: {ex.Message}";
            }
        }
        else
        {
            ViewBag.Message = "Por favor, rellena todos los campos correctamente.";
        }

        // Retornar la vista con el mensaje (éxito o error)
        return View();
    }

    // Método de validación de correo electrónico (para asegurarse de que el correo es válido)
    // private bool IsValidEmail(string email)
    // {
    //     try
    //     {
    //         var addr = new MailboxAddress(email);
    //         return addr.Address == email;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }
}


}