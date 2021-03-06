﻿using Microsoft.Extensions.Primitives;

namespace Vestigen.Extensions.Metrics.CloudWatch
{
    public class CloudWatchMetricSettings : ICloudWatchMetricSettings
    {
        public IChangeToken ChangeToken { get; set; }
        
        public ICloudWatchMetricSettings Reload()
        {
            return this;
        }

        public string Prefix { get; set; }
        
        public string Region { get; set; }
    }
}