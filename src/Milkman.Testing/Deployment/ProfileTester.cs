using Bottles.Deployment;
using NUnit.Framework;
using FubuTestingSupport;
using System.Linq;

namespace Bottles.Tests.Deployment
{
    [TestFixture]
    public class ProfileTester
    {
        private Profile theProfile;

        [SetUp]
        public void SetUp()
        {
            theProfile = new Profile("something");
        }

        [Test]
        public void read_text_of_a_recipe()
        {
            theProfile.ReadText("recipe:baseline");
            theProfile.Recipes.ShouldHaveTheSameElementsAs("baseline");
        }

        [Test]
        public void read_another_setting()
        {
            theProfile.ReadText("Key1=Value1");
            theProfile.Recipes.Any().ShouldBeFalse();

            
            theProfile.Data["Key1"].ShouldEqual("Value1");
        }

        [Test]
        public void read_comment()
        {
            theProfile.ReadText("#a comment");
            theProfile.Recipes.Any().ShouldBeFalse();

        }
    }
}