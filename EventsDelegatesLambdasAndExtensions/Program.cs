using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsDelegatesLambdasAndExtensions
{
    delegate void MyDelegate(int number, string text);

    class Program
    {
        static void Main(string[] args)
        {
            MyDelegate variable;

            //won't work
            //variable += MyMethod;

            //variable initialization
            variable = null;

            //throws NullReferenceException
            //variable(123, "hello");

            //adding method
            variable += MyMethod;

            var b1 = variable == null; //false

            //removing method
            variable -= MyMethod;

            var b2 = variable == null; //true

            //settings values:

            //set null value
            variable = null;

            //method
            variable += MyMethod;

            //anonymous method
            variable += delegate(int number, string text)
            {
                Console.WriteLine("I'm anonymous method. Parameters: {0}, {1}", number, text);
            };

            //lambda expression
            variable += (int number, string text) =>
                {
                    Console.WriteLine("I'm lambda expression 1. Parameters: {0}, {1}", number, text);
                };

            //lambda expression
            variable += (number, text) =>
            {
                Console.WriteLine("I'm lambda expression 2. Parameters: {0}, {1}", number, text);
            };

            //lambda expression
            variable += (number, text) => Console.WriteLine("I'm lambda expression 3. Parameters: {0}, {1}", number, text);


            MyDelegate otherVariable = null;
            Action<int, string> anotherVariable = null;

            //this won't work
            //anotherVariable = otherVariable;

            //but there's a workaround:
            anotherVariable = (a, b) => otherVariable(a, b);

            //variable
            variable += otherVariable;

            variable(123, "hello");

            MethodAcceptingDelegateAsParameter(variable);
            MethodAcceptingDelegateAsParameter(MyMethod);
            MethodAcceptingDelegateAsParameter(MyMethod + (MyDelegate)((a, b) => { }));

            var collection1 = new[] { "abc", "xyz", "xxx" };
            var collection2 = new List<int> { 123, 456, 789 };


            Func<int, bool> numberCondition = (int item) => { return item > 400; };
            Func<string, bool> textCondition = (string item) => { return !string.IsNullOrEmpty(item) && item.Length >= 3; };

            //call as regular method
            var r1 = MyEnumerableExtensions.MyWhere1(collection1, textCondition);
            var r2 = MyEnumerableExtensions.MyWhere1(collection2, numberCondition);

            //call as extension method
            var r3 = collection1.MyWhere1(textCondition);
            var r4 = collection2.MyWhere1(numberCondition);

            //LINQ Where method
            var r5 = collection1.Where(textCondition);
            var r6 = collection2.Where(numberCondition);
        }

        private static void MethodAcceptingDelegateAsParameter(MyDelegate d)
        {
            if (d == null)
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                d(i * 10, (i + 100).ToString());
            }
        }

        private static void MyMethod(int integerParameter, string stringParameter)
        {
            Console.WriteLine("I'm MyMethod. Parameters: {0}, {1}", integerParameter, stringParameter);
        }

        //field
        private static MyDelegate _myField;

        //event
        public static event MyDelegate MyEvent;

        //event with accessors
        public static event MyDelegate MyEventWithAccessors
        {
            add
            {
                MyEvent += value;
            }
            remove
            {
                MyEvent -= value;
            }
        }

        public static MyDelegate MyEventAsProperty
        {
            get
            {
                return _myField;
            }
            set
            {
                _myField = value;
            }
        }
    }

    static class MyEnumerableExtensions
    {
        public static IEnumerable<T> MyWhere1<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            if (collection == null || condition == null)
            {
                return null;
            }

            var result = new List<T>();

            foreach (var item in collection)
            {
                if (condition(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static IEnumerable<T> MyWhere2<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            if (collection == null || condition == null)
            {
                yield break;
            }

            foreach (var item in collection)
            {
                if (condition(item))
                {
                    yield return item;
                }
            }
        }
    }

}
