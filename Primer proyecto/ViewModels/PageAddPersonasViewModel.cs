using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;
using Primer_proyecto.Models;
using Primer_proyecto.Services;

namespace Primer_proyecto.ViewModels;

public partial class PageAddPersonasViewModel : BaseViewModel
{
    private readonly DatabaseServices _database;

    private Personas _personaEditar;

    [ObservableProperty]
    private string nombre;

    [ObservableProperty]
    private string apellido;

    [ObservableProperty]
    private DateTime fechaNac = DateTime.Now;

    [ObservableProperty]
    private string correo;

    [ObservableProperty]
    private string telefono;

    [ObservableProperty]
    private string fotoBase64 = string.Empty;

    [ObservableProperty]
    private string textoBoton = "Guardar";

    public PageAddPersonasViewModel(DatabaseServices database)
    {
        _database = database;
    }

    public void CargarPersona(Personas persona)
    {
        if (persona == null)
            return;

        _personaEditar = persona;

        Nombre = persona.Nombre;
        Apellido = persona.Apellido;
        FechaNac = persona.FechaNac;
        Correo = persona.Correo;
        Telefono = persona.Telefono;
        FotoBase64 = persona.FotoBase64;

        TextoBoton = "Actualizar";
    }

    [RelayCommand]
    private async Task TomarFoto()
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Permiso requerido",
                "Se necesita acceso a la cámara",
                "OK");

            return;
        }

        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Este dispositivo no soporta captura de cámara.",
                "OK");

            return;
        }

        var foto = await MediaPicker.Default.CapturePhotoAsync();

        if (foto == null)
            return;

        using var stream = await foto.OpenReadAsync();

        using var memory = new MemoryStream();

        await stream.CopyToAsync(memory);

        FotoBase64 = Convert.ToBase64String(memory.ToArray());
    }

    [RelayCommand]
    private async Task Guardar()
    {
        if (string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(Apellido))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Nombre y Apellido son obligatorios",
                "OK");

            return;
        }

        Personas persona = new Personas
        {
            Nombre = Nombre,
            Apellido = Apellido,
            FechaNac = FechaNac,
            Correo = Correo,
            Telefono = Telefono,
            FotoBase64 = string.IsNullOrWhiteSpace(FotoBase64)
                ? _personaEditar?.FotoBase64 ?? ""
                : FotoBase64
        };

        if (_personaEditar != null)
        {
            persona.Id = _personaEditar.Id;

            await _database.UpdatePersona(persona);

            await Application.Current.MainPage.DisplayAlert(
                "Información",
                "Registro Actualizado",
                "OK");
        }
        else
        {
            await _database.InsertPersona(persona);

            await Application.Current.MainPage.DisplayAlert(
                "Información",
                "Registro Guardado",
                "OK");
        }

        Nombre = "";
        Apellido = "";
        Correo = "";
        Telefono = "";
        FechaNac = DateTime.Now;
        FotoBase64 = "";
        TextoBoton = "Guardar";
    }
}