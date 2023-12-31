namespace Voting_App.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() 
        { 
        
        }

        public NotFoundException(string m) : base (m)
        {

        }

        public NotFoundException(string m, Exception i) : base (m, i)
        {

        }

    }
}
