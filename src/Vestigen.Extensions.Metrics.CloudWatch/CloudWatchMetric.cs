using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Vestigen.Extensions.Metrics.Abstractions;

namespace Vestigen.Extensions.Metrics.CloudWatch
{
    public class CloudWatchMetric : IMetric
    {
        private readonly AmazonCloudWatchClient _service;
        private readonly string _namespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudWatchMetric"/> class using explicit settings.
        /// </summary>
        /// <param name="prefix">The name of the metric.</param>
        public CloudWatchMetric(string @namespace)
            : this(@namespace, new AmazonCloudWatchClient())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudWatchMetric"/> class using a settings class.
        /// </summary>
        /// <param name="settings">The settings class used to configure the metric</param>
        public CloudWatchMetric(ICloudWatchMetricSettings settings)
            : this(settings.Namespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudWatchMetric"/> class using a <see cref="AmazonCloudWatchConfig"/> instance.
        /// </summary>
        /// <param name="prefix">The name of the metric.</param>
        /// <param name="config">The settings class used to configure the metric</param>
        public CloudWatchMetric(string @namespace, AmazonCloudWatchConfig config)
            : this(@namespace, new AmazonCloudWatchClient(config))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudWatchMetric"/> class using a <see cref="AmazonCloudWatchClient"/> instance.
        /// </summary>
        /// <param name="prefix">The name of the metric.</param>
        /// <param name="client">The pre-configured CloudWatch client.</param>
        public CloudWatchMetric(string @namespace, AmazonCloudWatchClient client)
        {
            _namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            _service = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return MetricScope.Push(state.ToString());
        }

        public void Push<T>(MetricType type, string statName, T value, double sampleRate, string[] tags)
        {
            var metricBuilder = new StringBuilder();
            metricBuilder.Append(MetricScope.Current.CompleteName);
            metricBuilder.Append((metricBuilder.Length > 0 ? "." : string.Empty) + statName);

            // Configure tags as dimensions
            var dimensions = new List<Dimension>();
            if (tags != null)
            {
                dimensions.AddRange(tags
                    .Select(tag => tag.Split(':'))
                    .Select(items => new Dimension
                    {
                        Name = items[0],
                        Value = items[1]
                    }));
            }
            
            // Configure the datum to report
            var datum = new MetricDatum
            {
                Dimensions = dimensions,
                MetricName = metricBuilder.ToString(),
                Timestamp = DateTime.UtcNow,
                Value = double.Parse(value.ToString()),
                StorageResolution = 1
            };
            
            switch (type)
            {
                case MetricType.Timer:
                    datum.Unit = StandardUnit.Milliseconds;
                    break;               
                case MetricType.Gauge:
                    datum.Unit = StandardUnit.Percent;
                    break;
                case MetricType.Counter:
                case MetricType.Histogram:
                case MetricType.Set:
                    datum.Unit = StandardUnit.Count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            // Configure the request object and send it
            var request = new PutMetricDataRequest
            {
                Namespace = _namespace,
                MetricData = new List<MetricDatum>
                {
                    datum
                }
            };

            _service.PutMetricDataAsync(request).Wait();
        }

        public void Counter<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Increment(string statName, int value = 1, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Decrement(string statName, int value = 1, double sampleRate = 1, params string[] tags)
        {
            Push(MetricType.Counter, statName, value, sampleRate, tags);
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Gauge, statName, value, sampleRate, tags);
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Histogram, statName, value, sampleRate, tags);
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Set, statName, value, sampleRate, tags);
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, string[] tags = null)
        {
            Push(MetricType.Timer, statName, value, sampleRate, tags);
        }

        public IMetricTimer StartTimer(string name, double sampleRate = 1, string[] tags = null)
        {
            return new MetricTimer(this, name, sampleRate, tags);
        }

        public void Time(Action action, string statName, double sampleRate = 1, string[] tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                action();
            }
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, string[] tags = null)
        {
            using (StartTimer(statName, sampleRate, tags))
            {
                return func();
            }
        }
    }
}