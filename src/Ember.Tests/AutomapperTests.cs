using AutoMapper;
using Ember.Application.Mappings;
using System.Collections.Generic;
using Xunit;

namespace Ember.Tests
{
    public class AutomapperTests
    {
        private static IEnumerable<Profile> GetProfiles() 
        {
            yield return new AccountProfile();
            yield return new AccrualProfile();
            yield return new NewsProfile();
            yield return new PaymentProfile();
        }

        [Fact]
        public void CorrectlyConfigured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(GetProfiles());
            });

            config.AssertConfigurationIsValid();
        }
    }
}
