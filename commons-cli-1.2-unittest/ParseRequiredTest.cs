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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace org.apache.commons.cli
{

    /**
     * @author John Keyes (john at integralsource.com)
     * @version $Revision: 678662 $
     */
    [TestClass]
    public class ParseRequiredTest
    {
        private Options _options = null;
        private CommandLineParser parser = new PosixParser();

        [TestInitialize]
        public void setUp()
        {
            _options = new Options()
                .addOption("a",
                           "enable-a",
                           false,
                           "turn [a] on or off")
                .addOption(OptionBuilder.withLongOpt("bfile")
                                         .hasArg()
                                         .isRequired()
                                         .withDescription("set the value of [b]")
                                         .create('b'));
        }

        [TestMethod]
        public void testWithRequiredOption()
        {
            string[] args = new string[] { "-b", "file" };

            CommandLine cl = parser.parse(_options, args);

            Assert.IsTrue( !cl.hasOption("a"),"Confirm -a is NOT set");
            Assert.IsTrue( cl.hasOption("b"),"Confirm -b is set");
            Assert.IsTrue( cl.getOptionValue("b").Equals("file"),"Confirm arg of -b");
            Assert.IsTrue( cl.getArgList().Count == 0,"Confirm NO of extra args");
        }

        [TestMethod]
        public void testOptionAndRequiredOption()
        {
            string[] args = new string[] { "-a", "-b", "file" };

            CommandLine cl = parser.parse(_options, args);

            Assert.IsTrue( cl.hasOption("a"),"Confirm -a is set");
            Assert.IsTrue( cl.hasOption("b"),"Confirm -b is set");
            Assert.IsTrue( cl.getOptionValue("b").Equals("file"),"Confirm arg of -b");
            Assert.IsTrue( cl.getArgList().Count == 0,"Confirm NO of extra args");
        }

        [TestMethod]
        public void testMissingRequiredOption()
        {
            string[] args = new string[] { "-a" };

            try
            {
                CommandLine cl = parser.parse(_options, args);
                Assert.Fail("exception should have been thrown");
            }
            catch (MissingOptionException e)
            {
                Assert.AreEqual( "Missing required option: b", e.Message,"Incorrect exception message");
                Assert.IsTrue(e.getMissingOptions().Contains("b"));
            }
            catch (ParseException)
            {
                Assert.Fail("expected to catch MissingOptionException");
            }
        }

        [TestMethod]
        public void testMissingRequiredOptions()
        {
            string[] args = new string[] { "-a" };

            _options.addOption(OptionBuilder.withLongOpt("cfile")
                                         .hasArg()
                                         .isRequired()
                                         .withDescription("set the value of [c]")
                                         .create('c'));

            try
            {
                CommandLine cl = parser.parse(_options, args);
                Assert.Fail("exception should have been thrown");
            }
            catch (MissingOptionException e)
            {
                Assert.AreEqual( "Missing required options: b, c", e.Message,"Incorrect exception message");
                Assert.IsTrue(e.getMissingOptions().Contains("b"));
                Assert.IsTrue(e.getMissingOptions().Contains("c"));
            }
            catch (ParseException)
            {
                Assert.Fail("expected to catch MissingOptionException");
            }
        }

        [TestMethod]
        public void testReuseOptionsTwice()
        {
            Options opts = new Options();
            opts.addOption(OptionBuilder.isRequired().create('v'));

            GnuParser parser = new GnuParser();

            // first parsing
            parser.parse(opts, new string[] { "-v" });

            try
            {
                // second parsing, with the same Options instance and an invalid command line
                parser.parse(opts, new string[0]);
                Assert.Fail("MissingOptionException not thrown");
            }
            catch (MissingOptionException)
            {
                // expected
            }
        }

    }
}