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
using System.Text;

namespace org.apache.commons.cli
{

    /** <p>Describes a single command-line option.  It maintains
     * information regarding the short-name of the option, the long-name,
     * if any exists, a flag indicating if an argument is required for
     * this option, and a self-documenting description of the option.</p>
     *
     * <p>An Option is not created independantly, but is create through
     * an instance of {@link Options}.<p>
     *
     * @see org.apache.commons.cli.Options
     * @see org.apache.commons.cli.CommandLine
     *
     * @author bob mcwhirter (bob @ werken.com)
     * @author <a href="mailto:jstrachan@apache.org">James Strachan</a>
     * @version $Revision: 680644 $, $Date: 2008-07-29 01:13:48 -0700 (Tue, 29 Jul 2008) $
     */
    [Serializable]
    public class Option : ICloneable
    {
        private static readonly long serialVersionUID = 1L;

        /** constant that specifies the number of argument values has not been specified */
        public static readonly int UNINITIALIZED = -1;

        /** constant that specifies the number of argument values is infinite */
        public static readonly int UNLIMITED_VALUES = -2;

        /** the name of the option */
        private string opt;

        /** the long representation of the option */
        private string longOpt;

        /** the name of the argument for this option */
        private string argName = "arg";

        /** description of the option */
        private string description;

        /** specifies whether this option is required to be present */
        private bool required;

        /** specifies whether the argument value of this Option is optional */
        private bool optionalArg;

        /** the number of argument values this option can have */
        private int numberOfArgs = UNINITIALIZED;

        /** the type of this Option */
        private Type valueType;

        /** the list of argument values **/
        private List<string> values = new List<string>();

        /** the character that is the value separator */
        private char valuesep;

        /**
         * Creates an Option using the specified parameters.
         *
         * @param opt short representation of the option
         * @param description describes the function of the option
         *
         * @throws IllegalArgumentException if there are any non valid
         * Option characters in <code>opt</code>.
         */
        public Option(string opt, string description)
            : this(opt, null, false, description)
        {
        }

        /**
         * Creates an Option using the specified parameters.
         *
         * @param opt short representation of the option
         * @param hasArg specifies whether the Option takes an argument or not
         * @param description describes the function of the option
         *
         * @throws IllegalArgumentException if there are any non valid
         * Option characters in <code>opt</code>.
         */
        public Option(string opt, bool hasArg, string description)
            : this(opt, null, hasArg, description)
        {
        }

        /**
         * Creates an Option using the specified parameters.
         *
         * @param opt short representation of the option
         * @param longOpt the long representation of the option
         * @param hasArg specifies whether the Option takes an argument or not
         * @param description describes the function of the option
         *
         * @throws IllegalArgumentException if there are any non valid
         * Option characters in <code>opt</code>.
         */
        public Option(string opt, string longOpt, bool hasArg, string description)
        {
            // ensure that the option is valid
            OptionValidator.validateOption(opt);

            this.opt = opt;
            this.longOpt = longOpt;

            // if hasArg is set then the number of arguments is 1
            if (hasArg)
            {
                this.numberOfArgs = 1;
            }

            this.description = description;
        }

        /**
         * Returns the id of this Option.  This is only set when the
         * Option shortOpt is a single character.  This is used for switch
         * statements.
         *
         * @return the id of this Option
         */
        public int getId()
        {
            return getKey()[0];
        }

        /**
         * Returns the 'unique' Option identifier.
         * 
         * @return the 'unique' Option identifier
         */
        internal string getKey()
        {
            // if 'opt' is null, then it is a 'long' option
            if (opt == null)
            {
                return longOpt;
            }

            return opt;
        }

        /** 
         * Retrieve the name of this Option.
         *
         * It is this string which can be used with
         * {@link CommandLine#hasOption(string opt)} and
         * {@link CommandLine#getOptionValue(string opt)} to check
         * for existence and argument.
         *
         * @return The name of this option
         */
        public string getOpt()
        {
            return opt;
        }

        /**
         * Retrieve the type of this Option.
         * 
         * @return The type of this option
         */
        public Type getValueType()
        {
            return valueType;
        }

        /**
         * Sets the type of this Option.
         *
         * @param type the type of this Option
         */
        public void setValueType(Type type)
        {
            this.valueType = type;
        }

        /** 
         * Retrieve the long name of this Option.
         *
         * @return Long name of this option, or null, if there is no long name
         */
        public string getLongOpt()
        {
            return longOpt;
        }

        /**
         * Sets the long name of this Option.
         *
         * @param longOpt the long name of this Option
         */
        public void setLongOpt(string longOpt)
        {
            this.longOpt = longOpt;
        }

        /**
         * Sets whether this Option can have an optional argument.
         *
         * @param optionalArg specifies whether the Option can have
         * an optional argument.
         */
        public void setOptionalArg(bool optionalArg)
        {
            this.optionalArg = optionalArg;
        }

        /**
         * @return whether this Option can have an optional argument
         */
        public bool hasOptionalArg()
        {
            return optionalArg;
        }

        /** 
         * Query to see if this Option has a long name
         *
         * @return bool flag indicating existence of a long name
         */
        public bool hasLongOpt()
        {
            return longOpt != null;
        }

        /** 
         * Query to see if this Option requires an argument
         *
         * @return bool flag indicating if an argument is required
         */
        public bool hasArg()
        {
            return numberOfArgs > 0 || numberOfArgs == UNLIMITED_VALUES;
        }

        /** 
         * Retrieve the self-documenting description of this Option
         *
         * @return The string description of this option
         */
        public string getDescription()
        {
            return description;
        }

        /**
         * Sets the self-documenting description of this Option
         *
         * @param description The description of this option
         * @since 1.1
         */
        public void setDescription(string description)
        {
            this.description = description;
        }

        /** 
         * Query to see if this Option requires an argument
         *
         * @return bool flag indicating if an argument is required
         */
        public bool isRequired()
        {
            return required;
        }

        /**
         * Sets whether this Option is mandatory.
         *
         * @param required specifies whether this Option is mandatory
         */
        public void setRequired(bool required)
        {
            this.required = required;
        }

        /**
         * Sets the display name for the argument value.
         *
         * @param argName the display name for the argument value.
         */
        public void setArgName(string argName)
        {
            this.argName = argName;
        }

        /**
         * Gets the display name for the argument value.
         *
         * @return the display name for the argument value.
         */
        public string getArgName()
        {
            return argName;
        }

        /**
         * Returns whether the display name for the argument value
         * has been set.
         *
         * @return if the display name for the argument value has been
         * set.
         */
        public bool hasArgName()
        {
            return argName != null && argName.Length > 0;
        }

        /** 
         * Query to see if this Option can take many values.
         *
         * @return bool flag indicating if multiple values are allowed
         */
        public bool hasArgs()
        {
            return numberOfArgs > 1 || numberOfArgs == UNLIMITED_VALUES;
        }

        /** 
         * Sets the number of argument values this Option can take.
         *
         * @param num the number of argument values
         */
        public void setArgs(int num)
        {
            this.numberOfArgs = num;
        }

        /**
         * Sets the value separator.  For example if the argument value
         * was a Java property, the value separator would be '='.
         *
         * @param sep The value separator.
         */
        public void setValueSeparator(char sep)
        {
            this.valuesep = sep;
        }

        /**
         * Returns the value separator character.
         *
         * @return the value separator character.
         */
        public char getValueSeparator()
        {
            return valuesep;
        }

        /**
         * Return whether this Option has specified a value separator.
         * 
         * @return whether this Option has specified a value separator.
         * @since 1.1
         */
        public bool hasValueSeparator()
        {
            return valuesep > 0;
        }

        /** 
         * Returns the number of argument values this Option can take.
         *
         * @return num the number of argument values
         */
        public int getArgs()
        {
            return numberOfArgs;
        }

        /**
         * Adds the specified value to this Option.
         * 
         * @param value is a/the value of this Option
         */
        internal protected void addValueForProcessing(string value)
        {
            //switch (numberOfArgs)
            //{
            //    case UNINITIALIZED:
            //        throw new ApplicationException("NO_ARGS_ALLOWED");

            //    default:
            //        processValue(value);
            //}
            if (numberOfArgs == UNINITIALIZED)
            {
                throw new ApplicationException("NO_ARGS_ALLOWED");
            }
            else
            {
                processValue(value);
            }
        }

        /**
         * Processes the value.  If this Option has a value separator
         * the value will have to be parsed into individual tokens.  When
         * n-1 tokens have been processed and there are more value separators
         * in the value, parsing is ceased and the remaining characters are
         * added as a single token.
         *
         * @param value The string to be processed.
         *
         * @since 1.0.1
         */
        private void processValue(string value)
        {
            // this Option has a separator character
            if (hasValueSeparator())
            {
                // get the separator character
                char sep = getValueSeparator();

                // store the index for the value separator
                int index = value.IndexOf(sep);

                // while there are more value separators
                while (index != -1)
                {
                    // next value to be added 
                    if (values.Count == (numberOfArgs - 1))
                    {
                        break;
                    }

                    // store
                    add(value.Substring(0, index));

                    // parse
                    value = value.Substring(index + 1);

                    // get new index
                    index = value.IndexOf(sep);
                }
            }

            // store the actual value or the last value that has been parsed
            add(value);
        }

        /**
         * Add the value to this Option.  If the number of arguments
         * is greater than zero and there is enough space in the list then
         * add the value.  Otherwise, throw a runtime exception.
         *
         * @param value The value to be added to this Option
         *
         * @since 1.0.1
         */
        private void add(string value)
        {
            if ((numberOfArgs > 0) && (values.Count > (numberOfArgs - 1)))
            {
                throw new ApplicationException("Cannot add value, list full.");
            }

            // store value
            values.Add(value);
        }

        /**
         * Returns the specified value of this Option or 
         * <code>null</code> if there is no value.
         *
         * @return the value/first value of this Option or 
         * <code>null</code> if there is no value.
         */
        public virtual string getValue()
        {
            return hasNoValues() ? null : (string)values[0];
        }

        /**
         * Returns the specified value of this Option or 
         * <code>null</code> if there is no value.
         *
         * @param index The index of the value to be returned.
         *
         * @return the specified value of this Option or 
         * <code>null</code> if there is no value.
         *
         * @throws IndexOutOfBoundsException if index is less than 1
         * or greater than the number of the values for this Option.
         */
        public string getValue(int index)
        {
            return hasNoValues() ? null : (string)values[index];
        }

        /**
         * Returns the value/first value of this Option or the 
         * <code>defaultValue</code> if there is no value.
         *
         * @param defaultValue The value to be returned if ther
         * is no value.
         *
         * @return the value/first value of this Option or the 
         * <code>defaultValue</code> if there are no values.
         */
        public string getValue(string defaultValue)
        {
            string value = getValue();

            return (value != null) ? value : defaultValue;
        }

        /**
         * Return the values of this Option as a string array 
         * or null if there are no values
         *
         * @return the values of this Option as a string array 
         * or null if there are no values
         */
        public string[] getValues()
        {
            return hasNoValues() ? null : (string[])values.ToArray();
        }

        /**
         * @return the values of this Option as a List
         * or null if there are no values
         */
        public IList<string> getValuesList()
        {
            return values;
        }

        /** 
         * Dump state, suitable for debugging.
         *
         * @return Stringified form of this object
         */
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder().Append("[ option: ");

            buf.Append(opt);

            if (longOpt != null)
            {
                buf.Append(" ").Append(longOpt);
            }

            buf.Append(" ");

            if (hasArgs())
            {
                buf.Append("[ARG...]");
            }
            else if (hasArg())
            {
                buf.Append(" [ARG]");
            }

            buf.Append(" :: ").Append(description);

            if (valueType != null)
            {
                buf.Append(" :: ").Append(valueType);
            }

            buf.Append(" ]");

            return buf.ToString();
        }

        /**
         * Returns whether this Option has any values.
         *
         * @return whether this Option has any values.
         */
        private bool hasNoValues()
        {
            return  values.isEmpty();
        }

        public bool equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || GetType() != o.GetType())
            {
                return false;
            }

            Option option = (Option)o;


            if (opt != null ? !opt.Equals(option.opt) : option.opt != null)
            {
                return false;
            }
            if (longOpt != null ? !longOpt.Equals(option.longOpt) : option.longOpt != null)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = (opt != null ? opt.GetHashCode() : 0);
            result = 31 * result + (longOpt != null ? longOpt.GetHashCode() : 0);
            return result;
        }

        /**
         * A rather odd clone method - due to incorrect code in 1.0 it is public 
         * and in 1.1 rather than throwing a CloneNotSupportedException it throws 
         * a RuntimeException so as to maintain backwards compat at the API level. 
         *
         * After calling this method, it is very likely you will want to call 
         * clearValues(). 
         *
         * @throws RuntimeException
         */
        public Option clone()
        {
            //try
            //{
            Option option = (Option)this.MemberwiseClone();
            option.values = new List<string>(values);
            return option;
            //}
            //catch (CloneNotSupportedException cnse)
            //{
            //    throw new RuntimeException("A CloneNotSupportedException was thrown: " + cnse.getMessage());
            //}
        }

        /**
         * Clear the Option values. After a parse is complete, these are left with
         * data in them and they need clearing if another parse is done.
         *
         * See: <a href="https://issues.apache.org/jira/browse/CLI-71">CLI-71</a>
         */
        internal void clearValues()
        {
            values.Clear();
        }

        /**
         * This method is not intended to be used. It was a piece of internal 
         * API that was made public in 1.0. It currently throws an UnsupportedOperationException. 
         * @deprecated
         * @throws UnsupportedOperationException
         */
        public virtual bool addValue(string value)
        {
            throw new NotSupportedException("The addValue method is not intended for client use. "
                    + "Subclasses should use the addValueForProcessing method instead. ");
        }


        object ICloneable.Clone()
        {
            return this.clone();
        }

        public bool Equals(Option option)
        {
            if (option != null)
            {
                return (this.opt == option.opt) &&
                    (this.longOpt == option.longOpt) &&
                    (this.numberOfArgs == option.numberOfArgs) &&
                    (this.description == option.description);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Option)
            {
                return Equals((Option)obj);
            }
            else
            {
                return false;
            }
        }

    }
}