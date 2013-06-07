﻿#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Integration;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.FileTypes
{
    [TestClass]
    [DeploymentItem(@"Cleaning\FileTypes\Data\CSharp.cs", "Data")]
    public class CSharpTests
    {
        #region Setup

        private static CodeCleanupAvailabilityLogic _codeCleanupAvailabilityLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _codeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_codeCleanupAvailabilityLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\CSharp.cs");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesCSharp_EnablesForDocument()
        {
            Settings.Default.Cleaning_IncludeCSharp = true;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Make sure the document is the active document for the environment.
                var document = CleaningTestHelper.GetActivatedDocument(_projectItem);
                Assert.AreEqual(document, TestEnvironment.Package.IDE.ActiveDocument);

                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsTrue(_codeCleanupAvailabilityLogic.ShouldCleanup(document));

                // Get the CleanupActiveCodeCommand and confirm it is in the expected state.
                //Assert.IsTrue(CleanupActiveCodeCommand.Enabled);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesCSharp_EnablesForProjectItem()
        {
            Settings.Default.Cleaning_IncludeCSharp = true;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsTrue(_codeCleanupAvailabilityLogic.ShouldCleanup(_projectItem));

                // Get the CleanupSelectedCodeCommand and confirm it is in the expected state.
                //Assert.IsTrue(CleanupSelectedCodeCommand.Enabled);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesCSharp_DisablesForDocumentWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_IncludeCSharp = false;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Make sure the document is the active document for the environment.
                var document = CleaningTestHelper.GetActivatedDocument(_projectItem);
                Assert.AreEqual(document, TestEnvironment.Package.IDE.ActiveDocument);

                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsFalse(_codeCleanupAvailabilityLogic.ShouldCleanup(document));

                // Get the CleanupActiveCodeCommand and confirm it is in the expected state.
                //Assert.IsFalse(CleanupActiveCodeCommand.Enabled);
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningFileTypesCSharp_DisablesForProjectItemWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_IncludeCSharp = false;

            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Confirm the code cleanup availability logic is in the expected state.
                Assert.IsFalse(_codeCleanupAvailabilityLogic.ShouldCleanup(_projectItem));

                // Get the CleanupSelectedCodeCommand and confirm it is in the expected state.
                //Assert.IsFalse(CleanupSelectedCodeCommand.Enabled);
            }));
        }

        #endregion Tests

        #region Helpers

        /// <summary>
        /// Gets the cleanup active code command.
        /// </summary>
        private static MenuCommand CleanupActiveCodeCommand
        {
            get
            {
                var cleanupActiveCodeCommandID = new CommandID(GuidList.GuidCodeMaidCommandCleanupActiveCode, (int)PkgCmdIDList.CmdIDCodeMaidCleanupActiveCode);
                var cleanupActiveCodeCommand = TestEnvironment.GetPackageCommand(cleanupActiveCodeCommandID);

                return cleanupActiveCodeCommand;
            }
        }

        /// <summary>
        /// Gets the cleanup selected code command.
        /// </summary>
        private static MenuCommand CleanupSelectedCodeCommand
        {
            get
            {
                var cleanupSelectedCodeCommandID = new CommandID(GuidList.GuidCodeMaidCommandCleanupSelectedCode, (int)PkgCmdIDList.CmdIDCodeMaidCleanupSelectedCode);
                var cleanupSelectedCodeCommand = TestEnvironment.GetPackageCommand(cleanupSelectedCodeCommandID);

                return cleanupSelectedCodeCommand;
            }
        }

        #endregion Helpers
    }
}