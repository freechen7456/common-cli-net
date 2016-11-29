/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using org.apache.commons.cli;

namespace org.apache.commons.cli.bug
{

    [TestClass]
    public class BugCLI162Test
    {
        /** Constant for the line separator.*/
        private static readonly string CR = Environment.NewLine;

        [TestMethod]
        public void testInfiniteLoop()
        {
            Options options = new Options();
            options.addOption("h", "help", false, "This is a looooong description");
            HelpFormatter formatter = new HelpFormatter();
            formatter.setWidth(20);
            formatter.printHelp("app", options); // used to hang & crash
        }

        [TestMethod]
        public void testPrintHelpLongLines()
        {
            // Constants used for options
            const string OPT = "-";

            const string OPT_COLUMN_NAMES = "l";

            const string OPT_CONNECTION = "c";

            const string OPT_DESCRIPTION = "e";

            const string OPT_DRIVER = "d";

            const string OPT_DRIVER_INFO = "n";

            const string OPT_FILE_BINDING = "b";

            const string OPT_FILE_JDBC = "j";

            const string OPT_FILE_SFMD = "f";

            const string OPT_HELP = "h";

            const string OPT_HELP_ = "help";

            const string OPT_INTERACTIVE = "i";

            const string OPT_JDBC_TO_SFMD = "2";

            const string OPT_JDBC_TO_SFMD_L = "jdbc2sfmd";

            const string OPT_METADATA = "m";

            const string OPT_PARAM_MODES_INT = "o";

            const string OPT_PARAM_MODES_NAME = "O";

            const string OPT_PARAM_NAMES = "a";

            const string OPT_PARAM_TYPES_INT = "y";

            const string OPT_PARAM_TYPES_NAME = "Y";

            const string OPT_PASSWORD = "p";

            const string OPT_PASSWORD_L = "password";

            const string OPT_SQL = "s";

            const string OPT_SQL_L = "sql";

            const string OPT_SQL_SPLIT_DEFAULT = "###";

            const string OPT_SQL_SPLIT_L = "splitSql";

            const string OPT_STACK_TRACE = "t";

            const string OPT_TIMING = "g";

            const string OPT_TRIM_L = "trim";

            const string OPT_USER = "u";

            const string OPT_WRITE_TO_FILE = "w";

            const string _PMODE_IN = "IN";

            const string _PMODE_INOUT = "INOUT";

            const string _PMODE_OUT = "OUT";

            const string _PMODE_UNK = "Unknown";

            const string PMODES = _PMODE_IN + ", " + _PMODE_INOUT + ", " + _PMODE_OUT + ", " + _PMODE_UNK;

            // Options build
            Options commandLineOptions;
            commandLineOptions = new Options();
            commandLineOptions.addOption(OPT_HELP, OPT_HELP_, false, "Prints help and quits");
            commandLineOptions.addOption(OPT_DRIVER, "driver", true, "JDBC driver class name");
            commandLineOptions.addOption(OPT_DRIVER_INFO, "info", false, "Prints driver information and properties. If "
                + OPT
                + OPT_CONNECTION
                + " is not specified, all drivers on the classpath are displayed.");
            commandLineOptions.addOption(OPT_CONNECTION, "url", true, "Connection URL");
            commandLineOptions.addOption(OPT_USER, "user", true, "A database user name");
            commandLineOptions
                    .addOption(
                            OPT_PASSWORD,
                            OPT_PASSWORD_L,
                            true,
                            "The database password for the user specified with the "
                                + OPT
                                + OPT_USER
                                + " option. You can obfuscate the password with org.mortbay.jetty.security.Password, see http://docs.codehaus.org/display/JETTY/Securing+Passwords");
            commandLineOptions.addOption(OPT_SQL, OPT_SQL_L, true, "Runs SQL or {call stored_procedure(?, ?)} or {?=call function(?, ?)}");
            commandLineOptions.addOption(OPT_FILE_SFMD, "sfmd", true, "Writes a SFMD file for the given SQL");
            commandLineOptions.addOption(OPT_FILE_BINDING, "jdbc", true, "Writes a JDBC binding node file for the given SQL");
            commandLineOptions.addOption(OPT_FILE_JDBC, "node", true, "Writes a JDBC node file for the given SQL (internal debugging)");
            commandLineOptions.addOption(OPT_WRITE_TO_FILE, "outfile", true, "Writes the SQL output to the given file");
            commandLineOptions.addOption(OPT_DESCRIPTION, "description", true,
                    "SFMD description. A default description is used if omited. Example: " + OPT + OPT_DESCRIPTION + " \"Runs such and such\"");
            commandLineOptions.addOption(OPT_INTERACTIVE, "interactive", false,
                    "Runs in interactive mode, reading and writing from the console, 'go' or '/' sends a statement");
            commandLineOptions.addOption(OPT_TIMING, "printTiming", false, "Prints timing information");
            commandLineOptions.addOption(OPT_METADATA, "printMetaData", false, "Prints metadata information");
            commandLineOptions.addOption(OPT_STACK_TRACE, "printStack", false, "Prints stack traces on errors");
            Option option = new Option(OPT_COLUMN_NAMES, "columnNames", true, "Column XML names; default names column labels. Example: "
                + OPT
                + OPT_COLUMN_NAMES
                + " \"cname1 cname2\"");
            commandLineOptions.addOption(option);
            option = new Option(OPT_PARAM_NAMES, "paramNames", true, "Parameter XML names; default names are param1, param2, etc. Example: "
                + OPT
                + OPT_PARAM_NAMES
                + " \"pname1 pname2\"");
            commandLineOptions.addOption(option);
            //
            OptionGroup pOutTypesOptionGroup = new OptionGroup();
            string pOutTypesOptionGroupDoc = OPT + OPT_PARAM_TYPES_INT + " and " + OPT + OPT_PARAM_TYPES_NAME + " are mutually exclusive.";
            string typesClassName = typeof(System.Data.DbType).Name;
            option = new Option(OPT_PARAM_TYPES_INT, "paramTypes", true, "Parameter types from "
                + typesClassName
                + ". "
                + pOutTypesOptionGroupDoc
                + " Example: "
                + OPT
                + OPT_PARAM_TYPES_INT
                + " \"-10 12\"");
            commandLineOptions.addOption(option);
            option = new Option(OPT_PARAM_TYPES_NAME, "paramTypeNames", true, "Parameter "
                + typesClassName
                + " names. "
                + pOutTypesOptionGroupDoc
                + " Example: "
                + OPT
                + OPT_PARAM_TYPES_NAME
                + " \"CURSOR VARCHAR\"");
            commandLineOptions.addOption(option);
            commandLineOptions.addOptionGroup(pOutTypesOptionGroup);
            //
            OptionGroup modesOptionGroup = new OptionGroup();
            string modesOptionGroupDoc = OPT + OPT_PARAM_MODES_INT + " and " + OPT + OPT_PARAM_MODES_NAME + " are mutually exclusive.";
            option = new Option(OPT_PARAM_MODES_INT, "paramModes", true, "Parameters modes ("
                + System.Data.ParameterDirection.Input
                + "=IN, "
                + System.Data.ParameterDirection.InputOutput
                + "=INOUT, "
                + System.Data.ParameterDirection.Output
                + "=OUT, "
                + System.Data.ParameterDirection.ReturnValue
                + "=Unknown"
                + "). "
                + modesOptionGroupDoc
                + " Example for 2 parameters, OUT and IN: "
                + OPT
                + OPT_PARAM_MODES_INT
                + " \""
                + System.Data.ParameterDirection.Output
                + " "
                + System.Data.ParameterDirection.Input
                + "\"");
            modesOptionGroup.addOption(option);
            option = new Option(OPT_PARAM_MODES_NAME, "paramModeNames", true, "Parameters mode names ("
                + PMODES
                + "). "
                + modesOptionGroupDoc
                + " Example for 2 parameters, OUT and IN: "
                + OPT
                + OPT_PARAM_MODES_NAME
                + " \""
                + _PMODE_OUT
                + " "
                + _PMODE_IN
                + "\"");
            modesOptionGroup.addOption(option);
            commandLineOptions.addOptionGroup(modesOptionGroup);
            option = new Option(null, OPT_TRIM_L, true,
                    "Trims leading and trailing spaces from all column values. Column XML names can be optionally specified to set which columns to trim.");
            option.setOptionalArg(true);
            commandLineOptions.addOption(option);
            option = new Option(OPT_JDBC_TO_SFMD, OPT_JDBC_TO_SFMD_L, true,
                    "Converts the JDBC file in the first argument to an SMFD file specified in the second argument.");
            option.setArgs(2);
            commandLineOptions.addOption(option);
            new HelpFormatter().printHelp(this.GetType().Name, commandLineOptions);
        }

        [TestMethod]
        public void testLongLineChunking()
        {
            Options options = new Options();
            options.addOption("x", "extralongarg", false,
                                         "This description has ReallyLongValuesThatAreLongerThanTheWidthOfTheColumns " +
                                         "and also other ReallyLongValuesThatAreHugerAndBiggerThanTheWidthOfTheColumnsBob, " +
                                         "yes. ");
            HelpFormatter formatter = new HelpFormatter();
            StringWriter sw = new StringWriter();
            formatter.printHelp(sw, 35, this.GetType().FullName, "Header", options, 0, 5, "Footer");
            string expected = "usage:" + CR +
                              "       org.apache.commons.cli.bug.B" + CR +
                              "       ugCLI162Test" + CR +
                              "Header" + CR +
                              "-x,--extralongarg     This" + CR +
                              "                      description" + CR +
                              "                      has" + CR +
                              "                      ReallyLongVal" + CR +
                              "                      uesThatAreLon" + CR +
                              "                      gerThanTheWid" + CR +
                              "                      thOfTheColumn" + CR +
                              "                      s and also" + CR +
                              "                      other" + CR +
                              "                      ReallyLongVal" + CR +
                              "                      uesThatAreHug" + CR +
                              "                      erAndBiggerTh" + CR +
                              "                      anTheWidthOfT" + CR +
                              "                      heColumnsBob," + CR +
                              "                      yes." + CR +
                              "Footer" + CR;
            Assert.AreEqual(expected, sw.ToString(), "Long arguments did not split as expected");
        }

        [TestMethod]
        public void testLongLineChunkingIndentIgnored()
        {
            Options options = new Options();
            options.addOption("x", "extralongarg", false, "This description is Long.");
            HelpFormatter formatter = new HelpFormatter();
            StringWriter sw = new StringWriter();
            formatter.printHelp(sw, 22, this.GetType().FullName, "Header", options, 0, 5, "Footer");
            Console.Error.WriteLine(sw.ToString());
            string expected = "usage:" + CR +
                              "       org.apache.comm" + CR +
                              "       ons.cli.bug.Bug" + CR +
                              "       CLI162Test" + CR +
                              "Header" + CR +
                              "-x,--extralongarg" + CR +
                              " This description is" + CR +
                              " Long." + CR +
                              "Footer" + CR;
            Assert.AreEqual(expected, sw.ToString(), "Long arguments did not split as expected");
        }

    }
}