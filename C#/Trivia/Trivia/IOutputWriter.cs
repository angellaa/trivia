using System;
using System.Text;

namespace Trivia
{
    public interface IOutputWriter
    {
        void WriteLine(string message);
    }

    class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }

    class TestOutputWriter : IOutputWriter
    {
        private StringBuilder sb = new StringBuilder();

        public void WriteLine(string message)
        {
           sb.AppendLine(message);
        }

        public string Output
        {
            get
            {
                return sb.ToString();
            }
        }
    }
}
