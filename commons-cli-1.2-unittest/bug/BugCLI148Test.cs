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

using org.apache.commons.cli;

namespace org.apache.commons.cli.bug
{

    /**
     * http://issues.apache.org/jira/browse/CLI-148
     *
     * @author brianegge
     */
    [TestClass]
    public class BugCLI148Test
    {

        [TestMethod]
        public void testWorkaround1()
        {
            Options options = buildCommandLineOptions();
            CommandLineParser parser = new PosixParser();
            string[] args = new string[] { "-t-something" };
            CommandLine commandLine;
            commandLine = parser.parse(options, args);
            Assert.AreEqual("-something", commandLine.getOptionValue('t'));
        }

        [TestMethod]
        public void testWorkaround2()
        {
            Options options = buildCommandLineOptions();
            CommandLineParser parser = new PosixParser();
            string[] args = new string[] { "-t", "\"-something\"" };
            CommandLine commandLine;
            commandLine = parser.parse(options, args);
            Assert.AreEqual("-something", commandLine.getOptionValue('t'));
        }

        private Options buildCommandLineOptions()
        {
            Option t = OptionBuilder.withArgName("t").hasArg().create('t');
            Option s = OptionBuilder.withArgName("s").hasArg().create('s');
            Options options = new Options();
            options.addOption(t);
            options.addOption(s);
            return options;
        }

    }
}