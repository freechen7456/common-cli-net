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

namespace org.apache.commons.cli
{

    /** 
     * A formatter of help messages for the current command line options
     *
     * @author Slawek Zachcial
     * @author John Keyes (john at integralsource.com)
     * @version $Revision: 751120 $, $Date: 2009-03-06 14:45:57 -0800 (Fri, 06 Mar 2009) $
     */
    public class HelpFormatter
    {
        // --------------------------------------------------------------- Constants

        /** default number of characters per line */
        public static readonly int DEFAULT_WIDTH = 74;

        /** default padding to the left of each line */
        public static readonly int DEFAULT_LEFT_PAD = 1;

        /**
         * the number of characters of padding to be prefixed
         * to each description line
         */
        public static readonly int DEFAULT_DESC_PAD = 3;

        /** the string to display at the beginning of the usage statement */
        public static readonly string DEFAULT_SYNTAX_PREFIX = "usage: ";

        /** default prefix for shortOpts */
        public static readonly string DEFAULT_OPT_PREFIX = "-";

        /** default prefix for long Option */
        public static readonly string DEFAULT_LONG_OPT_PREFIX = "--";

        /** default name for an argument */
        public static readonly string DEFAULT_ARG_NAME = "arg";

        // -------------------------------------------------------------- Attributes

        /**
         * number of characters per line
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setWidth methods instead.
         */
        public int defaultWidth = DEFAULT_WIDTH;

        /**
         * amount of padding to the left of each line
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setLeftPadding methods instead.
         */
        public int defaultLeftPad = DEFAULT_LEFT_PAD;

        /**
         * the number of characters of padding to be prefixed
         * to each description line
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setDescPadding methods instead.
         */
        public int defaultDescPad = DEFAULT_DESC_PAD;

        /**
         * the string to display at the begining of the usage statement
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setSyntaxPrefix methods instead.
         */
        public string defaultSyntaxPrefix = DEFAULT_SYNTAX_PREFIX;

        /**
         * the new line string
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setNewLine methods instead.
         */
        public string defaultNewLine = Environment.NewLine;

        /**
         * the shortOpt prefix
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setOptPrefix methods instead.
         */
        public string defaultOptPrefix = DEFAULT_OPT_PREFIX;

        /**
         * the long Opt prefix
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setLongOptPrefix methods instead.
         */
        public string defaultLongOptPrefix = DEFAULT_LONG_OPT_PREFIX;

        /**
         * the name of the argument
         *
         * @deprecated Scope will be made private for next major version
         * - use get/setArgName methods instead.
         */
        public string defaultArgName = DEFAULT_ARG_NAME;

        /**
         * Comparator used to sort the options when they output in help text
         * 
         * Defaults to case-insensitive alphabetical sorting by option key
         */
        protected IComparer<Option> optionComparator = new OptionComparator();

        /**
         * Sets the 'width'.
         *
         * @param width the new value of 'width'
         */
        public void setWidth(int width)
        {
            this.defaultWidth = width;
        }

        /**
         * Returns the 'width'.
         *
         * @return the 'width'
         */
        public int getWidth()
        {
            return defaultWidth;
        }

        /**
         * Sets the 'leftPadding'.
         *
         * @param padding the new value of 'leftPadding'
         */
        public void setLeftPadding(int padding)
        {
            this.defaultLeftPad = padding;
        }

        /**
         * Returns the 'leftPadding'.
         *
         * @return the 'leftPadding'
         */
        public int getLeftPadding()
        {
            return defaultLeftPad;
        }

        /**
         * Sets the 'descPadding'.
         *
         * @param padding the new value of 'descPadding'
         */
        public void setDescPadding(int padding)
        {
            this.defaultDescPad = padding;
        }

        /**
         * Returns the 'descPadding'.
         *
         * @return the 'descPadding'
         */
        public int getDescPadding()
        {
            return defaultDescPad;
        }

        /**
         * Sets the 'syntaxPrefix'.
         *
         * @param prefix the new value of 'syntaxPrefix'
         */
        public void setSyntaxPrefix(string prefix)
        {
            this.defaultSyntaxPrefix = prefix;
        }

        /**
         * Returns the 'syntaxPrefix'.
         *
         * @return the 'syntaxPrefix'
         */
        public string getSyntaxPrefix()
        {
            return defaultSyntaxPrefix;
        }

        /**
         * Sets the 'newLine'.
         *
         * @param newline the new value of 'newLine'
         */
        public void setNewLine(string newline)
        {
            this.defaultNewLine = newline;
        }

        /**
         * Returns the 'newLine'.
         *
         * @return the 'newLine'
         */
        public string getNewLine()
        {
            return defaultNewLine;
        }

        /**
         * Sets the 'optPrefix'.
         *
         * @param prefix the new value of 'optPrefix'
         */
        public void setOptPrefix(string prefix)
        {
            this.defaultOptPrefix = prefix;
        }

        /**
         * Returns the 'optPrefix'.
         *
         * @return the 'optPrefix'
         */
        public string getOptPrefix()
        {
            return defaultOptPrefix;
        }

        /**
         * Sets the 'longOptPrefix'.
         *
         * @param prefix the new value of 'longOptPrefix'
         */
        public void setLongOptPrefix(string prefix)
        {
            this.defaultLongOptPrefix = prefix;
        }

        /**
         * Returns the 'longOptPrefix'.
         *
         * @return the 'longOptPrefix'
         */
        public string getLongOptPrefix()
        {
            return defaultLongOptPrefix;
        }

        /**
         * Sets the 'argName'.
         *
         * @param name the new value of 'argName'
         */
        public void setArgName(string name)
        {
            this.defaultArgName = name;
        }

        /**
         * Returns the 'argName'.
         *
         * @return the 'argName'
         */
        public string getArgName()
        {
            return defaultArgName;
        }

        /**
         * Comparator used to sort the options when they output in help text
         * 
         * Defaults to case-insensitive alphabetical sorting by option key
         */
        public IComparer<Option> getOptionComparator()
        {
            return optionComparator;
        }

        /**
         * Set the comparator used to sort the options when they output in help text
         * 
         * Passing in a null parameter will set the ordering to the default mode
         */
        public void setOptionComparator(IComparer<Option> comparator)
        {
            if (comparator == null)
            {
                this.optionComparator = new OptionComparator();
            }
            else
            {
                this.optionComparator = comparator;
            }
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.  This method prints help information to
         * System.out.
         *
         * @param cmdLineSyntax the syntax for this application
         * @param options the Options instance
         */
        public void printHelp(string cmdLineSyntax, Options options)
        {
            printHelp(defaultWidth, cmdLineSyntax, null, options, null, false);
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.  This method prints help information to 
         * System.out.
         *
         * @param cmdLineSyntax the syntax for this application
         * @param options the Options instance
         * @param autoUsage whether to print an automatically generated
         * usage statement
         */
        public void printHelp(string cmdLineSyntax, Options options, bool autoUsage)
        {
            printHelp(defaultWidth, cmdLineSyntax, null, options, null, autoUsage);
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.  This method prints help information to
         * System.out.
         *
         * @param cmdLineSyntax the syntax for this application
         * @param header the banner to display at the begining of the help
         * @param options the Options instance
         * @param footer the banner to display at the end of the help
         */
        public void printHelp(string cmdLineSyntax, string header, Options options, string footer)
        {
            printHelp(cmdLineSyntax, header, options, footer, false);
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.  This method prints help information to 
         * System.out.
         *
         * @param cmdLineSyntax the syntax for this application
         * @param header the banner to display at the begining of the help
         * @param options the Options instance
         * @param footer the banner to display at the end of the help
         * @param autoUsage whether to print an automatically generated
         * usage statement
         */
        public void printHelp(string cmdLineSyntax, string header, Options options, string footer, bool autoUsage)
        {
            printHelp(defaultWidth, cmdLineSyntax, header, options, footer, autoUsage);
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.  This method prints help information to
         * System.out.
         *
         * @param width the number of characters to be displayed on each line
         * @param cmdLineSyntax the syntax for this application
         * @param header the banner to display at the beginning of the help
         * @param options the Options instance
         * @param footer the banner to display at the end of the help
         */
        public void printHelp(int width, string cmdLineSyntax, string header, Options options, string footer)
        {
            printHelp(width, cmdLineSyntax, header, options, footer, false);
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.  This method prints help information to
         * System.out.
         *
         * @param width the number of characters to be displayed on each line
         * @param cmdLineSyntax the syntax for this application
         * @param header the banner to display at the begining of the help
         * @param options the Options instance
         * @param footer the banner to display at the end of the help
         * @param autoUsage whether to print an automatically generated 
         * usage statement
         */
        public void printHelp(int width, string cmdLineSyntax, string header,
                              Options options, string footer, bool autoUsage)
        {
            TextWriter pw = Console.Out;
            printHelp(pw, width, cmdLineSyntax, header, options, defaultLeftPad, defaultDescPad, footer, autoUsage);
            pw.Flush();
        }

        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.
         *
         * @param pw the writer to which the help will be written
         * @param width the number of characters to be displayed on each line
         * @param cmdLineSyntax the syntax for this application
         * @param header the banner to display at the begining of the help
         * @param options the Options instance
         * @param leftPad the number of characters of padding to be prefixed
         * to each line
         * @param descPad the number of characters of padding to be prefixed
         * to each description line
         * @param footer the banner to display at the end of the help
         *
         * @throws IllegalStateException if there is no room to print a line
         */
        public void printHelp(TextWriter pw, int width, string cmdLineSyntax,
                              string header, Options options, int leftPad,
                              int descPad, string footer)
        {
            printHelp(pw, width, cmdLineSyntax, header, options, leftPad, descPad, footer, false);
        }


        /**
         * Print the help for <code>options</code> with the specified
         * command line syntax.
         *
         * @param pw the writer to which the help will be written
         * @param width the number of characters to be displayed on each line
         * @param cmdLineSyntax the syntax for this application
         * @param header the banner to display at the begining of the help
         * @param options the Options instance
         * @param leftPad the number of characters of padding to be prefixed
         * to each line
         * @param descPad the number of characters of padding to be prefixed
         * to each description line
         * @param footer the banner to display at the end of the help
         * @param autoUsage whether to print an automatically generated
         * usage statement
         *
         * @throws IllegalStateException if there is no room to print a line
         */
        public void printHelp(TextWriter pw, int width, string cmdLineSyntax,
                              string header, Options options, int leftPad,
                              int descPad, string footer, bool autoUsage)
        {
            if ((cmdLineSyntax == null) || (cmdLineSyntax.Length == 0))
            {
                throw new ArgumentException("cmdLineSyntax not provided");
            }

            if (autoUsage)
            {
                printUsage(pw, width, cmdLineSyntax, options);
            }
            else
            {
                printUsage(pw, width, cmdLineSyntax);
            }

            if ((header != null) && (header.Trim().Length > 0))
            {
                printWrapped(pw, width, header);
            }

            printOptions(pw, width, options, leftPad, descPad);

            if ((footer != null) && (footer.Trim().Length > 0))
            {
                printWrapped(pw, width, footer);
            }
        }

        /**
         * <p>Prints the usage statement for the specified application.</p>
         *
         * @param pw The PrintWriter to print the usage statement 
         * @param width The number of characters to display per line
         * @param app The application name
         * @param options The command line Options
         *
         */
        public void printUsage(TextWriter pw, int width, string app, Options options)
        {
            // initialise the string buffer
            StringBuilder buff = new StringBuilder(defaultSyntaxPrefix).Append(app).Append(" ");

            // create a list for processed option groups
            List<OptionGroup> processedGroups = new List<OptionGroup>();

            // temp variable
            Option option;

            List<Option> optList = new List<Option>(options.getOptions());
            optList.Sort(getOptionComparator());
            // iterate over the options
            for (IIterator i = optList.iterator(); i.hasNext(); )
            {
                // get the next Option
                option = (Option)i.next();

                // check if the option is part of an OptionGroup
                OptionGroup group = options.getOptionGroup(option);

                // if the option is part of a group 
                if (group != null)
                {
                    // and if the group has not already been processed
                    if (!processedGroups.Contains(group))
                    {
                        // add the group to the processed list
                        processedGroups.Add(group);


                        // add the usage clause
                        appendOptionGroup(buff, group);
                    }

                    // otherwise the option was displayed in the group
                    // previously so ignore it.
                }

                // if the Option is not part of an OptionGroup
                else
                {
                    appendOption(buff, option, option.isRequired());
                }

                if (i.hasNext())
                {
                    buff.Append(" ");
                }
            }


            // call printWrapped
            printWrapped(pw, width, buff.ToString().IndexOf(' ') + 1, buff.ToString());
        }

        /**
         * Appends the usage clause for an OptionGroup to a StringBuilder.  
         * The clause is wrapped in square brackets if the group is required.
         * The display of the options is handled by appendOption
         * @param buff the StringBuilder to append to
         * @param group the group to append
         * @see #appendOption(StringBuilder,Option,bool)
         */
        private void appendOptionGroup(StringBuilder buff, OptionGroup group)
        {
            if (!group.isRequired())
            {
                buff.Append("[");
            }

            List<Option> optList = new List<cli.Option>(group.getOptions());
            optList.Sort(getOptionComparator());

            // for each option in the OptionGroup
            for (IIterator i = optList.iterator(); i.hasNext(); )
            {
                // whether the option is required or not is handled at group level
                appendOption(buff, (Option)i.next(), true);

                if (i.hasNext())
                {
                    buff.Append(" | ");
                }
            }

            if (!group.isRequired())
            {
                buff.Append("]");
            }
        }

        /**
         * Appends the usage clause for an Option to a StringBuilder.  
         *
         * @param buff the StringBuilder to append to
         * @param option the Option to append
         * @param required whether the Option is required or not
         */
        private static void appendOption(StringBuilder buff, Option option, bool required)
        {
            if (!required)
            {
                buff.Append("[");
            }

            if (option.getOpt() != null)
            {
                buff.Append("-").Append(option.getOpt());
            }
            else
            {
                buff.Append("--").Append(option.getLongOpt());
            }

            // if the Option has a value
            if (option.hasArg() && option.hasArgName())
            {
                buff.Append(" <").Append(option.getArgName()).Append(">");
            }

            // if the Option is not a required option
            if (!required)
            {
                buff.Append("]");
            }
        }

        /**
         * Print the cmdLineSyntax to the specified writer, using the
         * specified width.
         *
         * @param pw The printWriter to write the help to
         * @param width The number of characters per line for the usage statement.
         * @param cmdLineSyntax The usage statement.
         */
        public void printUsage(TextWriter pw, int width, string cmdLineSyntax)
        {
            int argPos = cmdLineSyntax.IndexOf(' ') + 1;

            printWrapped(pw, width, defaultSyntaxPrefix.Length + argPos, defaultSyntaxPrefix + cmdLineSyntax);
        }

        /**
         * <p>Print the help for the specified Options to the specified writer, 
         * using the specified width, left padding and description padding.</p>
         *
         * @param pw The printWriter to write the help to
         * @param width The number of characters to display per line
         * @param options The command line Options
         * @param leftPad the number of characters of padding to be prefixed
         * to each line
         * @param descPad the number of characters of padding to be prefixed
         * to each description line
         */
        public void printOptions(TextWriter pw, int width, Options options,
                                 int leftPad, int descPad)
        {
            StringBuilder sb = new StringBuilder();

            renderOptions(sb, width, options, leftPad, descPad);
            pw.WriteLine(sb.ToString());
        }

        /**
         * Print the specified text to the specified PrintWriter.
         *
         * @param pw The printWriter to write the help to
         * @param width The number of characters to display per line
         * @param text The text to be written to the PrintWriter
         */
        public void printWrapped(TextWriter pw, int width, string text)
        {
            printWrapped(pw, width, 0, text);
        }

        /**
         * Print the specified text to the specified PrintWriter.
         *
         * @param pw The printWriter to write the help to
         * @param width The number of characters to display per line
         * @param nextLineTabStop The position on the next line for the first tab.
         * @param text The text to be written to the PrintWriter
         */
        public void printWrapped(TextWriter pw, int width, int nextLineTabStop, string text)
        {
            StringBuilder sb = new StringBuilder(text.Length);

            renderWrappedText(sb, width, nextLineTabStop, text);
            pw.WriteLine(sb.ToString());
        }

        // --------------------------------------------------------------- Protected

        /**
         * Render the specified Options and return the rendered Options
         * in a StringBuilder.
         *
         * @param sb The StringBuilder to place the rendered Options into.
         * @param width The number of characters to display per line
         * @param options The command line Options
         * @param leftPad the number of characters of padding to be prefixed
         * to each line
         * @param descPad the number of characters of padding to be prefixed
         * to each description line
         *
         * @return the StringBuilder with the rendered Options contents.
         */
        internal protected StringBuilder renderOptions(StringBuilder sb, int width, Options options, int leftPad, int descPad)
        {
            string lpad = createPadding(leftPad);
            string dpad = createPadding(descPad);

            // first create list containing only <lpad>-a,--aaa where 
            // -a is opt and --aaa is long opt; in parallel look for 
            // the longest opt string this list will be then used to 
            // sort options ascending
            int max = 0;
            StringBuilder optBuf;
            List<string> prefixList = new List<string>();

            List<Option> optList = options.helpOptions();

            optList.Sort(getOptionComparator());

            for (IIterator i = optList.iterator(); i.hasNext(); )
            {
                Option option = (Option)i.next();
                optBuf = new StringBuilder(8);

                if (option.getOpt() == null)
                {
                    optBuf.Append(lpad).Append("   " + defaultLongOptPrefix).Append(option.getLongOpt());
                }
                else
                {
                    optBuf.Append(lpad).Append(defaultOptPrefix).Append(option.getOpt());

                    if (option.hasLongOpt())
                    {
                        optBuf.Append(',').Append(defaultLongOptPrefix).Append(option.getLongOpt());
                    }
                }

                if (option.hasArg())
                {
                    if (option.hasArgName())
                    {
                        optBuf.Append(" <").Append(option.getArgName()).Append(">");
                    }
                    else
                    {
                        optBuf.Append(' ');
                    }
                }

                prefixList.Add(optBuf.ToString());
                max = (optBuf.Length > max) ? optBuf.Length : max;
            }

            int x = 0;

            for (IIterator i = optList.iterator(); i.hasNext(); )
            {
                Option option = (Option)i.next();
                optBuf = new StringBuilder(prefixList[x++]);

                if (optBuf.Length < max)
                {
                    optBuf.Append(createPadding(max - optBuf.Length));
                }

                optBuf.Append(dpad);

                int nextLineTabStop = max + descPad;

                if (option.getDescription() != null)
                {
                    optBuf.Append(option.getDescription());
                }

                renderWrappedText(sb, width, nextLineTabStop, optBuf.ToString());

                if (i.hasNext())
                {
                    sb.Append(defaultNewLine);
                }
            }

            return sb;
        }

        /**
         * Render the specified text and return the rendered Options
         * in a StringBuilder.
         *
         * @param sb The StringBuilder to place the rendered text into.
         * @param width The number of characters to display per line
         * @param nextLineTabStop The position on the next line for the first tab.
         * @param text The text to be rendered.
         *
         * @return the StringBuilder with the rendered Options contents.
         */
        internal protected StringBuilder renderWrappedText(StringBuilder sb, int width,
                                                 int nextLineTabStop, string text)
        {
            int pos = findWrapPos(text, width, 0);

            if (pos == -1)
            {
                sb.Append(rtrim(text));

                return sb;
            }
            sb.Append(rtrim(text.Substring(0, pos))).Append(defaultNewLine);

            if (nextLineTabStop >= width)
            {
                // stops infinite loop happening
                nextLineTabStop = 1;
            }

            // all following lines must be padded with nextLineTabStop space 
            // characters
            string padding = createPadding(nextLineTabStop);

            while (true)
            {
                text = padding + text.Substring(pos).Trim();
                pos = findWrapPos(text, width, 0);

                if (pos == -1)
                {
                    sb.Append(text);

                    return sb;
                }

                if ((text.Length > width) && (pos == nextLineTabStop - 1))
                {
                    pos = width;
                }

                sb.Append(rtrim(text.Substring(0, pos))).Append(defaultNewLine);
            }
        }

        /**
         * Finds the next text wrap position after <code>startPos</code> for the
         * text in <code>text</code> with the column width <code>width</code>.
         * The wrap point is the last postion before startPos+width having a 
         * whitespace character (space, \n, \r).
         *
         * @param text The text being searched for the wrap position
         * @param width width of the wrapped text
         * @param startPos position from which to start the lookup whitespace
         * character
         * @return postion on which the text must be wrapped or -1 if the wrap
         * position is at the end of the text
         */
        internal protected int findWrapPos(string text, int width, int startPos)
        {
            int pos = -1;

            // the line ends before the max wrap pos or a new line char found
            if (((pos = text.IndexOf('\n', startPos)) != -1 && pos <= width)
                    || ((pos = text.IndexOf('\t', startPos)) != -1 && pos <= width))
            {
                return pos + 1;
            }
            else if (startPos + width >= text.Length)
            {
                return -1;
            }


            // look for the last whitespace character before startPos+width
            pos = startPos + width;

            char c;

            while ((pos >= startPos) && ((c = text[pos]) != ' ')
                    && (c != '\n') && (c != '\r'))
            {
                --pos;
            }

            // if we found it - just return
            if (pos > startPos)
            {
                return pos;
            }

            // must look for the first whitespace chearacter after startPos 
            // + width
            pos = startPos + width;

            while ((pos <= text.Length) && ((c = text[pos]) != ' ')
                   && (c != '\n') && (c != '\r'))
            {
                ++pos;
            }

            return (pos == text.Length) ? (-1) : pos;
        }

        /**
         * Return a string of padding of length <code>len</code>.
         *
         * @param len The length of the string of padding to create.
         *
         * @return The string of padding
         */
        internal protected string createPadding(int len)
        {
            StringBuilder sb = new StringBuilder(len);

            for (int i = 0; i < len; ++i)
            {
                sb.Append(' ');
            }

            return sb.ToString();
        }

        /**
         * Remove the trailing whitespace from the specified string.
         *
         * @param s The string to remove the trailing padding from.
         *
         * @return The string of without the trailing padding
         */
        internal protected string rtrim(string s)
        {
            if ((s == null) || (s.Length == 0))
            {
                return s;
            }

            int pos = s.Length;

            while ((pos > 0) && Char.IsWhiteSpace(s[pos - 1]))
            {
                --pos;
            }

            return s.Substring(0, pos);
        }

        // ------------------------------------------------------ Package protected
        // ---------------------------------------------------------------- Private
        // ---------------------------------------------------------- Inner classes
        /**
         * This class implements the <code>Comparator</code> interface
         * for comparing Options.
         */
        internal class OptionComparator : IComparer<Option>
        {

            /**
             * Compares its two arguments for order. Returns a negative
             * integer, zero, or a positive integer as the first argument
             * is less than, equal to, or greater than the second.
             *
             * @param o1 The first Option to be compared.
             * @param o2 The second Option to be compared.
             * @return a negative integer, zero, or a positive integer as
             *         the first argument is less than, equal to, or greater than the
             *         second.
             */
            int IComparer<Option>.Compare(Option x, Option y)
            {
                return string.Compare(x.getKey(), y.getKey(), true);
            }
        }
    }
}