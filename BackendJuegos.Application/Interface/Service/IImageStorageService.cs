namespace BackendJuegos.Application.Interface.Service
{
    public interface IImageStorageService
    {
        Task EliminarImagenAsync(string imageUrl);
        Task<string> SubirImagenAsync(Stream fileStream, string filename, string contentType, string folder);
    }
}
