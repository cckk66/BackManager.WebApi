namespace BackManager.Utility.Tool
{
    public class Check
    {
       
        public static void Exception(bool isException, string message, params string[] args)
        {
            if (isException)
                throw new BmSqlException(string.Format(message, args));
        }
    }
}
