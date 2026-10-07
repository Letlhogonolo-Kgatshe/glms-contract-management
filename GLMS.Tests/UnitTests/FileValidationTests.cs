using GLMS.POE.Backend.Services.Implementations;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace GLMS.Tests.UnitTests
{
    public class FileValidationTests
    {
        private readonly FileService _sut;

        public FileValidationTests()
        {
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());

            _sut = new FileService(
                env: mockEnv.Object,
                logger: NullLogger<FileService>.Instance
            );
        }

        // ── Valid files ───────────────────────────────────────────────────────

        [Fact]
        public void IsValidPdf_WithPdfExtension_ReturnsTrue()
        {
            Assert.True(_sut.IsValidPdf("signed_agreement.pdf"));
        }

        [Fact]
        public void IsValidPdf_WithUpperCasePdfExtension_ReturnsTrue()
        {
            // Extension check must be case-insensitive
            Assert.True(_sut.IsValidPdf("CONTRACT.PDF"));
        }

        [Fact]
        public void IsValidPdf_WithMixedCasePdfExtension_ReturnsTrue()
        {
            Assert.True(_sut.IsValidPdf("agreement.Pdf"));
        }

        // ── Invalid / restricted file types ──────────────────────────────────

        [Fact]
        public void IsValidPdf_WithExeExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("malware.exe"));
        }

        [Fact]
        public void IsValidPdf_WithDocxExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("contract.docx"));
        }

        [Fact]
        public void IsValidPdf_WithJpgExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("scan.jpg"));
        }

        [Fact]
        public void IsValidPdf_WithZipExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("archive.zip"));
        }

        [Fact]
        public void IsValidPdf_WithBatExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("script.bat"));
        }

        [Fact]
        public void IsValidPdf_WithPhpExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("shell.php"));
        }

        // ── Edge cases ────────────────────────────────────────────────────────

        [Fact]
        public void IsValidPdf_WithNullFileName_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf(null!));
        }

        [Fact]
        public void IsValidPdf_WithEmptyString_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf(""));
        }

        [Fact]
        public void IsValidPdf_WithWhitespaceOnly_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("   "));
        }

        [Fact]
        public void IsValidPdf_WithNoExtension_ReturnsFalse()
        {
            Assert.False(_sut.IsValidPdf("contractfile"));
        }

        [Fact]
        public void IsValidPdf_WithPdfInNameButWrongExtension_ReturnsFalse()
        {
            // Tricky: "mypdf.exe" has "pdf" in the name but .exe extension
            Assert.False(_sut.IsValidPdf("mypdf.exe"));
        }

        [Theory]
        [InlineData(".exe")]
        [InlineData(".bat")]
        [InlineData(".sh")]
        [InlineData(".js")]
        [InlineData(".vbs")]
        [InlineData(".msi")]
        [InlineData(".dll")]
        public void IsValidPdf_RestrictedExtensions_AlwaysReturnFalse(string extension)
        {
            Assert.False(_sut.IsValidPdf($"file{extension}"));
        }
    }
}
