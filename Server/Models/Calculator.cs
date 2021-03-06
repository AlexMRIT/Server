using System;
using System.Linq;
using System.Collections.Generic;

namespace Server.Models
{
    public sealed class Calculator
    {
        public static Calculator[] GetCalculatorsForStats() => new Calculator[Enum.GetNames(typeof(CharacterStatId)).Length];

        public int Size { get; private set; }

        private readonly List<StatFunction> _functions;

        public Calculator()
        {
            _functions = new List<StatFunction>();
            Size = 0;
        }

        public void AddFunc(StatFunction func)
        {
            Size++;
            _functions.Add(func);
        }

        public void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            foreach (StatFunction function in _functions.OrderBy(x => x.Order))
                function.Calculate(statFuncEnv);
        }
    }
}
