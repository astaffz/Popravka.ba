using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X509;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Infrastructure.Adapters.Options;
using System.Text;


namespace PopravkaBa.Infrastructure.Adapters
{
    public class S3FileStorageAdapter : IFileStorage
    {
        private readonly IAmazonS3 _s3;
        private readonly R2Options _r2Options;

        public S3FileStorageAdapter(IAmazonS3 s3, IOptions<R2Options> options)
        {
            _s3 = s3;
            _r2Options = options.Value;
        }

        public static string Extension(string content) => content switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            "application/pdf" => ".pdf",
            _ => ""
        };
        public async Task<string> SpremiPublic(Stream sadrzaj, string contentType, CancellationToken ct = default)
        {
            // Generiši GUID u N formatu, bez D [ - ] , B [ {} ], P [ () ]
            var key = $"slike/{Guid.NewGuid():N}{Extension(contentType)}";
            await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _r2Options.PublicBucket,
                Key = key,
                InputStream = sadrzaj,
                ContentType = contentType,
                DisablePayloadSigning = true
            }, ct);
            return $"{_r2Options.PublicBaseURL}/{key}";
        }

        public async Task<string> SpremiPrivate(Stream sadrzaj, string fileName, string contentType, CancellationToken ct = default)
        {
            // Generiši GUID u N formatu, bez D [ - ] , B [ {} ], P [ () ]
            var key = $"prilozi/{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
            await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _r2Options.PrivateBucket,
                Key = key,
                InputStream = sadrzaj,
                ContentType = contentType,
                DisablePayloadSigning = true
            }, ct);
            return key;
        }
        public string GetSignedURL(string key, TimeSpan trajanje)
        => _s3.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = _r2Options.PrivateBucket,
                Key = key,
                Expires = DateTime.UtcNow.Add(trajanje),
                Verb = HttpVerb.GET,
                Protocol = Protocol.HTTPS,

            });
        
        public Task ObrisiPublic(string url, CancellationToken ct = default)
        {
            var key = url.Replace(_r2Options.PublicBaseURL.TrimEnd('/') + "/", "");
            return _s3.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _r2Options.PublicBucket,
                Key = key
            }, ct);
        }

        public Task ObrisiPrivate(string key, CancellationToken ct = default)
              => _s3.DeleteObjectAsync(new DeleteObjectRequest
              {
                  BucketName = _r2Options.PrivateBucket,
                  Key = key
              }, ct);

    }
}
