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

    [TestClass]
    public class ValuesTest
    {
        /** CommandLine instance */
        private CommandLine _cmdline = null;

        [TestInitialize]
        public void setUp()
        {
            Options options = new Options();

            options.addOption("a", false, "toggle -a");
            options.addOption("b", true, "set -b");
            options.addOption("c", "c", false, "toggle -c");
            options.addOption("d", "d", true, "set -d");

            options.addOption(OptionBuilder.withLongOpt("e").hasArgs().withDescription("set -e ").create('e'));
            options.addOption("f", "f", false, "jk");
            options.addOption(OptionBuilder.withLongOpt("g").hasArgs(2).withDescription("set -g").create('g'));
            options.addOption(OptionBuilder.withLongOpt("h").hasArgs(2).withDescription("set -h").create('h'));
            options.addOption(OptionBuilder.withLongOpt("i").withDescription("set -i").create('i'));
            options.addOption(OptionBuilder.withLongOpt("j").hasArgs().withDescription("set -j").withValueSeparator('=').create('j'));
            options.addOption(OptionBuilder.withLongOpt("k").hasArgs().withDescription("set -k").withValueSeparator('=').create('k'));
            options.addOption(OptionBuilder.withLongOpt("m").hasArgs().withDescription("set -m").withValueSeparator().create('m'));

            String[] args = new String[] { "-a",
                                       "-b", "foo",
                                       "--c",
                                       "--d", "bar",
                                       "-e", "one", "two",
                                       "-f",
                                       "arg1", "arg2",
                                       "-g", "val1", "val2" , "arg3",
                                       "-h", "val1", "-i",
                                       "-h", "val2",
                                       "-jkey=value",
                                       "-j", "key=value",
                                       "-kkey1=value1", 
                                       "-kkey2=value2",
                                       "-mkey=value"};

            CommandLineParser parser = new PosixParser();

            _cmdline = parser.parse(options, args);
        }

        [TestMethod]
        public void testShortArgs()
        {
            Assert.IsTrue(_cmdline.hasOption("a"));
            Assert.IsTrue(_cmdline.hasOption("c"));

            Assert.IsNull(_cmdline.getOptionValues("a"));
            Assert.IsNull(_cmdline.getOptionValues("c"));
        }

        [TestMethod]
        public void testShortArgsWithValue()
        {
            Assert.IsTrue(_cmdline.hasOption("b"));
            Assert.IsTrue(_cmdline.getOptionValue("b").Equals("foo"));
            Assert.AreEqual(1, _cmdline.getOptionValues("b").Length);

            Assert.IsTrue(_cmdline.hasOption("d"));
            Assert.IsTrue(_cmdline.getOptionValue("d").Equals("bar"));
            Assert.AreEqual(1, _cmdline.getOptionValues("d").Length);
        }

        [TestMethod]
        public void testMultipleArgValues()
        {
            String[] result = _cmdline.getOptionValues("e");
            String[] values = new String[] { "one", "two" };
            Assert.IsTrue(_cmdline.hasOption("e"));
            Assert.AreEqual(2, _cmdline.getOptionValues("e").Length);
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues("e"));
        }

        [TestMethod]
        public void testTwoArgValues()
        {
            String[] result = _cmdline.getOptionValues("g");
            String[] values = new String[] { "val1", "val2" };
            Assert.IsTrue(_cmdline.hasOption("g"));
            Assert.AreEqual(2, _cmdline.getOptionValues("g").Length);
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues("g"));
        }

        [TestMethod]
        public void testComplexValues()
        {
            String[] result = _cmdline.getOptionValues("h");
            String[] values = new String[] { "val1", "val2" };
            Assert.IsTrue(_cmdline.hasOption("i"));
            Assert.IsTrue(_cmdline.hasOption("h"));
            Assert.AreEqual(2, _cmdline.getOptionValues("h").Length);
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues("h"));
        }

        [TestMethod]
        public void testExtraArgs()
        {
            String[] args = new String[] { "arg1", "arg2", "arg3" };
            Assert.AreEqual(3, _cmdline.getArgs().Length);
            CollectionAssert.AreEqual(args, _cmdline.getArgs());
        }

        [TestMethod]
        public void testCharSeparator()
        {
            // tests the char methods of CommandLine that delegate to
            // the String methods
            String[] values = new String[] { "key", "value", "key", "value" };
            Assert.IsTrue(_cmdline.hasOption("j"));
            Assert.IsTrue(_cmdline.hasOption('j'));
            Assert.AreEqual(4, _cmdline.getOptionValues("j").Length);
            Assert.AreEqual(4, _cmdline.getOptionValues('j').Length);
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues("j"));
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues('j'));

            values = new String[] { "key1", "value1", "key2", "value2" };
            Assert.IsTrue(_cmdline.hasOption("k"));
            Assert.IsTrue(_cmdline.hasOption('k'));
            Assert.AreEqual(4, _cmdline.getOptionValues("k").Length);
            Assert.AreEqual(4, _cmdline.getOptionValues('k').Length);
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues("k"));
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues('k'));

            values = new String[] { "key", "value" };
            Assert.IsTrue(_cmdline.hasOption("m"));
            Assert.IsTrue(_cmdline.hasOption('m'));
            Assert.AreEqual(2, _cmdline.getOptionValues("m").Length);
            Assert.AreEqual(2, _cmdline.getOptionValues('m').Length);
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues("m"));
            CollectionAssert.AreEqual(values, _cmdline.getOptionValues('m'));
        }

        /**
         * jkeyes - commented out this test as the new architecture
         * breaks this type of functionality.  I have left the test 
         * here in case I get a brainwave on how to resolve this.
         */
        /*
        public void testGetValue()
        {
            // the 'm' option
            Assert.IsTrue( _option.getValues().length == 2 );
            Assert.AreEqual( _option.getValue(), "key" );
            Assert.AreEqual( _option.getValue( 0 ), "key" );
            Assert.AreEqual( _option.getValue( 1 ), "value" );

            try {
                Assert.AreEqual( _option.getValue( 2 ), "key" );
                fail( "IndexOutOfBounds not caught" );
            }
            catch( IndexOutOfBoundsException exp ) {
            
            }

            try {
                Assert.AreEqual( _option.getValue( -1 ), "key" );
                fail( "IndexOutOfBounds not caught" );
            }
            catch( IndexOutOfBoundsException exp ) {

            }
        }
        */
    }
}