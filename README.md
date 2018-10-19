Metrics
=======

AppVeyor on master:

[![Build status](https://ci.appveyor.com/api/projects/status/f3r19qows4jnko2r/branch/master?svg=true)](https://ci.appveyor.com/project/wholroyd/vestigen-extensions-metrics/branch/master)

[![AppVeyor build](https://img.shields.io/appveyor/ci/vestigenltd/vestigen-extensions-metrics/master.svg?logo=appveyor)](https://ci.appveyor.com/project/vestigenLtd/vestigen-extensions-metrics/branch/master)

[![AppVeyor tests](https://img.shields.io/appveyor/tests/vestigenltd/vestigen-extensions-metrics/master.svg?logo=appveyor)](https://ci.appveyor.com/project/vestigenLtd/vestigen-extensions-metrics/branch/master/tests)


AppVeyor on develop:

[![Build status](https://ci.appveyor.com/api/projects/status/f3r19qows4jnko2r/branch/develop?svg=true)](https://ci.appveyor.com/project/VestigenLtd/vestigen-extensions-metrics/branch/develop)

[![AppVeyor build](https://img.shields.io/appveyor/ci/vestigenltd/vestigen-extensions-metrics/develop.svg?logo=appveyor)](https://ci.appveyor.com/project/vestigenLtd/vestigen-extensions-metrics/branch/develop)

[![AppVeyor tests](https://img.shields.io/appveyor/tests/vestigenltd/vestigen-extensions-metrics/develop.svg?logo=appveyor)](https://ci.appveyor.com/project/vestigenLtd/vestigen-extensions-metrics/branch/develop/tests)

Metrics is a framework and set of abstractions for publishing metrics in an application using providers for Datadog, NewRelic, CloudWatch, and simple debug output, based on StatsD-like interface.

Overview
--------

This framework follows the same architectual model as the `Microsoft.Extensions.*` NuGet packages that are owned and maintained by the Microsoft ASP.NET team.

This particular framework is a **standalone** package for which there are no upstream packages on which to build. As soon as Microsoft provides one, this framework will move to build upon it as a framework extension.

Packages
--------

The vision for this framework is to maintain framework compatibility for `netstandard1.4` and `netstandard2.0` as much as possible. This is only possible if the underlying libraries maintain support for these same framework versions and may/may not follow this vision. Any exceptions to this rule will be noted below in the package list. For more information on framework version targeting, please see [this document](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

The packages are published with unsigned assemblies.

The packages produced from this repository are as follows:

- Vestigen.Extensions.Metrics
- Vestigen.Extensions.Metrics.Abstractions
- Vestigen.Extensions.Metrics.CloudWatch
- Vestigen.Extensions.Metrics.Datadog
- Vestigen.Extensions.Metrics.Debug
- Vestigen.Extensions.Metrics.NewRelic (netstandard2.0 only)

Samples
-------

See `samples/ConsoleApp` as an example of how to implement the package.

Each provider will have it's own implementation specific details for the given framework in which it encapsulates. Viewing the test suites can assist with bootstraping a particular implementation.
