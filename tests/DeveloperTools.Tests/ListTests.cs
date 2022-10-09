using System.Collections.Generic;
using System.Linq;
using DeveloperTools.Models;
using Xunit;

namespace DeveloperTools.Tests;

public class ListTests
{
    [Fact]
    public void UnionTest()
    {
        var l1 = new List<ModuleInfo> { new() { Id = "KEY1" }, new() { Id = "KEY2" } };
        var l2 = new List<ModuleInfo> { new() { Id = "KEY2" }, new() { Id = "KEY3" } };

        var result = l1.Union(l2).DistinctBy(_ => _.Id);

        Assert.Equal(3, result.Count());
    }
}
