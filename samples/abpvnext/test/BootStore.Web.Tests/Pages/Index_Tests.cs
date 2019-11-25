using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace BootStore.Pages
{
    public class Index_Tests : BootStoreWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
