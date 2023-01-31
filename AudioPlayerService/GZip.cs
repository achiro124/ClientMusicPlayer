using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerService
{
    public class GZip
    {
        public string SourceFile { get; set; }
        public string CompressedFile { get; set; }
        public string TargetFile { get; set; }
        public string Path { get; set; }

        public void Compress()
        {
            // поток для чтения исходного файла
            FileStream sourceStream = new FileStream(SourceFile, FileMode.OpenOrCreate);
            // поток для записи сжатого файла
            FileStream targetStream = File.Create(CompressedFile);

            // поток архивации
            GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
            sourceStream.CopyToAsync(compressionStream); // копируем байты из одного потока в другой

            Console.WriteLine($"Сжатие файла {SourceFile} завершено.");
            Console.WriteLine($"Исходный размер: {sourceStream.Length}  сжатый размер: {targetStream.Length}");
        }

        public void Decompress()
        {
            // поток для чтения из сжатого файла
            FileStream sourceStream = new FileStream(CompressedFile, FileMode.OpenOrCreate);
            // поток для записи восстановленного файла
            FileStream targetStream = File.Create(TargetFile);
            // поток разархивации
            GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
            decompressionStream.CopyToAsync(targetStream);
            Console.WriteLine($"Восстановлен файл: {TargetFile}");
        }


    }
}
