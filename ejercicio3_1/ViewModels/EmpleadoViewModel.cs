using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ejercicio3_1.ViewModels
{
    public class EmpleadoViewModel : BaseViewModel
    {
        Plugin.Media.Abstractions.MediaFile FileFoto = null;

        public bool btncrearIsVisible { get; set; }
        public bool btnactualizarIsVisible { get; set; }
        public bool btneliminarIsVisible { get; set; }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        private string _apellidos;

        public string Apellidos
        {
            get { return _apellidos; }
            set { _apellidos = value; OnPropertyChanged(); }
        }

        private int _edad;

        public int Edad
        {
            get { return _edad; }
            set { _edad = value; OnPropertyChanged(); }
        }

        private string _direccion;

        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; OnPropertyChanged(); }
        }

        private string _puesto;

        public string Puesto
        {
            get { return _puesto; }
            set { _puesto = value; OnPropertyChanged(); }
        }

        private byte[] _foto;

        public byte[] Foto
        {
            get { return _foto; }
            set { _foto = value; OnPropertyChanged(); }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                SetProperty(ref _imageSource, value);
            }
        }



        public ICommand ClearCommand { private set; get; }
        public ICommand CrearCommand { private set; get; }
        public ICommand ActualizarCommand { private set; get; }
        public ICommand EliminarCommand { private set; get; }
        public ICommand TomarFotoCommand { private set; get; }

        public INavigation Navigation { get; set; }

        public EmpleadoViewModel()
        {
            ClearCommand = new Command(() => Clear());
            CrearCommand = new Command(() => Crear());
            ActualizarCommand = new Command(() => Actualizar());
            EliminarCommand = new Command(() => Eliminar());
            TomarFotoCommand = new Command(() => TomarFoto());
        }

        void Clear()
        {
            Nombre = string.Empty;
            Apellidos = string.Empty;
            Edad = 0;
            Direccion = string.Empty;
            Puesto = string.Empty;
            Foto = null;
        }

        async void Crear()
        {

            var resultado = await App.DBase.EmpleadoSave(new Models.Empleado()
            {
                Id = 0,
                Nombre = Nombre,
                Apellidos = Apellidos,
                Edad = Edad,
                Direccion = Direccion,
                Puesto = Puesto,
                Foto = Foto
            });

            if (resultado == 1)
            {
                await App.Current.MainPage.DisplayAlert("Éxito", "Empleado creado con éxito", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Éxito", "El empleado no se creó", "Ok");
            }

            await Navigation.PopAsync();

        }

        async void Actualizar()
        {

            var resultado = await App.DBase.EmpleadoSave(new Models.Empleado()
            {
                Id = Id,
                Nombre = Nombre,
                Apellidos = Apellidos,
                Edad = Edad,
                Direccion = Direccion,
                Puesto = Puesto,
                Foto = Foto
            });

            if (resultado == 1)
            {
                await App.Current.MainPage.DisplayAlert("Éxito", "Empleado actualizado con éxito", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Éxito", "El empleado no se actualizó", "Ok");
            }

            await Navigation.PopAsync();

        }

        async void Eliminar()
        {

            var resultado = await App.DBase.EmpleadoDelete(new Models.Empleado()
            {
                Id = Id,
                Nombre = Nombre,
                Apellidos = Apellidos,
                Edad = Edad,
                Direccion = Direccion,
                Puesto = Puesto,
                Foto = null
            });

            if (resultado == 1)
            {
                await App.Current.MainPage.DisplayAlert("Aviso", "Empleado eliminado con éxito", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Aviso", "El empleado no se eliminó", "Ok");
            }

            await Navigation.PopAsync();
        }

        async void TomarFoto()
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Obtener fotografía", "Cancelar", null, "Seleccionar de galería", "Tomar foto");

            if (action == "Seleccionar de galería") { seleccionarfoto(); }
            if (action == "Tomar foto") { tomarfoto(); }
        }

        private async void tomarfoto()
        {
            FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Fotos",
                Name = "fotografia.jpg",
                SaveToAlbum = true,
                CompressionQuality = 20
            });


            if (FileFoto != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    return FileFoto.GetStream();
                });

                using (System.IO.MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    Foto = memory.ToArray();
                }
            }
        }

        private async void seleccionarfoto()
        {

            FileFoto = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 20
            });


            if (FileFoto == null)
                return;

            ImageSource = ImageSource.FromStream(() =>
            {
                return FileFoto.GetStream();
            });

            using (System.IO.MemoryStream memory = new MemoryStream())
            {
                Stream stream = FileFoto.GetStream();
                stream.CopyTo(memory);
                Foto = memory.ToArray();
            }
        }

    }
}
