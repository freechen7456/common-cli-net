/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace org.apache.commons.cli
{

    /**
     * Test case for the PosixParser.
     *
     * @version $Revision: 695410 $, $Date: 2008-09-15 03:25:38 -0700 (Mon, 15 Sep 2008) $
     */
    [TestClass]
    public class PosixParserTest : ParserTestCase
    {
        [TestInitialize]
        public new void setUp()
        {
            base.setUp();
            parser = new PosixParser();
        }

        [TestMethod]
        public void testBursting()
        {
            string[] args = new string[] { "-acbtoast",
                                       "foo", "bar" };

            CommandLine cl = parser.parse(options, args);

            Assert.IsTrue(cl.hasOption("a"), "Confirm -a is set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.hasOption("c"), "Confirm -c is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("toast"), "Confirm arg of -b");
            Assert.IsTrue(cl.getArgList().Count == 2, "Confirm size of extra args");
        }

        [TestMethod]
        public void testUnrecognizedOptionWithBursting()
        {
            string[] args = new string[] { "-adbtoast", "foo", "bar" };

            try
            {
                parser.parse(options, args);
                Assert.Fail("UnrecognizedOptionException wasn't thrown");
            }
            catch (UnrecognizedOptionException e)
            {
                Assert.AreEqual("-adbtoast", e.getOption());
            }
        }

        [TestMethod]
        public void testMissingArgWithBursting()
        {
            string[] args = new string[] { "-acb" };

            bool caught = false;

            try
            {
                parser.parse(options, args);
            }
            catch (MissingArgumentException e)
            {
                caught = true;
                Assert.AreEqual("b", e.getOption().getOpt(), "option missing an argument");
            }

            Assert.IsTrue(caught, "Confirm MissingArgumentException caught");
        }

        [TestMethod]
        public void testStopBursting()
        {
            string[] args = new string[] { "-azc" };

            CommandLine cl = parser.parse(options, args, true);
            Assert.IsTrue(cl.hasOption("a"), "Confirm -a is set");
            Assert.IsFalse(cl.hasOption("c"), "Confirm -c is not set");

            Assert.IsTrue(cl.getArgList().Count == 1, "Confirm  1 extra arg: " + cl.getArgList().Count);
            Assert.IsTrue(cl.getArgList().Contains("zc"));
        }

        [TestMethod]
        public void testStopBursting2()
        {
            string[] args = new string[] { "-c",
                                       "foobar",
                                       "-btoast" };

            CommandLine cl = parser.parse(options, args, true);
            Assert.IsTrue(cl.hasOption("c"), "Confirm -c is set");
            Assert.IsTrue(cl.getArgList().Count == 2, "Confirm  2 extra args: " + cl.getArgList().Count);

            cl = parser.parse(options, cl.getArgs());

            Assert.IsTrue(!cl.hasOption("c"), "Confirm -c is not set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("toast"), "Confirm arg of -b");
            Assert.IsTrue(cl.getArgList().Count == 1, "Confirm  1 extra arg: " + cl.getArgList().Count);
            Assert.IsTrue(cl.getArgList()[0].Equals("foobar"), "Confirm  value of extra arg: " + cl.getArgList()[0]);
        }

        /**
         * Real world test with long and short options.
         */
        [TestMethod]
        public void testLongOptionWithShort()
        {
            Option help = new Option("h", "help", false, "print this message");
            Option version = new Option("v", "version", false, "print version information");
            Option newRun = new Option("n", "new", false, "Create NLT cache entries only for new items");
            Option trackerRun = new Option("t", "tracker", false, "Create NLT cache entries only for tracker items");

            Option timeLimit = OptionBuilder.withLongOpt("limit").hasArg()
                                            .withValueSeparator()
                                            .withDescription("Set time limit for execution, in minutes")
                                            .create("l");

            Option age = OptionBuilder.withLongOpt("age").hasArg()
                                      .withValueSeparator()
                                      .withDescription("Age (in days) of cache item before being recomputed")
                                      .create("a");

            Option server = OptionBuilder.withLongOpt("server").hasArg()
                                         .withValueSeparator()
                                         .withDescription("The NLT server address")
                                         .create("s");

            Option numResults = OptionBuilder.withLongOpt("results").hasArg()
                                             .withValueSeparator()
                                             .withDescription("Number of results per item")
                                             .create("r");

            Option configFile = OptionBuilder.withLongOpt("file").hasArg()
                                             .withValueSeparator()
                                             .withDescription("Use the specified configuration file")
                                             .create();

            Options options = new Options();
            options.addOption(help);
            options.addOption(version);
            options.addOption(newRun);
            options.addOption(trackerRun);
            options.addOption(timeLimit);
            options.addOption(age);
            options.addOption(server);
            options.addOption(numResults);
            options.addOption(configFile);

            // create the command line parser
            CommandLineParser parser = new PosixParser();

            string[] args = new string[] {
                "-v",
                "-l",
                "10",
                "-age",
                "5",
                "-file",
                "filename"
            };

            CommandLine line = parser.parse(options, args);
            Assert.IsTrue(line.hasOption("v"));
            Assert.AreEqual(line.getOptionValue("l"), "10");
            Assert.AreEqual(line.getOptionValue("limit"), "10");
            Assert.AreEqual(line.getOptionValue("a"), "5");
            Assert.AreEqual(line.getOptionValue("age"), "5");
            Assert.AreEqual(line.getOptionValue("file"), "filename");
        }

        [TestMethod]
        public new void testLongWithEqualSingleDash()
        {
            // not supported by the PosixParser
        }

        [TestMethod]
        public new void testShortWithEqual()
        {
            // not supported by the PosixParser
        }
    }
}