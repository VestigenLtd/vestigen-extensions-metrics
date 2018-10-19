using System;
using System.Threading;
using Amazon;
using Amazon.Runtime.CredentialManagement;

namespace Vestigen.Extensions.Metrics.CloudWatch.UnitTests
{
    /// <summary>
    /// Ensures a default AWS profile exists for tests
    /// </summary>
    /// <remarks>
    /// We don't need this fixture in any of the classes that utilize it (yet). We just need to ensure that a default
    /// profile exists long enough to perform all of the tests to avoid throwing an AmazonClientException on the
    /// CloudWatchMetric constructors where we create a client blindly (developer is only giving us the namespace)
    /// </remarks>   
    public class ApplicationInsightsMetricTestFixture : IDisposable
    {
        private readonly bool _profileExistedBefore;
        private readonly ICredentialProfileStore _profileManager;
        

        public ApplicationInsightsMetricTestFixture()
        {
            _profileManager = new SharedCredentialsFile();
            _profileExistedBefore = _profileManager.TryGetProfile(SharedCredentialsFile.DefaultProfileName, out _);
            if (_profileExistedBefore == false)
            {
                Profile = new CredentialProfile(
                    SharedCredentialsFile.DefaultProfileName,
                    new CredentialProfileOptions
                    {
                        AccessKey = "TemporaryCredentialsAccessKey",
                        SecretKey = "TemporaryCredentialsSecretKey"
                    })
                {
                    Region = RegionEndpoint.USEast1
                };

                _profileManager.RegisterProfile(Profile);

                Thread.Sleep(500);
            }
        }

        public CredentialProfile Profile { get; set; }

        public void Dispose()
        {
            if (_profileExistedBefore == false)
            {
                _profileManager.UnregisterProfile(SharedCredentialsFile.DefaultProfileName);
            }
        }
    }
}