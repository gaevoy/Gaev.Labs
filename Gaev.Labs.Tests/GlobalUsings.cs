global using FluentAssertions;
global using NUnit.Framework;
global using System.Collections.Immutable;
global using NSubstitute;

// run tests in parallel by default, read more https://gaevoy.com/2023/07/19/speed-up-nunit-tests.html
[assembly: Parallelizable(ParallelScope.All)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
