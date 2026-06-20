using Primer_proyecto.Controllers;
using Primer_proyecto.Models;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;

namespace Primer_proyecto.Views;

public partial class PageAddPersonas : ContentPage
{
    private Personas _personaEditar;
    private string _fotoBase64 = string.Empty;

    public string FotoBase64
    {
        get => _fotoBase64;
        set
        {
            if (_fotoBase64 == value)
                return;

            _fotoBase64 = value;
            OnPropertyChanged();
        }
    }

    public PageAddPersonas(Personas persona = null)
    {
        InitializeComponent();
        BindingContext = this;

        if (persona != null)
        {
            _personaEditar = persona;
            Nombre.Text = persona.Nombre;
            Apellido.Text = persona.Apellido;
            FechaNac.Date = persona.FechaNac;
            Correo.Text = persona.Correo;
            Telefono.Text = persona.Telefono;
            FotoBase64 = persona.FotoBase64 ?? string.Empty;
            btnguardar.Text = "Actualizar";
        }
        else
        {
            btnguardar.Text = "Guardar";
        }
    }

    private async void TomarFoto_Clicked(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso requerido", "Se necesita acceso a la cámara", "OK");
            return;
        }

        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await DisplayAlert("Error", "Este dispositivo no soporta captura de cámara.", "OK");
            return;
        }
        //tomamos foto
        var foto = await MediaPicker.Default.CapturePhotoAsync();
        if (foto == null)
            return;


        //Leemos foto y transformamos a bytes
        using var stream = await foto.OpenReadAsync();
        //creamos memoria
        using var memoryStream = new MemoryStream();
        //copiamos la foto a memoria
        await stream.CopyToAsync(memoryStream);

        //Convertimos a base64
        FotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
    }

    private async void btnguardar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Nombre.Text) || string.IsNullOrWhiteSpace(Apellido.Text))
        {
            await DisplayAlert("Error", "Nombre y Apellido son obligatorios", "OK");
            return;
        }

        Personas person = new Personas
        {
            Nombre = Nombre.Text,
            Apellido = Apellido.Text,
            FechaNac = FechaNac.Date ?? DateTime.Now,
            Correo = Correo.Text,
            Telefono = Telefono.Text,
            FotoBase64 = string.IsNullOrWhiteSpace(FotoBase64)
                ? (_personaEditar?.FotoBase64 ?? string.Empty)
                : FotoBase64
        };

        PersonasController controller = new PersonasController();

        if (_personaEditar != null)
        {
            person.Id = _personaEditar.Id;
            await controller.ActualizarPersona(person);
            await DisplayAlert("Información", "Registro Actualizado", "OK");
        }
        else
        {
            await controller.GuardarPerson(person);
            await DisplayAlert("Información", "Registro Guardado", "OK");
        }

        Nombre.Text = string.Empty;
        Apellido.Text = string.Empty;
        Correo.Text = string.Empty;
        Telefono.Text = string.Empty;
        FechaNac.Date = DateTime.Now;
        FotoBase64 = string.Empty;
    }
}