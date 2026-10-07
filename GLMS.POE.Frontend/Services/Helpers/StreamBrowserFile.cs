using Microsoft.AspNetCore.Components.Forms;

namespace GLMS.Services.Helpers
{
    
    public class StreamBrowserFile : IBrowserFile
    {
        private readonly Stream _stream;

        public string Name { get; }

        public DateTimeOffset LastModified 
        { 
            get { return DateTimeOffset.Now; }
        }

        public long Size => _stream.Length;

        public string ContentType => "application/pdf";

        public StreamBrowserFile(Stream stream, string fileName)
        {
            _stream = stream;
            Name = fileName;
        }

        public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        {
            if (_stream.Length > maxAllowedSize)
                throw new IOException($"File size exceeds maximum allowed size of {maxAllowedSize} bytes.");

            _stream.Position = 0;
            return _stream;
        }
    }
}
