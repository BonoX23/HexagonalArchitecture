namespace Domain.UtilsTools
{
    public static class Utils
    {
        //Modelo demostrativo de validação de email
        public static bool ValidateEmail(string email)
        {
            if (email == "b@b.com") return false;

            return true;
        }
    }
}