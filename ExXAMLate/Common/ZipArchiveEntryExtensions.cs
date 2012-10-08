using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExXAMLate.Common
{
    public static class ZipArchiveEntryExtensions
    {
        public static async Task SaveToFile(this ZipArchiveEntry entry, IStorageFile file)
        {
            Stream newFileStream = await file.OpenStreamForWriteAsync();
            Stream fileData = entry.Open();
            var data = new byte[entry.Length];
            await fileData.ReadAsync(data, 0, data.Length);
            newFileStream.Write(data, 0, data.Length);
            await newFileStream.FlushAsync();
        }
    }
}