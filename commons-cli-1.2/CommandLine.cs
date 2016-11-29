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

namespace org.apache.commons.cli
{

    /**
     * Represents list of arguments parsed against a {@link Options} descriptor.
     *
     * <p>It allows querying of a bool {@link #hasOption(string opt)},
     * in addition to retrieving the {@link #getOptionValue(string opt)}
     * for options requiring arguments.</p>
     *
     * <p>Additionally, any left-over or unrecognized arguments,
     * are available for further processing.</p>
     *
     * @author bob mcwhirter (bob @ werken.com)
     * @author <a href="mailto:jstrachan@apache.org">James Strachan</a>
     * @author John Keyes (john at integralsource.com)
     * @version $Revision: 735247 $, $Date: 2009-01-17 00:23:35 -0800 (Sat, 17 Jan 2009) $
     */
    [Serializable]
    public class CommandLine
    {
        private static readonly long serialVersionUID = 1L;

        /** the unrecognised options/arguments */
        private List<string> args = new List<string>();

        /** the processed options */
        private List<Option> options = new List<Option>();

        /**
         * Creates a command line.
         */
        internal CommandLine()
        {
            // nothing to do
        }

        /** 
         * Query to see if an option has been set.
         *
         * @param opt Short name of the option
         * @return true if set, false if not
         */
        public bool hasOption(string opt)
        {
            return options.Contains(resolveOption(opt));
        }

        /** 
         * Query to see if an option has been set.
         *
         * @param opt character name of the option
         * @return true if set, false if not
         */
        public bool hasOption(char opt)
        {
            return hasOption(Convert.ToString(opt));
        }

        /**
         * Return the <code>object</code> type of this <code>Option</code>.
         *
         * @param opt the name of the option
         * @return the type of this <code>Option</code>
         * @deprecated due to System.err message. Instead use getParsedOptionValue(string)
         */
        public object getOptionObject(string opt)
        {
            try
            {
                return getParsedOptionValue(opt);
            }
            catch (ParseException pe)
            {
                Console.Error.WriteLine("Exception found converting " + opt + " to desired type: " +
                    pe.Message);
                return null;
            }
        }

        /**
         * Return a version of this <code>Option</code> converted to a particular type. 
         *
         * @param opt the name of the option
         * @return the value parsed into a particluar object
         * @throws ParseException if there are problems turning the option value into the desired type
         * @see PatternOptionBuilder
         */
        public object getParsedOptionValue(string opt)
        {
            string res = getOptionValue(opt);

            Option option = resolveOption(opt);
            if (option == null)
            {
                return null;
            }

            object type = option.getValueType();

            return (res == null) ? null : TypeHandler.createValue(res, type);
        }

        /**
         * Return the <code>object</code> type of this <code>Option</code>.
         *
         * @param opt the name of the option
         * @return the type of opt
         */
        public object getOptionObject(char opt)
        {
            return getOptionObject(Convert.ToString(opt));
        }

        /** 
         * Retrieve the argument, if any, of this option.
         *
         * @param opt the name of the option
         * @return Value of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string getOptionValue(string opt)
        {
            string[] values = getOptionValues(opt);

            return (values == null) ? null : values[0];
        }

        /** 
         * Retrieve the argument, if any, of this option.
         *
         * @param opt the character name of the option
         * @return Value of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string getOptionValue(char opt)
        {
            return getOptionValue(Convert.ToString(opt));
        }

        /** 
         * Retrieves the array of values, if any, of an option.
         *
         * @param opt string name of the option
         * @return Values of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string[] getOptionValues(string opt)
        {
            List<string> values = new List<string>();

            foreach (var option in options)
            {
                if (opt.Equals(option.getOpt()) || opt.Equals(option.getLongOpt()))
                {
                    values.AddRange(option.getValuesList());
                }
            }

            return values.isEmpty() ? null : (string[])values.ToArray();
        }

        /**
         * Retrieves the option object given the long or short option as a string
         * 
         * @param opt short or long name of the option
         * @return Canonicalized option
         */
        private Option resolveOption(string opt)
        {
            opt = Util.stripLeadingHyphens(opt);
            foreach (var option in options)
            {
                if (opt.Equals(option.getOpt()))
                {
                    return option;
                }

                if (opt.Equals(option.getLongOpt()))
                {
                    return option;
                }

            }
            return null;
        }

        /** 
         * Retrieves the array of values, if any, of an option.
         *
         * @param opt character name of the option
         * @return Values of the argument if option is set, and has an argument,
         * otherwise null.
         */
        public string[] getOptionValues(char opt)
        {
            return getOptionValues(Convert.ToString(opt));
        }

        /** 
         * Retrieve the argument, if any, of an option.
         *
         * @param opt name of the option
         * @param defaultValue is the default value to be returned if the option
         * is not specified
         * @return Value of the argument if option is set, and has an argument,
         * otherwise <code>defaultValue</code>.
         */
        public string getOptionValue(string opt, string defaultValue)
        {
            string answer = getOptionValue(opt);

            return (answer != null) ? answer : defaultValue;
        }

        /** 
         * Retrieve the argument, if any, of an option.
         *
         * @param opt character name of the option
         * @param defaultValue is the default value to be returned if the option
         * is not specified
         * @return Value of the argument if option is set, and has an argument,
         * otherwise <code>defaultValue</code>.
         */
        public string getOptionValue(char opt, string defaultValue)
        {
            return getOptionValue(Convert.ToString(opt), defaultValue);
        }

        /**
         * Retrieve the map of values associated to the option. This is convenient
         * for options specifying Java properties like <tt>-Dparam1=value1
         * -Dparam2=value2</tt>. The first argument of the option is the key, and
         * the 2nd argument is the value. If the option has only one argument
         * (<tt>-Dfoo</tt>) it is considered as a bool flag and the value is
         * <tt>"true"</tt>.
         *
         * @param opt name of the option
         * @return The Properties mapped by the option, never <tt>null</tt>
         *         even if the option doesn't exists
         * @since 1.2
         */
        public Dictionary<string, string> getOptionProperties(string opt)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            foreach (var option in options)
            {
                if (opt.Equals(option.getOpt()) || opt.Equals(option.getLongOpt()))
                {
                    IList<string> values = option.getValuesList();
                    if (values.Count >= 2)
                    {
                        // use the first 2 arguments as the key/value pair
                        props[values[0]] = values[1];
                    }
                    else if (values.Count == 1)
                    {
                        // no explicit value, handle it as a bool
                        props[values[0]] = "true";
                    }
                }
            }

            return props;
        }

        /** 
         * Retrieve any left-over non-recognized options and arguments
         *
         * @return remaining items passed in but not parsed as an array
         */
        public string[] getArgs()
        {
            return args.ToArray();
        }

        /** 
         * Retrieve any left-over non-recognized options and arguments
         *
         * @return remaining items passed in but not parsed as a <code>List</code>.
         */
        public IList<string> getArgList()
        {
            return args;
        }

        /** 
         * jkeyes
         * - commented out until it is implemented properly
         * <p>Dump state, suitable for debugging.</p>
         *
         * @return Stringified form of this object
         */

        /*
        public string toString() {
            StringBuffer buf = new StringBuffer();
            
            buf.append("[ CommandLine: [ options: ");
            buf.append(options.toString());
            buf.append(" ] [ args: ");
            buf.append(args.toString());
            buf.append(" ] ]");
            
            return buf.toString();
        }
        */

        /**
         * Add left-over unrecognized option/argument.
         *
         * @param arg the unrecognised option/argument.
         */
        internal void addArg(string arg)
        {
            args.Add(arg);
        }

        /**
         * Add an option to the command line.  The values of the option are stored.
         *
         * @param opt the processed option
         */
        internal void addOption(Option opt)
        {
            options.Add(opt);
        }

        /**
         * Returns an iterator over the Option members of CommandLine.
         *
         * @return an <code>Iterator</code> over the processed {@link Option}
         * members of this {@link CommandLine}
         */
        public IIterator iterator()
        {
            return options.iterator();
        }

        /**
         * Returns an array of the processed {@link Option}s.
         *
         * @return an array of the processed {@link Option}s.
         */
        public Option[] getOptions()
        {
            // return the array
            return options.ToArray();
        }
    }
}