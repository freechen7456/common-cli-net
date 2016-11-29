/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * instance work for additional information regarding copyright ownership.
 * The ASF licenses instance file to You under the Apache License, Version 2.0
 * (the "License"); you may not use instance file except in compliance with
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

namespace org.apache.commons.cli
{
    /**
     * IOptionBuilder allows the user to create Options using descriptive methods.
     *
     * <p>Details on the Builder pattern can be found at
     * <a href="http://c2.com/cgi-bin/wiki?BuilderPattern">
     * http://c2.com/cgi-bin/wiki?BuilderPattern</a>.</p>
     *
     * @author John Keyes (john at integralsource.com)
     * @version $Revision: 754830 $, $Date: 2009-03-16 00:26:44 -0700 (Mon, 16 Mar 2009) $
     * @since 1.0
     */
    public sealed class OptionBuilder
    {
        ///** long option */
        //private string longopt;

        ///** option description */
        //private string description;

        ///** argument name */
        //private string argName;

        ///** is required? */
        //private bool required;

        ///** the number of arguments */
        //private int numberOfArgs = Option.UNINITIALIZED;

        ///** option type */
        //private Type type;

        ///** option can have an optional argument value */
        //private bool optionalArg;

        ///** value separator for argument value */
        //private char valuesep;

        /** option builder instance */
        private static InternalOptionBuilder instance = new InternalOptionBuilder();

        /**
         * private constructor to prevent instances being created
         */
        private OptionBuilder()
        {
            // hide the constructor
        }

        ///**
        // * Resets the member variables to their default values.
        // */
        //private void reset()
        //{
        //    description = null;
        //    argName = "arg";
        //    longopt = null;
        //    type = null;
        //    required = false;
        //    numberOfArgs = Option.UNINITIALIZED;


        //    // PMM 9/6/02 - these were missing
        //    optionalArg = false;
        //    valuesep = (char)0;
        //}

        /**
         * The next Option created will have the following long option value.
         *
         * @param newLongopt the long option value
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder withLongOpt(string newLongopt)
        {
            instance.longopt = newLongopt;

            return instance;
        }

        /**
         * The next Option created will require an argument value.
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasArg()
        {
            instance.numberOfArgs = 1;

            return instance;
        }

        /**
         * The next Option created will require an argument value if
         * <code>hasArg</code> is true.
         *
         * @param hasArg if true then the Option has an argument value
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasArg(bool hasArg)
        {
            instance.numberOfArgs = hasArg ? 1 : Option.UNINITIALIZED;

            return instance;
        }

        /**
         * The next Option created will have the specified argument value name.
         *
         * @param name the name for the argument value
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder withArgName(string name)
        {
            instance.argName = name;

            return instance;
        }

        /**
         * The next Option created will be required.
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder isRequired()
        {
            instance.required = true;

            return instance;
        }

        /**
         * The next Option created uses <code>sep</code> as a means to
         * separate argument values.
         *
         * <b>Example:</b>
         * <pre>
         * Option opt = instance.withValueSeparator(':')
         *                           .create('D');
         *
         * CommandLine line = parser.parse(args);
         * string propertyName = opt.getValue(0);
         * string propertyValue = opt.getValue(1);
         * </pre>
         *
         * @param sep The value separator to be used for the argument values.
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder withValueSeparator(char sep)
        {
            instance.valuesep = sep;

            return instance;
        }

        /**
         * The next Option created uses '<code>=</code>' as a means to
         * separate argument values.
         *
         * <b>Example:</b>
         * <pre>
         * Option opt = instance.withValueSeparator()
         *                           .create('D');
         *
         * CommandLine line = parser.parse(args);
         * string propertyName = opt.getValue(0);
         * string propertyValue = opt.getValue(1);
         * </pre>
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder withValueSeparator()
        {
            instance.valuesep = '=';

            return instance;
        }

        /**
         * The next Option created will be required if <code>required</code>
         * is true.
         *
         * @param newRequired if true then the Option is required
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder isRequired(bool newRequired)
        {
            instance.required = newRequired;

            return instance;
        }

        /**
         * The next Option created can have unlimited argument values.
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasArgs()
        {
            instance.numberOfArgs = Option.UNLIMITED_VALUES;

            return instance;
        }

        /**
         * The next Option created can have <code>num</code> argument values.
         *
         * @param num the number of args that the option can have
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasArgs(int num)
        {
            instance.numberOfArgs = num;

            return instance;
        }

        /**
         * The next Option can have an optional argument.
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasOptionalArg()
        {
            instance.numberOfArgs = 1;
            instance.optionalArg = true;

            return instance;
        }

        /**
         * The next Option can have an unlimited number of optional arguments.
         *
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasOptionalArgs()
        {
            instance.numberOfArgs = Option.UNLIMITED_VALUES;
            instance.optionalArg = true;

            return instance;
        }

        /**
         * The next Option can have the specified number of optional arguments.
         *
         * @param numArgs - the maximum number of optional arguments
         * the next Option created can have.
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder hasOptionalArgs(int numArgs)
        {
            instance.numberOfArgs = numArgs;
            instance.optionalArg = true;

            return instance;
        }

        /**
         * The next Option created will have a value that will be an instance
         * of <code>type</code>.
         *
         * @param newType the type of the Options argument value
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder withType(object newType)
        {
            instance.type = (newType == null) ?  null : (newType is Type ? (Type)newType : newType.GetType());

            return instance;
        }

        /**
         * The next Option created will have the specified description
         *
         * @param newDescription a description of the Option's purpose
         * @return the IOptionBuilder instance
         */
        public static IOptionBuilder withDescription(string newDescription)
        {
            instance.description = newDescription;

            return instance;
        }

        /**
         * Create an Option using the current settings and with
         * the specified Option <code>char</code>.
         *
         * @param opt the character representation of the Option
         * @return the Option instance
         * @throws IllegalArgumentException if <code>opt</code> is not
         * a valid character.  See Option.
         */
        public static Option create(char opt)
        {
            return instance.create(opt);
        }

        /**
         * Create an Option using the current settings
         *
         * @return the Option instance
         * @throws IllegalArgumentException if <code>longOpt</code> has not been set.
         */
        public static Option create()
        {
            return instance.create();
        }

        /**
         * Create an Option using the current settings and with
         * the specified Option <code>char</code>.
         *
         * @param opt the <code>java.lang.string</code> representation
         * of the Option
         * @return the Option instance
         * @throws IllegalArgumentException if <code>opt</code> is not
         * a valid character.  See Option.
         */
        public static Option create(string opt)
        {
            return instance.create(opt);
        }
    }
}