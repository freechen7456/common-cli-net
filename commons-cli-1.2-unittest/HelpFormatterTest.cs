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
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace org.apache.commons.cli
{

    /** 
     * Test case for the HelpFormatter class 
     *
     * @author Slawek Zachcial
     * @author John Keyes ( john at integralsource.com )
     * @author brianegge
     */
    [TestClass]
    public class HelpFormatterTest
    {
        private static readonly string EOL = Environment.NewLine;

        [TestMethod]
        public void testFindWrapPos()
        {
            HelpFormatter hf = new HelpFormatter();

            string text = "This is a test.";
            //text width should be max 8; the wrap position is 7
            Assert.AreEqual(7, hf.findWrapPos(text, 8, 0), "wrap position");
            //starting from 8 must give -1 - the wrap pos is after end
            Assert.AreEqual(-1, hf.findWrapPos(text, 8, 8), "wrap position 2");
            //if there is no a good position before width to make a wrapping look for the next one
            text = "aaaa aa";
            Assert.AreEqual(4, hf.findWrapPos(text, 3, 0), "wrap position 3");
        }

        [TestMethod]
        public void testPrintWrapped()
        {
            StringBuilder sb = new StringBuilder();
            HelpFormatter hf = new HelpFormatter();

            string text = "This is a test.";

            string expected = "This is a" + hf.getNewLine() + "test.";
            hf.renderWrappedText(sb, 12, 0, text);
            Assert.AreEqual(expected, sb.ToString(), "single line text");

            sb.setLength(0);
            expected = "This is a" + hf.getNewLine() + "    test.";
            hf.renderWrappedText(sb, 12, 4, text);
            Assert.AreEqual(expected, sb.ToString(), "single line padded text");

            text = "  -p,--period <PERIOD>  PERIOD is time duration of form " +
                   "DATE[-DATE] where DATE has form YYYY[MM[DD]]";

            sb.setLength(0);
            expected = "  -p,--period <PERIOD>  PERIOD is time duration of" +
                    hf.getNewLine() +
                    "                        form DATE[-DATE] where DATE" +
                    hf.getNewLine() +
                    "                        has form YYYY[MM[DD]]";
            hf.renderWrappedText(sb, 53, 24, text);
            Assert.AreEqual(expected, sb.ToString(), "single line padded text 2");

            text = "aaaa aaaa aaaa" + hf.getNewLine() +
                   "aaaaaa" + hf.getNewLine() +
                   "aaaaa";

            expected = text;
            sb.setLength(0);
            hf.renderWrappedText(sb, 16, 0, text);
            Assert.AreEqual(expected, sb.ToString(), "multi line text");

            expected = "aaaa aaaa aaaa" + hf.getNewLine() +
                       "    aaaaaa" + hf.getNewLine() +
                       "    aaaaa";
            sb.setLength(0);
            hf.renderWrappedText(sb, 16, 4, text);
            Assert.AreEqual(expected, sb.ToString(), "multi-line padded text");
        }

        [TestMethod]
        public void testPrintOptions()
        {
            StringBuilder sb = new StringBuilder();
            HelpFormatter hf = new HelpFormatter();
            int leftPad = 1;
            int descPad = 3;
            string lpad = hf.createPadding(leftPad);
            string dpad = hf.createPadding(descPad);
            Options options = null;
            string expected = null;

            options = new Options().addOption("a", false, "aaaa aaaa aaaa aaaa aaaa");
            expected = lpad + "-a" + dpad + "aaaa aaaa aaaa aaaa aaaa";
            hf.renderOptions(sb, 60, options, leftPad, descPad);
            Assert.AreEqual(expected, sb.ToString(), "simple non-wrapped option");

            int nextLineTabStop = leftPad + descPad + "-a".Length;
            expected = lpad + "-a" + dpad + "aaaa aaaa aaaa" + hf.getNewLine() +
                       hf.createPadding(nextLineTabStop) + "aaaa aaaa";
            sb.setLength(0);
            hf.renderOptions(sb, nextLineTabStop + 17, options, leftPad, descPad);
            Assert.AreEqual(expected, sb.ToString(), "simple wrapped option");


            options = new Options().addOption("a", "aaa", false, "dddd dddd dddd dddd");
            expected = lpad + "-a,--aaa" + dpad + "dddd dddd dddd dddd";
            sb.setLength(0);
            hf.renderOptions(sb, 60, options, leftPad, descPad);
            Assert.AreEqual(expected, sb.ToString(), "long non-wrapped option");

            nextLineTabStop = leftPad + descPad + "-a,--aaa".Length;
            expected = lpad + "-a,--aaa" + dpad + "dddd dddd" + hf.getNewLine() +
                       hf.createPadding(nextLineTabStop) + "dddd dddd";
            sb.setLength(0);
            hf.renderOptions(sb, 25, options, leftPad, descPad);
            Assert.AreEqual(expected, sb.ToString(), "long wrapped option");

            options = new Options().
                    addOption("a", "aaa", false, "dddd dddd dddd dddd").
                    addOption("b", false, "feeee eeee eeee eeee");
            expected = lpad + "-a,--aaa" + dpad + "dddd dddd" + hf.getNewLine() +
                       hf.createPadding(nextLineTabStop) + "dddd dddd" + hf.getNewLine() +
                       lpad + "-b      " + dpad + "feeee eeee" + hf.getNewLine() +
                       hf.createPadding(nextLineTabStop) + "eeee eeee";
            sb.setLength(0);
            hf.renderOptions(sb, 25, options, leftPad, descPad);
            Assert.AreEqual(expected, sb.ToString(), "multiple wrapped options");
        }

        [TestMethod]
        public void testPrintHelpWithEmptySyntax()
        {
            HelpFormatter formatter = new HelpFormatter();
            try
            {
                formatter.printHelp(null, new Options());
                Assert.Fail("null command line syntax should be rejected");
            }
            catch (ArgumentException)
            {
                // expected
            }

            try
            {
                formatter.printHelp("", new Options());
                Assert.Fail("empty command line syntax should be rejected");
            }
            catch (ArgumentException)
            {
                // expected
            }
        }

        [TestMethod]
        public void testAutomaticUsage()
        {
            HelpFormatter hf = new HelpFormatter();
            Options options = null;
            string expected = "usage: app [-a]";
            StringWriter output = new StringWriter();
            TextWriter pw = output;

            options = new Options().addOption("a", false, "aaaa aaaa aaaa aaaa aaaa");
            hf.printUsage(pw, 60, "app", options);
            pw.Flush();
            Assert.AreEqual(expected, output.ToString().Trim(), "simple auto usage");
            output.reset();

            expected = "usage: app [-a] [-b]";
            options = new Options().addOption("a", false, "aaaa aaaa aaaa aaaa aaaa")
                    .addOption("b", false, "bbb");
            hf.printUsage(pw, 60, "app", options);
            pw.Flush();
            Assert.AreEqual(expected, output.ToString().Trim(), "simple auto usage");
            output.reset();
        }

        // This test ensures the options are properly sorted
        // See https://issues.apache.org/jira/browse/CLI-131
        [TestMethod]
        public void testPrintUsage()
        {
            Option optionA = new Option("a", "first");
            Option optionB = new Option("b", "second");
            Option optionC = new Option("c", "third");
            Options opts = new Options();
            opts.addOption(optionA);
            opts.addOption(optionB);
            opts.addOption(optionC);
            HelpFormatter helpFormatter = new HelpFormatter();
            StringWriter bytesOut = new StringWriter();
            TextWriter printWriter = bytesOut;
            helpFormatter.printUsage(printWriter, 80, "app", opts);
            printWriter.Close();
            Assert.AreEqual("usage: app [-a] [-b] [-c]" + EOL, bytesOut.ToString());
        }

        // uses the test for CLI-131 to implement CLI-155
        [TestMethod]
        public void testPrintSortedUsage()
        {
            Options opts = new Options();
            opts.addOption(new Option("a", "first"));
            opts.addOption(new Option("b", "second"));
            opts.addOption(new Option("c", "third"));

            HelpFormatter helpFormatter = new HelpFormatter();
            helpFormatter.setOptionComparator(new HelpFormatter.OptionComparator());

            StringWriter output = new StringWriter();
            helpFormatter.printUsage(output, 80, "app", opts);

            Assert.AreEqual("usage: app [-a] [-b] [-c]" + EOL, output.ToString());
        }

        [TestMethod]
        public void testPrintSortedUsageWithNullComparator()
        {
            Options opts = new Options();
            opts.addOption(new Option("a", "first"));
            opts.addOption(new Option("b", "second"));
            opts.addOption(new Option("c", "third"));

            HelpFormatter helpFormatter = new HelpFormatter();
            helpFormatter.setOptionComparator(null);

            StringWriter output = new StringWriter();
            helpFormatter.printUsage(output, 80, "app", opts);

            Assert.AreEqual("usage: app [-a] [-b] [-c]" + EOL, output.ToString());
        }

        [TestMethod]
        public void testPrintOptionGroupUsage()
        {
            OptionGroup group = new OptionGroup();
            group.addOption(OptionBuilder.create("a"));
            group.addOption(OptionBuilder.create("b"));
            group.addOption(OptionBuilder.create("c"));

            Options options = new Options();
            options.addOptionGroup(group);

            StringWriter output = new StringWriter();

            HelpFormatter formatter = new HelpFormatter();
            formatter.printUsage(output, 80, "app", options);

            Assert.AreEqual("usage: app [-a | -b | -c]" + EOL, output.ToString());
        }

        [TestMethod]
        public void testPrintRequiredOptionGroupUsage()
        {
            OptionGroup group = new OptionGroup();
            group.addOption(OptionBuilder.create("a"));
            group.addOption(OptionBuilder.create("b"));
            group.addOption(OptionBuilder.create("c"));
            group.setRequired(true);

            Options options = new Options();
            options.addOptionGroup(group);

            StringWriter output = new StringWriter();

            HelpFormatter formatter = new HelpFormatter();
            formatter.printUsage(output, 80, "app", options);

            Assert.AreEqual("usage: app -a | -b | -c" + EOL, output.ToString());
        }

        [TestMethod]
        public void testPrintOptionWithEmptyArgNameUsage()
        {
            Option option = new Option("f", true, null);
            option.setArgName("");
            option.setRequired(true);

            Options options = new Options();
            options.addOption(option);

            StringWriter output = new StringWriter();

            HelpFormatter formatter = new HelpFormatter();
            formatter.printUsage(output, 80, "app", options);

            Assert.AreEqual("usage: app -f" + EOL, output.ToString());
        }

        [TestMethod]
        public void testRtrim()
        {
            HelpFormatter formatter = new HelpFormatter();

            Assert.AreEqual(null, formatter.rtrim(null));
            Assert.AreEqual("", formatter.rtrim(""));
            Assert.AreEqual("  foo", formatter.rtrim("  foo  "));
        }

        [TestMethod]
        public void testAccessors()
        {
            HelpFormatter formatter = new HelpFormatter();

            formatter.setArgName("argname");
            Assert.AreEqual("argname", formatter.getArgName(), "arg name");

            formatter.setDescPadding(3);
            Assert.AreEqual(3, formatter.getDescPadding(), "desc padding");

            formatter.setLeftPadding(7);
            Assert.AreEqual(7, formatter.getLeftPadding(), "left padding");

            formatter.setLongOptPrefix("~~");
            Assert.AreEqual("~~", formatter.getLongOptPrefix(), "long opt prefix");

            formatter.setNewLine("\n");
            Assert.AreEqual("\n", formatter.getNewLine(), "new line");

            formatter.setOptPrefix("~");
            Assert.AreEqual("~", formatter.getOptPrefix(), "opt prefix");

            formatter.setSyntaxPrefix("-> ");
            Assert.AreEqual("-> ", formatter.getSyntaxPrefix(), "syntax prefix");

            formatter.setWidth(80);
            Assert.AreEqual(80, formatter.getWidth(), "width");
        }

        [TestMethod]
        public void testHeaderStartingWithLineSeparator()
        {
            // related to Bugzilla #21215
            Options options = new Options();
            HelpFormatter formatter = new HelpFormatter();
            string header = EOL + "Header";
            string footer = "Footer";
            StringWriter output = new StringWriter();
            formatter.printHelp(output, 80, "foobar", header, options, 2, 2, footer, true);
            Assert.AreEqual(
                    "usage: foobar" + EOL +
                    "" + EOL +
                    "Header" + EOL +
                    "" + EOL +
                    "Footer" + EOL
                    , output.ToString());
        }

        [TestMethod]
        public void testOptionWithoutShortFormat()
        {
            // related to Bugzilla #19383 (CLI-67)
            Options options = new Options();
            options.addOption(new Option("a", "aaa", false, "aaaaaaa"));
            options.addOption(new Option(null, "bbb", false, "bbbbbbb"));
            options.addOption(new Option("c", null, false, "ccccccc"));

            HelpFormatter formatter = new HelpFormatter();
            StringWriter output = new StringWriter();
            formatter.printHelp(output, 80, "foobar", "", options, 2, 2, "", true);
            Assert.AreEqual(
                    "usage: foobar [-a] [--bbb] [-c]" + EOL +
                    "  -a,--aaa  aaaaaaa" + EOL +
                    "     --bbb  bbbbbbb" + EOL +
                    "  -c        ccccccc" + EOL
                    , output.ToString());
        }

        [TestMethod]
        public void testOptionWithoutShortFormat2()
        {
            // related to Bugzilla #27635 (CLI-26)
            Option help = new Option("h", "help", false, "print this message");
            Option version = new Option("v", "version", false, "print version information");
            Option newRun = new Option("n", "new", false, "Create NLT cache entries only for new items");
            Option trackerRun = new Option("t", "tracker", false, "Create NLT cache entries only for tracker items");

            Option timeLimit = OptionBuilder.withLongOpt("limit")
                                            .hasArg()
                                            .withValueSeparator()
                                            .withDescription("Set time limit for execution, in mintues")
                                            .create("l");

            Option age = OptionBuilder.withLongOpt("age")
                                            .hasArg()
                                            .withValueSeparator()
                                            .withDescription("Age (in days) of cache item before being recomputed")
                                            .create("a");

            Option server = OptionBuilder.withLongOpt("server")
                                            .hasArg()
                                            .withValueSeparator()
                                            .withDescription("The NLT server address")
                                            .create("s");

            Option numResults = OptionBuilder.withLongOpt("results")
                                            .hasArg()
                                            .withValueSeparator()
                                            .withDescription("Number of results per item")
                                            .create("r");

            Option configFile = OptionBuilder.withLongOpt("config")
                                            .hasArg()
                                            .withValueSeparator()
                                            .withDescription("Use the specified configuration file")
                                            .create();

            Options mOptions = new Options();
            mOptions.addOption(help);
            mOptions.addOption(version);
            mOptions.addOption(newRun);
            mOptions.addOption(trackerRun);
            mOptions.addOption(timeLimit);
            mOptions.addOption(age);
            mOptions.addOption(server);
            mOptions.addOption(numResults);
            mOptions.addOption(configFile);

            HelpFormatter formatter = new HelpFormatter();
            string EOL = Environment.NewLine;
            StringWriter output = new StringWriter();
            formatter.printHelp(output, 80, "commandline", "header", mOptions, 2, 2, "footer", true);
            Assert.AreEqual(
                    "usage: commandline [-a <arg>] [--config <arg>] [-h] [-l <arg>] [-n] [-r <arg>]" + EOL +
                    "       [-s <arg>] [-t] [-v]" + EOL +
                    "header" + EOL +
                    "  -a,--age <arg>      Age (in days) of cache item before being recomputed" + EOL +
                    "     --config <arg>   Use the specified configuration file" + EOL +
                    "  -h,--help           print this message" + EOL +
                    "  -l,--limit <arg>    Set time limit for execution, in mintues" + EOL +
                    "  -n,--new            Create NLT cache entries only for new items" + EOL +
                    "  -r,--results <arg>  Number of results per item" + EOL +
                    "  -s,--server <arg>   The NLT server address" + EOL +
                    "  -t,--tracker        Create NLT cache entries only for tracker items" + EOL +
                    "  -v,--version        print version information" + EOL +
                    "footer" + EOL
                    , output.ToString());
        }
    }
}