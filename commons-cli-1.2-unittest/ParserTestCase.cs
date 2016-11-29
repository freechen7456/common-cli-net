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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace org.apache.commons.cli
{

    /**
     * Abstract test case testing common parser features.
     *
     * @author Emmanuel Bourg
     * @version $Revision: 695672 $, $Date: 2008-09-15 15:19:11 -0700 (Mon, 15 Sep 2008) $
     */
    [TestClass]
    public abstract class ParserTestCase
    {
        protected Parser parser;

        protected Options options;

        [TestInitialize]
        public void setUp()
        {
            options = new Options()
                .addOption("a", "enable-a", false, "turn [a] on or off")
                .addOption("b", "bfile", true, "set the value of [b]")
                .addOption("c", "copt", false, "turn [c] on or off");
        }

        [TestMethod]
        public void testSimpleShort()
        {
            string[] args = new string[] { "-a",
                                       "-b", "toast",
                                       "foo", "bar" };

            CommandLine cl = parser.parse(options, args);

            Assert.IsTrue(cl.hasOption("a"), "Confirm -a is set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("toast"), "Confirm arg of -b");
            Assert.IsTrue(cl.getArgList().Count == 2, "Confirm size of extra args");
        }

        [TestMethod]
        public void testSimpleLong()
        {
            string[] args = new string[] { "--enable-a",
                                       "--bfile", "toast",
                                       "foo", "bar" };

            CommandLine cl = parser.parse(options, args);

            Assert.IsTrue(cl.hasOption("a"), "Confirm -a is set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("toast"), "Confirm arg of -b");
            Assert.IsTrue(cl.getOptionValue("bfile").Equals("toast"), "Confirm arg of --bfile");
            Assert.IsTrue(cl.getArgList().Count == 2, "Confirm size of extra args");
        }

        [TestMethod]
        public void testMultiple()
        {
            string[] args = new string[] { "-c",
                                       "foobar",
                                       "-b", "toast" };

            CommandLine cl = parser.parse(options, args, true);
            Assert.IsTrue(cl.hasOption("c"), "Confirm -c is set");
            Assert.IsTrue(cl.getArgList().Count == 3, "Confirm  3 extra args: " + cl.getArgList().Count);

            cl = parser.parse(options, cl.getArgs());

            Assert.IsTrue(!cl.hasOption("c"), "Confirm -c is not set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("toast"), "Confirm arg of -b");
            Assert.IsTrue(cl.getArgList().Count == 1, "Confirm  1 extra arg: " + cl.getArgList().Count);
            Assert.IsTrue(cl.getArgList()[0].Equals("foobar"), "Confirm  value of extra arg: " + cl.getArgList()[0]);
        }

        [TestMethod]
        public void testMultipleWithLong()
        {
            string[] args = new string[] { "--copt",
                                       "foobar",
                                       "--bfile", "toast" };

            CommandLine cl = parser.parse(options, args, true);
            Assert.IsTrue(cl.hasOption("c"), "Confirm -c is set");
            Assert.IsTrue(cl.getArgList().Count == 3, "Confirm  3 extra args: " + cl.getArgList().Count);

            cl = parser.parse(options, cl.getArgs());

            Assert.IsTrue(!cl.hasOption("c"), "Confirm -c is not set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("toast"), "Confirm arg of -b");
            Assert.IsTrue(cl.getArgList().Count == 1, "Confirm  1 extra arg: " + cl.getArgList().Count);
            Assert.IsTrue(cl.getArgList()[0].Equals("foobar"), "Confirm  value of extra arg: " + cl.getArgList()[0]);
        }

        [TestMethod]
        public void testUnrecognizedOption()
        {
            string[] args = new string[] { "-a", "-d", "-b", "toast", "foo", "bar" };

            try
            {
                parser.parse(options, args);
                Assert.Fail("UnrecognizedOptionException wasn't thrown");
            }
            catch (UnrecognizedOptionException e)
            {
                Assert.AreEqual("-d", e.getOption());
            }
        }

        [TestMethod]
        public void testMissingArg()
        {
            string[] args = new string[] { "-b" };

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
        public void testDoubleDash()
        {
            string[] args = new string[] { "--copt",
                                       "--",
                                       "-b", "toast" };

            CommandLine cl = parser.parse(options, args);

            Assert.IsTrue(cl.hasOption("c"), "Confirm -c is set");
            Assert.IsTrue(!cl.hasOption("b"), "Confirm -b is not set");
            Assert.IsTrue(cl.getArgList().Count == 2, "Confirm 2 extra args: " + cl.getArgList().Count);
        }

        [TestMethod]
        public void testSingleDash()
        {
            string[] args = new string[] { "--copt",
                                       "-b", "-",
                                       "-a",
                                       "-" };

            CommandLine cl = parser.parse(options, args);

            Assert.IsTrue(cl.hasOption("a"), "Confirm -a is set");
            Assert.IsTrue(cl.hasOption("b"), "Confirm -b is set");
            Assert.IsTrue(cl.getOptionValue("b").Equals("-"), "Confirm arg of -b");
            Assert.IsTrue(cl.getArgList().Count == 1, "Confirm 1 extra arg: " + cl.getArgList().Count);
            Assert.IsTrue(cl.getArgList()[0].Equals("-"), "Confirm value of extra arg: " + cl.getArgList()[0]);
        }

        [TestMethod]
        public void testStopAtUnexpectedArg()
        {
            string[] args = new string[] { "-c",
                                       "foober",
                                       "-b",
                                       "toast" };

            CommandLine cl = parser.parse(options, args, true);
            Assert.IsTrue(cl.hasOption("c"), "Confirm -c is set");
            Assert.IsTrue(cl.getArgList().Count == 3, "Confirm  3 extra args: " + cl.getArgList().Count);
        }

        [TestMethod]
        public void testStopAtExpectedArg()
        {
            string[] args = new string[] { "-b", "foo" };

            CommandLine cl = parser.parse(options, args, true);

            Assert.IsTrue(cl.hasOption('b'), "Confirm -b is set");
            Assert.AreEqual("foo", cl.getOptionValue('b'), "Confirm -b is set");
            Assert.IsTrue(cl.getArgList().Count == 0, "Confirm no extra args: " + cl.getArgList().Count);
        }

        [TestMethod]
        public void testStopAtNonOptionShort()
        {
            string[] args = new string[]{"-z",
                                     "-a",
                                     "-btoast"};

            CommandLine cl = parser.parse(options, args, true);
            Assert.IsFalse(cl.hasOption("a"), "Confirm -a is not set");
            Assert.IsTrue(cl.getArgList().Count == 3, "Confirm  3 extra args: " + cl.getArgList().Count);
        }

        [TestMethod]
        public void testStopAtNonOptionLong()
        {
            string[] args = new string[]{"--zop==1",
                                     "-abtoast",
                                     "--b=bar"};

            CommandLine cl = parser.parse(options, args, true);

            Assert.IsFalse(cl.hasOption("a"), "Confirm -a is not set");
            Assert.IsFalse(cl.hasOption("b"), "Confirm -b is not set");
            Assert.IsTrue(cl.getArgList().Count == 3, "Confirm  3 extra args: " + cl.getArgList().Count);
        }

        [TestMethod]
        public void testNegativeArgument()
        {
            string[] args = new string[] { "-b", "-1" };

            CommandLine cl = parser.parse(options, args);
            Assert.AreEqual("-1", cl.getOptionValue("b"));
        }

        [TestMethod]
        public void testArgumentStartingWithHyphen()
        {
            string[] args = new string[] { "-b", "-foo" };

            CommandLine cl = parser.parse(options, args);
            Assert.AreEqual("-foo", cl.getOptionValue("b"));
        }

        [TestMethod]
        public void testShortWithEqual()
        {
            string[] args = new string[] { "-f=bar" };

            Options options = new Options();
            options.addOption(OptionBuilder.withLongOpt("foo").hasArg().create('f'));

            CommandLine cl = parser.parse(options, args);

            Assert.AreEqual("bar", cl.getOptionValue("foo"));
        }

        [TestMethod]
        public void testShortWithoutEqual()
        {
            string[] args = new string[] { "-fbar" };

            Options options = new Options();
            options.addOption(OptionBuilder.withLongOpt("foo").hasArg().create('f'));

            CommandLine cl = parser.parse(options, args);

            Assert.AreEqual("bar", cl.getOptionValue("foo"));
        }

        [TestMethod]
        public void testLongWithEqual()
        {
            string[] args = new string[] { "--foo=bar" };

            Options options = new Options();
            options.addOption(OptionBuilder.withLongOpt("foo").hasArg().create('f'));

            CommandLine cl = parser.parse(options, args);

            Assert.AreEqual("bar", cl.getOptionValue("foo"));
        }

        [TestMethod]
        public void testLongWithEqualSingleDash()
        {
            string[] args = new string[] { "-foo=bar" };

            Options options = new Options();
            options.addOption(OptionBuilder.withLongOpt("foo").hasArg().create('f'));

            CommandLine cl = parser.parse(options, args);

            Assert.AreEqual("bar", cl.getOptionValue("foo"));
        }

        [TestMethod]
        public void testPropertiesOption()
        {
            string[] args = new string[] { "-Jsource=1.5", "-J", "target", "1.5", "foo" };

            Options options = new Options();
            options.addOption(OptionBuilder.withValueSeparator().hasArgs(2).create('J'));

            CommandLine cl = parser.parse(options, args);

            IList<string> values = new List<string>(cl.getOptionValues("J"));
            Assert.IsNotNull(values, "null values");
            Assert.AreEqual(4, values.Count, "number of values");
            Assert.AreEqual("source", values[0], "value 1");
            Assert.AreEqual("1.5", values[1], "value 2");
            Assert.AreEqual("target", values[2], "value 3");
            Assert.AreEqual("1.5", values[3], "value 4");
            IList<string> argsleft = cl.getArgList();
            Assert.AreEqual(1, argsleft.Count, "Should be 1 arg left");
            Assert.AreEqual("foo", argsleft[0], "Expecting foo");
        }
    }
}