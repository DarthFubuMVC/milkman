﻿using System;
using System.Collections.Generic;
using System.IO;
using Bottles.Deployment.Commands;
using Bottles.Deployment.Diagnostics;
using Bottles.Diagnostics;
using Bottles.Environment;
using FubuCore;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace Bottles.Tests.Commands
{
    [TestFixture]
    public class InstallationRunnerTester : InteractionContext<InstallationRunner>
    {
        private EnvironmentLogEntry[] theEntries;
        private InstallInput theInput;

        protected override void beforeEach()
        {
            theEntries = new EnvironmentLogEntry[]{
                new EnvironmentLogEntry(), 
                new EnvironmentLogEntry(), 
                new EnvironmentLogEntry(), 
            };

            var root = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
            theInput = new InstallInput(){
                AppFolder = root.AppendPath("folder1")
            };

            MockFor<IEnvironmentGateway>().Stub(x => x.Install()).Return(theEntries);
            MockFor<IEnvironmentGateway>().Stub(x => x.CheckEnvironment()).Return(theEntries);
            MockFor<IEnvironmentGateway>().Stub(x => x.InstallAndCheckEnvironment()).Return(theEntries);
        }

        private void theEntriesIncludeFailures()
        {
            theEntries[0].Success = true;
            theEntries[1].Success = true;
            theEntries[2].Success = false;
        }

        private void theEntriesDoNotIncludeAnyFailures()
        {
            theEntries.Each(x => x.Success = true);
        }


        [Test]
        public void should_write_the_failure_to_the_console_if_there_are_any_failures()
        {
            theEntriesIncludeFailures();
            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IInstallationLogger>().AssertWasNotCalled(x => x.WriteSuccessToConsole());
            MockFor<IInstallationLogger>().AssertWasCalled(x => x.WriteFailureToConsole());
        }

        [Test]
        public void should_write_succes_to_the_console_if_not_failures_were_detected()
        {
            theEntriesDoNotIncludeAnyFailures();
            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IInstallationLogger>().AssertWasCalled(x => x.WriteSuccessToConsole());
            MockFor<IInstallationLogger>().AssertWasNotCalled(x => x.WriteFailureToConsole());
        }

        [Test]
        public void should_install_when_the_mode_is_install()
        {
            theInput.ModeFlag = InstallMode.install;

            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IEnvironmentGateway>().AssertWasCalled(x => x.Install());
        }

        [Test]
        public void should_check_environment_if_the_mode_is_check_environment()
        {
            theInput.ModeFlag = InstallMode.check;

            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IEnvironmentGateway>().AssertWasCalled(x => x.CheckEnvironment());
        }

        [Test]
        public void should_install_and_check_environment_if_mode_says_both()
        {
            theInput.ModeFlag = InstallMode.all;

            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IEnvironmentGateway>().AssertWasCalled(x => x.InstallAndCheckEnvironment());
        }

        [Test]
        public void should_write_the_logs_to_the_console()
        {
            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IInstallationLogger>().AssertWasCalled(x => x.WriteLogsToConsole(theEntries));
        }

        [Test]
        public void should_write_the_logs_to_file()
        {
            theInput.OpenFlag = false;

            ClassUnderTest.RunTheInstallation(theInput);

            MockFor<IInstallationLogger>().AssertWasCalled(x => x.WriteLogsToFile(theInput, theEntries));
        }

    }
}