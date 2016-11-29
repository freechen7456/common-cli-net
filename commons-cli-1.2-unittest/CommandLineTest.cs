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
     * @author Emmanuel Bourg
     * @version $Revision: 667595 $, $Date: 2008-06-13 10:03:31 -0700 (Fri, 13 Jun 2008) $
     */
    [TestClass]
    public class CommandLineTest
    {
        [TestMethod]
        public void testGetOptionProperties()
        {
            string[] args = new string[] { "-Dparam1=value1", "-Dparam2=value2", "-Dparam3", "-Dparam4=value4", "-D", "--property", "foo=bar" };

            Options options = new Options();
            options.addOption(OptionBuilder.withValueSeparator().hasOptionalArgs(2).create('D'));
            options.addOption(OptionBuilder.withValueSeparator().hasArgs(2).withLongOpt("property").create());

            Parser parser = new GnuParser();
            CommandLine cl = parser.parse(options, args);

            Dictionary<string, string> props = cl.getOptionProperties("D");
            Assert.IsNotNull(props, "null properties");
            Assert.AreEqual(4, props.Count, "number of properties in " + props);
            Assert.AreEqual("value1", props["param1"], "property 1");
            Assert.AreEqual("value2", props["param2"], "property 2");
            Assert.AreEqual("true", props["param3"], "property 3");
            Assert.AreEqual("value4", props["param4"], "property 4");

            Assert.AreEqual("bar", cl.getOptionProperties("property")["foo"], "property with long format");
        }
    }
}