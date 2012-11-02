using Bottles.Commands;
using Bottles.Deployment.Commands;
using Bottles.Diagnostics;
using FubuCore;
using FubuTestingSupport;
using Milkman;
using NUnit.Framework;
using Rhino.Mocks;

namespace Bottles.Tests.Deployment.Commands
{


    [TestFixture]
    public class when_the_directory_exists_but_the_force_flag_is_true : InitializeCommandContext
    {
        protected override void theContextIs()
        {
            theDeploymentDirectoryExists();
            theInput.ForceFlag = true;
        }

        [Test]
        public void should_have_deleted_the_existing_deployment_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.DeleteDirectory(TheDeploymentDirectory));
        }

        [Test]
        public void should_create_the_deployment_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory));
        }

        [Test]
        public void should_create_the_recipes_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory, ProfileFiles.RecipesDirectory));
        }

        [Test]
        public void should_create_the_bottles_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory, ProfileFiles.BottlesDirectory));
        }

        [Test]
        public void should_create_the_profiles_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory, ProfileFiles.ProfilesDirectory));
        }

        [Test]
        public void the_return_boolean_should_be_true_to_denote_success()
        {
            theReturnBooleanFlag.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_the_directory_does_not_already_exist_happy_path : InitializeCommandContext
    {
        protected override void theContextIs()
        {
            theDeploymentDirectoryDoesNotExist();
        }

        [Test]
        public void should_create_the_deployment_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory));
        }

        [Test]
        public void should_create_the_recipes_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory, ProfileFiles.RecipesDirectory));
        }

        [Test]
        public void should_create_the_bottles_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory, ProfileFiles.BottlesDirectory));
        }

        [Test]
        public void should_create_the_profiles_directory()
        {
            MockFor<IFileSystem>().AssertWasCalled(x => x.CreateDirectory(TheDeploymentDirectory, ProfileFiles.ProfilesDirectory));
        }

        [Test]
        public void the_return_boolean_should_be_true_to_denote_success()
        {
            theReturnBooleanFlag.ShouldBeTrue();
        }
    }


    [TestFixture]
    public class when_the_deployment_directory_already_exists_and_the_force_flag_is_false : InitializeCommandContext
    {
        protected override void theContextIs()
        {
            theDeploymentDirectoryExists();
            theInput.ForceFlag = false;
        }

        [Test]
        public void command_should_not_have_deleted_the_deployment_directory()
        {
            MockFor<IFileSystem>().AssertWasNotCalled(x => x.DeleteDirectory(theInput.Settings.DeploymentDirectory, ProfileFiles.DeploymentFolder));
        }

        [Test]
        public void should_not_have_made_any_new_folders()
        {
            MockFor<IFileSystem>().AssertWasNotCalled(x => x.CreateDirectory(null), x => x.IgnoreArguments());
        }

        [Test]
        public void should_return_false_denoting_that_the_command_did_not_complete_successfully()
        {
            theReturnBooleanFlag.ShouldBeFalse();
        }
    }


    public abstract class InitializeCommandContext : InteractionContext<InitializeCommand>
    {
        protected InitializeInput theInput;
        protected string TheDeploymentDirectory;
        protected bool theReturnBooleanFlag;

        protected sealed override void beforeEach()
        {
            theInput = new InitializeInput(){
                DeploymentFlag = "Application1"
            };

            TheDeploymentDirectory = theInput.Settings.DeploymentDirectory;

            theContextIs();

            theReturnBooleanFlag = ClassUnderTest.Initialize(theInput, MockFor<IFileSystem>());
        }

        protected abstract void theContextIs();



        protected void theDeploymentDirectoryExists()
        {
            MockFor<IFileSystem>().Stub(x => x.DirectoryExists(TheDeploymentDirectory))
                .Return(true);
        }

        protected void theDeploymentDirectoryDoesNotExist()
        {
            MockFor<IFileSystem>().Stub(x => x.DirectoryExists(TheDeploymentDirectory))
                .Return(false);
        }
    }
}