using System.Collections.Generic;
using System.Linq;
using DeveloperTools.Core;
using DeveloperTools.Models;
using Xunit;

namespace DeveloperTools.Tests
{
    public class ListTests
    {
        [Fact]
        public void UnionTest()
        {
            var l1 = new List<ModuleInfo>
                     {
                         new ModuleInfo
                         {
                             Id = "KEY1"
                         },
                         new ModuleInfo
                         {
                             Id = "KEY2"
                         }
                     };
            var l2 = new List<ModuleInfo>
                     {
                         new ModuleInfo
                         {
                             Id = "KEY2"
                         },
                         new ModuleInfo
                         {
                             Id = "KEY3"
                         }
                     };

            var result = l1.Union(l2).DistinctBy(_ => _.Id);

            Assert.Equal(3, result.Count());
        }
    }
}
