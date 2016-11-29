using System;
namespace org.apache.commons.cli
{
    public interface IOptionBuilder
    {
        Option create();
        Option create(char opt);
        Option create(string opt);
        IOptionBuilder hasArg();
        IOptionBuilder hasArg(bool hasArg);
        IOptionBuilder hasArgs();
        IOptionBuilder hasArgs(int num);
        IOptionBuilder hasOptionalArg();
        IOptionBuilder hasOptionalArgs();
        IOptionBuilder hasOptionalArgs(int numArgs);
        IOptionBuilder isRequired();
        IOptionBuilder isRequired(bool newRequired);
        IOptionBuilder withArgName(string name);
        IOptionBuilder withDescription(string newDescription);
        IOptionBuilder withLongOpt(string newLongopt);
        IOptionBuilder withType(object newType);
        IOptionBuilder withValueSeparator();
        IOptionBuilder withValueSeparator(char sep);
    }
}
