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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.apache.commons.cli;
using System.Collections.Generic;

namespace org.apache.commons.cli
{

    /**
     * @author brianegge
     */
    [TestClass]
    public class OptionTest
    {
        private class TestOption : Option
        {
            public TestOption(string opt, bool hasArg, string description)
                : base(opt, hasArg, description)
            {
            }

            public override bool addValue(string value)
            {
                addValueForProcessing(value);
                return true;
            }
        }

        [TestMethod]
        public void testClear()
        {
            TestOption option = new TestOption("x", true, "");
            Assert.AreEqual(0, option.getValuesList().Count);
            option.addValue("a");
            Assert.AreEqual(1, option.getValuesList().Count);
            option.clearValues();
            Assert.AreEqual(0, option.getValuesList().Count);
        }

        // See http://issues.apache.org/jira/browse/CLI-21
        [TestMethod]
        public void testClone()
        {
            TestOption a = new TestOption("a", true, "");
            TestOption b = (TestOption)a.clone();
            Assert.AreEqual(a, b);
            Assert.AreNotSame(a, b);
            a.setDescription("a");
            Assert.AreEqual("", b.getDescription());
            b.setArgs(2);
            b.addValue("b1");
            b.addValue("b2");
            Assert.AreEqual(1, a.getArgs());
            Assert.AreEqual(0, a.getValuesList().Count);
            Assert.AreEqual(2, b.getValues().Length);
        }

        private class DefaultOption : Option
        {
            private readonly string defaultValue;

            public DefaultOption(string opt, string description, string defaultValue)
                : base(opt, true, description)
            {
                this.defaultValue = defaultValue;
            }

            public override string getValue()
            {
                return base.getValue() != null ? base.getValue() : defaultValue;
            }
        }

        [TestMethod]
        public void testSubclass()
        {
            Option option = new DefaultOption("f", "file", "myfile.txt");
            Option clone = (Option)option.clone();
            Assert.AreEqual("myfile.txt", clone.getValue());
            Assert.AreEqual(typeof(DefaultOption), clone.GetType());
        }

        [TestMethod]
        public void testHasArgName()
        {
            Option option = new Option("f", null);

            option.setArgName(null);
            Assert.IsFalse(option.hasArgName());

            option.setArgName("");
            Assert.IsFalse(option.hasArgName());

            option.setArgName("file");
            Assert.IsTrue(option.hasArgName());
        }

        [TestMethod]
        public void testHasArgs()
        {
            Option option = new Option("f", null);

            option.setArgs(0);
            Assert.IsFalse(option.hasArgs());

            option.setArgs(1);
            Assert.IsFalse(option.hasArgs());

            option.setArgs(10);
            Assert.IsTrue(option.hasArgs());

            option.setArgs(Option.UNLIMITED_VALUES);
            Assert.IsTrue(option.hasArgs());

            option.setArgs(Option.UNINITIALIZED);
            Assert.IsFalse(option.hasArgs());
        }

        [TestMethod]
        public void testGetValue()
        {
            Option option = new Option("f", null);
            option.setArgs(Option.UNLIMITED_VALUES);

            Assert.AreEqual("default", option.getValue("default"));
            Assert.AreEqual(null, option.getValue(0));

            option.addValueForProcessing("foo");

            Assert.AreEqual("foo", option.getValue());
            Assert.AreEqual("foo", option.getValue(0));
            Assert.AreEqual("foo", option.getValue("default"));
        }
    }
}