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

    /**
     * A group of mutually exclusive options.
     *
     * @author John Keyes ( john at integralsource.com )
     * @version $Revision: 680644 $, $Date: 2008-07-29 01:13:48 -0700 (Tue, 29 Jul 2008) $
     */
    [Serializable]
    public class OptionGroup
    {
        private static readonly long serialVersionUID = 1L;

        /** hold the options */
        private Dictionary<string, Option> optionMap = new Dictionary<string, Option>();

        /** the name of the selected option */
        private String selected;

        /** specified whether this group is required */
        private bool required;

        /**
         * Add the specified <code>Option</code> to this group.
         *
         * @param option the option to add to this group
         * @return this option group with the option added
         */
        public OptionGroup addOption(Option option)
        {
            // key   - option name
            // value - the option
            optionMap[option.getKey()] = option;

            return this;
        }

        /**
         * @return the names of the options in this group as a 
         * <code>Collection</code>
         */
        public ICollection<string> getNames()
        {
            // the key set is the collection of names
            return optionMap.Keys;
        }

        /**
         * @return the options in this group as a <code>Collection</code>
         */
        public ICollection<Option> getOptions()
        {
            // the values are the collection of options
            return optionMap.Values;
        }

        /**
         * Set the selected option of this group to <code>name</code>.
         *
         * @param option the option that is selected
         * @throws AlreadySelectedException if an option from this group has 
         * already been selected.
         */
        public void setSelected(Option option)
        {
            // if no option has already been selected or the 
            // same option is being reselected then set the
            // selected member variable
            if (selected == null || selected.Equals(option.getOpt()))
            {
                selected = option.getOpt();
            }
            else
            {
                throw new AlreadySelectedException(this, option);
            }
        }

        /**
         * @return the selected option name
         */
        public String getSelected()
        {
            return selected;
        }

        /**
         * @param required specifies if this group is required
         */
        public void setRequired(bool required)
        {
            this.required = required;
        }

        /**
         * Returns whether this option group is required.
         *
         * @return whether this option group is required
         */
        public bool isRequired()
        {
            return required;
        }

        /**
         * Returns the stringified version of this OptionGroup.
         * 
         * @return the stringified representation of this group
         */
        public String toString()
        {
            StringBuilder buff = new StringBuilder();

            IEnumerator<Option> iter = getOptions().GetEnumerator();

            buff.Append("[");

            const string separator = ", ";
            foreach (var option in getOptions())
            {

                if (option.getOpt() != null)
                {
                    buff.Append("-");
                    buff.Append(option.getOpt());
                }
                else
                {
                    buff.Append("--");
                    buff.Append(option.getLongOpt());
                }

                buff.Append(" ");
                buff.Append(option.getDescription());

                buff.Append(separator);
            }
            if (getOptions().Count > 0)
            {
                buff.Remove(buff.Length - separator.Length, separator.Length);
            }

            buff.Append("]");

            return buff.ToString();
        }
    }
}